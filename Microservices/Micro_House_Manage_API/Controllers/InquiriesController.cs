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

namespace Micro_House_Manage_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InquiriesController : ControllerBase
    {
        private readonly IInquiryRepository _inquiryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<InquiriesController> _logger;

        public InquiriesController(IInquiryRepository inquiryRepository, IMapper mapper, ILogger<InquiriesController> logger)
        {
            _inquiryRepository = inquiryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Inquiries
        [HttpGet]
        public async Task<ActionResult<ICollection<InquiryDto>>> GetInquiries()
        {
            try
            {
                var entities = await _inquiryRepository.GetAllAsync();
                return Ok(_mapper.Map<List<InquiryDto>>(entities));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred, {ex}", ex);
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Inquiries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InquiryDto>> GetInquiry(int id)
        {
            try
            {
                var entity = await _inquiryRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<InquiryDto>(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred, {ex}", ex);
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Inquiries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInquiry(int id, InquiryDto inquiryDto)
        {
            if (id != inquiryDto.Id)
            {
                return BadRequest();
            }

            try
            {
                var inquiry = _mapper.Map<Inquiry>(inquiryDto);
                _inquiryRepository.Update(inquiry);
                await _inquiryRepository.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists = await _inquiryRepository.ExistsAsync(inquiry => inquiry.Id == id);
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

        // POST: api/Inquiries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InquiryDto>> PostInquiry(InquiryDto inquiryDto)
        {
            try
            {
                var inquiry = _mapper.Map<Inquiry>(inquiryDto);
                await _inquiryRepository.AddAsync(inquiry);
                await _inquiryRepository.SaveChangesAsync();

                var savedInquiry = _mapper.Map<InquiryDto>(inquiry);
                return CreatedAtAction(nameof(GetInquiry), new { id = savedInquiry.Id }, savedInquiry);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred, {ex}", ex);
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Inquiries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInquiry(int id)
        {
            try
            {
                var entity = await _inquiryRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return NotFound();
                }
                _inquiryRepository.Delete(entity);
                await _inquiryRepository.SaveChangesAsync();
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
