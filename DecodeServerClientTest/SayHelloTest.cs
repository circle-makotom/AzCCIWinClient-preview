namespace DecodeServerClientTest
{
    using DecodeServerClient;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SayHelloTest
    {
        [TestMethod]
        public void GreetingMessageTest()
        {
            string[] testedName = { "Alice", "Bob" };
            SayHello inst = new SayHello(testedName[0]);

            Assert.AreEqual($"Greetings, {testedName[0]}!", inst.GreetingMessage());

            inst.User = testedName[1];
            Assert.AreEqual($"Greetings, {testedName[1]}!", inst.GreetingMessage());
        }
    }
}
