using Catastro.Controles;
using Clases;
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
    public partial class BusquedaContribuyentes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {               
                ViewState["sortCampo"] = "Nombre";
                ViewState["sortOnden"] = "asc";              
                llenaFiltro();  
                Dictionary<string, string> parametros = (Dictionary<string, string>)Session["parametro"];
                if (parametros != null)
                {
                    if (parametros.ContainsKey("origen"))
                    {
                        if (parametros["origen"] == "BusquedaContribuyentes")
                        {
                            string[] filtro = new string[] { parametros["filtro0"], parametros["filtro1"], parametros["filtro2"] };
                            ViewState["filtro"] = filtro;
                            if (parametros["filtro2"] == "True")
                            { chkInactivo.Checked = true; }
                            else
                            { chkInactivo.Checked = false; }
                            ddlFiltro.SelectedValue = filtro[0];
                            ddlFiltro_SelectedIndexChanged(null, null);
                            txtFiltro.Text = filtro[1];

                            llenagrid();
                        }
                    }
                    else
                    {
                        chkInactivo.Checked = true;
                        txtFiltro.Enabled = false;
                    }
                }
                else
                {
                    chkInactivo.Checked = true;
                    txtFiltro.Enabled = false;
                }
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        private void llenaFiltro()
        {
            ddlFiltro.DataSource = new cContribuyenteBL().ListaCampos();
            ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Seleccione una opción", "%"));
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Session["parametro"] = null;
            Response.Redirect("catContribuyente.aspx");
        }

        #region Grid
        private void llenagrid()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new cContribuyenteBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new cContribuyenteBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();

            }
        }


        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd.PageIndex = e.NewPageIndex;
            llenagrid();
        }

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ConsultarRegistro")
            {
                string id = e.CommandArgument.ToString();
                string[] filtro = (string[])ViewState["filtro"];
                Dictionary<string, string> parametro = new Dictionary<string, string>();
                parametro.Add("idContribuyente", id);
                parametro.Add("tipoPantalla", "C");
                parametro.Add("origen", "BusquedaContribuyentes");
                parametro.Add("filtro0", filtro[0]);
                parametro.Add("filtro1", filtro[1]);
                parametro.Add("filtro2", filtro[2]);
                Session["parametro"] = parametro;
                Response.Redirect("catContribuyente.aspx");
            }
            else if (e.CommandName == "ModificarRegistro")
            {
                string id = e.CommandArgument.ToString();
                string[] filtro = (string[])ViewState["filtro"];
                Dictionary<string, string> parametro = new Dictionary<string, string>();
                parametro.Add("idContribuyente", id);
                parametro.Add("tipoPantalla", "M");
                parametro.Add("origen", "BusquedaContribuyentes");
                parametro.Add("filtro0", filtro[0]);
                parametro.Add("filtro1", filtro[1]);
                parametro.Add("filtro2", filtro[2]);
                Session["parametro"] = parametro;
                Response.Redirect("catContribuyente.aspx");
            }
            else if (e.CommandName == "EliminarRegistro")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar), ModalPopupMensaje.TypeMesssage.Confirm);
            }
            else if (e.CommandName == "ActivarRegistro")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro), ModalPopupMensaje.TypeMesssage.Confirm);
            }

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

        protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string activo = grd.DataKeys[e.Row.RowIndex].Values[1].ToString();
                if (activo.ToUpper() == "TRUE")
                {
                    ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                    imgActivar.Visible = false;
                }
                else
                {
                    ImageButton imgConsulta = (ImageButton)e.Row.FindControl("imgConsulta");
                    imgConsulta.Visible = false;
                    ImageButton imgUpdate = (ImageButton)e.Row.FindControl("imgUpdate");
                    imgUpdate.Visible = false;
                    ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                    imgDelete.Visible = false;
                }
            }
        }

        #endregion

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];

            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                cContribuyente Contribuyente = new cContribuyenteBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                Contribuyente.Activo = false;
                Contribuyente.IdUsuario = U.Id;
                Contribuyente.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cContribuyenteBL().Delete(Contribuyente);
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                cContribuyente Contribuyente = new cContribuyenteBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                Contribuyente.Activo = true;
                Contribuyente.IdUsuario = U.Id;
                Contribuyente.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cContribuyenteBL().Update(Contribuyente);
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            
        }

        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            string[] filtro = new string[] { ddlFiltro.SelectedValue, txtFiltro.Text, chkInactivo.Checked.ToString() };
            ViewState["filtro"] = filtro;
            llenagrid();
        }

        protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFiltro.Text = "";
            if (ddlFiltro.SelectedValue.ToString() == "%")
            {
                txtFiltro.Enabled = false;
            }
            else
            {
                txtFiltro.Enabled = true;
            }
        }
    }
}