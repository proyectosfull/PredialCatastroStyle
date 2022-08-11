using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Transactions;
using System.Web.UI.WebControls;
using Clases.BL;
using Catastro.Controles;
using Clases;
using Clases.Utilerias;
using System.IO;
using Microsoft.Reporting.WebForms;

namespace Catastro.Servicios
{
    public partial class catastroTramite : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["parametro"] = null;
                ViewState["idConcepto"] = null;
                ViewState["idTD"] = null;
                ViewState["claveGenerica"] = null;
                txtClvCastatral.Focus();
                lblCuentaPredialTxt.Text = "";
                lblCuentaPredial.Text = "";
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        private void llenarConcepto()
        {
            cTipoTramite cTipoTramite = new cTipoTramiteBL().GetByConstraint(4);
            String mesa = cTipoTramite.IdMesa.ToString();
            List<cConcepto> listConcepto = new cConceptoBL().listConceptosCatastro(DateTime.Now.Year, Convert.ToInt32(mesa));
            if (listConcepto.Count > 0)
            {
                if (grd.Rows.Count > 0)
                {
                    foreach (GridViewRow gvr in grd.Rows)
                    {
                        int index = -1;
                        Label clave = gvr.FindControl("lblClave") as Label;
                        for (int i = 0; i < listConcepto.Count; i++)
                            if (clave.Text == listConcepto[i].Id.ToString())
                            {
                                index = i;
                            }
                        if (index != -1)
                        {
                            listConcepto.RemoveAt(index);
                        }
                    }
                }
            }

            var lista = listConcepto.Select(o => new
            {
                Id = o.Id,
                Nombre = o.Cri + " -- " + o.Nombre
            });

            ddlConcepto.Items.Clear();
            ddlConcepto.DataValueField = "Id";
            ddlConcepto.DataTextField = "Nombre";
            ddlConcepto.DataSource = lista;
            ddlConcepto.DataBind();
            ddlConcepto.Items.Insert(0, new ListItem("Selecciona el Concepto", "%"));
        }

        protected void cancelarTramite()
        {
            try
            {
                tTramite tramite = new tTramiteBL().GetByConstraint(Convert.ToInt32(ViewState["idT"]));
                if (tramite != null)
                {
                    cUsuarios U = new cUsuarios();
                    U = (cUsuarios)Session["usuario"];
                    MensajesInterfaz msg = new MensajesInterfaz();
                    tramite.Status = "C";
                    tramite.IdUsuario = U.Id;
                    tramite.FechaModificacion = DateTime.Now;
                    msg = new tTramiteBL().Update(tramite);
                    buscarClaveCatastral(null, null);
                    vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                }
                else
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription("Imposible Realizar el Cambio, el tramite no se encuentra"), ModalPopupMensaje.TypeMesssage.Alert);
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("catastroTramite.cancelarTramite.Exception", ex);
                vtnModal.ShowPopup(new Utileria().GetDescription(ex.Message), ModalPopupMensaje.TypeMesssage.Alert);
            }
        }

        protected void guardarConcepto()
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (grdAlta.Rows.Count == 0 && grd.Rows.Count == 0)
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription("Debe de Agregar un concepto"), ModalPopupMensaje.TypeMesssage.Alert);
                        return;
                    }

                    cUsuarios U = new cUsuarios();
                    U = (cUsuarios)Session["usuario"];
                    cPredio predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);

                    if (new cParametroSistemaBL().GetValorByClave("VALIDACIONFECHAAVALUO") == "SI")
                    {
                        if (predio.FechaAvaluo < (DateTime.Now.AddYears(-2)))
                        {
                            throw new System.ArgumentException(new Utileria().GetDescription(MensajesInterfaz.NoAGenerarTramite));
                        }
                    }
                    MensajesInterfaz msg = MensajesInterfaz.Actualizacion;
                    if (grdAlta.Rows.Count > 0)
                    {
                        foreach (GridViewRow gvr in grdAlta.Rows)
                        {
                            Label clave = gvr.FindControl("lblClave") as Label;
                            Label Descripcion = gvr.FindControl("lblDescripcion") as Label;
                            Label Id = gvr.FindControl("lblId") as Label;
                            Label Status = gvr.FindControl("lblStatus") as Label;
                            Label Activo = gvr.FindControl("lblActivo") as Label;
                            Label Cambio = gvr.FindControl("lblCambio") as Label;
                            Label Importe = gvr.FindControl("lblImporte") as Label;
                            Label NombreAdquiriente = gvr.FindControl("lblNombreAdquiriente") as Label;
                            tTramite tramite = new tTramite();
                            if (Cambio.Text == "M")
                            {
                                tramite = new tTramiteBL().GetByConstraint(Convert.ToInt32(Id.Text));
                                if (tramite == null)
                                {
                                    throw new System.ArgumentException(new Utileria().GetDescription("Imposible Realizar el Cambio, el tramite no se encuentra"));
                                }
                                if (tramite.Status == "P")
                                {
                                    throw new System.ArgumentException(new Utileria().GetDescription("Imposible Realizar el Cambio, el tramite se encuentra en estado Pagado"));
                                }

                                if (!Convert.ToBoolean(Activo.Text) && tramite.Status == "A")
                                {
                                    tramite.Status = "C";
                                    tramite.IdUsuario = U.Id;
                                    tramite.FechaModificacion = DateTime.Now;
                                    msg = new tTramiteBL().Update(tramite);
                                }
                            }

                            if (msg != MensajesInterfaz.Actualizacion)
                            {
                                throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                            }
                            //ViewState["idT"] = tramite.Id;
                        }
                    }

                    if (grd.Rows.Count > 0)
                    {
                        tTramite tramite = new tTramiteBL().GetByConstraint(Convert.ToInt32(ViewState["idT"]));
                        if (tramite == null && Convert.ToInt32(ViewState["idT"]) == 0)
                        {
                            int nTramites = txtNTramites.Text == "0" ? 1 : Convert.ToInt32(txtNTramites.Text);
                            for (int i = 0; i < nTramites; i++)
                            {
                                tramite = new tTramite();
                                tramite.IdPredio = predio.Id;
                                tramite.FechaEstimado = Convert.ToDateTime(txtFechaEntrega.Text);
                                tramite.Fecha = DateTime.Now;
                                tramite.Status = "A";
                                tramite.IdTipoTramite = 4;
                                tramite.Activo = true;
                                tramite.IdUsuario = U.Id;
                                tramite.FechaModificacion = DateTime.Now;
                                tramite.Observacion = txtObservaciones.Text;
                                tramite.tTramiteDetalle = llenarTramiteDetalle(tramite);
                                if (txtContribuyente.Text != "")
                                {
                                    tramite.NombreAdquiriente = txtContribuyente.Text;
                                }
                                msg = new tTramiteBL().Insert(tramite);
                                if (msg != MensajesInterfaz.Ingreso)
                                {
                                    throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                                }

                                //ViewState["idT"] = nTramites==0? tramite.Id:0;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(ViewState["idT"]) != 0 && tramite == null)
                            {
                                throw new System.ArgumentException(new Utileria().GetDescription("Imposible Realizar el Cambio, el tramite no se encuentra"));
                            }

                            if (tramite.Status != "A")
                            {
                                throw new System.ArgumentException(new Utileria().GetDescription("Imposible Realizar el Cambio, el tramite se encuentra en estado " +
                                    (tramite.Status == "P" ? "Pagado" : "Cancelado")));
                            }
                            //ViewState["idT"] = tramite.Id;                            
                            if (txtContribuyente.Text != "")
                            {
                                tramite.FechaEstimado = Convert.ToDateTime(lblCuentaPredialTxt.Text);
                                tramite.NombreAdquiriente = txtContribuyente.Text;
                                tramite.IdUsuario = U.Id;
                                tramite.FechaModificacion = DateTime.Now;
                                msg = new tTramiteBL().Update(tramite);
                            }

                            if (msg == MensajesInterfaz.Actualizacion)
                            {
                                foreach (tTramiteDetalle td1 in llenarTramiteDetalle(tramite))
                                {
                                    if (td1.Id == 0)
                                    {
                                        td1.IdTramite = td1.tTramite.Id;
                                        td1.tTramite = null;
                                        msg = new tTramiteDetalleBL().Insert(td1);
                                    }
                                    else
                                    {
                                        msg = new tTramiteDetalleBL().Update(td1);
                                    }

                                    if (msg != MensajesInterfaz.Ingreso && msg != MensajesInterfaz.Actualizacion)
                                    {
                                        throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                                    }
                                }
                                msg = MensajesInterfaz.Actualizacion;
                            }
                            else
                            {
                                throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                            }
                        }
                    }

                    vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                    buscarClaveCatastral(null, null);
                    scope.Complete();
                }
            }
            catch (Exception error)
            {
                new Utileria().logError("catastroTramite.cancelarTramite.Exception", error);
                vtnModal.ShowPopup(new Utileria().GetDescription(error.Message), ModalPopupMensaje.TypeMesssage.Alert);

            }
        }

        protected List<tTramiteDetalle> llenarTramiteDetalle(tTramite tramite)
        {
            List<tTramiteDetalle> listaTramiteDetalle = new List<tTramiteDetalle>();
            if (grd.Rows.Count > 0)
            {
                cUsuarios U = new cUsuarios();
                U = (cUsuarios)Session["usuario"];
                foreach (GridViewRow gvr in grd.Rows)
                {
                    Label clave = gvr.FindControl("lblClave") as Label;
                    Label Descripcion = gvr.FindControl("lblDescripcion") as Label;
                    Label Id = gvr.FindControl("lblId") as Label;
                    Label Status = gvr.FindControl("lblStatus") as Label;
                    Label Activo = gvr.FindControl("lblActivo") as Label;
                    Label Cambio = gvr.FindControl("lblCambio") as Label;
                    Label Importe = gvr.FindControl("lblImporte") as Label;
                    tTramiteDetalle concepto = null;
                    if (Cambio.Text == "N")
                    {
                        concepto = new tTramiteDetalle();
                        concepto.Activo = true;
                    }
                    else
                    {
                        if (Cambio.Text == "M")
                        {
                            concepto = new tTramiteDetalleBL().getIdTramiteByIdConcepto(tramite.Id, Convert.ToInt32(clave.Text));
                            if (concepto == null)
                            {
                                throw new System.ArgumentException(new Utileria().GetDescription("Imposible actualizar, no se encontro el detalle del tramite con Concepto: " + Descripcion.Text), "Error");
                            }
                            concepto.Activo = Convert.ToBoolean(Activo.Text);
                        }
                        else
                        {
                            continue;
                        }
                    }

                    cConcepto conc = new cConceptoBL().GetByConstraint(Convert.ToInt32(clave.Text));

                    concepto.IdConcepto = Convert.ToInt32(clave.Text);
                    concepto.Importe = Convert.ToDecimal(Importe.Text.Replace("$", ""));
                    concepto.IdUsuario = U.Id;
                    concepto.FechaModificacion = DateTime.Now;
                    concepto.tTramite = tramite;
                    concepto.IdtipoTramite = 0;
                    if (conc.IdTipoTramite == 7 || conc.IdTipoTramite == 1 )
                        concepto.IdtipoTramite = Convert.ToInt32( conc.IdTipoTramite);
                     
                    listaTramiteDetalle.Add(concepto);
                    
                }
            }
            return listaTramiteDetalle;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {            
            btnEstado.Visible = true;
            vtnModal.DysplayCancelar = true;
            vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfirmacionGuardado), ModalPopupMensaje.TypeMesssage.Confirm);
        }

        protected void btnRecargar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ViewState["mensajeCambio"]) > 0 || Convert.ToInt32(ViewState["mensajeCambioAlta"]) > 0)
            {
                vtnModal.DysplayCancelar = true;
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.RegresarMSG), ModalPopupMensaje.TypeMesssage.Confirm);
            }
            else
            {
                Response.Redirect("catastroTramite.aspx");
            }
        }
        
        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EliminarRegistro")
            {
                string id = e.CommandArgument.ToString();

                if (id.Substring(0, id.Length - 1) != "N")
                {
                    eliminarActivarConcepto(false, id);
                }
                else
                {
                    eliminarActivarConcepto(false, id);
                    llenarConcepto();
                }
                if (grd.Rows.Count == 0)
                {
                    txtClvCastatral.ReadOnly = false;
                }
                pnlReport.Visible = false;
                btnEstado.Visible = false;
                rpt.LocalReport.DataSources.Clear();
            }
            else if (e.CommandName == "ActivarRegistro")
            {
                string id = e.CommandArgument.ToString();
                eliminarActivarConcepto(true, id);
                pnlReport.Visible = false;
                btnEstado.Visible = false;
                rpt.LocalReport.DataSources.Clear();
            }
            else if (e.CommandName == "EditarImporte")
            {

                tTramiteDetalle td = new tTramiteDetalleBL().GetByConstraint(Convert.ToInt32(e.CommandArgument.ToString()));
                ViewState["idConcepto"] = td.cConcepto.Id;
                ViewState["idTD"] = td.Id;
                btnAgregarConcepto_Click(null, null);
                pnlReport.Visible = false;
                btnEstado.Visible = false;
                rpt.LocalReport.DataSources.Clear();
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
        }

        protected void btnAgregarConcepto_Click(object sender, EventArgs e)
        {
            pnlReport.Visible = false;
            btnEstado.Visible = false;
            rpt.LocalReport.DataSources.Clear();
            ViewState["total"] = 0;
            Int32 id = ViewState["idConcepto"] == null ? Convert.ToInt32(ddlConcepto.SelectedValue) : Convert.ToInt32(ViewState["idConcepto"]);
            cConcepto concepto = new cConceptoBL().GetByConstraint(id);
            txtImporte.Text = "";
            txtUma.Text = "";
            pnl_Modal.Show();
            switch (concepto.TipoCobro)
            {
                case "S":
                    String clave = new cParametroSistemaBL().GetByTipoCampoCobro("cobroTCatastro");
                    txtImporte.Visible = false;
                    txtUma.Visible = true;
                    btnAceptarUma.Visible = true;
                    btnAceptarImporte.Visible = false;
                    lblImporte.Visible = false;
                    lblUma.Visible = true;
                    switch (clave)
                    {
                        case "":
                            vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.DefinirSerie), ModalPopupMensaje.TypeMesssage.Alert);
                            break;
                        case "cobroCatastroUMAMax":
                        case "cobroCatastroUMAMin":
                            btnAceptarCobro_Click(null, null);
                            pnl_Modal.Hide();
                            break;
                        case "cobroCatastroUMASalario":
                            validadorSalario.Type = ValidationDataType.Integer;
                            validadorSalario.MinimumValue = concepto.SalarioMin.ToString();
                            validadorSalario.MaximumValue = concepto.SalarioMax.ToString();
                            validadorSalario.ErrorMessage = "El Número de UMA no puede ser menor de " + concepto.SalarioMin + " y mayor a " +
                             concepto.SalarioMax + " (Solo Números enteros)";
                            break;
                        default:
                            vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.DefinirSerie), ModalPopupMensaje.TypeMesssage.Alert);
                            break;

                    }

                    break;
                case "I":
                    lblImporte.Visible = true;
                    lblUma.Visible = false;
                    txtImporte.Visible = true;
                    txtUma.Visible = false;
                    btnAceptarUma.Visible = false;
                    btnAceptarImporte.Visible = true;
                    break;
                default:
                    vtnModal.ShowPopup(new Utileria().GetDescription("Tipo de Cobro no definido"), ModalPopupMensaje.TypeMesssage.Alert);
                    break;

            }


        }

        //private void llenaGridAltas(Int32 idTramite, Decimal importe, string nombreAdquiriente, cConcepto con)//, Decimal NSalarioImporte)
        //{
        //    List<conceptoGrid> listConcepto = new List<conceptoGrid>();
        //    int index = 1;
        //    int idMod = 0;
        //    Boolean continuar = true;
        //    if (Convert.ToInt32(ViewState["idMod"]) != 0)
        //        idMod = Convert.ToInt32(ViewState["idMod"]);
        //    if (grdAlta.Rows.Count > 0)
        //    {
        //        foreach (GridViewRow gvr in grdAlta.Rows)
        //        {
        //            Label clave = gvr.FindControl("lblClave") as Label;
        //            Label Descripcion = gvr.FindControl("lblDescripcion") as Label;
        //            Label Id = gvr.FindControl("lblId") as Label;
        //            Label Status = gvr.FindControl("lblStatus") as Label;
        //            Label Activo = gvr.FindControl("lblActivo") as Label;
        //            Label Cambio = gvr.FindControl("lblCambio") as Label;
        //            Label Importe = gvr.FindControl("lblImporte") as Label;
        //            Label TipoCobro = gvr.FindControl("lblTipoCobro") as Label;
        //            Label NombreAdquiriente = gvr.FindControl("lblNombreAdquiriente") as Label;

        //            conceptoGrid conceptoAux = new conceptoGrid();
        //            conceptoAux.Clave = clave.Text;
        //            conceptoAux.Descripcion = Descripcion.Text;
        //            conceptoAux.Cambio = Cambio.Text;
        //            conceptoAux.Activo = Convert.ToBoolean(Activo.Text);
        //            conceptoAux.Importe = Importe.Text;
        //            conceptoAux.TipoCobro = TipoCobro.Text;
        //            conceptoAux.NombreAdquiriente = NombreAdquiriente.Text;
        //            if (ViewState["idTD"] != null)
        //            {
        //                if (Convert.ToString(Id.Text) == Convert.ToString(ViewState["idTD"]))
        //                {
        //                    conceptoAux.Importe = importe.ToString("C");
        //                    conceptoAux.NombreAdquiriente = nombreAdquiriente;
        //                    ViewState["idConcepto"] = null;
        //                    ViewState["idTD"] = null;
        //                    continuar = false;
        //                    if (Id.Text.Substring(0, 1) != "N")
        //                    {
        //                        conceptoAux.Cambio = "M";
        //                    }
        //                }
        //            }

        //            if (Id.Text.Substring(0,1) == "N")
        //            {
        //                conceptoAux.Id = "N" + index;
        //                index++;
        //            }
        //            else
        //            {
        //                conceptoAux.Id = Id.Text;
        //            }
        //            conceptoAux.Status = Status.Text;
        //            if (idMod != 0)
        //            {
        //                if (idMod.ToString() != Id.Text)
        //                {
        //                    listConcepto.Add(conceptoAux);
        //                }
        //            }
        //            else
        //            {
        //                listConcepto.Add(conceptoAux);
        //            }
        //        }
        //    }
        //    if (continuar)
        //    {
        //        int auxContador = 0;
        //        if (chkMismoImporte.Checked && chkMismoContribuyente.Checked)
        //        {
        //            auxContador = Convert.ToInt16(txtNTramites.Text);
        //        }
        //        else
        //        {
        //            auxContador = grdAlta.Rows.Count + 1;
        //        }

        //        for (int i = grdAlta.Rows.Count == 0 ? 1 : grdAlta.Rows.Count+1; i <= auxContador; i++)
        //        {
        //        conceptoGrid concepto = new conceptoGrid();
        //        if (idMod != 0)
        //        {

        //            concepto.Activo = true;
        //            concepto.Status = "A";
        //            concepto.Id = "1";//Pendiente de cambio
        //            concepto.Cambio = "M";
        //        }
        //        else
        //        {
        //            concepto.Activo = false;
        //            concepto.Status = "A";
        //            concepto.Id = "N" + i.ToString();
        //            concepto.Cambio = "N";
        //        }
        //        concepto.TipoCobro = con.TipoCobro;
        //        concepto.Clave = con.Id.ToString();
        //        concepto.Descripcion = con.Cri + " -- " + con.Nombre;
        //        concepto.Importe = importe.ToString("C");
        //        concepto.NombreAdquiriente = nombreAdquiriente;
        //        listConcepto.Add(concepto);
        //    }
        //        grdAlta.DataSource = listConcepto;
        //        grdAlta.DataBind();

        //        txtClvCastatral.ReadOnly = true;
        //        llenarConcepto();
        //        btnAgregarConcepto.Text = "Agregar Concepto";
        //    }
        //    else
        //    {
        //        grdAlta.DataSource = listConcepto;
        //        grdAlta.DataBind();
        //    }

        //}

        private void llenaGridConceptos(Int32 idTramite, Decimal importe, string nombreAdquiriente, cConcepto con)//, Decimal NSalarioImporte)
        {
            List<conceptoGrid> listConcepto = new List<conceptoGrid>();
            int index = 1;
            int idMod = 0;
            Boolean continuar = true;
            if (Convert.ToInt32(ViewState["idMod"]) != 0)
                idMod = Convert.ToInt32(ViewState["idMod"]);
            if (grd.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in grd.Rows)
                {
                    Label clave = gvr.FindControl("lblClave") as Label;
                    Label Descripcion = gvr.FindControl("lblDescripcion") as Label;
                    Label Id = gvr.FindControl("lblId") as Label;
                    Label Status = gvr.FindControl("lblStatus") as Label;
                    Label Activo = gvr.FindControl("lblActivo") as Label;
                    Label Cambio = gvr.FindControl("lblCambio") as Label;
                    Label Importe = gvr.FindControl("lblImporte") as Label;
                    Label TipoCobro = gvr.FindControl("lblTipoCobro") as Label;
                    //Label NombreAdquiriente = gvr.FindControl("lblNombreAdquiriente") as Label;

                    conceptoGrid conceptoAux = new conceptoGrid();
                    conceptoAux.Clave = clave.Text;
                    conceptoAux.Descripcion = Descripcion.Text;
                    conceptoAux.Cambio = Cambio.Text;
                    conceptoAux.Activo = Convert.ToBoolean(Activo.Text);
                    conceptoAux.Importe = Importe.Text;
                    conceptoAux.TipoCobro = TipoCobro.Text;
                    //conceptoAux.NombreAdquiriente = NombreAdquiriente.Text;
                    if (ViewState["idTD"] != null)
                    {
                        if (Convert.ToInt32(Id.Text) == Convert.ToInt32(ViewState["idTD"]))
                        {
                            conceptoAux.Importe = importe.ToString("C");
                            //conceptoAux.NombreAdquiriente = nombreAdquiriente;
                            conceptoAux.Cambio = "M";
                            ViewState["idConcepto"] = null;
                            ViewState["idTD"] = null;
                            continuar = false;
                        }
                    }

                    if (Id.Text.Substring(0, 1) == "N")
                    {
                        conceptoAux.Id = "N" + index;
                        index++;
                    }
                    else
                    {
                        conceptoAux.Id = Id.Text;
                    }
                    conceptoAux.Status = Status.Text;
                    if (idMod != 0)
                    {
                        if (idMod.ToString() != Id.Text)
                        {
                            listConcepto.Add(conceptoAux);
                        }
                    }
                    else
                    {
                        listConcepto.Add(conceptoAux);
                    }
                }
            }
            if (continuar)
            {
                conceptoGrid concepto = new conceptoGrid();
                if (idMod != 0)
                {

                    concepto.Activo = true;
                    concepto.Status = "A";
                    concepto.Id = "1";//Pendiente de cambio
                    concepto.Cambio = "M";
                }
                else
                {
                    concepto.Activo = false;
                    concepto.Status = "A";
                    concepto.Id = "N" + index;
                    concepto.Cambio = "N";
                }
                concepto.TipoCobro = con.TipoCobro;
                concepto.Clave = con.Id.ToString();
                concepto.Descripcion = con.Cri + " -- " + con.Nombre;
                concepto.Importe = importe.ToString("C");
                concepto.NombreAdquiriente = nombreAdquiriente;
                listConcepto.Add(concepto);
                grd.DataSource = listConcepto;
                grd.DataBind();

                txtClvCastatral.ReadOnly = true;
                llenarConcepto();
                btnAgregarConcepto.Text = "Agregar Concepto";
            }
            else
            {
                grd.DataSource = listConcepto;
                grd.DataBind();
            }
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.RegresarMSG))
            {
                Response.Redirect("catastroTramite.aspx");
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfirmacionGuardado))
            {
                guardarConcepto();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfirmacionCancelar))
            {
                cancelarTramite();
            }
        }

        protected void buscarClaveCatastral(object sender, System.EventArgs e)
        {
            ViewState["total"] = 0;

            int eActual = DateTime.Now.Year;
            double mesActual = Utileria.Redondeo(DateTime.Now.Month / 2.0);
            int bActual = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(DateTime.Now.Month / 2.0)));

            if (txtClvCastatral.Text.Length == 12)
            {
                cPredio Predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);
                if (Predio == null)
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.PredioInexistente), ModalPopupMensaje.TypeMesssage.Alert);
                    txtClvCastatral.Text = "";
                    limpiarPant();
                }
                else
                {
                    if (!Predio.Activo)
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.PredioInactivo), ModalPopupMensaje.TypeMesssage.Alert);
                        txtClvCastatral.Text = "";
                    }
                    else if (eActual * 10 + bActual > Predio.AaFinalIp * 10 + Predio.BimestreFinIp)
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription("El predio tiene un adeudo, no es posible generar tramites catastrales"), ModalPopupMensaje.TypeMesssage.Alert);
                        txtClvCastatral.Text = "";
                    }
                    else if (Predio.cStatusPredio.Descripcion == "S")
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription("El predio esta suspendido, favor de revisar la clave con el administrador"), ModalPopupMensaje.TypeMesssage.Alert);
                        txtClvCastatral.Text = "";
                    }
                    else if (Predio.cStatusPredio.Descripcion == "B")
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription("El predio esta dado de baja, favor de revisar la clave con el administrador"), ModalPopupMensaje.TypeMesssage.Alert);
                        txtClvCastatral.Text = "";
                    }
                    else
                    {
                        ViewState["IdPredio"] = Predio.Id;
                        ViewState["idMod"] = 0;
                        ViewState["mensajeCambio"] = 0;
                        ViewState["mensajeCambioAlta"] = 0;
                        ViewState["idT"] = 0;
                        ViewState["claveGenerica"] = new cParametroSistemaBL().GetValorByClave("CLAVE_CATASTRAL_GENERICA");
                        llenaPant(Predio);
                        if (ViewState["claveGenerica"].ToString().Trim() == Predio.ClavePredial.Trim())
                        {
                            grdAlta.Visible = true;
                            grd.Visible = true;
                            llenaGridTramite(Predio.Id);
                        }
                        else
                        {
                            grd.Visible = true;
                            llenaGridConcepto(Predio.Id, 0);
                        }
                        llenarConcepto();
                    }
                    
                }
            }
            else
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.FormatoPredio), ModalPopupMensaje.TypeMesssage.Alert);
                txtClvCastatral.Text = "";
                limpiarPant();
            }
        }

        protected void limpiarPant()
        {
            ViewState["idP"] = 0;
            txtContruyente.Text = "";
            txtCalle.Text = "";
            txtNumero.Text = "";
            txtColonia.Text = "";
            txtCP.Text = "";
            txtLocalidad.Text = "";
            tablaDatosAltas.Visible = false;
            tablaColumnaGrid.Visible = false;
            divGridAlta.Visible = false;
            revContribuyente.Enabled = false;
            rfvNTramites.Enabled = false;
            revNTramites.Enabled = false;
            btnCancelarTramite.Visible = false;
            txtContribuyente.Text = "";
            txtNTramites.Text = "1";
            txtNTramites.ReadOnly = false;
            grdAlta.Visible = false;
            grdAlta.DataSource = null;
            grdAlta.DataBind();
            grd.Visible = false;
            grd.DataSource = null;
            grd.DataBind();
            btnCancelarTramite.Visible = false;
            lblCuentaPredialTxt.Text = "";
            lblCuentaPredial.Text = "";
        }

        protected void llenaPant(cPredio Predio)
        {
            limpiarPant();
            ViewState["idP"] = Predio.Id.ToString();
            txtContruyente.Text = Predio.cContribuyente.ApellidoPaterno + " " + Predio.cContribuyente.ApellidoMaterno + " " + Predio.cContribuyente.Nombre;
            txtCalle.Text = Predio.Calle;
            txtNumero.Text = Predio.Numero;
            txtColonia.Text = Predio.cColonia.NombreColonia;
            txtCP.Text = Predio.CP;
            txtLocalidad.Text = Predio.Localidad;
            Boolean activaAlta = Predio.ClavePredial.Trim() == ViewState["claveGenerica"].ToString().Trim() ? true : false;
            tablaDatosAltas.Visible = activaAlta;
            tablaColumnaGrid.Visible = activaAlta;
            divGridAlta.Visible = activaAlta;
            revContribuyente.Enabled = activaAlta;
            rfvNTramites.Enabled = activaAlta;
            revNTramites.Enabled = activaAlta;
            txtContribuyente.Text = "";
            txtNTramites.Text = "1";
            txtNTramites.ReadOnly = false;
            if ((activaAlta == false) && (Predio.ClavePredial.Substring(0,1) == "0"))
            {
                lblCuentaPredial.Text = "Cuenta Predial: ";
                lblCuentaPredialTxt.Text = Predio.ClaveAnterior.Substring(0, 4) + "-" + Predio.ClaveAnterior.Substring(4, 2) + "-" + Predio.ClaveAnterior.Substring(6, 3) + "-" + Predio.ClaveAnterior.Substring(9, 3); 
            }

        }

        public partial class conceptoGrid
        {
            public string Clave { get; set; }
            public string Descripcion { get; set; }
            public string Status { get; set; }
            public Boolean Activo { get; set; }
            public string Id { get; set; }
            public string Cambio { get; set; }
            public string Importe { get; set; }
            public string TipoCobro { get; set; }
            public string NombreAdquiriente { get; set; }
            public string IdMesa { get; set; }
            public string Concepto { get; set; }
            public string IdTramite { get; set; }
            public string IdConceptoP { get; set; }
            public string Costo { get; set; }
            public String Porcentaje { get; set; }
            public string Descuento { get; set; }
        }
        private void eliminarActivarConcepto(Boolean activo, string id)
        {
            List<conceptoGrid> listConcepto = new List<conceptoGrid>();
            if (grd.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in grd.Rows)
                {
                    Boolean continuar = true;
                    Label clave = gvr.FindControl("lblClave") as Label;
                    Label Descripcion = gvr.FindControl("lblDescripcion") as Label;
                    Label Id = gvr.FindControl("lblId") as Label;
                    Label Status = gvr.FindControl("lblStatus") as Label;
                    Label Activo = gvr.FindControl("lblActivo") as Label;
                    Label Cambio = gvr.FindControl("lblCambio") as Label;
                    Label Importe = gvr.FindControl("lblImporte") as Label;
                    Label TipoCobro = gvr.FindControl("lblTipoCobro") as Label;
                    Label NombreAdquiriente = gvr.FindControl("lblNombreAdquiriente") as Label;
                    conceptoGrid conceptoAux = new conceptoGrid();

                    if (id.Substring(0, 1) == "N")
                    {
                        if (id == Id.Text)
                        {
                            continuar = false;
                        }
                    }

                    if (continuar)
                    {
                        Boolean ActivoAux = Convert.ToBoolean(Activo.Text);
                        string CambioAux = Cambio.Text;
                        if (id.Substring(0, 1) != "N")
                        {
                            if (id == Id.Text)
                            {
                                ActivoAux = activo;
                                CambioAux = "M";
                            }
                        }
                        conceptoAux.Clave = clave.Text;
                        conceptoAux.Descripcion = Descripcion.Text;
                        conceptoAux.Cambio = CambioAux;
                        conceptoAux.Activo = ActivoAux;
                        conceptoAux.Id = Id.Text;
                        conceptoAux.Status = Status.Text;
                        conceptoAux.Importe = Importe.Text;
                        conceptoAux.TipoCobro = TipoCobro.Text;
                        conceptoAux.NombreAdquiriente = NombreAdquiriente.Text;
                        listConcepto.Add(conceptoAux);
                    }
                }
            }
            ViewState["total"] = 0;
            grd.DataSource = listConcepto;
            grd.DataBind();
            llenarConcepto();
        }

        private void eliminarActivarAltas(Boolean activo, string id)
        {
            List<conceptoGrid> listConcepto = new List<conceptoGrid>();
            if (grdAlta.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in grdAlta.Rows)
                {
                    Boolean continuar = true;
                    Label clave = gvr.FindControl("lblClave") as Label;
                    Label Descripcion = gvr.FindControl("lblDescripcion") as Label;
                    Label Id = gvr.FindControl("lblId") as Label;
                    Label Status = gvr.FindControl("lblStatus") as Label;
                    Label Activo = gvr.FindControl("lblActivo") as Label;
                    Label Cambio = gvr.FindControl("lblCambio") as Label;
                    Label Importe = gvr.FindControl("lblImporte") as Label;
                    Label TipoCobro = gvr.FindControl("lblTipoCobro") as Label;
                    Label NombreAdquiriente = gvr.FindControl("lblNombreAdquiriente") as Label;
                    conceptoGrid conceptoAux = new conceptoGrid();

                    if (id.Substring(0, 1) == "N")
                    {
                        if (id == Id.Text)
                        {
                            continuar = false;
                        }
                    }

                    if (continuar)
                    {
                        Boolean ActivoAux = Convert.ToBoolean(Activo.Text);
                        string CambioAux = Cambio.Text;
                        if (id.Substring(0, 1) != "N")
                        {
                            if (id == Id.Text)
                            {
                                ActivoAux = activo;
                                CambioAux = "M";
                            }
                        }
                        conceptoAux.Clave = clave.Text;
                        conceptoAux.Descripcion = Descripcion.Text;
                        conceptoAux.Cambio = CambioAux;
                        conceptoAux.Activo = ActivoAux;
                        conceptoAux.Id = Id.Text;
                        conceptoAux.Status = Status.Text;
                        conceptoAux.Importe = Importe.Text;
                        conceptoAux.TipoCobro = TipoCobro.Text;
                        conceptoAux.NombreAdquiriente = NombreAdquiriente.Text;
                        listConcepto.Add(conceptoAux);
                    }
                }
            }
            grdAlta.DataSource = listConcepto;
            grdAlta.DataBind();
            llenarConcepto();
        }

        private void llenaGridTramite(int idPredio)
        {
            List<tTramite> listTramites = new tTramiteBL().GetTiposTramiteIdPredio(idPredio, 4, "A");
            List<conceptoGrid> tramites = null;
            ViewState["idT"] = 0;

            if (listTramites != null)
            {
                if (listTramites.Count > 0)
                {
                    tramites = new List<conceptoGrid>();
                    foreach (tTramite t in listTramites)
                    {
                        conceptoGrid concepto = new conceptoGrid();
                        concepto.Clave = "";
                        concepto.Activo = t.Activo;
                        concepto.Descripcion = t.Fecha.ToString("dd-MMMM-yyyy");
                        concepto.Id = t.Id.ToString();
                        concepto.Status = "A";
                        concepto.Cambio = "G";
                        concepto.TipoCobro = "";
                        concepto.NombreAdquiriente = t.NombreAdquiriente;
                        concepto.Importe = "";
                        tramites.Add(concepto);
                    }
                }
                else
                {
                    tablaColumnaGrid.Visible = false;
                    divGridAlta.Visible = false;
                }
            }
            else
            {
                tablaColumnaGrid.Visible = false;
                divGridAlta.Visible = false;
            }
            grdAlta.DataSource = tramites;
            grdAlta.DataBind();
        }

        private void llenaGridConcepto(int idPredio, int idTramite)
        {
            List<tTramiteDetalle> listTramiteD = idPredio != 0 ? new tTramiteDetalleBL().GetAllIdPredio(idPredio, "A", 4) : new tTramiteDetalleBL().GetAllIdTramite(idTramite);
            List<conceptoGrid> conceptos = null;
            if (listTramiteD.Count > 0 )
            { 
                tTramite iTramite = new tTramiteBL().GetByConstraint(listTramiteD.FirstOrDefault().tTramite.Id);
                txtFechaEntrega.Text = Convert.ToDateTime(iTramite.FechaEstimado).ToString("dd/MM/yyyy");
            }
            grd.Visible = true;
            if (listTramiteD != null)
            {
                if (listTramiteD.Count > 0)
                {
                    btnEstado.Visible = true;                    
                    btnCancelarTramite.Visible = true;
                    conceptos = new List<conceptoGrid>();
                    ViewState["idT"] = listTramiteD.FirstOrDefault().tTramite.Id;
                    // lblFechaEntregaTxt.Text = 
                    foreach (tTramiteDetalle td in listTramiteD)
                    {
                        conceptoGrid concepto = new conceptoGrid();
                        concepto.Clave = td.IdConcepto.ToString();
                        concepto.Activo = td.Activo;
                        concepto.Descripcion = td.cConcepto.Cri + " -- " + td.cConcepto.Nombre;
                        concepto.Id = td.Id.ToString();
                        concepto.Status = "A";
                        concepto.Cambio = "G";
                        concepto.TipoCobro = td.cConcepto.TipoCobro;
                        concepto.NombreAdquiriente = td.tTramite.NombreAdquiriente;
                        concepto.Importe = Convert.ToDecimal(td.Importe).ToString("C");
                        conceptos.Add(concepto);
                    }
                    
                }
                else
                {
                    btnEstado.Visible = false;
                    btnCancelarTramite.Visible = false;
                    rpt.LocalReport.DataSources.Clear();
                    btnCancelarTramite.Visible = false;
                    ViewState["idT"] = 0;
                }
            }
            else
            {
                btnCancelarTramite.Visible = false;
                ViewState["idT"] = 0;
            }

            grd.DataSource = conceptos;
            grd.DataBind();
        }

        protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (DataBinder.Eval(e.Row.DataItem, "Cambio") != "G")
                {
                    int cambios = Convert.ToInt32(ViewState["mensajeCambio"]);
                    cambios += 1;
                    ViewState["mensajeCambio"] = cambios;
                }

                string id = grd.DataKeys[e.Row.RowIndex].Values[0].ToString();
                string activo = grd.DataKeys[e.Row.RowIndex].Values[1].ToString();
                string status = grd.DataKeys[e.Row.RowIndex].Values[2].ToString();
                string tipoCobro = grd.DataKeys[e.Row.RowIndex].Values[3].ToString();
                string cri = new cConceptoBL().GetByConstraint(int.Parse(grd.DataKeys[e.Row.RowIndex].Values[4].ToString())).Cri;
   
                if (activo.ToUpper() == "TRUE")
                {
                    ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                    imgActivar.Visible = false;
                    if (tipoCobro != "I")
                    {
                        ImageButton imgEditar = (ImageButton)e.Row.FindControl("imgUpdate");
                        imgEditar.Visible = false;
                    }
                }
                else
                {
                    ImageButton imgEditar = (ImageButton)e.Row.FindControl("imgUpdate");
                    imgEditar.Visible = false;

                    if (id.Substring(0, 1) == "N")
                    {
                        ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                        imgActivar.Visible = false;
                    }
                    else
                    {
                        ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                        imgDelete.Visible = false;
                    }
                }
                //}
                ImageButton imgDeleteValida = (ImageButton)e.Row.FindControl("imgDelete");
                if (imgDeleteValida.Visible == true)
                {
                    ViewState["total"] = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Importe").ToString().Replace("$", "")) + Convert.ToDecimal(ViewState["total"]);
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lbl = (Label)e.Row.FindControl("lblCambios");
                Label lblTotal = (Label)e.Row.FindControl("lblTotal");
                lblTotal.Text = Convert.ToDecimal(ViewState["total"]).ToString("C");
                if (Convert.ToInt32(ViewState["mensajeCambio"]) > 0)
                {
                    lbl.Text = "Cambios pendientes por Guardar";
                    lbl.ForeColor = System.Drawing.Color.Red;
                    revContribuyente.Enabled = txtClvCastatral.Text.Trim() == ViewState["claveGenerica"].ToString().Trim() ? true : false;
                }
                else
                {
                    lbl.Text = "";
                }
            }
        }

        protected void btnCancelarTramite_Click(object sender, EventArgs e)
        {
            vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfirmacionCancelar), ModalPopupMensaje.TypeMesssage.Confirm);
        }

        protected void btnCancelarCobro_Click(object sender, EventArgs e)
        {
            ViewState["idConcepto"] = null;
            pnl_Modal.Hide();

        }

        protected void btnAceptarCobro_Click(object sender, EventArgs e)
        {
            //Int32 id = Convert.ToInt32(ddlConcepto.SelectedValue);
            Int32 id = ViewState["idConcepto"] == null ? Convert.ToInt32(ddlConcepto.SelectedValue) : Convert.ToInt32(ViewState["idConcepto"]);
            cConcepto concepto = new cConceptoBL().GetByConstraint(id);
            string observacion = "";
#pragma warning disable CS0219 // La variable 'altas' está asignada pero su valor nunca se usa
            Boolean altas = false;
#pragma warning restore CS0219 // La variable 'altas' está asignada pero su valor nunca se usa

            pnl_Modal.Hide();

            switch (concepto.TipoCobro)
            {
                case "S":
                    cSalarioMinimo s = new cSalarioMinimoBL().GetByEjercicio(Convert.ToInt32(DateTime.Now.Year));
                    if (s != null)
                    {
                        String clave = new cParametroSistemaBL().GetByTipoCampoCobro("cobroTCatastro");
                        switch (clave)
                        {
                            case "":
                                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.DefinirSerie), ModalPopupMensaje.TypeMesssage.Alert);
                                break;
                            case "cobroCatastroUMAMax":
                                llenaGridConceptos(id, Utileria.Redondeo(s.Importe * concepto.SalarioMax), observacion, concepto);
                                break;
                            case "cobroCatastroUMAMin":
                                llenaGridConceptos(id, Utileria.Redondeo(s.Importe * concepto.SalarioMin), observacion, concepto);
                                break;
                            case "cobroCatastroUMASalario":
                                llenaGridConceptos(id, Utileria.Redondeo(s.Importe * Convert.ToInt32(txtUma.Text)), observacion, concepto);
                                break;
                            default:
                                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.DefinirSerie), ModalPopupMensaje.TypeMesssage.Alert);
                                break;
                        }
                    }
                    else
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription("No se Encuentra UMA del Ejercicio Actual"), ModalPopupMensaje.TypeMesssage.Alert);
                    }
                    break;
                case "I":
                    llenaGridConceptos(id, Utileria.Redondeo(Convert.ToDecimal(txtImporte.Text)), observacion, concepto);
                    break;
            }
        }

        protected void grdAlta_Sorting(object sender, GridViewSortEventArgs e)
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

        }

        protected void grdAlta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

#pragma warning disable CS0252 // Posible comparación de referencias no intencionada; para obtener una comparación de valores, convierta el lado de la izquierda en el tipo 'string'
                if (DataBinder.Eval(e.Row.DataItem, "Cambio") != "G")
                {

                    int cambios = Convert.ToInt32(ViewState["mensajeCambioAlta"]);
                    cambios += 1;
                    ViewState["mensajeCambioAlta"] = cambios;

                }
#pragma warning restore CS0252 // Posible comparación de referencias no intencionada; para obtener una comparación de valores, convierta el lado de la izquierda en el tipo 'string'

                string id = grdAlta.DataKeys[e.Row.RowIndex].Values[0].ToString();
                string activo = grdAlta.DataKeys[e.Row.RowIndex].Values[1].ToString();
                string status = grdAlta.DataKeys[e.Row.RowIndex].Values[2].ToString();

                if (activo.ToUpper() == "TRUE")
                {
                    ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                    imgActivar.Visible = false;
                    //if (tipoCobro != "I")
                    //{
                    //    ImageButton imgEditar = (ImageButton)e.Row.FindControl("imgUpdate");
                    //    imgEditar.Visible = false;

                    //}
                }
                else
                {
                    //ImageButton imgEditar = (ImageButton)e.Row.FindControl("imgUpdate");
                    //imgEditar.Visible = false;
                    if (id.Substring(0, 1) == "N")
                    {
                        ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                        imgActivar.Visible = false;
                    }
                    else
                    {
                        ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                        imgDelete.Visible = false;
                    }
                }
                //}

            }
            //if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    Label lbl = (Label)e.Row.FindControl("lblCambios");
            if (Convert.ToInt32(ViewState["mensajeCambioAlta"]) > 0)
            {
                lblCambiosFooter.Text = "Cambios pendientes por Guardar";
                lblCambiosFooter.ForeColor = System.Drawing.Color.Red;
                revContribuyente.Enabled = false;
            }
            else
            {
                lblCambiosFooter.Text = "";
            }
            //}
        }

        protected void grdAlta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EliminarRegistro")
            {
                string id = e.CommandArgument.ToString();
                eliminarActivarAltas(false, id);

            }
            else if (e.CommandName == "ActivarRegistro")
            {
                string id = e.CommandArgument.ToString();
                eliminarActivarAltas(true, id);
            }
            else if (e.CommandName == "EditarImporte")
            {

                String id = e.CommandArgument.ToString();
                if (id.Substring(0, 1) == "N")
                {
                    ViewState["idConcepto"] = ViewState["idConceptoN"];
                    ViewState["idTD"] = id;
                    btnAgregarConcepto_Click(null, null);
                }
                else
                {
                    tTramite t = new tTramiteBL().GetByConstraint(Convert.ToInt32(id));
                    ViewState["idT"] = t.Id;
                    llenaGridConcepto(0, t.Id);
                    txtContribuyente.Text = t.NombreAdquiriente;
                    txtNTramites.Text = "1";
                    txtNTramites.ReadOnly = true;
                }

            }
        }

        protected void txtContribuyente_TextChanged(object sender, EventArgs e)
        {
            if (grd.Rows.Count > 0)
            {
                Label lbl = (Label)grd.FooterRow.FindControl("lblCambios");
                ViewState["mensajeCambio"] = 1;
                lbl.Text = "Cambios pendientes por Guardar";
                lbl.ForeColor = System.Drawing.Color.Red;
            }

        }

        protected void btnEstado_Click(object sender, EventArgs e)
        {
            List<cParametroSistema> listConfiguraciones = new cParametroSistemaBL().GetAll();
            string NombreMunicipio = listConfiguraciones.FirstOrDefault(c => c.Clave == "NOMBRE_MUNICIPIO").Valor;
            string Dependencia = listConfiguraciones.FirstOrDefault(c => c.Clave == "DEPENDENCIA").Valor;
            string Area = listConfiguraciones.FirstOrDefault(c => c.Clave == "AREA").Valor;
            string UrlLogo = Server.MapPath("~") + listConfiguraciones.FirstOrDefault(c => c.Clave == "LOGO").Valor;
            string CLAVE_CATASTRAL_GENERICA = listConfiguraciones.FirstOrDefault(c => c.Clave == "CLAVE_CATASTRAL_GENERICA").Valor;
            FileStream fS = new FileStream(UrlLogo, FileMode.Open, FileAccess.Read);
            byte[] LogoByte = new byte[fS.Length];
            fS.Read(LogoByte, 0, (int)fS.Length);
            fS.Close();

            DataTable ConfGral = new DataTable("ConfGral");
            ConfGral.Columns.Add("NombreMunicipio");
            ConfGral.Columns.Add("Dependencia");
            ConfGral.Columns.Add("Area");
            ConfGral.Columns.Add("Logo", typeof(Byte[]));
            ConfGral.Columns.Add("Usuario");
            ConfGral.Columns.Add("Elaboro");
            ConfGral.Columns.Add("VoBo");
            cUsuarios U = (cUsuarios)Session["usuario"];
            string nombre = U.Nombre + " " + U.ApellidoPaterno + " " + U.ApellidoMaterno;

            //--------------------------------
            DataTable Conceptos = new DataTable("Conceptos");
            Conceptos.Columns.Add("Clave");
            Conceptos.Columns.Add("Descripcion");
            Conceptos.Columns.Add("Costo");
            Conceptos.Columns.Add("Porcentaje");
            Conceptos.Columns.Add("Descuento");
            Conceptos.Columns.Add("Importe");

            decimal total = 0;
            foreach (conceptoGrid Con in llenaGridConceptos(Convert.ToInt32(ViewState["idT"])))
            {
                total = total + Convert.ToDecimal(Con.Costo);
                Conceptos.Rows.Add(Con.Id, Con.Concepto,Convert.ToDecimal(Con.Importe).ToString("C"), Con.Porcentaje, Convert.ToDecimal(Con.Descuento).ToString("C"), Convert.ToDecimal(Con.Costo).ToString("C"));
            }

            ConfGral.Rows.Add(NombreMunicipio, Dependencia, Area, LogoByte, "", nombre, total.ToString("C"));

            ////INICIA REPORTE
            pnlReport.Visible = true;
            rpt.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            rpt.LocalReport.DisplayName = "Estado de Cuenta";
            rpt.LocalReport.ReportPath = "Reportes/EdoCuentaCatastro.rdlc";
            rpt.LocalReport.EnableExternalImages = true;
            rpt.LocalReport.DataSources.Clear();
            rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ConfGral", ConfGral));
            rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("Conceptos", Conceptos));

            cPredio P = new cPredioBL().GetByConstraint(Convert.ToInt32(ViewState["IdPredio"]));

            List<ReportParameter> parameters = new List<ReportParameter>();
            parameters.Add(new ReportParameter("ClavePredial", P.ClavePredial));
            if (P.ClavePredial.ToString().Trim() == CLAVE_CATASTRAL_GENERICA.Trim())
            {
                tTramite tramite = new tTramiteBL().GetByConstraint(Convert.ToInt32(ViewState["idT"].ToString()));
                parameters.Add(new ReportParameter("Propietario", tramite.NombreAdquiriente.Trim()));
            }
            else
            {
                parameters.Add(new ReportParameter("Propietario", P.cContribuyente.ApellidoPaterno.Trim() + " " + P.cContribuyente.ApellidoMaterno.Trim() + " " + P.cContribuyente.Nombre.Trim()));
            }            
            parameters.Add(new ReportParameter("Direccion", P.Calle.Trim() + ", " + P.Numero.Trim() + ", " + P.cColonia.NombreColonia.Trim() + ", " + P.CP.Trim() + ", " + P.Localidad.Trim()));
            parameters.Add(new ReportParameter("Vigencia", DateTime.Now.Date.ToString("dd/MM/yyyy")));

            rpt.LocalReport.SetParameters(parameters);
            rpt.LocalReport.Refresh();
        }

        private List<conceptoGrid> llenaGridConceptos(Int32 idTramite)//, Decimal NSalarioImporte)
        {
            ViewState["TipoTramite"] = 4;
            ViewState["Importe"] = "0";
            ViewState["idT"] = idTramite;
            DateTime d = DateTime.Now;
#pragma warning disable CS0219 // La variable 'tt' está asignada pero su valor nunca se usa
            int tt = 4;
#pragma warning restore CS0219 // La variable 'tt' está asignada pero su valor nunca se usa
            int idConceptoP = 0;
            //cConcepto concepto = new cConceptoBL().GetListByConstraintEjercicio(idConcepto, Convert.ToInt32(d.Year));
            cSalarioMinimo s = new cSalarioMinimoBL().GetByEjercicio(Convert.ToInt32(d.Year));
            //List<cConcepto> listconcepto =new tTramiteDetalleBL().GetAllIdTramite(idTramite);
            List<tTramiteDetalle> listTramiteDetalle = new tTramiteDetalleBL().GetAllIdTramite(idTramite);
            List<conceptoGrid> listConcepto = new List<conceptoGrid>();
            if (listTramiteDetalle.Count > 0)
            {
                if (s != null)
                {
                    foreach (tTramiteDetalle td in listTramiteDetalle)
                    {
                        cConcepto concepto = td.cConcepto;
                        bool add = false; bool TipoS = false; decimal costoI = 0; //bool cerrar = true;                    
                        conceptoGrid conceptoAux = new conceptoGrid();
                        conceptoAux.Id = concepto.Id.ToString();
                        conceptoAux.IdMesa = concepto.IdMesa.ToString();
                        conceptoAux.Concepto = concepto.Nombre;
                        conceptoAux.IdTramite = "0";
                        conceptoAux.TipoCobro = concepto.TipoCobro;
                        conceptoAux.IdConceptoP = "0";
                        int idcConcepto = concepto.Id;
                        String costo = "";
                        idConceptoP = concepto.EsAdicional == false ? concepto.Id : 0;
                        if (concepto.TipoCobro == "S")
                        {
                            add = concepto.Adicional == true ? true : false;
                            TipoS = true;
                            costoI = s.Importe * concepto.SalarioMax;
                            costo = Convert.ToString(Utileria.Redondeo(costoI));
                            conceptoAux.Costo = costo;
                            conceptoAux.Porcentaje = "0";
                            conceptoAux.Descuento = "0";
                            conceptoAux.Importe = costo;
                            if (concepto.SinDescuento == true)
                            {
                                tPrediosDescuento preddesc = new tPrediosDescuentoBL().GetForCobro(concepto.Id, Convert.ToInt32(ViewState["idP"].ToString()), Convert.ToInt32(ViewState["idT"].ToString()), d);
                                if (preddesc != null)
                                {

                                    decimal pI = Convert.ToDecimal(preddesc.Porcentaje);
                                    decimal desc = Utileria.Redondeo(pI * (costoI / 100));
                                    decimal import = Utileria.Redondeo(costoI - desc);
                                    conceptoAux.Porcentaje = Convert.ToString(Utileria.Redondeo(preddesc.Porcentaje));
                                    conceptoAux.Descuento = Convert.ToString(Utileria.Redondeo(desc));
                                    conceptoAux.Importe = Convert.ToString(Utileria.Redondeo(import));

                                }
                            }
                        }
                        if (concepto.TipoCobro == "P")
                        {
                            if (TipoS == true)
                            {
                                decimal pI = Convert.ToDecimal(concepto.Importe);
                                decimal desc = Utileria.Redondeo((pI * (costoI / 100)));
                                costo = Convert.ToString(Utileria.Redondeo(desc));
                                conceptoAux.Costo = costo;
                                conceptoAux.Porcentaje = "0";
                                conceptoAux.Descuento = "0";
                                conceptoAux.Importe = costo;
                            }
                            else
                            {
                                conceptoAux.Porcentaje = concepto.Importe.ToString();
                            }
                        }
                        if (concepto.TipoCobro == "I")
                        {
                            costoI = Convert.ToDecimal(td.Importe);
                            conceptoAux.Costo = td.Importe.ToString();
                            conceptoAux.Porcentaje = "0";
                            conceptoAux.Descuento = "0";
                            conceptoAux.Importe = td.Importe.ToString();
                            conceptoAux.TipoCobro = "S";
                            if (concepto.SinDescuento == true)
                            {
                                tPrediosDescuento preddesc = new tPrediosDescuentoBL().GetForCobro(concepto.Id, Convert.ToInt32(ViewState["idP"].ToString()), Convert.ToInt32(ViewState["idT"].ToString()), d);
                                if (preddesc != null)
                                {
                                    decimal pI = Convert.ToDecimal(preddesc.Porcentaje);
                                    decimal desc = Utileria.Redondeo((pI * (Convert.ToDecimal(td.Importe) / 100)));
                                    decimal import = Utileria.Redondeo(costoI - desc);
                                    costo = Convert.ToString(Utileria.Redondeo(desc));
                                    conceptoAux.Porcentaje = Convert.ToString(Utileria.Redondeo(preddesc.Porcentaje));
                                    conceptoAux.Descuento = Convert.ToString(Utileria.Redondeo(desc));
                                    conceptoAux.Importe = Convert.ToString(Utileria.Redondeo(import));
                                }
                            }
                        }
                        listConcepto.Add(conceptoAux);
                        if (concepto.Adicional == true)
                        {
                            cConcepto addic = new cConceptoBL().AdicionalMesaCatastro(Convert.ToInt32(d.Year), Convert.ToInt32(3));
                            if (addic != null)
                            {
                                conceptoGrid cx = new conceptoGrid();
                                cx.IdConceptoP = idConceptoP == 0 ? "0" : idConceptoP.ToString();
                                cx.Id = addic.Id.ToString();
                                cx.IdMesa = addic.IdMesa.ToString();
                                cx.IdTramite = "0";
                                cx.TipoCobro = addic.TipoCobro;
                                decimal ouio = Utileria.Redondeo(Convert.ToDecimal(conceptoAux.Costo) * (Convert.ToDecimal(addic.Importe) / 100));
                                cx.Costo = Convert.ToString(ouio);
                                if (addic.TipoCobro == "P")
                                {
                                    cx.Porcentaje = addic.Importe.ToString();
                                }
                                else
                                {
                                    cx.Porcentaje = "0";
                                }
                                cx.Descuento = "0";
                                cx.Importe = cx.Costo;
                                cx.Concepto = addic.Descripcion;
                                listConcepto.Add(cx);
                            }
                        }

                    }

                    //listConcepto = llenarListaConcepto(listConcepto);
                    //if (listConcepto != null)
                    //{
                    //    grdCobros.DataSource = listConcepto;
                    //    grdCobros.DataBind();
                    //    cerrarGrid();

                    //}
                    //else
                    //{
                    //    //btnRecalculo.Enabled = false;
                    //    vtnModal.ShowPopup(new Utileria().GetDescription("Imposible agregar el concepto ya se encuentra en la lista"), ModalPopupMensaje.TypeMesssage.Alert);
                    //}

                }
                else
                {
                    //btnRecalculo.Enabled = false;
                    vtnModal.ShowPopup(new Utileria().GetDescription("No se Encuentra UMA del Ejercicio Actual"), ModalPopupMensaje.TypeMesssage.Alert);
                }
            }
            else
            {
                //btnRecalculo.Enabled = false;
                vtnModal.ShowPopup(new Utileria().GetDescription("El tramite no cuenta con concepto de cobro activo"), ModalPopupMensaje.TypeMesssage.Alert);
            }
            return listConcepto;
        }

        //private List<conceptoGrid> llenarListaConcepto(List<conceptoGrid> listConcepto)
        //{
        //    String idConcepto = listConcepto.Select(i => i.Id).First();
        //    foreach (GridViewRow gvr in grdCobros.Rows)
        //    {
        //        Label Id = gvr.FindControl("lblId") as Label;
        //        if (idConcepto != Id.Text || ddlTipoTramite.SelectedValue == "3")
        //        {
        //            Label tramite = gvr.FindControl("lblIdTramite") as Label;
        //            Label Mesa = gvr.FindControl("lblIdMesa") as Label;
        //            Label tipoCobro = gvr.FindControl("lblTipoCobro") as Label;
        //            Label Conceptos = gvr.FindControl("lblConcepto") as Label;
        //            Label ConceptoP = gvr.FindControl("lblIdConceptoP") as Label;
        //            TextBox Costo = gvr.FindControl("txtCosto") as TextBox;
        //            TextBox Porcentaje = gvr.FindControl("txtPorcentaje") as TextBox;
        //            TextBox Descuento = gvr.FindControl("txtDescuento") as TextBox;
        //            TextBox Importe = gvr.FindControl("txtImporte") as TextBox;

        //            conceptoGrid conceptoAux = new conceptoGrid();
        //            conceptoAux.Id = Id.Text;
        //            conceptoAux.IdMesa = Mesa.Text;
        //            conceptoAux.Importe = Importe.Text;
        //            conceptoAux.Porcentaje = Porcentaje.Text;
        //            conceptoAux.IdTramite = tramite.Text;
        //            conceptoAux.Concepto = Conceptos.Text;
        //            conceptoAux.Costo = Costo.Text;
        //            conceptoAux.Descuento = Descuento.Text;
        //            conceptoAux.TipoCobro = tipoCobro.Text;
        //            conceptoAux.IdConceptoP = ConceptoP.Text;
        //            listConcepto.Add(conceptoAux);
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    return listConcepto;
        //}

    }
}