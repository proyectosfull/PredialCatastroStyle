using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using Clases.Utilerias;
using Catastro.Controles;



namespace Catastro.Servicios
{
    public partial class CancelacionRecibo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFolio.Text ="";
            txtObservacion.Text = "";
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtObservacion.Text.Length == 0 )
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("Motivo de cancelación vacio,ingrese datos"), ModalPopupMensaje.TypeMesssage.Alert);
            }

            //ServicioClient rec = new ServicioClient(); 
            //llamar al metodo de cancelacion
        }

        protected void Button1_Click(object sender, EventArgs e)
        {            
            string urlpath = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;
            urlpath = urlpath + "/Temporales/110038800001.pdf";
            string parametros = "','Estado de cuenta','height=600,width=550,toolbar=no,directories=no,menubar=no,scrollbars=no,resizable=no'";
            String Clientscript = "window.open('" + urlpath + parametros+ " )";                       
          
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newWindow", Clientscript, true);
        }

        public void ConsultaRecibo()
        {
            // Private Sub btnConsultaRecibo_Click(sender As Object, e As EventArgs) Handles btnConsultaRecibo.Click
            //Dim servicio As ServicioClient = New ServicioClient
            //Dim recibo As Recibo = New Recibo()
            //recibo = servicio.cReciboXfolio(1)

            //Dim saveFileDialog1 As New SaveFileDialog()

            //saveFileDialog1.Filter = "*PDF files (*.pdf)|*.pdf"
            //saveFileDialog1.FilterIndex = 2
            //saveFileDialog1.RestoreDirectory = True
            //saveFileDialog1.FileName = "Recibo"
            //If saveFileDialog1.ShowDialog() = DialogResult.OK Then
            //    Dim newFile As New FileStream(saveFileDialog1.FileName, FileMode.Create)
            //    newFile.Write(recibo.pdfBytes, 0, recibo.pdfBytes.Length)
            //    newFile.Close()
            //End If


        }

        public void ConsultaReciboCodigo()
        {
            //Private Sub btnConsultaReciboCodigo_Click(sender As Object, e As EventArgs) Handles btnConsultaReciboCodigo.Click
            //   Dim servicio As ServicioClient = New ServicioClient
            //   Dim recibo As Recibo = New Recibo()
            //   recibo = servicio.cReciboXcodigo("ee4a3b65263ab9767c324db5db67274007a5e56a")

            //   Dim saveFileDialog1 As New SaveFileDialog()

            //   saveFileDialog1.Filter = "*PDF files (*.pdf)|*.pdf"
            //   saveFileDialog1.FilterIndex = 2
            //   saveFileDialog1.RestoreDirectory = True
            //   saveFileDialog1.FileName = "Recibo"
            //   If saveFileDialog1.ShowDialog() = DialogResult.OK Then
            //       Dim newFile As New FileStream(saveFileDialog1.FileName, FileMode.Create)
            //       newFile.Write(recibo.pdfBytes, 0, recibo.pdfBytes.Length)
            //       newFile.Close()
            //   End If


        }

       
    }
}