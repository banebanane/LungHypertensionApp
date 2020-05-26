namespace LungHypertensionApp.Services
{
    public interface IMailService
    {
        void SendMessage(string from, string to, string message);
    }
}