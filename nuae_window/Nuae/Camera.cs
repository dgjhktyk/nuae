using Accord.Video.FFMPEG;
using AForge.Controls;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Windows.Forms;

namespace Nuae
{
    public class Camera
    {
        /* 연결된 카메라 정보를 담을 애 */
        public static FilterInfoCollection filters;

        /* 생성된 카메라 객체를 담을 애 */
        public static List<VideoCaptureDevice> cameras = new List<VideoCaptureDevice>();

        /* 영상 만드는 */
        public static List<VideoFileWriter> CamWriters = new List<VideoFileWriter>();

        /* 카메라 개수 */
        public static int numberOfCamera = 0;

        /* 각 카메라 녹화 여부 */
        public static bool[] recordings = new bool[6] { false, false, false, false, false, false };

        /* 
        * 녹화 파일을 최종 생성할때 영상 파일에 이미지를 쓰는 것을 멈추는데 그때 recordings를 false로 바꾸고 있습니다
        * 그런데 파일을 만들고 나서는 다시 이미지를 영상 파일에 써야 하기 때문에
        * 녹화중이던 카메라를 기록할 필요가 있었고 여기에 그 정보를 기록하고 있습니다*/
        public static bool[] recording_record = new bool[6] { false, false, false, false, false, false };

        /* 각 카메라의 선택된 해상도 인덱스 값 */
        public static string[] resolution_index_strings = new string[6] { "0", "0", "0", "0", "0", "0" };


        /// <summary>
        /// 어떤 카메라가 분리되었는지 확인합니다
        /// </summary>
        /// <returns>
        /// ret : 분리된 카메라의 cameras 배열에서의 인덱스
        /// </returns>
        public static int whoIsDisconnected()
        {
            int ret = 0;
            List<VideoCaptureDevice> new_cameras = new List<VideoCaptureDevice>();
            List<String> new_sources = new List<String>();
            filters = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo filterInfo in filters)
            {
                VideoCaptureDevice camera = new VideoCaptureDevice(filterInfo.MonikerString);
                new_cameras.Add(camera);
                new_sources.Add(camera.Source);
            }

            foreach (VideoCaptureDevice camera in cameras)
            {
                if (camera == null)
                    continue;
                if (new_sources.Contains(camera.Source))
                {
                    continue;
                }
                else
                {
                    ret = cameras.IndexOf(camera);
                    cameras[ret] = null;
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// 카메라가 추가되거나 분리되었을 때 호출됩니다
        /// </summary>
        /// <returns></returns>
        public static int isCameraAdded()
        {
            filters = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            int activeCamera = 0;

            for(int i = 0; i < numberOfCamera; i++)
            {
                if(cameras[i] != null)
                {
                    activeCamera++;
                }
            }

            if (activeCamera > filters.Count)
            {
                // 분리됨
                return 0;
            }
            else if (activeCamera < filters.Count)
            {
                // 추가됨
                return 1;
            }
            // 에러
            return -1;
        }

        /// <summary>
        /// 카메라 검색 함수에서 기존에 연결된 카메라는 추가하지 않기 위하여
        /// 현재 cameras 배열에 있는 카메라인지를 확인하는 함수입니다
        /// 
        /// cameras 배열에는 기존에 검색된 카메라들이 담겨 있습니다
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        static bool isInCheck(string src)
        {
            if(cameras.Count == 0)
            { return false; }
            for (int i = 0; i < cameras.Count; i++)
            {
                if (cameras[i] == null)
                    continue;
                if (cameras[i].Source == src)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 카메라가 갖고 있는 해상도 정보를 반환해 줍니다
        /// 현재는 가로가 1024 이하인 해상도만 반환하고 있습니다
        /// </summary>
        /// <param name="vcs">각 카메라가 갖고 있는 해상도 정보</param>
        /// <returns></returns>
        public static List<VideoCapabilities> GetVideoCapabilities(VideoCapabilities[] vcs)
        {
            List<VideoCapabilities> videoCapabilities = new List<VideoCapabilities>();
            foreach (VideoCapabilities vc in vcs)
            {
                if (vc.FrameSize.Width <= 1024)
                {
                    videoCapabilities.Add(vc);
                }
            }
            return videoCapabilities;
        }

        /// <summary>
        /// 카메라를 검색하고 카메라 페이지의 해상도 콤보박스에 카메라의 해상도를 넣습니다
        /// </summary>
        public static void SearchCamera()
        {
            int null_idx = 1000;
            filters = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (filters.Count == 0)
            {
                MessageBox.Show("카메라가 없습니다");
                return;
            }

            numberOfCamera = filters.Count;

            foreach (FilterInfo filterInfo in filters)
            {
                VideoCaptureDevice camera = new VideoCaptureDevice(filterInfo.MonikerString);
                if (camera.SnapshotCapabilities.Length == 0)
                {
                    numberOfCamera--;
                    continue;
                }
                if (!isInCheck(camera.Source))
                {
                    for(int i = 0; i < cameras.Count; i++)
                    {
                        if (cameras[i] == null)
                        {
                            null_idx = i;
                            break;
                        }
                    }
                    if (null_idx == 1000)
                    {
                        cameras.Add(camera);
                        CamWriters.Add(new VideoFileWriter());
                    } 
                    else
                    {
                        cameras[null_idx] = camera;
                        CamWriters[null_idx] = new VideoFileWriter();
                    }
                }
            }

            for (int i = 0; i < numberOfCamera; i++)
            {
                if (cameras[i] == null)
                    continue;
                cameras[i].VideoResolution = cameras[i].VideoCapabilities[int.Parse(resolution_index_strings[i])];
                switch (i)
                {
                    case 0:
                        if (MainPage.cameraPage.cam1ResolutionsCombo.DataSource == null)
                        {
                            MainPage.cameraPage.cam1ResolutionsCombo.DataSource = GetResolutions(GetVideoCapabilities(cameras[0].VideoCapabilities));
                        }
                        break;
                    case 1:
                        if (MainPage.cameraPage.cam2ResolutionsCombo.DataSource == null)
                        {
                            MainPage.cameraPage.cam2ResolutionsCombo.DataSource = GetResolutions(GetVideoCapabilities(cameras[1].VideoCapabilities));
                        }
                        break;
                    case 2:
                        if (MainPage.cameraPage.cam3ResolutionsCombo.DataSource == null)
                        {
                            MainPage.cameraPage.cam3ResolutionsCombo.DataSource = GetResolutions(GetVideoCapabilities(cameras[2].VideoCapabilities));
                        }

                        break;
                    case 3:
                        if (MainPage.cameraPage.cam4ResolutionsCombo.DataSource == null)
                        {
                            MainPage.cameraPage.cam4ResolutionsCombo.DataSource = GetResolutions(GetVideoCapabilities(cameras[3].VideoCapabilities));
                        }
                        break;
                    case 4:
                        if (MainPage.cameraPage.cam5ResolutionsCombo.DataSource == null)
                        {
                            MainPage.cameraPage.cam5ResolutionsCombo.DataSource = GetResolutions(GetVideoCapabilities(cameras[4].VideoCapabilities));
                        }
                        break;
                    case 5:
                        if (MainPage.cameraPage.cam6ResolutionsCombo.DataSource == null)
                        {
                            MainPage.cameraPage.cam6ResolutionsCombo.DataSource = GetResolutions(GetVideoCapabilities(cameras[5].VideoCapabilities));
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// GetVideoCapabilities에서 반환한 List<VideoCapabilities>을 콤보박스에 넣기 위하여
        /// List<string>으로 변환하는 함수입니다
        /// </summary>
        /// <param name="videoCapabilities">Frame Size들이 담긴</param>
        /// <returns></returns>
        public static List<string> GetResolutions(List<VideoCapabilities> videoCapabilities)
        {
            List<string> resolurions = new List<string>();

            foreach (VideoCapabilities capabilitie in videoCapabilities)
            {
                resolurions.Add(capabilitie.FrameSize.ToString());
            }
            return resolurions;
        }

        /// <summary>
        /// 현재 녹화로 설정된 카메라의 동영상 파일을 생성 합니다
        /// </summary>
        public static void OpenFiles()
        {
            int cam_number = 0;
            foreach (string dateFolderName in Paths.dateFolderByCamera)
            {
                if (Directory.Exists(dateFolderName) == false)
                    Directory.CreateDirectory(dateFolderName);
            }

            for (int i = 0; i < numberOfCamera; i++)
            {
                if (cameras[i] == null)
                    continue;
                if(!CamWriters[i].IsOpen)
                {
                    cam_number = i + 1;
                    Paths.videoFiles[i] = Path.Combine(Paths.dateFolderByCamera[i] + @"\" + "Cam" + cam_number + "_" + DateTime.Now.ToString("HH_mm_ss") + ".avi");
                    Size size = cameras[i].VideoResolution.FrameSize;

                    CamWriters[i].Open(Paths.videoFiles[i], size.Width, size.Height, 25, VideoCodec.Default, 5000000);
                }
            }
        }

        /// <summary>
        /// 카메라 페이지에서 녹화 버튼을 클릭 했을때 호출되어 영상 파일을 생성합니다
        /// </summary>
        /// <param name="n">카메라 번호</param>
        public static void OpenFiles(int n)
        {
            int cam_number = 0;
            foreach (string dateFolderName in Paths.dateFolderByCamera)
            {
                if (Directory.Exists(dateFolderName) == false)
                    Directory.CreateDirectory(dateFolderName);
            }

            for (int i = 0; i < cameras.Count; i++)
            {
                if (!CamWriters[i].IsOpen && i == n-1)
                {
                    cam_number = i + 1;
                    Paths.videoFiles[i] = Path.Combine(Paths.dateFolderByCamera[i] + @"\" + "Cam" + cam_number + "_" + DateTime.Now.ToString("HH_mm_ss") + ".avi");
                    Size size = cameras[i].VideoResolution.FrameSize;

                    CamWriters[i].Open(Paths.videoFiles[i], size.Width, size.Height, 25, VideoCodec.Default, 5000000);
                }
            }
        }

        /// <summary>
        /// VideoSourcePlayer를 시작합니다
        /// </summary>
        public static void VspStart()
        {
            for (int i = 0; i < numberOfCamera; i++)
            {
                switch (i)
                {
                    case 0:
                        CameraPage.cameraPage.videoSourcePlayer1.Start();
                        break;
                    case 1:
                        CameraPage.cameraPage.videoSourcePlayer2.Start();
                        break;
                    case 2:
                        CameraPage.cameraPage.videoSourcePlayer3.Start();
                        break;
                    case 3:
                        CameraPage.cameraPage.videoSourcePlayer4.Start();
                        break;
                    case 4:
                        CameraPage.cameraPage.videoSourcePlayer5.Start();
                        break;
                    case 5:
                        CameraPage.cameraPage.videoSourcePlayer6.Start();
                        break;
                }
            }
        }

        /// <summary>
        /// VideoSourcePlayer를 멈춥니다
        /// </summary>
        public static void VspStop()
        {
            if (CameraPage.cameraPage.videoSourcePlayer1 != null && CameraPage.cameraPage.videoSourcePlayer1.IsRunning)
            {
                CameraPage.cameraPage.videoSourcePlayer1.Stop();
            }

            if (CameraPage.cameraPage.videoSourcePlayer2 != null && CameraPage.cameraPage.videoSourcePlayer2.IsRunning)
            {
                CameraPage.cameraPage.videoSourcePlayer2.Stop();
            }

            if (CameraPage.cameraPage.videoSourcePlayer3 != null && CameraPage.cameraPage.videoSourcePlayer3.IsRunning)
            {
                CameraPage.cameraPage.videoSourcePlayer3.Stop();
            }

            if (CameraPage.cameraPage.videoSourcePlayer4 != null && CameraPage.cameraPage.videoSourcePlayer4.IsRunning)
            {
                CameraPage.cameraPage.videoSourcePlayer4.Stop();
            }

            if (CameraPage.cameraPage.videoSourcePlayer5 != null && CameraPage.cameraPage.videoSourcePlayer5.IsRunning)
            {
                CameraPage.cameraPage.videoSourcePlayer5.Stop();
            }

            if (CameraPage.cameraPage.videoSourcePlayer6 != null && CameraPage.cameraPage.videoSourcePlayer6.IsRunning)
            {
                CameraPage.cameraPage.videoSourcePlayer6.Stop();
            }
        }

        /// <summary>
        /// VideoSourcePlayer를 멈추고
        /// Camera도 멈춥니다
        /// </summary>
        public static void CameraFinish()
        {
            VspStop();
            foreach (VideoCaptureDevice camera in cameras)
            {
                if (camera == null)
                    continue;
                camera.Stop();
            }
        }

        /// <summary>
        /// 열려있는 CamWriters를 닫습니다
        /// </summary>
        public static void CloseFiles()
        {
            for (int i = 0; i < numberOfCamera; i++)
            {
                if (CamWriters[i].IsOpen)
                    CamWriters[i].Close();
            }
        }

        #region 해상도 파일 관련
        /// <summary>
        /// 사용자가 설정한 해상도 저장용 텍스트 파일 생성
        /// </summary>
        public static void CreateVideoResolutionInfoTxT()
        {
            Paths.resolution_file_path = Paths.basePath + @"\video_resolution_info.txt";

            if (!File.Exists(Paths.resolution_file_path))
            {
                File.WriteAllLines(Paths.resolution_file_path, resolution_index_strings);
            }
            else
            {
                // 이전에 저장한 파일 있을 때
                resolution_index_strings = File.ReadAllLines(Paths.resolution_file_path);
            }
        }

        /// <summary>
        /// 텍스트에 쓰는 카메라 번호와 해당 카메라에 설정된 해상도의 인덱스를 텍스트 파일을 생성하고 파일에 내용을 쓰는 함수
        /// </summary>
        /// <param name="camera_number">카메라 번호</param>
        /// <param name="selected_index">선택된 해상도 이미지</param>
        public static void WriteResolutionTXT(int camera_number, int selected_index)
        {
            //파일 존재 유무 확인
            if (File.Exists(Paths.resolution_file_path))
            {
                TxtWrite(camera_number, selected_index);
            }
            else
            {
                CreateVideoResolutionInfoTxT(); //파일이 존재 하지 않으면 다시 생성
                TxtWrite(camera_number, selected_index);
            }
        }


        /// <summary>
        /// 텍스트 파일에 내용을 쓰는 함수
        /// </summary>
        /// <param name="camera_number"></param>
        /// <param name="selected_index"></param>
        public static void TxtWrite(int camera_number, int selected_index)
        {
            // 파일에 저장할 resolution 정보
            resolution_index_strings[camera_number - 1] = selected_index.ToString();
            File.WriteAllLines(Paths.resolution_file_path, resolution_index_strings);
        }
        #endregion

        /// <summary>
        /// 실행중인 비디오 소스 플레이어를 멈춥니다
        /// </summary>
        public static void CloseVSP()
        {
            if (CameraPage.cameraPage.videoSourcePlayer1 != null && CameraPage.cameraPage.videoSourcePlayer1.IsRunning)
            {
                CameraPage.cameraPage.videoSourcePlayer1.Stop();
            }

            if (CameraPage.cameraPage.videoSourcePlayer2 != null && CameraPage.cameraPage.videoSourcePlayer2.IsRunning)
            {
                CameraPage.cameraPage.videoSourcePlayer2.Stop();
            }

            if (CameraPage.cameraPage.videoSourcePlayer3 != null && CameraPage.cameraPage.videoSourcePlayer3.IsRunning)
            {
                CameraPage.cameraPage.videoSourcePlayer3.Stop();
            }

            if (CameraPage.cameraPage.videoSourcePlayer4 != null && CameraPage.cameraPage.videoSourcePlayer4.IsRunning)
            {
                CameraPage.cameraPage.videoSourcePlayer4.Stop();
            }

            if (CameraPage.cameraPage.videoSourcePlayer5 != null && CameraPage.cameraPage.videoSourcePlayer5.IsRunning)
            {
                CameraPage.cameraPage.videoSourcePlayer5.Stop();
            }

            if (CameraPage.cameraPage.videoSourcePlayer6 != null && CameraPage.cameraPage.videoSourcePlayer6.IsRunning)
            {
                CameraPage.cameraPage.videoSourcePlayer6.Stop();
            }
        }

        /// <summary>
        /// 녹화를 중지합니다
        /// </summary>
        public static void RecordStop()
        {
            
            for(int i = 0; i < 6; i++)
            {
                if(recordings[i])
                {
                    recording_record[i] = true;
                    recordings[i] = false;
                }
                else
                {
                    recording_record[i] = false;
                }
            }
        }

        /// <summary>
        /// 녹화를 시작합니다
        /// </summary>
        public static void BringingUpRecords()
        {
            for(int i = 0; i < 6; i++)
            {
                recordings[i] = false ;
            }
            for (int i = 0; i < 6; i++)
            {
                if (recording_record[i])
                {
                    recordings[i] = true;
                }
            }
            for (int i = 0; i < 6; i++)
            {
                recording_record[i] = false;
            }
        }
    }
}
