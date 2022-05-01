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
        private readonly int _messageId;
        public IUserInterfaceManager ParentUi { get { return _parentUI; } }

        public MessageDetailManager(IUserInterfaceManager parentUI, int messageId)
        {
            _parentUI = parentUI;
            _connection = Program.Connection;
            _data = Program.Data;
            _messageId = messageId;
        }

        public IUserInterfaceManager Execute()
        {
            return Run().Result;
        }

        private async Task<IUserInterfaceManager> Run()
        {
            Message message = _data.Messages.FirstOrDefault(m => m.Id == _messageId);
            Console.WriteLine("Message Details");
            Console.WriteLine($@"Content: {message.Content}
Placed By {message.UserProfile.UserName}
X: {message.X}
Y: {message.Y}
Z: {message.Z}
VoteCount: {message.VoteCount}");
            Console.WriteLine(" 1) Upvote");
            Console.WriteLine(" 2) Downvote");
            Console.WriteLine(" 0) Go Back");
            Console.Write("> ");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    //create vote object
                    Vote vote = new Vote()
                    {
                        Upvote = true,
                        MessageId = message.Id,
                        UserProfileId = _data.User.Id
                    };
                    //send it off
                    await _connection.InvokeAsync("SendVote", vote);
                    Console.WriteLine("Upvote Sent");
                    return this;
                case "2":
                    vote = new Vote()
                    {
                        Upvote = false,
                        MessageId = message.Id,
                        UserProfileId = _data.User.Id
                    };
                    await _connection.InvokeAsync("SendVote", vote);
                    Console.WriteLine("Downvote Sent");
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
