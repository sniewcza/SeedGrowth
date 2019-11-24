using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedGrowth
{
    public enum CellState
    {
        alive,
        dead
    }

    public class Cell
    {
        private int _xCordinate;
        private int _yCordinate;
        private CellState _state;

        public int XCordinate { get => _xCordinate; set => _xCordinate = value; }
        public int YCordinate { get => _yCordinate; set => _yCordinate = value; }
        public CellState State { get => _state; set => _state = value; }

        public Cell(int x, int y, CellState state)
        {
            _xCordinate = x;
            _yCordinate = y;
            _state = state;
        }
    }
}
