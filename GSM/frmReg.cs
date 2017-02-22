using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.Threading;

namespace GSM
{
    public partial class frmReg : Form
    {
        private string publicKey = string.Empty;
        private Cls.DES des;
        private string _key = string.Empty;// = Convert.ToString(Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\" + "{" + CS.SoftRegister.getMd5("wegoPacsnkj") + "}", CS.SoftRegister.getMd5("md5Key"), ""));
        RegistryKey res;
        
        public frmReg()
        {
            InitializeComponent();
        }

        //将机器码写入注册表
        private void frmReg_Load(object sender, EventArgs e)
        {
            des = new GSM.Cls.DES();
            publicKey = "<RSAKeyValue><Modulus>pl4q/wNkWPs7RLPYknv0CkFjoAUJosIaFcBWCN7x9g8M9f/l2aj6XDe8ehN6iCPb9ksvsQPS5t5lA1sDE3o/fxvZGuttN7DYva0Xv8x+0/mXflVCEOnnbkQ2iDEqFDr9EVowrTW9hSBkyNRSfQWojO+JFtNqOJfGesctc+pKo9s=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            _key = Convert.ToString(Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\" + "{" + GSM.Cls.SoftRegister.getMd5("Go-Dex") + "}", GSM.Cls.SoftRegister.getMd5("md5Key"), ""));
            res = Registry.LocalMachine.OpenSubKey("Software\\" + "{" + GSM.Cls.SoftRegister.getMd5("Go-Dex") + "}", true);
            getMachineCode_Click(sender, e);
        }

        //获取机器码
        private void getMachineCode_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.mathineCode.Text = GSM.Cls.SoftRegister.getMNum();
            Cursor.Current = Cursors.Default;
            res.SetValue(GSM.Cls.SoftRegister.getMd5("machineCode"), des.EncryptString(this.mathineCode.Text, _key), RegistryValueKind.String);
        }


        //注册
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(publicKey);
                    RSAPKCS1SignatureDeformatter fRsapacs = new RSAPKCS1SignatureDeformatter(rsa);
                    fRsapacs.SetHashAlgorithm("SHA1");
                    //key是注册码
                    byte[] key = Convert.FromBase64String(txtKey.Text);
                    SHA1Managed sha = new SHA1Managed();
                    //name为 需加密的字符串
                    byte[] name = sha.ComputeHash(ASCIIEncoding.ASCII.GetBytes(this.mathineCode.Text));
                    //判断是否成功
                    if (fRsapacs.VerifySignature(name, key))
                    {
                        res.SetValue(GSM.Cls.SoftRegister.getMd5("Lisence"), des.EncryptString(this.txtKey.Text, _key), RegistryValueKind.String);
                        MessageBox.Show("注册成功，感谢您的使用，请您重新启动软件！");
                        Application.Exit();
                        //写入注册表
                    }
                    else
                    {
                        MessageBox.Show("您输入的注册码不正确，请重新输入！");
                        return;
                    }
                }
            }
            catch { MessageBox.Show("您输入的注册码不正确，请重新输入！"); return; }
        }

        private void frmReg_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
