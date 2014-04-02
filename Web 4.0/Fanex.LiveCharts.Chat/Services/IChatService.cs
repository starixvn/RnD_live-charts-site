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
    using Fanex.LiveCharts.Models;
    using Fanex.NAP.Framework.Data;
    using NPOI.HSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IChatService
    {
        IEnumerable<ChatMessage> GetChatMessagesByTime(GetChatMessageByTimeCriteria getChatMessageCriteria);

        IEnumerable<ChatMessage> GetChatMessagesByUser(GetChatMessageByUserIdCriteria getChatMessageCriteria);

        IEnumerable<ChatMessage> GetChatMessageByRoom(GetChatMessageByRoomCriteria getChatMessageByRoomCriteria);

        ChatRoom GetChatRoomByName(GetChatRoomByNameCriteria getChatRoomByName);

        IEnumerable<ChatRoom> GetChatRoomByMatchId(GetChatRoomByMatchIdCriteria getChatRoomByMatchIdCriteria);

        IEnumerable<ChatUser> GetChatRoomUsers(GetChatRoomUsersCriteria getChatRoomUsersCriteria);

        ChatUser GetChatUser(GetChatUserCriteria getChatUserCriteria);

        ChatUserInRoom GetChatUserInRoom(GetChatUserInRoomCriteria getChatUserInRoomCriteria);

        ChatUserInRoom GetChatUserInRoomByConnectionId(GetChatUserInRoomByConnectionIdCriteria getChatUserInRoomByConnectionIdCriteria);

        InsertResult InsertChatMessage(InsertChatMessageCriteria insertChatMessageCriteria);

        InsertResult InsertChatRoom(InsertChatRoomCriteria insertChatRoomCriteria);

        InsertResult InsertChatUser(InsertChatUserCriteria insertchatUserCriteria);

        InsertResult InsertChatUserInRoom(InsertChatUserInRoomCriteria insertChatUserInRoomCriteria);

        ICommandResult UpdateChatMessage(UpdateChatMessageCriteria updateChatMessageCriteria);

        ICommandResult UpdateChatRoom(UpdateChatRoomCriteria updateChatRoomCriteria);

        ICommandResult UpdateChatUser(UpdateChatUserCriteria updateChatUserCriteria);

        ICommandResult UpdateChatUserInRoom(UpdateChatUserInRoomCriteria updateChatUserInRoomCriteria);

        HSSFWorkbook ExportChatConversation(IEnumerable<ChatMessage> chatMessages, Match match, ChatRoom chatRoom, string templeteFilePath);
    }
}