using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using SoulsText.ConsoleApp.Models;

namespace SoulsText.ConsoleApp.UserInterfaceManagers
{
    internal class LoginManager : IUserInterfaceManager
    {
        private readonly InMemoryData _data;
        private readonly HubConnection _connection;
        public IUserInterfaceManager ParentUi { get { return null; } }

        public LoginManager()
        {
            _data = Program.Data;
            _connection = Program.Connection;
        }
        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Login/Register");
            Console.WriteLine(" 1) Login");
            Console.WriteLine(" 2) Register");
            Console.WriteLine(" 0) Exit App");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    return Login().Result;
                case "2":
                    return Register().Result;
                case "0":
                    return null;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private async Task<IUserInterfaceManager> Login()
        {
            Console.WriteLine("Available Users");
            _data.Users.ForEach(user => Console.WriteLine($" ID: {user.Id} - {user.UserName}"));
            Console.WriteLine(" 0 - Return to Login Menu");
            Console.WriteLine("Select user with Id or Return to Login Menu");
            Console.Write("> ");
            string input = Console.ReadLine();
            if (input == "0")
            {
                return this;
            }
            if (int.TryParse(input, out int choice))
            {
                //grab our temporary user
                var tempUser = _data.Users.FirstOrDefault(user => user.Id == choice);
                //make sure it's an actual user
                if (tempUser != null)
                {
                    // Let people know we have logged in
                    await _connection.InvokeAsync("UserLoggedIn", tempUser);
                    //assign user to data
                    _data.User = tempUser;
                    //Return main menu
                    return new MainMenuManager();
                } else
                {
                    //retry if does not exist
                    Console.WriteLine("Invalid Selection. The chosen Id does not exist.");
                    return this;
                }

            } else
            {
                Console.WriteLine("Invalid Selection");
                return this;
            }
        }

        private async Task<IUserInterfaceManager> Register()
        {
            Console.WriteLine("Write your chosen user name then press Enter. Enter 0 or Leave Blank to return to Login Menu.");
            Console.Write("> ");
            string input = Console.ReadLine();
            if (string.IsNullOrEmpty(input) || input == "0")
            {
                return this;
            }
            else
            {
                //create user object
                UserProfile profile = new UserProfile()
                {
                    UserName = input
                };
                //send it to our hub
                await _connection.InvokeAsync("RegisterUser", profile);
                Console.WriteLine("You are now registered. Taking you to main menu.");
                return new MainMenuManager();
            }
        }
    }
}
