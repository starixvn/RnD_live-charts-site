// --------------------------------------------------------------------------
// <copyright file="BaseRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.27">Created</change>
// </history>

namespace Fanex.LiveCharts.Repositories.Impl
{
    using System;
    using System.Collections;
    using Fanex.Data;
    using Fanex.LiveCharts.Resources;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BaseRepository : DynamicRepository
    {
        /// <summary>
        /// Modify data to database.
        /// </summary>
        /// <param name="criteria">Modify data criteria.</param>
        /// <returns>Command result.</returns>
        public ICommandResult ModifyData(ICriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                return GetCommandResult(hash);
            }
        }

        /// <summary>
        /// Get command result after execute store procedure.
        /// </summary>
        /// <param name="hash">Output parameter.</param>
        /// <returns>Command result.</returns>
        private CommandResult GetCommandResult(Hashtable hash)
        {
            var errCode = hash["errcode"];
            var errMsg = hash["errmsg"];

            var commandResult = new CommandResult
            {
                Code = (errCode is DBNull) ? 0 : Convert.ToInt32(errCode),
                Message = (errMsg is DBNull) ? string.Empty : Convert.ToString(errMsg)
            };

            if (commandResult.Code == 0 && string.IsNullOrEmpty(commandResult.Message))
            {
                commandResult.Message = Lang.UpdateSuccessfullyMessage;
            }

            return commandResult;
        }
    }
}