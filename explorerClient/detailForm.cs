using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SWPExplorer;

namespace explorerClient
{
    public partial class detailForm : Form
    {
        public detailForm(FileDetail file)
        {
            InitializeComponent();
            switch (file.fileInfo.type)
            {
                case FileType.Directory:
                    pictureBox1.Image = Properties.Resources.folder;
                    break;
                case FileType.Music:
                    pictureBox1.Image = Properties.Resources.music;
                    break;
                case FileType.Txt:
                    pictureBox1.Image = Properties.Resources.text;
                    break;
                case FileType.Movie:
                    pictureBox1.Image = Properties.Resources.avi;
                    break;
                case FileType.Image:
                    pictureBox1.Image = Properties.Resources.image;
                    break;
                case FileType.Etc:
                    pictureBox1.Image = Properties.Resources.temp;
                    break;
            }
            string[] path = file.fileInfo.filepath.Split('\\');
            txtFileName.Text = path.Last();
            labelFileType.Text = file.fileInfo.filepath.Split('.').Last();
            Array.Clear(path, path.Length - 1, 1);
            labelFileLocation.Text = string.Join("\\", path);
            labelFileSize.Text = Files.TranslateFileSize(file.fileSize, true);
            labelDayofMade.Text = file.DayofMade.ToString();
            labelDayofModify.Text = file.DayofModify.ToString();
            labelDayofAccess.Text = file.DayofAccess.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
