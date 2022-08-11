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
    public partial class BusquedaIsabis : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["parametro"] = null;
                ViewState["sortCampo"] = "ClavePredial";
                ViewState["sortOnden"] = "desc";
                chkInactivo.Checked = true;
                llenagrid();
                llenaFiltro();
                txtFiltro.Enabled = false;
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }
        private void llenaFiltro()
        {
            ddlFiltro.DataSource = new vTramiteBL().ListaCamposVTramite();
            ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
        }

        #region Grid
        private void llenagrid()
        {
            string[] filtro = (string[])ViewState["filtro"];

            int idTipoTramite = Convert.ToInt32(new cParametroSistemaBL().GetValorByClave("Isabis"));

            if (filtro == null)
            {
                grd.DataSource = new vTramiteBL().GetFilterVTramite("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString(), idTipoTramite);
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new vTramiteBL().GetFilterVTramite(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString(), idTipoTramite);
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
                Dictionary<string, string> parametro = new Dictionary<string, string>();
                parametro.Add("idTramite", id);
                parametro.Add("tipoPantalla", "C");
                Session["parametro"] = parametro;
                Response.Redirect("capturaIsabis.aspx");
            }
            else if (e.CommandName == "ModificarRegistro")
            {
                string id = e.CommandArgument.ToString();
                Dictionary<string, string> parametro = new Dictionary<string, string>();
                parametro.Add("idTramite", id);
                parametro.Add("tipoPantalla", "M");
                Session["parametro"] = parametro;
                Response.Redirect("capturaIsabis.aspx");
            }
            else if (e.CommandName == "EliminarRegistro")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfirmacionCancelar), ModalPopupMensaje.TypeMesssage.Confirm);
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
                string status = grd.DataKeys[e.Row.RowIndex].Values[2].ToString();
                switch (status.ToUpper())
                {
                    case "A": e.Row.Cells[11].Text = "ACTIVO"; break;
                    case "C": e.Row.Cells[11].Text = "CANCELADO"; break;
                    case "P": e.Row.Cells[11].Text = "PAGADO"; break;
                    case "I": e.Row.Cells[11].Text = "NO CAUSA"; break;
                    default: e.Row.Cells[11].Text = "INDEFINIDO"; break;

                }
                if (activo.ToUpper() == "TRUE")
                {
                    if (status.ToUpper() != "A" && status.ToUpper() != "I")
                    {
                        ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                        imgActivar.Visible = false;
                        ImageButton imgUpdate = (ImageButton)e.Row.FindControl("imgUpdate");
                        imgUpdate.Visible = false;
                        ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                        imgDelete.Visible = false;
                    }
                    else
                    {
                        ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                        imgActivar.Visible = false;
                    }
                    
                }
                else
                {
                    if (status.ToUpper() != "A" && status.ToUpper() != "I")
                    {
                        ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                        imgActivar.Visible = false;
                    }
                        //ImageButton imgConsulta = (ImageButton)e.Row.FindControl("imgConsulta");
                        //imgConsulta.Visible = false;
                        ImageButton imgUpdate = (ImageButton)e.Row.FindControl("imgUpdate");
                        imgUpdate.Visible = false;
                        ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                        imgDelete.Visible = false;                    
                    
                }
            }
        }

        #endregion

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Session["parametro"] = null;
            Response.Redirect("capturaIsabis.aspx");
        }
        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];

            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfirmacionCancelar))
            {
                tTramite Tramite = new tTramiteBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));               
                Tramite.IdUsuario = U.Id;
                Tramite.FechaModificacion = DateTime.Now;
                Tramite.Status = "C";
                MensajesInterfaz resul = new tTramiteBL().Update(Tramite);
                vtnModal.DysplayCancelar = true;
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                tTramite Tramite = new tTramiteBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                Tramite.Activo = true;
                Tramite.IdUsuario = U.Id;
                Tramite.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new tTramiteBL().Update(Tramite);
                vtnModal.DysplayCancelar = true;
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
            if (ddlFiltro.SelectedValue.ToString() == "")
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