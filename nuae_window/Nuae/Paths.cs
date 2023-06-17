
using System;

namespace Nuae
{
    internal class Paths
    {
        /* 기본 저장 위치 */
        public static string basePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\NuaeVideosAforge";
        //public static string basePath = @"Z:\NuaeVideosAforge";

        /* 카메라별 폴더 */
        public static string[] camFolders = new string[6];

        /* 날짜별 폴더 */
        public static string[] dateFolderByCamera = new string[6];

        /* 비디오 파일 이름 */
        public static string[] videoFiles = new string[6];

        /* 해상도 파일의 경로 */
        public static string resolution_file_path;

        /* 제한 용량 파일 경로 */
        public static string size_limit_file_path = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\size_limit.txt";

        /* 저장 위치 파일 경로 */
        public static string save_location_file_path = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\save_location.txt";
    }
}
