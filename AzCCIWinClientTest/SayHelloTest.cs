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
            string[] testedName = { "Alice", "Bob" };
            var inst = new SayHello(testedName[0]);

            Assert.AreEqual($"Bonjour, {testedName[0]}! Welcome to this app!", inst.GreetingMessage());

            inst.User = testedName[1];
            Assert.AreEqual($"Bonjour, {testedName[1]}! Welcome to this app!", inst.GreetingMessage());
        }
    }
}
