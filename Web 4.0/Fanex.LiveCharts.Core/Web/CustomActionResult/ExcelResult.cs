// --------------------------------------------------------------------------
// <copyright file="ExcelResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.4.2">Created</change>
// </history>

namespace Fanex.LiveCharts.Web.CustomActionResult
{
    using System;
    using System.IO;
    using Fanex.NAP.Framework.Mvc;
    using NPOI.HSSF.UserModel;

    /// <summary>
    /// Excel Result.
    /// </summary>
    public class ExcelResult : SpreadSheetResult
    {
        public ExcelResult(HSSFWorkbook hssfWorkBook, string fileName)
            : base(hssfWorkBook, fileName)
        {
        }

        protected override string ContentType
        {
            get { return "application/vnd.ms-excel"; }
        }

        protected override string Extension
        {
            get { return ".xls"; }
        }

        protected override Action<HSSFWorkbook, Stream> SaveAction
        {
            get { return (wb, s) => wb.Write(s); }
        }
    }
}