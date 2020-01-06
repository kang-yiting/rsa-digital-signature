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
    public partial class Form1 : Form
    {

        static byte[] bmpBytes;
        

        int i;//count
        byte[][] array1=new byte [1000][] ;//存簽章
        string SignatureString;

        static public string bobPrivateKey;
        static public string bobPublicKey;

        byte[] binaryData;
        string base64String;

   
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

        


        public Form1()
        {
            InitializeComponent();
            rsa = new RSACryptoServiceProvider();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
         
        }

        private void button1_Click(object sender, EventArgs e)// 產生 Bob 的金鑰對
        {
            button2.Enabled = true;
            // 產生 Bob 的金鑰對
            bobPrivateKey = rsa.ToXmlString(true);
            bobPublicKey = rsa.ToXmlString(false);
            richTextBox1.Text = bobPrivateKey;
        }

        private void button2_Click(object sender, EventArgs e)//選擇.dcm檔案
        {

            OpenFileDialog inFile2 = new OpenFileDialog();
            if (inFile2.ShowDialog() == DialogResult.OK)
            {

                var inFile = inFile2.OpenFile();
                binaryData = new Byte[inFile.Length];
                long bytesRead = inFile.Read(binaryData, 0, (int)inFile.Length);//
                inFile.Close();


                button3.Visible = true;
                richTextBox2.Visible = true;
                textBox6.Visible = true;
                label77.Visible = true;
                //
            }
            else
            {
                return;
            }

            base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);//再做檔案的轉換 轉換成長格式(64位元的string)    
           
        }

        private void button3_Click(object sender, EventArgs e)//簽章
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
            sw.Reset();
            sw.Start();

            button1.Enabled = false;
            button2.Enabled = false;

            bmpBytes = Encoding.UTF8.GetBytes(base64String);
            rsa.FromXmlString(bobPrivateKey);//私鑰簽章

            var singData = rsa.SignData(bmpBytes, new SHA256CryptoServiceProvider());//SHA1 128bit 其他的呢SHA256之類的是128ㄇ?
            SignatureString = Convert.ToBase64String(singData);
            array1[i] = rsa.SignData(bmpBytes, new SHA256CryptoServiceProvider());
            i++;

            richTextBox2.Text = SignatureString;
            
            
            sw.Stop();
            string result1 = sw.Elapsed.TotalSeconds.ToString();
            textBox6.Text = result1;


        }

        private void button4_Click(object sender, EventArgs e)//確定
        {

            Form2 f2 = new Form2(rsa, bobPublicKey, SignatureString);//產生Form2的物件，才可以使用它所提供的Method
            button3.Enabled = false;
            f2.Visible = true;//顯示第二個視窗
            this.Visible=false;//將Form1隱藏。由於在Form1的程式碼內使用this，所以this為Form1的物件本身
           
            
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
