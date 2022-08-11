using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases.BL;
using Catastro.Controles;
using Clases;
using Clases.Utilerias;


namespace Catastro.Configuracion
{
    public partial class BusquedaConfiguracionMesa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarControles(); 
                llenaGrid();
                Session["parametro"] = null;
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }
        public void CargarControles()
        {
            ddlMesa.DataValueField = "Id";
            ddlMesa.DataTextField = "Nombre";
            ddlMesa.DataSource = new cMesaBL().GetAll();
            ddlMesa.DataBind();
            ddlMesa.Items.Insert(0, new ListItem("Seleccionar Mesa", "0"));

        }
        protected void grdConfiguracion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ModificarRegistro")
            {
                string id = e.CommandArgument.ToString();
                Dictionary<string, string> parametro = new Dictionary<string, string>();
                parametro.Add("idConfiguracion", id);
                parametro.Add("tipoPantalla", "M");
                Session["parametro"] = parametro;
                Response.Redirect("configuracionMesa.aspx");
            }
            else if (e.CommandName == "EliminarRegistro")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;               
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar), ModalPopupMensaje.TypeMesssage.Confirm);
            }
        }

        protected void grdConfiguracion_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            llenaGrid();
        }

        protected void imbNuevo_Click(object sender, EventArgs e)
        {
            Session["parametro"] = null;
            Response.Redirect("configuracionMesa.aspx");
        }
        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                MensajesInterfaz resul = new tConfiguracionMesaBL().Delete(Convert.ToInt32(ViewState["idMod"]));                
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = null;
                llenaGrid();
            }
        }
        protected void llenaGrid()
        {
            grdConfiguracion.DataSource = new vVistasBL().ObtieneConfiguracionMesa(Convert.ToInt32(ddlMesa.SelectedItem.Value));
            grdConfiguracion.DataBind();
        }

        protected void grdConfiguracion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdConfiguracion.PageIndex = e.NewPageIndex;
            llenaGrid();
        }
    }
}