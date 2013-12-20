// --------------------------------------------------------------------------
// <copyright file="UserService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.27">Created</change>
// </history>

namespace Fanex.LiveCharts.Services.Impl
{
    using System;
    using Fanex.LiveCharts.Criterias;
    using Fanex.LiveCharts.Repositories.Impl;
    using Fanex.LiveCharts.Repositories.Interface;
    using Fanex.LiveCharts.Services.Interface;
    using Fanex.Logging;

    /// <summary>
    /// User service, handle every things relate to user.
    /// </summary>
    public class UserService : ServiceBase, IUserService
    {
        private const string CacheString = "Fanex.LiveCharts.Authentication";
        private readonly IUserRepository _userRepository;

        public UserService()
            : this(null)
        {
        }

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? new UserRepository();
        }

        /// <summary>
        /// Check online user is lost session, drop out.
        /// </summary>
        /// <param name="criteria">Checking user criteria.</param>
        /// <returns>Is kick out or not.</returns>
        public bool ReCheckIn(UserReCheckInCriteria criteria)
        {
            try
            {
                return _userRepository.ReCheckIn(criteria);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return false;
        }

        /// <summary>
        /// First check in user.
        /// </summary>
        /// <param name="criteria">Insert user online data to database.</param>
        /// <returns>Insert data result information.</returns>
        public int CheckIn(UserCheckInCriteria criteria)
        {
            try
            {
                return _userRepository.CheckIn(criteria);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return 0;
        }

        /// <summary>
        /// Check user token is valid or not.
        /// </summary>
        /// <param name="memberId">Current member id need check.</param>
        /// <param name="matchId">Current match for viewing chart.</param>
        /// <param name="token">Current Token information.</param>
        /// <returns>Current token is valid or not.</returns>
        public bool CheckToken(int memberId, int matchId, string token)
        {
            try
            {
                if (memberId == 0 || string.IsNullOrEmpty(token))
                {
                    return false;
                }

                var tokenCacheKey = string.Format(
                    "{0}_{1}_{2}",
                    CacheString,
                    memberId,
                    matchId);
                var cachedToken = CacheManager.Instance.Get<string>(tokenCacheKey);                
                return cachedToken == token;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return false;
        }
    }
}