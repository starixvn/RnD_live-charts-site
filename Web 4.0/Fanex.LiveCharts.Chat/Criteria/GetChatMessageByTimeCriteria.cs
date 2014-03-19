// -----------------------------------------------------------------------
// <copyright file="GetChatMessageByTimeCriteria.cs" company="">
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
    public class GetChatMessageByTimeCriteria : CriteriaBase
    {
        public int ChatRoomId { get; set; }

        public DateTime FromTime { get; set; }

        public DateTime ToTime { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Get_Chat_Message_By_Time";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
