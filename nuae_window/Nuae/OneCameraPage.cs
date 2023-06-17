using System.Windows.Forms;

namespace Nuae
{
    /// <summary>
    /// 카메라 페이지에서 각 카메라를 더블클릭 했을 때 카메라 하나만 단독으로 보여주는 페이지 입니다
    /// </summary>
    public partial class OneCameraPage : Form
    {
        /* OneCameraPage UI 접근용 */
        public static OneCameraPage onecameraPage;

        /* 카메라 번호 */
        public static int cameraNumber = 0;

        /// <summary>
        /// OneCameraPage를 생성합니다
        /// </summary>
        /// <param name="_cameraNumber">카메라 페이지에서 더블 클릭한 카메라의 번호 입니다</param>
        public OneCameraPage(int _cameraNumber)
        {
            InitializeComponent();
            onecameraPage = this;
            cameraNumber = _cameraNumber;
        }

        /// <summary>
        /// OneCameraPage 닫았을때 호출되는 함수입니다
        /// </summary>
        private void OneCameraPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 비디오 플레이어 제거
            SoloShotVideoPlayer.VideoSource = null;
        }

        /// <summary>
        /// OneCameraPage가 로드 되었을 때 호출됩니다
        /// 해당 카메라 번호의 영상을 보여줍니다
        /// </summary>
        private void OneCameraPage_Load(object sender, System.EventArgs e)
        {
            switch(cameraNumber)
            {
                case 1:
                    // 비디오 플레이어 재생
                    SoloShotVideoPlayer.VideoSource = Camera.cameras[0];
                    onecameraPage.SoloShotVideoPlayer.Start();
                    break;
                case 2:
                    SoloShotVideoPlayer.VideoSource = Camera.cameras[1];
                    onecameraPage.SoloShotVideoPlayer.Start();
                    break;
                case 3:
                    SoloShotVideoPlayer.VideoSource = Camera.cameras[2];
                    onecameraPage.SoloShotVideoPlayer.Start();
                    break;
                case 4:
                    SoloShotVideoPlayer.VideoSource = Camera.cameras[3];
                    onecameraPage.SoloShotVideoPlayer.Start();
                    break;
                case 5:
                    SoloShotVideoPlayer.VideoSource = Camera.cameras[4];
                    onecameraPage.SoloShotVideoPlayer.Start();
                    break;
                case 6:
                    SoloShotVideoPlayer.VideoSource = Camera.cameras[5];
                    onecameraPage.SoloShotVideoPlayer.Start();
                    break;
            }
        }

        /// <summary>
        /// 닫기 버튼 클릭
        /// </summary>
        private void SoloShotExitButton_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
