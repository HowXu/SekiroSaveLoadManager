using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SekiroSL
{
    public partial class LanguageSelect : Form
    {

        public List<FileInfo> FI = new List<FileInfo>();

        public LanguageSelect()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            FI = Setting.GetFile(Environment.CurrentDirectory + @"\Localization\", ".");
            comboBox1.DataSource = FI;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings1.Default.Language = comboBox1.Text;
            (Owner as MainWindow).Language = MainWindow.FileToString(Environment.CurrentDirectory + @"\Localization\" + Settings1.Default.Language);
            (Owner as MainWindow).Jo = (JObject)JsonConvert.DeserializeObject((Owner as MainWindow).Language);
            button1.Text = (Owner as MainWindow).Jo["Apply"].ToString();
            label1.Text = (Owner as MainWindow).Jo["Language"].ToString() + ":";
            Text = (Owner as MainWindow).Jo["Language"].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Environment.GetEnvironmentVariable("systemdrive") + @"\Users\" + Environment.UserName + @"\AppData\Roaming\Sekiro"))
            {
                openFileDialog1.FileName = Environment.GetEnvironmentVariable("systemdrive") + @"\Users\" + Environment.UserName + @"\AppData\Roaming\Sekiro";
                DirectoryInfo Dir = new DirectoryInfo(Environment.GetEnvironmentVariable("systemdrive") + @"\Users\" + Environment.UserName + @"\AppData\Roaming\Sekiro");
                DirectoryInfo[] di = Dir.GetDirectories();
                if (di.Count() == 1)
                {
                    Settings1.Default.GameDirectory = di[0].FullName + @"\" + Settings1.Default.SaveFileName;
                    MessageBox.Show((Owner as MainWindow).Jo["FindSekiro"].ToString());
                }
                else
                {
                    MessageBox.Show((Owner as MainWindow).Jo["SaveIntroduce"].ToString());
                    openFileDialog1.FileName = Environment.GetEnvironmentVariable("systemdrive") + @"\Users\" + Environment.UserName + @"\AppData\Roaming\Sekiro\S0000.sl2";
                    openFileDialog1.ShowDialog();
                    while (!openFileDialog1.CheckFileExists)
                    {
                        MessageBox.Show((Owner as MainWindow).Jo["SaveIntroduce"].ToString());
                        openFileDialog1.ShowDialog();
                    }
                    Settings1.Default.GameDirectory = openFileDialog1.FileName;
                }
            }
            else
            {
                MessageBox.Show((Owner as MainWindow).Jo["CannotFindSekiro"].ToString());
                openFileDialog1.ShowDialog();
                Settings1.Default.GameDirectory = openFileDialog1.FileName;
            }
            Settings1.Default.Save();
            Close();
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings1.Default.Save();
        }
    }
}
