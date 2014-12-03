using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise13
{
    class Program
    {
        static void Main(string[] args)
        {
            var transportLayer = new TransportLayer(1000);                    
            
            while (true)
            {
                Console.WriteLine("----- Mode Menu -----");
                Console.WriteLine("1) Send Mode");
                Console.WriteLine("2) Receive Mode");
                Console.WriteLine("ESC) Quit");

                ConsoleKeyInfo choice = Console.ReadKey();

                if (choice.Key == ConsoleKey.Escape)
                    break;

                if (choice.KeyChar == '1')
                {
                    Console.WriteLine();
                    Console.WriteLine("Enter string to send. Finish with <ENTER>");
                    string toSendString = Console.ReadLine();
                    var toSend = new byte[toSendString.Length];
                    toSend = Encoding.ASCII.GetBytes(toSendString);
                    transportLayer.Send(toSend, toSend.Length);
                }

                if (choice.KeyChar == '2')
                {
                    Console.WriteLine();
                    Console.WriteLine("Waiting for message...");
                    var buf = new byte[1000];
                    transportLayer.Receive(ref buf);
                    Console.WriteLine(Encoding.ASCII.GetString(buf));
                    Console.WriteLine("Press any key to return to menu.");
                    Console.ReadKey();
                }
                
            }
        }
    }
}
