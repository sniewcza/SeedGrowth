using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CellularAutomata
{
    public partial class Menu : Form
    {
        private LifeSimulator _simulator;
        private Form2 _form;
        private int _width;
        private int _height;
        private int _padding = 10;
        public Menu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            readUserData();
            createBoard();
            _simulator = new LifeSimulator(_width, _height, refreshView);
            _form.Show();

            BackgroundWorker w = new BackgroundWorker();
            w.DoWork += (o, s) => _simulator.Start();
            w.RunWorkerAsync();
        }

        private void createBoard()
        {
            _form = new Form2();
            _form.Width = _width + _padding;
            _form.Height = _height + _padding;

            _form.pictureBox.MouseClick += PictureBox_MouseClick;
            _form.FormClosing += _form_FormClosing;
            _form.pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
        }
        private void _form_FormClosing(object sender, FormClosingEventArgs e)
        {
            _simulator.Stop();
        }

        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            var index = comboBox1.SelectedIndex;
            _simulator.SetStructure(e.X, e.Y, (structure)index);
        }

        private void readUserData()
        {
            string widthInput = this.maskedTextBox1.Text;
            string heihtInput = this.maskedTextBox2.Text;
            _width = Convert.ToInt32(widthInput != string.Empty ? widthInput : "300");
            _height = Convert.ToInt32(heihtInput != string.Empty ? heihtInput : "300");
        }
        private void refreshView()
        {
            _form.Invoke(new Action(() =>
            {
                _form.pictureBox.Image = _simulator.GetGameSnapshoot();
            }
            ));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _simulator.Stop();
        }
    }
}
