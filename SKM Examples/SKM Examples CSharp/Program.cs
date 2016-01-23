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
            AddFeature();
            RemoveFeature();
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

        static void AddFeature()
        {
            var keydata = new FeatureModel() { Key = "LXWVI-HSJDU-CADTC-BAJGW", Feature = 2, ProductId = 3349 };
            var auth = new AuthDetails() { Token = "WyI2Iiwib3lFQjFGYk5pTHYrelhIK2pveWdReDdEMXd4ZDlQUFB3aGpCdTRxZiJd" };

            var result = SKM.AddFeature(auth, keydata);

            if (result != null && result.Result == ResultType.Success)
            {
                // feature 2 is set to true.
                Console.WriteLine("Feature 2 set to true");
            }
            else
            {
                Console.WriteLine("Feature 2 was not changed because of an error: " + result.Message);
            }
        }

        static void RemoveFeature()
        {

            var keydata = new FeatureModel() { Key = "LXWVI-HSJDU-CADTC-BAJGW", Feature = 2, ProductId = 3349 };
            var auth = new AuthDetails() { Token = "WyI2Iiwib3lFQjFGYk5pTHYrelhIK2pveWdReDdEMXd4ZDlQUFB3aGpCdTRxZiJd" };

            var result = SKM.RemoveFeature(auth, keydata);

            if (result != null && result.Result == ResultType.Success)
            {
                // feature 2 is set to false.
                Console.WriteLine("Feature 2 set to false");
            }
            else
            {
                Console.WriteLine("Feature 2 was not changed because of an error: " + result.Message);
            }

        }

        static void KeepTrackOfUsageCounter()
        {
            var authForNewToken = new AuthDetails { Token = "" };

            var result = SKM.KeyLock(authForNewToken, new KeyLockModel { Key = "key string", ProductId = 3 });

            var auth = result.GetAuthDetails();
            var keyId = result.KeyId;


            var counter = SKM.ListDataObjects(auth, new ListDataObjectsModel
            {
                Contains = "UsageCount",
                ReferencerType = DataObjectType.Key,
                ReferencerId = (int)keyId
            }).DataObjects.FirstOrDefault();

            if (counter == null)
            {
                // this license key does not have a counter configured yet.
                // so, let's create one!

                var addResult = SKM.AddDataObject(auth, new AddDataObjectModel
                {
                    IntValue = 1,
                    Name = "UsageCount",
                    ReferencerType = DataObjectType.Key,
                    ReferencerId = (int)keyId
                });

                if (addResult != null && addResult.Result == ResultType.Success)
                {
                    Console.WriteLine("Usage counter created!");
                }
                else
                {
                    Console.WriteLine("An error occurred. Could not create a new data object.");
                }
            }
            else
            {
                // this license has an existing usage counter.
                // we could check the value and then increment,
                // however, SKM will do it for us. 
            }
        }
    }
}
