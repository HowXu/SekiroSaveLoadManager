﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace SekiroSL
{
    public partial class Setting : Form
    {

        public List<FileInfo> FI = new List<FileInfo>();
        public string Version = "Alpha1.6";

        public Setting()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            LanguageLabel.Text = (Owner as MainWindow).Jo["Language"].ToString();
            groupBox1.Text = (Owner as MainWindow).Jo["Hotkeys"].ToString();
            ClearButton1.Text = (Owner as MainWindow).Jo["Clear"].ToString();
            ClearButton2.Text = ClearButton1.Text;
            ApplyButton.Text = (Owner as MainWindow).Jo["Apply"].ToString();
            CancelButton.Text = (Owner as MainWindow).Jo["Cancel"].ToString();
            Text = (Owner as MainWindow).Jo["Settings"].ToString();
            AboutLink.Text = (Owner as MainWindow).Jo["About"].ToString();
            label2.Text = (Owner as MainWindow).Jo["Load"].ToString();
            label3.Text = (Owner as MainWindow).Jo["Save"].ToString();
            FI = GetFile(Environment.CurrentDirectory + @"\Localization\", ".");
            comboBox1.DataSource = FI;
            comboBox1.Text = Settings1.Default.Language;
            label1.Text = (Owner as MainWindow).Jo["File"].ToString();
            textBox4.Text = Settings1.Default.GameDirectory;
            label4.Text = (Owner as MainWindow).Jo["SoundEffectType"].ToString();
            radioButton1.Text = (Owner as MainWindow).Jo["Ting"].ToString();
            radioButton2.Text = (Owner as MainWindow).Jo["Google"].ToString();
            radioButton3.Text = (Owner as MainWindow).Jo["Mute"].ToString();
            button2.Text = (Owner as MainWindow).Jo["AutoLocated"].ToString();
            if (Settings1.Default.SoundType == "Mute")
            {
                radioButton3.Checked = true;
            }
            else if (Settings1.Default.SoundType == "Google")
            {
                radioButton2.Checked = true;
            }
            else
            {
                radioButton1.Checked = true;
            }

            if (Settings1.Default.LoadModifier == 1)
            {
                checkBox3.Checked = true;
            }
            else if (Settings1.Default.LoadModifier == 2)
            {
                checkBox1.Checked = true;
            }
            else if (Settings1.Default.LoadModifier == 3)
            {
                checkBox1.Checked = true;
                checkBox3.Checked = true;
            }
            else if (Settings1.Default.LoadModifier == 4)
            {
                checkBox2.Checked = true;
            }
            else if (Settings1.Default.LoadModifier == 5)
            {
                checkBox3.Checked = true;
                checkBox2.Checked = true;
            }
            else if (Settings1.Default.LoadModifier == 6)
            {
                checkBox1.Checked = true;
                checkBox2.Checked = true;
            }
            else if (Settings1.Default.LoadModifier == 7)
            {
                checkBox1.Checked = true;
                checkBox2.Checked = true;
                checkBox3.Checked = true;
            }
            else if (Settings1.Default.LoadModifier == 0)
            {
                checkBox4.Checked = true;
            }

            if (Settings1.Default.SaveModifier == 1)
            {
                checkBox6.Checked = true;
            }
            else if (Settings1.Default.SaveModifier == 2)
            {
                checkBox8.Checked = true;
            }
            else if (Settings1.Default.SaveModifier == 3)
            {
                checkBox8.Checked = true;
                checkBox6.Checked = true;
            }
            else if (Settings1.Default.LoadModifier == 4)
            {
                checkBox7.Checked = true;
            }
            else if (Settings1.Default.SaveModifier == 5)
            {
                checkBox6.Checked = true;
                checkBox7.Checked = true;
            }
            else if (Settings1.Default.SaveModifier == 6)
            {
                checkBox8.Checked = true;
                checkBox7.Checked = true;
            }
            else if (Settings1.Default.SaveModifier == 7)
            {
                checkBox6.Checked = true;
                checkBox7.Checked = true;
                checkBox8.Checked = true;
            }
            else if (Settings1.Default.SaveModifier == 0)
            {
                checkBox5.Checked = true;
            }
            switch (Settings1.Default.LoadHotkey.ToString())
            {
                case "D0":
                    LoadHotkey.Text = "0";
                    break;
                case "D1":
                    LoadHotkey.Text = "1";
                    break;
                case "D2":
                    LoadHotkey.Text = "2";
                    break;
                case "D3":
                    LoadHotkey.Text = "3";
                    break;
                case "D4":
                    LoadHotkey.Text = "4";
                    break;
                case "D5":
                    LoadHotkey.Text = "5";
                    break;
                case "D6":
                    LoadHotkey.Text = "6";
                    break;
                case "D7":
                    LoadHotkey.Text = "7";
                    break;
                case "D8":
                    LoadHotkey.Text = "8";
                    break;
                case "D9":
                    LoadHotkey.Text = "9";
                    break;
                default:
                    LoadHotkey.Text = Settings1.Default.LoadHotkey.ToString();
                    break;
            }

            switch (Settings1.Default.SaveHotkey.ToString())
            {
                case "D0":
                    SaveHotKey.Text = "0";
                    break;
                case "D1":
                    SaveHotKey.Text = "1";
                    break;
                case "D2":
                    SaveHotKey.Text = "2";
                    break;
                case "D3":
                    SaveHotKey.Text = "3";
                    break;
                case "D4":
                    SaveHotKey.Text = "4";
                    break;
                case "D5":
                    SaveHotKey.Text = "5";
                    break;
                case "D6":
                    SaveHotKey.Text = "6";
                    break;
                case "D7":
                    SaveHotKey.Text = "7";
                    break;
                case "D8":
                    SaveHotKey.Text = "8";
                    break;
                case "D9":
                    SaveHotKey.Text = "9";
                    break;
                default:
                    SaveHotKey.Text = Settings1.Default.SaveHotkey.ToString();
                    break;
            }
        }

        public static List<FileInfo> GetFile(string path, string extName)
        {
            List<FileInfo> lst = new List<FileInfo>();
            string[] dir = Directory.GetDirectories(path);
            DirectoryInfo fdir = new DirectoryInfo(path);
            FileInfo[] file = fdir.GetFiles();
            if (file.Length != 0 || dir.Length != 0)
            {
                foreach (FileInfo f in file)
                {
                    if (extName.ToLower().IndexOf(f.Extension.ToLower()) >= 0)
                    {
                        lst.Add(f);
                    }
                }
                foreach (string d in dir)
                {
                    GetFile(d, extName);
                }
            }
            return lst;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Text = (Owner as MainWindow).Jo["AutoLocated"].ToString();
            (Owner as MainWindow).Language = MainWindow.FileToString(Environment.CurrentDirectory + @"\Localization\" + comboBox1.Text);
            (Owner as MainWindow).Jo = (JObject)JsonConvert.DeserializeObject((Owner as MainWindow).Language);
            LanguageLabel.Text = (Owner as MainWindow).Jo["Language"].ToString();
            groupBox1.Text = (Owner as MainWindow).Jo["Hotkeys"].ToString();
            ClearButton1.Text = (Owner as MainWindow).Jo["Clear"].ToString();
            ClearButton2.Text = ClearButton1.Text;
            ApplyButton.Text = (Owner as MainWindow).Jo["Apply"].ToString();
            CancelButton.Text = (Owner as MainWindow).Jo["Cancel"].ToString();
            Text = (Owner as MainWindow).Jo["Settings"].ToString();
            AboutLink.Text = (Owner as MainWindow).Jo["About"].ToString();
            label2.Text = (Owner as MainWindow).Jo["Load"].ToString();
            label3.Text = (Owner as MainWindow).Jo["Save"].ToString();
            label1.Text = (Owner as MainWindow).Jo["File"].ToString();
            button1.Text = (Owner as MainWindow).Jo["CheckUpdate"].ToString();
            label4.Text = (Owner as MainWindow).Jo["SoundEffectType"].ToString();
            radioButton1.Text = (Owner as MainWindow).Jo["Ting"].ToString();
            radioButton2.Text = (Owner as MainWindow).Jo["Google"].ToString();
            radioButton3.Text = (Owner as MainWindow).Jo["Mute"].ToString();
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            int LoadMValue = 0;
            int SaveMValue = 0;
            if (checkBox1.Checked == true)
                LoadMValue += 2;
            if (checkBox2.Checked == true)
                LoadMValue += 4;
            if (checkBox3.Checked == true)
                LoadMValue += 1;
            if (checkBox8.Checked == true)
                SaveMValue += 2;
            if (checkBox7.Checked == true)
                SaveMValue += 4;
            if (checkBox6.Checked == true)
                SaveMValue += 1;
            Settings1.Default.LoadModifier = LoadMValue;
            Settings1.Default.SaveModifier = SaveMValue;
            if (LoadHotkey.Text != "Back" && LoadHotkey.Text != "Space" && LoadHotkey.Text != "" && LoadHotkey.Text != "None")
            {
                char[] Temp = LoadHotkey.Text.ToCharArray();
                Settings1.Default.LoadHotkey = (Keys)Temp[0];
            }
            else if (LoadHotkey.Text == "Back")
            {
                Settings1.Default.LoadHotkey = Keys.Back;
            }
            else if (LoadHotkey.Text == "Space")
            {
                Settings1.Default.LoadHotkey = Keys.Space;
            }
            else
            {
                Settings1.Default.LoadHotkey = Keys.None;
            }

            if (SaveHotKey.Text != "Back" && SaveHotKey.Text != "Space" && SaveHotKey.Text != "" && SaveHotKey.Text != "None")
            {
                char[] Temp = SaveHotKey.Text.ToCharArray();
                Settings1.Default.SaveHotkey = (Keys)Temp[0];
            }
            else if (SaveHotKey.Text == "Back")
            {
                Settings1.Default.SaveHotkey = Keys.Back;
            }
            else if (SaveHotKey.Text == "Space")
            {
                Settings1.Default.SaveHotkey = Keys.Space;
            }
            else
            {
                Settings1.Default.SaveHotkey = Keys.None;
            }

            Settings1.Default.Language = comboBox1.Text;
            Settings1.Default.GameDirectory = textBox4.Text;
            Settings1.Default.Save();
            (Owner as MainWindow).ReTranslateForm();
            Close();
        }

        private void AboutLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/ShenKSPZ/SekiroSaveLoadManager");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show((Owner as MainWindow).Jo["CannotTransferAccount"].ToString());
            openFileDialog1.Title = (Owner as MainWindow).Jo["File"].ToString();
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "openFileDialog1")
            {
                textBox4.Text = openFileDialog1.FileName;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Settings1.Default.Reload();
            (Owner as MainWindow).ReTranslateForm();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sourcecode = GetHtmlWithUtf("https://shenkspz.wixsite.com/collection/blank");
            Log.log(sourcecode);
            if (sourcecode != null)
            {
                string VA = GetBetweenArr(sourcecode, "只狼存档工具 <span style=\"font-size:17px;\">", "</span>");
                if (VA != "")
                {
                    if (VA != Version)
                    {
                        Log.log(VA);
                        if ((int)MessageBox.Show((Owner as MainWindow).Jo["NewVersion"].ToString() + VA, (Owner as MainWindow).Jo["SekiroSL"].ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == 6)
                        {
                            System.Diagnostics.Process.Start("https://github.com/ShenKSPZ/SekiroSaveLoadManager/release");
                        }
                    }
                    else
                    {
                        MessageBox.Show((Owner as MainWindow).Jo["AlreadyNewest"].ToString(), (Owner as MainWindow).Jo["SekiroSL"].ToString());
                    }
                }
                else
                {
                    Log.log("Can't get source code");
                }
            }

        }

        public string GetHttpWebRequest(string url)
        {
            try
            {
                Uri uri = new Uri(url);
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(uri);
                myReq.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
                myReq.Accept = "*/*";
                myReq.KeepAlive = true;
                myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
                HttpWebResponse result = (HttpWebResponse)myReq.GetResponse();
                Stream receviceStream = result.GetResponseStream();
                StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("utf-8"));
                string strHTML = readerOfStream.ReadToEnd();
                readerOfStream.Close();
                receviceStream.Close();
                result.Close();
                return strHTML;
            }
            catch (Exception ex)
            {
                throw new Exception("采集指定网址异常，" + ex.Message);
            }
        }

        public static string GetHtmlWithUtf(string url)
        {
            if (!(url.Contains("http://") || url.Contains("https://")))
            {
                url = "http://" + url;
            }
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.UserAgent = "User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
            req.Accept = "*/*";
            req.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
            req.ContentType = "text/xml";

            string sHTML = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            {
                if (response.ContentEncoding.ToLower().Contains("gzip"))
                {
                    using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            sHTML = reader.ReadToEnd();
                        }
                    }
                }
                else if (response.ContentEncoding.ToLower().Contains("deflate"))
                {
                    using (DeflateStream stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress))
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            sHTML = reader.ReadToEnd();
                        }
                    }
                }
                else
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            sHTML = reader.ReadToEnd();
                        }
                    }
                }
            }
            return sHTML;
        }

        public string GetBetweenArr(string str, string leftstr, string rightstr)
        {
            int leftIndex = str.IndexOf(leftstr);//左文本起始位置
            int leftlength = leftstr.Length;//左文本长度
            int rightIndex = 0;
            string temp = "";
            while (leftIndex != -1)
            {
                rightIndex = str.IndexOf(rightstr, leftIndex + leftlength);
                if (rightIndex == -1)
                {
                    break;
                }
                temp = str.Substring(leftIndex + leftlength, rightIndex - leftIndex - leftlength);
                leftIndex = str.IndexOf(leftstr, rightIndex + 1);
            }
            return temp;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Settings1.Default.SoundType = "Mute";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Settings1.Default.SoundType = "Google";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Settings1.Default.SoundType = "Ting";
        }

        private void LoadHotkey_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != 32 && e.KeyChar != 27)
            {
                LoadHotkey.Text = e.KeyChar.ToString().ToUpper();
                e.Handled = true;
            }
            else if (e.KeyChar == 8)
            {
                LoadHotkey.Text = "Back";
                e.Handled = true;
            }
            else if (e.KeyChar == 32)
            {
                LoadHotkey.Text = "Space";
                e.Handled = true;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox4.Checked = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox4.Checked = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                checkBox4.Checked = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
            {
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                checkBox8.Checked = false;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked == true)
            {
                checkBox5.Checked = false;
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked == true)
            {
                checkBox5.Checked = false;
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked == true)
            {
                checkBox5.Checked = false;
            }
        }

        private void SaveHotKey_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != 32 && e.KeyChar != 27)
            {
                SaveHotKey.Text = e.KeyChar.ToString().ToUpper();
                e.Handled = true;
            }
            else if (e.KeyChar == 8)
            {
                SaveHotKey.Text = "Back";
                e.Handled = true;
            }
            else if (e.KeyChar == 32)
            {
                SaveHotKey.Text = "Space";
                e.Handled = true;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void ClearButton1_Click(object sender, EventArgs e)
        {
            LoadHotkey.Text = "None";
            checkBox4.Checked = true;
        }

        private void ClearButton2_Click(object sender, EventArgs e)
        {
            SaveHotKey.Text = "None";
            checkBox5.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
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
                    openFileDialog1.ShowDialog();
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
            textBox4.Text = Settings1.Default.GameDirectory;
        }
    }
}