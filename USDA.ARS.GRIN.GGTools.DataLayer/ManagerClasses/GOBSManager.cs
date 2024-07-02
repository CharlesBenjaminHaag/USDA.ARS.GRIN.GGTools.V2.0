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
            GOBSDataset gobsDataset = new GOBSDataset();
            SQL = "SELECT * FROM gobs.get_dataset WHERE dataset_id = @EntityID" ;

            var parameters = new List<IDbDataParameter> {
                CreateParameter("EntityID", (object)entityId, false)
            };

            gobsDataset = GetRecord<GOBSDataset>(SQL, CommandType.Text, parameters.ToArray());

            return gobsDataset;
        }

        public GOBSDatasetAttachment GetDatasetAttachment(int entityId)
        {
            GOBSDatasetAttachment entity = new GOBSDatasetAttachment();
            SQL = "SELECT * FROM gobs.get_dataset_attach WHERE dataset_attach_id = @EntityID";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("EntityID", (object)entityId, false)
            };

            entity = GetRecord<GOBSDatasetAttachment>(SQL, CommandType.Text, parameters.ToArray());

            return entity;
        }

        public GOBSDatasetField GetDatasetField(int entityId)
        {
            GOBSDatasetField entity = new GOBSDatasetField();
            SQL = "SELECT * FROM gobs.get_dataset_field WHERE dataset_field_id = @EntityID";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("EntityID", (object)entityId, false)
            };

            entity = GetRecord<GOBSDatasetField>(SQL, CommandType.Text, parameters.ToArray());

            return entity;
        }

        public GOBSDatasetInventory GetDatasetInventory(int entityId)
        {
            GOBSDatasetInventory entity = new GOBSDatasetInventory();
            SQL = "SELECT * FROM gobs.get_dataset_inventory WHERE dataset_inventory_id = @EntityID";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("EntityID", (object)entityId, false)
            };

            entity = GetRecord<GOBSDatasetInventory>(SQL, CommandType.Text, parameters.ToArray());

            return entity;
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
