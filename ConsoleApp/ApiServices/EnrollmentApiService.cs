using ConsoleApp.DTOs;
using Project.Entities.Enums;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ConsoleApp.ApiServices
{
    public class EnrollmentApiService
    {
        private readonly HttpClient httpClient;

        public EnrollmentApiService(string token)
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7060/")
            };
            httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public async Task Create(int studentId, int courseId, DateTime startDate, EnrollmentStatus status)
        {
            var requestBody = new
            {
                StudentId = studentId,
                CourseId = courseId,
                StartEnrollmentDate = startDate,
                Status = status
            };

            await sendAsync(HttpMethod.Post, "api/enrollment", requestBody);
        }

        public async Task Drop(int studentId, int courseId) 
            => await sendAsync(HttpMethod.Delete, $"api/enrollment/{studentId}/{courseId}");

        public async Task Restore(int studentId, int courseId)
        {
            var body = new
            {
                StudentId = studentId,
                CourseId = courseId,
                Status = EnrollmentStatus.ACTIVE
            };

            await sendAsync(HttpMethod.Put, "api/enrollment/restore", body);
        }

        public async Task<List<EnrollmentDTO>> GetAll() => await GetAsync<List<EnrollmentDTO>>("api/enrollment");

        public async Task<List<EnrollmentDTO>> GetStudentEnrollments(int studentId)
            => await GetAsync<List<EnrollmentDTO>>($"api/enrollment/{studentId}/student");

        public async Task<List<EnrollmentDTO>> GetCourseEnrollments(int courseId)
            => await GetAsync<List<EnrollmentDTO>>($"api/enrollment/{courseId}/course");

        public async Task<EnrollmentDTO> CheckEntrollment(int studentId, int courseId)
            => await GetAsync<EnrollmentDTO>($"api/enrollment/{studentId}/{courseId}/check");

        public async Task<List<EnrollmentDTO>> GetStatus(EnrollmentStatus status)
            => await GetAsync<List<EnrollmentDTO>>($"api/enrollment/status/{status}");

        private static readonly JsonSerializerOptions _jsonOptions =
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        private async Task<T> GetAsync<T>(string url)
        {
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(json, _jsonOptions) ?? throw new ArgumentNullException();
        }
        private async Task sendAsync(HttpMethod httpMethod, string url, object? body = null)
        {
            HttpContent? content = null;

            if(body != null)
            {
                var json = JsonSerializer.Serialize(body);
                content = new StringContent(json, Encoding.UTF8, "application/json");
            }
            var request = new HttpRequestMessage(httpMethod, url)
            {
                Content = content
            };
            var response = await httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
        }
    }
}