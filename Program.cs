using System;

namespace AIMLTGBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var token = System.IO.File.ReadAllText("TGToken.txt");
            using (var tg = new TelegramService(token, new AIMLService()))
            {
                Console.WriteLine($"Подключились к телеграмму как бот { tg.Username }. Ожидаем сообщений. Для завершения работы нажимте Enter");
                Console.ReadLine();
            }
        }
    }
}
