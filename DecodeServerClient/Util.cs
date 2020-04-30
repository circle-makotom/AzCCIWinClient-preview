namespace DecodeServerClient
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Web;

    internal class Util
    {
        private static string httpHost = "http://localhost:58888";

        internal static string GetUserFromArgs(string[] args)
        {
            return args.Length > 0 && args[0] != string.Empty ? args[0] : "anonymous";
        }

        internal static async Task<string> HTTPGet(string url)
        {
            WebRequest req = WebRequest.Create(new Uri(url));
            WebResponse res = await req.GetResponseAsync();
            Stream stream = res.GetResponseStream();
            StreamReader reader = new StreamReader(stream);

            return await reader.ReadToEndAsync();
        }

        internal static async Task<SerialNumerMsg> GetSerialNumberMsgForUserFromServer(string user)
        {
            return Util.SerialNumberMsgFromJSON(await Util.HTTPGet(Util.GetSanitizedURLForUser(user)));
        }

        internal static SerialNumerMsg SerialNumberMsgFromJSON(string str)
        {
            return JsonSerializer.Deserialize<SerialNumerMsg>(str, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        internal static string FormatSerialNumberMsg(SerialNumerMsg msg)
        {
            return $@"Your serial number is {msg.Serial}.
Here is the mssage from the server:
{msg.Message}";
        }

        internal static async Task<SerialNumberUser[]> GetSerialNumberUsersFromServer()
        {
            return JsonSerializer.Deserialize<SerialNumberUser[]>(await Util.HTTPGet($"{Util.httpHost}/users"), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        internal static string FormatSerialNumberUserList(SerialNumberUser[] userList)
        {
            List<string> userStr = new List<string>();

            Array.ForEach(userList, (SerialNumberUser user) => userStr.Add(Util.FormatSerialNumberUser(user)));

            return string.Join("\n", userStr.ToArray());
        }

        private static string GetSanitizedURLForUser(string user)
        {
            return $"{Util.httpHost}/serial?user={HttpUtility.UrlEncode(user)}";
        }

        private static string FormatSerialNumberUser(SerialNumberUser user)
        {
            return $"{user.Serial} = {user.User}";
        }
    }
}
