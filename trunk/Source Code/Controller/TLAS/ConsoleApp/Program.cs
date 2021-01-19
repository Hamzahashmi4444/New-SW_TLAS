using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string cardvalue= "22121";
            string BCUWeight ="23423";
            string compid="1";
            string BAY ="Bay01";

            string cmdArguments = String.Format("{0}{1}{2}", "key:" + cardvalue, " ", "weight:" + BCUWeight + " batchcomp:true comptid:" + compid +" baycode:" + BAY+ "");
           
            Console.WriteLine(args[0].ToString());
            Console.WriteLine(args[1].ToString());


            Console.ReadKey();
        }
    }
}
