// --------------------------------------------------------------------------
// <copyright file="SpreadSheetResult.cs" company="Nexcel Solutions Vietnam">
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
    using System.Web.Mvc;
    using NPOI.HSSF.UserModel;

    /// <summary>
    /// Spread Sheet Result.
    /// </summary>
    public abstract class SpreadSheetResult : ActionResult
    {
        private readonly HSSFWorkbook _hssfWorkBook;
        private readonly string _fileName;

        protected SpreadSheetResult(HSSFWorkbook hssfWorkBook, string fileName)
        {
            this._hssfWorkBook = hssfWorkBook;
            this._fileName = fileName;
        }

        protected abstract string ContentType { get; }

        protected abstract string Extension { get; }

        protected abstract Action<HSSFWorkbook, Stream> SaveAction { get; }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.ContentType = this.ContentType;
            context.HttpContext.Response.AddHeader("Content-disposition", "filename=" + this._fileName + this.Extension);

            this.SaveAction(this._hssfWorkBook, context.HttpContext.Response.OutputStream);
        }
    }
}