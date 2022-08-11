

Public Class DatosRecibo

#Region "Datos del comprobante"


    Public Comprobante_ As Comprobante

#End Region

#Region "Datos del emisor"

    '<DataMember>
    'Public RFC As String
    '<DataMember>
    'Public Calle As String
    '<DataMember>
    'Public Municipio As String
    '<DataMember>
    'Public Estado As String
    '<DataMember>
    'Public Pais As String
    '<DataMember>
    'Public CodigoPostal As String
    '<DataMember>
    'Public Nombre As String = "" 'opcional
    '<DataMember>
    'Public NoExterior As String = "" 'opcional
    '<DataMember>
    'Public NoInterior As String = "" 'opcional
    '<DataMember>
    'Public Colonia As String = "" 'opcional
    '<DataMember>
    'Public Localidad As String = "" 'opcional
    '<DataMember>
    'Public Referencia As String = "" 'opcional

#End Region

#Region "Datos de receptor"

    Public Receptor_ As Receptor
#End Region

#Region "Lista de conceptos"

    Public Conceptos_ As New List(Of Conceptos)
#End Region


End Class


Public Class Comprobante

    Public Folio As String

    Public Serie As String

    Public FormaDePago As String

    Public SubTotal As Double

    Public Total As Double

    Public TipoDeComprobante As String

    Public MetodoDePago As String

    Public Mesa As String

    Public cajero As String

    Public descuento As Double

    Public motivoDescuento As String

    Public DatosPredio As String

    Public Observaciones As String

    Public FolioInfraccion As String

    Public LugarExpedicion As String

End Class

'<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929"), _
' System.SerializableAttribute(), _
' System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.sat.gob.mx/cfd/3")> _
'Public Enum ComprobanteTipoDeComprobante
'    ingreso
'    egreso
'    traslado
'End Enum


Public Class Conceptos
    Public Sub New()

    End Sub
    Public Sub New(ByVal Cantidad As Double, ByVal Unidad As String, ByVal Descripcion As String, ByVal ValorUnitario As Double, ByVal Id As String, ByVal Importe As Double, ByVal Descuento As Double, ByVal CuentaPredial As String)
        Me.Cantidad = Cantidad
        Me.Unidad = Unidad
        Me.Descripcion = Descripcion
        Me.ValorUnitario = ValorUnitario
        Me.Id = Id
        Me.Importe = Importe
        Me.Descuento = Descuento
        Me.CuentaPredial = CuentaPredial
    End Sub
    Public Sub New(ByVal Cantidad As Double, ByVal Unidad As String, ByVal Descripcion As String, ByVal ValorUnitario As Double, ByVal Id As String, ByVal Importe As Double, ByVal Descuento As Double, ByVal CuentaPredial As String, ByVal ClaveProdServ As String, ByVal ClaveUnidad As String)
        Me.Cantidad = Cantidad
        Me.Unidad = Unidad
        Me.Descripcion = Descripcion
        Me.ValorUnitario = ValorUnitario
        Me.Id = Id
        Me.Importe = Importe
        Me.Descuento = Descuento
        Me.CuentaPredial = CuentaPredial
        Me.ClaveProdServ = ClaveProdServ
        Me.ClaveUnidad = ClaveUnidad
    End Sub

    Public ClaveProdServ As String

    Public Cantidad As Double

    Public ClaveUnidad As String

    Public Unidad As String

    Public Descripcion As String

    Public ValorUnitario As Double

    Public Id As String

    Public Importe As Double

    Public Descuento As Double

    Public CuentaPredial As String

End Class


Public Class Receptor


    Public Id As Integer = 0 'opcional

    Public RFC As String = "" 'opcional

    Public Nombre As String = "" 'opcional

    Public UsoCFDI As String = "" 'opcional

    Public email As String = "" 'email

    Public Domicilio As String = "" 'email

End Class

