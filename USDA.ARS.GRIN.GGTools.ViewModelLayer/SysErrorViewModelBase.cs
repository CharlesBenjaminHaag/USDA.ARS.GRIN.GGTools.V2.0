using System;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class SysErrorViewModelBase :AppViewModelBase
    {
        private SysDBError _Entity = new SysDBError();
        private SysDBErrorSearch _SearchEntity = new SysDBErrorSearch();
        private Collection<SysDBError> _DataCollectionSysDBErrors = new Collection<SysDBError>();
        private Collection<SysAppError> _DataCollectionSysAppErrors = new Collection<SysAppError>();

        public SysDBError Entity
        {
            get { return this._Entity; }
            set { this._Entity = value; }
        }

        public SysDBErrorSearch SearchEntity
        {
            get { return this._SearchEntity; }
            set { this._SearchEntity = value; }
        }
        public Collection<SysDBError> DataCollectionSysDBErrors
        {
            get { return this._DataCollectionSysDBErrors; }
            set { this._DataCollectionSysDBErrors = value; }
        }
        public Collection<SysAppError> DataCollectionSysAppErrors
        {
            get { return this._DataCollectionSysAppErrors; }
            set { this._DataCollectionSysAppErrors = value; }
        }

    }
}
