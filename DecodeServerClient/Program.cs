using System;
using System.Diagnostics;
using System.Threading.Tasks;

[assembly: System.Reflection.AssemblyVersion("0.0.*")]

namespace DecodeServerClient
{


    public class Program
    {
        public static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                string user = Util.GetUserFromArgs(args);

                Console.WriteLine((new SayHello(user)).GreetingMessage());
                Console.WriteLine();
                Console.WriteLine($"Client version: {FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).ProductVersion}");
                Console.WriteLine($"Server version: {await Util.GetServerVersion()}");
                Console.WriteLine();

                {
                    SerialNumerMsg serverMsg = await Util.GetSerialNumberMsgForUserFromServer(user);
                    Console.WriteLine(Util.FormatSerialNumberMsg(serverMsg));
                }

                Console.WriteLine();

                {
                    SerialNumberUser[] users = await Util.GetSerialNumberUsersFromServer();

                    Console.WriteLine("Below is the list of users:");
                    Console.WriteLine(Util.FormatSerialNumberUserList(users));
                }

                Console.WriteLine();

                Console.WriteLine("Press enter to close.");
                Console.ReadLine();
            }).GetAwaiter().GetResult();
        }
    }
}
