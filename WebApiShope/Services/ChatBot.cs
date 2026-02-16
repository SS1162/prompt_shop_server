using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Services
{
    using DTO;
    using Entities;
    using Google.GenAI.Types;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GeminiSdkChatService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<GeminiSdkChatService> _logger;
        private readonly string _apiKey;

        public GeminiSdkChatService(IConfiguration config, ILogger<GeminiSdkChatService> logger)
        {
            _config = config;
            _logger = logger;
            _apiKey = _config.GetValue<string>("GEMINI_API_KEY");
        }

        public async Task<Resulte<string>> SendMessageAsync(ChatRequestDto request)
        {
            try
            {
                // 1. יצירת החיבור ל-Gemini
                var client = new GoogleAIClient(_apiKey);
                var model = client.GenerativeModel(modelId: "gemini-3-flash-preview");

                // 2. המרת היסטוריית השיחה לאובייקטים מסוג Content של ה-SDK
                var chatHistory = request.History.Select(msg => new Content
                {
                    Role = msg.Role,
                    Parts = new List<IPart> { new TextPart { Text = msg.Text } }
                }).ToList();

                // 3. הגדרת הוראות המערכת (System Instruction)
                var systemInstruction = new Content
                {
                    Role = "user",
                    Parts = new List<IPart> { new TextPart { Text = "You are a helpful AI assistant. Provide clear, concise, and accurate responses." } }
                };

                // 4. אתחול אובייקט Chat עם ההיסטוריה וההוראות
                var chatSession = model.StartChat(chatHistory);

                // 5. שליחת ההודעה החדשה וקבלת התשובה
                var response = await chatSession.SendMessage(request.NewMessage);

                // 6. בדיקה שהתשובה לא null
                if (response?.Text == null)
                {
                    _logger.LogWarning("Gemini API returned null response. Message: {Message}, History Count: {HistoryCount}", 
                        request.NewMessage, request.History?.Count ?? 0);
                    return Resulte<string>.Failure("Server error");
                }

                return Resulte<string>.Success(response.Text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending message to Gemini API. Message: {Message}, History Count: {HistoryCount}", 
                    request.NewMessage, request.History?.Count ?? 0);
                return Resulte<string>.Failure("Server error");
            }
        }
    }
}
