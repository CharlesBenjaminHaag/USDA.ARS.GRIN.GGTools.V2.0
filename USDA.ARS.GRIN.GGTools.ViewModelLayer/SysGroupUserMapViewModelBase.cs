using System;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class SysGroupUserMapViewModelBase : AppViewModelBase
    {
        private SysGroupUserMap _Entity = new SysGroupUserMap();
        private SysGroupUserMapSearch _SearchEntity = new SysGroupUserMapSearch();
        private Collection<SysGroupUserMap> _DataCollection = new Collection<SysGroupUserMap>();

        public SysGroupUserMap Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public SysGroupUserMapSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<SysGroupUserMap> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
    }
}
