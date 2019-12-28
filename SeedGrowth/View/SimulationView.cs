using System;
using System.Windows.Forms;

namespace SeedGrowth
{
    public partial class SimulationView : Form
    {
        public new event EventHandler<MouseEventArgs> OnMouseClick;
        public new event EventHandler<MouseEventArgs> OnMouseMove;
        public SimulationView()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            pictureBox1.MouseMove += PictureBox1_MouseMove;
            pictureBox1.MouseClick += PictureBox1_MouseClick;
        }

        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            OnMouseClick?.Invoke(this, e);
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            OnMouseMove?.Invoke(this, e);
        }

        public PictureBox pictureBox
        {
            get => pictureBox1;
        }
    }
}
