// --------------------------------------------------------------------------
// <copyright file="LiveChartAuthenticationAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.27">Created</change>
// </history>

namespace Fanex.LiveCharts.Web.Filters
{
    using System;
    using System.Web.Mvc;
    using Fanex.LiveCharts.Resources;
    using Fanex.Logging;

    /// <summary>
    /// Recheck in filter, check current user status for every request.
    /// </summary>
    public class LiveChartAuthenticationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Check current session is valid or not, kick out if not valid.
        /// </summary>
        /// <param name="filterContext">Current filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var isKickOut = false;

                if (SessionManager.CurrentSession == null)
                {
                    isKickOut = true;
                }
                else
                {
                    isKickOut = SessionManager.CurrentSession.MemberId == 0 && SessionManager.CurrentSession.AdminId == 0;
                }

                if (isKickOut)
                {
                    var response = filterContext.HttpContext.Response;
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        response.Clear();
                        response.Write(Lang.UnauthorizedMsg);
                        filterContext.Result = new HttpUnauthorizedResult();
                        filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                        response.End();
                        SessionManager.ClearAll();
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("~/User/Unauthorized");
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}