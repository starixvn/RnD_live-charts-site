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
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.LiveCharts.Models;
    using Fanex.LiveCharts.Resources;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Error Code Default.
    /// </summary>
    public class BaseRepository : DynamicRepository
    {
        private const int ErrorCodeDefault = 1001;

        /// <summary>
        /// Modify data to database.
        /// </summary>
        /// <param name="criteria">Modify data criteria.</param>
        /// <returns>Command result.</returns>
        public ICommandResult ModifyData(ICriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);

            if (criteria.IsValid())
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(hash);
                    return GetCommandResult(hash);
                }
            }
            else
            {
                return new CommandResult
                {
                    Code = ErrorCodeDefault,
                    Message = Lang.InvalidInput
                };
            }
        }

        /// <summary>
        /// Modify data with insert command.
        /// </summary>
        /// <param name="criteria">Get data criteria.</param>
        /// <param name="resultIdParamName">Result parameters name of id field.</param>
        /// <returns>Insert result.</returns>
        public InsertResult ModifyData(ICriteria criteria, string resultIdParamName)
        {
            var insertResult = new InsertResult
            {
                CommandResult = new CommandResult
                {
                    Code = ErrorCodeDefault,
                    Message = Lang.InvalidInput
                }
            };

            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);

            resultIdParamName = resultIdParamName.ToLower();
            if (!hash.ContainsKey(resultIdParamName))
            {
                hash.Add(resultIdParamName, 0);
            }

            if (criteria.IsValid())
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(hash);
                    insertResult.CommandResult = GetCommandResult(hash);
                    insertResult.InsertedId = Converter.ToInt(hash[resultIdParamName]);
                }
            }

            return insertResult;
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