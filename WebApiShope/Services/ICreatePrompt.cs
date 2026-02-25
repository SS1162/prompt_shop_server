
namespace Services
{
    public interface ICreatePrompt
    {
        Task<string> Prompt(long orderId);
    }
}