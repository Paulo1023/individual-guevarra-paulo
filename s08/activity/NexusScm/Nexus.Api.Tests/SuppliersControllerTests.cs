using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexus.API.Controllers;
using Nexus.API.Data;
using Nexus.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Api.Tests
{
    public class SuppliersControllerTests
    {
        private ApplicationDbContext GetDBContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase
                (databaseName: Guid.NewGuid().ToString()).Options;

            var dbContext = new ApplicationDbContext(options);

            dbContext.Database.EnsureCreated();

            return dbContext;
        }

        [Fact]
        public async Task GetSupplier_WithExistingId_ShouldReturnOkResultWithSupplier()
        {

            //ARRANGE - prepare all necessary data
            var dbContext = GetDBContext();

            //test data
            var testSupplier = new Supplier
            {
                Id = 1,
                Name = "Test Corporation",
                Email = "test@corporation.com"
            };

            dbContext.Suppliers.Add(testSupplier);

            await dbContext.SaveChangesAsync();

            var controller = new SuppliersController(dbContext);

            //ACT - execute methods to be tested
            var actionResult = await controller.GetSupplier(1);

            //ASSERT - check if the expected result matches the actual result
            var OkResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            var returnedSupplier = Assert.IsType<Supplier>(OkResult.Value);

            Assert.Equal(1, returnedSupplier.Id);
            Assert.Equal("Test Corporation", returnedSupplier.Name);
        }

        [Fact]
        public async Task GetSupplier_IdDoesNotExist_ShouldReturnNotFound()
        {
            var dbContext = GetDBContext();

            int nonExistingId = 99;

            var controller = new SuppliersController(dbContext);

            var actionResult = await controller.GetSupplier(nonExistingId);

            var NotFoundResult = Assert.IsType<NotFoundResult>(actionResult.Result);

        }

        [Theory]
        [InlineData(null, "test@supplier.com")]
        [InlineData("Test Supplier", null)]
        [InlineData("Test Supplier", "test@supplier.com")]
        public async Task PostSupplier_WithInvalidModel_ShouldReturnBadRequest(string name, string email)
        {
            var dbContext = GetDBContext();
            var controller = new SuppliersController(dbContext);
            var invalidSupplier = new Supplier
            {
                Name = name,
                Email = email
            };

            controller.ModelState.AddModelError("Error", "Model state is invalid for test");

            var actionResult = await controller.PostSupplier(invalidSupplier);

            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }
    }
}
