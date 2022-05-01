using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using SoulsText.ConsoleApp.Models;

namespace SoulsText.ConsoleApp.UserInterfaceManagers
{
    internal class MessageManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUi;
        private readonly HubConnection _connection;
        private readonly InMemoryData _data;
        public IUserInterfaceManager ParentUi { get { return _parentUi; } }

        public MessageManager(IUserInterfaceManager parentUI)
        {
            _parentUi = parentUI;
            _connection = Program.Connection;
            _data = Program.Data;
        }

        public IUserInterfaceManager Execute()
        {
            Console.Clear();
            Console.WriteLine("Messages");
            _data.Messages.ForEach(message => Console.WriteLine($" ID: {message.Id} - {message.Content}"));
            Console.WriteLine(" Enter Id Details. 0 or Empty Selection will take you back.");

            Console.Write("> ");
            string choice = Console.ReadLine();
            if (choice == "0" || string.IsNullOrEmpty(choice))
            {
                return _parentUi;
            }
            if (int.TryParse(choice, out int id))
            {
                var chosenMessage = _data.Messages.FirstOrDefault(message => message.Id == id);
                if (chosenMessage == null)
                {
                    Console.WriteLine("Message does not exist");
                    return _parentUi;
                }
                return new MessageDetailManager(this, chosenMessage);
            }
            else
            {
                //go back to main menu to retry
                Console.WriteLine("Invalid Option");
                return _parentUi;
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
