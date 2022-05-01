using System;
using SoulsText.ConsoleApp.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace SoulsText.ConsoleApp.UserInterfaceManagers
{
    internal class UserProfileManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private readonly string _apiUrl;
        private readonly HubConnection _connection;
        private readonly InMemoryData _data;
        public IUserInterfaceManager ParentUi { get { return _parentUI; } }

        public UserProfileManager(IUserInterfaceManager parentUI, string apiUrl)
        {
            _parentUI = parentUI;
            _apiUrl = apiUrl;
            _connection = Program.Connection;
            _data = Program.Data;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("User Menu");
            Console.WriteLine(" 1) My Info");
            Console.WriteLine(" 2) List of Users");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    UserInfo();
                    return this;
                case "2":
                    ListUsers();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;

            }
        }

        private void UserInfo()
        {
            Console.WriteLine($"ID - {_data.User.Id}");
            Console.WriteLine($"UserName - {_data.User.UserName}");
        }

        private void ListUsers()
        {
            Console.WriteLine("All Users");
            _data.Users.ForEach(user => Console.WriteLine($" ID: {user.Id} - {user.UserName}"));
        }
    }
}
