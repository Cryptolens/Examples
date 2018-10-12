using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SKM.V3;
using SKM.V3.Methods;
using SKM.V3.Models;


namespace offline_verification
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

                // -------------------new code starts -------------------
                // we will try to check for a cached response/certificate

                var licensefile = new LicenseKey();

                if (licensefile.LoadFromFile("licensefile")
                              .HasValidSignature(RSAPubKey, 3)
                              .IsValid())
                {
                    Console.WriteLine("The license is valid!");
                }
                else
                {
                    Console.WriteLine("The license does not work.");
                }
                // -------------------new code ends ---------------------
            }
            else
            {
                // everything went fine if we are here!
                Console.WriteLine("The license is valid!");

                // -------------------new code starts -------------------
                // saving a copy of the response/certificate
                result.LicenseKey.SaveToFile("licensefile");
                // -------------------new code ends ---------------------
            }

            Console.ReadLine();
        }
    }
}
