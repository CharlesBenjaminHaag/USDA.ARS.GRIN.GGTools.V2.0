using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;

namespace USDA.ARS.GRIN.GGTools.GOBS.DataLayer
{
    public class GOBSManager : GRINGlobalDataManagerBase
    {
        
    
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(GOBSDataset entity)
        {
            throw new NotImplementedException();
        }

        public GOBSDataset Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public int Insert(GOBSDataset entity)
        {
            throw new NotImplementedException();
        }

        public List<GOBSDataset> Search(GOBSDatasetSearch searchEntity)
        {
            SQL = "usp_GRINGlobal_GOBSDataset_Search";
            List<GOBSDataset> GOBSDatasets = new List<GOBSDataset>();

           
            
            return GOBSDatasets;
        }

        public int Update(GOBSDataset entity)
        {
            

            return RowsAffected;
        }

        
    }
}
