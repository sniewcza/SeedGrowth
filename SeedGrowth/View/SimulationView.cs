using System.Windows.Forms;

namespace SeedGrowth
{
    public partial class SimulationView : Form
    {
        
        public SimulationView()
        { 
            
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            
        }


        public PictureBox pictureBox
        {
            get => pictureBox1;
            
        }
    }
}
