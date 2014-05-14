// --------------------------------------------------------------------------
// <copyright file="UserController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.27">Created</change>
// </history>

namespace Fanex.LiveCharts.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Common;
    using Fanex.LiveCharts.ClientAuthentication.Core;
    using Fanex.LiveCharts.Core;
    using Fanex.LiveCharts.Criterias;
    using Fanex.LiveCharts.Resources;
    using Fanex.LiveCharts.Services.Impl;
    using Fanex.LiveCharts.Services.Interface;
    using Fanex.LiveCharts.Web;
    using Fanex.LiveCharts.Web.Filters;
    using Fanex.Logging;

    /// <summary>
    /// User controller.
    /// </summary>
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController()
            : this(null)
        {
        }

        public UserController(IUserService userService)
        {
            _userService = userService ?? new UserService();
        }

        /// <summary>
        /// Unauthorized handle.
        /// </summary>
        /// <returns>Unauthorized page.</returns>
        public ActionResult Unauthorized()
        {
            FormsAuthentication.SignOut();
            SessionManager.ClearAll();
            Session.Abandon();

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, string.Empty);
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", string.Empty);
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);
            return View();
        }

        /// <summary>
        /// Authentication request coming.
        /// </summary>
        /// <param name="matchId">Match id need be show.</param>
        /// <param name="token">Token to access Chart.</param>
        /// <param name="language">Language for globalization.</param>
        /// <returns>Return view.</returns>
        [P3P]
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult Authentication(int matchId, string token, int language = (int)LanguageType.English)
        {
            try
            {
                if (AppSettings.EnableLogAuthentication)
                {
                    Logger.Log.Info(string.Format("MatchId: {0}, Token: {1}", matchId, token));
                }

                if (token.Contains(" "))
                {
                    token = token.Replace(' ', '+');
                }

                var dataDecrypted = Fanex.Common.Cryptography.CfsDeCrypt(token).Split(',');
                if (dataDecrypted.Length < 2)
                {
                    return RedirectToAction("Unauthorized");
                }

                var tokenDecrypted = dataDecrypted[0];
                var memberId = Converter.ToInt(dataDecrypted[1]);

                if (!_userService.CheckToken(memberId, matchId, tokenDecrypted))
                {
                    return RedirectToAction("Unauthorized");
                }
                else
                {
                    if (!AppSettings.LocalTest)
                    {
                        var isKickOut = _userService.ReCheckIn(new UserReCheckInCriteria
                        {
                            MemberId = memberId
                        });
                        if (isKickOut)
                        {
                            return RedirectToAction("Unauthorized");
                        }
                    }

                    if (SessionManager.CurrentSession == null)
                    {
                        SessionManager.CurrentSession = new CurrentSession();
                    }

                    SessionManager.CurrentSession.Token = token;
                    SessionManager.CurrentSession.MatchId = matchId;
                    if (SessionManager.CurrentSession.MemberId != memberId
                        || !HttpContext.User.Identity.IsAuthenticated)
                    {
                        SessionManager.CurrentSession.MemberId = memberId;
                        FormsAuthentication.SetAuthCookie(memberId.ToString(), false);
                    }

                    SetLanguage(language);
                    return RedirectToAction(
                        "MainPage",
                        "Charts",
                        new
                    {
                        matchId = matchId
                    });
                }
            }
            catch (Exception ex)
            {
                var logMessage = ex.Message;
                if (Request != null)
                {
                    logMessage += "\r\nPath: " + Request.Url.AbsoluteUri
                        + "\r\nTokenDecoded: " + token
                        + "\r\nTokenDecodedWithHttpUtility: " + HttpUtility.UrlDecode(token);
                }

                Logger.Log.Error(logMessage, ex);
            }

            return RedirectToAction("Unauthorized");
        }

        /// <summary>
        /// Check current session is valid.
        /// </summary>
        /// <param name="matchId">Check current match id is equal with match id save in session.</param>
        /// <returns>Return kick out status.</returns>
        [HttpPost, LiveChartAuthentication]
        public JsonResult ReCheckIn(int matchId)
        {
            var isKickOut = false;
            var isViewTwoChart = false;

            if (SessionManager.CurrentSession.MemberId > 0
                && SessionManager.CurrentSession.AdminId == 0
                && !AppSettings.LocalTest)
            {
                isKickOut = _userService.ReCheckIn(new UserReCheckInCriteria
                {
                    MemberId = SessionManager.CurrentSession.MemberId
                });

                if (!isKickOut)
                {
                    isViewTwoChart = matchId != SessionManager.CurrentSession.MatchId;
                }
            }

            return Json(new
            {
                KickOut = isKickOut,
                IsViewTwoChart = isViewTwoChart,
                Message = string.Empty
            });
        }

        public ActionResult ShowOnlyOneMatch()
        {
            return View();
        }

        /// <summary>
        /// Error handler.
        /// </summary>
        /// <param name="err">Error code string.</param>
        /// <returns>Error page.</returns>
        public ActionResult Error(string err)
        {
            switch (err)
            {
                case "403":
                    ViewBag.ErrorMessage = Lang.HttpError_403;
                    break;

                case "404":
                    ViewBag.ErrorMessage = Lang.HttpError_404;
                    break;

                default:
                    ViewBag.ErrorMessage = Lang.HttpError_Default;
                    break;
            }

            return View();
        }

        [HttpGet]
        public JsonResult TestReCheckIn()
        {
            return Json(
                _userService.ReCheckIn(
                new UserReCheckInCriteria
            {
                MemberId = 1
            }),
            JsonRequestBehavior.AllowGet);
        }

        #region Helpers

        /// <summary>
        /// Set current language.
        /// </summary>
        /// <param name="language">Current language id.</param>
        private void SetLanguage(int language)
        {
            switch (language)
            {
                case (int)LanguageType.ChineseTraditional:  // Traditional
                    SessionManager.CurrentSession.Language = "zh-TW";
                    break;

                case (int)LanguageType.ChineseSimplified:  // Simplified
                    SessionManager.CurrentSession.Language = "zh-CN";
                    break;

                case (int)LanguageType.Japanese:
                    SessionManager.CurrentSession.Language = "ja-JP";
                    break;

                case (int)LanguageType.Thai: // Thai
                    SessionManager.CurrentSession.Language = "th-TH";
                    break;

                case (int)LanguageType.Korean:  // Korean
                    SessionManager.CurrentSession.Language = "ko-KR";
                    break;

                case (int)LanguageType.Vietnamese:  // Vietnamese
                    SessionManager.CurrentSession.Language = "en-US";
                    break;

                case (int)LanguageType.English:
                default:
                    SessionManager.CurrentSession.Language = "en-US";
                    break;
            }
        }

        #endregion Helpers
    }
}