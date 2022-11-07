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
    public class SysTableViewModel : SysTableViewModelBase, IViewModel<SysTable>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public SysTable Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public void GetPermissions()
        {
            using (SysPermissionManager mgr = new SysPermissionManager())
            {
                DataCollectionPermissions = new Collection<SysPermission>(mgr.Search(new SysPermissionSearch { TableName = SearchEntity.TableName }));
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
            throw new NotImplementedException();
        }

        public int Update()
        {
            throw new NotImplementedException();
        }
    }
}
