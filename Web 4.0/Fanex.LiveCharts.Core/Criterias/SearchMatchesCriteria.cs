// --------------------------------------------------------------------------
// <copyright file="SearchMatchesCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.13">Created</change>
// </history>

namespace Fanex.LiveCharts.Criterias
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NexcelRules.NexcelRulesChecking", "EX1003:MethodMustBeDocumented", Justification = "Checked it.")]
    public class SearchMatchesCriteria : CriteriaBase
    {
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime MatchDate { get; set; }

        public int SportId { get; set; }

        public int LeagueId { get; set; }

        public string Keyword { get; set; }

        public bool KickedOff { get; set; }

        public override string GetSettingKey()
        {
            return "Chart_Admin_Search_Matches";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}