using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using SoulsText.ConsoleApp.Models;

namespace SoulsText.ConsoleApp.UserInterfaceManagers
{
    internal class MessageDetailManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private readonly HubConnection _connection;
        private readonly InMemoryData _data;
        private readonly Message _message;
        public IUserInterfaceManager ParentUi { get { return _parentUI; } }

        public MessageDetailManager(IUserInterfaceManager parentUI, Message message)
        {
            _parentUI = parentUI;
            _connection = Program.Connection;
            _data = Program.Data;
            _message = message;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Message Details");
            Console.WriteLine($@"Content: {_message.Content}
Placed By {_message.UserProfile.UserName}
X: {_message.X}
Y: {_message.Y}
Z: {_message.Z}
VoteCount: {_message.VoteCount}");
            Console.WriteLine(" 1) Upvote");
            Console.WriteLine(" 2) Downvote");
            Console.WriteLine(" 0) Go Back");
            Console.Write("> ");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    //do an upvote
                    return this;
                case "2":
                    //do a downvote
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Option");
                    return this;
            }
        }
    }
}
