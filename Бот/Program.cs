using System;

namespace AIMLTGBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var studentNetwork = new StudentNetwork(@"student-network.txt");
            var token = System.IO.File.ReadAllText("TGToken.txt");
            using (var tg = new TelegramService(token, new AIMLService(), studentNetwork))
            {
                Console.WriteLine($"Подключились к телеграмму как бот { tg.Username }. Ожидаем сообщений. Для завершения работы нажимте Enter");
                Console.ReadLine();
            }
        }
    }
}
