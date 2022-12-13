using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class SysAppErrorManager : AppDataManagerBase
    {
        public List<SysAppError> SearchAppErrors(SysAppErrorSearch searchEntity)
        {
            List<SysAppError> results = new List<SysAppError>();

            SQL = "SELECT * FROM sys_app_error_log WHERE CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE()) ORDER BY ID DESC";

            results = GetRecords<SysAppError>(SQL);
            RowsAffected = results.Count;

            return results;
        }
    }
}
