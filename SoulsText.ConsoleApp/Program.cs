using SoulsText.ConsoleApp.UserInterfaceManagers;
using SoulsText.ConsoleApp.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace SoulsText.ConsoleApp
{
    internal class Program
    {
        public static InMemoryData Data = new InMemoryData();
        public static HubConnection Connection = CreateConnection.ConfigureConnection().Result;

        static void Main(string[] args)
        {
            
            IUserInterfaceManager ui = new LoginManager();
            while (ui != null)
            {
                //Each call to Execute will return the next IUserInterfaceManager that we should execute.
                //When it returns null, we will exit the program.
                ui = ui.Execute();
            }
        }

 
    }
}
