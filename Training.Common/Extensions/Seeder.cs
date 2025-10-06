using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Training.Common.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class Seeder<T> where T : class
    {
        public static T SeedIt(string jsonData)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new PrivateSetterContractResolver()
            };
            return JsonConvert.DeserializeObject<T>(jsonData, settings);
        }
    }
}