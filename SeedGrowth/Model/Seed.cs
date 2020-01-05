using System;

namespace SeedGrowth
{
    [Serializable]
    class Seed : Cell
    {
        private Guid _grainId;
        private Guid _phaseId;

        public Seed(int x, int y, CellState state) : base(x, y, state)
        {
            //_grainId = null;
            //_phaseId = null;
        }

        public Guid PhaseId { get => _phaseId; set => _phaseId = value; }
        public Guid GrainId { get => _grainId; set => _grainId = value; }
    }
}
