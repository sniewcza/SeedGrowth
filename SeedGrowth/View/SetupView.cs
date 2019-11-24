using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using SeedGrowth.Interfaces;
using SeedGrowth.Controllers;

namespace SeedGrowth
{
    public enum BoundaryConditions
    {
        Normal,
        Periodic
    }
    public enum Neighbourhood
    {
        Moore,
        VonNoyman,
        RandomPentagonal,
        RandomHexagonal
    }
    public enum SeedDraw
    {
        Random,
        Evenly,
        RandomWithRadius
    }
    public partial class SetupView : Form, IView
    {
        private SeedGrowthController _controller;
        SimulationView simulationDisplayer;
        private List<String> bc = new List<String>();
        private List<String> nh = new List<String>();
        private List<String> sd = new List<String>();

        public SetupView()
        {
            InitializeComponent();
            foreach (var condition in Enum.GetNames(typeof(BoundaryConditions)))
                bc.Add(condition);
            foreach (var condition in Enum.GetNames(typeof(Neighbourhood)))
                nh.Add(condition);
            foreach (var condition in Enum.GetNames(typeof(SeedDraw)))
                sd.Add(condition);

            BCcomboBox.DataSource = new BindingSource(Enum.GetNames(typeof(BoundaryConditions)), null);
            NHcomboBox.DataSource = new BindingSource(Enum.GetNames(typeof(Neighbourhood)), null);
            SDcomboBox.DataSource = new BindingSource(Enum.GetNames(typeof(SeedDraw)), null);
            SDcomboBox.SelectedValueChanged += ComboBox3_SelectedValueChanged;
        }

        private void ComboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            switch ((string)SDcomboBox.SelectedValue)
            {
                case "Evenly":
                    _controller.setSeedDrawType(SeedDraw.Evenly);
                    groupBox1.Enabled = true;
                    groupBox2.Enabled = false;
                    break;
                case "RandomWithRadius":
                    _controller.setSeedDrawType(SeedDraw.RandomWithRadius);
                    groupBox1.Enabled = false;
                    groupBox2.Enabled = true;
                    break;
                case "Random":
                    _controller.setSeedDrawType(SeedDraw.Random);
                    groupBox1.Enabled = false;
                    groupBox2.Enabled = false;
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _controller.StartSeedGrowth();
            stopButton.Text = "Stop";
        }

        private void Form2_Disposed(object sender, EventArgs e)
        {
            _controller.StopSeedGrowth();
            actionsGroupBox.Enabled = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            _controller.StopSeedGrowth();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _controller.exportSeedGrowthData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int seeds = Convert.ToInt32(seedAmountInputBox.Text == "" ? "3" : seedAmountInputBox.Text);
            _controller.setNumberOfSeeds(seeds);
        }

        public void setController(SeedGrowthController controller)
        {
            _controller = controller;
        }

        public void setBitmap(Bitmap bitmap)
        {
            try
            {
                simulationDisplayer?.Invoke(new Action(() =>
                {
                    simulationDisplayer.pictureBox.Image = bitmap;
                    simulationDisplayer.Refresh();
                }));
            }
            catch (ObjectDisposedException ex)
            {
                _controller.StopSeedGrowth();
            }
            catch (InvalidOperationException)
            {
                _controller.StopSeedGrowth();
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            _controller.performNextStep();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int width = Convert.ToInt32(widthInputBox.Text == "" ? "500" : widthInputBox.Text);
            int height = Convert.ToInt32(heightInputBox.Text == "" ? "500" : heightInputBox.Text);
            int seeds = Convert.ToInt32(seedAmountInputBox.Text == "" ? "3" : seedAmountInputBox.Text);
            int xAxisSeeds = Convert.ToInt32(xAxisSeedsInputBox.Text == "" ? "3" : xAxisSeedsInputBox.Text);
            int yAxisSeeds = Convert.ToInt32(yAxisSeedsInputBox.Text == "" ? "3" : xAxisSeedsInputBox.Text);
            int radius = Convert.ToInt32(radiusInputBox.Text == "" ? "10" : radiusInputBox.Text);
            int numberOfInclusions = Convert.ToInt32(inclusionNumberInputBox.Text == "" ? "0" : inclusionNumberInputBox.Text);
            int inclusionsMaxRadius = Convert.ToInt32(inclusionMaxRadiusInputBox.Text == "" ? "0" : inclusionMaxRadiusInputBox.Text);
            int inclusionsMinRadius = Convert.ToInt32(inclusionMinRadiusInputBox.Text == "" ? "0" : inclusionMinRadiusInputBox.Text);
            var boundoryContidionType = Enum.Parse(typeof(BoundaryConditions), BCcomboBox.SelectedValue.ToString());
            var neighbourhoodType = Enum.Parse(typeof(Neighbourhood), NHcomboBox.SelectedValue.ToString());

            simulationDisplayer?.Dispose();
            simulationDisplayer = new SimulationView
            {
                Size = new Size(width + 20, height + 20)
            };
            simulationDisplayer.pictureBox.Size = new Size(width, height);
            simulationDisplayer.Disposed += Form2_Disposed;
            simulationDisplayer.Show();

            _controller.setWidth(width);
            _controller.setHeight(height);
            _controller.setBoundaryConditionType((BoundaryConditions)boundoryContidionType);
            _controller.setNeighbourhoodType((Neighbourhood)neighbourhoodType);
            _controller.setNumberOfSeeds(seeds);
            _controller.setRadius(radius);
            _controller.setXAxisSeeds(xAxisSeeds);
            _controller.setYAxisSeeds(yAxisSeeds);
            _controller.setInclusionsNumber(numberOfInclusions);
            _controller.setInclusionMaxRadius(inclusionsMaxRadius);
            _controller.setInclusionMinRadius(inclusionsMinRadius);
            _controller.initializeDomain();

            actionsGroupBox.Enabled = true;

        }

        public void showExceptionMessage(string message)
        {
            MessageBox.Show(message);
        }

        public string getFilePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                AddExtension = true,
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                return saveFileDialog.FileName;
            }
            return null;
        }
    }
}
