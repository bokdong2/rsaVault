using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace rsaVault
{
    public partial class MainForm : Form
    {
		public static int bitlength = 4096;	//4096 bits is bitlength for keys, by default. 4096 [bits] / 8 [bits/bytes] = 512 [bytes] - in each block of cyphertext.
		
        public MainForm()
        {
			this.TopMost = true;			//show before all windows
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

		private void SetStatus(string status) 
		{
			lblStatus.Text = status;
			lblStatus.Invalidate();
			Application.DoEvents();
		}

        private void btnGenerate_Click(object sender, EventArgs e)
        {
			//int bitlength = 2048;
			SetStatus("Wait for generate keys... (key size bitlength = "+bitlength+" bits)");
			
			progressBar.Minimum = 0;
			progressBar.Maximum = 100;
			progressBar.Value = 0;
			GoToBusyMode();

			try{
				rsa = new RSACryptoServiceProvider(bitlength);
				RSAParameters pa = rsa.ExportParameters(true);

				progressBar.Increment(100);
				lblStatus.Text = "New key pair generated! BitLength: "+bitlength+" bits ("+bitlength/8+" bytes).";
			}catch (Exception ex){
                MessageBox.Show("Error:" + ex.Message);
            }

            GoToReadyMode();
        }

        private void btnLoadPublic_Click(object sender, EventArgs e)
        {
			SetStatus("Select file with public key...");

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
			SetStatus("Select file with private key...");

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
			SetStatus("Input filename to save public key...");

            try
            {
                SaveFileDialog s = new SaveFileDialog();
                s.Filter = "RSA Key Files(*.rkf)|*.rkf";
                if (s.ShowDialog() == DialogResult.OK)
                {
                    //RSAParameters pa = rsa.ExportParameters(true);
                    if (System.IO.File.Exists(s.FileName))
                        System.IO.File.Delete(s.FileName);
                    System.IO.StreamWriter writer = System.IO.File.CreateText(s.FileName);
					
					string xml_pub = 	rsa.ToXmlString(false)
										.Replace("<", "\n\t<")
										.Replace(">",">\n\t\t")
										.Replace("\t\t\n","")
										.Replace("\t</RSAKeyValue>\n\t\t","</RSAKeyValue>")
										.Replace("\n\t<R","<R")
					;
					writer.Write(xml_pub);
                    
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
			SetStatus("Input filename to save private key...");

            try
            {
                SaveFileDialog s = new SaveFileDialog();
                s.Filter = "RSA Key Files(*.rkf)|*.rkf";
                if (s.ShowDialog() == DialogResult.OK)
                {
                    //RSAParameters pa = rsa.ExportParameters(true);
                    if (System.IO.File.Exists(s.FileName))
                        System.IO.File.Delete(s.FileName);
                    System.IO.StreamWriter writer = System.IO.File.CreateText(s.FileName);
					
					string xml_priv = 	rsa.ToXmlString(true)
										.Replace("<", "\n\t<")
										.Replace(">",">\n\t\t")
										.Replace("\t\t\n","")
										.Replace("\t</RSAKeyValue>\n\t\t","</RSAKeyValue>")
										.Replace("\n\t<R","<R")
					;
					writer.Write(xml_priv);
                    
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
			SetStatus("Select file to encrypt it...");

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
			SetStatus(	"Select file to decrypt it...\n"+
						"WARNING! (\"src.ext.encrypted\" -> \"src.ext\") and src.ext CAN BE OVERWRITTED."
			);

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
		//	string debug = @"debug.log";
			
		//	if (!File.Exists(debug)) 
		//	{
		//		// Create a file to write to.
		//		using (StreamWriter sw = File.CreateText(debug)) 
		//		{
		//			sw.WriteLine("Hello");
		//			sw.WriteLine("And");
		//			sw.WriteLine("Welcome");
		//		}	
		//	}
			

			System.IO.FileStream fout = System.IO.File.Open(outFile, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Write);
			System.IO.FileStream fin = System.IO.File.Open(inFile, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);

            try
            {
		//		File.AppendAllText("debug.log", "\n\n"+"Encrypt"+": \n");

				int block_length 	= 	(bitlength/8)-12;						//block length for source file
				int block_size 		= 	(bitlength/8);							//block length for destination file
				
		//		File.AppendAllText("debug.log", "\nSource file: "+"block_length"+": "+block_length);				
                GoToBusyMode();
                progressBar.Minimum = 0;
		//			File.AppendAllText("debug.log", "\n"+"progressBar.Minimum"+": "+progressBar.Minimum);
                System.IO.FileInfo finfo = new System.IO.FileInfo(inFile);
                //progressBar.Maximum = (int)(finfo.Length / 32);
				progressBar.Maximum = (int)(finfo.Length / block_length);
		//			File.AppendAllText("debug.log", "\n"+"progressBar.Maximum"+": "+progressBar.Maximum);
                progressBar.Value = 0;
		//			File.AppendAllText("debug.log", "\n"+"progressBar.Value"+": "+progressBar.Value);
                lblStatus.Text = "Encrypting...";
		//			File.AppendAllText("debug.log", "\n"+"lblStatus.Text"+": "+lblStatus.Text);

                //byte[] buffer = new byte[32];
				byte[] buffer = new byte[block_length];
		//			File.AppendAllText("debug.log", "\n"+"buffer.Length"+": "+buffer.Length);
                //byte[] encbuffer = new byte[32];
				byte[] encbuffer = new byte[block_size];
		//			File.AppendAllText("debug.log", "\n"+"encbuffer.Length"+": "+encbuffer.Length);
                int c = 0;
		//			File.AppendAllText("debug.log", "\n"+"c"+": "+c);
                //while ((c = fin.Read(buffer, 0, 32)) > 0)
                while ((c = fin.Read(buffer, 0, block_length)) > 0)
                {
					//File.AppendAllText("debug.log", "\n"+"c"+": "+c);
                    if (c != buffer.Length)
                    {
                        byte[] buffer2 = new byte[c];
                        for (int i = 0; i < c; i++)
                            buffer2[i] = buffer[i];
                        encbuffer = rsa.Encrypt(buffer2, false);
							//File.AppendAllText("debug.log", "\nif: "+"encbuffer.Length"+": "+encbuffer.Length);
                        fout.Write(encbuffer, 0, encbuffer.Length);
                    }
                    else
                    {
                        encbuffer = rsa.Encrypt(buffer, false);
							//File.AppendAllText("debug.log", "\nelse: "+"encbuffer.Length"+": "+encbuffer.Length);
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
				fin.Close();
				fout.Close();
                MessageBox.Show("Error:" + ex.Message);
                thread.Abort();
                GoToReadyMode();
            }
        }

        private void linklabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
			SetStatus("Commits and modifications of this OPEN SOURCE CODE - you can see here.");
            System.Diagnostics.Process.Start("https://github.com/bokdong2/rsaVault/network/");
        }

        private void Decrypt()
        {
			
		//	string debug = @"debug.log";
		//	if (!File.Exists(debug)) 
		//	{
		//		// Create a file to write to.
		//		using (StreamWriter sw = File.CreateText(debug)) 
		//		{
		//			sw.WriteLine("Hello");
		//			sw.WriteLine("And");
		//			sw.WriteLine("Welcome");
		//		}	
		//	}

			System.IO.FileStream fin = System.IO.File.Open(inFile, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
			System.IO.FileStream fout = System.IO.File.Open(outFile, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Write);

			try
            {
		//		File.AppendAllText("debug.log", "\n\n"+"Decrypt"+": \n");
                GoToBusyMode();
				int block_size = (bitlength/8);
		//			File.AppendAllText("debug.log", "\n"+"Cypher file: block_size"+": "+block_size);
                progressBar.Minimum = 0;
		//			File.AppendAllText("debug.log", "\nprogressBar.Minimum: "+progressBar.Minimum);
                System.IO.FileInfo finfo = new System.IO.FileInfo(inFile);
                progressBar.Maximum = (int)(finfo.Length / block_size);
		//			File.AppendAllText("debug.log", "\nprogressBar.Maximum: "+progressBar.Maximum);
                progressBar.Value = 0;
		//			File.AppendAllText("debug.log", "\nprogressBar.Value: "+progressBar.Value);
                lblStatus.Text = "Decrypting...";
		//			File.AppendAllText("debug.log", "\nlblStatus.Text: "+lblStatus.Text);

                byte[] buffer = new byte[block_size];
		//			File.AppendAllText("debug.log", "\n"+"buffer.Length"+": "+buffer.Length);
                byte[] encbuffer = new byte[block_size];
		//			File.AppendAllText("debug.log", "\n"+"encbuffer.Length"+": "+encbuffer.Length);
                int c = 0;
		//			File.AppendAllText("debug.log", "\n"+"c"+": "+c);
                while ((c = fin.Read(buffer, 0, block_size)) > 0)
                {
		//			File.AppendAllText("debug.log", "\n"+"c"+": "+c);
                    encbuffer = rsa.Decrypt(buffer, false);
                    fout.Write(encbuffer, 0, encbuffer.Length);
                    progressBar.Increment(1);
                }
                lblStatus.Text = "Decryption successful";
                GoToReadyMode();
				fin.Close();
				fout.Close();
            }
            catch (Exception ex)
            {
				fin.Close();
				fout.Close();
                MessageBox.Show("Error:" + ex.Message);
                thread.Abort();
                GoToReadyMode();
            }
        }

    }
}
