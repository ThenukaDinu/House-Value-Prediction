using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Micro_House_Manage_API.Controllers;
using Micro_House_Manage_API.Dtos;
using Micro_House_Manage_API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.Others;
using Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Micro_House_Manage_API.Tests.Controllers
{
    public class HousesControllerTests
    {
        private readonly IMapper _mapper;
        private readonly IHouseRepository _houseRepository;
        private readonly ILogger<HousesController> _logger;
        private readonly HousesController _controller;

        public HousesControllerTests()
        {
            _houseRepository = A.Fake<IHouseRepository>();
            _logger = A.Fake<ILogger<HousesController>>();
            _mapper = A.Fake<IMapper>();
            _controller = new HousesController(_houseRepository, _mapper, _logger);
        }

        [Fact]
        public async Task HousesController_GetHouses_ReturnOK()
        {
            // Arranges
            var houses = A.Fake<ICollection<HouseDto>>();
            var housesList = A.Fake<List<HouseDto>>();
            A.CallTo(() => _mapper.Map<List<HouseDto>>(houses)).Returns(housesList);

            // Act
            var result = await _controller.GetHouses();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<ICollection<HouseDto>>>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(housesList);
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task HouseController_GetHouse_ReturnOK()
        {
            // Arranges
            int id = 5;
            var house = A.Fake<House>();
            var houseDto = A.Fake<HouseDto>();

            A.CallTo(() => _houseRepository.GetByIdAsync(id)).Returns(house);
            A.CallTo(() => _mapper.Map<HouseDto>(house)).Returns(houseDto);

            // Act
            var results = await _controller.GetHouse(id);

            // Asserts
            results.Should().NotBeNull();
            var okResult = results.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(houseDto);
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task HouseController_PutHouse_WithValidData_ReturnsNoContent()
        {
            // Arranges
            int id = 6;
            var house = A.Fake<House>();
            var houseDto = A.Fake<HouseDto>();
            houseDto.Id = id;
            A.CallTo(() => _mapper.Map<House>(houseDto)).Returns(house);
            A.CallTo(() => _houseRepository.ExistsAsync(h => h.Id == id)).Returns(true);

            // Act
            var results = await _controller.PutHouse(id, houseDto);

            // Asserts
            results.Should().NotBeNull();
            results.Should().BeOfType<NoContentResult>();
            A.CallTo(() => _houseRepository.Update(house)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _houseRepository.SaveChangesAsync()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HouseController_PutHouse_WithInvalidData_ReturnsBadRequest()
        {
            // Arranges
            int id = 6;
            var house = A.Fake<House>();
            var houseDto = A.Fake<HouseDto>();
            houseDto.Id = 7;

            // Act
            var results = await _controller.PutHouse(id, houseDto);

            // Asserts
            results.Should().NotBeNull();
            results.Should().BeOfType<BadRequestResult>();
            A.CallTo(() => _houseRepository.Update(house)).MustNotHaveHappened();
            A.CallTo(() => _houseRepository.SaveChangesAsync()).MustNotHaveHappened();
        }

        [Fact]
        public async Task HouseController_PostHouse_ReturnsCreatedActionResult()
        {
            // Arranges
            var house = A.Fake<House>();
            var houseDto = A.Fake<HouseDto>();
            var savedHouseDto = A.Fake<HouseDto>();
            var userId = Guid.NewGuid().ToString();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim("sub", userId) },
                Guid.NewGuid().ToString()));
            var httpContext = new DefaultHttpContext { User = user };
            _controller.ControllerContext.HttpContext = httpContext;

            A.CallTo(() => _mapper.Map<House>(houseDto)).Returns(house);
            A.CallTo(() => _houseRepository.AddAsync(house));
            A.CallTo(() => _houseRepository.SaveChangesAsync());
            A.CallTo(() => _mapper.Map<HouseDto>(house)).Returns(savedHouseDto);

            // Act
            var results = await _controller.PostHouse(houseDto);

            // Asserts
            results.Should().NotBeNull();
            results.Should().BeOfType<ActionResult<HouseDto>>();
            A.CallTo(() => _houseRepository.AddAsync(house)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _houseRepository.SaveChangesAsync()).MustHaveHappenedOnceExactly();
            var result = results.Result as CreatedAtActionResult;
            result.Value.Should().BeEquivalentTo(savedHouseDto);
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
        }

        [Fact]
        public async Task HouseController_GetValuePrediction_ReturnsOk()
        {
            // Arrange
            var predictionRequests = A.Fake<List<PredictionRequest>>();
            var json = JsonSerializer.Serialize(predictionRequests.ToArray());
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var responseString = "test response";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseString, Encoding.UTF8, "application/json")
            };

            var httpClient = A.Fake<HttpClient>();
            A.CallTo(() => httpClient.PostAsync(A<string>.Ignored, content)).Returns(Task.FromResult(response));

            // Act
            var result = await _controller.GetValuePrediction(predictionRequests);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okObjectResult = result as OkObjectResult;
            okObjectResult.Value.Should().Be(responseString);
        }
    }
}
