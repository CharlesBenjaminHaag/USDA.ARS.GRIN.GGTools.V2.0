﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class ExplorationMap: AppEntityBase
    {
        public int ExplorationID { get; set; }
        public int CooperatorID { get; set; }
        public string CooperatorName { get; set; }
    }
}
