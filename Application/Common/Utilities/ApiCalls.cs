using Application.Services.IServiceCaller;
using Domain.Contracts.V1;
using Domain.ViewModel.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Utilities
{
    public class ApiCalls<T> where T : class
    {
        public static async Task<IEnumerable<T>> GetData(string apiUrl)
        {
            List<T> list = new List<T>();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(data);
                }
            }
            return list;
        }
        public static async Task<IEnumerable<T>> GetDataById(string apiUrl)
        {
            string data = "";
            List<T> list = new List<T>();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    data = await response.Content.ReadAsStringAsync();
                    list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(data);
                }
            }
            return list;
        }
        public static async Task<bool> Add(T entity, string apiUrl)
        {
            string data = "";
            bool result = false;
            var json = JsonConvert.SerializeObject(entity);
            using (HttpClient client = new HttpClient())
            {
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    data = await response.Content.ReadAsStringAsync();
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(data);
                }
            }
            return result;
        }
        public static async Task<bool> ChangeStatus( string apiUrl)
        {
            string data = "";
            bool result = false;
            //var json = JsonConvert.SerializeObject(Id);
            using (HttpClient client = new HttpClient())
            {
               // var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    data = await response.Content.ReadAsStringAsync();
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(data);
                }
            }
            return result;
        }
        public static async Task<GenericRequestResponse> AddResponseMessage(T entity, string apiUrl)
        {
            string data = "";
            GenericRequestResponse result = new GenericRequestResponse();
            var json = JsonConvert.SerializeObject(entity);
            using (HttpClient client = new HttpClient())
            {
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    data = await response.Content.ReadAsStringAsync();
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<GenericRequestResponse>(data);
                }
            }
            return result;
        }

        public static async Task<bool> Remove(string apiUrl)
        {
            string data;
            bool result = false;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.DeleteAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    data = await response.Content.ReadAsStringAsync();
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(data);
                }
            }
            return result;
        }

        public static async Task<GenericRequestResponse> UpdateResponseMessage(T entity, string apiUrl)
        {
            string data = "";
            GenericRequestResponse result = new GenericRequestResponse();
            var json = JsonConvert.SerializeObject(entity);
            using (HttpClient client = new HttpClient())
            {
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(apiUrl, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    data = await response.Content.ReadAsStringAsync();
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<GenericRequestResponse>(data);
                }
            }
            return result;
        }
        public static async Task<bool> Update(T entity, string apiUrl)
        {
            string data = "";
            bool result = false;
            var json = JsonConvert.SerializeObject(entity);
            using (HttpClient client = new HttpClient())
            {
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(apiUrl, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    data = await response.Content.ReadAsStringAsync();
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(data);
                }
            }
            return result;
        }
        public static async Task<T> GetById(string apiUrl)
        {
            string data = "";
            T record = null;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    data = await response.Content.ReadAsStringAsync();
                    record = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data);
                }
            }
            return record;
        }
        public static async Task<T> GetSingleData(string apiUrl)
        {
            string data = "";
            T record = null;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    data = await response.Content.ReadAsStringAsync();
                    record = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data);
                }
            }
            return record;
        }
        public static async Task<string> GetDropDownList(string apiUrl)
        {
            try
            {
                string data = "";
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        data = await response.Content.ReadAsStringAsync();
                        //record = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static async Task<int> GetCounts(string apiUrl)
        {
            try
            {
                int count = 0;
                string data = "";
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        data = await response.Content.ReadAsStringAsync();
                        //record = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data);
                    }
                }
                count = Convert.ToInt32(data);
                return count;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        
    }
}
