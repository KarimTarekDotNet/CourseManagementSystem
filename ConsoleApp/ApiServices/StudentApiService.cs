using ConsoleApp.DTOs;
using System.Text;
using System.Text.Json;

namespace ConsoleApp.ApiServices
{
    public class StudentApiService
    {
        private readonly HttpClient _httpClient;

        public StudentApiService(string token)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7060/")
            };
            _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public Task AddStudent(StudentDTO studentDTO) =>
            SendAsync(HttpMethod.Post, "api/students", studentDTO);

        public Task DeleteStudent(int id) =>
            SendAsync(HttpMethod.Delete, $"api/students/{id}");

        public Task RestoreStudent(int id) =>
            SendAsync(HttpMethod.Post, $"api/students/{id}/restore");

        public Task UpdateStudentFirstName(int id, string fName) =>
            SendAsync(HttpMethod.Put, $"api/students/{id}/firstname", fName);

        public Task UpdateStudentLastName(int id, string lName) =>
            SendAsync(HttpMethod.Put, $"api/students/{id}/lastname", lName);

        public Task UpdateStudentEmail(int id, string email) =>
            SendAsync(HttpMethod.Put, $"api/students/{id}/email", email);

        public Task UpdateStudentPhoneNumber(int id, string phone) =>
            SendAsync(HttpMethod.Put, $"api/students/{id}/phone", phone);

        public Task UpdateStudentCollege(int id, string? college) =>
            SendAsync(
                HttpMethod.Put,
                $"api/students/{id}/college",
                string.IsNullOrWhiteSpace(college) ? null : college
            );


        public Task<List<StudentDTO>> GetAllStudents() =>
            GetAsync<List<StudentDTO>>("api/students");

        public Task<List<StudentDTO>> GetStudentsByName(string firstName, string lastName) =>
            GetAsync<List<StudentDTO>>(
                $"api/students/{firstName}/{lastName}/name"
            );

        public async Task<StudentDTO?> GetStudentByEmail(string email)
        {
            return await GetOrDefaultAsync<StudentDTO>(
                $"api/students/{email}/email"
            );
        }

        public async Task<StudentDTO?> GetStudentByPhone(string phone)
        {
            return await GetOrDefaultAsync<StudentDTO>(
                $"api/students/{phone}/phone"
            );
        }

        public Task<List<StudentDTO>> GetStudentsByCollege(string? college) => GetAsync<List<StudentDTO>>("api/students/college");


        private static readonly JsonSerializerOptions _jsonOptions =
            new() { PropertyNameCaseInsensitive = true };

        private async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, _jsonOptions)!;
        }

        private async Task<T?> GetOrDefaultAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return default;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, _jsonOptions);
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
            response.EnsureSuccessStatusCode();
        }
    }
}
