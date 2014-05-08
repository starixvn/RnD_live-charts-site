// --------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Add properties _currentContext</change>
// </history>

namespace Fanex.NAP.Hub.Areas.UserLogin.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Context;

    [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "EX1002:ClassMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1609:PropertyDocumentationMustHaveValue", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed.")]
    public class BaseController : Controller
    {
        protected readonly string Environment = NapSettings.Settings.Environment;

        protected readonly int SiteId = NapSettings.Settings.SiteId;

        private bool _issiteLogin = NapSettings.Settings.EnableDirectLogin;

        protected readonly CurrentContext CurrentContext;

        private ViewFlags _viewFlags;

        protected ViewFlags ViewFlags
        {
            get
            {
                return ViewBag.ViewFlags;
            }

            set
            {
                ViewBag.ViewFlags = _viewFlags = value;
            }
        }

        public BaseController()
        {
            CurrentContext = new CurrentContext();

            ViewFlags = new ViewFlags
            {
                IsReCheckin = NapSettings.Settings.EnableReCheckIn
            };
        }

        /// <summary>
        /// Indicates whether the current site allows users login directly.
        /// If "0", users must login through NAP Hub.
        /// </summary>
        public bool IsSiteLogin
        {
            get
            {
                return _issiteLogin;
            }
        }
    }
}