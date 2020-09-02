[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("AzCCIWinClientTest")]

namespace AzCCIWinClient
{
    internal class SayHello
    {
        internal SayHello(string user)
        {
            this.User = user;
        }

        internal string User { get; set; }

        internal string GreetingMessage()
        {
            return $"Hello, {this.User}!";
        }
    }
}
