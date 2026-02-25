using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   
        public class ChatRequestDto
        {
            public List<ChatMessageDto> History { get; set; } = new List<ChatMessageDto>();
            public string NewMessage { get; set; }
        }

        public class ChatMessageDto
        {
             public string Role { get; set; } // "user" או "model"
            public string Text { get; set; }

         }
}
