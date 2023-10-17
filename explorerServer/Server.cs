using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

using System.IO;

using System.Net;
using System.Net.Sockets;

using System.Diagnostics;

using SWPExplorer;

namespace Server
{
    public partial class Server : Form
    {
        TcpListener listener;
        Thread tServer;
        TcpClient client = null;
        NetworkStream netStream = null;

        private int DownloadLogWriteTime // second
        {
            get => 3;
        }

        #region Server

        private void ServerStart()
        {
            if (listener != null) return;

            if (txtIP.Text == "")
            {
                MessageBox.Show("IP를 입력하세요.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtIP.Focus();
                return;
            }

            if (txtPort.Text == "")
            {
                MessageBox.Show("Port를 입력하세요.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtIP.Focus();
                return;
            }

            if(txtPath.Text == "")
            {
                MessageBox.Show("경로를 입력하세요.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Log("서버 실행");
                listener = new TcpListener(IPAddress.Parse(txtIP.Text), int.Parse(txtPort.Text));
                tServer = new Thread(ServerListen);
                listener.Start();
                tServer.Start();

                btnServerExecute.Text = "서버끊기";
                btnServerExecute.ForeColor = Color.Red;


            }
            catch (Exception ex)
            {
                listener = null;
                if (tServer.IsAlive) tServer.Abort();

                MessageBox.Show("서버 실행에 실패했습니다.\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ServerStop()
        {
            if (listener == null) return;

            listener.Stop();
            listener = null;
            tServer.Abort();
            tServer = null;
            btnServerExecute.Text = "서버켜기";
            btnServerExecute.ForeColor = Color.Black;
            Log("서버 종료");
        }

        private void ServerListen()
        {
            try
            {
                Log("클라이언트 연결을 기다리는 중");
                client = listener.AcceptTcpClient();
                netStream = client.GetStream();
                Log("클라이언트 연결 성공");

            }
            catch (ThreadAbortException)
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Log("클라이언트 연결을 기다리는 중 오류 발생");
                if (netStream != null) netStream.Close();
                if (client != null) client.Close();
                ServerStop();
                return;
            }
            
            while (client != null && client.Connected)
            {
                try
                {
                    Meta m = new Meta();
                    object obj = m.Receive(netStream);

                    switch (m.type)
                    {
                        case PacketType.REQ_RETRIVE_DIRECTORY:
                            ResponseRetriveDirectory((RetriveDirectory)obj);
                            break;
                        case PacketType.REQ_FILEINFO:
                            ResponseFileInfo((FileSummary)obj);
                            break;
                        case PacketType.REQ_FILE_DOWNLOAD:
                            ResponseFileDownload((FileData)obj);
                            break;
                        case PacketType.CONNECTION_CLOSE:
                            netStream.Close();
                            client.Close();
                            netStream = null;
                            client = null;
                            break;
                    }
                }
                catch(ThreadAbortException)
                {

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }

            Log("클라이언트와 연결이 해제 됨");

            ServerListen();
        }

        #endregion

        #region Response

        private void ResponseRetriveDirectory(RetriveDirectory o)
        {
            if (o.root == "")
            {
                o.root = txtPath.Text;
            }
            Log($"디렉토리 검색: {o.root}");
            RetriveDirectory retriveDirectory = new RetriveDirectory(new DirectoryInfo(o.root));
            Meta meta = new Meta(PacketType.RES_RETRIVE_DIRECTORY);
            meta.Send(netStream, retriveDirectory);
        }

        private void ResponseFileInfo(FileSummary file)
        {
            Log($"파일 정보 요청: {file.filepath}");
            FileDetail detail;
            if(file.type == FileType.Directory)
            {
                detail = new FileDetail(new DirectoryInfo(file.filepath));
            }
            else
            {
                detail = new FileDetail(new FileInfo(file.filepath));
            }
            Meta meta = new Meta(PacketType.RES_FILEINFO);
            meta.Send(netStream, detail);
        }

        private void ResponseFileDownload(FileData file)
        {
            Log($"파일 전송 요청: {file.FilePath}");
            FileStream fStream = File.OpenRead(file.FilePath);
            BinaryReader stream = new BinaryReader(fStream);
            FileData fData = new FileData(file.FilePath);
            Meta meta;

            DateTime current = DateTime.Now;
            do
            {
                fData.SetData(stream);
                meta = new Meta(fData.Size == FileData.FilePeiceSize ? PacketType.RES_FILE_DOWNLOAD : PacketType.RES_FILE_DOWNLOAD_END);
                meta.Send(netStream, fData);

                if ((DateTime.Now - current).TotalSeconds > DownloadLogWriteTime)
                {
                    Log($"전송 경과: {string.Format("{0:##.##}",fStream.Position / (double)fStream.Length * 100)}%");
                    current = DateTime.Now;
                }
            } while (fData.Size == FileData.FilePeiceSize);
            Log($"파일 전송 완료: {file.FilePath}");
            fStream.Close();
            stream.Close();

        }

        #endregion

        #region Invoker

        private void Log(string str)
        {
            try
            {
                if (txtLog.InvokeRequired)
                {
                    txtLog.Invoke(new MethodInvoker(() => Log(str)));
                    return;
                }
                txtLog.AppendText($"{str}\r\n");

            }
            catch
            {

            }
        }

        #endregion

        protected override void WndProc(ref Message m)
        {
            Trace.WriteLine(m.Msg);
            base.WndProc(ref m);
        }

        public Server()
        {
            InitializeComponent();

//#if DEBUG
//            txtPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
//#endif
        }

        private void Server_FormClosed(object sender, FormClosedEventArgs e)
        {
            ServerStop();
        }

        private void btnServerExecute_Click(object sender, EventArgs e)
        {
            if (listener == null) ServerStart();
            else ServerStop();
        }

        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = folderBrowserDialog1.SelectedPath;
                Log($"{txtPath.Text}로 경로가 수정되었습니다.");
            }

        }
    }

}
