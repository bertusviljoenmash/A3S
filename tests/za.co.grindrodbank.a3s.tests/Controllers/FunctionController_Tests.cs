/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using za.co.grindrodbank.a3s.Controllers;
using za.co.grindrodbank.a3s.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;
using za.co.grindrodbank.a3s.A3SApiResources;
using za.co.grindrodbank.a3s.Helpers;
using AutoMapper;
using za.co.grindrodbank.a3s.MappingProfiles;
using za.co.grindrodbank.a3s.Models;
using za.co.grindrodbank.a3s.Repositories;

namespace za.co.grindrodbank.a3s.tests.Controllers
{
    public class FunctionController_Tests
    {
        private readonly Function functionModel;
        private readonly FunctionSubmit functionSubmitModel;
        private readonly IFunctionService functionService;
        private readonly IOrderByHelper orderByHelper;
        private readonly IPaginationHelper paginationHelper;
        private readonly IMapper mapper;

        public FunctionController_Tests()
        {
            functionService = Substitute.For<IFunctionService>();
            orderByHelper = Substitute.For<IOrderByHelper>();
            paginationHelper = Substitute.For<IPaginationHelper>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new FunctionResourceFunctionModelProfile());
            });

            mapper = config.CreateMapper();

            functionModel = new Function()
            {
                Uuid = Guid.NewGuid(),
                Name = "Test Function Name",
                Description = "Test Function Description",
                Application = new ApplicationSimple { Name = "Test Application", Uuid = Guid.NewGuid()},
                Permissions = new List<Permission>()
                {
                    new Permission() { Uuid = Guid.NewGuid() },
                    new Permission() { Uuid = Guid.NewGuid() }
                }
            };

            functionSubmitModel = new FunctionSubmit()
            {
                Uuid = functionModel.Uuid,
                Name = functionModel.Name,
                Description = functionModel.Description,
                ApplicationId = functionModel.Application.Uuid,
                Permissions = new List<Guid>()
                {
                    functionModel.Permissions[0].Uuid,
                    functionModel.Permissions[1].Uuid
                }
            };
        }

        [Fact]
        public async Task GetFunctionAsync_WithEmptyGuid_ReturnsBadRequest()
        {
            // Arrange
            var controller = new FunctionController(functionService, orderByHelper, paginationHelper, mapper);

            // Act
            var result = await controller.GetFunctionAsync(Guid.Empty);

            // Assert
            var badRequestResult = result as BadRequestResult;
            Assert.NotNull(badRequestResult);
        }

        [Fact]
        public async Task GetFunctionAsync_WithRandomGuid_ReturnsNotFoundResult()
        {
            // Arrange
            var controller = new FunctionController(functionService, orderByHelper, paginationHelper, mapper);

            // Act
            var result = await controller.GetFunctionAsync(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetFunctionAsync_WithTestGuid_ReturnsCorrectResult()
        {
            // Arrange
            var functionService = Substitute.For<IFunctionService>();
            var testGuid = Guid.NewGuid();
            var testName = "TestUserName";

            functionService.GetByIdAsync(testGuid).Returns(new Function { Uuid = testGuid, Name = testName });

            var controller = new FunctionController(functionService, orderByHelper, paginationHelper, mapper);

            // Act
            IActionResult actionResult = await controller.GetFunctionAsync(testGuid);

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.NotNull(okResult);

            var function = okResult.Value as Function;
            Assert.NotNull(function);
            Assert.True(function.Uuid == testGuid, $"Retrieved Id {function.Uuid} not the same as sample id {testGuid}.");
            Assert.True(function.Name == testName, $"Retrieved Name {function.Name} not the same as sample id {testName}.");
        }

        [Fact]
        public async Task ListFunctionsAsync_WithNoInputs_ReturnsList()
        {
            // Arrange
            var functionService = Substitute.For<IFunctionService>();

            var inList = new List<FunctionModel>();
            inList.Add(new FunctionModel { Name = "Test Functions 1", Id = Guid.NewGuid() });
            inList.Add(new FunctionModel { Name = "Test Functions 2", Id = Guid.NewGuid() });
            inList.Add(new FunctionModel { Name = "Test Functions 3", Id = Guid.NewGuid() });

            PaginatedResult<FunctionModel> paginatedResult = new PaginatedResult<FunctionModel>
            {
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 3,
                Results = inList
            };

            functionService.GetPaginatedListAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<bool>(), Arg.Any<string>(), Arg.Any<List<KeyValuePair<string, string>>>()).Returns(paginatedResult);

            var controller = new FunctionController(functionService, orderByHelper, paginationHelper, mapper);

            // Act
            IActionResult actionResult = await controller.ListFunctionsAsync(1, 10, true, string.Empty, string.Empty);

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.NotNull(okResult);

            var outList = okResult.Value as List<Function>;
            Assert.NotNull(outList);

            for (var i = 0; i < outList.Count; i++)
            {
                Assert.Equal(outList[i].Uuid, inList[i].Id);
                Assert.Equal(outList[i].Name, inList[i].Name);
            }
        }

        [Fact]
        public async Task UpdateFunctionAsync_WithRandomGuid_ReturnsNotImplemented()
        {
            // Arrange
            var controller = new FunctionController(functionService, orderByHelper, paginationHelper, mapper);

            // Act
            IActionResult actionResult = await controller.UpdateFunctionAsync(Guid.NewGuid(), new FunctionSubmit());

            // Assert
            var badRequestResult = actionResult as BadRequestResult;
            Assert.NotNull(badRequestResult);
        }

        [Fact]
        public async Task UpdateFunctionAsync_WithBlankGuid_ReturnsNotImplemented()
        {
            // Arrange
            var controller = new FunctionController(functionService, orderByHelper, paginationHelper, mapper);

            functionService.UpdateAsync(functionSubmitModel, Arg.Any<Guid>(), Arg.Any<Guid>())
                .Returns(new FunctionTransient
                {
                    Name = functionSubmitModel.Name,
                    FunctionId = functionSubmitModel.Uuid,
                    Description = functionSubmitModel.Description,
                    ApplicationId = functionSubmitModel.ApplicationId
                });

            // Act
            IActionResult actionResult = await controller.UpdateFunctionAsync(Guid.Empty, functionSubmitModel);

            // Assert
            var badRequestResult = actionResult as BadRequestResult;
            Assert.NotNull(badRequestResult);
        }

        [Fact]
        public async Task UpdateFunctionAsync_WithBlankGuidInBody_ReturnsNotImplemented()
        {
            // Arrange
            var controller = new FunctionController(functionService, orderByHelper, paginationHelper, mapper);

            functionService.UpdateAsync(functionSubmitModel, Arg.Any<Guid>(), Arg.Any<Guid>())
                .Returns(new FunctionTransient
                {
                    Name = functionSubmitModel.Name,
                    FunctionId = functionSubmitModel.Uuid,
                    Description = functionSubmitModel.Description,
                    ApplicationId = functionSubmitModel.ApplicationId
                });

            functionSubmitModel.Uuid = Guid.Empty;

            // Act
            IActionResult actionResult = await controller.UpdateFunctionAsync(Guid.NewGuid(), functionSubmitModel);

            // Assert
            var badRequestResult = actionResult as BadRequestResult;
            Assert.NotNull(badRequestResult);
        }

        [Fact]
        public async Task UpdateFunctionAsync_WithTestFunction_ReturnsFunctionModel()
        {
            // Arrange
            var controller = new FunctionController(functionService, orderByHelper, paginationHelper, mapper);

            functionService.UpdateAsync(functionSubmitModel, Arg.Any<Guid>(), Arg.Any<Guid>())
                .Returns(new FunctionTransient {
                    Name = functionSubmitModel.Name,
                    FunctionId = functionSubmitModel.Uuid,
                    Description = functionSubmitModel.Description,
                    ApplicationId = functionSubmitModel.ApplicationId
                });

            // Act
            IActionResult actionResult = await controller.UpdateFunctionAsync(functionSubmitModel.Uuid, functionSubmitModel);

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.NotNull(okResult);

            var function = okResult.Value as FunctionTransient;
            Assert.NotNull(function);
            Assert.True(function.FunctionId == functionSubmitModel.Uuid, $"Retrieved Id {function.Uuid} not the same as sample id {functionSubmitModel.Uuid}.");
            Assert.True(function.Name == functionSubmitModel.Name, $"Retrieved Name {function.Name} not the same as sample Name {functionSubmitModel.Name}.");
            Assert.True(function.Description == functionSubmitModel.Description, $"Retrieved Description {function.Description} not the same as sample Description {functionSubmitModel.Description}.");
            Assert.True(function.ApplicationId == functionSubmitModel.ApplicationId, $"Retrieved ApplicationId {function.ApplicationId} not the same as sample ApplicationId {functionSubmitModel.ApplicationId}.");

        }
    }
}
