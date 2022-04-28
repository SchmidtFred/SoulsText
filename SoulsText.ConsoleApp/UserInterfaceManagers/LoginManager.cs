using System;
using System.Collections.Generic;
using System.Linq;
using SoulsText.ConsoleApp.Models;

namespace SoulsText.ConsoleApp.UserInterfaceManagers
{
    internal class LoginManager : IUserInterfaceManager
    {
        private string _apiUrl;

        public LoginManager(string apiUrl)
        {
            _apiUrl = apiUrl;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Login/Register");
            Console.WriteLine(" 1) Login");
            Console.WriteLine(" 2) Register");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.WriteLine("Write your user name and press enter");
                    Console.Write("> ");

                    string input = Console.ReadLine();

                    Console.WriteLine($"Login with API not implemented yet. Your data will not be saved.");
                    return new MainMenuManager(new UserProfile()
                    {
                        UserName = input,
                    });
                case "2":
                    Console.WriteLine("Write your chosen user name and press enter");
                    Console.Write("> ");

                    input = Console.ReadLine();

                    Console.WriteLine($"Register with API not implemented yet. Your data will not be saved.");
                    return new MainMenuManager(new UserProfile()
                    {
                        UserName = input,
                    });
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }
    }
}
