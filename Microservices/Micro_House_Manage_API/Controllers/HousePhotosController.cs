using AutoMapper;
using Micro_House_Manage_API.Interfaces;
using Micro_House_Manage_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;

namespace Micro_House_Manage_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HousePhotosController : ControllerBase
    {
        private readonly ILogger<HousePhotosController> _logger;
        private readonly IMapper _mapper;
        private readonly IHousePhotoRepository _housePhotoRepository;
        private readonly IConfigurationService _configurationService;

        public HousePhotosController(ILogger<HousePhotosController> logger, IMapper mapper, IHousePhotoRepository housePhotoRepository, IConfigurationService configurationService)
        {
            _housePhotoRepository = housePhotoRepository;
            _logger = logger;
            _mapper = mapper;
            _configurationService = configurationService;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadPhoto()
        {
            try
            {
                var file = Request.Form.Files[0];
                var fileName = Path.GetFileName(file.FileName);
                var fileExtension = Path.GetExtension(file.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var imagePath = _configurationService.GetSingleValue<string>("ImageServer:ImagesPath");
                var imageServerURL = _configurationService.GetSingleValue<string>("ImageServer:BaseURL");
                var filePath = Path.Combine(imagePath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok(new { name = uniqueFileName, originalName = fileName, fullLink = imageServerURL + uniqueFileName });
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured, {ex}", ex);
                return BadRequest(ex);
            }
        }
    }
}
