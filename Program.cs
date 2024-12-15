// користувач
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

class FileClient
{
    static void Main()
    {
        try
        {
            Console.Write("Введіть IP-адресу сервера: ");
            string serverIp = Console.ReadLine();

            Console.Write("Введіть ім'я файлу для завантаження: ");
            string fileName = Console.ReadLine();

            TcpClient client = new TcpClient(serverIp, 6000);
            NetworkStream stream = client.GetStream();

            // Надсилаємо ім'я файлу на сервер
            byte[] fileNameData = Encoding.UTF8.GetBytes(fileName);
            stream.Write(fileNameData, 0, fileNameData.Length);

            // Отримуємо відповідь від сервера
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buffer = new byte[1024];
                int bytesRead;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, bytesRead);
                }

                byte[] fileData = ms.ToArray();
                string response = Encoding.UTF8.GetString(fileData);

                if (response.StartsWith("Файл не знайдено"))
                {
                    Console.WriteLine("Файл не знайдено на сервері.");
                }
                else
                {
                    File.WriteAllBytes(fileName, fileData);
                    Console.WriteLine($"Файл {fileName} успішно збережено.");
                }
            }

            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }
}
