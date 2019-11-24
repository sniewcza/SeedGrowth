using SeedGrowth;
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

    delegate List<Cell> getNeighbours(int i, int j);

    class CellularAutomata2D
    {
        public event EventHandler<Cell[,]> OnIterationComplette;

        private Cell[,] cells;
        private readonly int ibound;
        private readonly int jbound;
        private volatile bool isWorking = false;
        protected getNeighbours getNeighboursState;
        private Random random = new Random(DateTime.Now.Millisecond);

        public CellularAutomata2D(int N, int M)
        {
            cells = new Cell[N, M];
            ibound = cells.GetUpperBound(0);
            jbound = cells.GetUpperBound(1);
            for (int i = 0; i < ibound + 1; i++)
                for (int j = 0; j < jbound + 1; j++)
                    cells[i, j] = new Cell(i, j, CellState.dead);


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
                    cells[xmid, ymid].State = CellState.alive;
                    cells[xmid, ymid + 1].State = CellState.alive;
                    cells[xmid + 1, ymid - 1].State = CellState.alive;
                    cells[xmid + 1, ymid + 2].State = CellState.alive;
                    cells[xmid + 2, ymid].State = CellState.alive;
                    cells[xmid + 2, ymid + 1].State = CellState.alive;
                    break;
                case Structure.Oscilator:
                    cells[xmid, ++ymid].State = CellState.alive;
                    cells[xmid, ++ymid].State = CellState.alive;
                    cells[xmid, ++ymid].State = CellState.alive;
                    break;
                case Structure.Glider:
                    cells[xmid, ymid].State = CellState.alive;
                    cells[xmid + 1, ymid].State = CellState.alive;
                    cells[xmid + 2, ymid].State = CellState.alive;
                    cells[xmid, ymid + 1].State = CellState.alive;
                    cells[xmid + 1, ymid + 2].State = CellState.alive;
                    break;
                case Structure.Dakota:
                    cells[xmid, ymid].State = CellState.alive;
                    cells[xmid, ymid - 1].State = CellState.alive;
                    cells[xmid, ymid - 2].State = CellState.alive;
                    cells[xmid + 1, ymid].State = CellState.alive;
                    cells[xmid + 2, ymid].State = CellState.alive;
                    cells[xmid + 3, ymid].State = CellState.alive;
                    cells[xmid + 1, ymid - 3].State = CellState.alive;
                    cells[xmid + 4, ymid - 1].State = CellState.alive;
                    cells[xmid + 4, ymid - 3].State = CellState.alive;
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
                    cells[xmid, ymid].State = CellState.alive;
                    cells[xmid, ymid + 1].State = CellState.alive;
                    cells[xmid + 1, ymid - 1].State = CellState.alive;
                    cells[xmid + 1, ymid + 2].State = CellState.alive;
                    cells[xmid + 2, ymid].State = CellState.alive;
                    cells[xmid + 2, ymid + 1].State = CellState.alive;
                    break;
                case Structure.Oscilator:
                    cells[xmid, ++ymid].State = CellState.alive;
                    cells[xmid, ++ymid].State = CellState.alive;
                    cells[xmid, ++ymid].State = CellState.alive;
                    break;
                case Structure.Glider:
                    cells[xmid, ymid].State = CellState.alive;
                    cells[xmid + 1, ymid].State = CellState.alive;
                    cells[xmid + 2, ymid].State = CellState.alive;
                    cells[xmid, ymid + 1].State = CellState.alive;
                    cells[xmid + 1, ymid + 2].State = CellState.alive;
                    break;
                case Structure.Dakota:
                    cells[xmid, ymid].State = CellState.alive;
                    cells[xmid, ymid - 1].State = CellState.alive;
                    cells[xmid, ymid - 2].State = CellState.alive;
                    cells[xmid + 1, ymid].State = CellState.alive;
                    cells[xmid + 2, ymid].State = CellState.alive;
                    cells[xmid + 3, ymid].State = CellState.alive;
                    cells[xmid + 1, ymid - 3].State = CellState.alive;
                    cells[xmid + 4, ymid - 1].State = CellState.alive;
                    cells[xmid + 4, ymid - 3].State = CellState.alive;
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

        private Cell[,] createCoppy(Cell[,] cells)
        {
            Cell[,] newCells = new Cell[ibound + 1, jbound + 1];
            for (int i = 0; i < ibound + 1; i++)
                for (int j = 0; j < jbound + 1; j++)
                {
                    var cell = cells[i, j];
                    newCells[i, j] = new Cell(cell.XCordinate, cell.YCordinate, CellState.dead);
                }
            return newCells;
        }

        private void ComputeNextIterationCells()
        {
            Cell[,] newCells = createCoppy(Cells);
            // parallel version
            var length = newCells.GetLength(1);
            Parallel.For(0, ibound + 1, index =>
               {
                   for (int j = 0; j < length; j++)
                   {
                       newCells[index, j].State = getCellstate(index, j);
                   }
               });

            // sequential version
            //for (int i = 0; i < newCells.GetLength(0); i++)
            //    for (int j = 0; j < newCells.GetLength(1); j++)
            //        newCells[i, j].State = getCellstate(i, j);

            cells = newCells;
        }

        private List<Cell> getNeighboursMoorePeriodic(int i, int j)
        {
            List<Cell> neighbours = new List<Cell>(8);

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

        private List<Cell> getNeigboursVonNoymanPeriodic(int i, int j)
        {
            List<Cell> neighbours = new List<Cell>(4);
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

        private List<Cell> getNeigboursVonNoyman(int i, int j)
        {
            List<Cell> neighbours = new List<Cell>();

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

        private List<Cell> getNeighboursMoore(int i, int j)
        {
            List<Cell> neighbours = new List<Cell>();

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

        private List<Cell> getNeighboursRandomPentagonal(int i, int j)
        {
            List<Cell> neighbours = getNeighboursMoore(i, j);

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

        private List<Cell> getNeighboursRandomPentagonalPeriodic(int i, int j)
        {
            List<Cell> neighbours = new List<Cell>();
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

        private List<Cell> getNeighboursRandomHexagonal(int i, int j)
        {
            List<Cell> neighbours = neighbours = getNeighboursMoore(i, j);
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

        private List<Cell> getNeighboursRandomHexagonalPeriodic(int i, int j)
        {
            List<Cell> neighbours = getNeighboursMoorePeriodic(i, j);

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

        public virtual CellState getCellstate(int i, int j)
        {
            //simple game of life rules
            List<Cell> neighbours = getNeighboursState(i, j);
            int count = (from x in neighbours
                         where x.State != CellState.dead
                         select x).Count();

            switch (cells[i, j].State)
            {
                case CellState.dead:
                    return count == 3 ? CellState.alive : CellState.dead;
                case CellState.alive:
                    if (count == 2 || count == 3)
                        return CellState.alive;
                    else if (count > 3 || count < 2)
                        return CellState.dead;
                    break;
            }
            return 0;
        }

        public Cell[,] Cells
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
