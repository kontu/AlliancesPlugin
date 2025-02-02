﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlliancesPlugin.Shipyard
{
    public class ShipyardBlockConfig
    {
        public void SetShipyardBlockConfig(String input)
        {
            SubtypeId = input;
        }
        public Boolean enabled = true;
        public int MaximumBlockSize = 10000;
        public string SubtypeId;
        public float SecondsPerBlock = 1f;
        public int SCPerBlock = 5000;
        public string FuelTypeId = "MyObjectBuilder_Ingot";
        public string FuelSubTypeId = "Uranium";
        public float SecondsPerInterval = 10;
        public int FuelPerInterval = 1;
    }
}
