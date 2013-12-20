// --------------------------------------------------------------------------
// <copyright file="IMatchService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.8.20">Created</change>
// </history>

namespace Fanex.LiveCharts.Services.Interface
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.LiveCharts.Criterias;
    using Fanex.LiveCharts.Models;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NexcelRules.NexcelRulesChecking", "EX1003:MethodMustBeDocumented", Justification = "Checked it.")]
    public interface IMatchService
    {
        IEnumerable<Match> GetEnableCharts(GetEnableChartsCriteria criteria);

        IEnumerable<Match> SearchMatches(SearchMatchesCriteria criteria);

        IEnumerable<Event> GetEventsByMatchId(GetEventsByMatchIdCriteria criteria);

        Match GetMatchById(GetMatchByIdCriteria criteria);

        IEnumerable<EventType> GetEventTypes(GetEventTypesCriteria criteria);

        Event GetEventById(GetEventByIdCriteria criteria);

        IEnumerable<Event> GetEventsByEventType(GetEventsByEventTypeCriteria criteria);

        IEnumerable<League> GetLeagues(GetLeaguesAdminCriteria getLeaguesCriteria);

        ICommandResult InsertEvent(InsertEventCriteria criteria);

        ICommandResult UpdateEvent(UpdateEventCriteria criteria);

        ICommandResult DeleteEvent(DeleteEventCriteria criteria);

        ICommandResult ImportMatchQueue(ImportMatchQueueCriteria criteria);

        ICommandResult UpdateOddsStatus(UpdateOddsStatusCriteria updateOddsStatusCriteria);

        ICommandResult UpdateChartShowingStatus(UpdateChartShowingStatusCriteria updateChartShowingStatusCriteria);
    }
}