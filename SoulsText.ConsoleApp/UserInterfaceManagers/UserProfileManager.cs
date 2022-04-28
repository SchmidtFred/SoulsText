using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoulsText.ConsoleApp.UserInterfaceManagers;
using SoulsText.ConsoleApp.Models;

namespace SoulsText.ConsoleApp.UserInterfaceManagers
{
    internal class UserProfileManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private readonly string _apiUrl;
        private readonly UserProfile _userProfile;

        public UserProfileManager(IUserInterfaceManager parentUI, string apiUrl, UserProfile userProfile)
        {
            _parentUI = parentUI;
            _apiUrl = apiUrl;
            _userProfile = userProfile;
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
            Console.WriteLine($"ID - {_userProfile.Id}");
            Console.WriteLine($"UserName - {_userProfile.Name}");
            Console.ReadLine();
        }

        private void ListUsers()
        {
            Console.WriteLine("Not Implemented Yet");
        }
    }
}
