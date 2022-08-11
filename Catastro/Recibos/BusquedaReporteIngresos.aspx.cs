using System;
using System.Collections.Generic;
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
    public partial class BusquedaReporteIngresos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                llenarGrid();
                Session["parametro"] = null;
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        private void llenarGrid()
        {
            if (txtFechaInicio.Text == string.Empty || txtFechaFin.Text == String.Empty)
            {
                grdReporte.DataSource = new vVistasBL().ObtieneReportesIngresos(null,null);
            }
            else
            {
                grdReporte.DataSource = new vVistasBL().ObtieneReportesIngresos(Convert.ToDateTime(txtFechaInicio.Text), Convert.ToDateTime(txtFechaFin.Text));
            }
            grdReporte.DataBind();
        }

        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            llenarGrid();
        }

        protected void grdReporte_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "VerReporte")
            {
                string id = e.CommandArgument.ToString();
                Dictionary<string, string> parametro = new Dictionary<string, string>();
                parametro.Add("idPrepoliza", id);
                parametro.Add("tipoPantalla", "C");
                Session["parametro"] = parametro;
                Response.Redirect("ReporteIngresosPorGrupo.aspx");
            }
            else if (e.CommandName == "EliminarRegistro")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;
                vtnModal.DysplayCancelar = true;
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar), ModalPopupMensaje.TypeMesssage.Confirm);
            }
        }

        protected void grdReporte_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void grdReporte_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
           grdReporte.PageIndex = e.NewPageIndex;
           llenarGrid();
        }
        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                MensajesInterfaz resul = new tPrepolizaBL().Delete(Convert.ToInt32(ViewState["idMod"]), ((cUsuarios)Session["usuario"]).Id);
                vtnModal.DysplayCancelar = false;
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Confirm);
                ViewState["idMod"] = null;
                llenarGrid();
            }
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Eliminacion))
            {
                llenarGrid();
            }
        }

        protected void imbNuevo_Click(object sender, EventArgs e)
        {
            Session["parametro"] = null;
            Response.Redirect("ReporteIngresosPorGrupo.aspx");
        }
    }
}