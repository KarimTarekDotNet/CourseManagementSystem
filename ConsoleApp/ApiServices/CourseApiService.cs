using ConsoleApp.DTOs;
using Project.Entities.Enums;
using System.Text;
using System.Text.Json;

namespace ConsoleApp.ApiServices
{
    public class CourseApiService
    {
        private readonly HttpClient _httpClient;

        public CourseApiService(string token)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7060/")
            };
            _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public Task<CourseDTO> AddCourse(CourseDTO courseDTO) =>
            SendAndReturnAsync<CourseDTO>(HttpMethod.Post, "api/Courses", courseDTO);

        public Task DeleteCourse(int id) =>
            SendAsync(HttpMethod.Delete, $"api/Courses/{id}");

        public Task RestoreCourse(int id) =>
            SendAsync(HttpMethod.Post, $"api/Courses/{id}/restore");

        public Task UpdateName(int id, string name) =>
            SendAsync(HttpMethod.Put, $"api/Courses/{id}/name", name);

        public Task UpdateCapacity(int id, int capacity) =>
            SendAsync(HttpMethod.Put, $"api/Courses/{id}/capacity", capacity);

        public Task UpdateTotalHours(int id, int totalHours) =>
            SendAsync(HttpMethod.Put, $"api/Courses/{id}/totalHours", totalHours);

        public Task UpdateSessionDuration(int id, int sessionDuration) =>
            SendAsync(HttpMethod.Put, $"api/Courses/{id}/sessionDuration", sessionDuration);

        public Task UpdateLevel(int id, CourseLevel level) =>
            SendAsync(HttpMethod.Put, $"api/Courses/{id}/level", level);

        public Task UpdateDescription(int id, string? description) =>
            SendAsync(
                HttpMethod.Put,
                $"api/Courses/{id}/description",
                string.IsNullOrWhiteSpace(description) ? null : description
            );


        public Task<List<CourseDTO>> GetAllCourses() =>
            GetAsync<List<CourseDTO>>("api/Courses");

        public Task<List<CourseDTO>> GetCourseByName(string name) =>
            GetAsync<List<CourseDTO>>($"api/Courses/byname/{name}");

        public Task<List<CourseDTO>> GetCoursesWithoutInstructor() =>
            GetAsync<List<CourseDTO>>("api/Courses/WithoutInstructor");

        public Task<List<CourseDTO>> GetCoursesWithMinStudents(int count) =>
            GetAsync<List<CourseDTO>>($"api/Courses/WithMinStudents/{count}");

        public Task<List<CourseDTO>> GetCoursesWithLevel(CourseLevel level) =>
            GetAsync<List<CourseDTO>>($"api/Courses/WithLevel/{level}");


        private static readonly JsonSerializerOptions _jsonOptions =
            new() { PropertyNameCaseInsensitive = true };

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
            response.EnsureSuccessStatusCode();
        }

        private async Task<T> SendAndReturnAsync<T>(HttpMethod method, string url, object body)
        {
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(method, url)
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseJson, _jsonOptions)!;
        }
    }
}
