using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SKM;
using SKM.V3.Methods;
using SKM.V3.Models;

namespace SKM_Examples_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            //KeepTrackOfUsageCounter();
            //KeepTrackOfUsageCounterWithoutStoringToken();
            //KeyActivation();
            //AddFeature();
            //RemoveFeature();
            Console.ReadLine();
        }

        static void AddFeature()
        {
            var keydata = new FeatureModel() { Key = "LXWVI-HSJDU-CADTC-BAJGW", Feature = 2, ProductId = 3349 };
            var auth = "WyI2Iiwib3lFQjFGYk5pTHYrelhIK2pveWdReDdEMXd4ZDlQUFB3aGpCdTRxZiJd";

            var result = Key.AddFeature(auth, keydata);

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
            var auth = "WyI2Iiwib3lFQjFGYk5pTHYrelhIK2pveWdReDdEMXd4ZDlQUFB3aGpCdTRxZiJd";

            var result = Key.RemoveFeature(auth, keydata);

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



    }
}
