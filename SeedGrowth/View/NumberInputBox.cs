using System;
using System.Windows.Forms;

namespace SeedGrowth.View
{
    public partial class NumberInputBox : Form
    {
        public int Value { get => Convert.ToInt32(maskedTextBox1.Text); }
        public string Label { set => label1.Text = value; }
        public NumberInputBox()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
            button1.Click += Button1_Click;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
