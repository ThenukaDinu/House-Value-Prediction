using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Micro_House_Manage_API.Data;
using Micro_House_Manage_API.Models;
using Micro_House_Manage_API.Interfaces;
using NuGet.Protocol.Core.Types;

namespace Micro_House_Manage_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HousesController : ControllerBase
    {
        private readonly IHouseRepository _houseRepository;

        public HousesController(IHouseRepository houseRepository)
        {
            _houseRepository = houseRepository;
        }

        // GET: api/Houses
        [HttpGet]
        public async Task<ActionResult<ICollection<House>>> GetHouses()
        {
            try
            {
                var entities = await _houseRepository.GetAllAsync();
                return Ok(entities);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Houses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<House>> GetHouse(int id)
        {
            try
            {
                var entity = await _houseRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return NotFound();
                }
                return Ok(entity);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Houses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHouse(int id, House house)
        {
            if (id != house.Id)
            {
                return BadRequest();
            }

            try
            {
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
        public async Task<ActionResult<House>> PostHouse(House house)
        {
            try
            {
                await _houseRepository.AddAsync(house);
                await _houseRepository.SaveChangesAsync();
                return CreatedAtAction(nameof(GetHouse), new { id = house.Id }, house);
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
    }
}
