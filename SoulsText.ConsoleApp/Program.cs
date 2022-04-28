using SoulsText.ConsoleApp.UserInterfaceManagers;

namespace SoulsText.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IUserInterfaceManager ui = new MainMenuManager();
            while (ui != null)
            {
                //Each call to Execute will return the next IUserInterfaceManager that we should execute.
                //When it returns null, we will exit the program.
                ui = ui.Execute();
            }
        }
    }
}
