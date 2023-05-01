using AutoMapper;
using Micro_House_Manage_API.Dtos;
using Micro_House_Manage_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Security.Claims;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Micro_House_Manage_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IHttpClientService _httpClientService;
        private readonly IUserAccess _userAccess;
        private readonly IConfigurationService _configurationService;
        private readonly IMapper _mapper;

        public UsersController(ILogger<UsersController> logger, IMapper mapper, IHttpClientService httpClientService, IUserAccess userAccess, IConfigurationService configurationService)
        {
            _logger = logger;
            _mapper = mapper;
            _httpClientService = httpClientService;
            _userAccess = userAccess;
            _configurationService = configurationService;
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> GetUserProfile()
        {
            try
            {
                var user = User;
                var userId = user.FindFirstValue("sub");
                if (!Guid.TryParse(userId, out Guid userGuid))
                {
                    return Unauthorized();
                }

                // Get the access token
                var accessToken = await _userAccess.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, "access_token", HttpContext);
                var userInfo = await _userAccess.GetUserProfile(accessToken);

                string idpServiceBaseUrl = _configurationService.GetSingleValue<string>("IdentityServerSettings:Authority");

                // Get user interests
                HttpResponseMessage response = await _httpClientService.GetAsync($"{idpServiceBaseUrl}api/Interests?userId={userId}");
                string responseString = await _httpClientService.ReadAsStringAsync(response);
                List<Interest> interests = JsonSerializer.Deserialize<List<Interest>>(responseString);
                List<InterestDto> interestDtos = _mapper.Map<List<InterestDto>>(interests);

                // Prepaire User Dto to send as a final response
                UserDto userDto = _mapper.Map<UserDto>(userInfo);
                userDto.UserInterests = interestDtos;

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error coccurred. {ex}", ex);
                throw;
            }
        }
    }
}
