namespace DecodeServerClientTest
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using DecodeServerClient;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Moq.Protected;

    [TestClass]
    public class ServerWrapperTest
    {
        private static readonly string HttpHost = "https://server-preview.azurewebsites.net";

        [TestMethod]
        public void GetServerVersionTest()
        {
            Task.Run(async () =>
            {
                Mock<HttpMessageHandler> mock = ArmMockForHttpClient("\"testing\"");

                string actual = await ServerWrapper.GetServerVersion();

                VerifyGetReqAtPath(mock, "/");

                Assert.AreEqual("testing", actual);
                VerifyGetReqAtPath(mock, "/");
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void GetSerialNumberFromServerTest()
        {
            Task.Run(async () =>
            {
                Mock<HttpMessageHandler> mock = ArmMockForHttpClient("{\"serial\":100,\"message\":\"Hello user!\"}");

                SerialNumerMsg actual = await ServerWrapper.GetSerialNumberFromServer("user");

                VerifyGetReqAtPath(mock, "/serial?user=user");

                Assert.AreEqual(100, actual.Serial);
                Assert.AreEqual("Hello user!", actual.Message);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void GetSerialNumberUsersFromServerTest()
        {
            Task.Run(async () => 
            {
                Mock<HttpMessageHandler> mock = ArmMockForHttpClient("[{\"serial\":0,\"user\":\"Alice\"},{\"serial\":1,\"user\":\"Bob\"}]");

                SerialNumberUser[] actual = await ServerWrapper.GetSerialNumberUsersFromServer();

                VerifyGetReqAtPath(mock, "/users");

                Assert.AreEqual(0, actual[0].Serial);
                Assert.AreEqual("Alice", actual[0].User);
                Assert.AreEqual(1, actual[1].Serial);
                Assert.AreEqual("Bob", actual[1].User);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void GetUserFromArgsTest()
        {
            Assert.AreEqual("anonymous", ServerWrapper.GetUserFromArgs(new string[0]));
            Assert.AreEqual("anonymous", ServerWrapper.GetUserFromArgs(new string[1] { string.Empty }));
            Assert.AreEqual("Alice", ServerWrapper.GetUserFromArgs(new string[1] { "Alice" }));
            Assert.AreEqual("Alice", ServerWrapper.GetUserFromArgs(new string[2] { "Alice", "Bob" }));
        }

        [TestMethod]
        public void GetSanitizedURLForUserTest()
        {
            Assert.AreEqual("/serial?user=%25test+user%25", ServerWrapper.GetSanitizedURLForSerial("%test user%"));
        }

        [TestMethod]
        public void FormatSerialNumberMsgTest()
        {
            Assert.AreEqual($"Your serial number is 0.\r\nHere is the message from the server:\r\nMessage", ServerWrapper.FormatSerialNumberMsg(new SerialNumerMsg { Serial = 0, Message = "Message" }));
        }

        [TestMethod]
        public void FormatSerialNumberUserListTest()
        {
            SerialNumberUser[] testedList = new SerialNumberUser[2]
            {
                new SerialNumberUser
                {
                    Serial = 0,
                    User = "Alice"
                },
                new SerialNumberUser
                {
                    Serial = 1,
                    User = "Bob"
                }
            };

            Assert.AreEqual("0 = Alice\n1 = Bob", ServerWrapper.FormatSerialNumberUserList(testedList));
        }

        [TestMethod]
        public void FormatSerialNumberUserTest()
        {
            SerialNumberUser testedUser = new SerialNumberUser
            {
                Serial = 0,
                User = "John"
            };

            testedUser.Serial = 0;
            testedUser.User = "John";

            Assert.AreEqual("0 = John", ServerWrapper.FormatSerialNumberUser(testedUser));
        }

        private Mock<HttpMessageHandler> ArmMockForHttpClient(string mockedResponse)
        {
            Mock<HttpMessageHandler> mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(mockedResponse) })
                .Verifiable();

            ServerWrapper.Client = new HttpClient(mock.Object)
            {
                BaseAddress = new Uri(HttpHost)
            };

            return mock;
        }

        private void VerifyGetReqAtPath(Mock<HttpMessageHandler> mock, string path)
        {
            mock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == new Uri($"{HttpHost}{path}")),
                ItExpr.IsAny<CancellationToken>());
        } 
    }
}
