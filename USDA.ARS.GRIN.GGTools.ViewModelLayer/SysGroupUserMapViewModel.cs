using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class SysGroupUserMapViewModel : SysGroupUserMapViewModelBase, IViewModel<SysGroupUserMap>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public SysGroupUserMap Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public void GetBySysUser(int sysUserId, string isAvailable)
        {
            using (SysGroupUserMapManager mgr = new SysGroupUserMapManager())
            {
                if (isAvailable == "Y")
                {
                  DataCollectionAvailable = new Collection<SysGroupUserMap>(mgr.GetAvailable(sysUserId));
                }
                else
                {
                  DataCollectionUnavailable = new Collection<SysGroupUserMap>(mgr.GetUnavailable(sysUserId));
                }
            }
        }

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            throw new NotImplementedException();
        }

        public void Search()
        {
            using (SysGroupUserMapManager mgr = new SysGroupUserMapManager())
            {
                try
                {
                    DataCollection = new Collection<SysGroupUserMap>(mgr.Search(SearchEntity));
                    if (DataCollection.Count() == 1)
                    {
                        Entity = DataCollection[0];
                    }

                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public int Update()
        {
            throw new NotImplementedException();
        }
    }
}
