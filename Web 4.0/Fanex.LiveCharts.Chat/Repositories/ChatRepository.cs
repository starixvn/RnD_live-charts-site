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
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.LiveCharts.Chat.Criteria;
    using Fanex.LiveCharts.Chat.Models;
    using Fanex.LiveCharts.Repositories.Impl;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChatRepository : BaseRepository, IChatRepository
    {
        /// <summary>
        /// Get history message.
        /// </summary>
        /// <param name="criteria">Criteria for get data.</param>
        /// <returns>List message.</returns>
        public IEnumerable<ChatMessage> GetChatMessages(ICriteria criteria)
        {
            var chatMessages = new List<ChatMessage>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();

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