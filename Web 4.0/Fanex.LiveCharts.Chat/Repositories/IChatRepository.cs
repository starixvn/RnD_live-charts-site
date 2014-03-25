// --------------------------------------------------------------------------
// <copyright file="IChatRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.20">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Repositories
{
    using Fanex.LiveCharts.Repositories.Interface;
    using System.Collections.Generic;
    using Fanex.LiveCharts.Chat.Criteria;
    using Fanex.LiveCharts.Chat.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IChatRepository : IBaseRepository
    {
        IEnumerable<ChatMessage> GetChatMessagesByTime(GetChatMessageByTimeCriteria getChatMessageCriteria);
    }
}