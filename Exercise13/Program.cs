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
            var link = new Link(1000);
            while (true)
            {
                string toSendString = Console.ReadLine();
                var toSend = new byte[toSendString.Length];
                toSend = Encoding.ASCII.GetBytes(toSendString);
                link.Send(toSend, toSend.Length);
            }
            
        }
    }
}
