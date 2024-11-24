using System;
using System.IO;
using System.Windows.Forms;

namespace SekiroSL
{
    public partial class GroupName : Form
    {
        public GroupName()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            Text = (Owner as MainWindow).Jo["New Profile"].ToString();
            button1.Text = (Owner as MainWindow).Jo["Apply"].ToString();
            button2.Text = (Owner as MainWindow).Jo["Cancel"].ToString();
            label1.Text = (Owner as MainWindow).Jo["Profile Name"].ToString();
            Log.log(Settings1.Default.GameDirectory.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ProfileNameBox.Text != "")
            {
                if (!Directory.Exists(Environment.CurrentDirectory + @"\Save\" + ProfileNameBox.Text))
                {
                    Directory.CreateDirectory(Environment.CurrentDirectory + @"\Save\" + ProfileNameBox.Text);

                    Settings1.Default.Save();
                    Close();
                }
                else
                {
                    MessageBox.Show((Owner as MainWindow).Jo["Name already existed"].ToString());
                }
            }
            else
            {
                MessageBox.Show((Owner as MainWindow).Jo["Please enter profile name"].ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
