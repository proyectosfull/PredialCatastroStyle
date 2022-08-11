using Clases;
using Clases.BL;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Catastro.Reportes
{
    public partial class TabularIngresos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlReport.Visible = false;
                llenaDdls();
            }
        }

        protected void llenaDdls()
        {
            //ddlStatus.DataValueField = "Id";
            //ddlStatus.DataTextField = "Descripcion";
            //ddlStatus.DataSource = new cStatusPredioBL().GetAll();
            //ddlStatus.DataBind();
            //ddlStatus.Items.Insert(0, new ListItem("Todos", "0"));
            ddlTipo.DataValueField = "Id";
            ddlTipo.DataTextField = "Descripcion";
            ddlTipo.DataSource = new cTipoPredioBL().GetAll();
            ddlTipo.DataBind();
            ddlTipo.Items.Insert(0, new ListItem("Todos", "0"));

            ddlTipoTramite.DataValueField = "Id";
            ddlTipoTramite.DataTextField = "Descripcion";
            ddlTipoTramite.DataSource = new cTipoTramiteBL().GetAll();
            ddlTipoTramite.DataBind();
            ddlTipoTramite.Items.Insert(0, new ListItem("Todos", "0"));

            //Int32 Anio = DateTime.Today.Year;
            //ddlAnio.Items.Insert(0, new ListItem("Todos", "0"));
            //ddlAnio.Items.Insert(1, new ListItem(Anio.ToString(), Anio.ToString()));
            //for (Int32 i = 1; i <= 5; i++)
            //{
            //    ddlAnio.Items.Insert(i + 1, new ListItem((Anio - i).ToString(), (Anio - 1).ToString()));
            //}

            ddlMesa.DataValueField = "Id";
            ddlMesa.DataTextField = "Descripcion";
            ddlMesa.DataSource = new cMesaBL().GetAll();
            ddlMesa.DataBind();
            ddlMesa.Items.Insert(0, new ListItem("Todos", "0"));

            ddlCondominio.DataValueField = "Id";
            ddlCondominio.DataTextField = "Descripcion";
            ddlCondominio.DataSource = new cCondominioBL().GetAll();
            ddlCondominio.DataBind();
            ddlCondominio.Items.Insert(0, new ListItem("Todos", "0"));
        }

        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            pnlReport.Visible = true;
            List<cParametroSistema> listConfiguraciones = new cParametroSistemaBL().GetAll();
            string NombreMunicipio = listConfiguraciones.FirstOrDefault(c => c.Clave.ToUpper() == "NOMBRE_MUNICIPIO").Valor;
            string Dependencia = listConfiguraciones.FirstOrDefault(c => c.Clave.ToUpper() == "DEPENDENCIA").Valor;
            string Area = listConfiguraciones.FirstOrDefault(c => c.Clave.ToUpper() == "AREA").Valor;
            string UrlLogo = Server.MapPath("~") + "\\"  + listConfiguraciones.FirstOrDefault(c => c.Clave.ToUpper() == "LOGO").Valor;
            FileStream fS = new FileStream(UrlLogo, FileMode.Open, FileAccess.Read);
            byte[] LogoByte = new byte[fS.Length];
            fS.Read(LogoByte, 0, (int)fS.Length);
            fS.Close();
            
            cUsuarios user = ((cUsuarios)Session["usuario"]);

            DataTable ConfGral = new DataTable("ConfGral");
            ConfGral.Columns.Add("NombreMunicipio");
            ConfGral.Columns.Add("Dependencia");
            ConfGral.Columns.Add("Area");
            ConfGral.Columns.Add("Logo", typeof(Byte[]));
            ConfGral.Rows.Add(NombreMunicipio, Dependencia, Area, LogoByte);
                        
            List<vTabularIngresos> listado = new List<vTabularIngresos>();
            DateTime fin = Convert.ToDateTime(txtFechaFin.Text + " 23:59:59");
            DateTime inicio = Convert.ToDateTime(txtFechaInicio.Text);
                       
            listado = new vVistasBL().TabularIngresos(int.Parse(ddlMesa.SelectedValue), int.Parse(ddlTipoTramite.SelectedValue),int.Parse(ddlStatus.SelectedValue), int.Parse(ddlTipo.SelectedValue), int.Parse(ddlCondominio.SelectedValue), txtColonia.Text,inicio,fin);
      
            //INICIA REPORTE
            this.rpvtPredios.ProcessingMode = ProcessingMode.Local;
            rpvtPredios.LocalReport.DisplayName = "TABULAR DE INGRESOS";
            rpvtPredios.LocalReport.ReportPath = "Reportes/tIngresos.rdlc";
            rpvtPredios.LocalReport.EnableExternalImages = true;
            rpvtPredios.LocalReport.DataSources.Clear();
            rpvtPredios.LocalReport.DataSources.Add(new ReportDataSource("ConfGral", ConfGral));
            rpvtPredios.LocalReport.DataSources.Add(new ReportDataSource("tIngresos", listado));
            rpvtPredios.LocalReport.Refresh();
        }

        public string RemoveSpecialCharacters(string str)
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

        #region filtros
        protected void imbBuscarCon_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["sortCampo"] = "Nombre";
            lblbuscarContribuyente.Text = "Buscar Contribuyente";
            llenaFiltroContribuyente();
            pnl_Modal.Show();
        }
        private void llenaFiltroContribuyente()
        {
            ddlFiltroContribuyente.Items.Clear();
            ddlFiltroContribuyente.Items.Insert(0, new ListItem("Todos", ""));
            ddlFiltroContribuyente.Items.Insert(1, new ListItem("Nombre", "Nombre"));
            ddlFiltroContribuyente.Items.Insert(2, new ListItem("Apellido Paterno", "ApellidoPaterno"));
            ddlFiltroContribuyente.Items.Insert(3, new ListItem("Apellido Materno", "ApellidoMaterno"));
            ddlFiltroContribuyente.Items.Insert(4, new ListItem("CURP", "Curp"));
        }
        protected void imbCondominio_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["sortCampo"] = "Descripcion";
            lblbuscarContribuyente.Text = "Buscar Condominio";
            llenaFiltroCondominio();
            pnl_Modal.Show();
        }
        private void llenaFiltroCondominio()
        {
            ddlFiltroContribuyente.Items.Clear();
            ddlFiltroContribuyente.Items.Insert(0, new ListItem("Todos", ""));
            ddlFiltroContribuyente.Items.Insert(1, new ListItem("Nombre", "Descripcion"));
        }
        protected void imbColonia_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["sortCampo"] = "NombreColonia";
            lblbuscarContribuyente.Text = "Buscar Colonia";
            llenaFiltroColonia();
            pnl_Modal.Show();
        }
        private void llenaFiltroColonia()
        {
            ddlFiltroContribuyente.Items.Clear();
            ddlFiltroContribuyente.Items.Insert(0, new ListItem("Todos", ""));
            ddlFiltroContribuyente.Items.Insert(1, new ListItem("Nombre", "NombreColonia"));
        }

        
        protected void imbBuscarContribuyenteFiltro_Click(object sender, ImageClickEventArgs e)
        {
            string[] filtro = new string[] { ddlFiltroContribuyente.SelectedValue, txtFiltroContribuyente.Text, "true" };
            ViewState["filtro"] = filtro;
            if (lblbuscarContribuyente.Text.Contains("Contribuyente"))
                llenagrid();
            else if (lblbuscarContribuyente.Text.Contains("Condominio"))
                llenagridCondominio();
            else
                llenagridColonia();
        }
        private void llenagrid()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new cContribuyenteBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOrden"].ToString());
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new cContribuyenteBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOrden"].ToString());
                grd.DataBind();

            }
            grd.Visible = true;
            pnl_Modal.Show();
        }
        private void llenagridCondominio()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grdCon.DataSource = new cCondominioBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), "ASC");
                grdCon.DataBind();
            }
            else
            {
                grdCon.DataSource = new cCondominioBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), "ASC");
                grdCon.DataBind();

            }
            grdCon.Visible = true;
            pnl_Modal.Show();
        }
        private void llenagridColonia()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grdCol.DataSource = new cColoniaBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOrden"].ToString());
                grdCol.DataBind();
            }
            else
            {
                grdCol.DataSource = new cColoniaBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOrden"].ToString());
                grdCol.DataBind();

            }
            grdCol.Visible = true;
            pnl_Modal.Show();
        }


        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SeleccionarPersona")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;
                cContribuyenteBL cBL = new cContribuyenteBL();
                cContribuyente contribuyente = cBL.GetByConstraint(id);
                //txtContribuyente.Text = contribuyente.ApellidoPaterno + " " + contribuyente.ApellidoMaterno + " " + contribuyente.Nombre;
                //hdfIdCondominio.Value = contribuyente.Id.ToString();
                txtFiltroContribuyente.Text = "";
                grd.DataSource = null;
                grd.DataBind();
                grd.Visible = false;
                pnl_Modal.Hide();
            }

        }
        protected void grd_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["sortCampo"] == null)
            {
                ViewState["sortCampo"] = e.SortExpression.ToString();
                ViewState["sortOrden"] = "asc";
            }
            else
            {
                if (e.SortExpression.ToString() == ViewState["sortCampo"].ToString())
                {
                    if (ViewState["sortOrden"].ToString() == "asc")
                        ViewState["sortOrden"] = "desc";
                    else
                        ViewState["sortOrden"] = "asc";
                }
                else
                {
                    ViewState["sortCampo"] = e.SortExpression.ToString();
                    ViewState["sortOrden"] = "asc";
                }
            }

            llenagrid();
        }
        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd.PageIndex = e.NewPageIndex;
            llenagrid();
        }

        protected void grdCon_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SeleccionarPersona")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;
                cCondominioBL cBL = new cCondominioBL();
                cCondominio condominio = cBL.GetByConstraint(id);
                //txtCondominio.Text = condominio.Descripcion;
                //hdfIdCondominio.Value = condominio.Id.ToString();
                txtFiltroContribuyente.Text = "";
                grdCon.DataSource = null;
                grdCon.DataBind();
                grdCon.Visible = false;
                pnl_Modal.Hide();
            }
        }
        protected void grdCon_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["sortCampo"] == null)
            {
                ViewState["sortCampo"] = e.SortExpression.ToString();
                ViewState["sortOrden"] = "asc";
            }
            else
            {
                if (e.SortExpression.ToString() == ViewState["sortCampo"].ToString())
                {
                    if (ViewState["sortOrden"].ToString() == "asc")
                        ViewState["sortOrden"] = "desc";
                    else
                        ViewState["sortOrden"] = "asc";
                }
                else
                {
                    ViewState["sortCampo"] = e.SortExpression.ToString();
                    ViewState["sortOrden"] = "asc";
                }
            }

            llenagridCondominio();
        }
        protected void grdCon_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCon.PageIndex = e.NewPageIndex;
            llenagridCondominio();
        }

        protected void grdCol_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SeleccionarPersona")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;
                cColoniaBL cBL = new cColoniaBL();
                cColonia colonia = cBL.GetByConstraint(id);
                txtColonia.Text = colonia.NombreColonia;
                //hdfIdColonia.Value = colonia.Id.ToString();
                txtFiltroContribuyente.Text = "";
                grdCol.DataSource = null;
                grdCol.DataBind();
                grdCol.Visible = false;
                pnl_Modal.Hide();
            }

        }
        protected void grdCol_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["sortCampo"] == null)
            {
                ViewState["sortCampo"] = e.SortExpression.ToString();
                ViewState["sortOrden"] = "asc";
            }
            else
            {
                if (e.SortExpression.ToString() == ViewState["sortCampo"].ToString())
                {
                    if (ViewState["sortOrden"].ToString() == "asc")
                        ViewState["sortOrden"] = "desc";
                    else
                        ViewState["sortOrden"] = "asc";
                }
                else
                {
                    ViewState["sortCampo"] = e.SortExpression.ToString();
                    ViewState["sortOrden"] = "asc";
                }
            }

            llenagridColonia();
        }
        protected void grdCol_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCon.PageIndex = e.NewPageIndex;
            llenagridColonia();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtFiltroContribuyente.Text = "";
            grd.DataSource = null;
            grd.DataBind();
            grd.Visible = false;
            grdCon.DataSource = null;
            grdCon.DataBind();
            grdCon.Visible = false;
            grdCol.DataSource = null;
            grdCol.DataBind();
            grdCol.Visible = false;
            pnl_Modal.Hide();
        }

        protected void imgBorrarCon_Click(object sender, ImageClickEventArgs e)
        {
            //txtContribuyente.Text = "";
            //hdfIdCondominio.Value = "";
        }
        protected void imbBorrarCondominio_Click(object sender, ImageClickEventArgs e)
        {
            //txtCondominio.Text = "";
            //hdfIdCondominio.Value = "";
        }
        protected void imbBorrarColonia_Click(object sender, ImageClickEventArgs e)
        {
            txtColonia.Text = "";
            //hdfIdColonia.Value = "";
        }
        #endregion

        protected void ddlAnio_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }
}