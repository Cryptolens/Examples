using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using SKM.V3;
using SKM.V3.Methods;
using SKM.V3.Models;

namespace SDKExample
{
    public class MathMethods
    {
        private Dictionary<string, bool> permittedMethods = new Dictionary<string, bool>();

        public MathMethods(string licenseKey)
        {
            if (!licenseCheck(licenseKey))
            {
                // error occured when verifying the license.
                throw new ArgumentException("License verification failed.");
            }

        }

        private bool licenseCheck(string licenseKey)
        {
            // From: https://help.cryptolens.io/examples/key-verification

            var RSAPubKey = "<RSAKeyValue><Modulus>sGbvxwdlDbqFXOMlVUnAF5ew0t0WpPW7rFpI5jHQOFkht/326dvh7t74RYeMpjy357NljouhpTLA3a6idnn4j6c3jmPWBkjZndGsPL4Bqm+fwE48nKpGPjkj4q/yzT4tHXBTyvaBjA8bVoCTnu+LiC4XEaLZRThGzIn5KQXKCigg6tQRy0GXE13XYFVz/x1mjFbT9/7dS8p85n8BuwlY5JvuBIQkKhuCNFfrUxBWyu87CFnXWjIupCD2VO/GbxaCvzrRjLZjAngLCMtZbYBALksqGPgTUN7ZM24XbPWyLtKPaXF2i4XRR9u6eTj5BfnLbKAU5PIVfjIS+vNYYogteQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

            var auth = "WyIxNjY5IiwiZW93b1ZmbFZ2RllVSUF6d1JMcnRpemVTK2Y0eTVvVXlyNHZFWjNSWCJd";
            var result = Key.Activate(token: auth, parameters: new ActivateModel()
            {
                Key = licenseKey,
                ProductId = 3941,
                Sign = true,
                MachineCode = Helpers.GetMachineCode()
            });

            if (result == null || result.Result == ResultType.Error ||
                !result.LicenseKey.HasValidSignature(RSAPubKey).IsValid())
            {
                // an error occurred or the key is invalid or it cannot be activated
                // (eg. the limit of activated devices was achieved)
                return false;
            }
            else
            {
                // this is a time-limited license, so check expiration date.
                if (result.LicenseKey.F1 && !result.LicenseKey.HasNotExpired().IsValid())
                    return false;

                // everything went fine if we are here!
                permittedMethods = new Dictionary<string, bool>();

                if (result.LicenseKey.F5)
                {
                    permittedMethods.Add("Abs", true);
                }
                if (result.LicenseKey.F6)
                {
                    permittedMethods.Add("Factorial", true);
                }
                if (result.LicenseKey.F7)
                {
                    permittedMethods.Add("Fibonacci", true);
                }

                return true;
            }

        }

        /// <summary>
        /// Compute the absolute value of a number
        /// </summary>
        public int Abs(int num)
        {
            if (!permittedMethods.ContainsKey("Abs"))
                throw new ArgumentException("The use of this method is not permitted.");

            if (num > 0)
                return num;
            return -num;
        }

        /// <summary>
        /// Compute the factorial, eg. num!.
        /// </summary>
        public int Factorial(int num)
        {
            if (!permittedMethods.ContainsKey("Factorial"))
                throw new ArgumentException("The use of this method is not permitted.");

            if (num == 0)
                return 1;
            return num * Factorial(num - 1);
        }

        /// <summary>
        /// Compute the nth fibonacci number.
        /// </summary>
        public int Fibonacci(int num)
        {
            if (!permittedMethods.ContainsKey("Fibonacci"))
                throw new ArgumentException("The use of this method is not permitted.");

            return fibonacciHelper(num);
        }

        // we use a helper methods to avoid checking the permission on each iteration.
        private int fibonacciHelper(int num)
        {
            if (num == 0 || num == 1)
                return 1;
            return fibonacciHelper(num - 1) + fibonacciHelper(num - 2);
        }
    }
}
