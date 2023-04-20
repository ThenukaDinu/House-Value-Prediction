using FakeItEasy;
using FluentAssertions;
using Micro_House_Manage_API.Data;
using Micro_House_Manage_API.Interfaces;
using Micro_House_Manage_API.Repository;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.PublicEnum;

namespace Micro_House_Manage_API.Tests.Repository
{
    public class BaseRepositoryTests
    {
        public BaseRepositoryTests() 
        { 
            
        }

        private async Task<DataContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new DataContext(options);
            databaseContext.Database.EnsureCreated();

            if (await databaseContext.Inquiries.CountAsync() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    await databaseContext.Inquiries.AddAsync(new Inquiry { Id=i+1, CreatedDate=DateTime.Now, UserId=Guid.NewGuid(), HouseId = i+10000+1, Message=$"Sample message {i+1}", ResponseMessage=$"Sample response message {i+1}", InquiryStatus = InquiryStatus.Responded });
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }

        [Fact]
        public async Task BaseRepository_GetAllAsync_ReturnAllEntities()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            IBaseRepository<Inquiry> baseRepository = new BaseRepository<Inquiry>(dbContext);

            // Act
            var results = baseRepository.GetAllAsync();

            // Assert
            results.Should().NotBeNull();
            results.Should().BeOfType<Task<ICollection<Inquiry>>>();
            results.Result.Count.Should().NotBe(0);
        }

        [Fact]
        public async Task BaseRepository_GetByIdAsync_ReturnEntity()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            IBaseRepository<Inquiry> baseRepository = new BaseRepository<Inquiry>(dbContext);
            int id = 1;

            // Act
            var results = await baseRepository.GetByIdAsync(id);
            
            // Assert
            results.Should().NotBeNull();
            results.Id.Should().Be(id);
            results.Message.Should().Be($"Sample message {id}");
        }

        [Fact]
        public async Task BaseRepository_AddAsync_ShouldAddEntityToDatabase()
        {
            // Arrange
            int id = 11;
            var dbContext = await GetDatabaseContext();
            var oldRecord = await dbContext.Inquiries.FindAsync(id);
            if (oldRecord != null)
            {
                dbContext.Inquiries.Remove(oldRecord);
                await dbContext.SaveChangesAsync();
            }

            IBaseRepository<Inquiry> baseRepository = new BaseRepository<Inquiry>(dbContext);
            var inquiry = new Inquiry() { Id=id, CreatedDate=DateTime.Now, Message="Newly added inquiry", HouseId=-999, ResponseMessage="Newly added response message", InquiryStatus=InquiryStatus.Responded, UserId=Guid.NewGuid() };

            // Act
            await baseRepository.AddAsync(inquiry);
            await baseRepository.SaveChangesAsync();

            // Assert
            var results = await dbContext.Inquiries.FindAsync(inquiry.Id);
            results.Should().NotBeNull();
            results?.Id.Should().NotBe(0);
        }

        [Fact]
        public async Task BaseRepository_Update_ShouldUpdateEntityInDatabase()
       {
            // Arrange
            int id = 11;
            var dbContext = await GetDatabaseContext();
            var existingRecord = await dbContext.Inquiries.FindAsync(id);
            IBaseRepository<Inquiry> baseRepository = new BaseRepository<Inquiry>(dbContext);
           
            if (existingRecord == null)
            {
                existingRecord = new Inquiry() { Id = id, CreatedDate = DateTime.Now, Message = "Newly added inquiry", HouseId = -999, ResponseMessage = "Newly added response message", InquiryStatus = InquiryStatus.Responded, UserId = Guid.NewGuid() };
                await dbContext.Inquiries.AddAsync(existingRecord);
                await dbContext.SaveChangesAsync();
            }

            existingRecord.Message = "Updated message";
            existingRecord.ResponseMessage = "Updated response message";

            // Act
            baseRepository.Update(existingRecord);
            await baseRepository.SaveChangesAsync();

            // Assert
            var results = await dbContext.Inquiries.FirstOrDefaultAsync(i => i.Message == "Updated message");
            results.Should().NotBeNull();
            results?.Id.Should().NotBe(0);
        }

        [Fact]
        public async Task BaseRepository_Delete_ShouldRemoveEntityFromDatabase()
        {
            // Arrange
            int id = 11;
            var dbContext = await GetDatabaseContext();
            IBaseRepository<Inquiry> baseRepository = new BaseRepository<Inquiry>(dbContext);
            var existingRecord = await dbContext.Inquiries.FindAsync(id);

            if (existingRecord == null)
            {
                existingRecord = new Inquiry() { Id = id, CreatedDate = DateTime.Now, Message = "Newly added inquiry", HouseId = -999, ResponseMessage = "Newly added response message", InquiryStatus = InquiryStatus.Responded, UserId = Guid.NewGuid() };
                await dbContext.Inquiries.AddAsync(existingRecord);
                await dbContext.SaveChangesAsync();
            }

            // Act
            baseRepository.Delete(existingRecord);
            await baseRepository.SaveChangesAsync();

            // Assert
            var results = await dbContext.Inquiries.FindAsync(11);
            results.Should().BeNull();
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnTrueIfEntityExists()
        {
            // Arrange
            int id = 11;
            var dbContext = await GetDatabaseContext();
            IBaseRepository<Inquiry> baseRepository = new BaseRepository<Inquiry>(dbContext);
            var existingRecord = await dbContext.Inquiries.FindAsync(id);

            if (existingRecord == null)
            {
                existingRecord = new Inquiry() { Id = id, CreatedDate = DateTime.Now, Message = "Newly added inquiry", HouseId = -999, ResponseMessage = "Newly added response message", InquiryStatus = InquiryStatus.Responded, UserId = Guid.NewGuid() };
                await dbContext.Inquiries.AddAsync(existingRecord);
                await dbContext.SaveChangesAsync();
            }

            // Act
            var results = await baseRepository.ExistsAsync(e => e.Id == id);

            // Assert
            results.Should().BeTrue();
        }
    }
}
