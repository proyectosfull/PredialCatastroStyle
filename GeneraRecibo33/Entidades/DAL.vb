Imports System.Data.SqlClient
Imports System.Web

Public Class DAL

    Dim conStr As String = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("connectionString").ToString()



    Public Function cFIELGetByConstraint(id As Integer) As DataTable
        Dim cmd As SqlCommand = New SqlCommand("SELECT  Id, KeyFile, CerFile, KeyPass, RFC, Calle, Municipio, Estado, Pais, CodigoPostal, Nombre, NoExterior, NoInterior, Colonia, Localidad, Referencia, Logo FROM cFIEL where id= @id;")
        cmd.CommandType = CommandType.Text
        cmd.Parameters.Add(New SqlParameter("@id", id))
        Return ejecuta_comandoDataTable(cmd)
    End Function

    Public Function cFIELGetByActive() As DataTable
        'Create a Connection object.

        Dim cmd As SqlCommand = New SqlCommand("SELECT  Id, KeyFile, CerFile, KeyPass, RFC, Calle, Municipio, Estado, Pais, CodigoPostal, Nombre, NoExterior, NoInterior, Colonia, Localidad, Referencia, Logo FROM cFIEL where Activo=1;")
        'cmd.CommandType = CommandType.Text
        Return ejecuta_comandoDataTable(cmd)
    End Function

    Public Function tReciboGetByConstraint(id As Integer) As DataTable
        Dim cmd As SqlCommand = New SqlCommand("SELECT R.Id, FechaPago, Ruta, RutaFactura, CodigoSeguridad,M.Descripcion as Mesa,U.Usuario AS Cajero ,R.Contribuyente,R.Domicilio,R.DatosPredio,R.Observaciones FROM tRecibo AS R INNER JOIN cMesa AS M ON M.Id=R.IdMesaCobro INNER JOIN cUsuarios AS U ON U.Id=R.IdUsuarioCobra where R.id= @id;")
        cmd.CommandType = CommandType.Text
        cmd.Parameters.Add(New SqlParameter("@id", id))
        Return ejecuta_comandoDataTable(cmd)
    End Function

    Public Function cRfGetByRfc(rfc As String) As DataTable
        Dim cmd As SqlCommand = New SqlCommand("SELECT  Id, RFC, Nombre, Calle, Municipio, Estado, Pais, CodigoPostal, NoExterior, NoInterior, Colonia, Localidad, Referencia, Email FROM cRFC where RFC=@rfc;")
        cmd.CommandType = CommandType.Text
        cmd.Parameters.Add(New SqlParameter("@rfc", rfc))
        Return ejecuta_comandoDataTable(cmd)
    End Function

    Public Function cParametroSistema(clave As String) As DataTable
        Dim cmd As SqlCommand = New SqlCommand("SELECT Valor FROM cParametroSistema where Clave =@clave and activo=1; ;")
        cmd.CommandType = CommandType.Text
        cmd.Parameters.Add(New SqlParameter("@clave", clave))
        Return ejecuta_comandoDataTable(cmd)
    End Function

    Public Function tInfraccionGetById(id As Integer) As DataTable
        Dim cmd As SqlCommand = New SqlCommand("SELECT cBlock.Nombre + '-'+ cast(tInfraccion.Folio as varchar(5)) as FolioInfraccion FROM tInfraccion INNER JOIN cBlock ON tInfraccion.IdBlock = cBlock.Id WHERE tInfraccion.Id =@id;")
        cmd.CommandType = CommandType.Text
        cmd.Parameters.Add(New SqlParameter("@id", id))
        Return ejecuta_comandoDataTable(cmd)
    End Function



    Private Function ejecuta_comandoDataTable(cmd As SqlCommand) As DataTable
        Dim dt As DataTable = New DataTable()
        Dim cadenaConexion As String = conStr
        Dim con As SqlConnection = New SqlConnection(cadenaConexion)
        Dim adapter As SqlDataAdapter = New SqlDataAdapter()
        cmd.Connection = con
        Try
            con.Open()
            adapter.SelectCommand = cmd
            adapter.Fill(dt)
            con.Close()
            con.Dispose()
        Catch ex As Exception
            con.Close()
            con.Dispose()
            Dim path_Name_File As String = HttpContext.Current.Server.MapPath("~/Log/" & String.Format("{0:yyyy_MM_dd}", Now) & ".txt")
            Dim file As New System.IO.StreamWriter(path_Name_File, True)
            file.WriteLine("**************************")
            file.WriteLine("Fecha Hora del Error: " & Now.ToString)
            file.WriteLine(ex.ToString)
            file.WriteLine("**************************")
            file.Close()
        End Try
        Return dt
    End Function

    Public Function acturalizaFacturaCancelada(recibo As Object, UUID As String, codigo As String, xmlFactura As String, xmlCancelacion As String, fecha As DateTime, usuarioCancela As String) As Boolean
        Dim con As SqlConnection = New SqlConnection(conStr)
        Dim tran As SqlTransaction = Nothing
        Dim result As Boolean = True

        Try
            con.Open()
            tran = con.BeginTransaction()
            Dim cmd As SqlCommand = New SqlCommand("UPDATE tRecibo SET RutaFactura = '', Facturado=0, FechaFactura= null WHERE Id = @Id", con, tran)

            cmd.Parameters.AddWithValue("@Id", recibo.Id)
            Dim r As Integer = cmd.ExecuteNonQuery()

            If (r = 1) Then
                Dim cmd_ins As SqlCommand = New SqlCommand("INSERT INTO tFacturaCancelacion(UUID,folioRecibo,codigo,xmlDataFactura,xmlDataCancelacion,fechaCancelacion,fecha,usuarioCancela)" +
                                                           "VALUES(@UUID,@folioRecibo,@codigo,@xmlFactura,@xmlDataCancelacion,@fechaCancelacion,@fecha,@usuarioCancela)", con, tran)
                cmd_ins.Parameters.AddWithValue("@UUID", UUID)
                cmd_ins.Parameters.AddWithValue("@folioRecibo", recibo.Id)
                cmd_ins.Parameters.AddWithValue("@codigo", codigo)
                cmd_ins.Parameters.AddWithValue("@xmlFactura", xmlFactura)
                cmd_ins.Parameters.AddWithValue("@xmlDataCancelacion", xmlCancelacion)
                cmd_ins.Parameters.AddWithValue("@fechaCancelacion", fecha)
                cmd_ins.Parameters.AddWithValue("@fecha", fecha)
                cmd_ins.Parameters.AddWithValue("@usuarioCancela", usuarioCancela)

                r = cmd_ins.ExecuteNonQuery()

                If (r = 1) Then
                    tran.Commit()
                Else
                    result = False
                    tran.Rollback()
                End If
                cmd_ins.Dispose()
            Else
                result = False
                tran.Rollback()
            End If
            cmd.Dispose()
        Catch ex As Exception
            result = False

            If tran.Connection IsNot Nothing Then
                tran.Rollback()
            End If

            Dim path_Name_File As String = HttpContext.Current.Server.MapPath("~/Log/" & String.Format("{0:yyyy_MM_dd}", Now) & ".txt")
            Dim file As New System.IO.StreamWriter(path_Name_File, True)
            file.WriteLine("**************************")
            file.WriteLine("Fecha Hora del Error: " & Now.ToString)
            file.WriteLine(ex.Message)
            file.WriteLine("**************************")
            file.Close()
        Finally
            tran.Dispose()
            con.Close()
            con.Dispose()
        End Try
        Return result
    End Function

End Class
