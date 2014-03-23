using Fanex.NAP.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.LiveCharts.Chat.Criteria
{
    public class GetChatUserInRoomByConnectionIdCriteria : CriteriaBase
    {
        public string ConnectionId { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Chat_Get_Chat_User_In_Room_By_ConnectionId";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
