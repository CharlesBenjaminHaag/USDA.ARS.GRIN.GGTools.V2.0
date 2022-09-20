using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class CropForCWRSearch : SearchEntityBase
    {
        public string Name { get; set; }
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public string SqlWhereClause { get; set; }

        //public string SqlWhereClause
        //{
        //        // Get object field names and values.
        //        //Object target = this;

        //        //PropertyInfo[] properties  = this.GetType().GetProperties();

        //        //string str = "";
        //        //foreach (PropertyInfo p in properties)
        //        //{
        //        //    str += GetPropValue(this, p.Name);
        //        //}
        //        ////DEBUG
        //        //return str;
        //        //int i = 0;
        //        //StringBuilder sbWhereClause = new StringBuilder();
        //        //foreach (QueryCriterion queryCriterion in QueryCriteria)
        //        //{
        //        //    if (i == 0)
        //        //        sbWhereClause.Append(" WHERE ");
        //        //    else
        //        //    {
        //        //        sbWhereClause.Append(" " + queryCriterion.LogicalOperator + " ");
        //        //    }

        //        //    sbWhereClause.Append(queryCriterion.FieldName);
        //        //    sbWhereClause.Append(" ");
        //        //    sbWhereClause.Append(queryCriterion.SearchOperatorCode);
        //        //    sbWhereClause.Append(" ");

        //        //    if (queryCriterion.DataType == "NVARCHAR")
        //        //    {
        //        //        if (queryCriterion.FieldValue == "NULL")
        //        //        {
        //        //            sbWhereClause.Append(queryCriterion.FieldValue);
        //        //        }
        //        //        else
        //        //        {
        //        //            sbWhereClause.Append("'");
        //        //            if (queryCriterion.SearchOperatorCode == "LIKE")
        //        //            {
        //        //                sbWhereClause.Append("%");
        //        //            }
        //        //            sbWhereClause.Append(queryCriterion.FieldValue);
        //        //            if (queryCriterion.SearchOperatorCode == "LIKE")
        //        //            {
        //        //                sbWhereClause.Append("%");
        //        //            }
        //        //            sbWhereClause.Append("'");
        //        //        }
        //        //    }
        //        //    else
        //        //    {
        //        //        if (queryCriterion.DataType == "DATETIME")
        //        //        {
        //        //            sbWhereClause.Append("'");
        //        //            sbWhereClause.Append(queryCriterion.FieldValue);
        //        //            sbWhereClause.Append("'");
        //        //        }
        //        //        else
        //        //        {
        //        //            if (queryCriterion.DataType == "INT")
        //        //            {
        //        //                sbWhereClause.Append(queryCriterion.FieldValue);
        //        //            }
        //        //        }
        //        //    }
        //        //    i++;
        //        //}
        //        //return sbWhereClause.ToString();
        //        return null;
        //    }

    }
}
