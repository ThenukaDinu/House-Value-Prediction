using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Micro_House_Manage_API.Controllers;
using Micro_House_Manage_API.Dtos;
using Micro_House_Manage_API.Interfaces;
using Micro_House_Manage_API.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Micro_House_Manage_API.Tests.Controllers
{
    public class ListingsControllerTests
    {
        private readonly IMapper _mapper;
        private readonly IListingRepository _listingRepository;
        private readonly ILogger<ListingsController> _logger;
        private readonly ListingsController _controller;
        private readonly IMessageProducer _messageProducer;
        private readonly IUserAccess _userAccess;

        public ListingsControllerTests()
        {
            _listingRepository = A.Fake<IListingRepository>();
            _logger = A.Fake<ILogger<ListingsController>>();
            _mapper = A.Fake<IMapper>();
            _messageProducer = A.Fake<IMessageProducer>();
            _userAccess = A.Fake<IUserAccess>();
            _controller = new ListingsController(_listingRepository, _mapper, _logger, _userAccess, _messageProducer);
        }

        [Fact]
        public async Task ListingsController_GetListings_ReturnOK()
        {
            // Arranges
            var listing = A.Fake<ICollection<ListingDto>>();
            var listingsList = A.Fake<List<ListingDto>>();
            A.CallTo(() => _mapper.Map<List<ListingDto>>(listing)).Returns(listingsList);

            // Act
            var result = await _controller.GetListings();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<ICollection<ListingDto>>>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(listingsList);
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task ListingsController_GetListing_ReturnOK()
        {
            // Arranges
            int id = 5;
            var listing = A.Fake<Listing>();
            var listingDto = A.Fake<ListingDto>();

            A.CallTo(() => _listingRepository.GetByIdAsync(id)).Returns(listing);
            A.CallTo(() => _mapper.Map<ListingDto>(listing)).Returns(listingDto);

            // Act
            var results = await _controller.GetListing(id);

            // Asserts
            results.Should().NotBeNull();
            var okResult = results.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(listingDto);
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task ListingsController_PutListing_WithValidData_ReturnsNoContent()
        {
            // Arranges
            int id = 6;
            var listing = A.Fake<Listing>();
            var listingDto = A.Fake<ListingDto>();
            listingDto.Id = id;
            A.CallTo(() => _mapper.Map<Listing>(listingDto)).Returns(listing);
            A.CallTo(() => _listingRepository.ExistsAsync(h => h.Id == id)).Returns(true);

            // Act
            var results = await _controller.PutListing(id, listingDto);

            // Asserts
            results.Should().NotBeNull();
            results.Should().BeOfType<NoContentResult>();
            A.CallTo(() => _listingRepository.Update(listing)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _listingRepository.SaveChangesAsync()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ListingsController_PutListing_WithInvalidData_ReturnsBadRequest()
        {
            // Arranges
            int id = 6;
            var listing = A.Fake<Listing>();
            var listingDto = A.Fake<ListingDto>();
            listingDto.Id = 7;

            // Act
            var results = await _controller.PutListing(id, listingDto);

            // Asserts
            results.Should().NotBeNull();
            results.Should().BeOfType<BadRequestResult>();
            A.CallTo(() => _listingRepository.Update(listing)).MustNotHaveHappened();
            A.CallTo(() => _listingRepository.SaveChangesAsync()).MustNotHaveHappened();
        }

        [Fact]
        public async Task ListingsController_PostListing_ReturnsCreatedActionResult()
        {
            // Arranges
            var accessToken = "fake_access_token";
            var listing = A.Fake<Listing>();
            var listingDto = A.Fake<ListingDto>();
            var userInfo = A.Fake<UserInfo>();
            var savedListingDto = A.Fake<ListingDto>();
            var email = A.Fake<EmailMessage>();
            var userId = Guid.NewGuid().ToString();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { 
                new Claim("sub", userId) },
                Guid.NewGuid().ToString()));

            var httpContext = new DefaultHttpContext { User = user };
            httpContext.Request.Headers["Authorization"] = $"Bearer {accessToken}";
            _controller.ControllerContext.HttpContext = httpContext;

            A.CallTo(() => _mapper.Map<Listing>(listingDto)).Returns(listing);
            A.CallTo(() => _listingRepository.AddAsync(listing));
            A.CallTo(() => _listingRepository.SaveChangesAsync());
            A.CallTo(() => _mapper.Map<ListingDto>(listing)).Returns(savedListingDto);
            A.CallTo(() => _userAccess.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, "access_token", httpContext)).Returns(accessToken);
            A.CallTo(() => _userAccess.GetUserProfile(accessToken)).Returns(userInfo);
            A.CallTo(() => _messageProducer.SendingMessage(email, "email", "email"));

            // Act
            var results = await _controller.PostListing(listingDto);

            // Asserts
            results.Should().NotBeNull();
            results.Should().BeOfType<ActionResult<ListingDto>>();
            A.CallTo(() => _listingRepository.AddAsync(listing)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _listingRepository.SaveChangesAsync()).MustHaveHappenedOnceExactly();
            var result = results.Result as CreatedAtActionResult;
            result.Value.Should().BeEquivalentTo(savedListingDto);
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
        }
    }
}
