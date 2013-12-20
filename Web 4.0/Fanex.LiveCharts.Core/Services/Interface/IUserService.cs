// --------------------------------------------------------------------------
// <copyright file="IUserService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.27">Created</change>
// </history>

namespace Fanex.LiveCharts.Services.Interface
{
    using Fanex.LiveCharts.Criterias;
    using Fanex.NAP.Framework.Data;

    public interface IUserService
    {
        bool ReCheckIn(UserReCheckInCriteria criteria);

        int CheckIn(UserCheckInCriteria criteria);

        bool CheckToken(int memberId, int matchId, string token);
    }
}