using DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{

    public class ChatBotServise : IChatBotServise
    {
        private readonly IGeminiSdkChatService _geminiSdkChatService;

        public ChatBotServise(IGeminiSdkChatService geminiSdkChatService)
        {
            _geminiSdkChatService = geminiSdkChatService;
        }



        public async Task<Resulte<string>> SendMessage([FromBody] ChatRequestDto request)
        {
            if (request == null)
            {
                return Resulte<string>.Failure("Request body is required");
            }

            if (string.IsNullOrWhiteSpace(request.NewMessage))
            {
                return Resulte<string>.Failure("Request body is required");
            }
            return await _geminiSdkChatService.SendMessageAsync(request);

        }


    }
}
