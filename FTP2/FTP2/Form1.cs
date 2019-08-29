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
using System.IO;
using System.Drawing.Imaging;


namespace FTP2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpeg;*.jpg;*.gif;*.png;*.bmp;*.tiff";
            openFileDialog.ShowDialog();
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://192.168.1.121/images/" + openFileDialog.SafeFileName);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential("InterTraffic2019", "123Enc456");
            MemoryStream memoryStream = new MemoryStream();
            Image image = Image.FromFile(openFileDialog.FileName);

            switch (openFileDialog.SafeFileName.Substring(openFileDialog.SafeFileName.IndexOf('.') + 1).ToLower())
            {
                case "jpeg":
                case "jpg":
                    image.Save(memoryStream, ImageFormat.Jpeg);
                    break;
                case "png":
                    image.Save(memoryStream, ImageFormat.Png);
                    break;
                case "gif":
                    image.Save(memoryStream, ImageFormat.Gif);
                    break;
                case "bmp":
                    image.Save(memoryStream, ImageFormat.Bmp);
                    break;
                case "tiff":
                    image.Save(memoryStream, ImageFormat.Tiff);
                    break;
                default:
                    return;
            }
            byte[] fileContents = memoryStream.ToArray();
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            MessageBox.Show("Upload File Complete, Status: " + response.StatusDescription);
            response.Close();
        }
    }
}
