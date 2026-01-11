using ConsoleApp.ConsoleHelper;
using ProjectApi.DTOs;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleApp.ApiServices
{
    public class LoginApiService
    {
        private readonly HttpClient httpClient;
        public string? Token { get; private set; }

        public LoginApiService()
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7060/")
            };
        }


        public async Task<bool> Login(AdminDTO adminDTO)
        {
            try
            {
                var json = JsonSerializer.Serialize(adminDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("api/Admin/login", content);

                if(response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var jsonDoc = JsonDocument.Parse(responseContent);
                    Token = jsonDoc.RootElement.GetProperty("token").GetString();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error during login: {ex.Message}", ex);
            }
        }
    }
}
