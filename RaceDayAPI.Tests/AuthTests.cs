using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using RaceDayAPI.Data;

namespace RaceDayAPI.Tests
{
    public class AuthTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public AuthTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<RaceDayDbContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    // Add in-memory database for testing
                    services.AddDbContext<RaceDayDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb_" + Guid.NewGuid());
                    });
                });
            });
        }

        [Fact]
        public async Task Register_ValidUser_ReturnsOk()
        {
            var client = _factory.CreateClient();
            var registerData = new
            {
                email = "test@test.com",
                password = "Password123!",
                role = "Participant",
                firstName = "Test",
                lastName = "User",
                contactNumber = "1234567890",
                dateOfBirth = "2000-01-01",
                gender = "Male",
                emergencyContact = "Emergency",
                emergencyPhone = "0987654321"
            };

            var response = await client.PostAsJsonAsync("/api/auth/register", registerData);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            var client = _factory.CreateClient();
            var loginData = new
            {
                email = "wrong@test.com",
                password = "WrongPassword"
            };

            var response = await client.PostAsJsonAsync("/api/auth/login", loginData);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetEvents_WithoutAuth_ReturnsOk()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/events");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateEvent_WithoutAuth_ReturnsUnauthorized()
        {
            var client = _factory.CreateClient();
            var eventData = new
            {
                eventName = "Test Marathon",
                eventDate = "2025-12-01T06:00:00",
                location = "Cape Town",
                maxParticipants = 1000
            };

            var response = await client.PostAsJsonAsync("/api/events", eventData);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
