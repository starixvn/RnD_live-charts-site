// --------------------------------------------------------------------------
// <copyright file="IChatService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.19">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Services
{
    using System.Collections.Generic;
    using Fanex.LiveCharts.Chat.Criteria;
    using Fanex.LiveCharts.Chat.Models;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IChatService
    {
        IList<ChatMessage> GetChatMessagesByTime(GetChatMessageByTimeCriteria getChatMessageCriteria);

        IList<ChatMessage> GetChatMessagesByTime(GetChatMessageByUserIdCriteria getChatMessageCriteria);

        ChatUser GetChatUser(GetChatUserCriteria getChatUserCriteria);

        ICommandResult InsertChatMessage(InsertChatMessageCriteria insertChatMessageCriteria);

        ICommandResult InsertChatRoom(InsertChatRoomCriteria insertChatRoomCriteria);

        ICommandResult InsertChatUser(InsertChatUserCriteria insertchatUserCriteria);

        ICommandResult UpdateChatMessage(UpdateChatMessageCriteria updateChatMessageCriteria);

        ICommandResult UpdateChatRoom(UpdateChatRoomCriteria updateChatRoomCriteria);

        ICommandResult UpdateChatUser(UpdateChatUserCriteria updateChatUserCriteria);
    }
}