// --------------------------------------------------------------------------
// <copyright file="IUserRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.27">Created</change>
// </history>

namespace Fanex.LiveCharts.Repositories.Interface
{
    using Fanex.NAP.Framework.Data;

    public interface IUserRepository
    {
        int CheckIn(ICriteria criteria);

        bool ReCheckIn(ICriteria criteria);

        bool CheckToken(ICriteria criteria);
    }
}