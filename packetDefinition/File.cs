using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace SWPExplorer
{
    public enum FileType : uint
    {
        Directory = 0,
        Movie = 1,
        Image = 2,
        Music = 3,
        Txt = 4,
        Etc = 5,

    }

    public static class Files
    {
        public static FileType GetFileType(FileInfo file)
        {

            if (ExtTxt.Contains(file.Extension)) return FileType.Txt;
            if (ExtImage.Contains(file.Extension)) return FileType.Image;
            if (ExtMusic.Contains(file.Extension)) return FileType.Music;
            if (ExtMovie.Contains(file.Extension)) return FileType.Movie;

            return FileType.Etc;
        }

        public static string[] ExtMovie = new string[] {
            ".mp4", ".m4v", ".avi", ".wmv", ".mwa" ,".asf",
            ".mpg", "mpeg", ".mkv", ".mov", ".3gp", ".3g2",
            ".webm"
        };

        public static string[] ExtImage = new string[] {
            ".bmp", ".rle", ".jpg", ".jpeg", ".gif", ".png"
        };

        public static string[] ExtMusic = new string[] {
            ".mp3", ".ogg", ".wma", ".wav"
        };
        public static string[] ExtTxt = new string[] {
            ".txt"
        };

        public static string TranslateFileSize(ulong size, bool isDetail = false)
        {
            string[] unit = new string[]
            {
                "Byte", "KB", "MB","GB","TB"
            };
            int i;
            Single result = (Single)size;
            for(i = 0; i < unit.Length && result > 1024; ++i)
            {
                result /= 1024;
            }

            return $"{string.Format("{0:##.##}",(isDetail ? result : (ulong)result))}{unit[i]}";
        }

    }


}
