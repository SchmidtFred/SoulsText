using System;
using SoulsText.ConsoleApp.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace SoulsText.ConsoleApp.UserInterfaceManagers
{
    internal class WriteMessageManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private readonly HubConnection _connection;
        private readonly InMemoryData _data;
        public IUserInterfaceManager ParentUi { get { return _parentUI; } }

        public WriteMessageManager(IUserInterfaceManager parentUI)
        {
            _parentUI = parentUI;
            _connection = Program.Connection;
            _data = Program.Data;
        }

        public IUserInterfaceManager Execute()
        {
            //using this way to allow
            return Run().Result;
        }

        private async Task<IUserInterfaceManager> Run()
        {
            Console.WriteLine("Write Message. Leave blank or input 0 to go back.");
            Console.Write("> ");
            string input = Console.ReadLine();
            if (string.IsNullOrEmpty(input) || input == "0")
            {
                return _parentUI;
            }
            Message message = new Message()
            {
                Content = input,
                UserProfileId = _data.User.Id
            };
            //now send new message to socket
            await _connection.InvokeAsync("SendMessage", message);

            return _parentUI;
        }
    }
}
