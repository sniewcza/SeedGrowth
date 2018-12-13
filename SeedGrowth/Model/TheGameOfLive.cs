using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    public interface TheGameOfLive
    {
        void Start();
        void Stop();
        void SetStructure(int x, int y, structure structureType);
        Bitmap GetGameSnapshoot();
    }
}
