namespace DecodeServerClient
{
    using System;
    using System.Threading.Tasks;

    public class Program
    {
        public static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                string user = Util.GetUserFromArgs(args);

                Console.WriteLine((new SayHello(user)).GreetingMessage());
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
