using CellularAutomata;

namespace SeedGrowth.Utils
{
    class SeedGrowthFactory
    {
        public static SeedGrowth Create(int domainWidth, int domainHeight, Neighbourhood neighbourhoodType, BoundaryConditions boundaryConditionsType)
        {
            SeedGrowth sd = new SeedGrowth(domainWidth, domainHeight);
            sd.setNeighbourhoodType(getNeigbhourhoodType(neighbourhoodType, boundaryConditionsType));
            return sd;
        }

        public static SeedGrowth Create(int domainWidth, int domainHeight)
        {
            return new SeedGrowth(domainWidth, domainHeight);
        }

        private static NeigbhourhoodType getNeigbhourhoodType(Neighbourhood neighbourhoodType, BoundaryConditions boundaryConditionsType)
        {
            switch (boundaryConditionsType)
            {
                case BoundaryConditions.Normal:
                    switch (neighbourhoodType)
                    {
                        case Neighbourhood.Moore:
                            return NeigbhourhoodType.Moore;
                        case Neighbourhood.VonNoyman:
                            return NeigbhourhoodType.VonNoyman;
                        case Neighbourhood.RandomPentagonal:
                            return NeigbhourhoodType.PentagonalRandom;
                        case Neighbourhood.RandomHexagonal:
                            return NeigbhourhoodType.HexagonalRandom;
                        default:
                            return NeigbhourhoodType.Moore;
                    }
                case BoundaryConditions.Periodic:
                    switch (neighbourhoodType)
                    {
                        case Neighbourhood.Moore:
                            return NeigbhourhoodType.MoorePeriodic;
                        case Neighbourhood.VonNoyman:
                            return NeigbhourhoodType.VonNoymanPeriodic;
                        case Neighbourhood.RandomPentagonal:
                            return NeigbhourhoodType.PentagonalRandomPeriodic;
                        case Neighbourhood.RandomHexagonal:
                            return NeigbhourhoodType.HexagonalRandomPeriodic;
                        default:
                            return NeigbhourhoodType.MoorePeriodic;
                    }
                default:
                    return NeigbhourhoodType.Moore;
            }
        }
    }
}
