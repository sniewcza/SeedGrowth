using SeedGrowth.Interfaces;
using SeedGrowth.Model;
using SeedGrowth.Utils;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

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
        private int _inclusionMaxRadius = 0;
        private int _inclusionMinRadius = 0;
        private int _numberOfInclusions = 0;
        private bool isGBCType = false;
        private int _activationThreshold = 50;
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
            catch (Exception exception)
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
                _seedGrowth.setInclusions(_numberOfInclusions, _inclusionMinRadius, _inclusionMaxRadius);
            }
            if (isGBCType)
            {
                _seedGrowth.useGBC(true);
                _seedGrowth._activationThreshold = _activationThreshold;
            }
            _seedGrowth.OnGrainChange += _seedGrowth_OnIterationComplette;
            _seedGrowth.init();
        }

        public void setInclusionMaxRadius(int radius)
        {
            _inclusionMaxRadius = radius;
        }

        public void setInclusionMinRadius(int radius)
        {
            _inclusionMinRadius = radius;
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
            
                _worker.RunWorkerAsync();
            
            
        }

        private void _seedGrowth_OnIterationComplette(object sender, Color[,] e)
        {
            _view.setBitmap(SeedGrowthConverter.ConvertToBitmap(e));
        }

        public void StopSeedGrowth()
        {
            _seedGrowth?.Stop();
        }

        public void removeGrain(int x, int y)
        {
            if (_seedGrowth.Work)
            {
                _seedGrowth.Stop();
            }

            _seedGrowth.removeGrain(x, y);
        }

        public void setupSubstructures()
        {
            var value = _view.getNumber();
            if (value != null)
            {
                _seedGrowth.setRemainingStructureAsImmutable();
                _seedGrowth.useSubstructures(true);
                _seedGrowth.setSeedsRandomly(value.Value);
                this.StartSeedGrowth();
            }
        }
        public void setupDualPhase()
        {
            var value = _view.getNumber();
            if (value != null)
            {
                _seedGrowth.markRemainingStructureAsPhase();
                _seedGrowth.useDP(true);
                _seedGrowth.setSeedsRandomly(value.Value);
                this.StartSeedGrowth();
            }
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
            _seedGrowth?.PerformIterationStep();
        }

        public void exportSeedGrowthData()
        {
            string filePath = _view.getExportFilePath();
            if (filePath != null)
            {
                IFormatter formatter = new BinaryFormatter();
                try
                {
                    var dataObject = _seedGrowth.getSerialziableData();
                    Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                    formatter.Serialize(stream, dataObject);
                    stream.Close();
                }
                catch (Exception ex)
                {
                    _view.showExceptionMessage(ex.Message);
                }
            }
        }

        public void importSeedGrowthData()
        {
            string filePath = _view.getImportFilePath();
            if(filePath != null)
            {
                IFormatter formatter = new BinaryFormatter();
                try
                {
                    Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    
                    var data = formatter.Deserialize(stream) as SeedGrowthDo;
                    var width = data.seeds.GetLength(0);
                    var height = data.seeds.GetLength(1);

                    _seedGrowth = SeedGrowthFactory.Create(width, height, _neighbourhoodType, _boundoryConditionType);
                    _seedGrowth.setSeeds(data.seeds);
                    _seedGrowth.setGrainMap(data.grainMap);
                    _seedGrowth.setCells(data.cells);
                    if (isGBCType)
                    {
                        _seedGrowth.useGBC(true);
                        _seedGrowth._activationThreshold = _activationThreshold;
                    }
                    _seedGrowth.OnGrainChange += _seedGrowth_OnIterationComplette;
                   // _seedGrowth.PerformIterationStep();
                }   
                catch(Exception ex)
                {
                    _view.showExceptionMessage(ex.Message);
                }
            }
        }

        public void getStatistic()
        {
            _view.showInfo($"Edges length: {_seedGrowth.getEdgesLength()} \nGrain mean size: {_seedGrowth.getGrainMeanSize()}");
        }
        public void markBoundaries()
        {
            _view.setBitmap(SeedGrowthConverter.ConvertToBitmap(_seedGrowth.findBoundaries()));
        }
        public void setGBC(bool cond)
        {
            isGBCType = cond;
        }
        public void setActivationThreshold(int t)
        {
            _activationThreshold = t;
        }
        public void getSeedInfoRequest(int x, int y)
        {
            _view.showInfo(_seedGrowth?.getSeedInfoAtPosition(x, y));
        }
    }
}
