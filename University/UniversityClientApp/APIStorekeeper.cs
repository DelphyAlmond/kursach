using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using UniversityContracts.ViewModels;
using Azure.Core;
using UniversityDataModels.Models;
using UniversityDatabaseImplement.Models;

namespace UniversityClientAppStorekeeper
{
	[Route("api/[controller]")]
	[ApiController]
	public class APIStorekeeper : ControllerBase
	{
		private static readonly HttpClient _client = new();
		public static UserViewModel? Client { get; set; } = null;
		public static void Connect(IConfiguration configuration)
		{
			_client.BaseAddress = new Uri(configuration["IPAddress"]);
			_client.DefaultRequestHeaders.Accept.Clear();
			_client.DefaultRequestHeaders.Accept.Add(new
		   MediaTypeWithQualityHeaderValue("application/json"));
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
        public static async Task<T?> GetRequestDisciplineAsync<T>(string requestUrl)
        {
        var response = await _client.GetAsync(requestUrl);
        var result = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StudentConverter()}
            };

            return JsonConvert.DeserializeObject<T>(result, settings);
        }
        else
        {
            throw new Exception(result);
        }
        }
	}

    

    public class StudentConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IStudentModel);
            //return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            if (existingValue is Student student)
                return student;

            var student1 = new Student();
            serializer.Populate(reader, student1);
            return student1;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var student = (Student)value;
            serializer.Serialize(writer, student);
        }
    }

    public class DisciplineConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IDisciplineModel);
            //return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize<Discipline>(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
