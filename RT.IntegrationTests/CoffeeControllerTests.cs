using System.Net.Http.Json;
using System.Net;
using RT.CoffeeMachine.Models;
using RT.CoffeeMachine.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;

namespace RT.IntegrationTests;

public class CoffeeControllerTests
{
    private const string Location = "Australia/Brisbane";
    private const string Endpoint = "/brew-coffee";
    private readonly bool _isAprilFirst = false;

    public CoffeeControllerTests()
    {
        var now = DateTime.UtcNow.ToLocalDate(Location);
        if (now.HasValue)
            _isAprilFirst = now.Value.Month == 4 && now.Value.Day == 1;
    }

    [Fact]
    public async Task CoffeeController_WhenCalledWithCorrectLocation_ReturnsCoffeeMessage()
    {
        // Arrange
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Location", Location);
        
        // Act
        var response = await client.GetAsync(Endpoint);

        // Assert
        if (_isAprilFirst)
        {
            var teaInsteadOfCoffee = (int)response.StatusCode;
            teaInsteadOfCoffee.Should().Be(418);
        }
        else
        {
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadFromJsonAsync<CoffeeResponse>();
            content.As<CoffeeResponse>().Message.Should()
                .Be("Your piping hot coffee is ready");
        }
    }

    [Fact]
    public async Task CoffeeController_WhenCalledWithBadLocation_ReturnsBadRequestError()
    {
        // Arrange
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Location", "SomeLocation");

        // Act
        var response = await client.GetAsync(Endpoint);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CoffeeController_WhenCalledWithNoLocation_ReturnsBadRequestError()
    {
        // Arrange
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync(Endpoint);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CoffeeController_WhenCalledFiveTimes_ReturnsServiceNotAvailable()
    {
        // Arrange
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Location", Location);

        // Act
        await client.GetAsync(Endpoint);
        await client.GetAsync(Endpoint);
        await client.GetAsync(Endpoint);
        await client.GetAsync(Endpoint);
        var response = await client.GetAsync(Endpoint);

        // Assert
        if (_isAprilFirst)
        {
            var teaInsteadOfCoffee = (int)response.StatusCode;
            teaInsteadOfCoffee.Should().Be(418);
        }
        else
        {
            response.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
        }
    }

    [Fact]
    public async Task CoffeeController_WhenCalledMultipleTimes_ReturnsCorrectResponseCode()
    {
        // Arrange
        var invokeCount = 7;
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Location", Location);
        static HttpStatusCode expectedStatus(int count) => count % 5 == 0 ?
                HttpStatusCode.ServiceUnavailable : HttpStatusCode.OK;

        // Act and Assert
        for (int i = 1; i <= invokeCount; i++)
        {
            var response = await client.GetAsync(Endpoint);
            var actual = response.StatusCode;
            if (_isAprilFirst)
            {
                var teaInsteadOfCoffee = (int)actual;
                teaInsteadOfCoffee.Should().Be(418);
            }
            else
            {
                actual.Should().Be(expectedStatus(i));
            }
        }
    }
}
