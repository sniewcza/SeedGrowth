
using System;
using System.Collections.Generic;
using System.Linq;
using CellularAutomata;
using System.Drawing;

namespace SeedGrowth
{
    class SeedGrowth : CellularAutomata2D
    {
        //private List<Color> colors = new List<Color>() { Color.FromKnownColor(KnownColor.Coral) };
        public event EventHandler<Color[,]> onGrainChange;
        Random random = new Random(DateTime.Now.Millisecond);
        private Seed[,] seeds;
        private Guid inclusionId = Guid.NewGuid();
        private Dictionary<Guid, Color> grainMap = new Dictionary<Guid, Color>();



        public SeedGrowth(int N, int M) : base(N, M)
        {
            grainMap.Add(Guid.Empty, Color.Black);
            grainMap.Add(inclusionId, Color.White);
            seeds = new Seed[N, M];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < M; j++)
                {
                    seeds[i, j] = new Seed(i, j, CellState.dead);
                    //colors[i, j] = Color.Black;
                }
            this.OnIterationComplette += SeedGrowth_OnIterationComplette;
        }

        private void SeedGrowth_OnIterationComplette(object sender, Cell[,] e)
        {
            Color[,] colors = new Color[e.GetUpperBound(0) + 1, e.GetUpperBound(1) + 1];
            foreach (var cell in e)
            {
                colors[cell.XCordinate, cell.YCordinate] = grainMap[seeds[cell.XCordinate, cell.YCordinate].GrainId];
            }


            this.onGrainChange?.Invoke(this, colors);
        }

        private List<Seed> toSeeds(List<Cell> cells)
        {
            List<Seed> seeds = new List<Seed>();
            foreach (var cell in cells)
            {
                seeds.Add(this.seeds[cell.XCordinate, cell.YCordinate]);
            }

            return seeds;
        }


        public override CellState getCellstate(int i, int j)
        {
            if (Cells[i, j].State == CellState.alive)
            {
                return CellState.alive;
            }
            List<Cell> neighbours = getNeighboursState(i, j);
            int aliveNeighbours = neighbours.Count(cell => cell.State == CellState.alive);

            if (Cells[i, j].State == CellState.dead && aliveNeighbours != 0)
            {
                var neigbourSeeds = toSeeds(neighbours);
                var grainsOnly = neigbourSeeds.Where(s => s.GrainId != Guid.Empty).Where(s => s.GrainId != inclusionId).GroupBy(s => s.GrainId);
                var orderedGrains = grainsOnly.OrderByDescending(g => g.Count());
                if (orderedGrains.Count() > 0)
                {
                    seeds[i, j].GrainId = orderedGrains.ElementAt(0).Key;
                    return CellState.alive;
                }
                else
                {
                    return CellState.dead;
                }

            }
            else
                return CellState.dead;
        }
        public void setSeed(int x, int y)
        {
            Color color = Color.FromArgb(255, random.Next(1, 254), random.Next(1, 254), random.Next(1, 254));
            var grainId = Guid.NewGuid();
            grainMap.Add(grainId, color);
            Cells[x, y].State = CellState.alive;
            seeds[x, y].GrainId = grainId;
        }

        public void setInclusions(int numberOfInclusions, int minRadius, int maxRadius)
        {
            for (int i = 0; i < numberOfInclusions; i++)
            {
                int r = random.Next(minRadius, maxRadius);
                int x = random.Next(r, Ibound - r);
                int y = random.Next(r, Jbound - r);
                if (enoughSpaceForInclusion(x, y, r))
                {
                    setInclusion(x, y, r);
                }
            }
        }


        private void setInclusion(int x, int y, int radius)
        {
            // int argbWhite = Color.White;
            Cells[x, y].State = CellState.alive;
            seeds[x, y].GrainId = inclusionId;
            createCircularInclusion(x, y, radius, Color.White);
        }

        private void createCircularInclusion(int x, int y, int radius, Color color)
        {
            for (int i = -radius; i < radius; i++)
            {
                for (int j = -radius; j < radius; j++)
                {
                    if (isInRangeRadius(i, j, radius))
                    {
                        Cells[i + x, j + y].State = CellState.alive;
                        seeds[i + x, j + y].GrainId = inclusionId;
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
            //int blackAGB = Color.Black.ToArgb();
            for (int i = x - radius; i < x + radius; i++)
            {
                for (int j = y - radius; j < y + radius; j++)
                {
                    if (Cells[i, j].State == CellState.alive)
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

        //private bool areFreeCells()
        //{
        //    foreach (int c in Cells)
        //        if (c == Color.Black.ToArgb())
        //            return true;
        //    return false;
        //}

        private double getSeedsDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }
    }
}

