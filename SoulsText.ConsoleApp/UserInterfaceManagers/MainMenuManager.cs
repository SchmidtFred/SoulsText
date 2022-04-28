using System;
using SoulsText.ConsoleApp.UserInterfaceManagers;
using SoulsText.ConsoleApp.Models;

namespace SoulsText.ConsoleApp.UserInterfaceManagers
{
    public class MainMenuManager : IUserInterfaceManager
    {
        private const string API_URL = @"https://localhost:5001/api";
        private UserProfile _user;

        public MainMenuManager(UserProfile user = null)
        {
            _user = user;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Welcome to the Souls Text Console App.");

            if (_user != null)
            {
                Console.WriteLine($"{_user.UserName} - Main Menu");

                Console.WriteLine(" 1) User Details");
                Console.WriteLine(" 2) Messages");
                Console.WriteLine(" 0) Exit");

                Console.Write("> ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        return new UserProfileManager(this, API_URL, _user);
                    case "2":
                        Console.WriteLine("Not implemented");
                        return this;
                    case "0":
                        Console.WriteLine("Now Exiting App");
                        return null;
                    default:
                        Console.WriteLine("Invalid Selection");
                        return this;
                }
            }
            else
            {
                return new LoginManager(API_URL);
            }
        }
    }
}