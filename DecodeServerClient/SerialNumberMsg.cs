using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DecodeServerClientTest")]

namespace DecodeServerClient
{
    internal class SerialNumerMsg
    {
        public int Serial { get; set; }

        public string Message { get; set; }
    }
}
