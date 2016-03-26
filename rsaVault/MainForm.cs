using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace rsaVault
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

        private string inFile, outFile;
        private System.Threading.Thread thread;

        private void GoToBusyMode()
        {

        }

        private void GoToReadyMode()
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            rsa = new RSACryptoServiceProvider();
            lblStatus.Text = "New key pair generated";
        }

        private void btnLoadPublic_Click(object sender, EventArgs e)
        {

            try
            {
                OpenFileDialog o = new OpenFileDialog();
                o.Filter = "RSA Key Files(*.rkf)|*.rkf";
                if (o.ShowDialog() == DialogResult.OK)
                {
                    System.IO.StreamReader reader = System.IO.File.OpenText(o.FileName);
                    string s = reader.ReadToEnd();
                    rsa.FromXmlString(s);
                    reader.Close();
                    lblStatus.Text = "Public key loaded";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
                thread.Abort();
                GoToReadyMode();
            }
        }

        private void btnLoadPrivate_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog o = new OpenFileDialog();
                o.Filter = "RSA Key Files(*.rkf)|*.rkf";
                if (o.ShowDialog() == DialogResult.OK)
                {
                    System.IO.StreamReader reader = System.IO.File.OpenText(o.FileName);
                    string s = reader.ReadToEnd();
                    rsa.FromXmlString(s);
                    reader.Close();
                    lblStatus.Text = "Private key loaded";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
                thread.Abort();
                GoToReadyMode();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                thread.Abort();
                GoToReadyMode();
            }
        }

        private void btnSavePublic_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog s = new SaveFileDialog();
                s.Filter = "RSA Key Files(*.rkf)|*.rkf";
                if (s.ShowDialog() == DialogResult.OK)
                {
                    RSAParameters pa = rsa.ExportParameters(true);
                    if (System.IO.File.Exists(s.FileName))
                        System.IO.File.Delete(s.FileName);
                    System.IO.StreamWriter writer = System.IO.File.CreateText(s.FileName);
                    writer.Write(rsa.ToXmlString(false));
                    writer.Close();
                    lblStatus.Text = "Public key saved to: " + s.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
                thread.Abort();
                GoToReadyMode();
            }
        }

        private void btnSavePrivate_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog s = new SaveFileDialog();
                s.Filter = "RSA Key Files(*.rkf)|*.rkf";
                if (s.ShowDialog() == DialogResult.OK)
                {
                    RSAParameters pa = rsa.ExportParameters(true);
                    if (System.IO.File.Exists(s.FileName))
                        System.IO.File.Delete(s.FileName);
                    System.IO.StreamWriter writer = System.IO.File.CreateText(s.FileName);
                    writer.Write(rsa.ToXmlString(true));
                    writer.Close();
                    lblStatus.Text = "Private key saved to: " + s.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
                thread.Abort();
                GoToReadyMode();
            }
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog o = new OpenFileDialog();
                o.Filter = "All Files(*.*)|*.*";
                if (o.ShowDialog() == DialogResult.OK)
                {
                    if (System.IO.File.Exists(o.FileName + ".encrypted"))
                        System.IO.File.Delete(o.FileName + ".encrypted");
                    inFile = o.FileName;
                    outFile = o.FileName + ".encrypted";

                    thread = new System.Threading.Thread(new System.Threading.ThreadStart(Encrypt));
                    thread.IsBackground = true;
                    btnCancel.Enabled = true;
                    thread.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
                thread.Abort();
                GoToReadyMode();
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog o = new OpenFileDialog();
                o.Filter = "All Files(*.*)|*.*";
                if (o.ShowDialog() == DialogResult.OK)
                {

                    outFile = o.FileName.Substring(0, o.FileName.Length - (".encrypted".Length));
                    if (System.IO.File.Exists(outFile))
                        System.IO.File.Delete(outFile);
                    inFile = o.FileName;
                    thread = new System.Threading.Thread(new System.Threading.ThreadStart(Decrypt));
                    thread.IsBackground = true;
                    btnCancel.Enabled = true;
                    thread.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
                thread.Abort();
                GoToReadyMode();
            }
        }

        private void Encrypt()
        {
            try
            {
                GoToBusyMode();
                progressBar.Minimum = 0;
                System.IO.FileInfo finfo = new System.IO.FileInfo(inFile);
                progressBar.Maximum = (int)(finfo.Length / 32);
                progressBar.Value = 0;
                lblStatus.Text = "Encrypting...";

                System.IO.FileStream fout = System.IO.File.Open(outFile, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Write);
                System.IO.FileStream fin = System.IO.File.Open(inFile, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                byte[] buffer = new byte[32];
                byte[] encbuffer = new byte[32];
                int c = 0;
                while ((c = fin.Read(buffer, 0, 32)) > 0)
                {
                    if (c != buffer.Length)
                    {
                        byte[] buffer2 = new byte[c];
                        for (int i = 0; i < c; i++)
                            buffer2[i] = buffer[i];
                        encbuffer = rsa.Encrypt(buffer2, false);
                        fout.Write(encbuffer, 0, encbuffer.Length);
                    }
                    else
                    {
                        encbuffer = rsa.Encrypt(buffer, false);
                        fout.Write(encbuffer, 0, encbuffer.Length);
                    }
                    progressBar.Increment(1);
                }
                fin.Close();
                fout.Close();
                lblStatus.Text = "Encryption successful";
                GoToReadyMode();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
                thread.Abort();
                GoToReadyMode();
            }
        }

        private void linklabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }

        private void Decrypt()
        {
            try
            {
                GoToBusyMode();
                progressBar.Minimum = 0;
                System.IO.FileInfo finfo = new System.IO.FileInfo(inFile);
                progressBar.Maximum = (int)(finfo.Length / 128);
                progressBar.Value = 0;
                lblStatus.Text = "Decrypting...";

                System.IO.FileStream fin = System.IO.File.Open(inFile, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                System.IO.FileStream fout = System.IO.File.Open(outFile, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Write);
                byte[] buffer = new byte[128];
                byte[] encbuffer = new byte[128];
                int c = 0;
                while ((c = fin.Read(buffer, 0, 128)) > 0)
                {
                    encbuffer = rsa.Decrypt(buffer, false);
                    fout.Write(encbuffer, 0, encbuffer.Length);
                    progressBar.Increment(1);
                }
                fin.Close();
                fout.Close();
                lblStatus.Text = "Decryption successful";
                GoToReadyMode();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
                thread.Abort();
                GoToReadyMode();
            }
        }

    }
}
