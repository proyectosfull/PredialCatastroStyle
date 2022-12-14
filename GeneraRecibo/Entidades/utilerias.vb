Imports System.Xml
Imports System.Security.Cryptography
Imports System.IO
Imports System.Text

Public Class utilerias

    Public Function remueveEspacios(ByVal df As XNamespace, ByVal xdoc As XDocument) As XDocument

        For Each atributo As XAttribute In xdoc.Root.Attributes
            atributo.Value = atributo.Value.ToString.Trim
        Next

        For Each elemento As XElement In xdoc.Elements
            recorre(elemento)
        Next

        Return xdoc
    End Function

    Public Sub recorre(ByRef d As XElement)
        If d.Elements.Count > 0 Then
            For Each elemento As XElement In d.Elements
                If elemento.Elements.Count > 0 Then
                    For Each atributo As XAttribute In elemento.Attributes
                        atributo.Value = atributo.Value.ToString.Trim
                    Next
                    recorre(elemento)
                Else
                    For Each atributo As XAttribute In elemento.Attributes
                        atributo.Value = atributo.Value.ToString.Trim
                    Next
                End If
            Next
        Else
            For Each atributo As XAttribute In d.Attributes
                atributo.Value = atributo.Value.ToString.Trim
            Next
        End If
    End Sub


    Public Function remueveCaracteres(ByVal xdoc As XDocument) As XDocument
        Dim cad As String = xdoc.ToString().Replace("&#xD;&#xA;", "")
        cad = cad.Replace("&#xA;", "")
        cad = cad.Replace(Chr(10), "")
        cad = cad.Replace(Chr(13), "")
        Return XDocument.Parse(cad)
    End Function

    Public Function remueveEnter(ByVal xdoc As XDocument) As XDocument
        Dim cad As String = xdoc.ToString().Replace(Chr(10), "").Replace(Chr(13), "")
        Return XDocument.Parse(cad)
    End Function

    Public Function Encrypt(ByVal plainText As String) As String

        Dim passPhrase As String = "yourPassPhrase_Cadena"
        Dim saltValue As String = "mySaltValue_Cadena"
        Dim hashAlgorithm As String = "SHA1"

        Dim passwordIterations As Integer = 2
        Dim initVector As String = "@1B2c3D4e5F6g7H8"
        Dim keySize As Integer = 256

        Dim initVectorBytes As Byte() = Encoding.ASCII.GetBytes(initVector)
        Dim saltValueBytes As Byte() = Encoding.ASCII.GetBytes(saltValue)

        Dim plainTextBytes As Byte() = Encoding.UTF8.GetBytes(plainText)


        Dim password As New PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations)

        Dim keyBytes As Byte() = password.GetBytes(keySize \ 8)
        Dim symmetricKey As New RijndaelManaged()

        symmetricKey.Mode = CipherMode.CBC

        Dim encryptor As ICryptoTransform = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes)

        Dim memoryStream As New MemoryStream()
        Dim cryptoStream As New CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)

        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length)
        cryptoStream.FlushFinalBlock()
        Dim cipherTextBytes As Byte() = memoryStream.ToArray()
        memoryStream.Close()
        cryptoStream.Close()
        Dim cipherText As String = Convert.ToBase64String(cipherTextBytes)
        Return cipherText
    End Function
    Public Function Decrypt(ByVal cipherText As String) As String
        Dim passPhrase As String = "yourPassPhrase_Cadena"
        Dim saltValue As String = "mySaltValue_Cadena"
        Dim hashAlgorithm As String = "SHA1"

        Dim passwordIterations As Integer = 2
        Dim initVector As String = "@1B2c3D4e5F6g7H8"
        Dim keySize As Integer = 256
        ' Convert strings defining encryption key characteristics into byte
        ' arrays. Let us assume that strings only contain ASCII codes.
        ' If strings include Unicode characters, use Unicode, UTF7, or UTF8
        ' encoding.
        Dim initVectorBytes As Byte() = Encoding.ASCII.GetBytes(initVector)
        Dim saltValueBytes As Byte() = Encoding.ASCII.GetBytes(saltValue)

        ' Convert our ciphertext into a byte array.
        Dim cipherTextBytes As Byte() = Convert.FromBase64String(cipherText)

        ' First, we must create a password, from which the key will be 
        ' derived. This password will be generated from the specified 
        ' passphrase and salt value. The password will be created using
        ' the specified hash algorithm. Password creation can be done in
        ' several iterations.
        Dim password As New PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations)

        ' Use the password to generate pseudo-random bytes for the encryption
        ' key. Specify the size of the key in bytes (instead of bits).
        Dim keyBytes As Byte() = password.GetBytes(keySize \ 8)

        ' Create uninitialized Rijndael encryption object.
        Dim symmetricKey As New RijndaelManaged()

        ' It is reasonable to set encryption mode to Cipher Block Chaining
        ' (CBC). Use default options for other symmetric key parameters.
        symmetricKey.Mode = CipherMode.CBC

        ' Generate decryptor from the existing key bytes and initialization 
        ' vector. Key size will be defined based on the number of the key 
        ' bytes.
        Dim decryptor As ICryptoTransform = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes)

        ' Define memory stream which will be used to hold encrypted data.
        Dim memoryStream As New MemoryStream(cipherTextBytes)

        ' Define cryptographic stream (always use Read mode for encryption).
        Dim cryptoStream As New CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read)

        ' Since at this point we don't know what the size of decrypted data
        ' will be, allocate the buffer long enough to hold ciphertext;
        ' plaintext is never longer than ciphertext.
        Dim plainTextBytes As Byte() = New Byte(cipherTextBytes.Length - 1) {}

        ' Start decrypting.
        Dim decryptedByteCount As Integer = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length)

        ' Close both streams.
        memoryStream.Close()
        cryptoStream.Close()

        ' Convert decrypted data into a string. 
        ' Let us assume that the original plaintext string was UTF8-encoded.
        Dim plainText As String = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount)

        ' Return decrypted string.   
        Return plainText
    End Function

    Public Function GetSHA1(str As String) As String
        Dim sha1 As SHA1 = SHA1Managed.Create()
        Dim encoding As New ASCIIEncoding()
        Dim stream As Byte() = Nothing
        Dim sb As New StringBuilder()
        stream = sha1.ComputeHash(encoding.GetBytes(str))
        For i As Integer = 0 To stream.Length - 1
            sb.AppendFormat("{0:x2}", stream(i))
        Next
        Return sb.ToString()
    End Function

End Class
