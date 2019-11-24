using System;
using System.Collections.Generic;
using Server.Receiver.TCP;
using System.Threading;
using System.Net;

namespace Server.Domain
{
    public class Domain
    {
        private string buffer { get; set; }

        private string Answer { get; set; }

        public string Command = "null";

        List<string> Users = new List<string>();

        Server.Receiver.TCP.Receiver Receiver = new Receiver.TCP.Receiver();

        public Domain(Receiver.TCP.Receiver reciver)
        {
            Receiver.OnMessageHandler += OnMessage;
        }

        public void Run()
        {
            new Thread(ThreadStart =>
            {
                Receiver.Start();
            }).Start();

            while (true)
            {
                try
                {
                    Terminal();
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Не верно описанная команда");
                    Console.ResetColor();
                }
            }
        }

        private string OnMessage(string message)
        {
            //Console.WriteLine(message);
            if (IsNewUser(message))
                Users.Add(message);

            var arr = Command.Split("~~~");
            if (message.IndexOf("ANSWER:") == 0)
            {
                Console.WriteLine("ANSWER: " + message.ToString().Substring(7));
            }
            else if (arr.Length == 2 && Equals(arr[0] , message))
            {
                Command = "null";
                return arr[1];
            }

            return "Ok";
        }

        private bool IsNewUser(string message)
        {
            if (IPAddress.TryParse(message, out IPAddress iPAddress) == true)
            {
                foreach (var item in Users)
                {
                    if (item == message)
                        return false;
                }
                return true;
            }
            else
                return false;
        }

        private void Terminal()
        {
            string command;
            while(true)
            {
                Console.Write(@"Sender\");
                command = Console.ReadLine();

                if (Equals(command.ToLower(), "bufferdel"))
                {
                    buffer = null;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Буфер удален.");
                    Console.ResetColor();
                }
                else if (Equals(command.ToLower(), "help"))
                {
                    Console.WriteLine($"users - Список пользователей\ncommand send {{ip}} {{command}} - Отправить запрос\nhelp - Помощь\n" +
                        $"buffer {{text}} - Текст будет воодиться перед командой отправки сообщения\n" +
                        $"bufferdel - Удалить буфер\n" +
                        $"buffer - Узнать буфер" +
                        $"bufferadd - добавить отрезок к существующему");
                }
                else if (command == "buffer")
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(buffer);
                    Console.ResetColor();
                }
                else if (command.ToLower().IndexOf("bufferadd") == 0)
                {
                    try
                    {
                        var i = command.Substring(10);
                        buffer += i;
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Добавлено.");
                        Console.ResetColor();
                    }
                    catch
                    {

                    }
                }
                else if (command.IndexOf("buffer") == 0)
                {
                    buffer = command.Substring(7);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Буфер установлен.");
                    Console.ResetColor();
                }
                else if(Equals(command.ToLower(), "users"))
                {
                    if (Users.Count == 0)
                    {
                        Console.ForegroundColor= ConsoleColor.Red;
                        Console.WriteLine("No users");
                        Console.ResetColor();
                    }
                    else
                    {
                        for(int i =0; i< Users.Count; i++)                        
                            Console.WriteLine($"{i+1}: {Users[i]}");
                    }
                }
                else if (buffer != null && (buffer+command).ToLower().IndexOf("command send") == 0)
                {
                    string[] arr;
                    if ((arr = (buffer+ command).Substring(13).ToString().ToLower().Split(' ')).Length >= 2)
                    {
                        Command = arr[0] + "~~~";
                        for (int i = 1; i < arr.Length; i++)
                        {
                             Command += arr[i]+ " ";
                        }
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Ожидайте...");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine("Формат: command send {ip} {command}");
                    }
                }
                else if (command.ToLower().IndexOf("command send") == 0)
                {
                    string[] arr;
                    if ((arr = command.Substring(13).ToString().ToLower().Split(' ')).Length == 2)
                    {
                        Command = arr[0] + "~~~" + arr[1];
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Ожидайте...");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine("Формат: command send {ip} {command}");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Данной команды не существует.");
                    Console.ResetColor();
                }
            }
        }
    }
}
