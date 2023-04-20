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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Micro_House_Manage_API.Tests.Controllers
{
    public class InquiriesControllerTests
    {
        private readonly IMapper _mapper;
        private readonly IInquiryRepository _inquiryRepository;
        private readonly ILogger<InquiriesController> _logger;
        private readonly InquiriesController _controller;

        public InquiriesControllerTests()
        {
            _inquiryRepository = A.Fake<IInquiryRepository>();
            _logger = A.Fake<ILogger<InquiriesController>>();
            _mapper = A.Fake<IMapper>();
            _controller = new InquiriesController(_inquiryRepository, _mapper, _logger);
        }

        [Fact]
        public async Task InquiriesController_GetInquiries_ReturnOK()
        {
            // Arranges
            var inquiry = A.Fake<ICollection<InquiryDto>>();
            var inqueriesList = A.Fake<List<InquiryDto>>();
            A.CallTo(() => _mapper.Map<List<InquiryDto>>(inquiry)).Returns(inqueriesList);

            // Act
            var result = await _controller.GetInquiries();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<ICollection<InquiryDto>>>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(inqueriesList);
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task InquiriesController_GetInquiry_ReturnOK()
        {
            // Arranges
            int id = 5;
            var inquiry = A.Fake<Inquiry>();
            var inquiryDto = A.Fake<InquiryDto>();

            A.CallTo(() => _inquiryRepository.GetByIdAsync(id)).Returns(inquiry);
            A.CallTo(() => _mapper.Map<InquiryDto>(inquiry)).Returns(inquiryDto);

            // Act
            var results = await _controller.GetInquiry(id);

            // Asserts
            results.Should().NotBeNull();
            var okResult = results.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(inquiryDto);
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task InquiriesController_PutInquiry_WithValidData_ReturnsNoContent()
        {
            // Arranges
            int id = 6;
            var inquiry = A.Fake<Inquiry>();
            var inquiryDto = A.Fake<InquiryDto>();
            inquiryDto.Id = id;
            A.CallTo(() => _mapper.Map<Inquiry>(inquiryDto)).Returns(inquiry);
            A.CallTo(() => _inquiryRepository.ExistsAsync(h => h.Id == id)).Returns(true);

            // Act
            var results = await _controller.PutInquiry(id, inquiryDto);

            // Asserts
            results.Should().NotBeNull();
            results.Should().BeOfType<NoContentResult>();
            A.CallTo(() => _inquiryRepository.Update(inquiry)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _inquiryRepository.SaveChangesAsync()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task InquiriesController_PutInquiry_WithInvalidData_ReturnsBadRequest()
        {
            // Arranges
            int id = 6;
            var inquiry = A.Fake<Inquiry>();
            var inquiryDto = A.Fake<InquiryDto>();
            inquiryDto.Id = 7;

            // Act
            var results = await _controller.PutInquiry(id, inquiryDto);

            // Asserts
            results.Should().NotBeNull();
            results.Should().BeOfType<BadRequestResult>();
            A.CallTo(() => _inquiryRepository.Update(inquiry)).MustNotHaveHappened();
            A.CallTo(() => _inquiryRepository.SaveChangesAsync()).MustNotHaveHappened();
        }

        [Fact]
        public async Task InquiriesController_PostInquiry_ReturnsCreatedActionResult()
        {
            // Arranges
            var inquiry = A.Fake<Inquiry>();
            var inquiryDto = A.Fake<InquiryDto>();
            A.CallTo(() => _mapper.Map<Inquiry>(inquiryDto)).Returns(inquiry);
            var savedInquiryDto = A.Fake<InquiryDto>();
            A.CallTo(() => _inquiryRepository.AddAsync(inquiry));
            A.CallTo(() => _inquiryRepository.SaveChangesAsync());
            A.CallTo(() => _mapper.Map<InquiryDto>(inquiry)).Returns(savedInquiryDto);

            // Act
            var results = await _controller.PostInquiry(inquiryDto);

            // Asserts
            results.Should().NotBeNull();
            results.Should().BeOfType<ActionResult<InquiryDto>>();
            A.CallTo(() => _inquiryRepository.AddAsync(inquiry)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _inquiryRepository.SaveChangesAsync()).MustHaveHappenedOnceExactly();
            var result = results.Result as CreatedAtActionResult;
            result.Value.Should().BeEquivalentTo(savedInquiryDto);
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
        }
    }
}
