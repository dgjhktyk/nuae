using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Nuae
{
    public partial class SettingPage : Form
    {
        public SettingPage()
        {
            InitializeComponent();
            size_limit_input_text_box.Text = (MainPage.size_limit / 1073741824).ToString();
            save_location_label.Text = Paths.basePath;
        }

        private void size_confrim_button_Click(object sender, EventArgs e)
        {
            // 녹화중이면 녹화 종료 요구
            foreach (var rec in Camera.recordings)
            {
                if (rec)
                {
                    MessageBox.Show("녹화를 종료한 후에 설정해 주세요");
                    return;
                }
            }

            MainPage.size_limit = long.Parse(size_limit_input_text_box.Text) * 1073741824;
            File.WriteAllText(Paths.size_limit_file_path, (MainPage.size_limit / 1073741824).ToString());
            MessageBox.Show("용량 제한 설정이 완료 되었습니다.");
        }

        private void setting_location_button_Click(object sender, EventArgs e)
        {
            // 녹화중이면 녹화 종료 요구
            foreach(var rec in Camera.recordings)
            {
                if(rec)
                {
                    MessageBox.Show("녹화를 종료한 후에 설정해 주세요");
                    return;
                }
            }
            // 위치 재설정
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                var folderName = folderBrowserDialog1.SelectedPath;
                string new_base_path = folderName + @"\NuaeVideosAforge";
                save_location_label.Text = new_base_path;
                Paths.basePath = new_base_path;
                File.WriteAllText(Paths.save_location_file_path, Paths.basePath);
                MessageBox.Show("저장 위치 설정이 완료 되었습니다. 잠시후 재시작 합니다");
                Thread.Sleep(2000);
                Application.Restart();
                Environment.Exit(0);
            }
        }


        public static void SizeLimitFile()
        {
            if (File.Exists(Paths.size_limit_file_path))
            {
                MainPage.size_limit = long.Parse(File.ReadAllText(Paths.size_limit_file_path)) * 1073741824;
            }
            else
            {
                File.WriteAllText(Paths.size_limit_file_path, (MainPage.size_limit / 1073741824).ToString());
            }
        }

        public static void SaveLocationFile()
        {
            if (File.Exists(Paths.save_location_file_path))
            {
                Paths.basePath = File.ReadAllText(Paths.save_location_file_path);
            }
            else
            {
                File.WriteAllText(Paths.save_location_file_path, Paths.basePath);
            }
        }
    }
}
