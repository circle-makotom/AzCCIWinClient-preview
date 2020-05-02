[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("DecodeServerClientTest")]

namespace DecodeServerClient
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
            return $"Greetings, {this.User}!";
        }
    }
}
