Imports Org.BouncyCastle.Crypto
Imports Org.BouncyCastle.Crypto.Parameters
Imports Org.BouncyCastle.Pkcs
Imports Org.BouncyCastle.Security
Imports Org.BouncyCastle.X509
Imports System.IO

Public Class Pkcs12Pfx

    Public Function Generar(llave As Byte(), cer As Byte(), password As String) As Byte()
        Try
            Dim key As AsymmetricKeyParameter = PrivateKeyFactory.DecryptKey(password.ToCharArray(), llave)
            If TypeOf key Is RsaPrivateCrtKeyParameters Then


            End If
            Dim cert As X509Certificate = New X509CertificateParser().ReadCertificate(cer)
            Dim pkcs12Store As New Pkcs12Store()
            pkcs12Store.SetKeyEntry("Pkcs12Cache", New AsymmetricKeyEntry(key), New X509CertificateEntry(0) {New X509CertificateEntry(cert)})
            Dim memoryStream As New MemoryStream()
            pkcs12Store.Save(DirectCast(memoryStream, Stream), password.ToCharArray(), New SecureRandom())
            Dim buffer As Byte() = memoryStream.GetBuffer()
            memoryStream.Flush()
            memoryStream.Close()
            Return buffer
        Catch ex As Exception
            Return DirectCast(Nothing, Byte())
        End Try
    End Function
End Class
