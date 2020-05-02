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
                {
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    Console.WriteLine($"Client file version: {fvi.FileVersion}");
                    Console.WriteLine($"Client product version: {fvi.ProductVersion}");
                    Console.WriteLine($"Server version: {await ServerWrapper.GetServerVersion()}");
                    Console.WriteLine();
                }

                {
                    string user = args.Length > 0 ? args[0] : Program.PromptUsername();

                    Console.WriteLine();
                    Console.WriteLine(new SayHello(user).GreetingMessage());
                    Console.WriteLine();

                    {
                        SerialNumerMsg serverMsg = await ServerWrapper.GetSerialNumberMsgForUserFromServer(user);
                        Console.WriteLine(ServerWrapper.FormatSerialNumberMsg(serverMsg));
                    }

                    Console.WriteLine();

                    {
                        SerialNumberUser[] users = await ServerWrapper.GetSerialNumberUsersFromServer();

                        Console.WriteLine("Below is the list of users:");
                        Console.WriteLine(ServerWrapper.FormatSerialNumberUserList(users));
                    }
                }

                Console.WriteLine();

                Console.WriteLine("Press enter to close.");
                Console.ReadLine();
            }).GetAwaiter().GetResult();
        }

        private static string PromptUsername()
        {
            Console.Write("Type your name: ");
            return Console.ReadLine();
        }
    }
}
