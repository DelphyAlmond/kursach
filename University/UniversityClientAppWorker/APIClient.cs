using Newtonsoft.Json;
using UniversityContracts.ViewModels;
using System.Net.Http.Headers;
using System.Text;
using Azure.Core;
using UniversityDataModels.Models;
using UniversityDatabaseImplement.Models;
using Microsoft.AspNetCore.Mvc;

namespace UniversityClientAppWorker
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIClient : ControllerBase
    {
            private static readonly HttpClient _client = new();
            public static UserViewModel? User { get; set; } = null;
            public static void Connect(IConfiguration configuration)
            {
                _client.BaseAddress = new Uri(configuration["IPAddress"]);
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            public static T? GetRequest<T>(string requestUrl)
            {
                var response = _client.GetAsync(requestUrl);
                var result = response.Result.Content.ReadAsStringAsync().Result;
                if (response.Result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(result);
                }
                else
                {
                    throw new Exception(result);
                }
            }
        public static async Task<T?> GetRequestPlanOfStudyAsync<T>(string requestUrl)
        {
            var response = await _client.GetAsync(requestUrl);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var settings = new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter> { new TeacherConverter() }
                };
                return JsonConvert.DeserializeObject<T>(result, settings);
            }
            else
            {
                throw new Exception(result);
            }
        }

        public static async Task<T?> GetRequestStudentsAsync<T>(string requestUrl)
        {
            var response = await _client.GetAsync(requestUrl);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var settings = new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter> { new TeacherConverter() }
                };
                return JsonConvert.DeserializeObject<T>(result, settings);
            }
            else
            {
                throw new Exception(result);
            }
        }

        public static async Task<T?> GetRequestAsync<T>(string requestUrl)
        {
            var response = await _client.GetAsync(requestUrl);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
            else
            {
                throw new Exception(result);
            }
        }
        public static void PostRequest<T>(string requestUrl, T model)
            {
                var json = JsonConvert.SerializeObject(model);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = _client.PostAsync(requestUrl, data);
                
                var result = response.Result.Content.ReadAsStringAsync().Result;
                if (!response.Result.IsSuccessStatusCode)
                {
                    throw new Exception(result);
                }
            }
    }
    public class TeacherConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ITeacherModel);
            //return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            // Пропускаем десериализацию, если уже есть экземпляр Teacher
            if (existingValue is Teacher teacher)
                return teacher;

            var teach = new Teacher();
            serializer.Populate(reader, teach);
            return teach;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var teacher = (Teacher)value;
            serializer.Serialize(writer, teacher);
        }
    }
}
