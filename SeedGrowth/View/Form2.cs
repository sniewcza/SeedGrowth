
using System.Windows.Forms;

namespace CellularAutomata
{
    public partial class Form2 : Form
    {
     
        public Form2()
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
