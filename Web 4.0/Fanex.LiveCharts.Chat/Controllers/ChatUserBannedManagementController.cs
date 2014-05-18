// --------------------------------------------------------------------------
// <copyright file="ChatUserBannedManagementController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.5.17">Created</change>
// </history>

namespace Fanex.LiveCharts.Chat.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using Fanex.LiveCharts.Chat.Criteria;
    using Fanex.LiveCharts.Chat.Services;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [NapSuperAuth]
    public class ChatUserBannedManagementController : Controller
    {
        private readonly IChatService _chatService;

        public ChatUserBannedManagementController()
            : this(null)
        {
        }

        public ChatUserBannedManagementController(IChatService chatService)
        {
            _chatService = chatService ?? new ChatService();
        }

        public ActionResult Index()
        {
            return View("~/Areas/Admin/Views/Chat/ChatUserBannedManagement.cshtml");
        }

        public ActionResult MemberBanByDefault()
        {
            return View("~/Areas/Admin/Views/Chat/MemberBanByDefault.cshtml");
        }

        [HttpPost]
        public JsonResult SearchMember(SearchMemberCriteria criteria)
        {
            return Json(_chatService.SearchMember(criteria));
        }

        [HttpPost]
        public JsonResult SearchChatUser(SearchChatUserCriteria criteria)
        {
            criteria.ChatName = HttpUtility.HtmlEncode(criteria.ChatName);
            return Json(_chatService.SearchChatUser(criteria));
        }

        [HttpPost]
        public JsonResult UpdateBanChatUsers(UpdateBanChatUsersCriteria criteria)
        {
            return Json(_chatService.UpdateBanChatUsersOnAdminSite(criteria));
        }
    }
}