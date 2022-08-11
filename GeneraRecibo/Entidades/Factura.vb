
Public Class Factura

    Public folio As String

    Public serie As String

    Public xml As String

    Public pdfBytes As Byte()

    Public mimeType As String

    Public filenameExtension As String

    Public selloDigital As String

    Public codigoReimpresion As String

    Public Errores As String

    Public RND As String

    Public Ruta As String

    Public Rfc As String

    Public FechaFactura As DateTime

    Public Sub Factura()
        Me.Errores = String.Empty
    End Sub

End Class

