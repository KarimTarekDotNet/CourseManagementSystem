using ConsoleApp.DTOs;
using Project.Entities;
using System.Text;
using System.Text.Json;

namespace ConsoleApp.ApiServices
{
    public class InstructorApiService
    {
        private readonly HttpClient _httpClient;

        public InstructorApiService(string token)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7060/")
            };
            _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
        public Task Create(InstructorDTO instructor) =>
            SendAsync(HttpMethod.Post, "api/instructor", instructor);

        public Task Delete(int id) =>
            SendAsync(HttpMethod.Delete, $"api/instructor/{id}");

        public Task Restore(int id) =>
            SendAsync(HttpMethod.Post, $"api/instructor/{id}/restore");

        public Task UpdateFirstName(int id, string firstName) =>
            SendAsync(HttpMethod.Put, $"api/instructor/{id}/firstname", firstName);

        public Task UpdateLastName(int id, string lastName) =>
            SendAsync(HttpMethod.Put, $"api/instructor/{id}/lastname", lastName);

        public Task UpdateLastEmail(int id, string email) =>
            SendAsync(HttpMethod.Put, $"api/instructor/{id}/email", email);

        public Task UpdatePhone(int id, string phone) =>
            SendAsync(HttpMethod.Put, $"api/instructor/{id}/phone", phone);

        public Task UpdateDepartment(int id, string? department) =>
            SendAsync(
                HttpMethod.Put,
                $"api/instructor/{id}/department"
            );

        public Task AssignCourse(int id, int courseId) => SendAsync(HttpMethod.Post, $"api/instructor/{id}/assign-course/{courseId}");

        public Task DropCourse(int id, int courseId) => SendAsync(HttpMethod.Delete, $"api/instructor/{id}/remove-course/{courseId}");

        public async Task<List<InstructorDTO>> GetByDepartment(string? department)
        {
            if (string.IsNullOrWhiteSpace(department))
                department = null;

            return await GetAsync<List<InstructorDTO>>(
                $"api/instructor/department"
            );
        }

        public async Task<List<InstructorDTO>> GetWithCourses()
        {
            return await GetAsync<List<InstructorDTO>>(
                "api/instructor/with-courses"
            );
        }

        public async Task<List<InstructorDTO>> GetWithoutCourses()
        {
            return await GetAsync<List<InstructorDTO>>(
                "api/instructor/without-courses"
            );
        }


        private static readonly JsonSerializerOptions _jsonOptions =
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true };


        private async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, _jsonOptions)!;
        }

        private async Task SendAsync(HttpMethod method, string url, object? body = null)
        {
            HttpContent? content = null;

            if (body != null)
            {
                var json = JsonSerializer.Serialize(body);
                content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var request = new HttpRequestMessage(method, url)
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
        }
    }
}
