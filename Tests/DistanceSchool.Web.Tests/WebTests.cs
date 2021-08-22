namespace DistanceSchool.Web.Tests
{
    using System.Net;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Testing;

    using Xunit;

    public class WebTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> server;

        public WebTests(WebApplicationFactory<Startup> server)
        {
            this.server = server;
        }

        [Theory]
        [InlineData("/School/OneSchool")]
        [InlineData("/Team/AddTeam")]
        [InlineData("/Administration/Dashboard/StudentCandidacies")]
        [InlineData("/Administration/Dashboard/SchoolManagerHome")]
        [InlineData("/Teacher/OneTeacher")]
        [InlineData("/Team/AddDiscipline")]
        [InlineData("/Team/ShiftsTecher")]
        [InlineData("/Teacher/AddDiscipline")]
        public async Task ChecAuthorizatioPagesReurnRedirectStatusCode(string url)
        {
            var client = this.server.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
            var response = await client.GetAsync(url);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Fact]
        public async Task AllSchoolPageContainHOneSchooList()
        {
            var client = this.server.CreateClient();
            var response = await client.GetAsync("School/AllSchool");
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Списък с училища :", responseContent);
        }
    }
}
