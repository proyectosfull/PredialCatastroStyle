using System;
using System.Data;
using System.Reflection;
using System.Text;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net.Mime;
using Clases.BL;
using System.IO;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace Clases.Utilerias
{

    /// <summary>
    /// Summary description for Utilerias
    /// </summary>
    public class Utileria
    {
        private string PasswordHash = "MAKV2SPBNIymAq6ux2Dm0rhB099212anrtC7gPjyKtA7cwftqeois5643jhgfd7802poiwqj9o5qe6PxSiNC1cdQDQKJHDNXE";
        private string SaltKey = "S@LT&KEY&";
        private string VIKey = "@1B2c3D4e5F6g7H8T";

        /// <summary>
        /// Obtiene la descripcion de un valor de una enumeración
        /// </summary>
        /// <param name="value">Recibe el valor de una enumeración</param>
        /// <returns>Regresa un string con la descripcion correspondiente al valor enumerado</returns>
        public string GetDescription(MensajesInterfaz value)
        {
            string description = string.Empty;

            try
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                description = attributes.Length > 0 ? attributes[0].Description : value.ToString();

            }
            catch { }

            return description;
        }

        public string GetDescription(string description)
        {
            // string description = string.Empty;
            return description;
        }

        //public void logError(string ObjetoMetodo, string error)
        //{
        //    PredialEntities Predial = new PredialEntities();
        //    cError e = new cError();
        //    e.Funcion = ObjetoMetodo;
        //    e.Error = error;
        //    e.Fecha = DateTime.Now;
        //    e.IdUsuario = 1;
        //    Predial.cError.Add(e);
        //    Predial.SaveChanges();
        //    //sendEMailThroughHotMail(ObjetoMetodo + " <br/>" + error);
        //}

        public void logError(string ObjetoMetodo, DbUpdateException error, string campos = "")
        {
            PredialEntities Predial = new PredialEntities();
            cError e = new cError();
            e.Funcion = ObjetoMetodo;
            StringBuilder sb = new StringBuilder();
            if (error.Message != null)
            {
                sb.AppendLine("Message=> " + error.Message.ToString());
            }
            if (error.InnerException != null)
            {
                sb.AppendLine(" <------------> ");
                sb.AppendLine("InnerException=> " + error.InnerException.ToString());
            }
            if (error.GetType() != null)
            {
                sb.AppendLine(" <------------> ");
                sb.AppendLine("GetType=> " + error.GetType().Name.ToString());
            }
            if (error.StackTrace != null)
            {
                sb.AppendLine(" <------------> ");
                sb.AppendLine("StackTrace=> " + error.StackTrace.ToString());
            }
            sb.AppendLine(" <------------> ");
            sb.AppendLine("Campos Filtros:" + campos);
            sb.AppendLine(" <------------> ");
            sb.AppendLine(" Exception: " + error.ToString());
            e.Error = sb.ToString();

            e.Fecha = DateTime.Now;
            e.IdUsuario = 1;
            Predial.cError.Add(e);
            Predial.SaveChanges();
            //sendEMailThroughHotMail(ObjetoMetodo + " <br/>" + error);
        }
        public void logError(string ObjetoMetodo, DataException error, string campos = "")
        {
            PredialEntities Predial = new PredialEntities();
            cError e = new cError();
            e.Funcion = ObjetoMetodo;
            StringBuilder sb = new StringBuilder();
            if (error.Message != null)
            {
                sb.AppendLine("Message=> " + error.Message.ToString());
            }
            if (error.InnerException != null)
            {
                sb.AppendLine(" <------------> ");
                sb.AppendLine("InnerException=> " + error.InnerException.ToString());
            }
            if (error.GetType() != null)
            {
                sb.AppendLine(" <------------> ");
                sb.AppendLine("GetType=> " + error.GetType().Name.ToString());
            }
            if (error.StackTrace != null)
            {
                sb.AppendLine(" <------------> ");
                sb.AppendLine("StackTrace=> " + error.StackTrace.ToString());
            }
            sb.AppendLine(" <------------> ");
            sb.AppendLine("Campos Filtros:" + campos);
            sb.AppendLine(" <------------> ");
            sb.AppendLine(" Exception: " + error.ToString());
            e.Error = sb.ToString();

            e.Fecha = DateTime.Now;
            e.IdUsuario = 1;
            Predial.cError.Add(e);
            Predial.SaveChanges();
            //sendEMailThroughHotMail(ObjetoMetodo + " <br/>" + error);
        }
        public void logError(string ObjetoMetodo, Exception error, string campos = "")
        {
            PredialEntities Predial = new PredialEntities();
            cError e = new cError();
            e.Funcion = ObjetoMetodo;
            StringBuilder sb = new StringBuilder();
            if (error.Message != null)
            {
                sb.AppendLine("Message=> " + error.Message.ToString());
            }
            if (error.InnerException != null)
            {
                sb.AppendLine(" <------------> ");
                sb.AppendLine("InnerException=> " + error.InnerException.ToString());
            }
            if (error.GetType() != null)
            {
                sb.AppendLine(" <------------> ");
                sb.AppendLine("GetType=> " + error.GetType().Name.ToString());
            }
            if (error.StackTrace != null)
            {
                sb.AppendLine(" <------------> ");
                sb.AppendLine("StackTrace=> " + error.StackTrace.ToString());
            }
            sb.AppendLine(" <------------> ");
            sb.AppendLine("Campos Filtros:" + campos);
            sb.AppendLine(" <------------> ");
            sb.AppendLine(" Exception: " + error.ToString());
            e.Error = sb.ToString();

            e.Fecha = DateTime.Now;
            e.IdUsuario = 1;
            Predial.cError.Add(e);
            Predial.SaveChanges();
            //sendEMailThroughHotMail(ObjetoMetodo + " <br/>" + error);
        }

        public string GetSHA1(string str)
        {
            SHA1 sha1 = SHA1Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha1.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        //method to send email to HOTMAIL
        public MensajesInterfaz sendEMail(string destinatario, string asunto, string htmlBody, MemoryStream XML, MemoryStream PDF)
        {
            MensajesInterfaz correo;
            try
            {
                if (destinatario.Length > 0)
                {
                    List<cParametroSistema> parametros = new cParametroSistemaBL().GetAll();
                    string SMTP = parametros.Find(c => c.Clave == "SMTP").Valor;
                    string REMITENTE = parametros.Find(c => c.Clave == "REMITENTE").Valor;
                    string CONTRASENIA_REMITENTE = parametros.Find(c => c.Clave == "CONTRASENIA_REMITENTE").Valor;
                    Int32 SMTP_PUERTO = Convert.ToInt32(parametros.Find(c => c.Clave == "SMTP_PUERTO").Valor);
                    bool SMTP_SSL = parametros.Find(c => c.Clave == "SMTP_SSL").Valor == "SI" ? true : false;

                    SmtpClient SmtpServer = new SmtpClient(SMTP);
                    var mail = new MailMessage();
                    mail.From = new MailAddress(REMITENTE);
                    mail.To.Add(destinatario);
                    mail.Subject = asunto;
                    mail.IsBodyHtml = true;
                    mail.Body = htmlBody;
                    //message.Body = mensaje;
                    if (XML != null)
                    {
                        Attachment xml_ = new Attachment(XML, "XML.xml", MediaTypeNames.Text.Xml);
                        mail.Attachments.Add(xml_);
                    }
                    if (PDF != null)
                    {
                        Attachment pdf_ = new Attachment(PDF, "PDF.pdf", MediaTypeNames.Application.Pdf);
                        mail.Attachments.Add(pdf_);
                    }
                    SmtpServer.Port = SMTP_PUERTO;
                    SmtpServer.UseDefaultCredentials = false;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(REMITENTE, CONTRASENIA_REMITENTE);
                    SmtpServer.EnableSsl = SMTP_SSL;
                    SmtpServer.Timeout = 30000;
                    SmtpServer.Send(mail);
                    correo = MensajesInterfaz.FacturaCorreo;
                }
                else
                {
                    correo = MensajesInterfaz.FacturaCorreoError;
                }
            }//end of try block
            catch (Exception ex)
            {
                new Utileria().logError("Error en envio de Correo:", ex);
                correo = MensajesInterfaz.FacturaCorreoError;
            }//end of catch
            return correo;
        }//end of Email Method 

        public string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        public string Decrypt(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }

        /// <summary>
        /// Compara dos objetos para ver si hubo cambios
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Object1"></param>
        /// <param name="object2"></param>
        public static void Compare<T>(T valNuevo, T valViejo)
        {
            StringBuilder resultado = new StringBuilder();
            int id = 0;
            int idPredio = 0;
            //int idUsuario = 0;
            //Get the type of the object
            Type type = typeof(T);

            //return false if any of the object is false
            if (valNuevo == null || valViejo == null)
                return;

            //Loop through each properties inside class and get values for the property from both the objects and compare
            foreach (System.Reflection.PropertyInfo property in type.GetProperties())
            {
                if (property.Name != "ExtensionData")
                {
                    string valViejoStr = string.Empty;
                    string valNuevoStr = string.Empty;
                    if (type.GetProperty(property.Name).GetValue(valNuevo, null) != null)
                        valViejoStr = type.GetProperty(property.Name).GetValue(valNuevo, null).ToString();
                    if (type.GetProperty(property.Name).GetValue(valViejo, null) != null)
                        valNuevoStr = type.GetProperty(property.Name).GetValue(valViejo, null).ToString();
                    if (property.Name.ToLower().Equals("id"))
                        id = int.Parse(valViejoStr);
                    if (property.Name.ToLower().Equals("idpredio"))
                        idPredio = int.Parse(valViejoStr);
                    //if (property.Name.ToLower().Equals("idusuario"))
                    //    idUsuario = int.Parse(valViejoStr);
                    if (valViejoStr.Trim() != valNuevoStr.Trim())
                    {
                        resultado.Append("" + property.Name + ": " + valNuevoStr + " => " + valViejoStr + "~ ");
                    }
                }
                //rompre el cliclo
                if (property.Name.ToLower().Equals("fechamodificacion"))
                {
                    break;
                }
            }
            tBitacora bit = new tBitacora();
            if (type.Name == "cPredio")
                bit.IdPredio = id;
            else if (type.Name == "tRecibo")
            {
                bit.IdPredio = 0;
            }
            else
                bit.IdPredio = idPredio;

            //bit.IdPredio = idPredio != 0 ? idPredio id:0;
            cUsuarios U = (cUsuarios)System.Web.HttpContext.Current.Session["usuario"];
            bit.IdUsuario = U.Id;
            bit.IdTabla = id;
            bit.NombreTabla = type.Name;
            bit.MaquinaIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
            bit.MaquinaNombre = System.Net.Dns.GetHostName().ToString();
            string ventana = System.Web.HttpContext.Current.Request.Path.ToUpper();
            bit.ventana = ventana.Substring(ventana.LastIndexOf('/') + 1);
            bit.Movimiento = resultado.ToString().ToUpper();
            bit.FechaModificacion = DateTime.Now;
            bit.Activo = true;
            PredialEntities Predial = new PredialEntities();
            Predial.tBitacora.Add(bit);
            Predial.SaveChanges();
        }

        public string RemoverCaracteresEspeciales(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || c == ',')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Redondeo valor tipo double
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static double Redondeo(double valor)
        {
            //int Numero = Convert.ToInt32(Math.Truncate(valor));
            //double Decimales = valor - Numero;
            //if (Decimales > 0.50)
            //{
            //    return Numero + 1;
            //}
            //else
            //{
            //    return Numero;
            //}
            double sinRedondeo = Math.Round(valor, 2);
            return sinRedondeo;
        }
        /// <summary>
        /// Redondeo valor tipo decimal
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static decimal Redondeo(decimal valor)
        {
            //int Numero = Convert.ToInt32(Math.Truncate(valor));
            //decimal Decimales = valor - Numero;
            //if (Decimales > Convert.ToDecimal(0.50))
            //{
            //    return Numero + 1;
            //}
            //else
            //{
            //    return Numero;
            //}
            decimal sinRedondeo = Math.Round(valor, 2);
            return sinRedondeo;
        }

        public DataTable ConcultaRegistro(string idTabla, string nombreTabla,string usuario,string claveCatastral)
        {
            DataTable dt = new DataTable("Registro");
            DataColumn Campo = new DataColumn("Campo");
            DataColumn Valor = new DataColumn("Valor");
            dt.Columns.Add(Campo);
            dt.Columns.Add(Valor);
            System.Data.DataRow row;

            string conn = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ToString();
            using (var con = new System.Data.SqlClient.SqlConnection(conn))
            {
                con.Open();
                System.Data.SqlClient.SqlCommand cmd = con.CreateCommand();               
                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter("SELECT * FROM " + nombreTabla + " where id=" + idTabla, conn);
                DataSet dtConsulta = new DataSet();
                adapter.Fill(dtConsulta, "dtConsulta");
                string nombre = string.Empty;
                string valor = string.Empty;
                string SQL = string.Empty;              
                foreach (System.Data.DataColumn c in dtConsulta.Tables[0].Columns)
                {
                    cmd.CommandText = string.Empty; ;
                    SQL = string.Empty;
                    nombre = string.Empty;
                    valor = string.Empty;
                    row = dt.NewRow();
                    nombre=c.ColumnName.ToUpper();
                    valor = dtConsulta.Tables[0].Rows[0][c.ColumnName].ToString();
                    row[0] = nombre;
                    if (nombre.Substring(0, 2) == "ID" && nombre.Length > 2)
                    {
                        switch (nombre)
                        {
                            case "IDUSUARIO":
                                row[1] = usuario;
                                break;
                            case "IDPREDIO":                               
                                    row[1] = claveCatastral;                                                              
                                break;
                            case "IDCOLONIA":                                
                                cmd.CommandText = (valor==string.Empty)?"":"Select NombreColonia from cColonia where ID=" + valor;
                                break;
                            case "IDCONTRIBUYENTE":
                                cmd.CommandText = (valor == string.Empty) ? "" : "Select Nombre + ' ' + ApellidoPaterno + ' ' + ApellidoMaterno from cContribuyente where ID=" + valor;
                                break;
                            case "IDUSOSUELO":
                                cmd.CommandText = (valor == string.Empty) ? "" : "Select  Clave + ' - ' +Descripcion  from cUsoSuelo where ID=" + valor;
                                break;
                            case "IDEXENTOPAGO":
                                cmd.CommandText = (valor == string.Empty) ? "" : "Select Descripcion from cExentoPago where ID=" + valor;
                                break;
                            case "IDSTATUSPREDIO":
                                cmd.CommandText = (valor == string.Empty) ? "" : "Select Descripcion from cStatusPredio where ID=" + valor;
                                break;
                            case "IDTIPOPREDIO":
                                cmd.CommandText = (valor == string.Empty) ? "" : "Select Descripcion from cTipoPredio where ID=" + valor;
                                break;
                            case "IDTIPOFASEIP":
                                cmd.CommandText = (valor == string.Empty) ? "" : "Select Descripcion from cTipoFase where ID=" + valor;
                                break;
                            case "IDTIPOFASESM":
                                cmd.CommandText = (valor == string.Empty) ? "" : "Select Descripcion from cTipoFase where ID=" + valor;
                                break;
                            case "IDTIPOMOVAVALUO":
                                cmd.CommandText = (valor == string.Empty) ? "" : "Select Descripcion from cTipoMovAvaluo where ID=" + valor;
                                break;
                            case "IDTIPOINMUEBLE":
                                cmd.CommandText = (valor == string.Empty) ? "" : "Select Descripcion from cTipoInmueble where ID=" + valor;
                                break;
                            case "IDCONDOMINIO":
                                cmd.CommandText = (valor == string.Empty) ? "" : "Select Descripcion from cCondominio where ID=" + valor;
                                break;
                            case "IDCONVENIO":
                                cmd.CommandText = (valor == string.Empty) ? "" : "Select Folio from tConvenio where ID=" + valor;
                                break;
                            case "IDTIPOAVALUO":
                                cmd.CommandText = (valor == string.Empty) ? "" : "Select Descripcion from cTipoAvaluo where ID=" + valor;
                                break;
                            case "IDVALUADOR":
                                cmd.CommandText = (valor == string.Empty) ? "" : "Select Nombre + ' ' + ApellidoPaterno + ' ' + ApellidoMaterno from cValuador where ID=" + valor;
                                break;
                            case "IDTIPOTRAMITE":
                                cmd.CommandText = (valor == string.Empty) ? "" : "Select Descripcion from cTipoTramite where ID=" + valor;
                                break;
                            case "IDREQUERIMIENTO":
                                cmd.CommandText = (valor == string.Empty) ? "" : "Select Folio from tRequerimiento where ID=" + valor;
                                break;
                            case "IDCAJA":
                                cmd.CommandText = (valor == string.Empty) ? "" : "Select Caja from cCaja where ID=" + valor;
                                break;
                            case "IDUSUARIOCANCELA":
                                cmd.CommandText = (valor == string.Empty) ? "" : "Select Usuario from cUsuarios where ID=" + valor;
                                break;
                            case "IDUSUARIOCOBRA":
                                cmd.CommandText = (valor == string.Empty) ? "" : "Select Usuario from cUsuarios where ID=" + valor;
                                break;
                            case "IDDESCUENTO":
                                cmd.CommandText = (valor == string.Empty) ? "" : "Select Clave from cDescuento where ID=" + valor;
                                break;
                            case "IDTIPOPAGO":
                                cmd.CommandText = (valor == string.Empty) ? "" : "Select Nombre from cTipoPago where ID=" + valor;
                                break;
                            case "IDAGENTEFISCAL":
                                cmd.CommandText = (valor == string.Empty) ? "" : "Select Nombre + ' ' + ApellidoPaterno + ' ' + ApellidoMaterno from cAgenteFiscal where ID=" + valor;
                                break;
                            default:
                                {
                                    row[1] = valor;
                                    break;
                                }
                        }
                        if (cmd.CommandText != string.Empty)
                        {
                            using (System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    row[1] = reader.GetValue(0);
                                }
                            }
                        }
                    }
                    else
                    {
                        row[1] = valor;
                    }

                    dt.Rows.Add(row);
                }
            }

            return dt;
        }

        public string validaRFC(string RFC)
        {
            string respuesta = string.Empty;            
            if (RFC.Length == 13)
            {
                System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex("[A-Z&amp;Ñ]{4}[0-9]{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])[A-Z0-9]{2}[0-9A]");
                if ((rgx.IsMatch(RFC)))
                    respuesta = "CORRECTO";
                else
                    respuesta = "ESTRUCTURA DE RFC ERRONEA.";
            }
            else if (RFC.Length == 12)
            {
                System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex("[A-Z&amp;Ñ]{3}[0-9]{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])[A-Z0-9]{2}[0-9A]");
                if ((rgx.IsMatch(RFC)))
                    respuesta = "CORRECTO";
                else
                    respuesta = "ESTRUCTURA DE RFC ERRONEA.";
            }
            else
                respuesta = "EL RFC DEBE CONTENER 12 O 13 CARACTERES VALIDOS.";

            return respuesta;
        }

        public string strOk(string cadena)
        { //Valida un cadena vacia o null
            if (string.IsNullOrEmpty(cadena))
                return "";
            else
                return cadena;
        }


    }

}