using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoulsText.ConsoleApp.Models;

namespace SoulsText.ConsoleApp.UserInterfaceManagers
{
    internal class MessageManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private readonly string _apiUrl;
        private readonly UserProfile _userProfile;

        public MessageManager(IUserInterfaceManager parentUI, string apiUrl, UserProfile userProfile)
        {
            _parentUI = parentUI;
            _apiUrl = apiUrl;
            _userProfile = userProfile;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Message Menu");
            Console.WriteLine(" 1) View All Messages");
            Console.WriteLine(" 2) Get Message By Id");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    Choose();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;

            }
        }

        private void List()
        {
            Console.WriteLine("Not Implemented");
        }

        private void Choose()
        {
            Console.WriteLine("Not Implemented");
        }
    }
}
