using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MVC_ASP.NET_Core_Learn.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<LoginResponse> Login(string username, string password)
        {
            // Создаем объект запроса
            var requestData = new { Username = username, Password = password };
            var jsonRequest = JsonSerializer.Serialize(requestData);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Отправляем POST запрос и получаем ответ
            var response = await _httpClient.PostAsync("api/User/Login", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            // Проверяем успешность запроса
            if (response.IsSuccessStatusCode)
            {
                // Получаем время истечения токена из тела ответа
                var responseObject = JsonSerializer.Deserialize<LoginResponse>(responseBody);

                if (responseObject != null)
                {
                    return new LoginResponse { Token = responseObject.Token, Expires = responseObject.Expires };
                }

                // Если не удалось получить время истечения из тела ответа, возвращаем пустой токен и 0 часов
                return new LoginResponse();
            }
            else
            {
                // Если не успешно, генерируем исключение
                throw new InvalidOperationException(responseBody);
            }
        }
    }

    public class LoginResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("expires")]
        public int Expires { get; set; }
    }

    public class ApiResponse
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }

        private ApiResponse(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static ApiResponse Success(string message)
        {
            return new ApiResponse(true, message);
        }

        public static ApiResponse Error(string message)
        {
            return new ApiResponse(false, message);
        }
    }
}
