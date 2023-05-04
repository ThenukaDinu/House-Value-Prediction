using Micro_Identity_API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Requests;

namespace Micro_Identity_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterestsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public InterestsController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserInterests(Guid userId)
        {
            try
            {
                var interests = new List<Interest>();
                if (_context.Interests != null)
                {
                    interests = await _context.Interests.Where(x => x.UserId == userId).ToListAsync();
                } else
                {
                    return NotFound();
                }

                return Ok(interests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostInterest([FromBody]List<InterestRequest> interestRequests)
        {
            try
            {
                List<Interest> interests = new();
                interestRequests.ForEach(req =>
                {
                    interests.Add(new Interest
                    {
                        Description = req.Description,
                        Type = req.Type,
                    });
                });

                if (_context.Interests == null)
                    return BadRequest();

                await _context.Interests.AddRangeAsync(interests);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUserInterests), new { id = interests[0].Id }, interests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInterest(int id)
        {
            try
            {
                if (_context.Interests == null)
                    return BadRequest();

                var entity = await _context.Interests.FindAsync(id);
                if (entity == null)
                {
                    return NotFound();
                }
                _context.Interests.Remove(entity);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutListing(int id, InterestRequest interestRequest)
        {
            if (id != interestRequest.Id)
            {
                return BadRequest();
            }

            try
            {
                var listing = new Interest()
                {
                    Description = interestRequest.Description,
                    Type = interestRequest.Type,
                };

                if (_context.Interests == null)
                    return BadRequest();

                _context.Interests.Update(listing);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists = await _context.Interests.AnyAsync(x => x.Id == id);
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
    }
}
