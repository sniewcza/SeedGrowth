using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    class LifeSimulator : TheGameOfLive
    {
        private Automat2D _automata;
        private Action _onLifeCycle;
        private int _width;
        private int _height;
        private Rectangle _rectangle;
        public LifeSimulator(int width, int height, Action onLifeCycle)
        {
            _width = width;
            _height = height;
            _automata = new Automat2D(width, height);
            _rectangle = new Rectangle(0,0,width,height);
            _onLifeCycle = onLifeCycle;
        }

        public Bitmap GetGameSnapshoot()
        {
           
            Bitmap bitmap = new Bitmap(_width, _height);

            // wersja rownoległa
            var bitmapData = bitmap.LockBits(_rectangle, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            
            IntPtr ptr = bitmapData.Scan0;
            Parallel.For(0, _automata.Ibound, index =>
            {

                Color color;

                for (int j = 0; j <= _automata.Jbound; j++)
                {

                    int offset = index * 4 + j * bitmapData.Stride;
                    color = (_automata.Cells[index, j] == 1) ? Color.White : Color.Black;
                    Marshal.WriteByte(ptr, offset, color.R);
                    Marshal.WriteByte(ptr, offset + 1, color.G);
                    Marshal.WriteByte(ptr, offset + 2, color.B);
                    Marshal.WriteByte(ptr, offset + 3, color.A);


                }
            });
            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }

        public void SetStructure(int x, int y, structure structureType)
        {
            _automata.setStructure(structureType, x, y);
        }

        public void Start()
        {
            _automata.Iterate(_onLifeCycle, neigbhourhoodType.MoorePeriodic);
        }

        public void Stop()
        {
            _automata.stop();
        }
    }
}
