::This file will be runned after sucessfully compilation

::Generate keys and save private key only
rsaVault.exe -g "only_priv.rkf"
::Generate keys and save priv and pub
rsaVault -g priv.rkf pub.rkf

::Save public key (-p) from "priv.rkf" to "pub_key.rkf", after importing "priv.rkf".
rsaVault -p "priv.rkf" "pub_key.rkf"
::Get private key from "priv.rkf" and save this
::to generated filename "priv(.rkf - substring)_public.rkf" (this was not specified)...
rsaVault -p "priv.rkf"

::Encrypt by "pub.rkf" the source file "test_text.txt" to out destination encrypted file "encrypted_text.enc"
rsaVault.exe -e "pub.rkf" "test_text.txt" "encrypted_text.enc"
::Decrypt by "priv.rkf" encrypted source file "encrypted_text.enc" to decrypted destination file "test_text2.txt"
rsaVault -d "priv.rkf" "encrypted_text.enc" "test_text2.txt"
::"test_text.txt" and "test_text2.txt" are equals, then

::Encrypt by "priv.rkf" the source file "test_text.txt" to out destination encrypted file "test_text.txt.encrypted" (filename not specified)
rsaVault -e "priv.rkf" "test_text.txt"
::Decrypt by "priv.rkf" encrypted source file "test_text.txt.encrypted" to decrypted destination file "test_text2.txt"
rsaVault.exe -d "priv.rkf" "test_text.txt.encrypted" test_text2.txt
::"test_text.txt" and "test_text2.txt" are equals, then

pause