'Imports Clases.BL
'Imports Clases
Imports Recibo
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Web
Imports GeneraRecibo.facturainteligente2
Imports MessagingToolkit.QRCode.Codec
Imports System.Drawing
Imports System.Configuration

Public Class ReciboCFDI
    Public Function generaRecibo(datos As DatosRecibo, path As String) As Recibo
        path = ConfigurationManager.AppSettings("RutaRecibos")
        Dim Errores As String = ""
        Try

            Dim datosFiel As DataTable = New DAL().cFIELGetByActive()
            Dim valorRecibo As Recibo = New Recibo()
            Dim FechaHora As DateTime = Now
            Dim TipoDeComprobante As ComprobanteTipoDeComprobante
            Select Case datos.Comprobante_.TipoDeComprobante.ToLower
                Case "ingreso"
                    TipoDeComprobante = ComprobanteTipoDeComprobante.ingreso
                Case "egreso"
                    TipoDeComprobante = ComprobanteTipoDeComprobante.egreso
                Case "traslado"
                    TipoDeComprobante = ComprobanteTipoDeComprobante.traslado
            End Select

            Dim Version As CFDx.VersionCFD = CFDx.VersionCFD.CFDv3_2

            Dim CFDs As New CFDx
            With CFDs
                .Comprobante(Version, datos.Comprobante_.Folio, FormatDateTime(FechaHora, DateFormat.GeneralDate), _
                        datos.Comprobante_.FormaDePago, datos.Comprobante_.SubTotal, datos.Comprobante_.Total,
                        TipoDeComprobante, datos.Comprobante_.MetodoDePago, datosFiel(0)("Municipio") & ", " & datosFiel(0)("Estado"), datos.Comprobante_.Serie, _
                         "", "", datos.Comprobante_.descuento, datos.Comprobante_.motivoDescuento)

                .AgregaEmisor(datosFiel(0)("RFC"), datosFiel(0)("Calle"), datosFiel(0)("Municipio"), datosFiel(0)("Estado"), datosFiel(0)("Pais"),
                              datosFiel(0)("CodigoPostal"), datosFiel(0)("Nombre"), datosFiel(0)("NoExterior"), datosFiel(0)("NoInterior"),
                              datosFiel(0)("Colonia"), datosFiel(0)("Localidad"), datosFiel(0)("Referencia"))

                .AgregaReceptor(datos.Receptor_.RFC,
                                IIf(datos.Receptor_.Nombre = Nothing, "Publico en general", datos.Receptor_.Nombre),
                                IIf(datos.Receptor_.Calle = Nothing, " ", datos.Receptor_.Calle),
                                IIf(datos.Receptor_.Municipio = Nothing, " ", datos.Receptor_.Municipio),
                                IIf(datos.Receptor_.Estado = Nothing, " ", datos.Receptor_.Estado),
                                IIf(datos.Receptor_.Pais = Nothing, " ", datos.Receptor_.Pais),
                                IIf(datos.Receptor_.CodigoPostal = Nothing, " ", datos.Receptor_.CodigoPostal),
                                IIf(datos.Receptor_.NoExterior = Nothing, " ", datos.Receptor_.NoExterior),
                                IIf(datos.Receptor_.NoInterior = Nothing, " ", datos.Receptor_.NoInterior),
                                IIf(datos.Receptor_.Colonia = Nothing, " ", datos.Receptor_.Colonia),
                                IIf(datos.Receptor_.Localidad = Nothing, " ", datos.Receptor_.Localidad),
                                IIf(datos.Receptor_.Referencia = Nothing, " ", datos.Receptor_.Referencia)
                                )

                'Solo en caso de ser CFD v3.2
                .AgregaRegimenFiscal("PERSONAS MORALES CON FINES NO LUCRATIVOS")

                For Each c As Conceptos In datos.Conceptos_
                    .AgregaConcepto(c.Cantidad, c.Unidad, c.Descripcion, c.ValorUnitario, c.Importe, c.Id, Nothing, IIf(c.CuentaPredial = Nothing, "", c.CuentaPredial))
                Next
            End With

            'Variables donde guardaremos la Ruta de los Archivos .cer y .key asi como la Contraseña de la Clave Privada
            'Dim path As String = HttpContext.Current.Server.MapPath("~/")
            Dim uti As utilerias = New utilerias()
            Dim KeyPass As String = uti.Decrypt(datosFiel(0)("KeyPass"))
            Dim anio As String = FechaHora.Year.ToString()
            Dim mes As String = FechaHora.Month.ToString()
            Dim dia As String = FechaHora.Day.ToString()

            Dim pathXML As String = path + "/Recibos/" + anio + "/" + mes + "/" + dia + "/"
            If Not Directory.Exists(pathXML) Then
                Directory.CreateDirectory(pathXML)
            End If

            'crea xml ------------------------------------------------------------
            Dim xDoc As XDocument = CFDs.CreaFacturaXML(pathXML, datosFiel(0)("KeyFile"), KeyPass, datosFiel(0)("CerFile"), Errores, datos.Comprobante_.Folio)


            Dim rdn As Integer = CInt(Int((100 * Rnd()) + 1))
            'objeto que se regresa en el web service generaPreFactura-------------------------
            valorRecibo.Ruta = IO.Path.Combine("Recibos/" + anio + "/" + mes + "/" + dia + "/", datos.Comprobante_.Folio & ".xml")
            valorRecibo.RND = rdn
            valorRecibo.serie = datos.Comprobante_.Serie
            valorRecibo.Errores = Errores
            valorRecibo.folio = datos.Comprobante_.Folio
            valorRecibo.xml = "<?xml version=""1.0"" encoding=""utf-8""?>" + vbCrLf + xDoc.ToString
            valorRecibo.codigoReimpresion = uti.GetSHA1(datos.Comprobante_.Folio & rdn.ToString)

            'genera pdf
            generaPDFrecibo(xDoc, datos, valorRecibo, FechaHora, valorRecibo.codigoReimpresion, datosFiel, path, Errores, "P")

            If Errores = "" Then
                ClearMemory()
                Return valorRecibo
            Else ' si hubo errores al generar el xml y poner sellos y certificado, o al generar el pdf

                Dim path_Name_File As String = HttpContext.Current.Server.MapPath("~/") + "/Log/" & String.Format("{0:yyyy_MM_dd}", Now) & ".txt"
                Dim valorFacturaErrorXML As Recibo = New Recibo()
                valorRecibo.Errores = "Errores Recuperados al generar el archivo XML: " & Errores
                Dim file As New System.IO.StreamWriter(path_Name_File, True)
                file.WriteLine("**************************")
                file.WriteLine("Fecha Hora del Error: " & Now.ToString)
                file.WriteLine("Errores Recuperados al generar el archivo XML: " & Errores)
                file.WriteLine("**************************")
                file.Close()
                ClearMemory()
                Return valorFacturaErrorXML
            End If
        Catch ex As Exception
            Dim path_Name_File As String = HttpContext.Current.Server.MapPath("~/") + "/Log/" & String.Format("{0:yyyy_MM_dd}", Now) & ".txt"

            Dim valorRecibo As Recibo = New Recibo()
            valorRecibo.Errores = ex.ToString & IIf(Errores = "", "", " *** Errores Recuperados: " & Errores)

            Dim file As New System.IO.StreamWriter(path_Name_File, True)
            file.WriteLine("**************************")
            file.WriteLine("Fecha Hora del Error: " & Now.ToString)
            file.WriteLine(ex.ToString & IIf(Errores = "", "", " Errores Recuperados" & Errores))
            file.WriteLine()
            file.WriteLine("**************************")
            file.Close()
            ClearMemory()
            Return valorRecibo
        End Try

    End Function

    Public Function generaFacturaRecibo(rfc As String, IdRecibo As Integer, usuarioFactura As String, passwordFactura As String, productivo As Boolean) As Factura
        Dim path As String = ConfigurationManager.AppSettings("RutaRecibos") ' HttpContext.Current.Server.MapPath("~/")
        Dim fact As Factura = New Factura()
        Dim fecha As Date = DateTime.Now()
        Dim datosFiel As DataTable = New DAL().cFIELGetByActive()
        Dim xDoc As XDocument
        Dim Errores As String = String.Empty
        Dim recibo As DataTable = New DAL().tReciboGetByConstraint(IdRecibo)
        Dim receptor As DataTable = New DAL().cRfGetByRfc(rfc)
        Dim dfactura As DatosRecibo = New DatosRecibo()
        Dim fechaRecibo As Date = Convert.ToDateTime(recibo(0)("FechaPago"))
        xDoc = XDocument.Load(path + recibo(0)("Ruta"))

        Try

            Dim df As XNamespace = xDoc.Root.Name.NamespaceName

            Dim elementComprobante = xDoc.Elements(df + "Comprobante").Single

            'INICIA cambia el metodo de pago de descripcion a clave
            Select Case elementComprobante.Attribute("metodoDePago").Value
                Case "EFECTIVO"
                    elementComprobante.Attribute("metodoDePago").Value = "01"
                Case "CHEQUE NOMINATIVO"
                    elementComprobante.Attribute("metodoDePago").Value = "02"
                Case "TRANSFERENCIA ELECTRONICA DE FONDOS"
                    elementComprobante.Attribute("metodoDePago").Value = "03"
                Case "TARJETA DE CREDITO"
                    elementComprobante.Attribute("metodoDePago").Value = "04"
                Case "MONEDERO ELECTRONICO"
                    elementComprobante.Attribute("metodoDePago").Value = "05"
                Case "DINERO ELECTRONICO"
                    elementComprobante.Attribute("metodoDePago").Value = "06"
                Case "VALES DE DESPENSA"
                    elementComprobante.Attribute("metodoDePago").Value = "08"
                Case "TARJETA DE DEBITO"
                    elementComprobante.Attribute("metodoDePago").Value = "28"
                Case "TARJETA DE SERVICIO"
                    elementComprobante.Attribute("metodoDePago").Value = "29"
                Case "OTROS"
                    elementComprobante.Attribute("metodoDePago").Value = "99"
                    'nombres internos********************************************
                Case "CHEQUE"
                    elementComprobante.Attribute("metodoDePago").Value = "02"
                Case "TARJETA"
                    elementComprobante.Attribute("metodoDePago").Value = "28"
                Case "VÍA TRANSFERENCIA ELECTRÓNICA"
                    elementComprobante.Attribute("metodoDePago").Value = "03"
                Case Else
                    elementComprobante.Attribute("metodoDePago").Value = "99"
            End Select

            fact.folio = elementComprobante.Attribute("folio").Value

            Dim elementReceptor = elementComprobante.Elements(df + "Receptor").Single
            elementReceptor.Attribute("rfc").Value = receptor(0)("RFC")

            ''valida rfc generico
            If (receptor(0)("RFC") = "XAXX010101000") Then
                elementReceptor.Attribute("nombre").Value = recibo(0)("Contribuyente")
            Else
                elementReceptor.Attribute("nombre").Value = receptor(0)("Nombre")
            End If


            ''valida rfc generico domicilio
            Dim elementRecDom = elementReceptor.Elements(df + "Domicilio").Single
            If (receptor(0)("RFC") = "XAXX010101000") Then
                If elementRecDom.Attribute("calle") Is Nothing Then
                    Dim atribute As XAttribute = New XAttribute("calle", recibo(0)("Domicilio"))
                    elementRecDom.Add(atribute)
                Else
                    elementRecDom.Attribute("calle").Value = recibo(0)("Domicilio")
                End If
                If elementRecDom.Attribute("noExterior") Is Nothing Then
                    Dim atribute As XAttribute = New XAttribute("noExterior", " ")
                    elementRecDom.Add(atribute)
                Else
                    elementRecDom.Attribute("noExterior").Value = " "
                End If
                If elementRecDom.Attribute("municipio") Is Nothing Then
                    Dim atribute As XAttribute = New XAttribute("municipio", receptor(0)("Municipio"))
                    elementRecDom.Add(atribute)
                Else
                    elementRecDom.Attribute("municipio").Value = receptor(0)("Municipio")
                End If
                If elementRecDom.Attribute("colonia") Is Nothing Then
                    Dim atribute As XAttribute = New XAttribute("colonia", " ")
                    elementRecDom.Add(atribute)
                Else
                    elementRecDom.Attribute("colonia").Value = " "
                End If
                If elementRecDom.Attribute("estado") Is Nothing Then
                    Dim atribute As XAttribute = New XAttribute("estado", receptor(0)("Estado"))
                    elementRecDom.Add(atribute)
                Else
                    elementRecDom.Attribute("estado").Value = receptor(0)("Estado")
                End If
                If elementRecDom.Attribute("pais") Is Nothing Then
                    Dim atribute As XAttribute = New XAttribute("pais", receptor(0)("Pais"))
                    elementRecDom.Add(atribute)
                Else
                    elementRecDom.Attribute("pais").Value = receptor(0)("Pais")
                End If
                If elementRecDom.Attribute("codigoPostal") Is Nothing Then
                    Dim atribute As XAttribute = New XAttribute("codigoPostal", " ")
                    elementRecDom.Add(atribute)
                Else
                    elementRecDom.Attribute("codigoPostal").Value = " "
                End If
              
                If elementRecDom.Attribute("referencia") Is Nothing Then
                    Dim atribute As XAttribute = New XAttribute("referencia", " ")
                    elementRecDom.Add(atribute)
                Else
                    elementRecDom.Attribute("referencia").Value = " "
                End If
                If elementRecDom.Attribute("localidad") Is Nothing Then
                    Dim atribute As XAttribute = New XAttribute("localidad", receptor(0)("Localidad"))
                    elementRecDom.Add(atribute)
                Else
                    elementRecDom.Attribute("localidad").Value = receptor(0)("Localidad")
                End If
                If elementRecDom.Attribute("noInterior") Is Nothing Then
                    Dim atribute As XAttribute = New XAttribute("noInterior", " ")
                    elementRecDom.Add(atribute)
                Else
                    elementRecDom.Attribute("noInterior").Value = " "
                End If
            Else
                If elementRecDom.Attribute("calle") Is Nothing Then
                    Dim atribute As XAttribute = New XAttribute("calle", receptor(0)("Calle"))
                    elementRecDom.Add(atribute)
                Else
                    elementRecDom.Attribute("calle").Value = receptor(0)("Calle")
                End If
                If elementRecDom.Attribute("noExterior") Is Nothing Then
                    Dim atribute As XAttribute = New XAttribute("noExterior", receptor(0)("NoExterior"))
                    elementRecDom.Add(atribute)
                Else
                    elementRecDom.Attribute("noExterior").Value = receptor(0)("NoExterior")
                End If
                If elementRecDom.Attribute("municipio") Is Nothing Then
                    Dim atribute As XAttribute = New XAttribute("municipio", receptor(0)("Municipio"))
                    elementRecDom.Add(atribute)
                Else
                    elementRecDom.Attribute("municipio").Value = receptor(0)("Municipio")
                End If
                If elementRecDom.Attribute("colonia") Is Nothing Then
                    Dim atribute As XAttribute = New XAttribute("colonia", receptor(0)("Colonia"))
                    elementRecDom.Add(atribute)
                Else
                    elementRecDom.Attribute("colonia").Value = receptor(0)("Colonia")
                End If
                If elementRecDom.Attribute("estado") Is Nothing Then
                    Dim atribute As XAttribute = New XAttribute("estado", receptor(0)("Estado"))
                    elementRecDom.Add(atribute)
                Else
                    elementRecDom.Attribute("estado").Value = receptor(0)("Estado")
                End If
                If elementRecDom.Attribute("pais") Is Nothing Then
                    Dim atribute As XAttribute = New XAttribute("pais", receptor(0)("Pais"))
                    elementRecDom.Add(atribute)
                Else
                    elementRecDom.Attribute("pais").Value = receptor(0)("Pais")
                End If
                If elementRecDom.Attribute("codigoPostal") Is Nothing Then
                    Dim atribute As XAttribute = New XAttribute("codigoPostal", receptor(0)("CodigoPostal"))
                    elementRecDom.Add(atribute)
                Else
                    elementRecDom.Attribute("codigoPostal").Value = receptor(0)("CodigoPostal")
                End If

                If elementRecDom.Attribute("referencia") Is Nothing Then
                    Dim atribute As XAttribute = New XAttribute("referencia", receptor(0)("Referencia"))
                    elementRecDom.Add(atribute)
                Else
                    elementRecDom.Attribute("referencia").Value = receptor(0)("Referencia")
                End If
                If elementRecDom.Attribute("localidad") Is Nothing Then
                    Dim atribute As XAttribute = New XAttribute("localidad", receptor(0)("Localidad"))
                    elementRecDom.Add(atribute)
                Else
                    elementRecDom.Attribute("localidad").Value = receptor(0)("Localidad")
                End If
                If elementRecDom.Attribute("noInterior") Is Nothing Then
                    Dim atribute As XAttribute = New XAttribute("noInterior", receptor(0)("NoInterior"))
                    elementRecDom.Add(atribute)
                Else
                    elementRecDom.Attribute("noInterior").Value = receptor(0)("NoInterior")
                End If
            End If
          

            'se vuelve a sellar el archivo debido a que 
            Dim CFDs As New CFDx

            'ACTUALIZA FECHA PARA TIMBRAR
            Dim FechaHora As DateTime = Now.AddSeconds(-120)
            elementComprobante.Attribute("fecha").Value = String.Format("{0:s}", FechaHora)
            'SE REMUEVEN ESPACIOS Y CARACTERES DEN TODO EL XML ANTES DE TIMBRARLO
            Dim utileria As utilerias = New utilerias()
            xDoc = utileria.remueveEspacios(df, xDoc)
            xDoc = utileria.remueveCaracteres(xDoc)
            Dim uti As utilerias = New utilerias()
            Dim KeyPass As String = uti.Decrypt(datosFiel(0)("KeyPass"))

            'SELLA EL RECIBO CON LA FECHA ACTUALIZADA
            CFDs.SellarXML(datosFiel(0)("KeyFile"), KeyPass, datosFiel(0)("CerFile"), xDoc, Errores)
            'SE REMUEVEN CARACTERES DEN TODO EL XML ANTES DE TIMBRARLO
            xDoc = utileria.remueveCaracteres(xDoc)

            Dim strXMLTimbrado As String = String.Empty
            'se timbra el xml
            If Errores = "" Then
                strXMLTimbrado = timbraFactura(xDoc.ToString, Errores, recibo(0)("Id"), productivo, usuarioFactura, passwordFactura)
            End If

            If Errores = "" Then
                xDoc = XDocument.Parse(strXMLTimbrado)
                Dim anio As String = FechaHora.Year.ToString()
                Dim mes As String = FechaHora.Month.ToString()
                Dim dia As String = FechaHora.Day.ToString()
                Dim pathXML As String = path + "/Recibos/Factura/" + anio + "/" + mes + "/" + dia + "/"
                If Not Directory.Exists(pathXML) Then
                    Directory.CreateDirectory(pathXML)
                End If
                xDoc = XDocument.Parse(strXMLTimbrado)
                xDoc.Save(pathXML + recibo(0)("Id").ToString() + ".xml")
                fact.Rfc = receptor(0)("RFC")
                fact.Ruta = "Recibos/Factura/" + anio + "/" + mes + "/" + dia + "/" + recibo(0)("Id").ToString() + ".xml"
                fact.FechaFactura = FechaHora

                'Comprobante
                Dim comprobante As Comprobante = New Comprobante()
                comprobante.TipoDeComprobante = ComprobanteTipoDeComprobante.ingreso
                comprobante.Serie = elementComprobante.Attribute("serie").Value
                comprobante.SubTotal = elementComprobante.Attribute("subTotal").Value
                comprobante.Total = elementComprobante.Attribute("total").Value
                comprobante.FormaDePago = elementComprobante.Attribute("formaDePago").Value
                comprobante.TipoDeComprobante = elementComprobante.Attribute("tipoDeComprobante").Value
                comprobante.MetodoDePago = elementComprobante.Attribute("metodoDePago").Value
                comprobante.Mesa = recibo(0)("Mesa")
                comprobante.cajero = recibo(0)("Cajero")
                comprobante.DatosPredio = recibo(0)("DatosPredio")
                comprobante.Observaciones = recibo(0)("Observaciones")
                dfactura.Comprobante_ = comprobante

                Dim elementConceptos = elementComprobante.Elements(df + "Conceptos").Single.Elements()

                Dim conceptos As New List(Of Conceptos)
                For Each var In elementConceptos
                    Dim cuentaPredial = var.Elements(df + "CuentaPredial").SingleOrDefault
                    Dim noIdentificacion As String = ""
                    Dim Importe As Double = 0

                    If cuentaPredial Is Nothing Then 'si cuenta predial esta vacia entra
                        If Not var.Attribute("noIdentificacion") Is Nothing Then
                            noIdentificacion = var.Attribute("noIdentificacion").Value
                        End If
                        If Not var.Attribute("Importe") Is Nothing Then
                            Importe = var.Attribute("Importe").Value
                        End If
                        conceptos.Add(New Conceptos(
                                   var.Attribute("cantidad").Value,
                                   var.Attribute("unidad").Value,
                                   var.Attribute("descripcion").Value,
                                   var.Attribute("valorUnitario").Value,
                                   noIdentificacion,
                                   Importe,
                                   ""))
                    Else 'solo aplica para predial
                        If Not var.Attribute("noIdentificacion") Is Nothing Then
                            noIdentificacion = var.Attribute("noIdentificacion").Value
                        End If
                        If Not var.Attribute("Importe") Is Nothing Then
                            Importe = var.Attribute("Importe").Value
                        End If
                        conceptos.Add(New Conceptos(
                                    var.Attribute("cantidad").Value,
                                    var.Attribute("unidad").Value,
                                    var.Attribute("descripcion").Value,
                                    var.Attribute("valorUnitario").Value,
                                    noIdentificacion,
                                    Importe,
                                    cuentaPredial.Attribute("numero").Value))

                    End If
                Next
                dfactura.Conceptos_ = conceptos
                Dim R As New Receptor()
                R.Nombre = receptor(0)("Nombre")
                R.RFC = receptor(0)("RFC")
                R.Calle = receptor(0)("calle")
                R.NoExterior = receptor(0)("NoExterior")
                R.NoInterior = receptor(0)("NoInterior")
                R.Colonia = receptor(0)("Colonia")
                R.Municipio = receptor(0)("Municipio")
                R.Estado = receptor(0)("Estado")
                R.Pais = receptor(0)("Pais")
                R.CodigoPostal = receptor(0)("CodigoPostal")
                dfactura.Receptor_ = R
                generaPDFfactura(xDoc, dfactura, fact, recibo(0)("CodigoSeguridad"), datosFiel)
                fact.xml = "<?xml version=""1.0"" encoding=""utf-8""?>" + vbCrLf + xDoc.ToString
            Else
                fact.Errores = Errores
                Dim path_Name_File As String = HttpContext.Current.Server.MapPath("~/Log/" & String.Format("{0:yyyy_MM_dd}", Now) & ".txt")
                Dim file As New System.IO.StreamWriter(path_Name_File, True)
                file.WriteLine("**************************")
                file.WriteLine("Fecha Hora del Error: " & Now.ToString)
                file.WriteLine(Errores)
                file.WriteLine("**************************")
                file.Close()
            End If

        Catch ex As Exception
            Dim path_Name_File As String = HttpContext.Current.Server.MapPath("~/Log/" & String.Format("{0:yyyy_MM_dd}", Now) & ".txt")
            Dim file As New System.IO.StreamWriter(path_Name_File, True)
            file.WriteLine("**************************")
            file.WriteLine("Fecha Hora del Error: " & Now.ToString)
            file.WriteLine(ex.ToString)
            file.WriteLine("**************************")
            file.Close()
            fact.Errores = ex.ToString
        End Try

        Return fact
    End Function

    Public Function consultaRecibo(recibo As Object, IdFiel As Integer, path As String) As Recibo
        Dim Errores As String
        path = ConfigurationManager.AppSettings("RutaRecibos")
        Dim xDoc As XDocument = XDocument.Load(path + recibo.Ruta)

        Dim valorRecibo As Recibo = New Recibo()

        Dim rpt As ReportViewer = New ReportViewer
        rpt.Height = 800
        rpt.Width = 500

        Try
            Dim datosFiel As DataTable = New DAL().cFIELGetByConstraint(IdFiel)

            'obtiene valores del xml
            Dim df As XNamespace = xDoc.Root.Name.[Namespace]

            Dim xeComprobante As XElement = xDoc.Descendants(df + "Comprobante").FirstOrDefault
            Dim xeEmisor As XElement = xDoc.Descendants(df + "Emisor").FirstOrDefault
            Dim xeDomicilioFiscal As XElement = xeEmisor.Descendants(df + "DomicilioFiscal").FirstOrDefault
            Dim xeReceptor As XElement = xDoc.Descendants(df + "Receptor").FirstOrDefault
            Dim xeReceptorDomicilio As XElement = xeReceptor.Descendants(df + "Domicilio").FirstOrDefault
            Dim xeConceptos As XElement = xDoc.Descendants(df + "Conceptos").FirstOrDefault

            Dim sello As String = xeComprobante.Attribute("sello").Value
            Dim noCertificado As String = xeComprobante.Attribute("noCertificado").Value

            'Data table del ConfGral
            Dim ConfGral As DataTable = New DataTable("ConfGral")
            ConfGral.Columns.Add(New DataColumn("Nombre"))
            ConfGral.Columns.Add(New DataColumn("Calle"))
            ConfGral.Columns.Add(New DataColumn("Colonia"))
            ConfGral.Columns.Add("Logo", GetType([Byte]()))
            ConfGral.Columns.Add(New DataColumn("Mesa"))
            ConfGral.Columns.Add(New DataColumn("Cajero"))
            ConfGral.Columns.Add(New DataColumn("RFC"))
            ConfGral.Columns.Add(New DataColumn("Regimen"))
            ConfGral.Columns.Add(New DataColumn("Fondo", GetType([Byte]())))

            Dim rowGral As DataRow = ConfGral.NewRow()
            'Dim myAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
            'Dim myStream As Stream = myAssembly.GetManifestResourceStream("cancelado.png")
            'Dim image As New Drawing.Bitmap(myStream)
            Dim rootPath As String = HttpContext.Current.Server.MapPath("~")

            rowGral("Nombre") = xeEmisor.Attribute("nombre").Value
            rowGral("Calle") = xeDomicilioFiscal.Attribute("calle").Value
            rowGral("Colonia") = datosFiel(0)("Colonia")
            rowGral("Logo") = datosFiel(0)("Logo")
            rowGral("Mesa") = recibo.cMesa.Nombre
            rowGral("Cajero") = recibo.cUsuarios.Usuario
            rowGral("RFC") = xeEmisor.Attribute("rfc").Value
            rowGral("Regimen") = "PERSONAS MORALES CON FINES NO LUCRATIVOS"
            If recibo.EstadoRecibo = "C" Then
                'rowGral("Fondo") = myAssembly.GetManifestResourceStream("cancelado.png")
                rowGral("Fondo") = File.ReadAllBytes(rootPath + "/img/cancelado.png")
            End If
            ConfGral.Rows.Add(rowGral)


            'Data table del comprobante
            Dim ComprobanteDTS As DataTable = New DataTable("ComprobanteDTS")
            ComprobanteDTS.Columns.Add(New DataColumn("Serie"))
            ComprobanteDTS.Columns.Add(New DataColumn("Folio"))
            ComprobanteDTS.Columns.Add(New DataColumn("FormaDePago"))
            ComprobanteDTS.Columns.Add(New DataColumn("SubTotal"))
            ComprobanteDTS.Columns.Add(New DataColumn("Total"))
            ComprobanteDTS.Columns.Add(New DataColumn("TipoDeComprobante"))
            ComprobanteDTS.Columns.Add(New DataColumn("MetodoDePago"))
            ComprobanteDTS.Columns.Add(New DataColumn("Fecha"))
            ComprobanteDTS.Columns.Add(New DataColumn("Hora"))
            ComprobanteDTS.Columns.Add(New DataColumn("TotalLetra"))
            ComprobanteDTS.Columns.Add(New DataColumn("sello"))
            ComprobanteDTS.Columns.Add(New DataColumn("noCertificado"))
            ComprobanteDTS.Columns.Add(New DataColumn("certificado"))
            ComprobanteDTS.Columns.Add(New DataColumn("codigoReimpresion"))
            ComprobanteDTS.Columns.Add(New DataColumn("cajero"))
            ComprobanteDTS.Columns.Add(New DataColumn("cveMesa"))
            ComprobanteDTS.Columns.Add(New DataColumn("descuento"))
            ComprobanteDTS.Columns.Add(New DataColumn("motivoDescuento"))
            ComprobanteDTS.Columns.Add(New DataColumn("DatosPredio"))
            ComprobanteDTS.Columns.Add(New DataColumn("Observaciones"))

            Dim row As DataRow = ComprobanteDTS.NewRow()
            row("Serie") = xeComprobante.Attribute("serie").Value
            row("Folio") = xeComprobante.Attribute("folio").Value
            row("FormaDePago") = xeComprobante.Attribute("formaDePago").Value
            row("SubTotal") = xeComprobante.Attribute("subTotal").Value
            row("Total") = xeComprobante.Attribute("total").Value
            row("TipoDeComprobante") = xeComprobante.Attribute("tipoDeComprobante").Value
            row("MetodoDePago") = xeComprobante.Attribute("metodoDePago").Value
            row("Fecha") = Convert.ToDateTime(xeComprobante.Attribute("fecha").Value).ToString("dd/MM/yyyy hh:mm:ss")

            row("TotalLetra") = Letras(xeComprobante.Attribute("total").Value).ToUpper
            row("sello") = sello.ToString
            row("noCertificado") = noCertificado
            row("certificado") = ""
            row("codigoReimpresion") = recibo.CodigoSeguridad
            row("cajero") = recibo.cUsuarios.Usuario
            row("cveMesa") = recibo.cMesa.Nombre
            row("descuento") = recibo.ImporteDescuento
            row("motivoDescuento") = "" 'datos.Comprobante_.motivoDescuento
            row("DatosPredio") = recibo.DatosPredio
            row("Observaciones") = recibo.Observaciones
            ComprobanteDTS.Rows.Add(row)

            'Data table del receptor
            Dim ReceptorDTS As DataTable = New DataTable("ReceptorDTS")
            ReceptorDTS.Columns.Add(New DataColumn("Nombre"))
            ReceptorDTS.Columns.Add(New DataColumn("RFC"))
            ReceptorDTS.Columns.Add(New DataColumn("Domicilio"))

            Dim rowReceptor As DataRow = ReceptorDTS.NewRow()
            rowReceptor("Nombre") = xeReceptor.Attribute("nombre").Value
            rowReceptor("RFC") = xeReceptor.Attribute("rfc").Value
            rowReceptor("Domicilio") = xeReceptorDomicilio.Attribute("calle").Value & " No. Ext." & xeReceptorDomicilio.Attribute("noExterior").Value &
                                      " Col. " & xeReceptorDomicilio.Attribute("colonia").Value & ", " & xeReceptorDomicilio.Attribute("municipio").Value &
                                      " " & xeReceptorDomicilio.Attribute("estado").Value & ", " & xeReceptorDomicilio.Attribute("pais").Value &
                                      " Cp." & xeReceptorDomicilio.Attribute("codigoPostal").Value
            ReceptorDTS.Rows.Add(rowReceptor)

            'Data table del Conceptos
            Dim ConceptosDTS As DataTable = New DataTable("ConceptosDTS")
            ConceptosDTS.Columns.Add(New DataColumn("Cantidad"))
            ConceptosDTS.Columns.Add(New DataColumn("Unidad"))
            ConceptosDTS.Columns.Add(New DataColumn("Descripcion"))
            ConceptosDTS.Columns.Add(New DataColumn("ValorUnitario"))
            ConceptosDTS.Columns.Add(New DataColumn("CuentaPredial"))
            ConceptosDTS.Columns.Add(New DataColumn("id"))
            Dim formatoPredial As Boolean = False

            For Each c As XElement In xeConceptos.Descendants(df + "Concepto")
                Dim rowConceptos As DataRow = ConceptosDTS.NewRow()
                rowConceptos("Cantidad") = c.Attribute("cantidad").Value ' Cantidad
                rowConceptos("Unidad") = c.Attribute("unidad").Value ' c.Unidad
                rowConceptos("Descripcion") = c.Attribute("descripcion").Value ' c.Descripcion
                rowConceptos("ValorUnitario") = c.Attribute("valorUnitario").Value ' c.ValorUnitario
                Dim xepredial As XElement = xeConceptos.Descendants(df + "CuentaPredial").FirstOrDefault
                'Dim predial As XAttribute = c.Attribute("CuentaPredial")
                If xepredial Is Nothing Then
                    rowConceptos("CuentaPredial") = ""
                Else
                    rowConceptos("CuentaPredial") = xepredial.Attribute("numero").Value
                    formatoPredial = True
                End If
                rowConceptos("id") = c.Attribute("noIdentificacion").Value 'c.Id
                ConceptosDTS.Rows.Add(rowConceptos)
            Next


            rpt.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local
            rpt.LocalReport.DisplayName = "Recibo"
            If recibo.EstadoRecibo = "P" Then
                rpt.LocalReport.ReportPath = "Reportes/ReciboPredialRpt.rdlc"
            Else
                rpt.LocalReport.ReportPath = "Reportes/cReciboPredialRpt.rdlc"
            End If


            'If formatoPredial Then
            '    rpt.LocalReport.ReportPath = "Reportes/ReciboPredialRpt.rdlc"
            'Else
            '    rpt.LocalReport.ReportPath = "Reportes/ReciboRpt.rdlc"
            'End If
            'Select Case datos.Comprobante_.t_FormatoRecibo
            '    Case "R" 'recibo simple
            '        rpt.LocalReport.ReportPath = "Reporte/ReciboRpt.rdlc"
            '    Case "P" ' recibo predial
            '        rpt.LocalReport.ReportPath = "Reporte/ReciboPredialRpt.rdlc"
            '    Case Else 'recibo simple
            '        rpt.LocalReport.ReportPath = "Reporte/ReciboRpt.rdlc"
            'End Select


            rpt.LocalReport.DataSources.Clear()
            rpt.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("ConfGral", ConfGral))
            rpt.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("ComprobanteDTS", ComprobanteDTS))
            rpt.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("ReceptorDTS", ReceptorDTS))
            rpt.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("ConceptosDTS", ConceptosDTS))

            rpt.LocalReport.Refresh()

            Dim warnings As Warning()
            Dim streamids As String()
            Dim mimeType As String
            Dim encoding As String
            Dim filenameExtension As String

            Dim bytes As Byte() = rpt.LocalReport.Render("PDF", Nothing, mimeType, encoding, filenameExtension, streamids, warnings)

            valorRecibo.pdfBytes = bytes
            valorRecibo.mimeType = mimeType
            valorRecibo.filenameExtension = filenameExtension
        Catch ex As Exception
            Errores = ex.ToString
            Dim path_Name_File As String = HttpContext.Current.Server.MapPath("~/") + "/Log/" & String.Format("{0:yyyy_MM_dd}", Now) & ".txt"
            Dim file As New System.IO.StreamWriter(path_Name_File, True)
            file.WriteLine("**************************")
            file.WriteLine("Fecha Hora del Error: " & Now.ToString)
            file.WriteLine(ex.ToString)
            file.WriteLine("**************************")
            file.Close()
        End Try
        Return valorRecibo
    End Function

    Public Function consultaFacturaPDF(recibo As Object, IdFiel As Integer, path As String) As Recibo
        Dim Errores As String
        path = ConfigurationManager.AppSettings("RutaRecibos")
        Dim xDoc As XDocument = XDocument.Load(path + recibo.RutaFactura)

        Dim valorRecibo As Recibo = New Recibo()

        Dim rpt As ReportViewer = New ReportViewer
        rpt.Height = 800
        rpt.Width = 500

        Try
            Dim datosFiel As DataTable = New DAL().cFIELGetByConstraint(IdFiel)

            'obtiene valores del xml
            Dim df As XNamespace = xDoc.Root.Name.[Namespace]

            Dim xeComprobante As XElement = xDoc.Descendants(df + "Comprobante").FirstOrDefault
            Dim xeEmisor As XElement = xDoc.Descendants(df + "Emisor").FirstOrDefault
            Dim xeDomicilioFiscal As XElement = xeEmisor.Descendants(df + "DomicilioFiscal").FirstOrDefault
            Dim xeReceptor As XElement = xDoc.Descendants(df + "Receptor").FirstOrDefault
            Dim xeReceptorDomicilio As XElement = xeReceptor.Descendants(df + "Domicilio").FirstOrDefault
            Dim xeConceptos As XElement = xDoc.Descendants(df + "Conceptos").FirstOrDefault

            'Data table del ConfGral
            Dim ConfGral As DataTable = New DataTable("ConfGral")
            ConfGral.Columns.Add(New DataColumn("NombreMunicipio"))
            ConfGral.Columns.Add(New DataColumn("Calle"))
            ConfGral.Columns.Add(New DataColumn("Colonia"))
            ConfGral.Columns.Add("Logo", GetType([Byte]()))
            ConfGral.Columns.Add(New DataColumn("Mesa"))
            ConfGral.Columns.Add(New DataColumn("Cajero"))
            ConfGral.Columns.Add(New DataColumn("RFC"))
            ConfGral.Columns.Add(New DataColumn("Regimen"))
            ConfGral.Columns.Add(New DataColumn("Fondo", GetType([Byte]())))

            Dim rowGral As DataRow = ConfGral.NewRow()
            Dim rootPath As String = HttpContext.Current.Server.MapPath("~")

            rowGral("NombreMunicipio") = xeEmisor.Attribute("nombre").Value
            rowGral("Calle") = xeDomicilioFiscal.Attribute("calle").Value
            rowGral("Colonia") = datosFiel(0)("Colonia")
            rowGral("Logo") = datosFiel(0)("Logo")
            rowGral("Mesa") = recibo.cMesa.Nombre
            rowGral("Cajero") = recibo.cUsuarios.Usuario
            rowGral("RFC") = xeEmisor.Attribute("rfc").Value
            rowGral("Regimen") = "PERSONAS MORALES CON FINES NO LUCRATIVOS"
            If recibo.EstadoRecibo = "C" Then
                'rowGral("Fondo") = myAssembly.GetManifestResourceStream("cancelado.png")
                rowGral("Fondo") = File.ReadAllBytes(rootPath + "/img/cancelado.png")
            End If
            ConfGral.Rows.Add(rowGral)


            'Data table del comprobante
            Dim ComprobanteDTS As DataTable = New DataTable("ComprobanteDTS")
            ComprobanteDTS.Columns.Add(New DataColumn("Serie"))
            ComprobanteDTS.Columns.Add(New DataColumn("Folio"))
            ComprobanteDTS.Columns.Add(New DataColumn("FormaDePago"))
            ComprobanteDTS.Columns.Add(New DataColumn("SubTotal"))
            ComprobanteDTS.Columns.Add(New DataColumn("Total"))
            ComprobanteDTS.Columns.Add(New DataColumn("TipoDeComprobante"))
            ComprobanteDTS.Columns.Add(New DataColumn("MetodoDePago"))
            ComprobanteDTS.Columns.Add(New DataColumn("Fecha"))
            ComprobanteDTS.Columns.Add(New DataColumn("Hora"))
            ComprobanteDTS.Columns.Add(New DataColumn("TotalLetra"))
            ComprobanteDTS.Columns.Add(New DataColumn("sello"))
            ComprobanteDTS.Columns.Add(New DataColumn("noCertificado"))
            ComprobanteDTS.Columns.Add(New DataColumn("certificado"))
            ComprobanteDTS.Columns.Add(New DataColumn("codigoReimpresion"))
            ComprobanteDTS.Columns.Add(New DataColumn("cajero"))
            ComprobanteDTS.Columns.Add(New DataColumn("cveMesa"))
            ComprobanteDTS.Columns.Add(New DataColumn("descuento"))
            ComprobanteDTS.Columns.Add(New DataColumn("motivoDescuento"))
            ComprobanteDTS.Columns.Add(New DataColumn("DatosPredio"))
            ComprobanteDTS.Columns.Add(New DataColumn("Observaciones"))
            Dim qr As DataColumn = New DataColumn
            qr.DataType = System.Type.GetType("System.Byte[]")
            qr.Caption = "qr"
            qr.ColumnName = "qr"
            ComprobanteDTS.Columns.Add(qr)
            'ComprobanteDTS.Columns.Add(New DataColumn("qr"))

            Dim row As DataRow = ComprobanteDTS.NewRow()
            Dim metodoDePago As String = xeComprobante.Attribute("metodoDePago").Value
            ''INICIA cambia el metodo de pago DE CLAVE A DESCRIPCION
            Select Case metodoDePago
                Case "01"
                    row("MetodoDePago") = "01-EFECTIVO"
                Case "02"
                    row("MetodoDePago") = "02-CHEQUE NOMINATIVO"
                Case "03"
                    row("MetodoDePago") = "03-TRANSFERENCIA ELECTRÓNICA DE FONDOS"
                Case "04"
                    row("MetodoDePago") = "04-TARJETA DE CRÉDITO"
                Case "05"
                    row("MetodoDePago") = "05-MONEDERO ELECTRÓNICO"
                Case "06"
                    row("MetodoDePago") = "06-DINERO ELECTRÓNICO"
                Case "08"
                    row("MetodoDePago") = "08-VALES DE DESPENSA"
                Case "28"
                    row("MetodoDePago") = "28-TARJETA DE DÉBITO"
                Case "29"
                    row("MetodoDePago") = "29-TARJETA DE SERVICIO"
                Case "99"
                    row("MetodoDePago") = "99-OTROS"
                Case Else
                    row("MetodoDePago") = metodoDePago
            End Select

            row("Fecha") = "MÉXICO " + xeComprobante.Attribute("fecha").Value  'FechaHora.ToString("dd/MM/yyyy")
            row("Serie") = xeComprobante.Attribute("serie").Value
            row("Folio") = xeComprobante.Attribute("folio").Value
            row("FormaDePago") = xeComprobante.Attribute("formaDePago").Value
            row("SubTotal") = xeComprobante.Attribute("subTotal").Value
            row("Total") = xeComprobante.Attribute("total").Value
            row("TipoDeComprobante") = xeComprobante.Attribute("tipoDeComprobante").Value

            row("TotalLetra") = Letras(xeComprobante.Attribute("total").Value).ToUpper
            row("sello") = xeComprobante.Attribute("sello").Value
            row("noCertificado") = xeComprobante.Attribute("noCertificado").Value
            row("certificado") = ""
            row("codigoReimpresion") = recibo.CodigoSeguridad
            row("cajero") = recibo.cUsuarios.Usuario
            row("cveMesa") = recibo.cMesa.Nombre
            row("descuento") = recibo.ImporteDescuento
            If Not xeComprobante.Attribute("motivoDescuento") Is Nothing Then
                row("motivoDescuento") = xeComprobante.Attribute("motivoDescuento").Value
            Else
                row("motivoDescuento") = ""
            End If
            row("DatosPredio") = recibo.DatosPredio
            row("Observaciones") = recibo.Observaciones
            ComprobanteDTS.Rows.Add(row)


            'Data table del receptor
            Dim ReceptorDTS As DataTable = New DataTable("ReceptorDTS")
            ReceptorDTS.Columns.Add(New DataColumn("Nombre"))
            ReceptorDTS.Columns.Add(New DataColumn("RFC"))
            ReceptorDTS.Columns.Add(New DataColumn("Domicilio"))

            Dim rowReceptor As DataRow = ReceptorDTS.NewRow()
            rowReceptor("Nombre") = xeReceptor.Attribute("nombre").Value
            rowReceptor("RFC") = xeReceptor.Attribute("rfc").Value

            If xeReceptor.Attribute("rfc").Value = "XAXX010101000" Then
                rowReceptor("Domicilio") = xeReceptorDomicilio.Attribute("calle").Value & ", " & xeReceptorDomicilio.Attribute("municipio").Value &
                                     " " & xeReceptorDomicilio.Attribute("estado").Value & ", " & xeReceptorDomicilio.Attribute("pais").Value 
            Else
                rowReceptor("Domicilio") = xeReceptorDomicilio.Attribute("calle").Value & " No. Ext." & xeReceptorDomicilio.Attribute("noExterior").Value &
                                     " Col. " & xeReceptorDomicilio.Attribute("colonia").Value & ", " & xeReceptorDomicilio.Attribute("municipio").Value &
                                     " " & xeReceptorDomicilio.Attribute("estado").Value & ", " & xeReceptorDomicilio.Attribute("pais").Value &
                                     " Cp." & xeReceptorDomicilio.Attribute("codigoPostal").Value
            End If
           
            ReceptorDTS.Rows.Add(rowReceptor)


            'Data table del FolioFiscalDTS
            Dim FolioFiscalDTS As DataTable = New DataTable("FolioFiscalDTS")
            FolioFiscalDTS.Columns.Add(New DataColumn("FolioFiscal"))
            FolioFiscalDTS.Columns.Add(New DataColumn("NoSerieCertificadoSAT"))
            FolioFiscalDTS.Columns.Add(New DataColumn("SelloSAT"))
            FolioFiscalDTS.Columns.Add(New DataColumn("CadenaOriginalSAT"))
            FolioFiscalDTS.Columns.Add(New DataColumn("FechaEmisionCertificado"))

            Dim elementComprobante = xDoc.Elements(df + "Comprobante").Single
            Dim elementComplemento = elementComprobante.Elements(df + "Complemento").Single
            Dim tfd As XNamespace = "http://www.sat.gob.mx/TimbreFiscalDigital"
            Dim elementTimbreFiscalDigital = elementComplemento.Elements(tfd + "TimbreFiscalDigital").Single

            Dim rowFolioFiscal As DataRow = FolioFiscalDTS.NewRow()
            rowFolioFiscal("FolioFiscal") = elementTimbreFiscalDigital.Attribute("UUID").Value
            rowFolioFiscal("NoSerieCertificadoSAT") = elementTimbreFiscalDigital.Attribute("noCertificadoSAT").Value
            rowFolioFiscal("SelloSAT") = elementTimbreFiscalDigital.Attribute("selloSAT").Value
            rowFolioFiscal("CadenaOriginalSAT") = elementTimbreFiscalDigital.Attribute("selloCFD").Value
            rowFolioFiscal("FechaEmisionCertificado") = elementTimbreFiscalDigital.Attribute("FechaTimbrado").Value
            FolioFiscalDTS.Rows.Add(rowFolioFiscal)

            'genera qr
            Dim rfc As String = xeEmisor.Attribute("rfc").Value
            Dim rfcReceptor As String = xeReceptor.Attribute("rfc").Value
            Dim total As Decimal = Convert.ToDecimal(xeComprobante.Attribute("total").Value)
            Dim uuid As String = elementTimbreFiscalDigital.Attribute("UUID").Value

            Dim qrEncoder As QRCodeEncoder = New QRCodeEncoder
            Dim qrBitmap As Bitmap = qrEncoder.Encode("?re=" + rfc + "&rr=" + rfcReceptor + "&tt=" + total.ToString("C2") + "&id=" + uuid + "")
            Dim qrBytes As Byte()
            Dim stream As New System.IO.MemoryStream
            qrBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png)
            qrBytes = stream.ToArray
            ComprobanteDTS.Rows(0)("qr") = qrBytes

            'Data table del Conceptos
            Dim ConceptosDTS As DataTable = New DataTable("ConceptosDTS")
            ConceptosDTS.Columns.Add(New DataColumn("Cantidad"))
            ConceptosDTS.Columns.Add(New DataColumn("Unidad"))
            ConceptosDTS.Columns.Add(New DataColumn("Descripcion"))
            ConceptosDTS.Columns.Add(New DataColumn("ValorUnitario"))
            ConceptosDTS.Columns.Add(New DataColumn("CuentaPredial"))
            ConceptosDTS.Columns.Add(New DataColumn("id"))
            Dim formatoPredial As Boolean = False

            For Each c As XElement In xeConceptos.Descendants(df + "Concepto")
                Dim rowConceptos As DataRow = ConceptosDTS.NewRow()
                rowConceptos("Cantidad") = c.Attribute("cantidad").Value ' Cantidad
                rowConceptos("Unidad") = c.Attribute("unidad").Value ' c.Unidad
                rowConceptos("Descripcion") = c.Attribute("descripcion").Value ' c.Descripcion
                rowConceptos("ValorUnitario") = c.Attribute("valorUnitario").Value ' c.ValorUnitario
                Dim xepredial As XElement = xeConceptos.Descendants(df + "CuentaPredial").FirstOrDefault
                'Dim predial As XAttribute = c.Attribute("CuentaPredial")
                If xepredial Is Nothing Then
                    rowConceptos("CuentaPredial") = ""
                Else
                    rowConceptos("CuentaPredial") = xepredial.Attribute("numero").Value
                    formatoPredial = True
                End If
                rowConceptos("id") = c.Attribute("noIdentificacion").Value 'c.Id
                ConceptosDTS.Rows.Add(rowConceptos)
            Next


            rpt.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local
            rpt.LocalReport.DisplayName = "Factura"

            ' factura predial
            rpt.LocalReport.ReportPath = "Reportes/FacturaPredialRpt.rdlc"
            rpt.LocalReport.DataSources.Clear()
            rpt.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("ConfGral", ConfGral))
            rpt.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("ComprobanteDTS", ComprobanteDTS))
            rpt.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("ReceptorDTS", ReceptorDTS))
            rpt.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("ConceptosDTS", ConceptosDTS))
            rpt.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("FolioFiscalDTS", FolioFiscalDTS))

            rpt.LocalReport.Refresh()

            Dim warnings As Warning()
            Dim streamids As String()
            Dim mimeType As String
            Dim encoding As String
            Dim filenameExtension As String

            Dim bytes As Byte() = rpt.LocalReport.Render("PDF", Nothing, mimeType, encoding, filenameExtension, streamids, warnings)
            valorRecibo.xml = xDoc.ToString()
            valorRecibo.pdfBytes = bytes
            valorRecibo.mimeType = mimeType
            valorRecibo.filenameExtension = filenameExtension
        Catch ex As Exception
            Errores = ex.ToString
            Dim path_Name_File As String = HttpContext.Current.Server.MapPath("~/") + "/Log/" & String.Format("{0:yyyy_MM_dd}", Now) & ".txt"
            Dim file As New System.IO.StreamWriter(path_Name_File, True)
            file.WriteLine("**************************")
            file.WriteLine("Fecha Hora del Error: " & Now.ToString)
            file.WriteLine(ex.ToString)
            file.WriteLine("**************************")
            file.Close()
        End Try
        Return valorRecibo
    End Function


    Public Function CancelacionFactura(recibo As Object, usuarioCancela As Int32, path As String, usuarioFactura As String, passwordFactura As String) As String
        path = ConfigurationManager.AppSettings("RutaRecibos")
        Dim errores As String = String.Empty
        Dim datosFiel As DataTable = New DAL().cFIELGetByActive()
        Dim uti As utilerias = New utilerias()
        Dim KeyPass As String = uti.Decrypt(datosFiel(0)("KeyPass"))

        Dim xDoc As XDocument = XDocument.Load(path + recibo.RutaFactura)
        Dim df As XNamespace = xDoc.Root.Name.[Namespace]
        Dim complemento = (From x In xDoc.Descendants() Where x.Name = df + "Complemento" Select x).Single
        Dim dfcomplemento As XNamespace = complemento.Elements().SingleOrDefault.Name.[Namespace]
        Dim UUID As String = (From c In complemento.Descendants() Where c.Name = dfcomplemento + "TimbreFiscalDigital" Select c).Single.Attribute("UUID").Value()
        Dim Respuesta() As String = cancelaTimbraFactura(UUID, recibo.Id, errores, datosFiel(0)("CerFile"), datosFiel(0)("KeyFile"), KeyPass, usuarioFactura, passwordFactura, datosFiel(0)("RFC"))

        If Respuesta(0) = String.Empty And Respuesta.Length = 3 Then
            'codigo = Respuesta(1)
            'xmlCancelacion = Respuesta(2)
            'reactivaRecibo
            Dim result As Boolean = New DAL().acturalizaFacturaCancelada(recibo, UUID, Respuesta(1), xDoc.ToString(), Respuesta(2), DateTime.Now(), usuarioCancela.ToString())
            If result = False Then
                errores = "El proceso de cancelacion ha quedado incompleto."
            End If
        Else
            Dim path_Name_File As String = HttpContext.Current.Server.MapPath("~/Log/" & String.Format("{0:yyyy_MM_dd}", Now) & ".txt")
            Dim file As New System.IO.StreamWriter(path_Name_File, True)
            file.WriteLine("**************************")
            file.WriteLine("Fecha Hora del Error: " & Now.ToString)
            file.WriteLine(Respuesta(0))
            file.WriteLine("**************************")
            file.Close()
            errores = "Ocurrio un problema al cancelar la factura."
        End If
        Return errores
    End Function

    Private Sub generaPDFrecibo(ByRef xDoc As XDocument, ByVal datos As DatosRecibo, ByRef valorRecibo As Recibo,
                     ByVal FechaHora As DateTime, codigo As String, datosFiel As DataTable, path As String, ByRef Errores As String, estado As String)
        '-------------------------------------------------------------------
        path = ConfigurationManager.AppSettings("RutaRecibos")
        Dim rpt As ReportViewer = New ReportViewer
        rpt.Height = 800
        rpt.Width = 500

        Try
            'obtiene valores del xml
            Dim df As XNamespace = xDoc.Root.Name.[Namespace]
            Dim sello As String = (From lv1 In xDoc.Descendants(df + "Comprobante")
                                   Select lv1.Attribute("sello").Value).First().ToString

            Dim noCertificado As String = (From lv1 In xDoc.Descendants(df + "Comprobante")
                                           Select lv1.Attribute("noCertificado").Value).First().ToString

            'Data table del ConfGral
            Dim ConfGral As DataTable = New DataTable("ConfGral")
            ConfGral.Columns.Add(New DataColumn("Nombre"))
            ConfGral.Columns.Add(New DataColumn("Calle"))
            ConfGral.Columns.Add(New DataColumn("Colonia"))
            ConfGral.Columns.Add("Logo", GetType([Byte]()))
            ConfGral.Columns.Add(New DataColumn("Mesa"))
            ConfGral.Columns.Add(New DataColumn("Cajero"))
            ConfGral.Columns.Add(New DataColumn("RFC"))
            ConfGral.Columns.Add(New DataColumn("Regimen"))
            ConfGral.Columns.Add(New DataColumn("Fondo", GetType([Byte]())))

            Dim rowGral As DataRow = ConfGral.NewRow()
            rowGral("Nombre") = datosFiel(0)("Nombre")
            rowGral("Calle") = datosFiel(0)("Calle")
            rowGral("Colonia") = datosFiel(0)("Colonia")
            rowGral("Logo") = datosFiel(0)("Logo")
            rowGral("Mesa") = datos.Comprobante_.Mesa
            rowGral("Cajero") = datos.Comprobante_.cajero
            rowGral("RFC") = datosFiel(0)("RFC")
            rowGral("Regimen") = "PERSONAS MORALES CON FINES NO LUCRATIVOS"
            'rowGral("Fondo") = 
            ConfGral.Rows.Add(rowGral)


            'Data table del comprobante
            Dim ComprobanteDTS As DataTable = New DataTable("ComprobanteDTS")
            ComprobanteDTS.Columns.Add(New DataColumn("Serie"))
            ComprobanteDTS.Columns.Add(New DataColumn("Folio"))
            ComprobanteDTS.Columns.Add(New DataColumn("FormaDePago"))
            ComprobanteDTS.Columns.Add(New DataColumn("SubTotal"))
            ComprobanteDTS.Columns.Add(New DataColumn("Total"))
            ComprobanteDTS.Columns.Add(New DataColumn("TipoDeComprobante"))
            ComprobanteDTS.Columns.Add(New DataColumn("MetodoDePago"))
            ComprobanteDTS.Columns.Add(New DataColumn("Fecha"))
            ComprobanteDTS.Columns.Add(New DataColumn("Hora"))
            ComprobanteDTS.Columns.Add(New DataColumn("TotalLetra"))
            ComprobanteDTS.Columns.Add(New DataColumn("sello"))
            ComprobanteDTS.Columns.Add(New DataColumn("noCertificado"))
            ComprobanteDTS.Columns.Add(New DataColumn("certificado"))
            ComprobanteDTS.Columns.Add(New DataColumn("codigoReimpresion"))
            ComprobanteDTS.Columns.Add(New DataColumn("cajero"))
            ComprobanteDTS.Columns.Add(New DataColumn("cveMesa"))
            ComprobanteDTS.Columns.Add(New DataColumn("descuento"))
            ComprobanteDTS.Columns.Add(New DataColumn("motivoDescuento"))
            ComprobanteDTS.Columns.Add(New DataColumn("DatosPredio"))
            ComprobanteDTS.Columns.Add(New DataColumn("Observaciones"))

            Dim row As DataRow = ComprobanteDTS.NewRow()
            row("Serie") = datos.Comprobante_.Serie
            row("Folio") = valorRecibo.folio
            row("FormaDePago") = datos.Comprobante_.FormaDePago
            row("SubTotal") = datos.Comprobante_.SubTotal
            row("Total") = datos.Comprobante_.Total
            row("TipoDeComprobante") = datos.Comprobante_.TipoDeComprobante.ToString
            row("MetodoDePago") = datos.Comprobante_.MetodoDePago
            row("Fecha") = FechaHora.ToString("dd/MM/yyyy hh:mm:ss")
            'row("Hora") = FechaHora.ToString("hh:mm:ss")
            row("TotalLetra") = Letras(datos.Comprobante_.Total).ToUpper
            row("sello") = sello.ToString
            row("noCertificado") = noCertificado
            row("certificado") = ""
            row("codigoReimpresion") = codigo
            row("cajero") = datos.Comprobante_.cajero
            row("cveMesa") = datos.Comprobante_.Mesa
            row("descuento") = datos.Comprobante_.descuento
            row("motivoDescuento") = datos.Comprobante_.motivoDescuento
            row("DatosPredio") = datos.Comprobante_.DatosPredio
            row("Observaciones") = datos.Comprobante_.Observaciones
            ComprobanteDTS.Rows.Add(row)

            'Data table del receptor
            Dim ReceptorDTS As DataTable = New DataTable("ReceptorDTS")
            ReceptorDTS.Columns.Add(New DataColumn("Nombre"))
            ReceptorDTS.Columns.Add(New DataColumn("RFC"))
            ReceptorDTS.Columns.Add(New DataColumn("Domicilio"))

            Dim rowReceptor As DataRow = ReceptorDTS.NewRow()
            rowReceptor("Nombre") = datos.Receptor_.Nombre
            rowReceptor("RFC") = datos.Receptor_.RFC
            rowReceptor("Domicilio") = datos.Receptor_.Calle &
                                        IIf(datos.Receptor_.NoExterior.Length > 0, " No. EXT." & datos.Receptor_.NoExterior, "") &
                                IIf(datos.Receptor_.Colonia.Length > 0, " COL." & datos.Receptor_.Colonia, "") &
                                IIf(datos.Receptor_.Municipio.Length > 0, ", " & datos.Receptor_.Municipio, "") &
                                IIf(datos.Receptor_.Estado.Length > 0, " " & datos.Receptor_.Estado, "") &
                                IIf(datos.Receptor_.Pais.Length > 0, " " & datos.Receptor_.Pais, "") &
                                IIf(datos.Receptor_.CodigoPostal.Length > 0, " CP." & datos.Receptor_.CodigoPostal, "")
            ReceptorDTS.Rows.Add(rowReceptor)

            'Data table del Conceptos
            Dim ConceptosDTS As DataTable = New DataTable("ConceptosDTS")
            ConceptosDTS.Columns.Add(New DataColumn("Cantidad"))
            ConceptosDTS.Columns.Add(New DataColumn("Unidad"))
            ConceptosDTS.Columns.Add(New DataColumn("Descripcion"))
            ConceptosDTS.Columns.Add(New DataColumn("ValorUnitario"))
            ConceptosDTS.Columns.Add(New DataColumn("CuentaPredial"))
            ConceptosDTS.Columns.Add(New DataColumn("id"))
            Dim formatoPredial As Boolean = False

            For Each c As Conceptos In datos.Conceptos_
                Dim rowConceptos As DataRow = ConceptosDTS.NewRow()
                rowConceptos("Cantidad") = c.Cantidad
                rowConceptos("Unidad") = c.Unidad
                rowConceptos("Descripcion") = c.Descripcion
                rowConceptos("ValorUnitario") = c.ValorUnitario
                If c.CuentaPredial = String.Empty Then
                    rowConceptos("CuentaPredial") = ""
                Else
                    rowConceptos("CuentaPredial") = c.CuentaPredial
                    formatoPredial = True
                End If
                rowConceptos("id") = c.Id
                ConceptosDTS.Rows.Add(rowConceptos)
            Next

            rpt.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local
            rpt.LocalReport.DisplayName = "Recibo"
            If estado = "P" Then
                rpt.LocalReport.ReportPath = "Reportes/ReciboPredialRpt.rdlc"
            Else
                rpt.LocalReport.ReportPath = "Reportes/cReciboPredialRpt.rdlc"
            End If


            'If formatoPredial Then
            '    rpt.LocalReport.ReportPath = "Reportes/ReciboPredialRpt.rdlc"
            'Else
            '    rpt.LocalReport.ReportPath = "Reportes/ReciboRpt.rdlc"
            'End If



            'Select Case datos.Comprobante_.t_FormatoRecibo
            '    Case "R" 'recibo simple
            '        rpt.LocalReport.ReportPath = "Reporte/ReciboRpt.rdlc"
            '    Case "P" ' recibo predial
            '        rpt.LocalReport.ReportPath = "Reporte/ReciboPredialRpt.rdlc"
            '    Case Else 'recibo simpl
            '        rpt.LocalReport.ReportPath = "Reporte/ReciboRpt.rdlc"
            'End Select


            rpt.LocalReport.DataSources.Clear()
            rpt.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("ConfGral", ConfGral))
            rpt.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("ComprobanteDTS", ComprobanteDTS))
            rpt.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("ReceptorDTS", ReceptorDTS))
            rpt.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("ConceptosDTS", ConceptosDTS))

            rpt.LocalReport.Refresh()

            Dim warnings As Warning()
            Dim streamids As String()
            Dim mimeType As String
            Dim encoding As String
            Dim filenameExtension As String

            Dim bytes As Byte() = rpt.LocalReport.Render("PDF", Nothing, mimeType, encoding, filenameExtension, streamids, warnings)

            valorRecibo.pdfBytes = bytes
            valorRecibo.mimeType = mimeType
            valorRecibo.filenameExtension = filenameExtension
        Catch ex As Exception
            Errores = ex.ToString
            Dim path_Name_File As String = HttpContext.Current.Server.MapPath("~/") + "/Log/" & String.Format("{0:yyyy_MM_dd}", Now) & ".txt"
            Dim file As New System.IO.StreamWriter(path_Name_File, True)
            file.WriteLine("**************************")
            file.WriteLine("Fecha Hora del Error: " & Now.ToString)
            file.WriteLine(ex.ToString)
            file.WriteLine("**************************")
            file.Close()
        End Try

    End Sub

    Private Sub generaPDFfactura(ByRef xDoc As XDocument, ByVal datos As DatosRecibo, ByRef valorFactura As Factura, codigo As String, datosFiel As DataTable)
        '-------------------------------------------------------------------
        Dim rpt As ReportViewer = New ReportViewer
        rpt.Height = 800
        rpt.Width = 500
        Try
            'Data table del ConfGral
            Dim ConfGral As DataTable = New DataTable("ConfGral")
            ConfGral.Columns.Add(New DataColumn("NombreMunicipio"))
            ConfGral.Columns.Add(New DataColumn("Calle"))
            ConfGral.Columns.Add(New DataColumn("Colonia"))
            ConfGral.Columns.Add("Logo", GetType([Byte]()))
            ConfGral.Columns.Add(New DataColumn("Mesa"))
            ConfGral.Columns.Add(New DataColumn("Cajero"))
            ConfGral.Columns.Add(New DataColumn("RFC"))
            ConfGral.Columns.Add(New DataColumn("Regimen"))
            ConfGral.Columns.Add(New DataColumn("Fondo", GetType([Byte]())))

            Dim rowGral As DataRow = ConfGral.NewRow()
            rowGral("NombreMunicipio") = datosFiel(0)("Nombre")
            rowGral("Calle") = datosFiel(0)("Calle")
            rowGral("Colonia") = datosFiel(0)("Colonia")
            rowGral("Logo") = datosFiel(0)("Logo")
            rowGral("Mesa") = datos.Comprobante_.Mesa
            rowGral("Cajero") = datos.Comprobante_.cajero
            rowGral("RFC") = datosFiel(0)("RFC")
            rowGral("Regimen") = "PERSONAS MORALES CON FINES NO LUCRATIVOS"
            'rowGral("Fondo") = 
            ConfGral.Rows.Add(rowGral)
            'obtiene valores del xml
            Dim df As XNamespace = xDoc.Root.Name.[Namespace]
            Dim sello As String = (From lv1 In xDoc.Descendants(df + "Comprobante")
                     Select lv1.Attribute("sello").Value).First().ToString

            Dim noCertificado As String = (From lv1 In xDoc.Descendants(df + "Comprobante")
                                Select lv1.Attribute("noCertificado").Value).First().ToString

            Dim fechaEmision As String = (From lv1 In xDoc.Descendants(df + "Comprobante")
                                Select lv1.Attribute("fecha").Value).First().ToString


            'Data table del comprobante
            Dim ComprobanteDTS As DataTable = New DataTable("ComprobanteDTS")
            ComprobanteDTS.Columns.Add(New DataColumn("Serie"))
            ComprobanteDTS.Columns.Add(New DataColumn("Folio"))
            ComprobanteDTS.Columns.Add(New DataColumn("FormaDePago"))
            ComprobanteDTS.Columns.Add(New DataColumn("SubTotal"))
            ComprobanteDTS.Columns.Add(New DataColumn("Total"))
            ComprobanteDTS.Columns.Add(New DataColumn("TipoDeComprobante"))
            ComprobanteDTS.Columns.Add(New DataColumn("MetodoDePago"))
            ComprobanteDTS.Columns.Add(New DataColumn("Fecha"))
            ComprobanteDTS.Columns.Add(New DataColumn("Hora"))
            ComprobanteDTS.Columns.Add(New DataColumn("TotalLetra"))
            ComprobanteDTS.Columns.Add(New DataColumn("sello"))
            ComprobanteDTS.Columns.Add(New DataColumn("noCertificado"))
            ComprobanteDTS.Columns.Add(New DataColumn("certificado"))
            ComprobanteDTS.Columns.Add(New DataColumn("codigoReimpresion"))
            ComprobanteDTS.Columns.Add(New DataColumn("descuento"))
            ComprobanteDTS.Columns.Add(New DataColumn("motivoDescuento"))
            ComprobanteDTS.Columns.Add(New DataColumn("DatosPredio"))
            ComprobanteDTS.Columns.Add(New DataColumn("Observaciones"))
            Dim qr As DataColumn = New DataColumn
            qr.DataType = System.Type.GetType("System.Byte[]")
            qr.Caption = "qr"
            qr.ColumnName = "qr"
            ComprobanteDTS.Columns.Add(qr)
            'ComprobanteDTS.Columns.Add(New DataColumn("qr"))

            Dim row As DataRow = ComprobanteDTS.NewRow()
            row("Serie") = datos.Comprobante_.Serie
            row("Folio") = valorFactura.folio
            row("FormaDePago") = datos.Comprobante_.FormaDePago
            row("SubTotal") = datos.Comprobante_.SubTotal
            row("Total") = datos.Comprobante_.Total
            row("TipoDeComprobante") = datos.Comprobante_.TipoDeComprobante.ToString

            ''INICIA cambia el metodo de pago DE CLAVE A DESCRIPCION
            Select Case datos.Comprobante_.MetodoDePago
                Case "01"
                    row("MetodoDePago") = "01-EFECTIVO"
                Case "02"
                    row("MetodoDePago") = "02-CHEQUE NOMINATIVO"
                Case "03"
                    row("MetodoDePago") = "03-TRANSFERENCIA ELECTRÓNICA DE FONDOS"
                Case "04"
                    row("MetodoDePago") = "04-TARJETA DE CRÉDITO"
                Case "05"
                    row("MetodoDePago") = "05-MONEDERO ELECTRÓNICO"
                Case "06"
                    row("MetodoDePago") = "06-DINERO ELECTRÓNICO"
                Case "08"
                    row("MetodoDePago") = "08-VALES DE DESPENSA"
                Case "28"
                    row("MetodoDePago") = "28-TARJETA DE DÉBITO"
                Case "29"
                    row("MetodoDePago") = "29-TARJETA DE SERVICIO"
                Case "99"
                    row("MetodoDePago") = "99-OTROS"
                Case Else
                    row("MetodoDePago") = datos.Comprobante_.MetodoDePago
            End Select

            row("Fecha") = "MÉXICO " + fechaEmision  'FechaHora.ToString("dd/MM/yyyy")
            row("Hora") = "" 'FechaHora.ToString("hh:mm:ss")
            row("TotalLetra") = Letras(datos.Comprobante_.Total).ToUpper
            row("sello") = sello.ToString
            row("noCertificado") = noCertificado
            row("certificado") = ""
            row("codigoReimpresion") = codigo
            row("descuento") = datos.Comprobante_.descuento
            row("motivoDescuento") = datos.Comprobante_.motivoDescuento
            row("DatosPredio") = datos.Comprobante_.DatosPredio
            row("Observaciones") = datos.Comprobante_.Observaciones
            ComprobanteDTS.Rows.Add(row)

            'Data table del FolioFiscalDTS
            Dim FolioFiscalDTS As DataTable = New DataTable("FolioFiscalDTS")
            FolioFiscalDTS.Columns.Add(New DataColumn("FolioFiscal"))
            FolioFiscalDTS.Columns.Add(New DataColumn("NoSerieCertificadoSAT"))
            FolioFiscalDTS.Columns.Add(New DataColumn("SelloSAT"))
            FolioFiscalDTS.Columns.Add(New DataColumn("CadenaOriginalSAT"))
            FolioFiscalDTS.Columns.Add(New DataColumn("FechaEmisionCertificado"))

            Dim elementComprobante = xDoc.Elements(df + "Comprobante").Single
            Dim elementComplemento = elementComprobante.Elements(df + "Complemento").Single
            Dim tfd As XNamespace = "http://www.sat.gob.mx/TimbreFiscalDigital"
            Dim elementTimbreFiscalDigital = elementComplemento.Elements(tfd + "TimbreFiscalDigital").Single

            Dim rowFolioFiscal As DataRow = FolioFiscalDTS.NewRow()
            rowFolioFiscal("FolioFiscal") = elementTimbreFiscalDigital.Attribute("UUID").Value
            rowFolioFiscal("NoSerieCertificadoSAT") = elementTimbreFiscalDigital.Attribute("noCertificadoSAT").Value
            rowFolioFiscal("SelloSAT") = elementTimbreFiscalDigital.Attribute("selloSAT").Value
            rowFolioFiscal("CadenaOriginalSAT") = elementTimbreFiscalDigital.Attribute("selloCFD").Value
            rowFolioFiscal("FechaEmisionCertificado") = elementTimbreFiscalDigital.Attribute("FechaTimbrado").Value
            FolioFiscalDTS.Rows.Add(rowFolioFiscal)

            'genera qr
            Dim qrEncoder As QRCodeEncoder = New QRCodeEncoder
            Dim qrBitmap As Bitmap = qrEncoder.Encode("?re=" + valorFactura.Rfc + "&rr=" + datos.Receptor_.RFC + "&tt=" + datos.Comprobante_.Total.ToString("C2") + "&id=" + elementTimbreFiscalDigital.Attribute("UUID").Value + "")
            Dim qrBytes As Byte()
            Dim stream As New System.IO.MemoryStream
            qrBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png)
            qrBytes = stream.ToArray
            ComprobanteDTS.Rows(0)("qr") = qrBytes


            'Data table del receptor
            Dim ReceptorDTS As DataTable = New DataTable("ReceptorDTS")
            ReceptorDTS.Columns.Add(New DataColumn("Nombre"))
            ReceptorDTS.Columns.Add(New DataColumn("RFC"))
            ReceptorDTS.Columns.Add(New DataColumn("Domicilio"))

            Dim rowReceptor As DataRow = ReceptorDTS.NewRow()
            rowReceptor("Nombre") = datos.Receptor_.Nombre
            rowReceptor("RFC") = datos.Receptor_.RFC
            rowReceptor("Domicilio") = datos.Receptor_.Calle &
                                        IIf(datos.Receptor_.NoExterior.Length > 0, " No. EXT." & datos.Receptor_.NoExterior, "") &
                                        IIf(datos.Receptor_.NoInterior.Length > 0, " No. INT." & datos.Receptor_.NoInterior, "") &
                                        IIf(datos.Receptor_.Colonia.Length > 0, " COL." & datos.Receptor_.Colonia, "") &
                                        IIf(datos.Receptor_.Municipio.Length > 0, ", " & datos.Receptor_.Municipio, "") &
                                        IIf(datos.Receptor_.Estado.Length > 0, " " & datos.Receptor_.Estado, "") &
                                        IIf(datos.Receptor_.Pais.Length > 0, " " & datos.Receptor_.Pais, "") &
                                        IIf(datos.Receptor_.CodigoPostal.Length > 0, " CP." & datos.Receptor_.CodigoPostal, "")
            ReceptorDTS.Rows.Add(rowReceptor)

            'Data table del Conceptos
            Dim ConceptosDTS As DataTable = New DataTable("ConceptosDTS")
            ConceptosDTS.Columns.Add(New DataColumn("Cantidad"))
            ConceptosDTS.Columns.Add(New DataColumn("Unidad"))
            ConceptosDTS.Columns.Add(New DataColumn("Descripcion"))
            ConceptosDTS.Columns.Add(New DataColumn("ValorUnitario"))
            ConceptosDTS.Columns.Add(New DataColumn("CuentaPredial"))
            ConceptosDTS.Columns.Add(New DataColumn("id"))

            If datos.Conceptos_ IsNot Nothing Then
                For Each c As Conceptos In datos.Conceptos_
                    Dim rowConceptos As DataRow = ConceptosDTS.NewRow()
                    rowConceptos("Cantidad") = c.Cantidad
                    rowConceptos("Unidad") = c.Unidad
                    rowConceptos("Descripcion") = c.Descripcion
                    rowConceptos("ValorUnitario") = c.ValorUnitario
                    rowConceptos("CuentaPredial") = c.CuentaPredial
                    rowConceptos("id") = c.Id
                    ConceptosDTS.Rows.Add(rowConceptos)
                Next
            End If

            rpt.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local
            rpt.LocalReport.DisplayName = "Factura"

            ' factura predial
            rpt.LocalReport.ReportPath = "Reportes/FacturaPredialRpt.rdlc"
            rpt.LocalReport.DataSources.Clear()
            rpt.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("ConfGral", ConfGral))
            rpt.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("ComprobanteDTS", ComprobanteDTS))
            rpt.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("ReceptorDTS", ReceptorDTS))
            rpt.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("ConceptosDTS", ConceptosDTS))
            rpt.LocalReport.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("FolioFiscalDTS", FolioFiscalDTS))



            rpt.LocalReport.Refresh()

            Dim warnings As Warning()
            Dim streamids As String()
            Dim mimeType As String
            Dim encoding As String
            Dim filenameExtension As String

            Dim bytes As Byte() = rpt.LocalReport.Render("PDF", Nothing, mimeType, encoding, filenameExtension, streamids, warnings)

            valorFactura.pdfBytes = bytes
            valorFactura.mimeType = mimeType
            valorFactura.filenameExtension = filenameExtension

        Catch ex As Exception
            Dim path_Name_File As String = HttpContext.Current.Server.MapPath("~/Log/" & String.Format("{0:yyyy_MM_dd}", Now) & ".txt")
            Dim file As New System.IO.StreamWriter(path_Name_File, True)
            file.WriteLine("**************************")
            file.WriteLine("Fecha Hora del Error: " & Now.ToString)
            file.WriteLine(ex.ToString)
            file.WriteLine("**************************")
            file.Close()
        End Try

    End Sub

    Private Function timbraFactura(ByVal strXML As String, ByRef Errores As String, anioFolio As String, ByRef Productivo As Boolean, ByRef usuarioFactura As String, ByRef passwordFactura As String)


        Dim ServicioFI As New facturainteligente2.FI_TFD

        Dim Respuesta() As String
        If Productivo Then
            '-----web service timbre PRODUCCION
            Respuesta = ServicioFI.TimbrarCFD(usuarioFactura, passwordFactura, strXML, anioFolio)
        Else
            '-----web service timbre de PRUEBAS  
            Respuesta = ServicioFI.TimbrarCFDIPrueba(usuarioFactura, passwordFactura, strXML)
        End If

        If Respuesta(0) = "" And Respuesta(1) = "" And Respuesta(2) = "" Then
            Return Respuesta(3)
        Else
            Errores = "Error Timbrado:-----" & Respuesta(0) & "-----" & Respuesta(1) & "-----" & Respuesta(2)
            'inicia log de errores genera log
            Dim path_Name_File As String = HttpContext.Current.Server.MapPath("~/Log/" & String.Format("{0:yyyy_MM_dd}", Now) & ".txt")
            Dim file As New System.IO.StreamWriter(path_Name_File, True)
            file.WriteLine("**************************")
            file.WriteLine("Error de timbrado: " & Now.ToString)
            file.WriteLine(Errores)
            file.WriteLine(strXML)
            file.WriteLine("**************************")
            file.Close()
            Return Errores
        End If
    End Function

    Private Function cancelaTimbraFactura(ByVal uuid As String, ByVal recibo As String, ByRef Errores As String, ByRef byteCertificado As Byte(), ByRef byteLLavePrivada As Byte(),
                                          ByRef KeyPass As String, ByRef usuario As String, ByRef password As String, ByRef RFCEmisor As String)

        Dim ServicioFI As New facturainteligente2.FI_TFD
        Dim Respuesta() As String

        Dim path As String = ConfigurationManager.AppSettings("RutaRecibos") 'HttpContext.Current.Server.MapPath("~/")

        Dim pkcs12Pfx As Pkcs12Pfx = New Pkcs12Pfx
        Dim generarBytePKS12 As Byte() = Nothing

        generarBytePKS12 = pkcs12Pfx.Generar(byteLLavePrivada, byteCertificado, KeyPass)

        Dim listaCFDI() As String = {uuid}
        '-----web service timbre PRODUCCION*********************
        Respuesta = ServicioFI.CancelarCFDI(usuario, password, RFCEmisor, listaCFDI, Convert.ToBase64String(generarBytePKS12), KeyPass)

        If Respuesta(0) = "" And Respuesta(1) <> "" And Respuesta(2) <> "" Then
            Return Respuesta
        Else
            Errores = "Error de cancelacion de timbrado Timbrado:-----Recibo: " & recibo & "-----" & Respuesta(0) & "-----" & Respuesta(1) & "-----" & Respuesta(2)
            'inicia log de errores genera log
            Dim path_Name_File As String = HttpContext.Current.Server.MapPath("~/Log/" & String.Format("{0:yyyy_MM_dd}", Now) & ".txt")
            Dim file As New System.IO.StreamWriter(path_Name_File, True)
            file.WriteLine("**************************")
            file.WriteLine("Error de timbrado: " & Now.ToString)
            file.WriteLine(Errores)
            file.WriteLine(uuid)
            file.WriteLine("**************************")
            file.Close()
            Return Errores
        End If
    End Function

    Public Function Letras(ByVal numero As String) As String

        Dim l As NumLetras = New NumLetras
        'al uso en México (creo):
        l.MascaraSalidaDecimal = "00/100 M.N."
        l.SeparadorDecimalSalida = "pesos"

        l.ApocoparUnoParteEntera = True

        Return l.ToCustomCardinal(Convert.ToDouble(numero))

    End Function


    'Declaración de la API
    Private Declare Auto Function SetProcessWorkingSetSize Lib "kernel32.dll" (ByVal procHandle As IntPtr, ByVal min As Int32, ByVal max As Int32) As Boolean
    'Funcion de liberacion de memoria
    Public Sub ClearMemory()
        Try
            Dim Mem As Process
            Mem = Process.GetCurrentProcess()
            SetProcessWorkingSetSize(Mem.Handle, -1, -1)
        Catch ex As Exception
            'Control de errores
        End Try
    End Sub
End Class
