// -----------------------------------------------------------------------
// <copyright file="GetChatRoomUsersCriteria.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.LiveCharts.Chat.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetChatRoomUsersCriteria : CriteriaBase
    {
        public int ChatRoomId { get; set; }

        public bool IncludingBanned { get; set; }

        public bool KickedOut { get; set; }

        public bool IsLeft { get; set; }
        public override string GetSettingKey()
        {
            return "Chart_Chat_Get_Chat_Room_Users";
        }

        public override bool IsValid()
        {
            return ChatRoomId > 0;
        }
    }
}
