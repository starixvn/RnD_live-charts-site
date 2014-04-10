

namespace Fanex.LiveCharts.Chat.Criteria
{
    using Fanex.NAP.Framework.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    public class SearchChatMessageCriteria : CriteriaBase
    {
        public int ChatRoomId { get; set; }

        public int ChatUserId { get; set; }

        public string SearchText { get; set; }

        public bool IncludingDeleted { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Search_Chat_Message";
        }

        public override bool IsValid()
        {
            return ChatRoomId > 0;
        }
    }
}
