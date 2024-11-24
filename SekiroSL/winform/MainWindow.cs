using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SekiroSL
{
    public partial class MainWindow : Form
    {
        public static string Version = "1.0";
        public static int LogLevel = 0;
        public Setting setting = new Setting();
        public GroupName group = new GroupName();
        public LanguageSelect lang_select = new LanguageSelect();
        public RenameDialog rename = new RenameDialog();
        public GroupRenameDialog gourp_rename = new GroupRenameDialog();
        public string Language;
        public JObject Jo = new JObject();
        public DirectoryInfo i18nDir = new DirectoryInfo(Environment.CurrentDirectory + @"\Localization\");
        public DirectoryInfo SaveDir = new DirectoryInfo(Environment.CurrentDirectory + @"\Save\");
        public DirectoryInfo SoundDir = new DirectoryInfo(Environment.CurrentDirectory + @"\Sound\");
        public string nameofre = "";
        SoundPlayer Savemp3 = new SoundPlayer();
        SoundPlayer Loadmp3 = new SoundPlayer();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            //加一个检测Save文件夹是否存在的机制 不然下面会直接空指针访问
            if (!SaveDir.Exists)
            {
                // 文件夹不存在，创建文件夹
                SaveDir.Create();
            }

            //判断资源文件是不是正常
            if(!i18nDir.Exists|| !SoundDir.Exists)
            {
                MessageBox.Show((Owner as MainWindow).Jo["WrongResources"].ToString());
                Environment.Exit(1);
            }

            setting.Owner = this;
            group.Owner = this;
            lang_select.Owner = this;
            rename.Owner = this;
            gourp_rename.Owner = this;
            if (Settings1.Default.Language == "")
            {
                lang_select.ShowDialog();
            }
            Language = FileToString(i18nDir + Settings1.Default.Language);
            Jo = (JObject)JsonConvert.DeserializeObject(Language);
            //ReTranslateForm();
            DirectoryInfo[] di = SaveDir.GetDirectories();
            comboBox1.DataSource = di;
            renameToolStripMenuItem.Enabled = false;
            deleteToolStripMenuItem.Enabled = false;
            if (comboBox1.Items.Count == 0 || comboBox1.Text == "")
            {
                deleteToolStripMenuItem1.Enabled = false;
                renameToolStripMenuItem1.Enabled = false;
            }
            SortToolStripMenuItem.Available = false;
            listBox1_SelectedIndexChanged(null, null);
            if (comboBox1.Text != "")
            {
                SaveButton.Enabled = true;
            }
            else
            {
                SaveButton.Enabled = false;
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;//如果m.Msg的值为0x0312那么表示用户按下了热键
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 1:
                            Savemp3.Play();
                            SaveButton_Click(null, null);
                            break;
                        case 2:
                            Loadmp3.Play();
                            LoadButton_Click(null, null);
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        public void ReTranslateForm()
        {
            Text = Jo["SekiroSL"].ToString();
            NewProflie.Text = Jo["New Profile"].ToString();
            LoadButton.Text = Jo["Load"].ToString();
            SaveButton.Text = Jo["Save"].ToString();
            ReplaceButton.Text = Jo["Replace"].ToString();
            renameToolStripMenuItem.Text = Jo["Rename"].ToString();
            deleteToolStripMenuItem.Text = Jo["Delete"].ToString();
            renameToolStripMenuItem1.Text = Jo["Rename"].ToString();
            deleteToolStripMenuItem1.Text = Jo["Delete"].ToString();
            SortToolStripMenuItem.Text = Jo["Sort"].ToString();
            standardSortAZToolStripMenuItem.Text = Jo["StandardSortAZ"].ToString();
            naturalSortAZToolStripMenuItem.Text = Jo["NaturalSortAZ"].ToString();
            standardSortZAToolStripMenuItem.Text = Jo["StandardSortZA"].ToString();
            naturalSortZAToolStripMenuItem.Text = Jo["NaturalSortZA"].ToString();

            if (Settings1.Default.SaveHotkey != Keys.None)
            {
                Hotkeys.RegisterHotKey(Handle, 1, Settings1.Default.SaveModifier, Settings1.Default.SaveHotkey);
                Log.log("SuccessREGIS");
            }
            if (Settings1.Default.LoadHotkey != Keys.None)
            {
                Hotkeys.RegisterHotKey(Handle, 2, Settings1.Default.LoadModifier, Settings1.Default.LoadHotkey);
                Log.log("SuccessREGIS");
            }
            if (Settings1.Default.SoundType != "Mute")
            {
                Loadmp3.SoundLocation = SoundDir + Settings1.Default.SoundType + "Load.wav";
                Savemp3.SoundLocation = SoundDir + Settings1.Default.SoundType + "Save.wav";
                Log.log(SoundDir + Settings1.Default.SoundType + "Save.wav");
            }
        }
        public static string FileToString(string filePath)
        {
            string strData = "";
            try
            {
                string line;
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        strData = strData + line;
                    }
                }
            }
            catch (Exception e)
            {
                Log.log("The file could not be read:");
                Log.log(e.Message);
            }
            return strData;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndices.Count > 1)
            {
                renameToolStripMenuItem.Enabled = false;
                deleteToolStripMenuItem.Enabled = true;
                LoadButton.Enabled = false;
                ReplaceButton.Enabled = false;
            }
            else if (listBox1.SelectedIndices.Count <= 0)
            {
                renameToolStripMenuItem.Enabled = false;
                deleteToolStripMenuItem.Enabled = false;
                LoadButton.Enabled = false;
                ReplaceButton.Enabled = false;
            }
            else
            {
                renameToolStripMenuItem.Enabled = true;
                deleteToolStripMenuItem.Enabled = true;
                LoadButton.Enabled = true;
                ReplaceButton.Enabled = true;
            }
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            Hotkeys.UnregisterHotKey(Handle, 1);
            Hotkeys.UnregisterHotKey(Handle, 2);
            setting.ShowDialog();
            Settings1.Default.Upgrade();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            File.Copy(SaveDir + comboBox1.Text + @"\" + listBox1.Text, Settings1.Default.GameDirectory, true);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (File.Exists(SaveDir + comboBox1.Text + @"\" + Settings1.Default.SaveFileName) == false)
            {
                File.Copy(Settings1.Default.GameDirectory, SaveDir + comboBox1.Text + @"\" + Settings1.Default.SaveFileName, false);
                comboBox1_SelectedIndexChanged(null, null);
                listBox1.SelectionMode = SelectionMode.One;
                listBox1.Text = Settings1.Default.SaveFileName;
                listBox1.SelectionMode = SelectionMode.MultiExtended;
            }
            else
            {
                int index = 0;
                while (File.Exists(SaveDir + comboBox1.Text + @"\" + index.ToString() + Settings1.Default.SaveFileName))
                {
                    index++;
                }
                File.Copy(Settings1.Default.GameDirectory, SaveDir + comboBox1.Text + @"\" + index.ToString() + Settings1.Default.SaveFileName, false);
                comboBox1_SelectedIndexChanged(null, null);
                listBox1.SelectionMode = SelectionMode.One;
                listBox1.Text = index.ToString() + Settings1.Default.SaveFileName;
                listBox1.SelectionMode = SelectionMode.MultiExtended;
            }
        }

        private void ReplaceSave_Click(object sender, EventArgs e)
        {
            if ((int)MessageBox.Show(Jo["Are you sure you want to reaplace?"].ToString(), Jo["SekiroSL"].ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == 6)
            {
                File.Copy(Settings1.Default.GameDirectory, SaveDir + comboBox1.Text + @"\" + listBox1.Text, true);
            }
        }

        private void NewProflie_Click(object sender, EventArgs e)
        {
            Hotkeys.UnregisterHotKey(Handle, 1);
            Hotkeys.UnregisterHotKey(Handle, 2);
            group.ShowDialog();
            if (Settings1.Default.SaveHotkey != Keys.None)
            {
                Hotkeys.RegisterHotKey(Handle, 1, Settings1.Default.SaveModifier, Settings1.Default.SaveHotkey);
                Log.log("SuccessREGIS");
            }
            if (Settings1.Default.LoadHotkey != Keys.None)
            {
                Hotkeys.RegisterHotKey(Handle, 2, Settings1.Default.LoadModifier, Settings1.Default.LoadHotkey);
                Log.log("SuccessREGIS");
            }
            DirectoryInfo[] di = SaveDir.GetDirectories();
            comboBox1.DataSource = di;
            if (comboBox1.Text != "")
            {
                renameToolStripMenuItem1.Enabled = true;
                deleteToolStripMenuItem1.Enabled = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<FileInfo> FileI = new List<FileInfo>();
            DirectoryInfo root = new DirectoryInfo(SaveDir + comboBox1.Text);
            FileInfo[] files = root.GetFiles();
            listBox1.DataSource = files;
            if (comboBox1.Text != "")
            {
                SaveButton.Enabled = true;
            }
            else
            {
                SaveButton.Enabled = false;
            }

            if (listBox1.Items.Count == 0)
            {
                LoadButton.Enabled = false;
                ReplaceButton.Enabled = false;
            }
        }

        private void standardSortAZToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void naturalSortAZToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void SortToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void standardSortZAToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void naturalSortZAToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hotkeys.UnregisterHotKey(Handle, 1);
            Hotkeys.UnregisterHotKey(Handle, 2);
            nameofre = listBox1.Text;
            rename.ShowDialog();
            if (Settings1.Default.SaveHotkey != Keys.None)
            {
                Hotkeys.RegisterHotKey(Handle, 1, Settings1.Default.SaveModifier, Settings1.Default.SaveHotkey);
                Log.log("SuccessREGIS");
            }
            if (Settings1.Default.LoadHotkey != Keys.None)
            {
                Hotkeys.RegisterHotKey(Handle, 2, Settings1.Default.LoadModifier, Settings1.Default.LoadHotkey);
                Log.log("SuccessREGIS");
            }
        }

        public bool Rename(string Name)
        {
            if (File.Exists(SaveDir + comboBox1.Text + @"\" + Name) || Name == "")
            {
                return false;
            }
            else
            {
                File.Move(SaveDir + comboBox1.Text + @"\" + listBox1.Text, Environment.CurrentDirectory + @"\Save\" + comboBox1.Text + @"\" + Name);
                comboBox1_SelectedIndexChanged(null, null);
                return true;
            }
        }

        public bool RenameProfile(string Name)
        {
            if (Directory.Exists(SaveDir + Name + @"\") || Name == "")
            {
                return false;
            }
            else
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + @"\Save\" + Name);
                string[] file = Directory.GetFiles(SaveDir + comboBox1.Text);
                for (int i = 0; i < file.Count(); i++)
                {
                    string[] a = Regex.Split(file[i], "\\\\");
                    Log.log(a[a.Count() - 1]);
                    File.Move(file[i], SaveDir + Name + @"\" + a[a.Count() - 1]);
                }
                Directory.Delete(SaveDir + comboBox1.Text + @"\");
                DirectoryInfo[] di = SaveDir.GetDirectories();
                comboBox1.DataSource = di;
                return true;
            }
        }

        private void renameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Hotkeys.UnregisterHotKey(Handle, 1);
            Hotkeys.UnregisterHotKey(Handle, 2);
            nameofre = comboBox1.Text;
            gourp_rename.ShowDialog();
            if (Settings1.Default.SaveHotkey != Keys.None)
            {
                Hotkeys.RegisterHotKey(Handle, 1, Settings1.Default.SaveModifier, Settings1.Default.SaveHotkey);
                Log.log("SuccessREGIS");
            }
            if (Settings1.Default.LoadHotkey != Keys.None)
            {
                Hotkeys.RegisterHotKey(Handle, 2, Settings1.Default.LoadModifier, Settings1.Default.LoadHotkey);
                Log.log("SuccessREGIS");
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndices.Count == 1)
            {
                if ((int)MessageBox.Show(Jo["Are you sure you want to delete?"].ToString(), Jo["SekiroSL"].ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == 6)
                {
                    File.Delete(SaveDir + comboBox1.Text + @"\" + listBox1.Text);
                    comboBox1_SelectedIndexChanged(null, null);
                }
            }
            else
            {
                if ((int)MessageBox.Show(Jo["Are you sure you want to delete?"].ToString(), Jo["SekiroSL"].ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == 6)
                {
                    ListBox.SelectedObjectCollection FG = listBox1.SelectedItems;
                    for (int i = 0; i < FG.Count; i++)
                    {
                        File.Delete(SaveDir + comboBox1.Text + @"\" + listBox1.GetItemText(FG[i]));
                        Log.log(listBox1.GetItemText(FG[i]));
                    }
                    comboBox1_SelectedIndexChanged(null, null);
                }
            }
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {

        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if ((int)MessageBox.Show(Jo["Are you sure you want to delete?"].ToString(), Jo["SekiroSL"].ToString(), MessageBoxButtons.YesNo) == 6)
            {
                string[] file = Directory.GetFiles(SaveDir + comboBox1.Text);
                for (int i = 0; i < file.Count(); i++)
                {
                    File.Delete(file[i]);
                }
                Directory.Delete(SaveDir + comboBox1.Text + @"\");
                DirectoryInfo[] di = SaveDir.GetDirectories();
                comboBox1.DataSource = di;
                if (comboBox1.Items.Count == 0)
                {
                    comboBox1.Text = "";
                    listBox1.DataSource = null;
                    LoadButton.Enabled = false;
                    SaveButton.Enabled = false;
                    ReplaceButton.Enabled = false;
                }
            }
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            if(LogLevel == 1)
            {
                Console.WriteLine(comboBox1.Items.Count);
            }
            
            if (comboBox1.Items.Count == 0 || comboBox1.Text == "")
            {
                deleteToolStripMenuItem1.Enabled = false;
                renameToolStripMenuItem1.Enabled = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hotkeys.UnregisterHotKey(Handle, 1);
            Hotkeys.UnregisterHotKey(Handle, 2);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            ReTranslateForm();
        }
    }
}
