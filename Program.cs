using System;

namespace AIMLTGBot
{
    class Program
    {
        static void Main(string[] args)
        {
            StudentNetwork Network = StudentNetwork.ReadFromFile("НАЗВАНИЕ ФАЙЛА");
            var token = System.IO.File.ReadAllText("TGToken.txt");
            using (var tg = new TelegramService(token, new AIMLService(),Network))
            {
                Console.WriteLine($"Подключились к телеграмму как бот { tg.Username }. Ожидаем сообщений. Для завершения работы нажимте Enter");
                Console.ReadLine();
            }
        }
    }
}
