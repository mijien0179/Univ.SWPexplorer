using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.Net.Sockets;

namespace SWPExplorer
{
    public enum PacketType : uint
    {
        NONE                    = 0,

        CONNECTION_CLOSE        = 0xF000_0001,

        FILEINFO                = 0x0000_0001,
        RETRIVE_DIRECTORY       = 0x0000_0002,
        FILE_DOWNLOAD           = 0x0000_0004,

        REQ_FILEINFO            = 0x1000_0001,
        REQ_RETRIVE_DIRECTORY   = 0x1000_0002,
        REQ_FILE_DOWNLOAD       = 0x1000_0004,

        RES_FILEINFO            = 0x2000_0001,
        RES_RETRIVE_DIRECTORY   = 0x2000_0002,

        RES_FILE_DOWNLOAD       = 0x2000_0004,
        RES_FILE_DOWNLOAD_BEGIN = 0x2000_1004,
        RES_FILE_DOWNLOAD_END   = 0x2000_2004,


    }



    public static class Packet
    {
        public static byte[] Serialize(object o)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, o);

            return ms.ToArray();
        }

        public static object Deserialize(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream();
            foreach (byte b in bytes)
            {
                ms.WriteByte(b);
            }
            ms.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(ms);
            ms.Close();
            return obj;
        }

    }

    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Meta
    {
        public long Length; // 전송시 결정 됨
        public PacketType type;

        public Meta(NetworkStream stream)
        {
            Length = 0; type = PacketType.NONE;
            Set(stream);
        }
        public Meta(PacketType type = PacketType.NONE)
        {
            this.type = type;
            Length = 0;
        }

        /// <summary>
        /// 현재 meta데이터를 byte 배열로 반환합니다.
        /// </summary>
        /// <returns>meta 데이터의 byte 배열</returns>
        public byte[] GetBytes()
        {
            byte[] bytes = new byte[12];
            BitConverter.GetBytes(Length).CopyTo(bytes, 0);
            BitConverter.GetBytes((uint)type).CopyTo(bytes, 8);
            return bytes;
        }

        /// <summary>
        /// stream으로부터 Meta데이터 정보를 가져옵니다.
        /// </summary>
        /// <param name="stream">데이터를 받은 stream</param>
        public void Set(NetworkStream stream)
        {
            byte[] bytes = new byte[12];
            stream.Read(bytes, 0, bytes.Length);
            Length = BitConverter.ToInt64(bytes, 0);
            type = (PacketType)BitConverter.ToUInt32(bytes, 8);
        }

        /// <summary>
        /// meta 데이터와 param으로 전달된 개체를 스트림에 씁니다.
        /// </summary>
        /// <param name="stream">데이터를 쓸 스트림</param>
        /// <param name="obj">쓸 개체</param>
        public void Send(NetworkStream stream, object obj)
        {
            byte[] bytes;
            if(obj != null) { 
                byte[] data = Packet.Serialize(obj);
                Length = data.Length;
                bytes = GetBytes();
                stream.Write(bytes, 0, bytes.Length);
                stream.Write(data, 0, data.Length);
            }
            else
            {
                bytes = GetBytes();
                stream.Write(bytes, 0, bytes.Length);
            }
            stream.Flush();
        }

        /// <summary>
        /// 스트림으로부터 데이터를 받아옵니다.
        /// </summary>
        /// <param name="stream">데이터를 받을 스트림</param>
        /// <returns>스트림에서 받은 데이터</returns>
        public object Receive(NetworkStream stream, int timeout = System.Threading.Timeout.Infinite)
        {
            Set(stream);
            if (Length > 0)
            {
                byte[] data = new byte[Length];
                stream.ReadTimeout = timeout;
                stream.Read(data, 0, data.Length);
                return Packet.Deserialize(data);
            }
            return null;
        }

    }

    [Serializable]
    public class RetriveDirectory
    {
        public RetriveDirectory(string dir)
        {
            root = dir;
        }
        public RetriveDirectory(DirectoryInfo info)
        {
            files = new List<FileSummary>();
            root = info.FullName;
            foreach(var dir in info.GetDirectories())
            {
                files.Add(new FileSummary(dir));
            }

            foreach (var file in info.GetFiles("*", SearchOption.TopDirectoryOnly))
            {
                files.Add(new FileSummary(file));
            }
        }
        public string root;
        public List<FileSummary> files;
    }

    [Serializable]
    public class FileSummary
    {
        public FileSummary(FileSummary obj)
        {
            filepath = obj.filepath;
            type = obj.type;
        }

        public FileSummary(object entry)
        {
            DirectoryInfo dir = entry as DirectoryInfo;
            if (dir != null)
            {
                filepath = dir.FullName;
                type = FileType.Directory;
                expanded = dir.GetDirectories().Length > 0;
            }
            else
            {
                FileInfo file = entry as FileInfo;
                filepath = file.FullName;
                type = Files.GetFileType(file);
            }
        }

        public readonly FileType type;
        public readonly string filepath;
        public readonly bool expanded;
    }

    [Serializable]
    public class FileDetail
    {

        public FileDetail(object entry)
        {
            fileInfo = new FileSummary(entry);

            fileSize = 0;
            if (fileInfo.type == FileType.Directory)
            {
                DirectoryInfo dir = entry as DirectoryInfo;
                foreach (var files in dir.GetFiles("*", SearchOption.AllDirectories))
                {
                    fileSize += (ulong)files.Length;
                }
                DayofMade = dir.CreationTime;
                DayofModify = dir.LastWriteTime;
                DayofAccess = dir.LastAccessTime;
            }
            else
            {
                FileInfo file = entry as FileInfo;
                fileSize = (ulong)file.Length;
                DayofMade = file.CreationTime;
                DayofModify = file.LastWriteTime;
                DayofAccess = file.LastAccessTime;
            }
        }
        public readonly FileSummary fileInfo;
        public readonly ulong fileSize;
        public readonly DateTime DayofMade;
        public readonly DateTime DayofModify;
        public readonly DateTime DayofAccess;
    }

    [Serializable]
    public class FileData
    {
        public static int FilePeiceSize = 0x200;

        private string filepath;
        public string FilePath
        {
            get => filepath;
            private set => filepath = value;
        }

        private long size;
        public long Size
        {
            get => size;
            private set => size = value;
        }

        private byte[] data;
        public byte[] Data
        {
            get => data;
            private set => data = value;
        }        
        
        public FileData(string filepath)
        {
            FilePath = filepath;
        }

        public void SetData(BinaryReader reader)
        {
            Data = reader.ReadBytes(FileData.FilePeiceSize);
            Size = Data.Count();
        }

        
        
    }

}
