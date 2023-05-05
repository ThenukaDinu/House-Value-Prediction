using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Micro_House_Manage_API.Data;
using Models;
using Micro_House_Manage_API.Interfaces;
using Micro_House_Manage_API.Repository;
using AutoMapper;
using Micro_House_Manage_API.Dtos;
using System.Security.Claims;
using Micro_House_Manage_API.Helper;
using static Data.PublicEnum;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Micro_House_Manage_API.Services;
using Models.Requests;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace Micro_House_Manage_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ListingsController : ControllerBase
    {
        private readonly IListingRepository _listingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ListingsController> _logger;
        private readonly IMessageProducer _messageProducer;
        private readonly IUserAccess _userAccess;

        public ListingsController(IListingRepository listingRepository, IMapper mapper, ILogger<ListingsController> logger, IUserAccess userAccess, IMessageProducer messageProducer)
        {
            _listingRepository = listingRepository;
            _mapper = mapper;
            _logger = logger;
            _userAccess = userAccess;
            _messageProducer = messageProducer;
        }

        // GET: api/Listings
        [HttpGet]
        public async Task<ActionResult<ICollection<ListingDto>>> GetListings()
        {
            try
            {
                var entities = await _listingRepository.GetAllAsync();
                return Ok(_mapper.Map<List<ListingDto>>(entities));
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Listings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ListingDto>> GetListing(int id)
        {
            try
            {
                var entity = await _listingRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<ListingDto>(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred, {ex}", ex);
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Listings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutListing(int id, ListingDto listingDto)
        {
            if (id != listingDto.Id)
            {
                return BadRequest();
            }

            try
            {
                var listing = _mapper.Map<Listing>(listingDto);
                _listingRepository.Update(listing);
                await _listingRepository.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists = await _listingRepository.ExistsAsync(listing => listing.Id == id);
                if (!exists)
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred, {ex}", ex);
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Listings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ListingDto>> PostListing(ListingDto listingDto)
        {
            try
            {
                var listing = _mapper.Map<Listing>(listingDto);
                var user = User;
                var userId = user.FindFirstValue("sub");
                if (!Guid.TryParse(userId, out Guid userGuid))
                {
                    return Unauthorized();
                }
                listing.UserId = userGuid;
                listing.ListingStatus = ListingStatus.Available;

                await _listingRepository.AddAsync(listing);
                await _listingRepository.SaveChangesAsync();

                // Get the access token
                var accessToken = await _userAccess.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, "access_token", HttpContext);
                var userInfo = await _userAccess.GetUserProfile(accessToken);

                if (userInfo != null && userInfo.PreferredUsername != null)
                {
                    // initiate email for user

                    // Load email template and replace the values 
                    string rootPath = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
                    string htmlString = System.IO.File.ReadAllText(Path.Combine(rootPath, "Templates", "ListingTemplate.html"));
                    htmlString = htmlString.Replace("[username]", userInfo.GivenName);
                    htmlString = htmlString.Replace("[email]", userInfo.PreferredUsername);
                    htmlString = htmlString.Replace("[listingprice]", listing.ListingPrice.ToString());

                    var email = new EmailMessage()
                    {
                        Body = htmlString,
                        Subject = "New listing added",
                        To = userInfo.PreferredUsername,
                        Attachments = null
                    };

                    _messageProducer.SendingMessage(email, "emails", "emails");
                }

                var savedListing = _mapper.Map<ListingDto>(listing);
                return CreatedAtAction(nameof(GetListing), new { id = savedListing.Id }, savedListing);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred, {ex}", ex);
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Listings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteListing(int id)
        {
            try
            {
                var entity = await _listingRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return NotFound();
                }
                _listingRepository.Delete(entity);
                await _listingRepository.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred, {ex}", ex);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
