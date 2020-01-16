
using System;
using System.Collections.Generic;
using System.Linq;
using CellularAutomata;
using System.Drawing;
using SeedGrowth.Model;
using System.Threading.Tasks;

namespace SeedGrowth
{
    class SeedGrowth : CellularAutomata2D
    {
        delegate Seed getSeedState(int x, int y);
        public event EventHandler<Color[,]> OnGrainChange;
        private getSeedState getSeedStateDelegate;
        Random random = new Random(DateTime.Now.Millisecond);
        private Seed[,] seeds;
        private Seed[,] seedsWithBoundaries;
        public int _activationThreshold;
        private bool useGBCFeature;
        private Guid inclusionId = Guid.NewGuid();
        private Guid phaseId = Guid.NewGuid();
        private Guid dualPhaseId = Guid.NewGuid();
        private Dictionary<Guid, Color> grainMap = new Dictionary<Guid, Color>();

        public SeedGrowth(int N, int M) : base(N, M)
        {
            grainMap.Add(Guid.Empty, Color.Black);
            grainMap.Add(inclusionId, Color.White);
            this.getSeedStateDelegate = this.getCellstate;
            seeds = new Seed[N, M];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < M; j++)
                    seeds[i, j] = new Seed(i, j, CellState.dead);

            this.OnIterationComplette += SeedGrowth_OnIterationComplette;
            this.computeNextIterationDelegate = this.computeNextIteration;
        }

        public void removeGrain(int x, int y)
        {
            var grainId = seeds[x, y].GrainId;
            if (grainId != Guid.Empty)
            {
                for (int i = 0; i < Ibound + 1; i++)
                    for (int j = 0; j < Jbound + 1; j++)
                    {
                        if (seeds[i, j].GrainId == grainId)
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

        public void useDP(bool cond)
        {
            getSeedStateDelegate = this.getCellStateDualPhase;
        }

        public void useGBC(bool cond)
        {
            if (cond)
            {

                useGBCFeature = true;
                getSeedStateDelegate = this.getCellStateGBC;
            }
            else
            {
                getSeedStateDelegate = this.getCellstate;
                useGBCFeature = false;
            }
        }

        public void init()
        {
            Color[,] colors = new Color[Cells.GetUpperBound(0) + 1, Cells.GetUpperBound(1) + 1];
            foreach (var s in seeds)
            {
                colors[s.XCordinate, s.YCordinate] = grainMap[seeds[s.XCordinate, s.YCordinate].GrainId];
            }
            OnGrainChange?.Invoke(this, colors);
        }

        private void SeedGrowth_OnIterationComplette(object sender, Cell[,] e)
        {
            Color[,] colors = new Color[Ibound + 1, Jbound + 1];
            Parallel.For(0, Ibound + 1, index =>
            {
                for (int j = 0; j < Jbound + 1; j++)
                {
                    var seed = seeds[index, j];
                    int x = seed.XCordinate;
                    int y = seed.YCordinate;
                    colors[x, y] = grainMap[seed.GrainId];
                }
            });
            //foreach (var seed in seeds)
            //{
            //    colors[seed.XCordinate, seed.YCordinate] = grainMap[seed.GrainId];
            //}

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

        private void computeNextIteration()
        {
            Seed[,] newCells = new Seed[Ibound + 1, Jbound + 1];
            // parallel version
            var length = newCells.GetLength(1);
            Parallel.For(0, Ibound + 1, index =>
            {
                for (int j = 0; j < length; j++)
                {
                    newCells[index, j] = getSeedStateDelegate(index, j);
                }
            });
            //for (int i = 0; i < newCells.GetLength(0); i++)
            //    for (int j = 0; j < newCells.GetLength(1); j++)
            //        newCells[i, j] = this.useGBCFeature ? getCellStateGBC(i, j) : getCellstate(i, j);
            seeds = newCells;
        }
        private Seed[,] createCoopy(Seed[,] seeds)
        {

            Seed[,] newSeeds = new Seed[Ibound + 1, Jbound + 1];
            for (int i = 0; i < Ibound + 1; i++)
                for (int j = 0; j < Jbound + 1; j++)
                {
                    var seed = seeds[i, j];
                    newSeeds[i, j] = new Seed(seed.XCordinate, seed.YCordinate, seed.State)
                    {
                        GrainId = seed.GrainId,
                        PhaseId = seed.PhaseId
                    };
                }
            return newSeeds;
        }
        public void markRemainingStructureAsPhase()
        {
            for (int i = 0; i < Ibound + 1; i++)
                for (int j = 0; j < Jbound + 1; j++)
                {
                    if (seeds[i, j].GrainId != Guid.Empty)
                    {
                        seeds[i, j].GrainId = dualPhaseId;
                    }
                }
            grainMap = new Dictionary<Guid, Color>
            {
                { dualPhaseId, Color.HotPink },
                { Guid.Empty,Color.Black},
                {inclusionId,Color.White }
            };

            this.SeedGrowth_OnIterationComplette(this, Cells);
        }
        public Seed getCellStateGBC(int i, int j)
        {
            if (seeds[i, j].State == CellState.alive)
            {
                return seeds[i, j];
            }

            var r1 = R1(i, j);
            if (r1 != null)
            {
                return r1;
            }
            else
            {
                var r2 = R2(i, j);
                if (r2 != null)
                {
                    return r2;
                }
                else
                {
                    var r3 = R3(i, j);
                    if (r3 != null)
                    {

                        return r3;
                    }
                    else
                    {
                        var r4 = R4(i, j);
                        if (r4 != null)
                        {
                            return r4;
                        }
                        else
                        {
                            return seeds[i, j];
                        }
                    }
                }
            }

        }

        private Seed R1(int i, int j)
        {
            var neighbours = getNeighboursState(seeds, i, j);
            int aliveNeighbours = neighbours.Count(cell => cell.State == CellState.alive);
            if (aliveNeighbours >= 5)
            {
                var grainsOnly = neighbours
                   .Where(s => s.GrainId != Guid.Empty)
                   .Where(s => s.GrainId != inclusionId)
                   .GroupBy(s => s.GrainId).Where(g => g.Count() >= 5);
                if (grainsOnly.Count() > 0)
                {
                    return new Seed(i, j, CellState.alive)
                    {
                        GrainId = grainsOnly.ElementAt(0).Key,
                        PhaseId = phaseId
                    };
                }
            }
            return null;
        }

        private Seed R2(int i, int j)
        {
            var neighbours = getNeighboursMoore(seeds, i, j).Where(c => c.XCordinate == i || c.YCordinate == j);
            int aliveNeighbours = neighbours.Count(cell => cell.State == CellState.alive);
            if (aliveNeighbours >= 3)
            {
                var grainsOnly = neighbours
                   .Where(s => s.GrainId != Guid.Empty)
                   .Where(s => s.GrainId != inclusionId)
                   .GroupBy(s => s.GrainId).Where(g => g.Count() >= 3);
                if (grainsOnly.Count() > 0)
                {
                    return new Seed(i, j, CellState.alive)
                    {
                        GrainId = grainsOnly.ElementAt(0).Key,
                        PhaseId = phaseId
                    };
                }
            }
            return null;
        }

        private Seed R3(int i, int j)
        {
            var neighbours = getNeighboursMoore(seeds, i, j).Where(c => c.XCordinate != i && c.YCordinate != j);
            int aliveNeighbours = neighbours.Count(cell => cell.State == CellState.alive);
            if (aliveNeighbours >= 3)
            {
                var grainsOnly = neighbours
                   .Where(s => s.GrainId != Guid.Empty)
                   .Where(s => s.GrainId != inclusionId)
                   .GroupBy(s => s.GrainId).Where(g => g.Count() >= 3);
                if (grainsOnly.Count() > 0)
                {
                    return new Seed(i, j, CellState.alive)
                    {
                        GrainId = grainsOnly.ElementAt(0).Key,
                        PhaseId = phaseId
                    };
                }
            }
            return null;
        }

        private Seed R4(int i, int j)
        {
            var r = new Random(DateTime.Now.Millisecond);
            var rx = r.Next(1, 100);
            if (rx >= _activationThreshold)
            {
                return this.getCellstate(i, j);
            }
            return null;
        }

        public Seed getCellStateDualPhase(int i, int j)
        {
            if (seeds[i, j].State == CellState.alive)
            {
                return seeds[i, j];
            }
            var neighbours = getNeighboursState(seeds, i, j);
            int aliveNeighbours = neighbours.Count(cell => cell.State == CellState.alive);

            if (seeds[i, j].State == CellState.dead && aliveNeighbours != 0)
            {
                var grainsOnly = neighbours
                    .Where(s => s.GrainId != Guid.Empty)
                    .Where(s => s.GrainId != inclusionId)
                    .Where(s => s.GrainId != dualPhaseId)
                    .GroupBy(s => s.GrainId);

                var orderedGrains = grainsOnly.OrderByDescending(g => g.Count());
                if (orderedGrains.Count() > 0)
                {
                    return new Seed(i, j, CellState.alive)
                    {
                        GrainId = orderedGrains.ElementAt(0).Key,
                        PhaseId = phaseId
                    };
                }
                else
                {
                    return new Seed(i, j, CellState.dead);
                }
            }
            else
                return new Seed(i, j, CellState.dead);
        }

        public Seed getCellstate(int i, int j)
        {
            if (seeds[i, j].State == CellState.alive)
            {
                return seeds[i, j];
            }
            var neighbours = getNeighboursState(seeds, i, j);
            int aliveNeighbours = neighbours.Count(cell => cell.State == CellState.alive);

            if (seeds[i, j].State == CellState.dead && aliveNeighbours != 0)
            {
                var grainsOnly = neighbours
                    .Where(s => s.GrainId != Guid.Empty)
                    .Where(s => s.GrainId != inclusionId)
                    .GroupBy(s => s.GrainId);

                var orderedGrains = grainsOnly.OrderByDescending(g => g.Count());
                if (orderedGrains.Count() > 0)
                {
                    return new Seed(i, j, CellState.alive)
                    {
                        GrainId = orderedGrains.ElementAt(0).Key,
                        PhaseId = phaseId
                    };
                }
                else
                {
                    return new Seed(i, j, CellState.dead);
                }

            }
            else
                return new Seed(i, j, CellState.dead);
        }

        public void setSeed(int x, int y)
        {
            Color color = Color.FromArgb(255, random.Next(1, 254), random.Next(1, 254), random.Next(1, 254));
            var grainId = Guid.NewGuid();
            grainMap.Add(grainId, color);
            seeds[x, y].State = CellState.alive;
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
            seeds[x, y].State = CellState.alive;
            seeds[x, y].GrainId = inclusionId;
            seeds[x, y].PhaseId = inclusionId;
        }

        private void createCircularInclusion(int x, int y, int radius)
        {
            seeds[x, y].State = CellState.alive;
            seeds[x, y].GrainId = inclusionId;
            seeds[x, y].PhaseId = inclusionId;

            for (int i = -radius; i < radius; i++)
            {
                for (int j = -radius; j < radius; j++)
                {
                    if (isInRangeRadius(i, j, radius))
                    {
                        seeds[i + x, j + y].State = CellState.alive;
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
                    if (seeds[i, j].State == CellState.alive)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public int getEdgesLength()
        {
            int c = 0;
            foreach (var s in seedsWithBoundaries)
            {
                if (s.GrainId == Guid.Empty)
                {
                    c++;
                }
            }
            return c / 2;
        }

        public int getGrainMeanSize()
        {
            return ((Ibound + 1) * (Jbound + 1)) / (grainMap.Count - 2);
        }

        public Color[,] findBoundaries()
        {
            seedsWithBoundaries = new Seed[Ibound + 1, Jbound + 1];
            foreach (Seed s in seeds)
            {
                var n = getNeighboursState(seeds, s.XCordinate, s.YCordinate);
                var isOnEdge = n.Any(c => s.GrainId != seeds[c.XCordinate, c.YCordinate].GrainId);
                seedsWithBoundaries[s.XCordinate, s.YCordinate] = new Seed(s.XCordinate, s.YCordinate, s.State) { GrainId = s.GrainId, PhaseId = s.PhaseId };
                if (isOnEdge)
                {
                    seedsWithBoundaries[s.XCordinate, s.YCordinate].GrainId = Guid.Empty;
                }
            }
            Color[,] colors = new Color[Ibound + 1, Jbound + 1];
            foreach (var seed in seedsWithBoundaries)
            {
                colors[seed.XCordinate, seed.YCordinate] = grainMap[seedsWithBoundaries[seed.XCordinate, seed.YCordinate].GrainId];
            }
            return colors;
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
                var cords = getValidCordinatesForSeed();
                setSeed(cords.Item1, cords.Item2);
            }
        }

        private Tuple<int, int> getValidCordinatesForSeed()
        {
            int x, y;
            do
            {
                x = random.Next(0, Ibound);
                y = random.Next(0, Jbound);
            } while (seeds[x, y].State != CellState.dead && seeds[x, y].GrainId != Guid.Empty);

            return new Tuple<int, int>(x, y);
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

