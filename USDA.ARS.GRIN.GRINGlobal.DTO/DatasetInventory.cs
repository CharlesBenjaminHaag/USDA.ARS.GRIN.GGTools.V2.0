﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USDA.ARS.GRIN.GRINGlobal.DTO
{
    public class DatasetInventory
    {
        public int dataset_inventory_id { get; set; }
        public int method_id { get; set; }
        public string method { get; set; }
        public int dataset_id { get; set; }
        public string dataset { get; set; }
        public int inventory_id { get; set; }
        public string inventory { get; set; }
        public DateTime valid_from { get; set; }
    }
}
