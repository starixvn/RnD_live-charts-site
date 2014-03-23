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
    using Fanex.LiveCharts.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IChatService
    {
        IEnumerable<ChatMessage> GetChatMessagesByTime(GetChatMessageByTimeCriteria getChatMessageCriteria);

        IEnumerable<ChatMessage> GetChatMessagesByUser(GetChatMessageByUserIdCriteria getChatMessageCriteria);

        ChatRoom GetChatRoomByName(GetChatRoomByNameCriteria getChatRoomByName);

        IEnumerable<ChatUser> GetChatRoomUsers(GetChatRoomUsersCriteria getChatRoomUsersCriteria);

        ChatUser GetChatUser(GetChatUserCriteria getChatUserCriteria);

        InsertResult InsertChatMessage(InsertChatMessageCriteria insertChatMessageCriteria);

        InsertResult InsertChatRoom(InsertChatRoomCriteria insertChatRoomCriteria);

        InsertResult InsertChatUser(InsertChatUserCriteria insertchatUserCriteria);

        InsertResult InsertChatUserInRoom(InsertChatUserInRoomCriteria insertChatUserInRoomCriteria);

        ICommandResult UpdateChatMessage(UpdateChatMessageCriteria updateChatMessageCriteria);

        ICommandResult UpdateChatRoom(UpdateChatRoomCriteria updateChatRoomCriteria);

        ICommandResult UpdateChatUser(UpdateChatUserCriteria updateChatUserCriteria);

        ICommandResult UpdateChatUserInRoom(UpdateChatUserInRoomCriteria updateChatUserInRoomCriteria);
    }
}