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

namespace Catastro.Catalogos
{
    public partial class BusquedaPredio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["sortCampo"] = "ClavePredial";
                ViewState["sortOnden"] = "asc";

                Dictionary<string, string> parametros = (Dictionary<string, string>)Session["parametro"];
                if (parametros != null)
                {
                    if (parametros.ContainsKey("origen"))
                    {
                        if (parametros["origen"] == "BusquedaPredio")
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
                    }
                }
                else
                {
                    chkInactivo.Checked = true;
                }
               
                
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        
        #region Grid
        private void llenagrid()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new vVistasBL().GetFilterVPredioCorto("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new vVistasBL().GetFilterVPredioCorto(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
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
                string[] filtro = (string[])ViewState["filtro"];
                string id = e.CommandArgument.ToString();
                Dictionary<string, string> parametro = new Dictionary<string, string>();
                parametro.Add("idPredio", id);
                parametro.Add("tipoPantalla", "C");
                parametro.Add("origen", "BusquedaPredio");
                parametro.Add("filtro0", filtro[0]);
                parametro.Add("filtro1", filtro[1]);
                parametro.Add("filtro2", filtro[2]);
                Session["parametro"] = parametro;
                Response.Redirect("catPredios.aspx");
            }
            else if (e.CommandName == "ModificarRegistro")
            {
                string[] filtro = (string[])ViewState["filtro"];
                string id = e.CommandArgument.ToString();
                Dictionary<string, string> parametro = new Dictionary<string, string>();
                parametro.Add("idPredio", id);
                parametro.Add("tipoPantalla", "M");
                parametro.Add("origen", "BusquedaPredio");
                parametro.Add("filtro0", filtro[0]);
                parametro.Add("filtro1", filtro[1]);
                parametro.Add("filtro2", filtro[2]);
                Session["parametro"] = parametro;
                Response.Redirect("catPredios.aspx");
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
                    //ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                    //imgActivar.Visible = false;
                }
                else
                {
                    ImageButton imgConsulta = (ImageButton)e.Row.FindControl("imgConsulta");
                    imgConsulta.Visible = false;
                    ImageButton imgUpdate = (ImageButton)e.Row.FindControl("imgUpdate");
                    imgUpdate.Visible = false;
                    //ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                    //imgDelete.Visible = false;
                }
            }
        }

        #endregion

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Session["parametro"] = null;
            Response.Redirect("catPredios.aspx");
        }

        
        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];

            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                cPredio predio = new cPredioBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                predio.Activo = false;
                predio.IdUsuario = U.Id;
                predio.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cPredioBL().Delete(predio);
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                cPredio predio = new cPredioBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                predio.Activo = true;
                predio.IdUsuario = U.Id;
                predio.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cPredioBL().Delete(predio);
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }            
        }

        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            int caracteres = ddlFiltro.SelectedValue.Equals("ClavePredial") ? 7 : 2;
            if (txtFiltro.Text.Length > caracteres)
            {
                string[] filtro = new string[] { ddlFiltro.SelectedValue, txtFiltro.Text, chkInactivo.Checked.ToString() };
                ViewState["filtro"] = filtro;
                llenagrid();
            }
            else
            {
                MensajesInterfaz msg = ddlFiltro.SelectedValue.Equals("ClavePredial") ? MensajesInterfaz.ClaveMinOcho : MensajesInterfaz.NombreMinTres;
                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
            }
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
                txtFiltro.Text = "";
                if (ddlFiltro.SelectedValue == "numero")
                {
                    meeFiltro.Enabled = false;
                }
                else
                {
                    meeFiltro.Enabled = true;
                }
            }
        }
    }
}