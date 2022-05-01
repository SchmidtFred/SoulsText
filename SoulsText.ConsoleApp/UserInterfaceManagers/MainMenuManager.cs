using System;
using SoulsText.ConsoleApp.UserInterfaceManagers;
using SoulsText.ConsoleApp.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace SoulsText.ConsoleApp.UserInterfaceManagers
{
    public class MainMenuManager : IUserInterfaceManager
    {
        private const string API_URL = @"https://localhost:5001/api";
        private readonly InMemoryData _data;
        private readonly HubConnection _connection;
        public IUserInterfaceManager ParentUi { get { return null; } }

        public MainMenuManager()
        {
            _data = Program.Data;
            _connection = Program.Connection;
        }

        public IUserInterfaceManager Execute()
        {
            if (_data.User != null)
            { 
                Console.WriteLine($"Welcome to the Souls Text Console App.");

                Console.WriteLine($"{_data.User.UserName} - Main Menu");

                Console.WriteLine(" 1) User Details");
                Console.WriteLine(" 2) Messages");
                Console.WriteLine(" 0) Exit");

                Console.Write("> ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        return new UserProfileManager(this, API_URL);
                    case "2":
                        return new MessageManager(this);
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
                Console.WriteLine("Something went wrong, please login again.");
                return new LoginManager();
            }
        }
    }
}