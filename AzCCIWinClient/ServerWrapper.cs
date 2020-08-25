[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("AzCCIWinClientTest")]

namespace AzCCIWinClient
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Web;

    internal class ServerWrapper
    {
        private static HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://server-preview.azurewebsites.net")
        };

        internal static HttpClient Client
        {
            get
            {
                return client;
            }

            set
            {
                client = value;
            }
        }

        internal static async Task<string> GetServerVersion()
        {
            return JsonSerializer.Deserialize<string>(await ServerWrapper.HTTPGet("/"));
        }

        internal static async Task<SerialNumerMsg> GetSerialNumberFromServer(string user)
        {
            return JsonSerializer.Deserialize<SerialNumerMsg>(await ServerWrapper.HTTPGet(ServerWrapper.GetSanitizedURLForSerial(user)), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        internal static async Task<SerialNumberUser[]> GetSerialNumberUsersFromServer()
        {
            return JsonSerializer.Deserialize<SerialNumberUser[]>(await ServerWrapper.HTTPGet("/users"), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        internal static string GetUserFromArgs(string[] args)
        {
            return args.Length > 0 && args[0] != string.Empty ? args[0] : "anonymous";
        }

        internal static string GetSanitizedURLForSerial(string user)
        {
            return $"/serial?user={HttpUtility.UrlEncode(user)}";
        }

        internal static string FormatSerialNumberMsg(SerialNumerMsg msg)
        {
            return $@"Your serial number is {msg.Serial}.
Here is the message from the server:
{msg.Message}";
        }

        internal static string FormatSerialNumberUserList(SerialNumberUser[] userList)
        {
            var userStr = new List<string>();

            Array.ForEach(userList, (SerialNumberUser user) => userStr.Add(ServerWrapper.FormatSerialNumberUser(user)));

            return string.Join("\n", userStr.ToArray());
        }

        internal static string FormatSerialNumberUser(SerialNumberUser user)
        {
            return $"{user.Serial} = {user.User}";
        }

        private static async Task<string> HTTPGet(string url)
        {
            HttpResponseMessage response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
