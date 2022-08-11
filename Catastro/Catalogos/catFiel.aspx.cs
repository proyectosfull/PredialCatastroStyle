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
    public partial class catFiel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["sortCampo"] = "Id";
                ViewState["sortOrden"] = "asc";
                chkInactivo.Checked = true;
                llenagrid();
                llenaFiltro();
                txtFiltro.Enabled = false;
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        private void llenaFiltro()
        {           
            ddlFiltro.DataSource = new cFIELBL().ListaCampos();
            ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
        }

        #region Grid
        private void llenagrid()
        {
          string[] filtro =  (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new cFIELBL().GetFilter("","","TRUE",ViewState["sortCampo"].ToString(), ViewState["sortOrden"].ToString());
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new cFIELBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOrden"].ToString());
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
                parametro.Add("idFIEL", id);
                parametro.Add("tipoPantalla", "C");
                Session["parametro"] = parametro;
                Response.Redirect("catFIELCapMod.aspx");
            }
          else if (e.CommandName == "ModificarRegistro")
            {
                string id = e.CommandArgument.ToString();
                Dictionary<string, string> parametro = new Dictionary<string, string>();
                parametro.Add("idFIEL", id);
                parametro.Add("tipoPantalla", "M");
                Session["parametro"] = parametro;
                Response.Redirect("catFIELCapMod.aspx");

            }
            else if (e.CommandName == "EliminarRegistro")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;
                vtnModal.DysplayCancelar = true;            
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar), ModalPopupMensaje.TypeMesssage.Confirm);
            }
            else if (e.CommandName == "ActivarRegistro")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;
                if (new cFIELBL().GetAll().Count == 0)
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro), ModalPopupMensaje.TypeMesssage.Confirm);
                }
                else
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.FIELVigente), ModalPopupMensaje.TypeMesssage.Alert);
                }
                
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

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            if (new cFIELBL().GetAll().Count == 0)
            {
                Session["parametro"] = null;
                Response.Redirect("catFIELCapMod.aspx");
            }
            else
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.FIELVigente), ModalPopupMensaje.TypeMesssage.Alert);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (new cFIELBL().GetAll().Count == 0)
            {
                cUsuarios U = new cUsuarios();
                U = (cUsuarios)Session["usuario"];
                pnl_Modal.Hide();
                vtnModal.DysplayCancelar = false;
                if (ViewState["idMod"] == null || ViewState["idMod"].ToString() == string.Empty || ViewState["idMod"].ToString() == "0")
                {
                    cFIEL FIEL = new cFIEL();


                    FIEL.IdUsuario = U.Id;
                    FIEL.Activo = true;
                    FIEL.FechaModificacion = DateTime.Now;
                    MensajesInterfaz msg = new cFIELBL().Insert(FIEL);
                    vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                }
                else
                {
                    cFIEL FIEL = new cFIELBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                    
                    FIEL.IdUsuario = U.Id;
                    FIEL.FechaModificacion = DateTime.Now;
                    MensajesInterfaz msg = new cFIELBL().Update(FIEL);
                    vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                }
            }
            else
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.SalarioVigente), ModalPopupMensaje.TypeMesssage.Alert);
            }
           
            limpiaCampos();
            llenagrid();

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ViewState["idMod"] = 0;
            limpiaCampos();
            pnl_Modal.Hide();
        }

        private void limpiaCampos()
        {
           
            ViewState["idMod"] = null;
        }
        private void habilitaCampos(bool activa)
        {
            
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];
            
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {               
                cFIEL FIEL = new cFIELBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                FIEL.Activo = false;
                FIEL.IdUsuario = U.Id;
                FIEL.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cFIELBL().Delete(FIEL);               
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                cFIEL FIEL = new cFIELBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                FIEL.Activo = true;
                FIEL.IdUsuario = U.Id;
                FIEL.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cFIELBL().Update(FIEL);                
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Ingreso) ||
              vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Actualizacion))
            {
                llenagrid();
                limpiaCampos();
            }
        }

        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            string[] filtro = new string[] { ddlFiltro.SelectedValue, txtFiltro.Text ,chkInactivo.Checked.ToString()};            
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

        protected void btnConsultarCreditos_Click(object sender, EventArgs e)
        {
            GeneraRecibo33.ReciboCFDI33 r = new GeneraRecibo33.ReciboCFDI33();
            System.Data.DataTable table = r.ConsultarCreditos();
            grdTimbres.DataSource = table;
            grdTimbres.DataBind();
            modalPnlCreditos.Show();


            //if (datos.Count() == 7)
            //{
            //    lblFechaActivacion.Text = datos[0];
            //    lblFechaVencimiento.Text = datos[1];
            //    lblPaquete.Text = datos[2];
            //    lblTimbres.Text = datos[3];
            //    lblTimbresRestantes.Text = datos[4];
            //    lblTimbresUsados.Text = datos[5];
            //    lblVigente.Text = datos[6];
            //    modalPnlCreditos.Show();
            //}
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            modalPnlCreditos.Hide();          
        }
        
        protected void grdTimbres_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdTimbres.PageIndex = e.NewPageIndex;
            GeneraRecibo33.ReciboCFDI33 r = new GeneraRecibo33.ReciboCFDI33();
            System.Data.DataTable table = r.ConsultarCreditos();
            grdTimbres.DataSource = table;
            grdTimbres.DataBind();
            modalPnlCreditos.Show();
        }
    }
}