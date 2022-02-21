using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace RestAPI.IntegrationTests.Fixture
{
    public class WireMockFixture
    {
        public readonly WireMockServer WireMockServer = WireMockServer.Start("http://localhost:9632/");

        public WireMockFixture()
        {
            MockTransportService();
        }

        private void MockTransportService()
        {
            WireMockServer
                .Given(Request.Create()
                    .WithPath("api/v1/notify/*")
                    .UsingPut())
                .RespondWith(Response.Create()
                    .WithStatusCode(HttpStatusCode.OK));
        }
    }
}