# rsaVault
rsa File Encryption

Hardcoded constants:
>MainForm.cs:
>public static int bitlength = 4096; //4096 bits is bitlength for keys, by default. 4096 [bits] / 8 [bits/bytes] = 512 [bytes] - in each block of cyphertext.
>...
>int block_length 	= 	(bitlength/8)-12;						//block length for source file
>int block_size 		= 	(bitlength/8);							//block length for destination file

Fast to encrypt, and slow decryption, because for each block:

c = (m^e mod N) faster than m = (c^d mod N);
where m - message, c - cyphertext, e - public exponent, d - private exponent, N - modulus.

_____

Here you can see the [source code](https://github.com/username1565/rsaVault/tree/master/rsaVault) and [instructions for compilation](https://github.com/username1565/rsaVault/blob/master/rsaVault/Compile.bat).

Win32 executable EXE (x86) you can find [in releases](https://github.com/username1565/rsaVault/releases).
