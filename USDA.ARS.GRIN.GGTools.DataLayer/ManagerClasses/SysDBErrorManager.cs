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
    public class  SysDBErrorManager: GRINGlobalDataManagerBase
    {
        public List<SysDBError> SearchDBErrors(SysDBErrorSearch searchEntity)
        {
            List<SysDBError> results = new List<SysDBError>();

            SQL = "SELECT * FROM sys_db_error WHERE CONVERT(DATE, ErrorDateTime) = CONVERT(DATE, GETDATE()) ORDER BY ErrorID DESC";

            results = GetRecords<SysDBError>(SQL);
            RowsAffected = results.Count;

            return results;
        }
    }
}
