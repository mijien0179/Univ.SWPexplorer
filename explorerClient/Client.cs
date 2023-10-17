using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

using System.Diagnostics;
using System.Threading;

using SWPExplorer;

using System.IO;

namespace explorerClient
{
    public partial class Client : Form
    {
        TcpClient tcpClient;
        NetworkStream netStream;

        public Client()
        {
            InitializeComponent();
            cmsBigIcon.Checked = true;
//#if DEBUG
//            txtIP.Text = "127.0.0.1";
//            txtPort.Text = "7777";
//#endif
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (tcpClient != null) UnConnecting();
            else Connecting();
        }

        private void Client_FormClosed(object sender, FormClosedEventArgs e)
        {
            UnConnecting();
        }

        #region Invoker

        private void InitialTreeView(RetriveDirectory dir)
        {
            if (treeView1.InvokeRequired)
            {
                treeView1.Invoke(new MethodInvoker(() => InitialTreeView(dir)));
                return;
            }

            TreeNode node = new TreeNode(dir.root);
            node.Tag = dir.root;
            foreach (var file in dir.files)
            {
                if (file.type != FileType.Directory) continue;
                TreeNode sub = new TreeNode(new DirectoryInfo(file.filepath).Name);
                sub.Nodes.Add("");
                node.Nodes.Add(sub);
            }
            node.Expand();
            treeView1.SelectedNode = node;
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(node);
            UpdateTree(treeView1.Nodes[0], dir);
            UpdateListView(dir);

        }

        private void UpdateTree(TreeNode node, RetriveDirectory dir)
        {
            if (treeView1.InvokeRequired)
            {
                treeView1.Invoke(new MethodInvoker(() => UpdateTree(node, dir)));
                return;
            }

            if (node == null) new TreeNode(dir.root);
            node.Nodes.Clear();
            foreach (var file in dir.files)
            {
                if (file.type != FileType.Directory) continue;
                TreeNode sub = new TreeNode(new DirectoryInfo(file.filepath).Name);
                sub.Tag = file.filepath;
                if (file.expanded) sub.Nodes.Add("");
                node.Nodes.Add(sub);
            }
            // UpdateListView(dir);
        }

        public void UpdateListView(RetriveDirectory dir)
        {
            if (listView1.InvokeRequired)
            {
                listView1.Invoke(new MethodInvoker(() => UpdateListView(dir)));
                return;
            }
            listView1.Items.Clear();
            ListViewItem item;
            foreach (var file in dir.files)
            {
                item = new ListViewItem(file.filepath.Split('\\').Last());
                listView1.Items.Add(item);
                FileDetail detail = GetFileDetail(file);
                item.Tag = detail;
                item.SubItems.Add(Files.TranslateFileSize(detail.fileSize));
                item.SubItems.Add(detail.DayofModify.ToString());
                item.ImageIndex = (int)file.type;
            }
        }

        #endregion

        #region Communication

        private void Connecting()
        {
            if (tcpClient != null) return;
            if (txtIP.Text == "")
            {
                MessageBox.Show("IP를 입력하세요.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtIP.Focus();
                return;
            }
            if (txtPort.Text == "")
            {
                MessageBox.Show("Port를 입력하세요.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPort.Focus();
                return;
            }
            if (txtDownloadPath.Text == "")
            {
                MessageBox.Show("다운로드 경로를 선택하세요", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect(txtIP.Text, int.Parse(txtPort.Text));
                netStream = tcpClient.GetStream();

                btnConnect.Text = "연결끊기";
                btnConnect.ForeColor = Color.Red;

                InitialTreeView(RetriveDirectory(""));
            }
            catch (Exception ex)
            {
                if (netStream != null) netStream.Close();
                if (tcpClient != null) tcpClient.Close();
                netStream = null;
                tcpClient = null;

#if DEBUG
                MessageBox.Show("연결에 실패했습니다.\r\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#else
                MessageBox.Show("연결에 실패했습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
            }

        }
        private void UnConnecting()
        {
            if (tcpClient == null) return;
            try
            {
                Meta closer = new Meta(PacketType.CONNECTION_CLOSE);
                closer.Send(netStream, null);
            }
            catch { }
            if (netStream != null) netStream.Close();
            tcpClient.Close();
            tcpClient = null;
            netStream = null;

            treeView1.Nodes.Clear();
            listView1.Clear();

            btnConnect.Text = "서버연결";
            btnConnect.ForeColor = Color.Black;

        }

        private RetriveDirectory RetriveDirectory(string path)
        {
            RetriveDirectory retDir = new RetriveDirectory(path);
            Meta meta = new Meta(PacketType.REQ_RETRIVE_DIRECTORY);
            meta.Send(netStream, retDir);

            retDir = (RetriveDirectory)meta.Receive(netStream);

            return retDir;

        }
        private FileDetail GetFileDetail(FileSummary file)
        {

            Meta meta = new Meta(PacketType.REQ_FILEINFO);
            meta.Send(netStream, file);

            return (FileDetail)meta.Receive(netStream);

        }

        private void DownloadFile(string path)
        {
            Meta meta = new Meta(PacketType.REQ_FILE_DOWNLOAD);
            meta.Send(netStream, new FileData(path));

            FileStream downloadedFile = File.Open($"{txtDownloadPath.Text}\\{path.Split('\\').Last()}", FileMode.Create);
            BinaryWriter writer = new BinaryWriter(downloadedFile);
            do
            {

                FileData fData = (FileData)meta.Receive(netStream);

                writer.Write(fData.Data);

            } while (meta.type == PacketType.RES_FILE_DOWNLOAD || meta.type == PacketType.RES_FILE_DOWNLOAD_BEGIN);
            writer.Close();
            downloadedFile.Close();
        }

        #endregion

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            UpdateTree(e.Node, RetriveDirectory((string)e.Node.Tag));

        }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach (var item in listView1.SelectedItems)
            {
                OpenItem((ListViewItem)item);
            }
        }

        private void OpenItem(ListViewItem item)
        {
            FileDetail file = item.Tag as FileDetail;
            if (file.fileInfo.type == FileType.Directory)
            {
                TreeNode node, child;
                node = treeView1.SelectedNode;
                node.Expand();
                child = node.FirstNode;
                while (child != null)
                {
                    if (child.Text == file.fileInfo.filepath.Split('\\').Last())
                    {
                        treeView1.SelectedNode = child;
                        treeView1.Focus();
                        break;
                    }
                    child = child.NextNode;
                }
            }
            else
            {
                OpenDetailForm();
            }
        }

        private void cmsList_Click(object sender, EventArgs e)
        {
            cmsBigIcon.Checked =
                cmsDetail.Checked =
                cmsSimple.Checked =
                cmsSmallIcon.Checked = false;

            ToolStripMenuItem item = sender as ToolStripMenuItem;
            item.Checked = true;

            listView1.View = (View)int.Parse((string)item.Tag);

        }

        private void 상세정보ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDetailForm();

        }

        private void OpenDetailForm()
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                detailForm dForm = new detailForm((FileDetail)item.Tag);
                dForm.Owner = this;
                dForm.Show();
            }
        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            UpdateListView(RetriveDirectory((string)e.Node.Tag));
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(txtDownloadPath.Text))
                Process.Start($"{txtDownloadPath.Text}");
            else
                MessageBox.Show("폴더가 존재하는지 확인하세요.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void 다운로드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tcpClient == null) return;


            if (!Directory.Exists(txtDownloadPath.Text))
            {
                Directory.CreateDirectory(txtDownloadPath.Text);
            }

            bool DirectoryAlert = false; // 폴더 경고 표시  했는지
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                FileDetail fd = item.Tag as FileDetail;
                if (fd.fileInfo.type == FileType.Directory)
                {
                    if (DirectoryAlert == false)
                    {
                        DirectoryAlert = true;
                        MessageBox.Show("폴더는 다운로드를 지원하지 않습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    continue;
                }
                DownloadFile(fd.fileInfo.filepath);
            }


        }

        private void btnPathSet_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowNewFolderButton = true;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtDownloadPath.Text = $"{folderBrowserDialog1.SelectedPath}";
            }

        }
    }
}
