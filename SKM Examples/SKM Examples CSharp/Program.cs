using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SKGL;

namespace SKM_Examples_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            KeyActivation();
            Console.ReadLine();
        }

        static void KeyActivation()
        {

            string machineCode = SKM.getMachineCode(SKM.getSHA1); // using SHA1 as the hashing algorithm
            string licensekey = "MNIVR-MGQRL-QGUZK-BGJHQ";

            var keyinfo = SKM.KeyActivation(pid: "3", uid: "2", hsum: "751963", 
                                            sid: licensekey, mid:  machineCode);

            var newKey = keyinfo.NewKey;

            if (keyinfo.IsValid())
            {
                //valid key

                Console.WriteLine("Created:" + keyinfo.CreationDate.ToShortDateString());
                Console.WriteLine("Expired?: " + keyinfo.HasNotExpired().IsValid());

            }
            else
            {
                //invalid key
                Console.WriteLine("Failed activation.");
            }

        }
    }
}
