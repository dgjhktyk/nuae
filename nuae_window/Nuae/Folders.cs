
using System;
using System.IO;

namespace Nuae
{
    internal class Folders
    {
        /// <summary>
        /// 각 카메라 폴더의 경로 설정
        /// </summary>
        public static void SetCamFolderPath()
        {
            for (int i = 0; i < 6; i++)
            {
                Paths.camFolders[i] = Paths.basePath + @"\Cam" + (i + 1);
            }
        }

        /// <summary>
        /// 날짜 폴더의 경로 설정
        /// </summary>
        public static void SetDateFolderPath()
        {
            for (int i = 0; i < 6; i++)
            {
                Paths.dateFolderByCamera[i] = Paths.camFolders[i] + @"\" + DateTime.Now.ToString("yyyyMMdd");
            }
        }
        /// <summary>
        /// 폴더 생성
        /// </summary>
        public static void CreateFolders()
        {
            if (Directory.Exists(Paths.basePath) == false)
                Directory.CreateDirectory(Paths.basePath);

            foreach (string folder_path in Paths.camFolders)
            {
                if (Directory.Exists(folder_path) == false)
                    Directory.CreateDirectory(folder_path);
            }

            foreach (string datePath in Paths.dateFolderByCamera)
            {
                if (Directory.Exists(datePath) == false)
                    Directory.CreateDirectory(datePath);
            }
        }
    }
}
