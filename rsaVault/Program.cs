using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace rsaVault
{
        public static class Program
        {
                //to hide console window
                [DllImport("kernel32.dll")]
                static extern IntPtr GetConsoleWindow();

                [DllImport("user32.dll")]
                static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

                const int SW_HIDE = 0;
                const int SW_SHOW = 5;
                //var handle = GetConsoleWindow();            //Now is possible to get console window
                //ShowWindow(handle, SW_HIDE);                //hide this...
                //ShowWindow(handle, SW_SHOW);                //or show this...
                
                /// <summary>
                /// 해당 응용 프로그램의 주 진입점입니다.
                /// </summary>
                [STAThread]
                private static void Main(string [] args)
                {
                        if( args.Length != 0 ){
                                for( int i = 0; i < args.Length; i++ ){
                                        if(args[i]==""){Console.WriteLine("Empty argument args["+i+"]"); return;}
                                }
                                if( ( args.Length<=3 ) && ( (args[0]=="-g") || (args[0]=="-gen") || (args[0]=="generate")) ){
                                        //generate keys and save to files if pathway specified or default filenames.
                                        Console.WriteLine("Start generating keys and save this.");

                                        //Run generate...
                                        priv = args[1];                        //priv
                                        if( args.Length >= 2 ){
                                                Console.WriteLine("Generate keys...");
                                                btnGenerate_Click();
                                                Console.WriteLine("Save private key...");
                                                btnSavePrivate_Click(priv);
                                        }
                                        if( args.Length == 3 ){
                                                Console.WriteLine("Save public key...");
                                                pub = args[2];                //pub
                                                btnSavePublic_Click(pub);
                                        }
                                        return;
                                        
                                }else if( ( args.Length<=3 ) && ( (args[0]=="-p") || (args[0]=="-pub") || (args[0]=="public")) ){
                                        Console.WriteLine("Save public key to file, after importing private key file.");
                                        //save pub from priv key
                                        if(args.Length == 1){Console.WriteLine("No keys specified..."); return;}

                                        priv = args[1];
                                        btnLoadPrivate_Click(priv);                                      //load priv key

                                        if( args.Length==2 && args[1].EndsWith(".rkf")){
                                                pub = args[1].Replace(".rkf", "") + "_public.rkf";
                                        }                                                                //save by generated filename, near privkey
                                        else if( args.Length==3 ){
                                                pub = args[2];
                                        }                                                                //save by specified pathway
                                        btnSavePublic_Click(pub);                                        //save public key, after importing privkey
                                        return;

                                }else if( ( args.Length <= 4 && args.Length > 1) ){
                                        //encrypt by pub[1] the file[2] and save to file[3].encrypted

                                        if( ( args[0] == "-e") || ( args[0]=="-enc" ) || ( args[0] == "encrypt" ) ){
                                                Console.WriteLine("Encrypt by public key the source file to destination encrypted...");
                                                if( args.Length >= 3 && args[1].IndexOf(".rkf")!=-1 ){pub = args[1]; src = args[2];}
                                                if( args.Length == 4 ){dest = args[3];}
                                                
                                                //run encrypt
                                                btnEncrypt_Click(pub, src, dest);
                                                return;

                                        }else if(         (args[0] == "-d") || (args[0] == "-dec") || ( args[0] == "decrypt" ) ){
                                                Console.WriteLine("Decrypt by private key the source file to destination decrypted...");
                                                //decrypt by priv[1] the file[2].encrypted and save to file[3]
                                                if( args.Length >= 3 && args[1].IndexOf(".rkf") != -1 ){priv = args[1]; src = args[2];}
                                                if( args.Length == 4 ){dest = args[3];}

                                                //run decrypt
                                                btnDecrypt_Click(priv, src, dest);
                                                return;

                                        }
                                }
                        }
                        
                        //else, if no any return - arguments not valid.
                        
                        //Show the error:
                        Console.Write("Invalid arguments specified: ");

                        if(args.Length==0){Console.Write ("No any command line arguments to display.");}
                        else{
                                //Show this arguments:
                                for(int i = 0; i < args.Length; i++ ){
                                        Console.Write(args[i]+" ");
                                }
                        }

                        //Show help usage:
                        Console.WriteLine(
                                                                "\n\nUsage:\n\n"+
                                                                "program.exe [param] [key] [src] [dest]\n"+
                                                                "\twhere key, src, dest - files pathways.\n\n"+
                                                                "params: \n"+
                                                                " [-g, -gen, generate]\n"+
                                                                "\tgenerate keypair (priv + pub)\n"+
                                                                "\tand save this to [key] (priv) and [src] (pub)\n"+
                                                                " [-p, -pub, public]\n"+
                                                                "\tget public key from imported [priv]\n"+
                                                                "\tand save this to [src] (pub keyfile)\n"+
                                                                " [-e, -enc, encrypt]\n"+
                                                                "\tencrypt with [pub_key.rkf]\n"+
                                                                "\t(or import pub from priv_key.rkf),\n"+
                                                                "\tthe file [src] to encrypted [dest]\n"+
                                                                " [-d, -dec, decrypt]\n"+
                                                                "\tdecrypt with [priv_key.rkf]\n"+
                                                                "\t the file [src] to decrypted [dest]\n"
                                                                
                        );
                        //Show this message:
                        Console.WriteLine("Press any key to exit...");
                        //And don't close console window.
                        Console.ReadKey();
                        
                        // Hide
                        var handle = GetConsoleWindow();            //get console window
                        ShowWindow(handle, SW_HIDE);                //hide this after press button...

                        // Show
                        //ShowWindow(handle, SW_SHOW);              //or show this...
                        
                        //And run winform application...
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new MainForm());
                }

                //progress bar for console.
                private static void ProgressBar(int progress, int tot, string value_name)
                {
                        //draw empty progress bar
                        Console.CursorLeft = 0;
                        Console.Write("["); //start
                        Console.CursorLeft = 32;
                        Console.Write("]"); //end
                        Console.CursorLeft = 1;
                        float onechunk = 30.0f / tot;

                        //draw filled part
                        int position = 1;
                        for (int i = 0; i < onechunk * progress; i++)
                        {
                                Console.BackgroundColor = ConsoleColor.Green;
                                Console.CursorLeft = position++;
                                Console.Write(" ");
                        }

                        //draw unfilled part
                        for (int i = position; i <= 31; i++)
                        {
                                Console.BackgroundColor = ConsoleColor.Gray;
                                Console.CursorLeft = position++;
                                Console.Write(" ");
                        }

                        //draw totals
                        Console.CursorLeft = 35;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(progress.ToString() + " "+ value_name + " of " + tot.ToString() + "        "); //blanks at the end remove any excess
                }
                
                //define private static variables
                private static int bitlength = 4096;        //4096 bits is bitlength for keys, by default. 4096 [bits] / 8 [bits/bytes] = 512 [bytes] - in each block of cyphertext.
                //private static int bitlength = 512;        //faster for testing
                
                private static string src        =        "";                //source file pathway
                private static string dest       =        "";                //destination file pathway
                private static string priv       =        "";                //pathway to keyfile (priv)
                private static string pub        =        "";                //pathway to keyfile (pub)
                
                private static RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                private static string inFile, outFile;

                private static void btnGenerate_Click()
                {
                        //int bitlength = 2048;        //already defined earlier

                        Console.WriteLine("Wait for generate keys... (key size bitlength = "+bitlength+" bits)");
                        
                        ProgressBar( 0, 100, "%" );

                        try{
                                rsa = new RSACryptoServiceProvider(bitlength);
                                RSAParameters pa = rsa.ExportParameters(true);

                                ProgressBar( 100, 100, "%" );
                                
                                Console.WriteLine("\nNew key pair generated! BitLength: "+bitlength+" bits ("+bitlength/8+" bytes).");
                        }catch (Exception ex){
                                Console.WriteLine("Error:" + ex.Message);
                        }
                }

                private static void btnLoadPublic_Click(string pub)
                {
                        try{
                                Console.WriteLine("Load the public key from "+pub+"...");
                                System.IO.StreamReader reader = System.IO.File.OpenText(pub);
                                string s = reader.ReadToEnd();
                                rsa.FromXmlString(s);
                                reader.Close();
                                Console.WriteLine("Public key loaded from "+pub);
                        }catch (Exception ex){
                                Console.WriteLine("Error:" + ex.Message);
                        }
                }

                private static void btnLoadPrivate_Click(string priv)
                {
                        try
                        {
                                Console.WriteLine("Load the private key from "+priv+"...");
                                System.IO.StreamReader reader = System.IO.File.OpenText(priv);
                                string s = reader.ReadToEnd();
                                rsa.FromXmlString(s);
                                reader.Close();
                                Console.WriteLine("Private key - loaded from "+priv);
                        }
                        catch (Exception ex)
                        {
                                Console.WriteLine("Error:" + ex.Message);
                        }
                }

                private static void btnCancel_Click()
                {
                        Console.WriteLine("Try to cancel...");
                }

                private static void btnSavePublic_Click(string pub)
                {
                        try
                        {
                                if (System.IO.File.Exists(pub)){        System.IO.File.Delete(pub);                }
                                System.IO.StreamWriter writer = System.IO.File.CreateText(pub);
                                writer.Write(rsa.ToXmlString(false));
                                writer.Close();
                                Console.WriteLine( "Public key saved to: \"" + pub +"\"" );
                        }
                        catch (Exception ex)
                        {
                                Console.WriteLine("Error:" + ex.Message);
                        }
                }

                private static void btnSavePrivate_Click(string priv)
                {
                        try
                        {
                                if (System.IO.File.Exists(priv)){        System.IO.File.Delete(priv);        }
                                System.IO.StreamWriter writer = System.IO.File.CreateText(priv);
                                writer.Write(rsa.ToXmlString(true));
                                writer.Close();
                                Console.WriteLine( "Private key saved to: \"" + priv + "\"" );
                        }
                        catch (Exception ex)
                        {
                                Console.WriteLine("Error:" + ex.Message);
                        }
                }

                private static void btnEncrypt_Click(string key="", string src="", string dest="")
                {
                        try
                        {
                                        if(key!=""){        btnLoadPublic_Click(key);        }

                                        inFile = src;
                                        outFile = (dest!="") ? dest : (src + ".encrypted");

                                        if (System.IO.File.Exists(outFile)){        System.IO.File.Delete(outFile); }

                                        Encrypt(key, inFile, outFile);
                        }
                        catch (Exception ex)
                        {
                                Console.WriteLine( "Error:" + ex.Message );
                        }
                }

                private static void btnDecrypt_Click(string priv="", string src="", string dest="")
                {
                        try
                        {
                                if(priv!=""){        btnLoadPublic_Click(priv);        }
                                        outFile = (dest!="") ? dest : src.Substring(0, src.Length - (".encrypted".Length));
                                        if (System.IO.File.Exists(outFile))
                                                System.IO.File.Delete(outFile);
                                        inFile = src;
                                        
                                        Decrypt(priv, inFile, outFile);
                        }
                        catch (Exception ex)
                        {
                                Console.WriteLine( "Error:" + ex.Message );
                        }
                }

                private static void Encrypt(string key="", string src = "", string dest = "")        //key - pub, for encrypt, or priv, to get pub from it, and encrypt then.
                {
                        Console.WriteLine( "Start encrypting by pub \""+key+"\" the source file \""+src+"\" to encrypted file \""+dest+"\"" );

                        System.IO.FileStream fout = System.IO.File.Open(outFile, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Write);
                        System.IO.FileStream fin = System.IO.File.Open(inFile, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);

                        try
                        {
                                int block_length         =         (bitlength/8)-12;                 //block length for source file
                                int block_size           =         (bitlength/8);                    //block length for destination file
                                
                                System.IO.FileInfo finfo = new System.IO.FileInfo( (src == "") ? inFile : src );

                                int min_progress = 0;
                                int max_progress = (int)(finfo.Length / block_length); //blocks
                                int current_progress = 0;
                                
                                Console.WriteLine( "Encrypting..." );
                                ProgressBar( min_progress, max_progress, "%");

                                byte[] buffer = new byte[block_length];
                                byte[] encbuffer = new byte[block_size];
                                int c = 0;
                                while ((c = fin.Read(buffer, 0, block_length)) > 0)
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

                                        current_progress += 1;
                                        ProgressBar( current_progress, max_progress, "blocks");
                                }
                                ProgressBar( max_progress, max_progress, "blocks");
                                
                                fin.Close();
                                fout.Close();
                                Console.WriteLine( "\nEncryption successful. Result: \"" + dest + "\"" );
                        }
                        catch (Exception ex)
                        {
                                ProgressBar( 0, 100, "blocks");
                                fin.Close();
                                fout.Close();
                                Console.WriteLine( "\nError:" + ex.Message );
                        }
                }

                private static void Decrypt(string priv="", string src="", string dest="")
                {
                        Console.WriteLine( "Start decrypting by priv \""+priv+"\" the encrypted file \""+src+"\" to decrypted file \""+dest+"\"" );
                        System.IO.FileStream fin = System.IO.File.Open(inFile, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                        System.IO.FileStream fout = System.IO.File.Open(outFile, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Write);
                                
                        try
                        {
                                int block_size = (bitlength/8);
                                System.IO.FileInfo finfo = new System.IO.FileInfo(inFile);

                                int min_progress = 0;
                                int max_progress = (int)(finfo.Length / block_size);
                                int current_progress = 0;

                                Console.WriteLine("Decrypting...");
                                ProgressBar( min_progress, max_progress, "%" );

                                byte[] buffer = new byte[block_size];
                                byte[] encbuffer = new byte[block_size];
                                int c = 0;
                                while ((c = fin.Read(buffer, 0, block_size)) > 0)
                                {
                                        encbuffer = rsa.Decrypt(buffer, false);
                                        fout.Write(encbuffer, 0, encbuffer.Length);
                                        current_progress += 1;
                                        ProgressBar( current_progress, max_progress, "blocks" );
                                }
                                ProgressBar( max_progress, max_progress, "blocks" );
                                fin.Close();
                                fout.Close();
                                Console.WriteLine( "\nDecryption successful. Result: \"" + dest +"\"" );
                        }
                        catch (Exception ex)
                        {
                                ProgressBar( 0, 100, "blocks" );
                                fin.Close();
                                fout.Close();
                                Console.WriteLine( "\nError:" + ex.Message +"aaaaaaaaaaa");
                        }
                }
        }
}
