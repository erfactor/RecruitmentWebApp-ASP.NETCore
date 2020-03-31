using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Recruitment.Core;
using Recruitment.Data;
using Xunit;

namespace Recruitment.IntegrationTests.IntegrationTests
{
    #region snippet1

    public class BasicTests
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        public BasicTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private readonly WebApplicationFactory<Startup> _factory;

        [Theory]
        [InlineData("/")]
        [InlineData("/JobOffers")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public void Check_DatabaseWorking()
        {
            IJobOfferData db = null;
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    db = serviceProvider.GetRequiredService<IJobOfferData>();
                });
            }).CreateClient();

            var testString = "TestJobOffer";
            var offers = db.GetAll().Where(o => o.Name == testString);

            foreach (var jobOffer in offers)
            {
                db.Delete(jobOffer.JobOfferId);
            }

            var offer = new JobOffer(0, testString, "desc");
            db.Create(offer);
            db.Commit();
            Assert.Contains(db.GetAll(), o => o.Name == testString);
        }
    }

    #endregion
}