using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Emulator
{
    static async Task Main(string[] args)
    {
        HttpClient client = new HttpClient();
        Random rnd = new Random();

        // Адрес вашего сервера
        string baseUrl = "http://localhost:8080/api/sensors";

        while (true)
        {
            // Генерация случайных значений
            double pressure = rnd.NextDouble() * (10 - 1) + 1; // Давление: 1-10 бар
            double flow = rnd.NextDouble() * (100 - 0); // Расход воды: 0-100 л/мин
            int waterLevel = rnd.Next(0, 101); // Уровень воды: 0-100%

            // Данные для отправки
            var sensorData = new
            {
                Pressure = Math.Round(pressure, 2), // Округление до 2 знаков
                Flow = Math.Round(flow, 2),         // Округление до 2 знаков
                WaterLevel = waterLevel             // Целое число, не требует округления
            };


            // Сериализация данных в JSON
            string jsonData = JsonConvert.SerializeObject(sensorData);

            // Подготовка запроса
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                // Отправка данных на сервер
                HttpResponseMessage response = await client.PostAsync(baseUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Data sent: Pressure={pressure}, Flow={flow}, WaterLevel={waterLevel}");
                }
                else
                {
                    Console.WriteLine($"Error: Server returned {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            // Задержка перед следующей отправкой
            await Task.Delay(2000);
        }
    }
}