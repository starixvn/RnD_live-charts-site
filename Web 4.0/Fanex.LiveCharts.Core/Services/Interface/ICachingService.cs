// --------------------------------------------------------------------------
// <copyright file="ICachingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.10">Created</change>
// </history>

namespace Fanex.LiveCharts.Services.Interface
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.NexcelRules.NexcelRulesChecking", "EX1003:MethodMustBeDocumented", Justification = "Checked it.")]
    public interface ICachingService
    {
        T Get<T>(string key);

        bool Set<T>(string key, T value);

        bool Set<T>(string key, T value, TimeSpan timeSpanExpiration);

        bool Set<T>(string key, T value, DateTime absoluteExpiration);

        void Remove(string key);
    }
}