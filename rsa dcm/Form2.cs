using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace rsa_dcm
{
    public partial class Form2 : Form
    {
       
        static byte[] bmpBytes;
        
        byte[] binaryData;
        string SignatureString;

        static public string bobPrivateKey;
        static public string bobPublicKey;
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

        string base64String;
        public Form2(RSACryptoServiceProvider rsa0,string bobPublicKey0,string SignatureString1)
        {
            bobPublicKey = bobPublicKey0;
            SignatureString=SignatureString1;
            rsa = rsa0;
            InitializeComponent();
            richTextBox5.Text = SignatureString;
        }

        private void button2_Click(object sender, EventArgs e)//選取檔案
        {
            //
            OpenFileDialog inFile2 = new OpenFileDialog();
            if (inFile2.ShowDialog() == DialogResult.OK)
            {
                var inFile = inFile2.OpenFile();
                binaryData = new Byte[inFile.Length];
                long bytesRead = inFile.Read(binaryData, 0, (int)inFile.Length);//
                inFile.Close();

                //
                button1.Enabled = true;
                richTextBox1.Text = inFile2.FileName;
                //
            }
            else
            {
                return;
            }

            base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

            bmpBytes = Encoding.UTF8.GetBytes(base64String);


            //
        }

        private void button1_Click(object sender, EventArgs e)//驗簽章
        {

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
            sw.Reset();
            sw.Start();

            rsa.FromXmlString(bobPublicKey);
            
            

            var signatureData = Convert.FromBase64String(SignatureString);

            var isVerify = rsa.VerifyData(bmpBytes, new SHA256CryptoServiceProvider(), signatureData);//驗簽章 看true false

            
            sw.Stop();
            string result1 = sw.Elapsed.TotalSeconds.ToString();
            textBox7.Text = result1;


            if (isVerify)
                MessageBox.Show("此醫學影像認證成功", "驗簽章成功", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            else
            {
                MessageBox.Show("此醫學影像認證失敗", "驗簽章失敗", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

            }
            


        }

     

        private void button3_Click_1(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();//產生Form2的物件，才可以使用它所提供的Method
            button3.Enabled = false;
            //        this.Visible = false;//將Form1隱藏。由於在Form1的程式碼內使用this，所以this為Form1的物件本身
            f1.Show();//顯示第1個視窗
            this.Visible = false;
        }
    }
}
