Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Xml.Schema

Public Class CFDx
    Private cComprobante As New Comprobante
    Private cEmisor As New ComprobanteEmisor
    Private cDomicilioFiscalEmisor As New t_UbicacionFiscal
    Private cExpedidoEn As New t_Ubicacion
    Private cRegimen() As ComprobanteEmisorRegimenFiscal
    Private cReceptor As New ComprobanteReceptor
    Private cDomicilioRecetor As New t_Ubicacion
    Private cConceptos As ComprobanteConcepto()
    Private cImpuestos As New ComprobanteImpuestos


    Private cSchemma As String
    Private Tasa As Double

    Public Enum VersionCFD
        CFDv3_0
        CFDv3_2
    End Enum

    ''' <summary>
    ''' Contiene dos 'Key's : CFDv3 y CFDv32
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Schemma As Hashtable
        Get
            Schemma = New Hashtable
            Schemma.Add("CFDv3", "http://www.sat.gob.mx/cfd/3  http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv3.xsd")
            Schemma.Add("CFDv32", "http://www.sat.gob.mx/cfd/3  http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd")
        End Get
    End Property

    ''' <summary>
    ''' Función que llena los Campos de la Classe Comprobante usando las variables privadas declaradas al principio
    ''' </summary>
    ''' <returns>Devuelve un Objeto de tipo Comprobante para la Serialización a XML</returns>
    ''' <remarks></remarks>
    Private Function CreaXML() As Comprobante
        Dim Comprobante As Comprobante = cComprobante

        With Comprobante
            .Emisor = cEmisor
            With .Emisor
                .DomicilioFiscal = cDomicilioFiscalEmisor
                .RegimenFiscal() = cRegimen
            End With

            If .version = "3.0" Then .Emisor.RegimenFiscal() = Nothing

            .Receptor = cReceptor
            With .Receptor
                .Domicilio = cDomicilioRecetor
            End With

            .Conceptos() = cConceptos

            .Impuestos = cImpuestos
        End With

        Return Comprobante
    End Function

    ''' <summary>
    ''' Método Privado para llenar los atributos y propiedades del objeto comprobante
    ''' </summary>
    ''' <param name="Version"></param>
    ''' <param name="folio"></param>
    ''' <param name="fecha"></param>
    ''' <param name="formaDePago"></param>
    ''' <param name="SubTotal"></param>
    ''' <param name="Total"></param>
    ''' <param name="tipoDeComprobante"></param>
    ''' <param name="metodoDePago"></param>
    ''' <param name="LugarDeExpedicion"></param>
    ''' <param name="serie"></param>
    ''' <param name="TipoDeCambio"></param>
    ''' <param name="condicionesDePago"></param>
    ''' <param name="descuento"></param>
    ''' <param name="motivoDescuento"></param>
    ''' <param name="Moneda"></param>
    ''' <param name="NumCtaPago"></param>
    ''' <param name="FolioFiscalOrig"></param>
    ''' <param name="SerieFolioFiscalOrig"></param>
    ''' <param name="FechaFolioFiscalOrig"></param>
    ''' <remarks></remarks>
    Public Sub Comprobante(ByVal Version As VersionCFD, ByVal folio As String, ByVal fecha As DateTime, ByVal formaDePago As String _
                       , ByVal SubTotal As Double, ByVal Total As Double, ByVal tipoDeComprobante As ComprobanteTipoDeComprobante _
                       , ByVal metodoDePago As String, ByVal LugarDeExpedicion As String _
                       , Optional ByVal serie As String = "", Optional ByVal TipoDeCambio As String = "" _
                       , Optional ByVal condicionesDePago As String = "", Optional ByVal descuento As Double = 0 _
                       , Optional ByVal motivoDescuento As String = "" _
                       , Optional ByVal Moneda As String = "M.N.", Optional ByVal NumCtaPago As String = "" _
                       , Optional ByVal FolioFiscalOrig As String = "", Optional ByVal SerieFolioFiscalOrig As String = "",
                         Optional ByVal FechaFolioFiscalOrig As Nullable(Of Date) = Nothing)

        With cComprobante
            Select Case Version
                Case VersionCFD.CFDv3_0
                    .version = "3.0"
                    .SchemaLocation = Schemma("CFDv3")
                Case VersionCFD.CFDv3_2
                    .version = "3.2"
                    .SchemaLocation = Schemma("CFDv32")
                    .LugarExpedicion = LugarDeExpedicion
            End Select

            .folio = folio
            .fecha = fecha
            .sello = ""
            .formaDePago = formaDePago
            .noCertificado = ""
            .certificado = ""
            .metodoDePago = metodoDePago
            .subTotal = SubTotal
            .total = Total
            .tipoDeComprobante = tipoDeComprobante
            .serie = serie
            .TipoCambio = TipoDeCambio
            .condicionesDePago = condicionesDePago
            If descuento > 0 Then
                .descuentoSpecified = True
                .descuento = descuento
                .motivoDescuento = motivoDescuento
            End If
            .Moneda = Moneda
            .NumCtaPago = NumCtaPago
            If FolioFiscalOrig <> "" Then
                .MontoFolioFiscalOrigSpecified = True
                .FolioFiscalOrig = FolioFiscalOrig
                .SerieFolioFiscalOrig = SerieFolioFiscalOrig
            End If
            If Not FechaFolioFiscalOrig Is Nothing Then
                .FechaFolioFiscalOrigSpecified = True
                .FechaFolioFiscalOrig = FechaFolioFiscalOrig
            End If

        End With

    End Sub
    ''' <summary>
    ''' Agrega los Datos del Emisor
    ''' </summary>
    ''' <param name="RFC"></param>
    ''' <param name="Calle"></param>
    ''' <param name="Municipio"></param>
    ''' <param name="Estado"></param>
    ''' <param name="Pais"></param>
    ''' <param name="CodigoPostal"></param>
    ''' <param name="Nombre"></param>
    ''' <param name="NoExterior"></param>
    ''' <param name="NoInterior"></param>
    ''' <param name="Colonia"></param>
    ''' <param name="Localidad"></param>
    ''' <param name="Referencia"></param>
    ''' <remarks></remarks>
    Public Sub AgregaEmisor(ByVal RFC As String, ByVal Calle As String, ByVal Municipio As String,
                 ByVal Estado As String, ByVal Pais As String, ByVal CodigoPostal As String,
                 Optional ByVal Nombre As String = "",
                 Optional NoExterior As String = "",
                 Optional NoInterior As String = "",
                 Optional Colonia As String = "",
                 Optional Localidad As String = "",
                 Optional Referencia As String = "")

        cEmisor.rfc = RFC
        cEmisor.nombre = Nombre
        AgregaEmisorDomicilioFiscal(Calle, Municipio, Estado, Pais, CodigoPostal, NoExterior, NoInterior, Colonia, Localidad, Referencia)

    End Sub
    Private Sub AgregaEmisorDomicilioFiscal(ByVal Calle As String, ByVal Municipio As String,
                                    ByVal Estado As String, ByVal Pais As String, ByVal CodigoPostal As String,
                                    Optional NoExterior As String = "",
                                    Optional NoInterior As String = "",
                                    Optional Colonia As String = "",
                                    Optional Localidad As String = "",
                                    Optional Referencia As String = "")

        With cDomicilioFiscalEmisor
            .calle = Calle
            .municipio = Municipio
            .estado = Estado
            .pais = Pais
            .codigoPostal = CodigoPostal
            .noExterior = NoExterior
            .noInterior = NoInterior
            .colonia = Colonia
            .localidad = Localidad
            .referencia = Referencia
        End With

    End Sub
    Public Sub AgregaRegimenFiscal(ByVal ParamArray Regimen() As String)
        Dim Regs As New List(Of ComprobanteEmisorRegimenFiscal)

        For Each r In Regimen
            Regs.Add(New ComprobanteEmisorRegimenFiscal With {.Regimen = r})
        Next

        cRegimen = Regs.ToArray
    End Sub
    Public Sub AgregaEmisorExpedidoEn(Optional Mismo As Boolean = True,
                                Optional Calle As String = "",
                                Optional Municipio As String = "",
                                Optional Estado As String = "",
                                Optional Pais As String = "",
                                Optional CodigoPostal As String = "",
                                Optional NoExterior As String = "",
                                Optional NoInterior As String = "",
                                Optional Colonia As String = "",
                                Optional Localidad As String = "",
                                Optional Referencia As String = "")

        If Mismo = True Then
            With cDomicilioFiscalEmisor
                Calle = .calle
                Municipio = .municipio
                Estado = .estado
                Pais = .pais
                CodigoPostal = .codigoPostal
                NoExterior = .noExterior
                NoInterior = .noInterior
                Colonia = .colonia
                Localidad = .localidad
                Referencia = .referencia
            End With
        End If

        With cExpedidoEn
            .calle = Calle
            .municipio = Municipio
            .estado = Estado
            .pais = Estado
            .codigoPostal = CodigoPostal
            .noExterior = NoExterior
            .noInterior = NoInterior
            .colonia = Colonia
            .localidad = Localidad
            .referencia = Referencia
        End With

    End Sub

    Public Sub AgregaReceptor(ByVal RFC As String, Optional ByVal Nombre As String = "",
                  Optional Calle As String = "",
                  Optional Municipio As String = "",
                  Optional Estado As String = "",
                  Optional Pais As String = "",
                  Optional CodigoPostal As String = "",
                  Optional NoExterior As String = "",
                  Optional NoInterior As String = "",
                  Optional Colonia As String = "",
                  Optional Localidad As String = "",
                  Optional Referencia As String = "")

        With cReceptor
            .rfc = RFC
            .nombre = Nombre
        End With

        AgregaReceptorDomicilioFiscal(Calle, Municipio, Estado, Pais, CodigoPostal, NoExterior, NoInterior, Colonia, Localidad, Referencia)

    End Sub
    Private Sub AgregaReceptorDomicilioFiscal(Optional Calle As String = "",
                                    Optional Municipio As String = "",
                                    Optional Estado As String = "",
                                    Optional Pais As String = "",
                                    Optional CodigoPostal As String = "",
                                    Optional NoExterior As String = "",
                                    Optional NoInterior As String = "",
                                    Optional Colonia As String = "",
                                    Optional Localidad As String = "",
                                    Optional Referencia As String = "")

        With cDomicilioRecetor
            .calle = Calle
            .municipio = Municipio
            .estado = Estado
            .pais = Pais
            .codigoPostal = CodigoPostal
            .noExterior = NoExterior
            .noInterior = NoInterior
            .colonia = Colonia
            .localidad = Localidad
            .referencia = Referencia
        End With

    End Sub

    'Aqui guardaremos todos los conceptos (Partidas de la Factura) para agregarlos al CFDI
    Dim Conceptos As New List(Of ComprobanteConcepto)
    Public Sub AgregaConcepto(ByVal Cantidad As Double,
                    ByVal Unidad As String,
                    ByVal Descripcion As String,
                    ByVal ValorUnitario As Double,
                    ByVal Importe As Double,
                    Optional NoIdentificacion As String = "",
                    Optional InformacionAduanera As t_InformacionAduanera = Nothing,
                    Optional CuentaPredialNumero As String = "",
                    Optional Complemento As String = "")

        If Not InformacionAduanera Is Nothing Then InformacionAduanera = _
                        New t_InformacionAduanera With {.aduana = "", .fecha = Today, .numero = ""}
        If Complemento Is Nothing Then Complemento = New XElement("Nothing", "")

        Conceptos.Add(New ComprobanteConcepto With _
                      {.cantidad = Cantidad,
                       .unidad = Unidad,
                       .descripcion = Descripcion,
                       .valorUnitario = ValorUnitario,
                       .importe = Importe,
                       .noIdentificacion = NoIdentificacion,
                       .Items = {IIf(Not InformacionAduanera Is Nothing,
                                InformacionAduanera, Nothing),
                                IIf(CuentaPredialNumero <> "",
                                New ComprobanteConceptoCuentaPredial With {.numero = CuentaPredialNumero}, Nothing)
                                }
                      }
                    )

        cConceptos = Conceptos.ToArray
    End Sub

    Dim Traslados As New List(Of ComprobanteImpuestosTraslado)
    Dim Retenciones As New List(Of ComprobanteImpuestosRetencion)
    Sub AgregaImpuesto(ByVal Trasladado As ComprobanteImpuestosTrasladoImpuesto,
                       ByVal TrasladadoImporte As Double,
                       ByVal TrasladoTasa As Double,
                       Optional Retenido As ComprobanteImpuestosRetencionImpuesto = Nothing,
                       Optional RetenidoImporte As Nullable(Of Double) = Nothing)

        Traslados.Add(New ComprobanteImpuestosTraslado With {.impuesto = Trasladado,
                                                             .importe = TrasladadoImporte,
                                                             .tasa = TrasladoTasa})
        If Not RetenidoImporte Is Nothing Then
            Retenciones.Add(New ComprobanteImpuestosRetencion With {.impuesto = Retenido, .importe = RetenidoImporte})
        End If

        With cImpuestos
            .Traslados = Traslados.ToArray
            If Retenciones.Count > 0 Then .Retenciones = Retenciones.ToArray

            .totalImpuestosTrasladados = .Traslados.Sum(Function(n) n.importe)
            If Retenciones.Count > 0 Then
                .totalImpuestosRetenidos = .Retenciones.Sum(Function(n) n.importe)
            Else
                .totalImpuestosRetenidos = 0
            End If
        End With

    End Sub

    ''' <summary>
    ''' Crea el Archivo XML con los Datos del CFDI
    ''' </summary>
    ''' <returns>Regresa True si se creó el CFDI exitósamente, sino, devuelve False</returns>
    ''' <remarks></remarks>
    Public Function CreaFacturaXML(ByVal path As String, ByVal KeyFile As Byte(), ByVal Pass As String, ByVal CerFile As Byte(), ByRef Errores As String, folio As String) As XDocument
        ' Dim Resultado As Boolean = False
        Dim c As Comprobante = CreaXML()
        Dim valueRND As Integer = CInt(Int((99 * Rnd()) + 1))

       

        'Dim Path As String = Environment.CurrentDirectory
        'Dim name fil
        Dim XMLfile As String = IO.Path.Combine(path, folio & ".xml")


        If IO.File.Exists(XMLfile) = True Then IO.File.Delete(XMLfile)

        Dim xmlTextW As New Xml.XmlTextWriter(XMLfile, Encoding.UTF8)

        Dim ns As New Xml.Serialization.XmlSerializerNamespaces

        ns.Add("cfdi", "http://www.sat.gob.mx/cfd/3")
        ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance")

        Dim Serialize As New Xml.Serialization.XmlSerializer(GetType(Comprobante))
        Serialize.Serialize(xmlTextW, c, ns)

        xmlTextW.Close()
        Dim xDoc As XDocument = XDocument.Load(XMLfile)



        SellarXML(KeyFile, Pass, CerFile, xDoc, Errores)

        If ValidaXML(xDoc, VersionCFD.CFDv3_2, Errores) = True Then
            ' Resultado = True            '
            'Process.Start("firefox.exe", "https://www.consulta.sat.gob.mx/sicofi_web/moduloECFD_plus/ValidadorCFDI/Validador%20cfdi.html")
            'Process.Start("firefox.exe", XMLfile)
            xDoc.Save(XMLfile)
            Return xDoc
        Else
            Return New XDocument
        End If
    End Function

    ''' <summary>
    ''' Función que Elimina los Atributos y Elementos que estan vacíos
    ''' </summary>
    ''' <param name="el"></param>
    ''' <param name="includeChildNodes"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function RemoveAllemptys(ByVal el As XElement, Optional ByVal includeChildNodes As Boolean = False) As XElement
        Dim current = el.LastAttribute

        Do While current IsNot Nothing
            Dim temp = current.PreviousAttribute
            If current.Value = "" Then
                If current.Name <> "sello" Then
                    current.Remove()
                End If
            End If
            current = temp
        Loop

        If includeChildNodes Then
            For Each child In el.Descendants
                RemoveAllemptys(child)
            Next
        End If

        Return el
    End Function

#Region "Validar XML"
    Dim TotErrores As New System.Text.StringBuilder
    ''' <summary>
    ''' Valida la estructura del XML generado usando un archivo XSD
    ''' </summary>
    ''' <param name="xml"></param>
    ''' <param name="xsdNameSpace" description="Name Space del Archivo XSD"></param>
    ''' <returns>Regresa False si el XML tiene errores VS el Archivo XSD, True si no Contiene Errores</returns>
    Public Function ValidaXML(ByVal xml As XDocument, ByVal Version As VersionCFD, ByRef Errores As String, _
                              Optional ByVal xsdNameSpace As String = "") As Boolean

        Dim Valido As Boolean = True
        Dim settings As New XmlReaderSettings()

        If String.IsNullOrEmpty(xsdNameSpace) = True Then
            xsdNameSpace = "http://www.sat.gob.mx/cfd/3"
        End If

        Dim Path As String = System.AppDomain.CurrentDomain.BaseDirectory
        Dim xsdFile As String = ""
        Select Case Version
            Case VersionCFD.CFDv3_0
                xsdFile = IO.Path.Combine(Path, "XSD\cfdv3.xsd")
            Case VersionCFD.CFDv3_2
                xsdFile = IO.Path.Combine(Path, "XSD\cfdv32.xsd")
        End Select

        settings.Schemas.Add(xsdNameSpace, xsdFile)
        settings.ValidationType = ValidationType.Schema
        settings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings

        AddHandler settings.ValidationEventHandler, _
        AddressOf settings_ValidationEventHandler

        'settings.IgnoreWhitespace = True
        'settings.IgnoreComments = True

        Dim Temp = System.IO.Path.GetTempFileName()
        xml.Save(Temp)

        Using reader As XmlReader = XmlReader.Create(Temp, settings)
            While (reader.Read())
                'Do Nothing
            End While
        End Using

        If TotErrores.ToString <> "" Then
            Valido = False
            Errores = TotErrores.ToString
        End If

        'Elimina archivo temporal Temp
        If File.Exists(Temp) Then
            File.Delete(Temp)
        End If

        Return Valido
    End Function
    Private Sub settings_ValidationEventHandler(ByVal sender As Object, _
            ByVal e As System.Xml.Schema.ValidationEventArgs)
        TotErrores.Append(e.Message & vbNewLine)
    End Sub
#End Region

#Region "Sello Digital"
    ''' <summary>
    ''' Obtiene la Cadena Original con las Especificaciones del Anexo 20 (SAT) en Base al Archivo XML
    ''' </summary>
    ''' <param name="XMLstring">El Archivo XML que contiene los Datos para la Factura Electrónica</param>
    ''' <param name="fileXSLT">El Archivo XSLT que contiene las especifícaciones (SAT) para Obtener la Cadena Original</param>
    ''' <returns>Regresa un Objeto tipo String con el Formato de Cadena Original (SAT)</returns>
    ''' <remarks></remarks>
    Public Function GetCadenaOriginal(ByVal XMLstring As XDocument, ByVal fileXSLT As String) As String
        Dim CadenaOriginalSTR As String

        Dim xComprobante As XElement = XMLstring.Root
        RemoveAllemptys(xComprobante, True)

        Dim XMLfile As String = Path.GetTempFileName()
        Dim newFile = Path.GetTempFileName()

        xComprobante.Save(XMLfile)

        Dim Xsl = New Xsl.XslCompiledTransform()
        Xsl.Load(fileXSLT)
        Xsl.Transform(XMLfile, newFile)
        Xsl = Nothing

        Dim StreamReader = New IO.StreamReader(newFile)
        CadenaOriginalSTR = StreamReader.ReadToEnd
        StreamReader.Close()

        '// Eliminamos los archivos
        System.IO.File.Delete(newFile)
        System.IO.File.Delete(XMLfile)

        fileXSLT = Nothing
        newFile = Nothing
        XMLfile = Nothing
        Xsl = Nothing
        StreamReader.Dispose()

        Return CadenaOriginalSTR
    End Function

    ''' <summary>
    ''' Crea el Sello Digital del Emisor Requerido para la Factura Electrónica (SAT)
    ''' </summary>
    ''' <param name="KeyFile">El archivo key (*.key) (FIEL) del Emisor</param>
    ''' <param name="Pass">La Contraseña para leer el Archivo key</param>
    ''' <param name="CerFile">El Certificado (*.cert) (FIEL) del Emisor</param>
    ''' <param name="XMLstring">El Archivo XML sin Sellar</param>
    ''' <remarks></remarks>
    Public Sub SellarXML(ByVal KeyFile As Byte(), ByVal Pass As String, _
                            ByVal CerFile As Byte(), ByRef XMLstring As XDocument, ByRef Errores As String)

        AgregaCertificado(CerFile, XMLstring)

        Dim Path As String = System.AppDomain.CurrentDomain.BaseDirectory
        Dim XSLT As String = IO.Path.Combine(Path, "XSLT\cadenaoriginal_3_2.xslt")
        If XMLstring.Root.@version = "3.0" Then XSLT = IO.Path.Combine(Path, "XSLT\cadenaoriginal_3_0.xslt")

        Dim Cadena As String = GetCadenaOriginal(XMLstring, XSLT)

        Dim Hash As String = "md-5"
        Dim Año As Integer = Year(Now)
        If Año >= 2011 Then Hash = "SHA-1"

        Dim privKey As New Chilkat.PrivateKey

        '  Load the private key from an RSA .key file:
        If privKey.LoadPkcs8Encrypted(KeyFile, Pass) = False Then
            'MsgBox(privKey.LastErrorText)
            Errores = privKey.LastErrorText
            Exit Sub
        End If

        '  Get the private key in XML format:
        Dim privKeyXml As String = privKey.GetXml()

        Dim rsa As New Chilkat.Rsa

        If rsa.UnlockComponent("RSAT34MB34N_7F1CD986 683M") = False Then
            'AlgoritmoRSA.UnlockComponent("RSAT34MB34N_7F1CD986 683M");
            'MsgBox(rsa.LastErrorText)
            Errores = privKey.LastErrorText
            Exit Sub
        End If

        '  Import the private key into the RSA component:
        If rsa.ImportPrivateKey(privKeyXml) = False Then
            'MsgBox(rsa.LastErrorText)
            Errores = privKey.LastErrorText
            Exit Sub
        End If

        '  Create the signature as a base64 string:
        rsa.Charset = "utf-8"
        rsa.EncodingMode = "base64"

        '  If some other non-Chilkat application or web service is going to be verifying
        '  the signature, it is important to match the byte-ordering.
        '  The LittleEndian property may be set to 1
        '  for little-endian byte ordering,
        '  or 0  for big-endian byte ordering.
        '  Microsoft apps typically use little-endian, while
        '  OpenSSL and other services (such as Amazon CloudFront)
        '  use big-endian.
        rsa.LittleEndian = 0

        ''CERTIFICADO
        Dim cert As New Chilkat.Cert
        ''  Load a digital certificate from a .cer file:
        If cert.LoadFromBinary(CerFile) = False Then
            'MsgBox(cert.LastErrorText)
            Errores = privKey.LastErrorText
            Exit Sub
        End If

        'Dim strData As String = "This is the string to be signed."
        '  Sign the string using the sha-1 hash algorithm.
        '  Other valid choices are "md2", "sha256", "sha384",
        '  "sha512", and "sha-1".
        Dim Sellobase64 As String = rsa.SignStringENC(Cadena, Hash)

        XMLstring.Root.SetAttributeValue("sello", Sellobase64)

        Dim xComprobante As XElement = XMLstring.Root
        xComprobante = RemoveAllemptys(XMLstring.Root, True)

        Dim pubKey As Chilkat.PublicKey
        pubKey = cert.ExportPublicKey()

        '  Now verify using a separate instance of the RSA object:
        Dim rsa2 As New Chilkat.Rsa

        '  Import the public key into the RSA object:
        If rsa2.ImportPublicKey(pubKey.GetXml()) = False Then
            'MsgBox(rsa2.LastErrorText)
            Errores = privKey.LastErrorText
            Exit Sub
        End If

        '  The signature is a hex string, so make sure the EncodingMode is correct:
        rsa2.Charset = "utf-8"
        rsa.EncodingMode = "base64"
        rsa2.LittleEndian = 0

        '  Verify the signature:
        If rsa2.VerifyStringENC(Cadena, Hash, Sellobase64) = False Then
            'MsgBox(rsa2.LastErrorText)
            Errores = privKey.LastErrorText
            Exit Sub
        End If

        KeyFile = Nothing
        CerFile = Nothing
        Pass = Nothing
        privKey = Nothing
        rsa = Nothing
        rsa2 = Nothing
        pubKey = Nothing
        Hash = Nothing
        cert.Dispose()
        xComprobante = Nothing
    End Sub
    Private Sub AgregaCertificado(ByVal CerFile As Byte(), ByRef XMLstring As XDocument)
        Dim cert As New Chilkat.Cert
        ''  Load a digital certificate from a .cer file:
        If cert.LoadFromBinary(CerFile) = False Then
            MsgBox(cert.LastErrorText)
            Exit Sub
        End If

        'Verifica que el Certificado no esté Caducado
        ' If cert.Expired = True Then
        'MsgBox("El Certificado ya está Caducado: " & cert.ValidTo) : Exit Sub
        'End If

        Dim Serie As String = cert.SerialNumber
        Dim strTemp As String = ""
        Dim NumCertificado As String = ""

        'Obtiene Número de Serie del Certificado
        For I As Long = 1 To Len(Serie) Step 2
            strTemp = Chr(Val("&H" & Mid$(Serie, I, 2)))
            NumCertificado = NumCertificado & strTemp
        Next I
        Serie = NumCertificado

        'Agrega Certificado y No.Certificado al XML
        XMLstring.Root.@noCertificado = NumCertificado
        XMLstring.Root.@certificado = cert.GetEncoded

        Serie = Nothing
        strTemp = Nothing
        NumCertificado = Nothing
    End Sub
#End Region


End Class
