using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SKM.V3;
using SKM.V3.Methods;
using SKM.V3.Models;

namespace key_verification
{
    class Program
    {
        static void Main(string[] args)
        {
            var licenseKey = "JJWTE-MVKLR-YQMKD-IFXUV";

            var RSAPubKey = "<RSAKeyValue><Modulus>wflqVfb+6gkb1lHr1fysux8o+oJREy0M2hM3yEKh92w5rXoc673ZRI9JTg725IqHvr/031TMqFAMvfdQ8X5gB0L3gb3E1tY/QvpZxwobRs6Xpz3XxuGhZ7cIhO9uWLZuykSvoD+PQZMOsdp+M05p9KS4eTsbuSo1w0kwkp6AnumMiG7ICOxYsdGg7YlzX5DN3m6QgG9Fg+6faIOLyBxXktBK4+ligCMNYdRLd3z9UqhGGy4u/Hnz3VD2bzJ99JWQYx00HtXyojmUHi25Yk2G5eki3sQXM7gYzWkZOzaEQ5/rMghp0O3/eJblWtranxQv4HBCm/b8l4oVh4GTcN0x7w==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

            var auth = "WyIxMDQwIiwiK094dW9yUWxkMitmYTBIaTVQdDBsYXpKenpoUithK2IyQVR3KzlzZCJd";
            var result = Key.Activate(token: auth, parameters: new ActivateModel()
            {
                Key = licenseKey,
                ProductId = 3843,
                Sign = true,
                MachineCode = Helpers.GetMachineCode()
            });

            if (result == null || result.Result == ResultType.Error ||
                !result.LicenseKey.HasValidSignature(RSAPubKey).IsValid())
            {
                // an error occurred or the key is invalid or it cannot be activated
                // (eg. the limit of activated devices was achieved)
                Console.WriteLine("The license does not work.");
            }
            else
            {
                // everything went fine if we are here!
                Console.WriteLine("The license is valid!");

                if (result.LicenseKey.HasFeature(1).HasNotExpired().IsValid())
                {
                    Console.WriteLine("This is a time-limited license that is still valid in " + result.LicenseKey.DaysLeft() + " day(s)." );
                }
                else if(result.LicenseKey.HasNotFeature(1).IsValid())
                {
                    Console.WriteLine("This license does not have a time limit.");
                }
                else
                {
                    Console.WriteLine("It appears this license has expired.");
                }

                if(result.LicenseKey.HasFeature(3).IsValid()) {
                    Console.WriteLine("You have all features!");
                }
            }

            Console.ReadLine();
        }
    }
}
