using System;
using System.Windows.Forms;

namespace SekiroSL
{
    public partial class RenameDialog : Form
    {
        public RenameDialog()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            Text = (Owner as MainWindow).Jo["Rename"].ToString();
            label1.Text = (Owner as MainWindow).Jo["Rename"].ToString();
            button1.Text = (Owner as MainWindow).Jo["Apply"].ToString();
            textBox1.Text = (Owner as MainWindow).nameofre;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((Owner as MainWindow).Rename(textBox1.Text))
            {
                Close();
            }
            else
            {
                MessageBox.Show((Owner as MainWindow).Jo["File already existed or didn't enter the file name"].ToString());
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
