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
            KeepTrackOfUsageCounter();
            //KeyActivation();
            //AddFeature();
            //RemoveFeature();
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
            // here are some variables we need to configure:

            var token = "WyIyNSIsInM0R3V0ckFBeVJJSmlJNmlDVStOM1pFWnQ5eUpXYWU4VzhWOHJPZ3YiXQ==";

            // we only need the key once. when it is saved, we don't need to ask the user again.
            var keyString = "KMIAK-KYVVZ-QSFQQ-KKSJC";
            var productId = 3349;
            var maxNoOfTimes = 10; // the number of times the user should have access to the feature.


            // 1. Load key information from a file (to see if we've done this before)
            var storedKey = new KeyInformation().LoadFromFile("licensefile.txt"); // make sure you have write permission here

            // The idea here is to check if we already have an access token to access the desired key.
            // If yes, we read it, if no, we call the KeyLock method with our main access token (defined above)
            // and get a new access token, which we later store.
            AuthDetails auth = null;
            long keyId = 0;
            if (storedKey.IsValid() && storedKey.Auth != null && storedKey.Auth.Token != "")
            {
                auth = storedKey.Auth;
                keyId = storedKey.Id;
            }
            else
            {
                TokenGen(token, keyString, productId, out auth, out keyId);
            }


            var dataresult = SKM.ListDataObjects(auth, new ListDataObjectsModel
            {
                Contains = "UsageCount",
                ReferencerType = DataObjectType.Key,
                ReferencerId = (int)keyId
            });

            DataObject counter = null;
            if(dataresult != null && dataresult.Result == ResultType.Success)
            {
                counter = dataresult.DataObjects.FirstOrDefault();
            }
            else
            {
                // the token no longer has permission. we need a new one.
                storedKey.Auth = null;
                storedKey.SaveToFile("licensefile.txt");
                Console.WriteLine("Unable to authenticate. Please try again!");
                return;
            }

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
                // we simply set an upper bound to maxNoOfTimes.

                var incrementResult = SKM.IncrementIntValue(auth, new ChangeIntValueModel
                {
                    Id = counter.Id,
                    IntValue = 1,
                    EnableBound = true,
                    Bound = maxNoOfTimes
                });

                if (incrementResult != null && incrementResult.Result == ResultType.Success)
                {
                    Console.WriteLine("Usage counter updated.");
                }
                else
                {
                    Console.WriteLine("The counter has reached the maximum number.");
                }
            }

        }
        static void TokenGen(string token, string keyString, int productId, out AuthDetails auth, out long keyId)
        {
            var authForNewToken = new AuthDetails { Token = token };

            var result = SKM.KeyLock(authForNewToken, new KeyLockModel { Key = keyString, ProductId = productId });
            auth = result.GetAuthDetails();
            keyId = result.KeyId;

            var keyInfo = new KeyInformation { Auth = auth, Id = keyId, Key = keyString }.SaveToFile("licensefile.txt");
        }
        
    }
}
