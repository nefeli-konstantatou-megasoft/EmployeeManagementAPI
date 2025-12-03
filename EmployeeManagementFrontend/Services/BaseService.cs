using EmployeeManagementModels;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace EmployeeManagementFrontend.Services
{
    public class BaseService
    {
        public BaseService(HttpClient client)
        {
            this.httpClient = client;
        }

        protected async Task<ResponseModel<T>> SerializeGet<T>(string uri)
        {
            using(var response = await httpClient.GetAsync(uri))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<ResponseModel<T>>(stream, jsonSerializerOptions) ?? new ResponseModel<T>() {
                    Success = false,
                    Message = "Could not deserialize get request response."
                };
                return result;
            }
        }

        protected async Task<ResponseModel<T>> SerializePost<T, U>(string uri, U content)
        {
            using (var response = await httpClient.PostAsJsonAsync<U>(uri, content))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<ResponseModel<T>>(stream, jsonSerializerOptions) ?? new ResponseModel<T>()
                {
                    Success = false,
                    Message = "Could not deserialize post request response."
                };
                return result;
            }    
        }
        protected async Task<ResponseModel<T>> SerializeDelete<T>(string uri)
        {
            using (var response = await httpClient.DeleteAsync(uri))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<ResponseModel<T>>(stream, jsonSerializerOptions) ?? new ResponseModel<T>()
                {
                    Success = false,
                    Message = "Could not deserialize delete request response."
                };
                return result;
            }
        }

        protected async Task<ResponseModel<T>> SerializePut<T, U>(string uri, U content)
        {
            using (var response = await httpClient.PutAsJsonAsync<U>(uri, content))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<ResponseModel<T>>(stream, jsonSerializerOptions) ?? new ResponseModel<T>()
                {
                    Success = false,
                    Message = "Could not deserialize post request response."
                };
                return result;
            }
        }

        protected HttpClient httpClient;
        static readonly JsonSerializerOptions jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
    }
}
