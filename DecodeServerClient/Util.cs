namespace DecodeServerClient
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Web;

    internal class Util
    {
        internal static string GetUserFromArgs(string[] args)
        {
            return args.Length > 0 && args[0] != string.Empty ? args[0] : "anonymous";
        }

        internal static async Task<SerialNumerMsg> GetSerialNumberMsgForUserFromServer(string user)
        {
            return Util.SerialNumberMsgFromJSON(await Util.HTTPGet(Util.GetSanitizedURLForUser(user)));
        }

        internal static async Task<string> HTTPGet(string url)
        {
            WebRequest req = WebRequest.Create(new Uri(url));
            WebResponse res = await req.GetResponseAsync();
            Stream stream = res.GetResponseStream();
            StreamReader reader = new StreamReader(stream);

            return await reader.ReadToEndAsync();
        }

        internal static SerialNumerMsg SerialNumberMsgFromJSON(string str)
        {
            return JsonSerializer.Deserialize<SerialNumerMsg>(str, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        internal static string FormatSerialNumberMsg(SerialNumerMsg msg)
        {
            return $@"Your serial number is {msg.Serial}.
Here is the mssage from the server:
{msg.Message}
";
        }

        private static string GetSanitizedURLForUser(string user)
        {
            return $"http://localhost:58888/?user={HttpUtility.UrlEncode(user)}";
        }
    }
}
