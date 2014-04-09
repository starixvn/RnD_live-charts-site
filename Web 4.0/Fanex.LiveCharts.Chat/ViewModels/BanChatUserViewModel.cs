

namespace Fanex.LiveCharts.Chat.ViewModels
{
    using Fanex.NAP.Framework.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    public class BanChatUserViewModel
    {
        public int ChatUserId { get; set; }

        public string TokenName { get; set; }

        public string TokenKey { get; set; }

        public ICommandResult CommandResult { get; set; }
    }
}
