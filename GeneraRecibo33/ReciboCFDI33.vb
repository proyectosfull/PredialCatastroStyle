Imports Recibo33
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports System.Web
Imports MessagingToolkit.QRCode.Codec
Imports System.Drawing
Imports System.Configuration

Public Class ReciboCFDI33
    Public Function generaRecibo(datos As DatosRecibo, path As String) As Recibo
        path = ConfigurationManager.AppSettings("RutaRecibos")
        Dim Errores As String = ""
        Try

            Dim datosFiel As DataTable = New DAL().cFIELGetByActive()
            Dim valorRecibo As Recibo = New Recibo()
            Dim FechaHora As DateTime = Now
            Dim TipoDeComprobante As String = "I"

            Dim Version As CFDx33.VersionCFD = CFDx33.VersionCFD.CFDv3_3

            Dim CFDs As New CFDx33
            With CFDs
                .Comprobante(Version, datos.Comprobante_.Folio, FormatDateTime(FechaHora, DateFormat.GeneralDate),
                        datos.Comprobante_.FormaDePago, datos.Comprobante_.SubTotal, datos.Comprobante_.Total,
                        TipoDeComprobante, datos.Comprobante_.MetodoDePago, datosFiel(0)("CodigoPostal"), datos.Comprobante_.Serie,
                         "", "", datos.Comprobante_.descuento, datos.Comprobante_.motivoDescuento)

                .AgregaEmisor(datosFiel(0)("RFC"), datosFiel(0)("Nombre"))

                .AgregaReceptor(datos.Receptor_.RFC,
                                IIf(datos.Receptor_.Nombre = Nothing, "Publico en general", datos.Receptor_.Nombre),
                                IIf(datos.Receptor_.UsoCFDI = Nothing, "P01", datos.Receptor_.UsoCFDI))

                'Solo en caso de ser CFD v3.2
                '.AgregaRegimenFiscal("PERSONAS MORALES CON FINES NO LUCRATIVOS")

                For Each c As Conceptos In datos.Conceptos_
                    .AgregaConcepto(c.Cantidad, c.ClaveProdServ, c.ClaveUnidad, c.Unidad, c.Descripcion, c.ValorUnitario, c.Importe, c.Descuento, c.Id, IIf(c.CuentaPredial = Nothing, "", c.CuentaPredial), Nothing)
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

    Public Function generaFacturaRecibo(rfc As String, IdRecibo As Integer, usuarioFactura As String, passwordFactura As String, productivo As Boolean, usoCFDI As String) As Factura
        Dim path As String = ConfigurationManager.AppSettings("RutaRecibos") ' HttpContext.Current.Server.MapPath("~/")
        Dim fact As Factura = New Factura()
        Dim fecha As Date = DateTime.Now()
        Dim datosFiel As DataTable = New DAL().cFIELGetByActive()
        Dim nombreMun As DataTable = New DAL().cParametroSistema("NOMBRE_MUNICIPIO")
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
            fact.folio = elementComprobante.Attribute("Folio").Value

            Dim elementReceptor = elementComprobante.Elements(df + "Receptor").Single
            elementReceptor.Attribute("Rfc").Value = receptor(0)("RFC")
            elementReceptor.Attribute("UsoCFDI").Value = usoCFDI

            ''valida rfc generico
            If (receptor(0)("RFC") = "XAXX010101000") Then
                elementReceptor.Attribute("Nombre").Value = recibo(0)("Contribuyente")
            Else
                elementReceptor.Attribute("Nombre").Value = receptor(0)("Nombre")
            End If

            'se vuelve a sellar el archivo debido a que 
            Dim CFDs As New CFDx33

            'ACTUALIZA FECHA PARA TIMBRAR
            Dim FechaHora As DateTime = Now.AddSeconds(-120)
            elementComprobante.Attribute("Fecha").Value = String.Format("{0:s}", FechaHora)
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
                comprobante.TipoDeComprobante = "I"
                comprobante.Serie = elementComprobante.Attribute("Serie").Value
                comprobante.SubTotal = elementComprobante.Attribute("SubTotal").Value
                comprobante.Total = elementComprobante.Attribute("Total").Value
                'comprobante.FormaDePago = elementComprobante.Attribute("FormaPago").Value
                Dim FormaPago As String = elementComprobante.Attribute("FormaPago").Value
                ''INICIA cambia el metodo de pago DE CLAVE A DESCRIPCION
                Select Case FormaPago
                    Case "01"
                        comprobante.FormaDePago = "01-EFECTIVO"
                    Case "02"
                        comprobante.FormaDePago = "02-CHEQUE NOMINATIVO"
                    Case "03"
                        comprobante.FormaDePago = "03-TRANSFERENCIA ELECTRÓNICA DE FONDOS"
                    Case "04"
                        comprobante.FormaDePago = "04-TARJETA DE CRÉDITO"
                    Case "05"
                        comprobante.FormaDePago = "05-MONEDERO ELECTRÓNICO"
                    Case "06"
                        comprobante.FormaDePago = "06-DINERO ELECTRÓNICO"
                    Case "08"
                        comprobante.FormaDePago = "08-VALES DE DESPENSA"
                    Case "28"
                        comprobante.FormaDePago = "28-TARJETA DE DÉBITO"
                    Case "29"
                        comprobante.FormaDePago = "29-TARJETA DE SERVICIO"
                    Case "99"
                        comprobante.FormaDePago = "99-OTROS"
                    Case Else
                        comprobante.FormaDePago = FormaPago
                End Select
                comprobante.TipoDeComprobante = elementComprobante.Attribute("TipoDeComprobante").Value
                comprobante.MetodoDePago = IIf(elementComprobante.Attribute("MetodoPago").Value = "PUE", "PAGO EN UNA SOLA EXHIBICIÓN", elementComprobante.Attribute("MetodoPago").Value)
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
                    Dim Descuento As Double = 0

                    If cuentaPredial Is Nothing Then 'si cuenta predial esta vacia entra
                        If Not var.Attribute("NoIdentificacion") Is Nothing Then
                            noIdentificacion = var.Attribute("NoIdentificacion").Value
                        End If
                        If Not var.Attribute("Importe") Is Nothing Then
                            Importe = var.Attribute("Importe").Value
                        End If
                        If Not var.Attribute("Descuento") Is Nothing Then
                            Descuento = var.Attribute("Descuento").Value
                        End If
                        conceptos.Add(New Conceptos(
                                    var.Attribute("Cantidad").Value,
                                    var.Attribute("Unidad").Value,
                                    var.Attribute("Descripcion").Value,
                                    var.Attribute("ValorUnitario").Value,
                                    noIdentificacion,
                                    Importe,
                                    Descuento,
                                    ""))
                    Else 'solo aplica para predial
                        If Not var.Attribute("NoIdentificacion") Is Nothing Then
                            noIdentificacion = var.Attribute("NoIdentificacion").Value
                        End If
                        If Not var.Attribute("Importe") Is Nothing Then
                            Importe = var.Attribute("Importe").Value
                        End If
                        If Not var.Attribute("Descuento") Is Nothing Then
                            Descuento = var.Attribute("Descuento").Value
                        End If
                        conceptos.Add(New Conceptos(
                                    var.Attribute("Cantidad").Value,
                                    var.Attribute("Unidad").Value,
                                    var.Attribute("Descripcion").Value,
                                    var.Attribute("ValorUnitario").Value,
                                    noIdentificacion,
                                    Importe,
                                    Descuento,
                                    cuentaPredial.Attribute("Numero").Value))
                    End If
                Next
                dfactura.Conceptos_ = conceptos
                Dim R As New Receptor()
                R.Nombre = receptor(0)("Nombre")
                R.RFC = receptor(0)("RFC")
                R.Domicilio = recibo(0)("Domicilio")
                'R.Calle = receptor(0)("calle")
                'R.NoExterior = receptor(0)("NoExterior")
                'R.NoInterior = receptor(0)("NoInterior")
                'R.Colonia = receptor(0)("Colonia")
                'R.Municipio = receptor(0)("Municipio")
                'R.Estado = receptor(0)("Estado")
                'R.Pais = receptor(0)("Pais")
                'R.CodigoPostal = receptor(0)("CodigoPostal")
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

            Dim sello As String = xeComprobante.Attribute("Sello").Value
            Dim noCertificado As String = xeComprobante.Attribute("NoCertificado").Value

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

            rowGral("Nombre") = xeEmisor.Attribute("Nombre").Value
            rowGral("Calle") = datosFiel(0)("Calle")
            rowGral("Colonia") = datosFiel(0)("Colonia")
            rowGral("Logo") = datosFiel(0)("Logo")
            rowGral("Mesa") = recibo.cMesa.Nombre
            rowGral("Cajero") = recibo.cUsuarios.Usuario
            rowGral("RFC") = xeEmisor.Attribute("Rfc").Value
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
            row("Serie") = xeComprobante.Attribute("Serie").Value
            row("Folio") = xeComprobante.Attribute("Folio").Value
            'row("FormaDePago") = xeComprobante.Attribute("FormaPago").Value
            Dim FormaPago As String = xeComprobante.Attribute("FormaPago").Value
            ''INICIA cambia el metodo de pago DE CLAVE A DESCRIPCION
            Select Case FormaPago
                Case "01"
                    row("FormaDePago") = "01-EFECTIVO"
                Case "02"
                    row("FormaDePago") = "02-CHEQUE NOMINATIVO"
                Case "03"
                    row("FormaDePago") = "03-TRANSFERENCIA ELECTRÓNICA DE FONDOS"
                Case "04"
                    row("FormaDePago") = "04-TARJETA DE CRÉDITO"
                Case "05"
                    row("FormaDePago") = "05-MONEDERO ELECTRÓNICO"
                Case "06"
                    row("FormaDePago") = "06-DINERO ELECTRÓNICO"
                Case "08"
                    row("FormaDePago") = "08-VALES DE DESPENSA"
                Case "28"
                    row("FormaDePago") = "28-TARJETA DE DÉBITO"
                Case "29"
                    row("FormaDePago") = "29-TARJETA DE SERVICIO"
                Case "99"
                    row("FormaDePago") = "99-OTROS"
                Case Else
                    row("FormaDePago") = FormaPago
            End Select
            row("SubTotal") = xeComprobante.Attribute("SubTotal").Value
            row("Total") = xeComprobante.Attribute("Total").Value
            row("TipoDeComprobante") = xeComprobante.Attribute("TipoDeComprobante").Value
            row("MetodoDePago") = IIf(xeComprobante.Attribute("MetodoPago").Value = "PUE", "PAGO EN UNA SOLA EXHIBICIÓN", xeComprobante.Attribute("MetodoPago").Value)
            row("Fecha") = Convert.ToDateTime(xeComprobante.Attribute("Fecha").Value).ToString("dd/MM/yyyy hh:mm:ss")

            row("TotalLetra") = Letras(xeComprobante.Attribute("Total").Value).ToUpper
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
            rowReceptor("Nombre") = xeReceptor.Attribute("Nombre").Value
            rowReceptor("RFC") = xeReceptor.Attribute("Rfc").Value
            rowReceptor("Domicilio")=recibo.Domicilio
            'rowReceptor("Domicilio") = xeReceptorDomicilio.Attribute("calle").Value & " No. Ext." & xeReceptorDomicilio.Attribute("noExterior").Value &
            '                          " Col. " & xeReceptorDomicilio.Attribute("colonia").Value & ", " & xeReceptorDomicilio.Attribute("municipio").Value &
            '                          " " & xeReceptorDomicilio.Attribute("estado").Value & ", " & xeReceptorDomicilio.Attribute("pais").Value &
            '                          " Cp." & xeReceptorDomicilio.Attribute("codigoPostal").Value
            ReceptorDTS.Rows.Add(rowReceptor)

            'Data table del Conceptos
            Dim ConceptosDTS As DataTable = New DataTable("ConceptosDTS")
            ConceptosDTS.Columns.Add(New DataColumn("Cantidad"))
            ConceptosDTS.Columns.Add(New DataColumn("Unidad"))
            ConceptosDTS.Columns.Add(New DataColumn("Descripcion"))
            ConceptosDTS.Columns.Add(New DataColumn("ValorUnitario"))
            ConceptosDTS.Columns.Add(New DataColumn("CuentaPredial"))
            ConceptosDTS.Columns.Add(New DataColumn("Id"))
            ConceptosDTS.Columns.Add(New DataColumn("Importe"))
            ConceptosDTS.Columns.Add(New DataColumn("Descuento"))
            ConceptosDTS.Columns.Add(New DataColumn("ClaveProdServ"))
            ConceptosDTS.Columns.Add(New DataColumn("ClaveUnidad"))
            Dim formatoPredial As Boolean = False

            For Each c As XElement In xeConceptos.Descendants(df + "Concepto")
                Dim rowConceptos As DataRow = ConceptosDTS.NewRow()
                rowConceptos("Cantidad") = c.Attribute("Cantidad").Value 'Cantidad
                rowConceptos("Unidad") = c.Attribute("Unidad").Value ' c.Unidad
                rowConceptos("Descripcion") = c.Attribute("Descripcion").Value 'c.Descripcion
                rowConceptos("ValorUnitario") = c.Attribute("ValorUnitario").Value 'c.ValorUnitario
                Dim xepredial As XElement = xeConceptos.Descendants(df + "CuentaPredial").FirstOrDefault
                'Dim predial As XAttribute = c.Attribute("CuentaPredial")
                If xepredial Is Nothing Then
                    rowConceptos("CuentaPredial") = ""
                Else
                    rowConceptos("CuentaPredial") = xepredial.Attribute("Numero").Value
                    formatoPredial = True
                End If
                rowConceptos("id") = c.Attribute("NoIdentificacion").Value 'c.Id
                rowConceptos("id") = c.Attribute("NoIdentificacion").Value 'c.Id
                rowConceptos("Importe") = c.Attribute("Importe").Value
                If Not c.Attribute("Descuento") Is Nothing Then
                    rowConceptos("Descuento") = c.Attribute("Descuento").Value
                Else
                    rowConceptos("Descuento") = "0"
                End If
                rowConceptos("ClaveProdServ") = c.Attribute("ClaveProdServ").Value
                rowConceptos("ClaveUnidad") = c.Attribute("ClaveUnidad").Value
                ConceptosDTS.Rows.Add(rowConceptos)
            Next


            rpt.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local
            rpt.LocalReport.DisplayName = "Recibo"
            If recibo.EstadoRecibo = "P" Then
                rpt.LocalReport.ReportPath = "Reportes/ReciboPredialRpt33.rdlc"
            Else
                rpt.LocalReport.ReportPath = "Reportes/cReciboPredialRpt33.rdlc"
            End If

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
            Dim nombreMun As DataTable = New DAL().cParametroSistema("NOMBRE_MUNICIPIO")
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

            rowGral("NombreMunicipio") = xeEmisor.Attribute("Nombre").Value
            'rowGral("Calle") = xeDomicilioFiscal.Attribute("calle").Value
            'rowGral("Colonia") = datosFiel(0)("Colonia")
            rowGral("Logo") = datosFiel(0)("Logo")
            rowGral("Mesa") = recibo.cMesa.Nombre
            rowGral("Cajero") = recibo.cUsuarios.Usuario
            rowGral("RFC") = xeEmisor.Attribute("Rfc").Value
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
            Dim FormaPago As String = xeComprobante.Attribute("FormaPago").Value
            ''INICIA cambia el metodo de pago DE CLAVE A DESCRIPCION
            Select Case FormaPago
                Case "01"
                    row("FormaDePago") = "01-EFECTIVO"
                Case "02"
                    row("FormaDePago") = "02-CHEQUE NOMINATIVO"
                Case "03"
                    row("FormaDePago") = "03-TRANSFERENCIA ELECTRÓNICA DE FONDOS"
                Case "04"
                    row("FormaDePago") = "04-TARJETA DE CRÉDITO"
                Case "05"
                    row("FormaDePago") = "05-MONEDERO ELECTRÓNICO"
                Case "06"
                    row("FormaDePago") = "06-DINERO ELECTRÓNICO"
                Case "08"
                    row("FormaDePago") = "08-VALES DE DESPENSA"
                Case "28"
                    row("FormaDePago") = "28-TARJETA DE DÉBITO"
                Case "29"
                    row("FormaDePago") = "29-TARJETA DE SERVICIO"
                Case "99"
                    row("FormaDePago") = "99-OTROS"
                Case Else
                    row("FormaDePago") = FormaPago
            End Select

            row("Fecha") = nombreMun(0)(0).ToString() + " ,MOR. " + xeComprobante.Attribute("Fecha").Value  'FechaHora.ToString("dd/MM/yyyy")
            row("Serie") = xeComprobante.Attribute("Serie").Value
            row("Folio") = xeComprobante.Attribute("Folio").Value
            row("MetodoDePago") = IIf(xeComprobante.Attribute("MetodoPago").Value = "PUE", "PAGO EN UNA SOLA EXHIBICIÓN", xeComprobante.Attribute("MetodoPago").Value)
            row("SubTotal") = xeComprobante.Attribute("SubTotal").Value
            row("Total") = xeComprobante.Attribute("Total").Value
            row("TipoDeComprobante") = xeComprobante.Attribute("TipoDeComprobante").Value

            row("TotalLetra") = Letras(xeComprobante.Attribute("Total").Value).ToUpper
            row("sello") = xeComprobante.Attribute("Sello").Value
            row("noCertificado") = xeComprobante.Attribute("NoCertificado").Value
            row("certificado") = ""
            row("codigoReimpresion") = recibo.CodigoSeguridad
            row("cajero") = recibo.cUsuarios.Usuario
            row("cveMesa") = recibo.cMesa.Nombre
            row("descuento") = recibo.ImporteDescuento
            If Not xeComprobante.Attribute("MotivoDescuento") Is Nothing Then
                row("motivoDescuento") = xeComprobante.Attribute("MotivoDescuento").Value
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
            rowReceptor("Nombre") = xeReceptor.Attribute("Nombre").Value
            rowReceptor("RFC") = xeReceptor.Attribute("Rfc").Value
            rowReceptor("Domicilio") = recibo.Domicilio
            'If xeReceptor.Attribute("Rfc").Value = "XAXX010101000" Then
            '    rowReceptor("Domicilio") = xeReceptorDomicilio.Attribute("calle").Value & ", " & xeReceptorDomicilio.Attribute("municipio").Value &
            '                         " " & xeReceptorDomicilio.Attribute("estado").Value & ", " & xeReceptorDomicilio.Attribute("pais").Value
            'Else
            '    rowReceptor("Domicilio") = xeReceptorDomicilio.Attribute("calle").Value & " No. Ext." & xeReceptorDomicilio.Attribute("noExterior").Value &
            '                         " Col. " & xeReceptorDomicilio.Attribute("colonia").Value & ", " & xeReceptorDomicilio.Attribute("municipio").Value &
            '                         " " & xeReceptorDomicilio.Attribute("estado").Value & ", " & xeReceptorDomicilio.Attribute("pais").Value &
            '                         " Cp." & xeReceptorDomicilio.Attribute("codigoPostal").Value
            'End If

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
            rowFolioFiscal("NoSerieCertificadoSAT") = elementTimbreFiscalDigital.Attribute("NoCertificadoSAT").Value
            rowFolioFiscal("SelloSAT") = elementTimbreFiscalDigital.Attribute("SelloSAT").Value
            rowFolioFiscal("CadenaOriginalSAT") = elementTimbreFiscalDigital.Attribute("SelloCFD").Value
            rowFolioFiscal("FechaEmisionCertificado") = elementTimbreFiscalDigital.Attribute("FechaTimbrado").Value
            FolioFiscalDTS.Rows.Add(rowFolioFiscal)

            'genera qr
            Dim rfc As String = xeEmisor.Attribute("Rfc").Value
            Dim rfcReceptor As String = xeReceptor.Attribute("Rfc").Value
            Dim total As Decimal = Convert.ToDecimal(xeComprobante.Attribute("Total").Value)
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
            ConceptosDTS.Columns.Add(New DataColumn("Id"))
            ConceptosDTS.Columns.Add(New DataColumn("Importe"))
            ConceptosDTS.Columns.Add(New DataColumn("Descuento"))
            ConceptosDTS.Columns.Add(New DataColumn("ClaveProdServ"))
            ConceptosDTS.Columns.Add(New DataColumn("ClaveUnidad"))
            Dim formatoPredial As Boolean = False

            For Each c As XElement In xeConceptos.Descendants(df + "Concepto")
                Dim rowConceptos As DataRow = ConceptosDTS.NewRow()
                rowConceptos("Cantidad") = c.Attribute("Cantidad").Value ' Cantidad
                rowConceptos("Unidad") = c.Attribute("Unidad").Value ' c.Unidad
                rowConceptos("Descripcion") = c.Attribute("Descripcion").Value ' c.Descripcion
                rowConceptos("ValorUnitario") = c.Attribute("ValorUnitario").Value ' c.ValorUnitario
                Dim xepredial As XElement = xeConceptos.Descendants(df + "CuentaPredial").FirstOrDefault
                'Dim predial As XAttribute = c.Attribute("CuentaPredial")
                If xepredial Is Nothing Then
                    rowConceptos("CuentaPredial") = ""
                Else
                    rowConceptos("CuentaPredial") = xepredial.Attribute("Numero").Value
                    formatoPredial = True
                End If
                rowConceptos("id") = c.Attribute("NoIdentificacion").Value 'c.Id
                rowConceptos("Importe") = c.Attribute("Importe").Value
                If Not c.Attribute("Descuento") Is Nothing Then
                    rowConceptos("Descuento") = c.Attribute("Descuento").Value
                Else
                    rowConceptos("Descuento") = "0"
                End If
                rowConceptos("ClaveProdServ") = c.Attribute("ClaveProdServ").Value
                rowConceptos("ClaveUnidad") = c.Attribute("ClaveUnidad").Value

                ConceptosDTS.Rows.Add(rowConceptos)
            Next


            rpt.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local
            rpt.LocalReport.DisplayName = "Factura"

            ' factura predial
            rpt.LocalReport.ReportPath = "Reportes/FacturaPredialRpt33.rdlc"
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
        'cancela factura sat
        Dim elementComprobante = xDoc.Elements(df + "Comprobante").Single
        Dim elementReceptor = elementComprobante.Elements(df + "Receptor").Single
        Dim UUID As String = (From c In complemento.Descendants() Where c.Name = dfcomplemento + "TimbreFiscalDigital" Select c).Single.Attribute("UUID").Value()
        Dim Respuesta() As String = cancelaTimbraFactura(UUID, recibo.Id, elementReceptor.Attribute("Rfc").Value, elementComprobante.Attribute("Total").Value, errores, datosFiel(0)("CerFile"), datosFiel(0)("KeyFile"), KeyPass, usuarioFactura, passwordFactura, datosFiel(0)("RFC"))

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
                                   Select lv1.Attribute("Sello").Value).First().ToString

            Dim noCertificado As String = (From lv1 In xDoc.Descendants(df + "Comprobante")
                                           Select lv1.Attribute("NoCertificado").Value).First().ToString

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
            Dim FormaPago As String = datos.Comprobante_.FormaDePago
            ''INICIA cambia el metodo de pago DE CLAVE A DESCRIPCION
            Select Case FormaPago
                Case "01"
                    row("FormaDePago") = "01-EFECTIVO"
                Case "02"
                    row("FormaDePago") = "02-CHEQUE NOMINATIVO"
                Case "03"
                    row("FormaDePago") = "03-TRANSFERENCIA ELECTRÓNICA DE FONDOS"
                Case "04"
                    row("FormaDePago") = "04-TARJETA DE CRÉDITO"
                Case "05"
                    row("FormaDePago") = "05-MONEDERO ELECTRÓNICO"
                Case "06"
                    row("FormaDePago") = "06-DINERO ELECTRÓNICO"
                Case "08"
                    row("FormaDePago") = "08-VALES DE DESPENSA"
                Case "28"
                    row("FormaDePago") = "28-TARJETA DE DÉBITO"
                Case "29"
                    row("FormaDePago") = "29-TARJETA DE SERVICIO"
                Case "99"
                    row("FormaDePago") = "99-OTROS"
                Case Else
                    row("FormaDePago") = FormaPago
            End Select
            row("SubTotal") = datos.Comprobante_.SubTotal
            row("Total") = datos.Comprobante_.Total
            row("TipoDeComprobante") = datos.Comprobante_.TipoDeComprobante.ToString()
            row("MetodoDePago") = IIf(datos.Comprobante_.MetodoDePago = "PUE", "PAGO EN UNA SOLA EXHIBICIÓN", datos.Comprobante_.MetodoDePago)
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
            'row("Observaciones") = datos.Comprobante_.Observaciones
            ComprobanteDTS.Rows.Add(row)

            'Data table del receptor
            Dim ReceptorDTS As DataTable = New DataTable("ReceptorDTS")
            ReceptorDTS.Columns.Add(New DataColumn("Nombre"))
            ReceptorDTS.Columns.Add(New DataColumn("RFC"))
            ReceptorDTS.Columns.Add(New DataColumn("Domicilio"))

            Dim rowReceptor As DataRow = ReceptorDTS.NewRow()
            rowReceptor("Nombre") = datos.Receptor_.Nombre
            rowReceptor("RFC") = datos.Receptor_.RFC
            rowReceptor("Domicilio") = datos.Receptor_.Domicilio '""
            'datos.Receptor_.Calle &
            '                            IIf(datos.Receptor_.NoExterior.Length > 0, " No. EXT." & datos.Receptor_.NoExterior, "") &
            '                    IIf(datos.Receptor_.Colonia.Length > 0, " COL." & datos.Receptor_.Colonia, "") &
            '                    IIf(datos.Receptor_.Municipio.Length > 0, ", " & datos.Receptor_.Municipio, "") &
            '                    IIf(datos.Receptor_.Estado.Length > 0, " " & datos.Receptor_.Estado, "") &
            '                    IIf(datos.Receptor_.Pais.Length > 0, " " & datos.Receptor_.Pais, "") &
            '                    IIf(datos.Receptor_.CodigoPostal.Length > 0, " CP." & datos.Receptor_.CodigoPostal, "")
            ReceptorDTS.Rows.Add(rowReceptor)

            'Data table del Conceptos
            Dim ConceptosDTS As DataTable = New DataTable("ConceptosDTS")
            ConceptosDTS.Columns.Add(New DataColumn("Cantidad"))
            ConceptosDTS.Columns.Add(New DataColumn("Unidad"))
            ConceptosDTS.Columns.Add(New DataColumn("Descripcion"))
            ConceptosDTS.Columns.Add(New DataColumn("ValorUnitario"))
            ConceptosDTS.Columns.Add(New DataColumn("CuentaPredial"))
            ConceptosDTS.Columns.Add(New DataColumn("Id"))
            ConceptosDTS.Columns.Add(New DataColumn("Importe"))
            ConceptosDTS.Columns.Add(New DataColumn("Descuento"))
            ConceptosDTS.Columns.Add(New DataColumn("ClaveProdServ"))
            ConceptosDTS.Columns.Add(New DataColumn("ClaveUnidad"))
            Dim formatoPredial As Boolean = False

            For Each c As Conceptos In datos.Conceptos_
                Dim rowConceptos As DataRow = ConceptosDTS.NewRow()
                rowConceptos("Cantidad") = c.Cantidad
                'rowConceptos("ClaveProdServ") = c.ClaveProdServ
                rowConceptos("Unidad") = c.ClaveUnidad
                rowConceptos("Descripcion") = c.Descripcion
                rowConceptos("ValorUnitario") = c.ValorUnitario
                If c.CuentaPredial = String.Empty Then
                    rowConceptos("CuentaPredial") = ""
                Else
                    rowConceptos("CuentaPredial") = c.CuentaPredial
                    formatoPredial = True
                End If
                rowConceptos("Id") = c.Id
                rowConceptos("Importe") = c.Importe
                rowConceptos("Descuento") = c.Descuento
                rowConceptos("ClaveProdServ") = c.ClaveProdServ
                rowConceptos("ClaveUnidad") = c.ClaveUnidad
                ConceptosDTS.Rows.Add(rowConceptos)
            Next

            rpt.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local
            rpt.LocalReport.DisplayName = "Recibo"
            If estado = "P" Then
                rpt.LocalReport.ReportPath = "Reportes/ReciboPredialRpt33.rdlc"
            Else
                rpt.LocalReport.ReportPath = "Reportes/cReciboPredialRpt33.rdlc"
            End If

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
            Dim nombreMun As DataTable = New DAL().cParametroSistema("NOMBRE_MUNICIPIO")
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
                                   Select lv1.Attribute("Sello").Value).First().ToString

            Dim noCertificado As String = (From lv1 In xDoc.Descendants(df + "Comprobante")
                                           Select lv1.Attribute("NoCertificado").Value).First().ToString

            Dim fechaEmision As String = (From lv1 In xDoc.Descendants(df + "Comprobante")
                                          Select lv1.Attribute("Fecha").Value).First().ToString


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
            row("MetodoDePago") = datos.Comprobante_.MetodoDePago
            ''INICIA cambia el metodo de pago DE CLAVE A DESCRIPCION
            'Select Case datos.Comprobante_.FormaDePago
            '    Case "01"
            '        row("FormaDePago") = "01-EFECTIVO"
            '    Case "02"
            '        row("FormaDePago") = "02-CHEQUE NOMINATIVO"
            '    Case "03"
            '        row("MetodoDePago") = "03-TRANSFERENCIA ELECTRÓNICA DE FONDOS"
            '    Case "04"
            '        row("MetodoDePago") = "04-TARJETA DE CRÉDITO"
            '    Case "05"
            '        row("MetodoDePago") = "05-MONEDERO ELECTRÓNICO"
            '    Case "06"
            '        row("MetodoDePago") = "06-DINERO ELECTRÓNICO"
            '    Case "08"
            '        row("MetodoDePago") = "08-VALES DE DESPENSA"
            '    Case "28"
            '        row("MetodoDePago") = "28-TARJETA DE DÉBITO"
            '    Case "29"
            '        row("MetodoDePago") = "29-TARJETA DE SERVICIO"
            '    Case "99"
            '        row("MetodoDePago") = "99-OTROS"
            '    Case Else
            '        row("MetodoDePago") = datos.Comprobante_.MetodoDePago
            'End Select

            'row("Fecha") = "MÉXICO " + fechaEmision  'FechaHora.ToString("dd/MM/yyyy")
            row("Fecha") = nombreMun(0)(0).ToString() + " ,MOR. " + fechaEmision
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
            rowFolioFiscal("NoSerieCertificadoSAT") = elementTimbreFiscalDigital.Attribute("NoCertificadoSAT").Value
            rowFolioFiscal("SelloSAT") = elementTimbreFiscalDigital.Attribute("SelloSAT").Value
            rowFolioFiscal("CadenaOriginalSAT") = elementTimbreFiscalDigital.Attribute("SelloCFD").Value
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
            rowReceptor("Domicilio") = datos.Receptor_.Domicilio
            'datos.Receptor_.Calle &
            '                            IIf(datos.Receptor_.NoExterior.Length > 0, " No. EXT." & datos.Receptor_.NoExterior, "") &
            '                            IIf(datos.Receptor_.NoInterior.Length > 0, " No. INT." & datos.Receptor_.NoInterior, "") &
            '                            IIf(datos.Receptor_.Colonia.Length > 0, " COL." & datos.Receptor_.Colonia, "") &
            '                            IIf(datos.Receptor_.Municipio.Length > 0, ", " & datos.Receptor_.Municipio, "") &
            '                            IIf(datos.Receptor_.Estado.Length > 0, " " & datos.Receptor_.Estado, "") &
            '                            IIf(datos.Receptor_.Pais.Length > 0, " " & datos.Receptor_.Pais, "") &
            '                            IIf(datos.Receptor_.CodigoPostal.Length > 0, " CP." & datos.Receptor_.CodigoPostal, "")
            ReceptorDTS.Rows.Add(rowReceptor)

            'Data table del Conceptos
            Dim ConceptosDTS As DataTable = New DataTable("ConceptosDTS")
            ConceptosDTS.Columns.Add(New DataColumn("Cantidad"))
            ConceptosDTS.Columns.Add(New DataColumn("Unidad"))
            ConceptosDTS.Columns.Add(New DataColumn("Descripcion"))
            ConceptosDTS.Columns.Add(New DataColumn("ValorUnitario"))
            ConceptosDTS.Columns.Add(New DataColumn("CuentaPredial"))
            ConceptosDTS.Columns.Add(New DataColumn("Id"))
            ConceptosDTS.Columns.Add(New DataColumn("Importe"))
            ConceptosDTS.Columns.Add(New DataColumn("Descuento"))
            ConceptosDTS.Columns.Add(New DataColumn("ClaveProdServ"))
            ConceptosDTS.Columns.Add(New DataColumn("ClaveUnidad"))

            If datos.Conceptos_ IsNot Nothing Then
                For Each c As Conceptos In datos.Conceptos_
                    Dim rowConceptos As DataRow = ConceptosDTS.NewRow()
                    rowConceptos("Cantidad") = c.Cantidad
                    'rowConceptos("ClaveProdServ") = c.ClaveProdServ
                    rowConceptos("Unidad") = c.ClaveUnidad
                    rowConceptos("Descripcion") = c.Descripcion
                    rowConceptos("ValorUnitario") = c.ValorUnitario
                    rowConceptos("CuentaPredial") = c.CuentaPredial
                    rowConceptos("Id") = c.Id
                    rowConceptos("Importe") = c.Importe
                    rowConceptos("Descuento") = c.Descuento
                    rowConceptos("ClaveProdServ") = c.ClaveProdServ
                    rowConceptos("ClaveUnidad") = c.ClaveUnidad
                    ConceptosDTS.Rows.Add(rowConceptos)
                Next
            End If

            rpt.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local
            rpt.LocalReport.DisplayName = "Factura"

            ' factura predial
            rpt.LocalReport.ReportPath = "Reportes/FacturaPredialRpt33.rdlc"
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

    Public Function timbraFactura(ByVal strXML As String, ByRef Errores As String, anioFolio As String, ByRef Productivo As Boolean, ByRef usuarioFactura As String, ByRef passwordFactura As String)
        Dim respuestaCadena As String = String.Empty
        'http://appfacturainteligente.com/WSTimbrado33Test/WSCFDI33.svc
        Dim proveedorFactura As String = ConfigurationManager.AppSettings("proveedorFactura").ToString

        If Productivo Then
            '-----web service timbre PRODUCCION
            If proveedorFactura = "FoliosDigitales" Then
                Dim ServicioFD As appFoliosDigitales.WSCFDI33Client = New appFoliosDigitales.WSCFDI33Client()
                Dim respuesta As New appFoliosDigitales.RespuestaTFD33
                respuesta = ServicioFD.TimbrarCFDI(usuarioFactura, passwordFactura, strXML, anioFolio)
                If respuesta.OperacionExitosa Then
                    respuestaCadena = respuesta.XMLResultado
                    Return respuestaCadena
                Else
                    Errores = "Error Timbrado:-----" & respuesta.MensajeError & "-----" & respuesta.MensajeErrorDetallado
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
            Else
                Dim ServicioFI As appfacturainteligente.WSCFDI33Client = New appfacturainteligente.WSCFDI33Client()
                Dim Respuesta As appfacturainteligente.RespuestaTFD33
                Respuesta = ServicioFI.TimbrarCFDI(usuarioFactura, passwordFactura, strXML, anioFolio)
                If Respuesta.MensajeError = "Éxito." Then
                    Return Respuesta.XMLResultado
                Else
                    Errores = "Error Timbrado:-----" & Respuesta.MensajeError & "-----" & Respuesta.MensajeErrorDetallado
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
            End If

        Else
            '-----web service timbre de PRUEBAS  
            'Respuesta = ServicioFI.TimbrarCFDI(usuarioFactura, passwordFactura, strXML, anioFolio)
            Return ""
        End If
    End Function

    Private Function cancelaTimbraFactura(ByVal uuid As String, ByVal recibo As String, ByVal RFCReceptor As String, ByVal Total As String, ByRef Errores As String, ByRef byteCertificado As Byte(), ByRef byteLLavePrivada As Byte(),
                                          ByRef KeyPass As String, ByRef usuario As String, ByRef password As String, ByRef RFCEmisor As String)
        Dim proveedorFactura As String = ConfigurationManager.AppSettings("proveedorFactura").ToString
        Dim path As String = ConfigurationManager.AppSettings("RutaRecibos") 'HttpContext.Current.Server.MapPath("~/")

        Dim pkcs12Pfx As Pkcs12Pfx = New Pkcs12Pfx
        Dim generarBytePKS12 As Byte() = Nothing
        generarBytePKS12 = pkcs12Pfx.Generar(byteLLavePrivada, byteCertificado, KeyPass)
        'Dim listaCFDI As List(Of String) = New List(Of String)
        Dim listaCFDI(1) As String
        listaCFDI(0) = uuid
        Dim RespuestaString(2) As String
        If proveedorFactura = "FoliosDigitales" Then
            Dim client As New appFoliosDigitales.WSCFDI33Client()
            Dim respuesta As New appFoliosDigitales.RespuestaCancelacion
            Dim listaCFDICancelacion As New List(Of appFoliosDigitales.DetalleCFDICancelacion)
            Dim CFDICancelacion As New appFoliosDigitales.DetalleCFDICancelacion
            CFDICancelacion.UUID = uuid
            CFDICancelacion.Total = Total
            CFDICancelacion.RFCReceptor = RFCReceptor
            listaCFDICancelacion.Add(CFDICancelacion)
            respuesta = client.CancelarCFDIConValidacion(usuario, password, RFCEmisor, listaCFDICancelacion.ToArray(), Convert.ToBase64String(generarBytePKS12), KeyPass)
            If respuesta.OperacionExitosa Then
                RespuestaString(0) = String.Empty
                RespuestaString(1) = respuesta.DetallesCancelacion(0).CodigoResultado & "-" & respuesta.DetallesCancelacion(0).MensajeResultado & "-" & respuesta.DetallesCancelacion(0).UUID
                RespuestaString(2) = respuesta.XMLAcuse
                Return RespuestaString
            Else
                Errores = "Error de cancelacion de timbrado Timbrado:-----Recibo: " & recibo & "-----" & respuesta.MensajeError & "-----" & respuesta.MensajeErrorDetallado
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
        Else
            Dim ServicioFI As New appfacturainteligente.WSCFDI33Client
            Dim Respuesta As appfacturainteligente.RespuestaCancelacion
            Dim listaCFDICancelacion As New List(Of appfacturainteligente.DetalleCFDICancelacion)
            Dim CFDICancelacion As New appfacturainteligente.DetalleCFDICancelacion
            CFDICancelacion.UUID = uuid
            CFDICancelacion.Total = Total
            CFDICancelacion.RFCReceptor = RFCReceptor
            listaCFDICancelacion.Add(CFDICancelacion)
            Respuesta = ServicioFI.CancelarCFDIConValidacion(usuario, password, RFCEmisor, listaCFDICancelacion.ToArray(), Convert.ToBase64String(generarBytePKS12), KeyPass)
            If Respuesta.OperacionExitosa Then
                RespuestaString(0) = String.Empty
                RespuestaString(1) = Respuesta.DetallesCancelacion(0).CodigoResultado & "-" & Respuesta.DetallesCancelacion(0).MensajeResultado & "-" & Respuesta.DetallesCancelacion(0).UUID
                RespuestaString(2) = Respuesta.XMLAcuse
                Return RespuestaString
            Else
                Errores = "Error de cancelacion de timbrado Timbrado:-----Recibo: " & recibo & "-----" & Respuesta.MensajeError & "-----" & Respuesta.MensajeErrorDetallado
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

    Public Function ConsultarCreditos() As DataTable
        Dim proveedorFactura As String = ConfigurationManager.AppSettings("proveedorFactura").ToString
        Dim usuarioFactura As String = ConfigurationManager.AppSettings("usuarioFactura").ToString
        Dim passwordFactura As String = ConfigurationManager.AppSettings("passwordFactura").ToString
        'Dim datos(6) As String
        Dim table As New DataTable
        table.Columns.Add("numPaquete", GetType(Integer))
        table.Columns.Add("fechaActivacion", GetType(DateTime))
        table.Columns.Add("fechaVencimiento", GetType(DateTime))
        table.Columns.Add("Paquete", GetType(String))
        table.Columns.Add("Timbres", GetType(Integer))
        table.Columns.Add("TimbresRestantes", GetType(Integer))
        table.Columns.Add("TimbresUsados", GetType(Integer))
        table.Columns.Add("Vigente", GetType(String))

        If (proveedorFactura = "FoliosDigitales") Then
            Dim ServicioFD As appFoliosDigitales.WSCFDI33Client = New appFoliosDigitales.WSCFDI33Client()
            Dim respuesta As New appFoliosDigitales.RespuestaCreditos
            respuesta = ServicioFD.ConsultarCreditos(usuarioFactura, passwordFactura)


            For i As Integer = 0 To respuesta.Paquetes.Count() - 1
                table.Rows.Add(i + 1, respuesta.Paquetes(i).FechaActivacion, respuesta.Paquetes(i).FechaVencimiento,
                               respuesta.Paquetes(i).Paquete, respuesta.Paquetes(i).Timbres, respuesta.Paquetes(i).TimbresRestantes,
                               respuesta.Paquetes(i).TimbresUsados, IIf(respuesta.Paquetes(i).Vigente = True, "SI", "NO"))
            Next
            'For Each paquete In respuesta.Paquetes
            '    If paquete.EnUso = True Then
            '        datos(0) = paquete.FechaActivacion
            '        datos(1) = paquete.FechaVencimiento
            '        datos(2) = paquete.Paquete
            '        datos(3) = paquete.Timbres
            '        datos(4) = paquete.TimbresRestantes
            '        datos(5) = paquete.TimbresUsados
            '        datos(6) = IIf(paquete.Vigente = True, "SI", "NO")
            '    End If
            'Next
            'Dim c As Integer = respuesta.Paquetes.Count()
            'datos(0) = respuesta.Paquetes(c - 1).FechaActivacion
            'datos(1) = respuesta.Paquetes(c - 1).FechaVencimiento
            'datos(2) = respuesta.Paquetes(c - 1).Paquete
            'datos(3) = respuesta.Paquetes(c - 1).Timbres
            'datos(4) = respuesta.Paquetes(c - 1).TimbresRestantes
            'datos(5) = respuesta.Paquetes(c - 1).TimbresUsados
            'datos(6) = IIf(respuesta.Paquetes(c - 1).Vigente = True, "SI", "NO")
        Else
            Dim ServicioFD As appfacturainteligente.WSCFDI33Client = New appfacturainteligente.WSCFDI33Client()
            Dim respuesta As New appfacturainteligente.RespuestaCreditos
            respuesta = ServicioFD.ConsultarCreditos(usuarioFactura, passwordFactura)
            For i As Integer = 0 To respuesta.Paquetes.Count() - 1
                table.Rows.Add(i + 1, respuesta.Paquetes(i).FechaActivacion, respuesta.Paquetes(i).FechaVencimiento,
                               respuesta.Paquetes(i).Paquete, respuesta.Paquetes(i).Timbres, respuesta.Paquetes(i).TimbresRestantes,
                               respuesta.Paquetes(i).TimbresUsados, IIf(respuesta.Paquetes(i).Vigente = True, "SI", "NO"))
            Next
            'Dim c As Integer = respuesta.Paquetes.Count()
            'datos(0) = respuesta.Paquetes(c - 1).FechaActivacion
            'datos(1) = respuesta.Paquetes(c - 1).FechaVencimiento
            'datos(2) = respuesta.Paquetes(c - 1).Paquete
            'datos(3) = respuesta.Paquetes(c - 1).Timbres
            'datos(4) = respuesta.Paquetes(c - 1).TimbresRestantes
            'datos(5) = respuesta.Paquetes(c - 1).TimbresUsados
            'datos(6) = IIf(respuesta.Paquetes(c - 1).Vigente = True, "SI", "NO")
        End If
        Return table
    End Function

    Public Function cambioTipoPago(ByVal rutaXML As String, ByVal formapago As String) As Boolean
        Try
            Dim path As String = ConfigurationManager.AppSettings("RutaRecibos")
            Dim xDoc As XDocument = XDocument.Load(path + rutaXML)
            Dim df As XNamespace = xDoc.Root.Name.NamespaceName
            Dim elementComprobante = xDoc.Elements(df + "Comprobante").Single
            elementComprobante.Attribute("FormaPago").Value = formapago
            xDoc.Save(path + rutaXML)
            Return 1
        Catch ex As Exception
            Return 0
        End Try

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
