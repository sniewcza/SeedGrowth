using System;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SeedGrowth.Utils
{
    class SeedGrowthConverter
    {
        public static Bitmap ConvertToBitmap(Color[,] seedGrowthState)
        {
            int iBound = seedGrowthState.GetUpperBound(0);
            int jBound = seedGrowthState.GetUpperBound(1);
            int width = jBound + 1;
            int height = iBound + 1;
            Bitmap bitmap = new Bitmap(width, height);
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr ptr = bitmapData.Scan0;
            Parallel.For(0, iBound, index =>
            {
                Color color;
                for (int j = 0; j <= jBound; j++)
                {
                    int offset = index * 4 + j * bitmapData.Stride;
                    color = seedGrowthState[index, j];
                    Marshal.WriteByte(ptr, offset, color.R);
                    Marshal.WriteByte(ptr, offset + 1, color.G);
                    Marshal.WriteByte(ptr, offset + 2, color.B);
                    Marshal.WriteByte(ptr, offset + 3, color.A);
                }
            });
            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }


    }
}
