using Catastro.ModelosFactura;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;

namespace SOAPT.Clases
{
    class Mail
    {

        //string From = ""; //de quien procede, puede ser un alias
        //string To;  //a quien vamos a enviar el mail
        //string Message;  //mensaje
        //string Subject; //asunto
        //List<string> Archivo = new List<string>(); //lista de archivos a enviar


        public Mail() { }

        /// <summary>
        /// Enviar correo HOTMAIL con archivos adjuntos.
        /// </summary>
        public ResultDAO enviarHotmail(string correoDestinatario, string mensaje, string asunto, List<string> archivos = null)
        {
            ResultDAO result = new ResultDAO();
            string correoEmisor = Constantes.correoEmail;
            string pwEmisor = Constantes.contraEmail;

            if (correoDestinatario.Trim().Equals("") || mensaje.Trim().Equals("") || asunto.Trim().Equals(""))
            {
                result.MESSAGE = "El correo, el asunto y el mensaje son obligatorios";
                result.SUCCESS = false;
            }

            try
            {
                MailMessage Email = new MailMessage(correoEmisor, correoDestinatario, asunto, mensaje);

                if (archivos != null)
                {
                    //agregado de archivo
                    foreach (string archivo in archivos)
                    {
                        //comprobamos si existe el archivo y lo agregamos a los adjuntos
                        if (System.IO.File.Exists(@archivo))
                            Email.Attachments.Add(new Attachment(@archivo, MediaTypeNames.Application.Octet));

                    }
                }

                Email.IsBodyHtml = true; //definimos si el contenido sera html
                Email.From = new MailAddress(correoEmisor, Constantes.nombreSistema, System.Text.Encoding.UTF8);//Correo de salida; //definimos la direccion de procedencia
                Email.Priority = MailPriority.Normal;

                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("smtp.office365.com");
                smtpMail.EnableSsl = true;//le definimos si es conexión ssl
                smtpMail.UseDefaultCredentials = false; //le decimos que no utilice la credencial por defecto
                smtpMail.Host = "smtp.office365.com"; //agregamos el servidor smtp
                smtpMail.Port = 587; //le asignamos el puerto, en este caso gmail utiliza el 465
                smtpMail.Credentials = new System.Net.NetworkCredential(correoEmisor, pwEmisor); //agregamos nuestro usuario y pass de gmail

                smtpMail.Send(Email);

                result.MESSAGE = "La factura fue enviada correctamente al correo del contribuyente.";
                result.SUCCESS = true;
            }
            catch (Exception ex)
            {
                result.MESSAGE = ex.Message;
                result.SUCCESS = false;
            }
            return result;
        }

        /// <summary>
        /// metodo que envia el mail
        /// </summary>
        /// <returns></returns>
        public bool enviaMailGmail()
        {

            return true;
            ////una validación básica
            //if (To.Trim().Equals("") || Message.Trim().Equals("") || Subject.Trim().Equals(""))
            //{
            //    error = "El correo, el asunto y el mensaje son obligatorios";
            //    return false;
            //}


            //try
            //{


            //    //creamos un objeto tipo MailMessage
            //    //este objeto recibe el sujeto o persona que envia el mail,
            //    //la direccion de procedencia, el asunto y el mensaje
            //    Email = new System.Net.Mail.MailMessage(From, To, Subject, Message);

            //    //si viene archivo a adjuntar
            //    //realizamos un recorrido por todos los adjuntos enviados en la lista
            //    //la lista se llena con direcciones fisicas, por ejemplo: c:/pato.txt
            //    if (Archivo != null)
            //    {
            //        //agregado de archivo
            //        foreach (string archivo in Archivo)
            //        {
            //            //comprobamos si existe el archivo y lo agregamos a los adjuntos
            //            if (System.IO.File.Exists(@archivo))
            //                Email.Attachments.Add(new Attachment(@archivo, MediaTypeNames.Application.Octet));

            //        }
            //    }

            //    Email.IsBodyHtml = true; //definimos si el contenido sera html
            //    Email.From = new MailAddress(From, Constantes.nombreSistema, System.Text.Encoding.UTF8);//Correo de salida; //definimos la direccion de procedencia
            //    Email.Priority = MailPriority.Normal;


            //    //aqui creamos un objeto tipo SmtpClient el cual recibe el servidor que utilizaremos como smtp
            //    //en este caso me colgare de gmail
            //    System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("smtp.gmail.com");

            //    smtpMail.EnableSsl = false;//le definimos si es conexión ssl
            //    smtpMail.UseDefaultCredentials = false; //le decimos que no utilice la credencial por defecto
            //    smtpMail.Host = "smtp.gmail.com"; //agregamos el servidor smtp
            //    smtpMail.Port = 587; //le asignamos el puerto, en este caso gmail utiliza el 465
            //    smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS); //agregamos nuestro usuario y pass de gmail
            //    ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            //    smtpMail.EnableSsl = true;//True si el servidor de correo permite ssl


            //    //enviamos el mail
            //    smtpMail.Send(Email);

            //    //eliminamos el objeto
            //    //smtpMail.Dispose();

            //    //regresamos true
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    //si ocurre un error regresamos false y el error
            //    error = "Ocurrio un error: " + ex.Message;
            //    return false;
            //}

        }
    }
}