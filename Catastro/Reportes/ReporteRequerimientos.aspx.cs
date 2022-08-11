using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using Clases.BL;
using Clases.Utilerias;
using Catastro.Controles;
using Microsoft.Reporting.WebForms;

namespace Catastro.Recibos
{
    public partial class ReporteRequerimientos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlReport.Visible = false;
                
                ddlCondominio.DataValueField = "Id";
                ddlCondominio.DataTextField = "Descripcion";
                ddlCondominio.DataSource = new cCondominioBL().GetAll();
                ddlCondominio.DataBind();
                ddlCondominio.Items.Insert(0, new ListItem("TODOS", "0"));

                var listAgente = new cAgenteFiscalBL().GetAll().Select(v => new { Id = v.Id, NombreCompleto =  v.ApellidoPaterno + " " + v.ApellidoMaterno + " " + v.Nombre}).OrderBy(o => o.NombreCompleto);
                ddlAgente.DataValueField = "Id";
                ddlAgente.DataTextField = "NombreCompleto";
                ddlAgente.DataSource = listAgente;
                ddlAgente.DataBind();
                ddlAgente.Items.Insert(0, new ListItem("TODOS", "0"));


            }
        }
        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
           
            pnlReport.Visible = true;
            //CARGA DATOS GENERALES y se crea datatable
            List<cParametroSistema> listConfiguraciones = new cParametroSistemaBL().GetAll();
            string NombreMunicipio = listConfiguraciones.FirstOrDefault(c => c.Clave == "NOMBRE_MUNICIPIO").Valor;
            string Dependencia = listConfiguraciones.FirstOrDefault(c => c.Clave == "DEPENDENCIA").Valor;
            string Area = listConfiguraciones.FirstOrDefault(c => c.Clave == "AREA").Valor;
            string UrlLogo = Server.MapPath("~") + listConfiguraciones.FirstOrDefault(c => c.Clave == "LOGO").Valor;
            FileStream fS = new FileStream(UrlLogo, FileMode.Open, FileAccess.Read);
            byte[] LogoByte = new byte[fS.Length];
            fS.Read(LogoByte, 0, (int)fS.Length);
            fS.Close();          

            DataTable ConfGral = new DataTable("ConfGral");
            ConfGral.Columns.Add("NombreMunicipio");
            ConfGral.Columns.Add("Dependencia");
            ConfGral.Columns.Add("Area");
            ConfGral.Columns.Add("Logo", typeof(Byte[]));
            ConfGral.Columns.Add("Mesa");
            ConfGral.Columns.Add("Cajero");
            ConfGral.Columns.Add("Entrego");
            ConfGral.Columns.Add("RecibioCajaGeneral");
            ConfGral.Columns.Add("VoBo");
            cUsuarios U = (cUsuarios)Session["usuario"];          
            string nombre = U.Nombre + " " + U.ApellidoPaterno + " " + U.ApellidoMaterno;
            ConfGral.Rows.Add(NombreMunicipio, Dependencia, Area, LogoByte, "", "", nombre, "", "");

            string fin = txtFechaFin.Text + " 23:59:59";
            string inicio = txtFechaInicio.Text;
            List<vRequerimientosCompleto> list= new vVistasBL().ObtieneRequerimientosCompletoFecha(ddlEstado.SelectedValue,Convert.ToInt32(ddlCondominio.SelectedValue), inicio, fin, Convert.ToInt32(ddlAgente.SelectedValue), RadioButtonFecha.SelectedValue);
                       
            ////INICIA REPORTE
            rpt.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            rpt.LocalReport.DisplayName = "Reporte Requerimientos";
            rpt.LocalReport.ReportPath = "Reportes/RequerimientosCompleto.rdlc";
            rpt.LocalReport.EnableExternalImages = true;
            rpt.LocalReport.DataSources.Clear();
            rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ConfGral", ConfGral));
            rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("Requerimiento", list));
            rpt.LocalReport.Refresh(); 
        }

        protected void RadioButtonFecha_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonFecha.SelectedValue == "Pago")
                lblFecha.Text = "Fecha Pago";
            else
                lblFecha.Text = "Fecha Emisión";
        }
    }
}