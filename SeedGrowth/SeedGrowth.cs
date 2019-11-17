
using System;
using System.Collections.Generic;
using System.Linq;
using CellularAutomata;
using System.Drawing;

namespace SeedGrowth
{
    class SeedGrowth : CellularAutomata2D
    {
        private List<Color> colors = new List<Color>() { Color.FromKnownColor(KnownColor.Coral) };
        Random random = new Random(DateTime.Now.Millisecond);

        public SeedGrowth(int N, int M) : base(N, M)
        {
            for (int i = 0; i < N; i++)
                for (int j = 0; j < M; j++)
                    Cells[i, j] = Color.Black.ToArgb();
        }

        public override int getCellstate(int i, int j)
        {

            List<int> neighbours = getNeighboursState(i, j);
            int aliveNeighbours = neighbours.Count(color => color != Color.Black.ToArgb());
            if (Cells[i, j] == Color.Black.ToArgb() && aliveNeighbours != 0)
            {
                int dominantColorcount = 0;
                Color dominantColor = Color.FromArgb(Cells[i, j]);
                for (int k = 0; k < colors.Count; k++)
                {
                    int colorcount = neighbours.Count(color => color == colors[k].ToArgb());
                    if (dominantColorcount < colorcount)
                    {
                        dominantColor = colors[k];
                        dominantColorcount = colorcount;
                    }
                }
                return dominantColor.ToArgb();
            }
            else
                return Cells[i, j];
        }
        public void setSeed(int x, int y)
        {
            Color color = Color.FromArgb(255, random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
            colors.Add(color);
            Cells[x, y] = colors[colors.Count - 1].ToArgb();
        }

        public void setInclusions(int numberOfInclusions, int radius)
        {
            for (int i = 0; i < numberOfInclusions; i++)
            {
                int x = random.Next(radius, Ibound - radius);
                int y = random.Next(radius, Jbound - radius);
                if (enoughSpaceForInclusion(x, y, radius))
                {
                    setInclusion(x, y, radius);
                }

            }
        }


        private void setInclusion(int x, int y, int radius)
        {
            int argbWhite = Color.White.ToArgb();
            Cells[x, y] = argbWhite;
            createCircularInclusion(x, y, radius, Color.White);
        }

        private void createCircularInclusion(int x, int y, int radius, Color color)
        {
            for (int i = -radius; i < radius; i++)
            {
                for (int j = -radius; j < radius; j++)
                {
                    if (isInRangeRadius(i,j,radius))
                    {
                        Cells[i+x, j+y] = Color.White.ToArgb();
                    }
                    
                }
            }
        }

        private bool isInRangeRadius(int x, int y, int radius)
        {
            return (x * x + y * y) <= radius * radius;
        }
        private bool enoughSpaceForInclusion(int x, int y, int radius)
        {
            int blackAGB = Color.Black.ToArgb();
            for (int i = x - radius; i < x + radius; i++)
            {
                for (int j = y - radius; j < y + radius; j++)
                {
                    if (Cells[i, j] != blackAGB)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public void setSeedsEvenly(int XaxisSeeds, int YaxisSeeds)
        {
            int dx = Convert.ToInt32(Math.Round(Ibound / (double)(XaxisSeeds + 1)));
            int dy = Convert.ToInt32(Math.Round(Jbound / (double)(YaxisSeeds + 1)));
            int tmpdx;
            int tmpdy;
            for (int i = 1; i <= XaxisSeeds; i++)
            {
                if (i == 1)
                    tmpdx = Convert.ToInt32(dx / 2.0);
                else if (i == XaxisSeeds)
                    tmpdx = Convert.ToInt32(i * dx + (dx / 2.0));
                else
                    tmpdx = i * dx;
                for (int j = 1; j <= YaxisSeeds; j++)
                {
                    if (j == 1)
                        tmpdy = Convert.ToInt32(dy / 2.0);
                    else if (j == YaxisSeeds)
                        tmpdy = Convert.ToInt32(j * dy + (dy / 2.0));
                    else
                        tmpdy = j * dy;
                    setSeed(tmpdx, tmpdy);
                }
            }
        }

        public void setSeedsRandomly(int NumberofSeeds)
        {
            for (int i = 0; i < NumberofSeeds; i++)
            {
                setSeed(random.Next(0, Ibound), random.Next(0, Jbound));
            }
        }

        public void setSeedswithRadius(int NumberofSeeds, int Radius)
        {
            if (Math.Ceiling((Ibound + Jbound) / (double)Radius) < NumberofSeeds)
                throw new Exception("Decrease radius or number of seeds");

            Random random = new Random(DateTime.Now.Second);
            List<Point> seeds = new List<Point>
            {
                new Point(random.Next(0, Ibound), random.Next(0, Jbound))
            };

            int i = 0;
            while (i < NumberofSeeds - 1)
            {
                bool badSeed = false;
                Point tmp = new Point(random.Next(0, Ibound), random.Next(0, Jbound));
                foreach (Point p in seeds)
                    if (Radius > getSeedsDistance(p, tmp))
                    {
                        badSeed = true;
                    }
                if (!badSeed)
                {
                    seeds.Add(tmp);
                    i++;
                }
            }

            foreach (Point p in seeds)
                setSeed(p.X, p.Y);
        }

        private bool areFreeCells()
        {
            foreach (int c in Cells)
                if (c == Color.Black.ToArgb())
                    return true;
            return false;
        }

        private double getSeedsDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }
    }
}

