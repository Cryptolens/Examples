using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SKM.V3;
using SKM.V3.Methods;
using SKM.V3.Models;


namespace air_gapped
{
    class Program
    {
        static void Main(string[] args)
        {
            var RSAPubKey = "<RSAKeyValue><Modulus>wflqVfb+6gkb1lHr1fysux8o+oJREy0M2hM3yEKh92w5rXoc673ZRI9JTg725IqHvr/031TMqFAMvfdQ8X5gB0L3gb3E1tY/QvpZxwobRs6Xpz3XxuGhZ7cIhO9uWLZuykSvoD+PQZMOsdp+M05p9KS4eTsbuSo1w0kwkp6AnumMiG7ICOxYsdGg7YlzX5DN3m6QgG9Fg+6faIOLyBxXktBK4+ligCMNYdRLd3z9UqhGGy4u/Hnz3VD2bzJ99JWQYx00HtXyojmUHi25Yk2G5eki3sQXM7gYzWkZOzaEQ5/rMghp0O3/eJblWtranxQv4HBCm/b8l4oVh4GTcN0x7w==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

            var licensefile = new LicenseKey().LoadFromFile("ActivationFile20180606.skm");

            if(licensefile.HasValidSignature(RSAPubKey, 365).IsOnRightMachine(SKGL.SKM.getSHA256).IsValid())
            {
                // if you have multiple products, make sure the license file has correct product id.
                
                //if(licensefile.ProductId != 123)
                //{
                //    Console.WriteLine("This license file is not for this product.");
                //    return;
                //}

                Console.WriteLine("License verification successful.");
            }
            else
            {
                Console.WriteLine("The license file is not valid or has expired.");
                Console.WriteLine("Please obtain a new one here: https://app.cryptolens.io/Form/A/onp4cDAc/222");
                Console.WriteLine("Your machine code is: " + Helpers.GetMachineCode());
            }

            Console.ReadLine();
        }
    }
}
