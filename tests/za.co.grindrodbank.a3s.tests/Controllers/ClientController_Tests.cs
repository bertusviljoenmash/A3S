/**
 * *************************************************
 * Copyright (c) 2019, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;
using za.co.grindrodbank.a3s.A3SApiResources;
using za.co.grindrodbank.a3s.Controllers;
using za.co.grindrodbank.a3s.Services;

namespace za.co.grindrodbank.a3s.tests.Controllers
{
    public class ClientController_Tests
    {
        [Fact]
        public async Task ListClientsAsync_WithNoInputs_ReturnsList()
        {
            // Arrange
            var clientService = Substitute.For<IClientService>();

            var inList = new List<Oauth2Client>();
            inList.Add(new Oauth2Client { Name = "Test Client 1", ClientId = "test-client-1", AllowedOfflineAccess = true });
            inList.Add(new Oauth2Client { Name = "Test Client 2", ClientId = "test-client-2", AllowedOfflineAccess = false });
            inList.Add(new Oauth2Client { Name = "Test Client 3", ClientId = "test-client-3", AllowedOfflineAccess = true });

            clientService.GetListAsync().Returns(inList);

            var controller = new ClientController(clientService);

            // Act
            IActionResult actionResult = await controller.ListClientsAsync(0, 50, string.Empty, null);

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.NotNull(okResult);

            var outList = okResult.Value as List<Oauth2Client>;
            Assert.NotNull(outList);

            for (var i = 0; i < outList.Count; i++)
            {
                Assert.Equal(outList[i].ClientId, inList[i].ClientId);
                Assert.Equal(outList[i].Name, inList[i].Name);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("     ")]
        public async Task GetClientAsync_WithVariousEmptyStrings_ReturnsBadRequest(string id)
        {
            // Arrange
            var clientService = Substitute.For<IClientService>();
            var controller = new ClientController(clientService);

            // Act
            var result = await controller.GetClientAsync(id);

            // Assert
            var badRequestResult = result as BadRequestResult;
            Assert.NotNull(badRequestResult);
        }

        [Fact]
        public async Task GetClientAsync_WithUnfindableId_ReturnsNotFoundRequest()
        {
            // Arrange
            var clientService = Substitute.For<IClientService>();
            var controller = new ClientController(clientService);

            // Act
            var result = await controller.GetClientAsync("unfindable-id");

            // Assert
            var requestResult = result as NotFoundResult;
            Assert.NotNull(requestResult);
        }

        [Fact]
        public async Task GetClientAsync_WithTestString_ReturnsCorrectResult()
        {
            // Arrange
            var clientService = Substitute.For<IClientService>();
            var testId = "test-id";
            var testName = "TestUserName";

            clientService.GetByClientIdAsync(testId).Returns(new Oauth2Client { ClientId = testId, Name = testName });

            var controller = new ClientController(clientService);

            // Act
            IActionResult actionResult = await controller.GetClientAsync(testId);

            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.NotNull(okResult);

            var resource = okResult.Value as Oauth2Client;
            Assert.NotNull(resource);
            Assert.True(resource.ClientId == testId, $"Retrieved Id {resource.ClientId} not the same as sample id {testId}.");
            Assert.True(resource.Name == testName, $"Retrieved Name {resource.Name} not the same as sample id {testName}.");
        }
    }
}
