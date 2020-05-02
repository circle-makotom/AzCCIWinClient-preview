using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DecodeServerClientTest")]

namespace DecodeServerClient
{
    internal class SerialNumberUser
    {
        public int Serial { get; set; }

        public string User { get; set; }
    }
}
