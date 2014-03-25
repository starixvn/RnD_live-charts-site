// --------------------------------------------------------------------------
// <copyright file="ChatRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.20">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.LiveCharts.Repositories.Impl;
    using Fanex.LiveCharts.Chat.Models;
    using Fanex.LiveCharts.Chat.Criteria;
    using Fanex.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChatRepository : BaseRepository, IChatRepository
    {
        public IEnumerable<ChatMessage> GetChatMessagesByTime(GetChatMessageByTimeCriteria getChatMessageCriteria)
        {
            var chatMessages = new List<ChatMessage>();

            using (var db = new ObjectDb(getChatMessageCriteria.GetSettingKey()))
            {
                var param = getChatMessageCriteria.GetSpParams();

                var list = db.Query<ChatMessage, ChatUser, ChatMessage>(
                    (cm, cu) =>
                    {
                        cm.ChatUser = cu;
                        return cm;
                    },
                    "ChatUserId",
                    param);

                return list;
            }
        }
    }
}
