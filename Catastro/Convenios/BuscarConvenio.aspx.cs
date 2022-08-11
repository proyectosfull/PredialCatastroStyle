using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Catastro.Controles;
using Clases;
using Clases.BL;
using Clases.Utilerias;
using System.Transactions;

namespace Catastro.Convenios
{
    public partial class BuscarConvenio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["sortCampo"] = "Folio";
                ViewState["sortOnden"] = "asc";
                chkInactivo.Checked = true;
                llenagrid();
                //llenaFiltro();
                txtFiltro.Enabled = false;
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        //private void llenaFiltro()
        //{
        //    ddlFiltro.DataSource = new tConvenioBL().ListaCampos();
        //    ddlFiltro.DataBind();
        //    ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
        //}

        #region Grid
        private void llenagrid()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new tConvenioBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();
            }
            else
            {
                if (filtro[0] == "clave")
                {
                    int Idpredial = new cPredioBL().GetByClavePredial(filtro[1].ToString()).Id;                    
                    grd.DataSource = new tConvenioBL().GetFilter("IdPredio", Idpredial.ToString(), filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                    grd.DataBind();
                }
                else
                {
                    grd.DataSource = new tConvenioBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                    grd.DataBind();
                }

            }
        }

        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd.PageIndex = e.NewPageIndex;
            llenagrid();
        }

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "ConsultarRegistro")
                Response.Redirect("Convenio.aspx?idConvenio=" + id.ToString() + "&tipo=c");
            else if (e.CommandName == "ModificarRegistro")
                Response.Redirect("Convenio.aspx?idConvenio=" + id.ToString() + "&tipo=m");
            else if (e.CommandName == "EliminarRegistro")
            {
                ViewState["idMod"] = id;
                vtnModal.DysplayCancelar = true;
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar), ModalPopupMensaje.TypeMesssage.Confirm);
            }
            else if (e.CommandName == "ActivarRegistro")
            {
                ViewState["idMod"] = id;
                vtnModal.DysplayCancelar = true;
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
                Int32 id = Convert.ToInt32(e.Row.Cells[0].Text);
                tConvenioEdoCta tedo = new tConvenioEdoCtaBL().GetByConstraint(id);
                e.Row.Cells[0].Text = tedo.cPredio.ClavePredial; //Convert.ToInt32(e.Row.Cells[0].Text))[0].cPredio.ClavePredial;

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
            llenargridPredio();
            pnl_Modal.Show();
          
        }

        private void llenargridPredio()
        {
            grdPredio.DataSource = new tConvenioEdoCtaBL().GetAllForConveniosDelMes();
            grdPredio.DataBind();            
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                 MensajesInterfaz resul;
                 using (TransactionScope scope = new TransactionScope())
                 {
                     //ViewState["idMod"]  -----> Id convenio
                     tConvenio convenio = new tConvenioBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                     convenio.Activo = false;
                     convenio.IdUsuario = U.Id;
                     convenio.FechaModificacion = DateTime.Now;
                     convenio.Status = "C";
                     resul = new tConvenioBL().Update(convenio);
                     if (resul == MensajesInterfaz.Actualizacion)
                     {
                         tConvenioEdoCta convedocta = new tConvenioEdoCtaBL().GetByConstraint(convenio.IdConvenioEdoCta);
                         convedocta.Activo = false;
                         convedocta.IdUsuario = U.Id;
                         convedocta.FechaModificacion = DateTime.Now;
                         convedocta.Status = "C";
                         resul = new tConvenioEdoCtaBL().Update(convedocta);
                     }
                     if (resul == MensajesInterfaz.Actualizacion)
                     {
                         List<tTramite> tramites = new tTramiteBL().GetFilter("IdConvenio", convenio.Id.ToString(), "true", "NoParcialidad", "asc");
                         if (tramites != null)
                         {
                             Int32 fila = 0;
                             Boolean guardar = true;
                             while (fila < tramites.Count && guardar)
                             {
                                 tTramite tramite = tramites[fila];
                                 tramite.Activo = false;
                                 if (tramite.Status == "A" )
                                    tramite.Status = "C";
                                 tramite.FechaModificacion = DateTime.Today;
                                 tramite.IdUsuario = U.Id;
                                 resul = new tTramiteBL().Update(tramite);
                                 if (resul == MensajesInterfaz.Actualizacion)
                                 {

                                    if (tramite.Status != "P")
                                    {
                                        List<tTramiteDetalle> tramdet = new tTramiteDetalleBL().GetAllIdTramite(tramite.Id);
                                        foreach(tTramiteDetalle tr in tramdet)
                                        {
                                             tr.Activo = false;
                                             resul = new tTramiteDetalleBL().Update(tr);
                                        }
                                    }
                                    fila = fila + 1;
                                 }
                                 else
                                     guardar = false;
                             }
                             if (guardar)
                             {
                                 List<tConvenioEdoCta> Predios = new tConvenioEdoCtaBL().GetByFolio(convenio.IdConvenioEdoCta).Where(p => p.Status == "A").ToList();
                                 if (Predios != null)
                                 {
                                     fila = 0;
                                     while (fila < Predios.Count && guardar)
                                     {
                                         tConvenioEdoCta predio = Predios[fila];
                                         predio.Status = "C";
                                         predio.Activo = false;
                                         resul = new tConvenioEdoCtaBL().Update(predio);
                                         if (resul == MensajesInterfaz.Actualizacion)
                                         {
                                             fila = fila + 1;
                                         }
                                         else
                                             guardar = false;
                                     }
                                     if (guardar)
                                         scope.Complete();
                                 }
                             }
                         }
                     }
                 }
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                tConvenio convenio = new tConvenioBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                convenio.Activo = true;
                convenio.Status = "A";
                convenio.IdUsuario = U.Id;
                convenio.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new tConvenioBL().Update(convenio);

                vtnModal.DysplayCancelar = false;
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
            if (ddlFiltro.SelectedValue.ToString() == "")
            {
                txtFiltro.Text = "";
                txtFiltro.Enabled = false;
            }
            else
            {
                txtFiltro.Enabled = true;
            }
        }

        protected void grdPredio_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string clave = Convert.ToString(e.CommandArgument);
            if (e.CommandName == "Selec")
                Response.Redirect("Convenio.aspx?Clave=" + clave );
           
        }
             
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            grdPredio.DataSource = null;
            grdPredio.DataBind();            
            pnl_Modal.Hide();
        }
    }
}