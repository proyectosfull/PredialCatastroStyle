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

namespace Catastro.Recibos
{
    public partial class BuscarCorte : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlReport.Visible = false;
            }
        }
        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            llenarGrid();
            //tCorteCaja corte = new tCorteCajaBL().GetByConstraint(Convert.ToInt32(txtBusqueda.Text));
            //reporte(corte);
        }

        private void llenarGrid()
        {
            if (ddlBusqueda.SelectedItem.Value == "1")
            {
                grdCorte.DataSource = new tCorteCajaBL().GetByFechas(Convert.ToDateTime(txtFechaInicio.Text),Convert.ToDateTime(txtFechaFin.Text + " 23:59:59"));
            }
            else
            {
                grdCorte.DataSource = new tCorteCajaBL().GetByFolio(Convert.ToInt32(txtBusqueda.Text.Trim()));
            }
            grdCorte.DataBind();
        }
        private void reporte(tCorteCaja corte)
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
            string RecibioCajaGeneral = listConfiguraciones.FirstOrDefault(c => c.Clave == "RecibioCajaGeneral").Valor;
            string VoBo = listConfiguraciones.FirstOrDefault(c => c.Clave == "VoBo").Valor;

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
            string nombreCajero = corte.cUsuarios.Nombre + " " + corte.cUsuarios.ApellidoPaterno + " " + corte.cUsuarios.ApellidoMaterno;
            ConfGral.Rows.Add(NombreMunicipio, Dependencia, Area, LogoByte, corte.cMesa.Nombre , corte.cUsuarios.Usuario, nombreCajero, RecibioCajaGeneral, VoBo);

            //lista tCorteCaja
            List<tCorteCaja> listcorte = new List<tCorteCaja>();
            listcorte.Add(corte);

            DataTable MetodoPagoDTS = new DataTable("MetodoPagoDTS");
            MetodoPagoDTS.Columns.Add("Metodo");
            MetodoPagoDTS.Columns.Add("Numero");
            MetodoPagoDTS.Columns.Add("Autorizacion");
            MetodoPagoDTS.Columns.Add("Importe", System.Type.GetType("System.Decimal"));

            //se llena lista de recibos
            List<vRecibo> listvRecibos = new List<vRecibo>();
            foreach (tCorteCajaDetalle ccd in corte.tCorteCajaDetalle)
            {
                vRecibo r = new vVistasBL().ObtieneRecibo(ccd.IdRecibo);
                if (r.EstadoRecibo == "CANCELADO")
                    r.ImportePagado =  0;//Convert.ToDecimal("0.00");
                listvRecibos.Add(r);
                if (r.IdTipoPago != 2 && r.EstadoRecibo != "CANCELADO")
                {
                    MetodoPagoDTS.Rows.Add(r.Nombre, r.NoTipoPago, r.NoAutorizacion, r.ImportePagado);
                }  
            }
            

            //INICIA REPORTE
            rpt.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            rpt.LocalReport.DisplayName = "Corte de Caja";
            rpt.LocalReport.ReportPath = "Reportes/CorteCajaRpt.rdlc";
            rpt.LocalReport.EnableExternalImages = true;
            rpt.LocalReport.DataSources.Clear();
            rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ConfGral", ConfGral));
            rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("tCorteCaja", listcorte));
            rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("vRecibo", listvRecibos));
            rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("MetodoPagoDTS", MetodoPagoDTS));
            rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("tCorteCajaDenominacion", corte.tCorteCajaDenominacion));
            rpt.LocalReport.Refresh();
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (ddlBusqueda.SelectedItem.Value == "2") // por folio
            {
                txtFechaInicio.Visible = false;
                txtFechaFin.Visible = false;
                txtBusqueda.Visible = true;
                txtBusqueda.Text = "";
                rfvFechaInicio.Enabled = false;
                rfvFechaInicio.Visible = false;
                rfvFechaFin.Enabled = false;                
                rfvFechaFin.Visible = false;
                rfvBusqueda.Enabled = true;
                rfvBusqueda.Visible = true;
                lblFiltro.Visible = true;
                lblFiltro.Text = "Folio:";
            }
            else if (ddlBusqueda.SelectedItem.Value == "1") // por fechas
            {
                txtFechaInicio.Text = DateTime.Now.ToShortDateString();
                txtFechaFin.Text = DateTime.Now.ToShortDateString();
                txtFechaInicio.Visible = true;
                txtFechaFin.Visible = true;
                txtBusqueda.Visible = false;
                rfvFechaInicio.Enabled = true;
                rfvFechaFin.Enabled = true;
                rfvFechaInicio.Visible = true;
                rfvFechaFin.Visible = true;
                rfvBusqueda.Enabled = false;
                rfvBusqueda.Visible = false;
                lblFiltro.Visible = true;
                lblFiltro.Text = "Fechas:";
            }
        }

        protected void grdInfraccion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void grdInfraccion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ConsultarRegistro")
            {
                string id = e.CommandArgument.ToString();
                tCorteCaja corte = new tCorteCajaBL().GetByConstraint(Convert.ToInt32(id));
                reporte(corte);
            }
        }
  
    
    }
}