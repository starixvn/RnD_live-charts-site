// --------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.13">Created</change>
// </history>

namespace Fanex.LiveCharts.Site
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Fanex.Data.Configuration;
    using Fanex.Logging;
    using Newtonsoft.Json;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801
    [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "EX1002:ClassMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "EX1003:MethodMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            JsonSerializerSettings jSettings = new Newtonsoft.Json.JsonSerializerSettings();
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings = jSettings;

            DbSettingProviderManager.Current.Start();
        }

        ////protected void Application_BeginRequest(object sender, EventArgs e)
        ////{
        ////    // Source: http://www.quickstepit.net/internet-explorer-iframe-cookies-mvc3/
        ////    HttpContext.Current.Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");
        ////}

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                var lastError = Server.GetLastError();
                if (lastError != null)
                {
                    // Logging
                    Exception ex = lastError.GetBaseException();

                    if (Request != null && !Request.IsLocal)
                    {
                        string message = ex.Message +
                            "\r\nSource: " + ex.Source +
                            "\r\nDomain: " + Request.Headers["Host"] +
                            "\r\nForm: " + Request.Form +
                            "\r\nPath: " + Request.Url.AbsoluteUri +
                            "\r\nTargetSite: " + ex.TargetSite +
                            "\r\nStackTrace: " + ex.ToString();

                        Logger.Log.Error(message);
                    }

                    // Redirect to Error pages
                    var serverError = Server.GetLastError() as HttpException;
                    string errorCode = string.Empty;

                    // Do not log http exception
                    if (serverError != null)
                    {
                        errorCode = serverError.GetHttpCode().ToString();
                    }

                    if (!Request.IsLocal)
                    {
                        Server.ClearError();

                        if (!Response.IsRequestBeingRedirected)
                        {
                            Response.Redirect(string.Format("~/user/error?err={0}", errorCode));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }
        }
    }
}