using Catastro.Controles;
using Clases.BL;
using Clases.Utilerias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Catastro.Catalogos
{
    public partial class BusquedaBitacora : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlUsuarios.DataSource = new cUsuariosBL().GetAll();
                ddlUsuarios.DataValueField = "Id";
                ddlUsuarios.DataTextField = "Usuario";
                ddlUsuarios.DataBind();
                ddlUsuarios.Items.Insert(0, new ListItem("Todos", "0"));    

                ddlVentana.DataSource = new vVistasBL().ventanaFiltro();
                ddlVentana.DataValueField = "CVEVENTANA";
                ddlVentana.DataTextField = "DECRIPCION";
                ddlVentana.DataBind();
                ddlVentana.Items.Insert(0, new ListItem("Todas", ""));
            }
        }
        
        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            string[] filtro = new string[] { ddlUsuarios.SelectedValue, ddlVentana.SelectedValue.Replace(" ",""), txtClave.Text, txtFechaInicio.Text, txtFechaFin.Text };
            ViewState["filtro"] = filtro;
            llenagrid();          
        }
        private void llenagrid()
        {
            string[] filtro = (string[])ViewState["filtro"];
            DateTime inicio = DateTime.MinValue;
            DateTime fin = DateTime.Now;
            if (!txtFechaInicio.Text.Equals(""))
                inicio = Convert.ToDateTime(txtFechaInicio.Text);
            if (!txtFechaFin.Text.Equals(""))
                fin = Convert.ToDateTime(txtFechaFin.Text + " 23:59:59");

            grd.DataSource = new vVistasBL().bitacora(Convert.ToInt32(filtro[0]), filtro[1], filtro[2], inicio, fin);            
            grd.DataBind();
        }
        protected void grd_Sorting(object sender, GridViewSortEventArgs e)
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
            llenagrid();
        }

        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd.PageIndex = e.NewPageIndex;
            llenagrid();
        }

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "consultar")
            {
                int rowIndex = int.Parse(e.CommandArgument.ToString());
                string IdTabla = this.grd.DataKeys[rowIndex]["IdTabla"].ToString();
                string NombreTabla = this.grd.DataKeys[rowIndex]["NombreTabla"].ToString();
                
                gdrRegistro.DataSource = new Utileria().ConcultaRegistro(IdTabla, NombreTabla, this.grd.DataKeys[rowIndex]["Usuario"].ToString(), this.grd.DataKeys[rowIndex]["ClavePredial"].ToString());
                gdrRegistro.DataBind();
                lbl_titulo.Text = "Consultar Registro de la Tabla: " + this.grd.DataKeys[rowIndex]["Tabla"].ToString();
                pnl_Modal.Show();
            }             
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            pnl_Modal.Hide();
            gdrRegistro.DataSource = null;
            gdrRegistro.DataBind();
        }
    }
}