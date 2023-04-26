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
using NuGet.Protocol.Core.Types;
using AutoMapper;
using Micro_House_Manage_API.Dtos;
using Models.Requests;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Micro_House_Manage_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HousesController : ControllerBase
    {
        private readonly IHouseRepository _houseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<HousesController> _logger;

        public HousesController(IHouseRepository houseRepository, IMapper mapper, ILogger<HousesController> logger)
        {
            _houseRepository = houseRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Houses
        [HttpGet]
        public async Task<ActionResult<ICollection<HouseDto>>> GetHouses()
        {
            try
            {
                _logger.LogInformation("HousesController GetHouses executing...");

                var entities = await _houseRepository.GetAllAsync();
                return Ok(_mapper.Map<List<HouseDto>>(entities));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Houses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HouseDto>> GetHouse(int id)
        {
            try
            {
                var entity = await _houseRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<HouseDto>(entity));
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Houses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHouse(int id, HouseDto houseDto)
        {
            if (id != houseDto.Id)
            {
                return BadRequest();
            }

            try
            {
                var house = _mapper.Map<House>(houseDto);
                _houseRepository.Update(house);
                await _houseRepository.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists = await _houseRepository.ExistsAsync(house => house.Id == id);
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
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Houses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HouseDto>> PostHouse(HouseDto houseDto)
        {
            try
            {
                var house = _mapper.Map<House>(houseDto);
                await _houseRepository.AddAsync(house);
                await _houseRepository.SaveChangesAsync();

                var savedHouseDto = _mapper.Map<HouseDto>(house);
                return CreatedAtAction(nameof(GetHouse), new { id = savedHouseDto.Id }, savedHouseDto);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Houses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHouse(int id)
        {
            try
            {
                var entity = await _houseRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return NotFound();
                }
                _houseRepository.Delete(entity);
                await _houseRepository.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("GetValuePrediction")]
        [Authorize(Roles = "UserRole")]
        public async Task<IActionResult> GetValuePrediction(List<PredictionRequest> predictionRequests)
        {
            try
            {
                var user = User;
                using var client = new HttpClient();

                // Create request content
                var json = JsonSerializer.Serialize(predictionRequests.ToArray());
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://localhost:44343/predict", content);

                // Read the response as a string
                var responseString = await response.Content.ReadAsStringAsync();

                return Ok(responseString);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
