using System;
using System.Collections.Generic;
using System.Drawing;

namespace SeedGrowth.Model
{
    [Serializable]
    class SeedGrowthDo
    {
        public Bitmap bitmap { get; set; }
        public Dictionary<Guid, Color> grainMap { get; set; }
        public Seed[,] seeds { get; set; }
        public Cell[,] cells { get; set; }
    }
}
