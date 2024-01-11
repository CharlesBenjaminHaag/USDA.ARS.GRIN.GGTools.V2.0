using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Data;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class ImportViewModel: AppViewModelBase
    {
        public HttpPostedFileBase DocumentUpload { get; set; }
        public DataTable DataCollectionDataTable { get; set; }

        public SysTableField GetColumnInfo(string sysTableName, string sysTableFieldName)
        {
            SysTableField sysTableField = new SysTableField();

            using (SysTableManager mgr = new SysTableManager())
            {
                sysTableField = mgr.GetSysTableField(sysTableName, sysTableFieldName);
            }
            return sysTableField;
        }
    }
}
