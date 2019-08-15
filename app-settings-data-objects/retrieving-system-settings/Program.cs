using System;

using SKM.V3;
using SKM.V3.Models;
using SKM.V3.Methods;
using System.Linq;

namespace retrieving_system_settings
{
    class Program
    {
        static void Main(string[] args)
        {

            var systemSettings = Data.ListDataObjects("", new ListDataObjectsModel
            {
                ReferencerType = DataObjectType.Product,
                ReferencerId = 3349,  // <- the product id
            });
            
            if(!Helpers.IsSuccessful(systemSettings) || systemSettings.DataObjects == null)
            {
                Console.WriteLine("Could not retrieve the settings.");
            }

            var settings = systemSettings.DataObjects.ToDictionary(x=> x.Name, x => x);

            if(settings.ContainsKey("DOTNET_RUNTIME"))
                Console.WriteLine(settings["DOTNET_RUNTIME"].StringValue);

            Console.WriteLine("Hello World!");
        }
    }
}
