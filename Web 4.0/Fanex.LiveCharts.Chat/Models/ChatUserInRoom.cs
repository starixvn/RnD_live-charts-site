using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.LiveCharts.Chat.Models
{
    public class ChatUserInRoom
    {
        public int ChatUserInRoomId { get; set; }
        public int ChatUserId { get; set; }
        public int ChatRoomId { get; set; }
        public DateTime LastOnlineTime { get; set; }
        public bool IsLeft { get; set; }
        public string ConnectionId { get; set; }
        public bool KickedOut { get; set; }

        public string ChatRoomName { get; set; }

        public string ChatName { get; set; }
    }
}
