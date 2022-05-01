using Microsoft.AspNetCore.SignalR.Client;
using SoulsText.ConsoleApp.Models;
using SoulsText.ConsoleApp.UserInterfaceManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoulsText.ConsoleApp
{
    internal static class CreateConnection
    {
        public static async Task<HubConnection> ConfigureConnection()
        {
            var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/SoulsHub")
            .Build();

            //Updates Data.Users with a List of all users.
            connection.On<List<UserProfile>>("ReceiveAllUsers", profiles => Program.Data.Users = profiles);

            //Updates Data.Users with a List of all messages.
            connection.On<List<Message>>("ReceiveAllMessages", messages => Program.Data.Messages = messages);

            //Add new user to Data and write a notification that someone else has connected
            connection.On<UserProfile>("NewUser", profile =>
            {
                if (Program.Data.Users.Any(p => p.Id == profile.Id))
                {
                    Console.WriteLine($"{profile.UserName} has logged on.");
                    Console.Write("> ");
                }
                else
                {
                    Program.Data.Users.Add(profile);
                    Console.WriteLine($"New User {profile.UserName} has registered with SoulsText.");
                    Console.Write("> ");
                }
            });

            //Add newly registered user to our data when registerd from this connection.
            connection.On<UserProfile>("UserRegistered", profile => Program.Data.User = profile);

            //Add update data to include new message. Rerender MessageManager if it is active
            connection.On<Message>("ReceiveNewMessage", message =>
            {
                //update data
                Program.Data.Messages.Add(message);
                //only show notification if it is not your message
                if (message.UserProfileId != Program.Data.User.Id)
                {
                    Console.WriteLine($"New Message Placed - {message.Content}");
                }
                Console.Write("> ");
            });

            //update appropriate message with votes
            connection.On<Message>("ReceiveUpdatedMessage", message =>
            {
                var index = Program.Data.Messages.FindIndex(m => m.Id == message.Id);
                Program.Data.Messages[index] = message;
            });


            //start connection
            await connection.StartAsync();

            return connection;
        }
    }
}
