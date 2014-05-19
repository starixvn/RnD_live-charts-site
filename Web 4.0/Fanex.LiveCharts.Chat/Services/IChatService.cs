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
    using Fanex.LiveCharts.Chat.ViewModels;
    using Fanex.LiveCharts.Models;
    using Fanex.NAP.Framework.Command;
    using NPOI.HSSF.UserModel;

    /// <summary>
    /// I Chat Service.
    /// </summary>
    public interface IChatService
    {
        IEnumerable<ChatMessage> GetChatMessagesByTime(GetChatMessageByTimeCriteria getChatMessageCriteria);

        IEnumerable<ChatMessage> GetChatMessagesByUser(GetChatMessageByUserIdCriteria getChatMessageCriteria);

        IEnumerable<ChatMessage> GetChatMessageByRoom(GetChatMessageByRoomCriteria getChatMessageByRoomCriteria);

        IEnumerable<ChatMessage> SearchChatMessage(SearchChatMessageCriteria criteria);

        ChatRoom GetChatRoomByName(GetChatRoomByNameCriteria getChatRoomByName);

        IEnumerable<ChatRoom> GetChatRoomByMatchId(GetChatRoomByMatchIdCriteria getChatRoomByMatchIdCriteria);

        IEnumerable<ChatUserInRoom> GetChatRoomUsers(GetChatRoomUsersCriteria getChatRoomUsersCriteria);

        IEnumerable<ChatUserInRoom> AdminGetChatRoomUsers(AdminGetChatRoomUsersCriteria getChatRoomUsersCriteria);

        IEnumerable<ChatUser> SearchChatUser(SearchChatUserCriteria criteria);

        IEnumerable<MemberInfo> SearchMember(SearchMemberCriteria criteria);

        ICommandResult InsertMemberBanned(InsertMemberBannedCriteria criteria);

        ChatUser GetChatUser(GetChatUserCriteria getChatUserCriteria);

        ChatUserInRoom GetChatUserInRoom(GetChatUserInRoomCriteria getChatUserInRoomCriteria);

        ChatUserInRoom GetChatUserInRoomByConnectionId(GetChatUserInRoomByConnectionIdCriteria getChatUserInRoomByConnectionIdCriteria);

        MemberInfo GetMemberBannedInformation(GetMemberBannedCriteria criteria);

        InsertResult InsertChatMessage(InsertChatMessageCriteria insertChatMessageCriteria);

        InsertResult InsertChatRoom(InsertChatRoomCriteria insertChatRoomCriteria);

        InsertResult InsertChatUser(InsertChatUserCriteria insertchatUserCriteria, string chatNameDenyList);

        InsertResult InsertChatUserInRoom(InsertChatUserInRoomCriteria insertChatUserInRoomCriteria);

        ICommandResult UpdateChatMessage(UpdateChatMessageCriteria updateChatMessageCriteria);

        ICommandResult UpdateChatUserAgreedTerms(UpdateChatUserAgreeTermsCriteria criteria);

        ICommandResult UpdateChatMessageByChatUser(UpdateChatMessageOfChatUserCriteria criteria);

        UpdateBanListUserViewModel UpdateBanChatUsersOnAdminSite(UpdateBanChatUsersCriteria criteria);

        UpdateMuteChatUserViewModel UpdateMuteChatUserOnAdminSite(UpdateMuteChatUserCriteria criteria);

        UpdateMuteChatRoomViewModel UpdateMuteChatRoomOnAdminSite(UpdateMuteChatRoomCriteria criteria);

        ICommandResult UpdateMembersBanned(UpdateBanMembersCriteria criteria);

        ICommandResult UpdateChatRoom(UpdateChatRoomCriteria updateChatRoomCriteria);

        ICommandResult UpdateChatUser(UpdateChatUserCriteria updateChatUserCriteria);

        ICommandResult UpdateChatUserInRoom(UpdateChatUserInRoomCriteria updateChatUserInRoomCriteria);

        ICommandResult UpdateKickOutChatUser(UpdateKickOutChatUserCriteria updateKickOutChatUserCriteria);

        HSSFWorkbook ExportChatConversation(IEnumerable<ChatMessage> chatMessages, Match match, SearchChatMessageCriteria criteria, string templeteFilePath);

        ICommandResult KickOutChatUserOnMemberSite(KickOutChatUserViewModel kickOutChatUserViewModel);

        ICommandResult UpdateChatMessageOnMemberSite(UpdateChatMessageViewModel deleteChatMessageViewModel);

        ICommandResult BanChatUserOnMemberSite(BanChatUserViewModel banChatUser);

        ICommandResult UpdateBanListUsersOnMemberSite(UpdateBanListUserViewModel banListUser);

        ICommandResult UpdateChatMessageByChatUserIdOnMemberSite(UpdateChatMessageByChatUserViewModel deleteMessageByChatUser);

        KickOutChatUserViewModel KickOutChatUserOnAdminSite(int chatUserId, int chatRoomId);

        UpdateChatMessageViewModel UpdateChatMessageOnAdminSite(UpdateChatMessageCriteria updateChatMessageCriteria);

        BanChatUserViewModel BanChatUserOnAdminSite(int chatUserId);

        UpdateChatMessageByChatUserViewModel UpdateChatMessageByChatUserOnAdminSite(UpdateChatMessageOfChatUserCriteria updateChatMessageByChatUserCriteria);

        ICommandResult UpdateMuteChatRoomOnMemberSite(UpdateMuteChatRoomViewModel muteChatRoom);

        ICommandResult UpdateMuteChatUserOnMemberSite(UpdateMuteChatUserViewModel muteChatUser);
    }
}