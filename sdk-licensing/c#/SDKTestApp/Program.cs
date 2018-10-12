using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SDKExample;

namespace SDKTestApp
{
    class Program
    {
        static void Main(string[] args)
        {

            // all features: FULXY-NADQW-ZAMPX-PQHUT
            // just abs method: KDDSE-QWAOT-LGFUL-JLZTM

            var math = new MathMethods("FULXY-NADQW-ZAMPX-PQHUT");

            Console.WriteLine(math.Abs(5));
            Console.WriteLine(math.Fibonacci(5));

            Console.ReadLine();

            return;
        }
    }
}
