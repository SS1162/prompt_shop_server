using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private readonly string _apiKey = "YOUR_API_KEY_HERE";

        public async Task<string> SendMessageAsync(ChatRequestDto request)
        {
            // 1. יצירת החיבור ל-Gemini
            var client = new GoogleAIClient(_apiKey);
            var model = client.GenerativeModel(modelId: "gemini-1.5-flash");

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

            return response.Text;
        }
    }
}
