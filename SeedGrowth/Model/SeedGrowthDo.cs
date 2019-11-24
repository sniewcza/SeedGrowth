using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedGrowth.Model
{
    [Serializable]
    class SeedGrowthDo
    {
        public Bitmap bitmap { get; set; }
        public Dictionary<Guid, Color> grainMap { get; set; }
        public Seed[,] seeds { get; set; }
    }
}
