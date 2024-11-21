using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HybridDataAccess.DataSerializator
{
    public sealed class JsonHandler
    {
        private readonly JsonSerializerOptions _options;

        

        public JsonHandler(bool viewNull=true)
        {
            if (viewNull)
            {
                _options = new JsonSerializerOptions()
                {
                
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    WriteIndented = true
                };
            }
            else
            {
                _options = new JsonSerializerOptions()
                {
                    WriteIndented = true
                };
            }
            
        }

        public string SerializeOne<T>(T value)
        {
            return JsonSerializer.Serialize(value, _options);
        }

        public string SerializeMany<T>(List<T> values)
        {
            return JsonSerializer.Serialize(values, _options);
        }

        public T DeserializeOne<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _options)?? throw new Exception("JsonHandler: Json is null");
        }
        public List<T> DeserializeMany<T>(string json)
        {
            return JsonSerializer.Deserialize<List<T>>(json, _options) ?? throw new Exception("JsonHandler: Json is null");
        }
    }
}
