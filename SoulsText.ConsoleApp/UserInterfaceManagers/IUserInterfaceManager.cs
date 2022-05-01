namespace SoulsText.ConsoleApp.UserInterfaceManagers
{
    public interface IUserInterfaceManager
    {
        public IUserInterfaceManager Execute();
        public IUserInterfaceManager ParentUi { get; }
    }
}