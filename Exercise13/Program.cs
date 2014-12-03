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
            var transportLayer = new TransportLayer();
            //var link = new LinkLayer(1000);
            var buf = new byte[1000];
            transportLayer.Receive(ref buf);
            Console.WriteLine(Encoding.ASCII.GetString(buf));
            Console.ReadLine();
        }
    }
}
