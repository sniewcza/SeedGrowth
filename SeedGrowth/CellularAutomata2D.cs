using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CellularAutomata
{
    enum Structure
    {
        Immutable,
        Oscilator,
        Glider,
        Dakota
    }
    enum NeigbhourhoodType
    {
        Moore,
        MoorePeriodic,
        VonNoyman,
        VonNoymanPeriodic,
        HexagonalRandom,
        HexagonalRandomPeriodic,
        PentagonalRandom,
        PentagonalRandomPeriodic
    }

    delegate List<int> getNeighbours(int i, int j);

    class CellularAutomata2D
    {
        public event EventHandler<int[,]> OnIterationComplette;

        private int[,] cells;
        private readonly int ibound;
        private readonly int jbound;
        private volatile bool isWorking = false;
        protected getNeighbours getNeighboursState;
        private Random random = new Random(DateTime.Now.Millisecond);

        public CellularAutomata2D(int N, int M)
        {
            cells = new int[N, M];
            ibound = cells.GetUpperBound(0);
            jbound = cells.GetUpperBound(1);
        }

        public void setNeighbourhoodType(NeigbhourhoodType type)
        {
            getNeighboursState = getNeighboursDelegate(type);
        }

        public void Stop()
        {
            isWorking = false;
        }

        public void setStructure(Structure structure)
        {
            int xmid = ibound / 2;
            int ymid = jbound / 2;

            switch (structure)
            {
                case Structure.Immutable:
                    cells[xmid, ymid] = 1;
                    cells[xmid, ymid + 1] = 1;
                    cells[xmid + 1, ymid - 1] = 1;
                    cells[xmid + 1, ymid + 2] = 1;
                    cells[xmid + 2, ymid] = 1;
                    cells[xmid + 2, ymid + 1] = 1;
                    break;
                case Structure.Oscilator:
                    cells[xmid, ++ymid] = 1;
                    cells[xmid, ++ymid] = 1;
                    cells[xmid, ++ymid] = 1;
                    break;
                case Structure.Glider:
                    cells[xmid, ymid] = 1;
                    cells[xmid + 1, ymid] = 1;
                    cells[xmid + 2, ymid] = 1;
                    cells[xmid, ymid + 1] = 1;
                    cells[xmid + 1, ymid + 2] = 1;
                    break;
                case Structure.Dakota:
                    cells[xmid, ymid] = 1;
                    cells[xmid, ymid - 1] = 1;
                    cells[xmid, ymid - 2] = 1;
                    cells[xmid + 1, ymid] = 1;
                    cells[xmid + 2, ymid] = 1;
                    cells[xmid + 3, ymid] = 1;
                    cells[xmid + 1, ymid - 3] = 1;
                    cells[xmid + 4, ymid - 1] = 1;
                    cells[xmid + 4, ymid - 3] = 1;
                    break;
            }
        }

        public void setStructure(Structure structure, int x, int y)
        {
            int xmid = x;
            int ymid = y;

            switch (structure)
            {
                case Structure.Immutable:
                    cells[xmid, ymid] = 1;
                    cells[xmid, ymid + 1] = 1;
                    cells[xmid + 1, ymid - 1] = 1;
                    cells[xmid + 1, ymid + 2] = 1;
                    cells[xmid + 2, ymid] = 1;
                    cells[xmid + 2, ymid + 1] = 1;
                    break;
                case Structure.Oscilator:
                    cells[xmid, ++ymid] = 1;
                    cells[xmid, ++ymid] = 1;
                    cells[xmid, ++ymid] = 1;
                    break;
                case Structure.Glider:
                    cells[xmid, ymid] = 1;
                    cells[xmid + 1, ymid] = 1;
                    cells[xmid + 2, ymid] = 1;
                    cells[xmid, ymid + 1] = 1;
                    cells[xmid + 1, ymid + 2] = 1;
                    break;
                case Structure.Dakota:
                    cells[xmid, ymid] = 1;
                    cells[xmid, ymid - 1] = 1;
                    cells[xmid, ymid - 2] = 1;
                    cells[xmid + 1, ymid] = 1;
                    cells[xmid + 2, ymid] = 1;
                    cells[xmid + 3, ymid] = 1;
                    cells[xmid + 1, ymid - 3] = 1;
                    cells[xmid + 4, ymid - 1] = 1;
                    cells[xmid + 4, ymid - 3] = 1;
                    break;
            }
        }

        private getNeighbours getNeighboursDelegate(NeigbhourhoodType type)
        {
            switch (type)
            {
                case NeigbhourhoodType.MoorePeriodic:
                    return new getNeighbours(getNeighboursMoorePeriodic);
                case NeigbhourhoodType.VonNoyman:
                    return new getNeighbours(getNeigboursVonNoyman);
                case NeigbhourhoodType.VonNoymanPeriodic:
                    return new getNeighbours(getNeigboursVonNoymanPeriodic);
                case NeigbhourhoodType.Moore:
                    return new getNeighbours(getNeighboursMoore);
                case NeigbhourhoodType.HexagonalRandom:
                    return new getNeighbours(getNeighboursRandomHexagonal);
                case NeigbhourhoodType.HexagonalRandomPeriodic:
                    return new getNeighbours(getNeighboursRandomHexagonalPeriodic);
                case NeigbhourhoodType.PentagonalRandom:
                    return new getNeighbours(getNeighboursRandomPentagonal);
                case NeigbhourhoodType.PentagonalRandomPeriodic:
                    return new getNeighbours(getNeighboursRandomPentagonalPeriodic);
            }
            return null;
        }

        public void PerformIterationStep()
        {
            ComputeNextIterationCells();
            OnIterationComplette?.Invoke(this, cells);
        }

        public void Start()
        {
            isWorking = true;
            while (isWorking)
            {
                PerformIterationStep();
            }
        }

        private void ComputeNextIterationCells()
        {
            int[,] newCells = new int[cells.GetLength(0), cells.GetLength(1)];
            //parallel version
            var length = newCells.GetLength(1);
            Parallel.For(0, ibound + 1, index =>
               {
                   for (int j = 0; j < length; j++)
                   {
                       newCells[index, j] = getCellstate(index, j);
                   }
               });

            //sequential version
            //for (int i = 0; i < newCells.GetLength(0); i++)
            //    for (int j = 0; j < newCells.GetLength(1); j++)
            //        newCells[i, j] = getCellstate(i, j);

            cells = newCells;
        }

        private List<int> getNeighboursMoorePeriodic(int i, int j)
        {
            List<int> neighbours = new List<int>(8);

            if (i == 0 && j != 0 && j != jbound)
            {
                neighbours.Add(cells[ibound, j]);
                neighbours.Add(cells[ibound, j - 1]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i + 1, j - 1]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i + 1, j + 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[ibound, j + 1]);
            }
            else if (i == ibound && j != 0 && j != jbound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i - 1, j - 1]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[0, j - 1]);
                neighbours.Add(cells[0, j]);
                neighbours.Add(cells[0, j + 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i - 1, j + 1]);

            }
            else if (j == 0 && i != 0 && i != ibound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i - 1, jbound]);
                neighbours.Add(cells[i, jbound]);
                neighbours.Add(cells[i + 1, jbound]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i + 1, j + 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i - 1, j + 1]);
            }
            else if (j == jbound && i != 0 && i != ibound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i - 1, j - 1]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i + 1, j - 1]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i + 1, 0]);
                neighbours.Add(cells[i, 0]);
                neighbours.Add(cells[i - 1, 0]);
            }
            else if (i == 0 && j == 0)
            {
                neighbours.Add(cells[ibound, j]);
                neighbours.Add(cells[ibound, jbound]);
                neighbours.Add(cells[i, jbound]);
                neighbours.Add(cells[i + 1, jbound]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i + 1, j + 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[ibound, j + 1]);

            }
            else if (i == ibound && j == 0)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i - 1, jbound]);
                neighbours.Add(cells[i, jbound]);
                neighbours.Add(cells[0, jbound]);
                neighbours.Add(cells[0, j]);
                neighbours.Add(cells[0, j + 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i - 1, j + 1]);
            }
            else if (i == ibound && j == jbound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i - 1, j - 1]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[0, j - 1]);
                neighbours.Add(cells[0, j]);
                neighbours.Add(cells[0, 0]);
                neighbours.Add(cells[i, 0]);
                neighbours.Add(cells[i - 1, 0]);

            }
            else if (i == 0 && j == jbound)
            {
                neighbours.Add(cells[ibound, j]);
                neighbours.Add(cells[ibound, j - 1]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i + 1, j - 1]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i + 1, 0]);
                neighbours.Add(cells[i, 0]);
                neighbours.Add(cells[ibound, 0]);

            }
            else
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i - 1, j - 1]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i + 1, j - 1]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i + 1, j + 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i - 1, j + 1]);

            }

            return neighbours;
        }

        private List<int> getNeigboursVonNoymanPeriodic(int i, int j)
        {
            List<int> neighbours = new List<int>(4);
            if (i == 0 && j != 0 && j != jbound)
            {
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[ibound, j]);
            }
            else if (i == ibound && j != 0 && j != jbound)
            {
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[0, j]);
            }
            else if (j == 0 && i != 0 && i != ibound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i, jbound]);
            }
            else if (j == jbound && i != 0 && i != ibound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i, 0]);
            }
            else if (i == 0 && j == 0)
            {
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i, jbound]);
                neighbours.Add(cells[ibound, j]);
            }
            else if (i == 0 && j == jbound)
            {
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[0, j]);
                neighbours.Add(cells[ibound, j]);
            }
            else if (i == ibound && j == 0)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[0, j]);
                neighbours.Add(cells[i, jbound]);
            }
            else if (i == ibound && j == jbound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[0, j]);
                neighbours.Add(cells[i, 0]);
            }
            else
            {
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i, j + 1]);
            }

            return neighbours;
        }

        private List<int> getNeigboursVonNoyman(int i, int j)
        {
            List<int> neighbours = new List<int>();

            if (i == 0 && j != 0 && j != jbound)
            {
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i + 1, j]);
            }
            else if (i == ibound && j != 0 && j != jbound)
            {
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i - 1, j]);
            }
            else if (j == 0 && i != 0 && i != ibound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i, j + 1]);
            }
            else if (j == jbound && i != 0 && i != ibound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i, j - 1]);
            }
            else if (i == 0 && j == 0)
            {
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i, j + 1]);
            }
            else if (i == 0 && j == jbound)
            {
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i + 1, j]);
            }
            else if (i == ibound && j == 0)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i, j + 1]);
            }
            else if (i == ibound && j == jbound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i, j - 1]);
            }
            else
            {
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i, j + 1]);
            }

            return neighbours;
        }

        private List<int> getNeighboursMoore(int i, int j)
        {
            List<int> neighbours = new List<int>();

            if (i == 0 && j != 0 && j != jbound)
            {
                neighbours.Add(cells[i, j - 1]);
                for (int k = j - 1; k <= j + 1; k++)
                {
                    neighbours.Add(cells[i + 1, k]);
                }
                neighbours.Add(cells[i, j + 1]);
            }
            else if (i == ibound && j != 0 && j != jbound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i - 1, j - 1]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i - 1, j + 1]);

            }
            else if (j == 0 && i != 0 && i != ibound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i - 1, j + 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i + 1, j + 1]);
            }
            else if (j == jbound && i != 0 && i != ibound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i - 1, j - 1]);
                neighbours.Add(cells[i, j]);
                neighbours.Add(cells[i + 1, j - 1]);
                neighbours.Add(cells[i + 1, j]);
            }
            else if (i == 0 && j == 0)
            {
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i + 1, j + 1]);
                neighbours.Add(cells[i, j + 1]);
            }
            else if (i == ibound && j == 0)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i - 1, j + 1]);

            }
            else if (i == ibound && j == jbound)
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i - 1, j - 1]);
                neighbours.Add(cells[i, j - 1]);
            }
            else if (i == 0 && j == jbound)
            {
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i + 1, j - 1]);
                neighbours.Add(cells[i + 1, j]);
            }
            else
            {
                neighbours.Add(cells[i - 1, j]);
                neighbours.Add(cells[i - 1, j - 1]);
                neighbours.Add(cells[i, j - 1]);
                neighbours.Add(cells[i + 1, j - 1]);
                neighbours.Add(cells[i + 1, j]);
                neighbours.Add(cells[i + 1, j + 1]);
                neighbours.Add(cells[i, j + 1]);
                neighbours.Add(cells[i - 1, j + 1]);
            }

            return neighbours;
        }

        private List<int> getNeighboursRandomPentagonal(int i, int j)
        {
            List<int> neighbours = getNeighboursMoore(i, j);

            if (i == 0 && j != 0 && j != jbound)
            {
                switch (random.Next(0, 4))
                {
                    case 0: // left Pentagon
                        neighbours.RemoveAt(4);
                        neighbours.RemoveAt(3);
                        break;
                    case 1: // right Pentagon
                        neighbours.RemoveAt(1);
                        neighbours.RemoveAt(0);
                        break;
                    case 2: // top Pentagon
                        neighbours.RemoveAt(3);
                        neighbours.RemoveAt(2);
                        neighbours.RemoveAt(1);
                        break;
                    case 3: // bottom Pentagon                       
                        break;
                }
            }
            else if (i == ibound && j != 0 && j != jbound)
            {
                switch (random.Next(0, 4))
                {
                    case 0:
                        neighbours.RemoveAt(4);
                        neighbours.RemoveAt(3);
                        break;
                    case 1:
                        neighbours.RemoveAt(2);
                        neighbours.RemoveAt(1);
                        break;
                    case 2:
                        break;
                    case 3:
                        neighbours.RemoveAt(4);
                        neighbours.RemoveAt(1);
                        neighbours.RemoveAt(0);
                        break;
                }

            }
            else if (j == 0 && i != 0 && i != ibound)
            {
                switch (random.Next(0, 4))
                {
                    case 0:
                        neighbours.RemoveAt(4);
                        neighbours.RemoveAt(3);
                        neighbours.RemoveAt(2);
                        break;
                    case 1:
                        break;
                    case 2:
                        neighbours.RemoveAt(2);
                        neighbours.RemoveAt(1);
                        break;
                    case 3:
                        neighbours.RemoveAt(4);
                        neighbours.RemoveAt(0);
                        break;
                }
            }
            else if (j == jbound && i != 0 && i != ibound)
            {
                switch (random.Next(0, 4))
                {
                    case 0:
                        break;
                    case 1:
                        neighbours.RemoveAt(3);
                        neighbours.RemoveAt(2);
                        neighbours.RemoveAt(1);
                        break;
                    case 2:
                        neighbours.RemoveAt(4);
                        neighbours.RemoveAt(3);

                        break;
                    case 3:
                        neighbours.RemoveAt(1);
                        neighbours.RemoveAt(0);
                        break;
                }
            }
            else if (i == 0 && j == 0)
            {
                switch (random.Next(0, 4))
                {
                    case 0:
                        neighbours.RemoveAt(2);
                        neighbours.RemoveAt(1);
                        break;
                    case 1:
                        break;
                    case 2:
                        neighbours.RemoveAt(1);
                        neighbours.RemoveAt(0);
                        break;
                    case 3:
                        break;
                }
            }
            else if (i == ibound && j == 0)
            {
                switch (random.Next(0, 4))
                {
                    case 0:
                        neighbours.RemoveAt(2);
                        neighbours.RemoveAt(1);
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        neighbours.RemoveAt(2);
                        neighbours.RemoveAt(0);
                        break;
                }

            }
            else if (i == ibound && j == jbound)
            {
                switch (random.Next(0, 4))
                {
                    case 0:
                        break;
                    case 1:
                        neighbours.RemoveAt(2);
                        neighbours.RemoveAt(1);
                        break;
                    case 2:
                        break;
                    case 3:
                        neighbours.RemoveAt(1);
                        neighbours.RemoveAt(0);
                        break;
                }
            }
            else if (i == 0 && j == jbound)
            {
                switch (random.Next(0, 4))
                {
                    case 0:
                        break;
                    case 1:
                        neighbours.RemoveAt(1);
                        neighbours.RemoveAt(0);
                        break;
                    case 2:
                        neighbours.RemoveAt(2);
                        neighbours.RemoveAt(1);
                        break;
                    case 3:
                        break;
                }
            }
            else
            {
                switch (random.Next(0, 4))
                {
                    case 0:
                        neighbours.RemoveAt(7);
                        neighbours.RemoveAt(6);
                        neighbours.RemoveAt(5);
                        break;
                    case 1:
                        neighbours.RemoveAt(3);
                        neighbours.RemoveAt(2);
                        neighbours.RemoveAt(1);
                        break;
                    case 2:
                        neighbours.RemoveAt(5);
                        neighbours.RemoveAt(4);
                        neighbours.RemoveAt(3);
                        break;
                    case 3:
                        neighbours.RemoveAt(7);
                        neighbours.RemoveAt(1);
                        neighbours.RemoveAt(0);
                        break;
                }
            }

            return neighbours;
        }

        private List<int> getNeighboursRandomPentagonalPeriodic(int i, int j)
        {
            List<int> neighbours = new List<int>();
            neighbours = getNeighboursMoorePeriodic(i, j);
            switch (random.Next(0, 4))
            {
                case 0: // left side
                    neighbours.RemoveAt(7);
                    neighbours.RemoveAt(6);
                    neighbours.RemoveAt(5);
                    break;
                case 1: // right side
                    neighbours.RemoveAt(1);
                    neighbours.RemoveAt(1);
                    neighbours.RemoveAt(1);
                    break;
                case 2: // top side
                    neighbours.RemoveAt(3);
                    neighbours.RemoveAt(3);
                    neighbours.RemoveAt(3);
                    break;
                case 3: // bottom side
                    neighbours.RemoveAt(0);
                    neighbours.RemoveAt(0);
                    neighbours.RemoveAt(5);
                    break;
            }
            return neighbours;
        }

        private List<int> getNeighboursRandomHexagonal(int i, int j)
        {
            List<int> neighbours = neighbours = getNeighboursMoore(i, j);
            if (i == 0 && j != 0 && j != jbound)
            {
                switch (random.Next(0, 2))
                {
                    case 0: // top to bottom Hexagon
                        neighbours.RemoveAt(1);
                        break;
                    case 1: // bottom to top Hexagon
                        neighbours.RemoveAt(3);
                        break;
                }
            }
            else if (i == ibound && j != 0 && j != jbound)
            {
                switch (random.Next(0, 2))
                {
                    case 0:
                        neighbours.RemoveAt(4);
                        break;
                    case 1:
                        neighbours.RemoveAt(1);
                        break;
                }

            }
            else if (j == 0 && i != 0 && i != ibound)
            {
                switch (random.Next(0, 2))
                {
                    case 0:
                        neighbours.RemoveAt(4);
                        break;
                    case 1:
                        neighbours.RemoveAt(2);
                        break;
                }
            }
            else if (j == jbound && i != 0 && i != ibound)
            {
                switch (random.Next(0, 2))
                {
                    case 0:
                        neighbours.RemoveAt(3);
                        break;
                    case 1:
                        neighbours.RemoveAt(1);
                        break;
                }
            }
            else if (i == 0 && j == 0)
            {
                switch (random.Next(0, 2))
                {
                    case 0:
                        break;
                    case 1:
                        neighbours.RemoveAt(1);
                        break;
                }


            }
            else if (i == ibound && j == 0)
            {
                switch (random.Next(0, 2))
                {
                    case 0:
                        neighbours.RemoveAt(2);
                        break;
                    case 1:
                        break;
                }

            }
            else if (i == ibound && j == jbound)
            {
                switch (random.Next(0, 2))
                {
                    case 0:
                        break;
                    case 1:
                        neighbours.RemoveAt(1);
                        break;
                }

            }
            else if (i == 0 && j == jbound)
            {
                switch (random.Next(0, 2))
                {
                    case 0:
                        neighbours.RemoveAt(1);
                        break;
                    case 1:
                        break;
                }
            }
            else
            {
                switch (random.Next(0, 2))
                {
                    case 0:
                        neighbours.RemoveAt(7);
                        neighbours.RemoveAt(4);
                        break;
                    case 1:
                        neighbours.RemoveAt(5);
                        neighbours.RemoveAt(1);
                        break;
                }
            }
            return neighbours;
        }

        private List<int> getNeighboursRandomHexagonalPeriodic(int i, int j)
        {
            List<int> neighbours = getNeighboursMoorePeriodic(i, j);

            switch (random.Next(0, 2))
            {
                case 0: // top to bottom Hexagon
                    neighbours.RemoveAt(6);
                    neighbours.RemoveAt(3);
                    break;
                case 1: // bottom to top Hexagon
                    neighbours.RemoveAt(4);
                    neighbours.RemoveAt(1);
                    break;
            }
            return neighbours;
        }

        public virtual int getCellstate(int i, int j)
        {
            //simple game of life rules
            List<int> neighbours = getNeighboursState(i, j);
            int count = (from x in neighbours
                         where x != 0
                         select x).Count();
            switch (cells[i, j])
            {
                case 0:
                    return count == 3 ? 1 : 0;
                case 1:
                    if (count == 2 || count == 3)
                        return 1;
                    else if (count > 3 || count < 2)
                        return 0;
                    break;
            }
            return 0;
        }

        public int[,] Cells
        {
            get
            {
                return cells;
            }
        }

        public int Ibound
        {
            get
            {
                return ibound;
            }
        }

        public int Jbound
        {
            get
            {
                return jbound;
            }
        }

        public bool Work
        {
            get
            {
                return isWorking;
            }
        }
    }
}
