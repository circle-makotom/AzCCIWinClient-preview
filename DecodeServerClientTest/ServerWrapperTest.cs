namespace DecodeServerClientTest
{
    using DecodeServerClient;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ServerWrapperTest
    {
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
    }
}
