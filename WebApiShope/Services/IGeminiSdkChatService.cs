using DTO;

namespace Services
{
    public interface IGeminiSdkChatService
    {
        Task<Resulte<string>> SendMessageAsync(ChatRequestDto request);
    }
}
