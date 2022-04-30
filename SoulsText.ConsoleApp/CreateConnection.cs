using Microsoft.AspNetCore.SignalR.Client;
using SoulsText.ConsoleApp.Models;
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
                if (Program.Data.Users.Contains(profile))
                {
                    Console.WriteLine($"{profile.UserName} has logged on.");
                }
                else
                {
                    Program.Data.Users.Add(profile);
                    Console.WriteLine($"{profile.UserName} ");
                }
            });

            //Add newly registered user to our data when registerd from this connection.
            connection.On<UserProfile>("UserRegistered", profile => Program.Data.User = profile);


            //start connection
            await connection.StartAsync();

            return connection;
        }
    }
}
