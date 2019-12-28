
using System;
using System.Collections.Generic;
using System.Linq;
using CellularAutomata;
using System.Drawing;
using SeedGrowth.Model;

namespace SeedGrowth
{
    class SeedGrowth : CellularAutomata2D
    {
        public event EventHandler<Color[,]> OnGrainChange;
        Random random = new Random(DateTime.Now.Millisecond);
        private Seed[,] seeds;
        public int _activationThreshold;
        private Guid inclusionId = Guid.NewGuid();
        private Guid phaseId = Guid.NewGuid();
        private Dictionary<Guid, Color> grainMap = new Dictionary<Guid, Color>();

        public SeedGrowth(int N, int M) : base(N, M)
        {
            grainMap.Add(Guid.Empty, Color.Black);
            grainMap.Add(inclusionId, Color.White);
            this.getCellStateDelegate = getCellstate;
            seeds = new Seed[N, M];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < M; j++)
                    seeds[i, j] = new Seed(i, j, CellState.dead);

            this.OnIterationComplette += SeedGrowth_OnIterationComplette;
        }

        public void removeGrain(int x, int y)
        {
            var grainId = seeds[x, y].GrainId;
            if(grainId != Guid.Empty)
            { 
                for(int i =0;i<Ibound;i++)
                    for(int j = 0; j < Jbound; j++)
                    {
                        if(seeds[i,j].GrainId == grainId)
                        {
                            Cells[i, j].State = CellState.dead;
                            seeds[i, j].State = CellState.dead;
                            seeds[i, j].GrainId = Guid.Empty;
                        }
                    }
                grainMap.Remove(grainId);
                this.SeedGrowth_OnIterationComplette(this, Cells);
            }
        }
        public void useGBC(bool cond)
        {
            if (cond)
            {
                this.getCellStateDelegate = this.getCellStateGBC;
            }
            else
            {
                this.getCellStateDelegate = this.getCellstate;
            }
        }
        private void SeedGrowth_OnIterationComplette(object sender, Cell[,] e)
        {
            Color[,] colors = new Color[e.GetUpperBound(0) + 1, e.GetUpperBound(1) + 1];
            foreach (var cell in e)
            {
                colors[cell.XCordinate, cell.YCordinate] = grainMap[seeds[cell.XCordinate, cell.YCordinate].GrainId];
            }

            OnGrainChange?.Invoke(this, colors);
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

        public CellState getCellStateGBC(int i, int j)
        {
            if (Cells[i, j].State == CellState.alive)
            {
                return CellState.alive;
            }

            var r1 = R1(i, j);
            if (r1 == true)
            {
                return CellState.alive;
            }
            else
            {
                var r2 = R2(i, j);
                if (r2 == true)
                {
                    return CellState.alive;
                }
                else
                {
                    var r3 = R3(i, j);
                    if (r3 == true)
                    {
                        return CellState.alive;
                    }
                    else
                    {
                        return R4(i, j);
                    }
                }
            }
        }

        private bool R1(int i, int j)
        {
            List<Cell> neighbours = getNeighboursState(i, j);
            int aliveNeighbours = neighbours.Count(cell => cell.State == CellState.alive);
            if (aliveNeighbours >= 5)
            {
                var neigbourSeeds = toSeeds(neighbours);
                var grainsOnly = neigbourSeeds
                   .Where(s => s.GrainId != Guid.Empty)
                   .Where(s => s.GrainId != inclusionId)
                   .GroupBy(s => s.GrainId).Where(g => g.Count() >= 5);
                if (grainsOnly.Count() > 0)
                {
                    seeds[i, j].GrainId = grainsOnly.ElementAt(0).Key;
                    seeds[i, j].PhaseId = phaseId;
                    return true;
                }
            }
            return false;
        }

        private bool R2(int i, int j)
        {
            List<Cell> neighbours = getNeighboursNearestMoorePeriodic(i, j);
            int aliveNeighbours = neighbours.Count(cell => cell.State == CellState.alive);
            if (aliveNeighbours >= 3)
            {
                var neigbourSeeds = toSeeds(neighbours);
                var grainsOnly = neigbourSeeds
                   .Where(s => s.GrainId != Guid.Empty)
                   .Where(s => s.GrainId != inclusionId)
                   .GroupBy(s => s.GrainId).Where(g => g.Count() >= 3);
                if (grainsOnly.Count() > 0)
                {
                    seeds[i, j].GrainId = grainsOnly.ElementAt(0).Key;
                    seeds[i, j].PhaseId = phaseId;
                    return true;
                }
            }
            return false;
        }

        private bool R3(int i, int j)
        {
            List<Cell> neighbours = getNeighboursFourtherMoorePeriodic(i, j);
            int aliveNeighbours = neighbours.Count(cell => cell.State == CellState.alive);
            if (aliveNeighbours >= 3)
            {
                var neigbourSeeds = toSeeds(neighbours);
                var grainsOnly = neigbourSeeds
                   .Where(s => s.GrainId != Guid.Empty)
                   .Where(s => s.GrainId != inclusionId)
                   .GroupBy(s => s.GrainId).Where(g => g.Count() >= 3);
                if (grainsOnly.Count() > 0)
                {
                    seeds[i, j].GrainId = grainsOnly.ElementAt(0).Key;
                    seeds[i, j].PhaseId = phaseId;
                    return true;
                }
            }
            return false;
        }
        private CellState R4(int i, int j)
        {
            var r = new Random(DateTime.Now.Millisecond);
            var rx = r.Next(1, 100);
            if (rx >= _activationThreshold)
            {
                return this.getCellstate(i, j);

            }
            return CellState.dead;
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
                var grainsOnly = neigbourSeeds
                    .Where(s => s.GrainId != Guid.Empty)
                    .Where(s => s.GrainId != inclusionId)
                    .GroupBy(s => s.GrainId);

                var orderedGrains = grainsOnly.OrderByDescending(g => g.Count());
                if (orderedGrains.Count() > 0)
                {
                    seeds[i, j].GrainId = orderedGrains.ElementAt(0).Key;
                    seeds[i, j].PhaseId = phaseId;
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
            seeds[x, y].PhaseId = phaseId;
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
                    createCircularInclusion(x, y, r);
                }
            }
        }

        private void setInclusion(int x, int y, int radius)
        {
            Cells[x, y].State = CellState.alive;
            seeds[x, y].GrainId = inclusionId;
            seeds[x, y].PhaseId = inclusionId;
        }

        private void createCircularInclusion(int x, int y, int radius)
        {
            Cells[x, y].State = CellState.alive;
            seeds[x, y].GrainId = inclusionId;
            seeds[x, y].PhaseId = inclusionId;

            for (int i = -radius; i < radius; i++)
            {
                for (int j = -radius; j < radius; j++)
                {
                    if (isInRangeRadius(i, j, radius))
                    {
                        Cells[i + x, j + y].State = CellState.alive;
                        seeds[i + x, j + y].GrainId = inclusionId;
                        seeds[i + x, j + y].PhaseId = inclusionId;
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

        private double getSeedsDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        public SeedGrowthDo getSerialziableData()
        {
            return new SeedGrowthDo
            {
                grainMap = this.grainMap,
                seeds = this.seeds,
                cells = this.Cells
            };
        }

        public void setSeeds(Seed[,] seeds)
        {
            this.seeds = seeds;
        }

        public void setCells(Cell[,] cells)
        {
            this.Cells = cells;
        }
        public void setGrainMap(Dictionary<Guid, Color> grainMap)
        {
            this.grainMap = grainMap;
        }

        public string getSeedInfoAtPosition(int x, int y)
        {
            var seed = seeds[x, y];
            return $"grain Id: {seed.GrainId} \nphase Id: {seed.PhaseId}";
        }
    }
}

