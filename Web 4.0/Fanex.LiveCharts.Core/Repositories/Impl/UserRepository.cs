// --------------------------------------------------------------------------
// <copyright file="UserRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.27">Created</change>
// </history>

namespace Fanex.LiveCharts.Repositories.Impl
{
    using System;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.LiveCharts.Repositories.Interface;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// User repository, handle request to database.
    /// </summary>
    public class UserRepository : BaseRepository, IUserRepository
    {
        /// <summary>
        /// Recheck in data.
        /// </summary>
        /// <param name="criteria">Recheck in criteria.</param>
        /// <returns>Is kick out or not.</returns>
        public bool ReCheckIn(ICriteria criteria)
        {
            if (criteria.IsValid())
            {
                using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var parameters = criteria.GetSpParams();
                    if (!parameters.ContainsKey("iskickedout"))
                    {
                        parameters.Add("iskickedout", string.Empty);
                    }

                    objDb.ExecuteNonQuery(parameters);

                    var isKickedOut = Converter.ToBool(parameters["iskickedout"]);
                    return isKickedOut;
                }
            }

            return false;
        }

        /// <summary>
        /// On user check in.
        /// </summary>
        /// <param name="criteria">Check in criteria.</param>
        /// <returns>Online user id.</returns>
        public int CheckIn(ICriteria criteria)
        {
            if (criteria.IsValid())
            {
                using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var parameters = criteria.GetSpParams();
                    if (!parameters.ContainsKey("onuserid"))
                    {
                        parameters.Add("onuserid", string.Empty);
                    }

                    objDb.ExecuteNonQuery(parameters);

                    return Converter.ToInt(parameters["onuserid"]);
                }
            }

            return 0;
        }

        /// <summary>
        /// Check valid token.
        /// </summary>
        /// <param name="criteria">Criteria check valid token.</param>
        /// <returns>Token valid or not.</returns>
        public bool CheckToken(ICriteria criteria)
        {
            if (criteria.IsValid())
            {
                using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var parameters = criteria.GetSpParams();
                    if (!parameters.ContainsKey("isvalid"))
                    {
                        parameters.Add("isvalid", string.Empty);
                    }

                    objDb.ExecuteNonQuery(parameters);

                    var isValid = Converter.ToBool(parameters["isvalid"]);
                    return isValid;
                }
            }

            return false;
        }
    }
}