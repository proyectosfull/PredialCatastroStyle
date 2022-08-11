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
    public partial class ReporteAntecPredio : System.Web.UI.Page
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

            DateTime fin = Convert.ToDateTime(txtFechaFin.Text + " 23:59:59");
            DateTime inicio = Convert.ToDateTime(txtFechaInicio.Text);

            bool fechaTramite = rblFecha.SelectedValue == "Tramite" ? true : false;
            List<vAntecedentePredio> listConsulta = new vVistasBL().ObtieneAntecedentePredio(txtClave.Text,Convert.ToInt32(hdfIdContribuyente.Value),fechaTramite, inicio, fin);
                     
            ////INICIA REPORTE
            rpt.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            rpt.LocalReport.DisplayName = "Antecendentes del Predio";
            rpt.LocalReport.ReportPath = "Reportes/AntecPredial.rdlc";
            rpt.LocalReport.EnableExternalImages = true;
            rpt.LocalReport.DataSources.Clear();
            rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ConfGral", ConfGral));
            rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("AntecPredial", listConsulta));
            List<ReportParameter> paramList = new List<ReportParameter>();
            paramList.Add(new ReportParameter("fInicio",inicio.ToString("dd/MM/yyyy"), false));
            paramList.Add(new ReportParameter("fFin",fin.ToString("dd/MM/yyyy"), false));
            this.rpt.LocalReport.SetParameters(paramList);
            rpt.LocalReport.Refresh(); 
        }

        protected void RBLtipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBLtipo.SelectedValue == "Clave")
            {
                hdfIdContribuyente.Value = "0";
                lblContribuyente.Text = "";
                lblContribuyente.Visible=false;                
                imBuscarPropietario.Visible = false;
                txtClave.Text = "";
                txtClave.Visible = true;
            }
            else
            {
                txtClave.Text = "";
                txtClave.Visible = false;
                lblContribuyente.Visible = true;
                imBuscarPropietario.Visible = true;
            }           
           
        }

        protected void buscarPropietario(object sender, ImageClickEventArgs e)
        {
            llenaFiltro();
            grdPropietarios.Visible = false;
            grdPropietarios.DataSource = null;
            grdPropietarios.DataBind();
            txtFiltro.Text = "";           
            modalPropietario.Show();
        }

        protected void btnCancelarPropietario_Click(object sender, EventArgs e)
        {
            modalPropietario.Hide();
        }
        private void llenaFiltro()
        {
            ddlFiltro.Items.Clear();
            ddlFiltro.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar", "%"));
            ddlFiltro.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Nombre", "NombreCompleto"));
            ddlFiltro.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Dirección", "Domicilio"));
            ddlFiltro.DataBind();
        }

        protected void consultarPropietario(object sender, ImageClickEventArgs e)
        {
            string[] filtro = new string[] { ddlFiltro.SelectedValue, txtFiltro.Text, "true" };
            ViewState["filtro"] = filtro;
            ViewState["sortCampo"] = "NombreCompleto";
            ViewState["sortOnden"] = "asc";
            grdPropietarios.Visible = true;
            llenagridPropietario();
        }
        private void llenagridPropietario()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grdPropietarios.DataSource = new vVistasBL().GetFiltervBuscarPredioContDom("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grdPropietarios.DataBind();
            }
            else
            {
                grdPropietarios.DataSource = new vVistasBL().GetFiltervBuscarPredioContDom(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grdPropietarios.DataBind();

            }
            modalPropietario.Show();
        }

        protected void grdPropietarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPropietarios.PageIndex = e.NewPageIndex;
            llenagridPropietario();
        }

        protected void grdPropietarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Activar")
            {                
                int rowIndex = int.Parse(e.CommandArgument.ToString());
                hdfIdContribuyente.Value = grdPropietarios.DataKeys[rowIndex]["IdContribuyente"].ToString();
                lblContribuyente.Text =HttpUtility.HtmlDecode(grdPropietarios.Rows[rowIndex].Cells[0].Text);                
            }
        }

        protected void grdPropietarios_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["sortCampo"] == null)
            {
                ViewState["sortCampo"] = e.SortExpression.ToString();
                ViewState["sortOnden"] = "asc";
            }
            else
            {
                if (e.SortExpression.ToString() == ViewState["sortCampo"].ToString())
                {
                    if (ViewState["sortOnden"].ToString() == "asc")
                        ViewState["sortOnden"] = "desc";
                    else
                        ViewState["sortOnden"] = "asc";
                }
                else
                {
                    ViewState["sortCampo"] = e.SortExpression.ToString();
                    ViewState["sortOnden"] = "asc";
                }
            }
            llenagridPropietario();
        }
             
    }
}