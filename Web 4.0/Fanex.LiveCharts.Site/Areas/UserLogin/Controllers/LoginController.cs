// --------------------------------------------------------------------------
// <copyright file="LoginController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Add properties _currentContext</change>
// </history>

namespace Fanex.NAP.Hub.Areas.UserLogin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.LiveCharts.Core;
    using Fanex.Logging;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Auth;
    using Fanex.NAP.Core.Web.Session;

    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1123:DoNotPlaceRegionsWithinElements", Justification = "Reviewed.")]
    [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "EX1002:ClassMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "EX1003:MethodMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
    public class LoginController : BaseController
    {
        #region members

        private IUserService _userService;
        private IGroupService _groupService;
        private ISiteService _siteService;
        private IMenuItemService _menuItemService;
        private ICurrentSession _currentSession;
        private IMaintainanceService _maintenanceService;

        #endregion members

        #region ctors

        public LoginController()
            : this(null, null, null, null, null, null)
        {
        }

        public LoginController(
            IUserService userService,
            IGroupService groupService,
            ISiteService siteService,
            IMenuItemService menuItemService,
            ICurrentSession currentSession,
            IMaintainanceService maintenanceService)
        {
            _userService = userService ?? new UserService();
            _groupService = groupService ?? new GroupService();
            _siteService = siteService ?? new SiteService();
            _menuItemService = menuItemService ?? new MenuItemService();
            _currentSession = currentSession ?? new CurrentSession();
            _maintenanceService = maintenanceService ?? new MaintainanceService();
        }

        #endregion ctors

        #region helpers

        private void CreateUserMenu(User user)
        {
            _currentSession.MenuItemList = (IList<MenuItem>)_menuItemService.GetByUserAndSiteIncludeGroup(
                                                                                    user.Id,
                                                                                    SiteId,
                                                                                    Environment,
                                                                                    true);
        }

        private void CreateAuthInfo(User user)
        {
            var currentDomain = CurrentContext.GetCurrentDomain();

            var authInfo = new AuthInfo();

            authInfo.UserId = user.Id;
            authInfo.OnUserId = this._currentSession.OnUserId;

            var authTicket = new FormsAuthenticationTicket(
                 1,
                user.Username,
                DateTime.Now,
                DateTime.Now.AddMinutes(20000),
                false,
                authInfo.ToXml(),
                currentDomain.Path);

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Path = currentDomain.Path,
                Name = FormsAuthentication.FormsCookieName
            };

            Response.Cookies.Add(authCookie);
        }

        #endregion helpers

        #region sign in

        [HttpGet]
        public ActionResult Index()
        {
            if (!AppSettings.IsAdminSite)
            {
                return Redirect(AppSettings.RedirectURLToAdminSite);
            }

            var maintenanceInfo = _maintenanceService.GetDowntimeInfo();

            if (maintenanceInfo.IsDowntime)
            {
                return RedirectToAction("Offline", "Common");
            }

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(string username, string password)
        {
            if (!username.IsNullOrTrimEmpty() && !password.IsNullOrTrimEmpty())
            {
                var user = _userService.GetByUsername(username.Trim()) as User;

                try
                {
                    if (user != null)
                    {
                        password = Cryptography.CfsEnCode(password);

                        if (password == user.Password)
                        {
                            var group = user.Group == null ? null : _groupService.GetById(user.Group.Id);
                            var siteLevelList = _siteService.GetSiteLevelByUser(user.Id, Environment); // gets all sites of logging-in user

                            if ((group == null || group.IsActive) && siteLevelList.Where(x => x.IsActive).Any())
                            {
                                #region allows user login

                                #region check-in user

                                var criteria = new UserCheckInUserCriteria
                                {
                                    UserId = user.Id,
                                    SiteId = this.SiteId,
                                    NetSessionId = Session.SessionID,
                                    Ip = CurrentContext.IP,
                                    LoginDate = DateTime.Now,
                                    BrowserName = CurrentContext.BrowserName,
                                    CountryCode = CurrentContext.CountryCode,
                                    Domain = CurrentContext.DomainName,
                                    Platform = CurrentContext.Platform
                                };

                                _userService.UserCheckIn(ref criteria);

                                #endregion check-in user

                                #region saves user session

                                _currentSession.Level = _userService.GetUserSiteLevel(user.Id, this.SiteId);
                                _currentSession.UserId = user.Id;
                                _currentSession.Username = username;
                                _currentSession.OnUserId = criteria.OnUserId;
                                _currentSession.SessionId = Session.SessionID.ToString();

                                //// Save authentication data.
                                LiveCharts.Web.SessionManager.CurrentSession = new LiveCharts.Web.CurrentSession
                                {
                                    AdminId = user.Id,
                                    AdminName = username,
                                    Language = "en-US"
                                };

                                // current site is not NAP Hub
                                if (this.SiteId != 0)
                                {
                                    CreateUserMenu(user);
                                }

                                CreateAuthInfo(user);

                                #endregion saves user session

                                #region redirects user

                                // if the user login directly, not through NAP Hub
                                if (this.IsSiteLogin == 1)
                                {
                                    #region checks if logging-in user is active on this site

                                    bool userIsActiveOnThisSite = false;

                                    foreach (var siteLevel in siteLevelList)
                                    {
                                        if (siteLevel.Site.Id == this.SiteId && siteLevel.IsActive)
                                        {
                                            userIsActiveOnThisSite = true;
                                        }
                                    }

                                    #endregion checks if logging-in user is active on this site

                                    if (userIsActiveOnThisSite)
                                    {
                                        return Redirect(FormsAuthentication.DefaultUrl);
                                    }
                                    else
                                    {
                                        DoSignOut();
                                        ViewBag.Message = ViewResources.Your_account_was_suspended;
                                    }
                                }
                                else
                                {
                                    // user has access to multiple sites
                                    if (siteLevelList.Where(s => s.IsActive).Count() > 1)
                                    {
                                        return RedirectToAction("Site", "UserSite");
                                    }

                                    #region user has acces to one site

                                    var site = siteLevelList.Where(s => s.IsActive).First().Site;

                                    var listSiteUrl = _siteService.GetSiteUrlBySiteId(site.Id, this.Environment).Where(u => u.IsActive).ToList();

                                    if (listSiteUrl.Count() > 1)
                                    {
                                        return RedirectToAction("Site", "UserSite");
                                    }

                                    return RedirectToAction("Forward", "UserSite", new { id = listSiteUrl.First().SiteUrlId });

                                    #endregion user has acces to one site
                                }

                                #endregion redirects user

                                #endregion allows user login
                            }
                            else
                            {
                                // user account was suspended on all sites
                                if (siteLevelList.Count() > 0)
                                {
                                    ViewBag.Message = ViewResources.Your_account_was_suspended;
                                }
                                else
                                {
                                    // logging-in user belongs to no site
                                    ViewBag.Message = ViewResources.You_have_no_authorization;
                                }
                            }
                        }
                        else
                        {
                            // wrong password
                            ViewBag.Message = ViewResources.Invalid_Username_or_Password;
                        }
                    }
                    else
                    {
                        // user doesn't exist
                        ViewBag.Message = ViewResources.Invalid_Username_or_Password;
                    }
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message, ex);
                    throw;
                }
            }
            else
            {
                ViewBag.Message = "Invalid input!";
            }

            return View();
        }

        #endregion sign in

        #region sign out

        public ActionResult LogOut()
        {
            DoSignOut();

            return Redirect(FormsAuthentication.LoginUrl);
        }

        private void DoSignOut()
        {
            _userService.UserLogOut(new LogOutUserCriteria
            {
                OnUserId = _currentSession.OnUserId
            });
            FormsAuthentication.SignOut();
            Response.Cookies.Clear();
            _currentSession.ClearAll();
        }

        #endregion sign out

        #region re check in

        [HttpPost]
        public ActionResult ReCheckLogin()
        {
            var maintenanceInfo = _maintenanceService.GetDowntimeInfo();

            if (maintenanceInfo.IsDowntime)
            {
                return Json("-1");
            }

            // check if not nap hub
            if (this.SiteId != 0)
            {
                var siteStatus = _siteService.GetStatusById(this.SiteId);

                if (!siteStatus)
                {
                    return Json("1");
                }
            }

            UserReCheckInUserCriteria criteria = new UserReCheckInUserCriteria();
            criteria.OnUserId = Convert.ToInt32(_currentSession.OnUserId);
            _userService.ReUserCheckIn(ref criteria);

            return Json(criteria.IsKickedOut ? "1" : "0");
        }

        #endregion re check in
    }
}