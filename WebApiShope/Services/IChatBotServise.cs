using DTO;
using Microsoft.AspNetCore.Mvc;

namespace Services
{
    public interface IChatBotServise
    {
        Task<Resulte<string>> SendMessage([FromBody] ChatRequestDto request);
    }
}