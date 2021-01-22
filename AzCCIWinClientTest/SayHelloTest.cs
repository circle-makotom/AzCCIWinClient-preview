namespace AzCCIWinClientTest
{
    using AzCCIWinClient;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SayHelloTest
    {
        [TestMethod]
        public void GreetingMessageTest()
        {
            string[] testedName = { "Alice", "Bob", string.Empty };
            var inst = new SayHello(testedName[0]);

            Assert.AreEqual($"Hello, {testedName[0]}! Welcome to this app.", inst.GreetingMessage());

            inst.User = testedName[1];
            Assert.AreEqual($"Hello, {testedName[1]}! Welcome to this app.", inst.GreetingMessage());

            inst.User = testedName[2];
            Assert.AreEqual($"Hello, anonymous! Welcome to this app.", inst.GreetingMessage());
        }
    }
}
