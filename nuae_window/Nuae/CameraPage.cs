using Accord.Video.FFMPEG;
using AForge.Controls;
using Nuae;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nuae
{
    public partial class CameraPage : Form
    {
        /* CameraPage UI 접근용 */
        public static CameraPage cameraPage;

        /* 활성화 되어있는 페이지 */
        private Form activeSoloShot = null;

        /* 각 카메라에 들어오는 이미지를 받을 Bitmap들 */
        Bitmap bitmap1, bitmap2, bitmap3, bitmap4, bitmap5, bitmap6 = null;

        public CameraPage()
        {
            InitializeComponent();
            cameraPage = this;
        }

        /// <summary>
        /// childForm을 MainPage의 contentPanel에 표시해 줍니다
        /// </summary>
        /// <param name="childForm">표시하려는 페이지</param>
        private void openChildForm(Form childForm)
        {
            if (activeSoloShot != null) activeSoloShot.Close();
            activeSoloShot = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            MainPage.mainPage.contentPanel.Controls.Add(childForm);
            MainPage.mainPage.contentPanel.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }


        #region 카메라 더블 클릭시 큰 화면으로 표시
        private void videoSourcePlayer1_DoubleClick(object sender, System.EventArgs e)
        {
            if (videoSourcePlayer1.IsRunning)
                openChildForm(new OneCameraPage(1));
        }

        private void videoSourcePlayer2_DoubleClick(object sender, System.EventArgs e)
        {
            if (videoSourcePlayer2.IsRunning)
                openChildForm(new OneCameraPage(2));
        }

        private void videoSourcePlayer3_DoubleClick(object sender, System.EventArgs e)
        {
            if (videoSourcePlayer3.IsRunning)
                openChildForm(new OneCameraPage(3));
        }

        private void videoSourcePlayer4_DoubleClick(object sender, System.EventArgs e)
        {
            if (videoSourcePlayer4.IsRunning)
                openChildForm(new OneCameraPage(4));
        }

        private void videoSourcePlayer5_DoubleClick(object sender, System.EventArgs e)
        {
            if (videoSourcePlayer5.IsRunning)
                openChildForm(new OneCameraPage(5));
        }

        private void videoSourcePlayer6_DoubleClick(object sender, System.EventArgs e)
        {
            if (videoSourcePlayer6.IsRunning)
                openChildForm(new OneCameraPage(6));
        }
        #endregion


        #region 카메라 1~6의 녹화 시작, 종료 버튼
        private void cam1RecordStartButton_Click(object sender, System.EventArgs e)
        {
            if (videoSourcePlayer1 == null)
                return;
            
            Camera.OpenFiles(1);
            Camera.recordings[0] = true;

            if (!MainPage.mainPage.saveTimer.Enabled)
                MainPage.mainPage.saveTimer.Enabled = true;

            cam1RecordStartButton.BackgroundImage = Properties.Resources.start_button_clicked;
            cam1RecordStopButton.BackgroundImage = Properties.Resources.stop_button;
        }

        private void cam1RecordStopButton_Click(object sender, System.EventArgs e)
        {
            if (!Camera.recordings[0])
                return;

            cam1RecordStartButton.BackgroundImage = Properties.Resources.start_button;
            cam1RecordStopButton.BackgroundImage = Properties.Resources.stop_button_clicked;

            Camera.recordings[0] = false;
            Camera.CamWriters[0].Close();
        }

        private void cam2RecordStartButton_Click(object sender, System.EventArgs e)
        {
            if (videoSourcePlayer2 == null)
                return;
            
            Camera.OpenFiles(2);
            Camera.recordings[1] = true;

            if (!MainPage.mainPage.saveTimer.Enabled)
                MainPage.mainPage.saveTimer.Enabled = true;

            cam2RecordStartButton.BackgroundImage = Properties.Resources.start_button_clicked;
            cam2RecordEndButton.BackgroundImage = Properties.Resources.stop_button;
        }

        private void cam2RecordEndButton_Click(object sender, System.EventArgs e)
        {
            if (!Camera.recordings[1])
                return;

            cam2RecordStartButton.BackgroundImage = Properties.Resources.start_button;
            cam2RecordEndButton.BackgroundImage = Properties.Resources.stop_button_clicked;

            Camera.recordings[1] = false;
            Camera.CamWriters[1].Close();
        }

        private void cam3RecordStartButton_Click(object sender, System.EventArgs e)
        {
            if (videoSourcePlayer3 == null)
                return;
            
            Camera.OpenFiles(3);
            Camera.recordings[2] = true;
            if (!MainPage.mainPage.saveTimer.Enabled)
                MainPage.mainPage.saveTimer.Enabled = true;
            cam3RecordStartButton.BackgroundImage = Properties.Resources.start_button_clicked;
            cam3RecordEndButton.BackgroundImage = Properties.Resources.stop_button;
        }

        private void cam3RecordEndButton_Click(object sender, System.EventArgs e)
        {
            if (!Camera.recordings[2])
                return;

            cam3RecordStartButton.BackgroundImage = Properties.Resources.start_button;
            cam3RecordEndButton.BackgroundImage = Properties.Resources.stop_button_clicked;

            Camera.recordings[2] = false;
            Camera.CamWriters[2].Close();

        }

        private void cam4RecordStartButton_Click(object sender, System.EventArgs e)
        {
            if (videoSourcePlayer4 == null)
                return;
            
            Camera.OpenFiles(4);
            Camera.recordings[3] = true;
            if (!MainPage.mainPage.saveTimer.Enabled)
                MainPage.mainPage.saveTimer.Enabled = true;
            cam4RecordStartButton.BackgroundImage = Properties.Resources.start_button_clicked;
            cam4RecordEndButton.BackgroundImage = Properties.Resources.stop_button;

        }

        private void cam4RecordEndButton_Click(object sender, System.EventArgs e)
        {
            if (!Camera.recordings[3])
                return;

            cam4RecordStartButton.BackgroundImage = Properties.Resources.start_button;
            cam4RecordEndButton.BackgroundImage = Properties.Resources.stop_button_clicked;

            Camera.recordings[3] = false;
            Camera.CamWriters[3].Close();
      
        }

        private void cam5RecordStartButton_Click(object sender, System.EventArgs e)
        {
            if (videoSourcePlayer5 == null)
                return;
            
            Camera.OpenFiles(5);
            Camera.recordings[4] = true;
            if (!MainPage.mainPage.saveTimer.Enabled)
                MainPage.mainPage.saveTimer.Enabled = true;
            cam5RecordStartButton.BackgroundImage = Properties.Resources.start_button_clicked;
            cam5RecordEndButton.BackgroundImage = Properties.Resources.stop_button;

        }

        private void cam5RecordEndButton_Click(object sender, System.EventArgs e)
        {
            if (!Camera.recordings[4])
                return;

            cam5RecordStartButton.BackgroundImage = Properties.Resources.start_button;
            cam5RecordEndButton.BackgroundImage = Properties.Resources.stop_button_clicked;

            Camera.recordings[4] = false;
            Camera.CamWriters[4].Close();
        }

        private void cam6RecordStartButton_Click(object sender, System.EventArgs e)
        {
            if (videoSourcePlayer6 == null)
                return;
            
            Camera.OpenFiles(6);
            Camera.recordings[5] = true;
            if (!MainPage.mainPage.saveTimer.Enabled)
                MainPage.mainPage.saveTimer.Enabled = true;
            cam6RecordStartButton.BackgroundImage = Properties.Resources.start_button_clicked;
            cam6RecordEndButton.BackgroundImage = Properties.Resources.stop_button;

        }

        private void cam6RecordEndButton_Click(object sender, System.EventArgs e)
        {
            if (!Camera.recordings[5])
                return;

            cam6RecordStartButton.BackgroundImage = Properties.Resources.start_button;
            cam6RecordEndButton.BackgroundImage = Properties.Resources.stop_button_clicked;

            Camera.recordings[5] = false;
            Camera.CamWriters[5].Close();
        }
        #endregion


        #region 카메라 1~6의 해상도 변경
        private void cam1ResolutionsCombo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (videoSourcePlayer1 != null && videoSourcePlayer1.IsRunning)
            {
                if (Camera.recordings[0])
                {
                    MessageBox.Show("녹화 중에는 해상도를 변경할 수 없습니다\n녹화를 종료한 후에 변경해 주세요");
                    cam1ResolutionsCombo.SelectedItem = Camera.cameras[0].VideoResolution.FrameSize.ToString();
                    return;
                }
                ComboBox comboBox = (ComboBox)sender;
                Camera.cameras[0].VideoResolution = Camera.cameras[0].VideoCapabilities[comboBox.SelectedIndex];
                videoSourcePlayer1.Stop();
                videoSourcePlayer1.Start();

                Camera.resolution_index_strings[0] = comboBox.SelectedIndex.ToString();
                Camera.WriteResolutionTXT(1, comboBox.SelectedIndex);
            }
        }

        private void cam2ResolutionsCombo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (videoSourcePlayer2 != null && videoSourcePlayer2.IsRunning)
            {
                if (Camera.recordings[1])
                {
                    MessageBox.Show("녹화 중에는 해상도를 변경할 수 없습니다\n녹화를 종료한 후에 변경해 주세요");
                    cam1ResolutionsCombo.SelectedItem = Camera.cameras[1].VideoResolution.FrameSize.ToString();
                    return;
                }
                ComboBox comboBox = (ComboBox)sender;
                Camera.cameras[1].VideoResolution = Camera.cameras[1].VideoCapabilities[comboBox.SelectedIndex];
                videoSourcePlayer2.Stop();
                videoSourcePlayer2.Start();

                Camera.resolution_index_strings[1] = comboBox.SelectedIndex.ToString();
                Camera.WriteResolutionTXT(2, comboBox.SelectedIndex);
            }
        }

        private void cam3ResolutionsCombo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (videoSourcePlayer3 != null && videoSourcePlayer3.IsRunning)
            {
                if (Camera.recordings[2])
                {
                    MessageBox.Show("녹화 중에는 해상도를 변경할 수 없습니다\n녹화를 종료한 후에 변경해 주세요");
                    cam1ResolutionsCombo.SelectedItem = Camera.cameras[2].VideoResolution.FrameSize.ToString();
                    return;
                }
                ComboBox comboBox = (ComboBox)sender;
                Camera.cameras[2].VideoResolution = Camera.cameras[2].VideoCapabilities[comboBox.SelectedIndex];
                videoSourcePlayer3.Stop();
                videoSourcePlayer3.Start();

                Camera.resolution_index_strings[2] = comboBox.SelectedIndex.ToString();
                Camera.WriteResolutionTXT(3, comboBox.SelectedIndex);
            }
        }

        private void cam4ResolutionsCombo_SelectedIndexChanged(object sender, System.EventArgs e)
        {


            if (videoSourcePlayer4 != null && videoSourcePlayer4.IsRunning)
            {
                if (Camera.recordings[3])
                {
                    MessageBox.Show("녹화 중에는 해상도를 변경할 수 없습니다\n녹화를 종료한 후에 변경해 주세요");
                    cam1ResolutionsCombo.SelectedItem = Camera.cameras[3].VideoResolution.FrameSize.ToString();
                    return;
                }
                ComboBox comboBox = (ComboBox)sender;
                Camera.cameras[3].VideoResolution = Camera.cameras[3].VideoCapabilities[comboBox.SelectedIndex];
                videoSourcePlayer4.Stop();
                videoSourcePlayer4.Start();

                Camera.resolution_index_strings[3] = comboBox.SelectedIndex.ToString();
                Camera.WriteResolutionTXT(4, comboBox.SelectedIndex);
            }
        }

        private void cam5ResolutionsCombo_SelectedIndexChanged(object sender, System.EventArgs e)
        {


            if (videoSourcePlayer5 != null && videoSourcePlayer5.IsRunning)
            {
                if (Camera.recordings[4])
                {
                    MessageBox.Show("녹화 중에는 해상도를 변경할 수 없습니다\n녹화를 종료한 후에 변경해 주세요");
                    cam1ResolutionsCombo.SelectedItem = Camera.cameras[4].VideoResolution.FrameSize.ToString();
                    return;
                }
                ComboBox comboBox = (ComboBox)sender;
                Camera.cameras[4].VideoResolution = Camera.cameras[4].VideoCapabilities[comboBox.SelectedIndex];
                videoSourcePlayer5.Stop();
                videoSourcePlayer5.Start();

                Camera.resolution_index_strings[4] = comboBox.SelectedIndex.ToString();
                Camera.WriteResolutionTXT(5, comboBox.SelectedIndex);
            }
        }

        private void cam6ResolutionsCombo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (videoSourcePlayer6 != null && videoSourcePlayer6.IsRunning)
            {
                if (Camera.recordings[5])
                {
                    MessageBox.Show("녹화 중에는 해상도를 변경할 수 없습니다\n녹화를 종료한 후에 변경해 주세요");
                    cam1ResolutionsCombo.SelectedItem = Camera.cameras[5].VideoResolution.FrameSize.ToString();
                    return;
                }
                ComboBox comboBox = (ComboBox)sender;
                Camera.cameras[5].VideoResolution = Camera.cameras[5].VideoCapabilities[comboBox.SelectedIndex];
                videoSourcePlayer6.Stop();
                videoSourcePlayer6.Start();

                Camera.resolution_index_strings[5] = comboBox.SelectedIndex.ToString();
                Camera.WriteResolutionTXT(6, comboBox.SelectedIndex);
            }
        }
        #endregion


        /// <summary>
        /// 해상도 파일에 저장된 각 카메라의 프레임 크기 인덱스를 콤보박스의 첫번째 칸에 표시
        /// </summary>
        private void CameraPage_Load(object sender, System.EventArgs e)
        {
            if (videoSourcePlayer1 != null && videoSourcePlayer1.IsRunning)
                cam1ResolutionsCombo.SelectedItem = Camera.cameras[0].VideoCapabilities[int.Parse(Camera.resolution_index_strings[0])].FrameSize.ToString();
            if (videoSourcePlayer2 != null && videoSourcePlayer2.IsRunning)
                cam2ResolutionsCombo.SelectedItem = Camera.cameras[1].VideoCapabilities[int.Parse(Camera.resolution_index_strings[1])].FrameSize.ToString();
            if (videoSourcePlayer3 != null && videoSourcePlayer3.IsRunning)
                cam3ResolutionsCombo.SelectedItem = Camera.cameras[2].VideoCapabilities[int.Parse(Camera.resolution_index_strings[2])].FrameSize.ToString();
            if (videoSourcePlayer4 != null && videoSourcePlayer4.IsRunning)
                cam4ResolutionsCombo.SelectedItem = Camera.cameras[3].VideoCapabilities[int.Parse(Camera.resolution_index_strings[3])].FrameSize.ToString();
            if (videoSourcePlayer5 != null && videoSourcePlayer5.IsRunning)
                cam5ResolutionsCombo.SelectedItem = Camera.cameras[4].VideoCapabilities[int.Parse(Camera.resolution_index_strings[4])].FrameSize.ToString();
            if (videoSourcePlayer6 != null && videoSourcePlayer6.IsRunning)
                cam6ResolutionsCombo.SelectedItem = Camera.cameras[5].VideoCapabilities[int.Parse(Camera.resolution_index_strings[5])].FrameSize.ToString();
        }

        /// <summary>
        /// VideoSourcePlayer에 Camera와 New Frame함수를 추가합니다
        /// </summary>
        public void SetVideoSourcePlayer()
        {
            for (int i = 0; i < Camera.numberOfCamera; i++)
            {
                switch (i)
                {
                    case 0:
                        if (videoSourcePlayer1.VideoSource == null)
                        {
                            videoSourcePlayer1.VideoSource = Camera.cameras[0];
                            videoSourcePlayer1.NewFrame += VideoSourcePlayer_NewFrame1Async;
                        }
                        break;
                    case 1:
                        if (videoSourcePlayer2.VideoSource == null)
                        {
                            videoSourcePlayer2.VideoSource = Camera.cameras[1];
                            videoSourcePlayer2.NewFrame += VideoSourcePlayer_NewFrame2;
                        }
                        break;
                    case 2:
                        if (videoSourcePlayer3.VideoSource == null)
                        {
                            videoSourcePlayer3.VideoSource = Camera.cameras[2];
                            videoSourcePlayer3.NewFrame += VideoSourcePlayer_NewFrame3;
                        }
                        break;
                    case 3:
                        if (videoSourcePlayer4.VideoSource == null)
                        {
                            videoSourcePlayer4.VideoSource = Camera.cameras[3];
                            videoSourcePlayer4.NewFrame += VideoSourcePlayer_NewFrame4;
                        }
                        break;
                    case 4:
                        if (videoSourcePlayer5.VideoSource == null)
                        {
                            videoSourcePlayer5.VideoSource = Camera.cameras[4];
                            videoSourcePlayer5.NewFrame += VideoSourcePlayer_NewFrame5;
                        }
                        break;
                    case 5:
                        if (videoSourcePlayer6.VideoSource == null)
                        {
                            videoSourcePlayer6.VideoSource = Camera.cameras[5];
                            videoSourcePlayer6.NewFrame += VideoSourcePlayer_NewFrame6;
                        }
                        break;
                }
            }
        }

        #region 각 카메라의 이미지가 들어오는 함수들 입니다
        [HandleProcessCorruptedStateExceptions]
        public void VideoSourcePlayer_NewFrame1Async(object sender, ref Bitmap image)
        {
            Thread.Sleep(32);
            if (Camera.recordings[0])
            {
                try
                {
                    bitmap1 = new Bitmap(image);
                }
                catch (Exception)
                {
                    return;
                }
                try
                {
                    if(Camera.CamWriters[0].IsOpen)
                    {
                        Camera.CamWriters[0].WriteVideoFrame(bitmap1);
                    }
                }
                catch (Exception)
                {
                }
                bitmap1.Dispose();
            }
        }

        [HandleProcessCorruptedStateExceptions]
        public void VideoSourcePlayer_NewFrame2(object sender, ref Bitmap image)
        {
            Thread.Sleep(32);
            if (Camera.recordings[1])
            {
                try
                {
                    bitmap2 = new Bitmap(image);
                }
                catch (Exception)
                {
                    return;
                }
                try
                {
                    if (Camera.CamWriters[1].IsOpen)
                    {
                        Camera.CamWriters[1].WriteVideoFrame(bitmap2);
                    }
                }
                catch (Exception)
                {
                }
                bitmap2.Dispose();
            }
        }
        [HandleProcessCorruptedStateExceptions]
        public void VideoSourcePlayer_NewFrame3(object sender, ref Bitmap image)
        {
            Thread.Sleep(32);
            if (Camera.recordings[2])
            {
                try
                {
                    bitmap3 = new Bitmap(image);
                }
                catch (Exception)
                {
                    return;
                }
                try
                {
                    if (Camera.CamWriters[2].IsOpen)
                    {
                        Camera.CamWriters[2].WriteVideoFrame(bitmap3);
                    }
                }
                catch (Exception)
                {
                }
                bitmap3.Dispose();
            }
        }
        [HandleProcessCorruptedStateExceptions]
        public void VideoSourcePlayer_NewFrame4(object sender, ref Bitmap image)
        {
            Thread.Sleep(32);
            if (Camera.recordings[3])
            {
                try
                {
                    bitmap4 = new Bitmap(image);
                }
                catch (Exception)
                {
                    return;
                }
                try
                {
                    if (Camera.CamWriters[3].IsOpen)
                    {
                        Camera.CamWriters[3].WriteVideoFrame(bitmap4);
                    }
                }
                catch (Exception)
                {
                }
                bitmap4.Dispose();
            }
        }
        [HandleProcessCorruptedStateExceptions]

        public void VideoSourcePlayer_NewFrame5(object sender, ref Bitmap image)
        {
            Thread.Sleep(32);
            if (Camera.recordings[4])
            {
                try
                {
                    bitmap5 = new Bitmap(image);
                }
                catch (Exception)
                {
                    return;
                }
                try
                {
                    if (Camera.CamWriters[4].IsOpen)
                    {
                        Camera.CamWriters[4].WriteVideoFrame(bitmap5);
                    }
                }
                catch (Exception)
                {
                }
                bitmap5.Dispose();
            }
        }
        [HandleProcessCorruptedStateExceptions]

        public void VideoSourcePlayer_NewFrame6(object sender, ref Bitmap image)
        {
            Thread.Sleep(32);
            if (Camera.recordings[5])
            {
                try
                {
                    bitmap6 = new Bitmap(image);
                }
                catch (Exception)
                {
                    return;
                }
                try
                {
                    if (Camera.CamWriters[5].IsOpen)
                    {
                        Camera.CamWriters[5].WriteVideoFrame(bitmap6);
                    }
                }
                catch (Exception)
                {
                }
                bitmap6.Dispose();
            }
        }
        #endregion
    }
}
