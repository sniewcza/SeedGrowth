using SeedGrowth.Interfaces;
using SeedGrowth.Utils;
using System;
using System.ComponentModel;

namespace SeedGrowth.Controllers
{
    public class SeedGrowthController
    {
        private readonly IView _view;
        private int _width;
        private int _height;
        private int _numberOfGrainSeeds;
        private int _radius;
        private int _xAxisSeeds;
        private int _yAxisSeeds;
        private int _inclusionRadius = 0;
        private int _numberOfInclusions = 0;
        private SeedGrowth _seedGrowth;
        private SeedDraw _seedDraw;
        private BoundaryConditions _boundoryConditionType;
        private Neighbourhood _neighbourhoodType;
        private BackgroundWorker _worker = new BackgroundWorker();

        public SeedGrowthController(IView view)
        {
            _view = view;
            _view.setController(this);
            _worker.DoWork += Worker_DoWork;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            _seedGrowth.Start();
        }

        private void setInitialSeeds()
        {
            try
            {
                switch (_seedDraw)
                {
                    case SeedDraw.Evenly:
                        _seedGrowth.setSeedsEvenly(_xAxisSeeds, _yAxisSeeds);
                        break;
                    case SeedDraw.RandomWithRadius:
                        _seedGrowth.setSeedswithRadius(_numberOfGrainSeeds, _radius);
                        break;
                    default:
                        _seedGrowth.setSeedsRandomly(_numberOfGrainSeeds);
                        break;
                }
            }
            catch(Exception exception)
            {
                _view.showExceptionMessage(exception.Message);
            }
        }

        public void initializeDomain()
        {
            _seedGrowth = SeedGrowthFactory.Create(_width, _height, _neighbourhoodType, _boundoryConditionType);
            setInitialSeeds();
            if (_numberOfInclusions != 0)
            {
                _seedGrowth.setInclusions(_numberOfInclusions, _inclusionRadius);
            }
            
            _seedGrowth.OnIterationComplette += _seedGrowth_OnIterationComplette;
            _seedGrowth.PerformIterationStep();
        }

        public void setInclusionRadius(int radius)
        {
            _inclusionRadius = radius;
        }

        public void setInclusionsNumber(int numberOfInclusions)
        {
            _numberOfInclusions = numberOfInclusions;
        }
        public void setRadius(int radius)
        {
            _radius = radius;
        }

        public void setXAxisSeeds(int numberOfSeeds)
        {
            _xAxisSeeds = numberOfSeeds;
        }

        public void setYAxisSeeds(int numberOfSeeds)
        {
            _yAxisSeeds = numberOfSeeds;
        }

        public void setWidth(int width)
        {
            _width = width;
        }

        public void setHeight(int height)
        {
            _height = height;
        }

        public void StartSeedGrowth()
        {
            try
            {
                _worker.RunWorkerAsync();
            }
            catch (InvalidOperationException)
            {

            }
        }

        private void _seedGrowth_OnIterationComplette(object sender, int[,] e)
        {
            _view.setBitmap(SeedGrowthConverter.ConvertToBitmap(e));
        }

        public void StopSeedGrowth()
        {
            _seedGrowth.Stop();
        }

        public void setBoundaryConditionType(BoundaryConditions type)
        {
            _boundoryConditionType = type;
        }

        public void setNeighbourhoodType(Neighbourhood type)
        {
            _neighbourhoodType = type;
        }

        public void setSeedDrawType(SeedDraw type)
        {
            _seedDraw = type;
        }

        public void setNumberOfSeeds(int numberOfseeds)
        {
            _numberOfGrainSeeds = numberOfseeds;
        }

        public void performNextStep()
        {
            _seedGrowth.PerformIterationStep();
        }
    }
}
