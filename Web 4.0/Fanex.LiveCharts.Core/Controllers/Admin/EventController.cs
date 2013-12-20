// --------------------------------------------------------------------------
// <copyright file="EventController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.8.19">Created</change>
// </history>

namespace Fanex.LiveCharts.Controllers.Admin
{
    using System.Web;
    using System.Web.Mvc;
    using Fanex.LiveCharts.Criterias;
    using Fanex.LiveCharts.Services.Impl;
    using Fanex.LiveCharts.Services.Interface;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Event Controller.
    /// </summary>
    [NapSuperAuth]
    public class EventController : Controller
    {
        private readonly IMatchService _matchService;

        public EventController()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventController" /> class.
        /// </summary>
        /// <param name="matchService">Match Service.</param>
        public EventController(IMatchService matchService)
        {
            _matchService = matchService ?? new MatchService();
        }

        /// <summary>
        /// Get Event with match Id.
        /// </summary>
        /// <param name="matchId">Current match id need searching event.</param>
        /// <returns>List events.</returns>
        [HttpGet]
        public JsonResult GetEvents(int matchId)
        {
            GetEventsByMatchIdCriteria criteria = new GetEventsByMatchIdCriteria
            {
                MatchId = matchId
            };
            var events = _matchService.GetEventsByMatchId(criteria);
            return Json(events, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Event with event id.
        /// </summary>
        /// <param name="id">Event Id for get data.</param>
        /// <returns>Current event.</returns>
        [HttpGet]
        public JsonResult GetEvent(long id)
        {
            GetEventByIdCriteria criteria = new GetEventByIdCriteria
            {
                EventId = id
            };
            var eve = _matchService.GetEventById(criteria);
            return Json(eve, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get event by event type.
        /// </summary>
        /// <param name="eventType">Event type.</param>
        /// <param name="matchId">Get event from this match id.</param>
        /// <returns>Event list.</returns>
        [HttpGet]
        public JsonResult GetEventsByEventType(int eventType, int matchId)
        {
            GetEventsByEventTypeCriteria criteria = new GetEventsByEventTypeCriteria
            {
                EventType = (short)eventType,
                MatchId = matchId
            };
            var events = _matchService.GetEventsByEventType(criteria);
            return Json(events, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Event type.
        /// </summary>
        /// <returns>List event type.</returns>
        [HttpGet]
        public JsonResult GetEventTypes()
        {
            var eventTypes = _matchService.GetEventTypes(new GetEventTypesCriteria());
            return Json(eventTypes, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Insert event.
        /// </summary>
        /// <param name="criteria">Insert criteria.</param>
        /// <returns>Success or not message.</returns>
        [HttpPost]
        public ActionResult InsertEvent(InsertEventCriteria criteria)
        {
            criteria.Detail = HttpUtility.HtmlEncode(criteria.Detail);
            var result = _matchService.InsertEvent(criteria);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// Update event.
        /// </summary>
        /// <param name="criteria">Update event criteria.</param>
        /// <returns>Success or not.</returns>
        [HttpPost]
        public ActionResult UpdateEvent(UpdateEventCriteria criteria)
        {
            criteria.Detail = HttpUtility.HtmlEncode(criteria.Detail);
            var result = _matchService.UpdateEvent(criteria);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// Delete event.
        /// </summary>
        /// <param name="criteria">Delete event criteria.</param>
        /// <returns>Success or not.</returns>
        [HttpPost]
        public ActionResult DeleteEvent(DeleteEventCriteria criteria)
        {
            var result = _matchService.DeleteEvent(criteria);
            return Json(result, JsonRequestBehavior.DenyGet);
        }
    }
}