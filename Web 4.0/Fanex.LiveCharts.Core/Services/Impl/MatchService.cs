// -----------------------------------------------------------------------
// <copyright file="MatchService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.7.24">Created</change>
// </history>

namespace Fanex.LiveCharts.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.LiveCharts.Core;
    using Fanex.LiveCharts.Criterias;
    using Fanex.LiveCharts.Models;
    using Fanex.LiveCharts.Repositories.Impl;
    using Fanex.LiveCharts.Repositories.Interface;
    using Fanex.LiveCharts.Resources;
    using Fanex.LiveCharts.Services.Interface;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MatchService : ServiceBase, IMatchService
    {
        private readonly IChartRepository _chartRepository;

        public MatchService()
            : this(null)
        {
        }

        public MatchService(IChartRepository chartRepository)
        {
            _chartRepository = chartRepository ?? new ChartRepository();
        }

        /// <summary>
        /// Get List enable chart.
        /// </summary>
        /// <param name="criteria">Get enable chart criteria.</param>
        /// <returns>List of match were enabled chart.</returns>
        public IEnumerable<Match> GetEnableCharts(GetEnableChartsCriteria criteria)
        {
            try
            {
                criteria.ToDate = criteria.ToDate.AddDays(1);
                return _chartRepository.GetEnableCharts(criteria).OrderBy(m => m.KickOffTime).ThenBy(m => m.Id);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return new List<Match>();
            }
        }

        /// <summary>
        /// Get all event in a match.
        /// </summary>
        /// <param name="criteria">Get event criteria.</param>
        /// <returns>List of event.</returns>
        public IEnumerable<Event> GetEventsByMatchId(GetEventsByMatchIdCriteria criteria)
        {
            try
            {
                return _chartRepository.Fetch<Event>(criteria);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return new List<Event>();
            }
        }

        /// <summary>
        /// Get event by id.
        /// </summary>
        /// <param name="criteria">Get event criteria.</param>
        /// <returns>Current event.</returns>
        public Event GetEventById(GetEventByIdCriteria criteria)
        {
            try
            {
                return _chartRepository.Get<Event>(criteria);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return new Event();
            }
        }

        /// <summary>
        /// Get events by event type.
        /// </summary>
        /// <param name="criteria">Get event criteria.</param>
        /// <returns>Event list.</returns>
        public IEnumerable<Event> GetEventsByEventType(GetEventsByEventTypeCriteria criteria)
        {
            try
            {
                return _chartRepository.Fetch<Event>(criteria);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return new List<Event>();
            }
        }

        /// <summary>
        /// Get match by id.
        /// </summary>
        /// <param name="criteria">Get match criteria.</param>
        /// <returns>Match information.</returns>
        public Match GetMatchById(GetMatchByIdCriteria criteria)
        {
            try
            {
                return _chartRepository.Get<Match>(criteria);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return new Match();
            }
        }

        /// <summary>
        /// Search match with filter.
        /// </summary>
        /// <param name="criteria">Search match criteria.</param>
        /// <returns>List of match match with criteria.</returns>
        public IEnumerable<Match> SearchMatches(SearchMatchesCriteria criteria)
        {
            try
            {
                var matches = _chartRepository.Fetch<Match>(criteria);
                var currentMatchesDictionary = new Dictionary<int, Match>();
                foreach (var match in matches)
                {
                    if (currentMatchesDictionary.ContainsKey(match.Id))
                    {
                        var matchExist = currentMatchesDictionary[match.Id];
                        if (!matchExist.ChartEnabled && match.ChartEnabled)
                        {
                            currentMatchesDictionary[match.Id].ChartEnabled = true;
                        }

                        if (match.ChartShowingStatus > (byte)ChartShowingStatus.QASite)
                        {
                            currentMatchesDictionary[match.Id].ChartShowingStatus = match.ChartShowingStatus;
                        }
                    }
                    else
                    {
                        currentMatchesDictionary.Add(match.Id, match);
                    }
                }

                var maxTime = _chartRepository.GetMaxTime(new GetMaxTimeCriteria
                {
                    ChartType = (int)ChartType.Market
                });
                foreach (var match in currentMatchesDictionary)
                {
                    if (match.Value.KickOffTime > maxTime)
                    {
                        match.Value.CanEnableChart = true;
                    }
                    else if (match.Value.KickOffTime < maxTime && !criteria.KickedOff)
                    {
                        match.Value.HideInSearch = true;
                    }
                }

                return currentMatchesDictionary.Values.OrderBy(m => m.KickOffTime).ThenBy(m => m.Id);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return new List<Match>();
            }
        }

        /// <summary>
        /// Get event types.
        /// </summary>
        /// <param name="criteria">Get event types criteria.</param>
        /// <returns>List of event type.</returns>
        public IEnumerable<EventType> GetEventTypes(GetEventTypesCriteria criteria)
        {
            try
            {
                var eventTypes = _chartRepository.Fetch<EventType>(criteria);
                return eventTypes.Where(e => e.EventTypeId != (short)IndicatorType.Over
                                            && e.EventTypeId != (short)IndicatorType.ExtraTime);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return new List<EventType>();
            }
        }

        /// <summary>
        /// Get leagues.
        /// </summary>
        /// <param name="getLeaguesCriteria">Get league criteria.</param>
        /// <returns>List of league.</returns>
        public IEnumerable<League> GetLeagues(GetLeaguesAdminCriteria getLeaguesCriteria)
        {
            try
            {
                var result = _chartRepository.Fetch<League>(getLeaguesCriteria).OrderBy(l => l.Name);
                return result;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return new List<League>();
            }
        }

        /// <summary>
        /// Insert event.
        /// </summary>
        /// <param name="criteria">Insert event criteria.</param>
        /// <returns>Command status.</returns>
        public ICommandResult InsertEvent(InsertEventCriteria criteria)
        {
            try
            {
                return _chartRepository.ModifyData(criteria);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return new CommandResult();
            }
        }

        /// <summary>
        /// Update event.
        /// </summary>
        /// <param name="criteria">Update event criteria.</param>
        /// <returns>Command result.</returns>
        public ICommandResult UpdateEvent(UpdateEventCriteria criteria)
        {
            try
            {
                return _chartRepository.ModifyData(criteria);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return new CommandResult();
            }
        }

        /// <summary>
        /// Delete event.
        /// </summary>
        /// <param name="criteria">Delete event criteria.</param>
        /// <returns>Delete event result.</returns>
        public ICommandResult DeleteEvent(DeleteEventCriteria criteria)
        {
            try
            {
                return _chartRepository.ModifyData(criteria);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return new CommandResult();
            }
        }

        /// <summary>
        /// Update odds status for monitor odds.
        /// </summary>
        /// <param name="updateOddsStatusCriteria">Update odds criteria.</param>
        /// <returns>Result of update odds status.</returns>
        public ICommandResult UpdateOddsStatus(UpdateOddsStatusCriteria updateOddsStatusCriteria)
        {
            try
            {
                return _chartRepository.ModifyData(updateOddsStatusCriteria);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return new CommandResult();
            }
        }

        /// <summary>
        /// Update chart status for showing data on production.
        /// </summary>
        /// <param name="updateChartShowingStatusCriteria">Update charts status criteria.</param>
        /// <returns>Command result after updated.</returns>
        public ICommandResult UpdateChartShowingStatus(UpdateChartShowingStatusCriteria updateChartShowingStatusCriteria)
        {
            try
            {
                return _chartRepository.ModifyData(updateChartShowingStatusCriteria);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return new CommandResult();
            }
        }

        /// <summary>
        /// Import a match to match queue.
        /// </summary>
        /// <param name="criteria">Import match queue criteria.</param>
        /// <returns>Command result.</returns>
        public ICommandResult ImportMatchQueue(ImportMatchQueueCriteria criteria)
        {
            try
            {
                var maxTime = _chartRepository.GetMaxTime(new GetMaxTimeCriteria
                {
                    ChartType = (int)ChartType.Market
                });

                if (maxTime == null)
                {
                    return new CommandResult
                    {
                        Code = 1001,
                        Message = AdminSiteLang.CannotGetCurrentTime
                    };
                }
                ////else if (maxTime.Value > criteria.KickOffTime.AddMinutes(120))
                ////{
                ////    return new CommandResult
                ////    {
                ////        Code = 1001,
                ////        Message = AdminSiteLang.CannotEnableMatch
                ////    };
                ////}

                return _chartRepository.ModifyData(criteria);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                return new CommandResult();
            }
        }
    }
}