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

                {
                    SerialNumerMsg serverMsg = await Util.GetSerialNumberMsgForUserFromServer(user);
                    Console.WriteLine(Util.FormatSerialNumberMsg(serverMsg));
                }

                Console.WriteLine("Press enter to close.");
                Console.ReadLine();
            }).GetAwaiter().GetResult();
        }
    }
}
