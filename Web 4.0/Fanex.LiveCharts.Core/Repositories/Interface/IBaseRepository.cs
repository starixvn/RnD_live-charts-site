// --------------------------------------------------------------------------
// <copyright file="IBaseRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.3.3">Created</change>
// </history>

namespace Fanex.LiveCharts.Repositories.Interface
{
    using Fanex.LiveCharts.Models;
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// I Base Repository.
    /// </summary>
    public interface IBaseRepository : IDynamicRepository
    {
        ICommandResult ModifyData(ICriteria criteria);

        InsertResult ModifyData(ICriteria criteria, string resultIdParamName);
    }
}