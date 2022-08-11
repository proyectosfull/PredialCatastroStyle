using Catastro.Controles;
using Clases;
using Clases.BL;
using Clases.Utilerias;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Catastro.Catalogos
{
    public partial class DescuentoPredio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                llenaTramite();
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        private void llenaTramite()
        {
            ddlTramite.DataValueField = "Id";
            ddlTramite.DataTextField = "Tramite";
            ddlTramite.DataSource = new cTipoTramiteBL().GetFilter("ConDescuento","1","TRUE","Id","asc");
            ddlTramite.DataBind();
            ddlTramite.Items.Insert(0, new ListItem("Selecciona un tramite", "0"));
        }

        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["IdPredio"] = null;
            ViewState["IdTramiteClaveGenerica"] = null;
            grdClaveGenerica.Visible = false;
            grd.Visible = false;
            cPredio predio = new cPredioBL().GetByClavePredial(txtCve.Text);
            String clave = new cParametroSistemaBL().GetValorByClave("CLAVE_CATASTRAL_GENERICA");
            if (predio != null)
            {
                if (clave != txtCve.Text)
                {
                    lblPred.Text = predio.ClavePredial;
                    cContribuyente contribuyente = new cContribuyenteBL().GetByConstraint(predio.IdContribuyente);
                    lblPropietario.Text = contribuyente.RazonSocial+" "+ contribuyente.ApellidoPaterno + " " + contribuyente.ApellidoMaterno + " " + contribuyente.Nombre;
                    int IdTramite = Convert.ToInt32(ddlTramite.SelectedValue);
                    if (IdTramite != 2)
                    {
                        llenaGrid(predio);
                    }
                    else
                    {
                        llenaGridISABI(predio);
                    }
                }
                else
                {
                    ViewState["IdPredio"] = predio.Id;
                    grdClaveGenerica.Visible = true;                    
                    llenaGridTramite(predio.Id);
                }
            }

            else
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.PredioInexistente), ModalPopupMensaje.TypeMesssage.Alert);
                txtCve.Text = "";
                lblPropietario.Text = "";
                lblPred.Text = "";
                grd.Visible = false;
                grdISABI.Visible = false;
            }
        }

        private void llenaGrid(cPredio predio)
        {
            DataTable DTConceptos = new DataTable();
            if(predio==null)
                predio = new cPredioBL().GetByClavePredial(txtCve.Text);
            //cTipoTramite t = new cTipoTramiteBL().GetByConstraint(Convert.ToInt32(ddlTramite.SelectedValue));
            tTramite tramite = new tTramite();
            List<tTramite> ltramite = new List<tTramite>();
            int IdTramite = Convert.ToInt32(ddlTramite.SelectedValue);
            if (IdTramite == 0)
            {
                grd.Visible = false;
                grdISABI.Visible = false;
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.Seleccionatramite), ModalPopupMensaje.TypeMesssage.Alert);
            }
            else
            {
                List<cConcepto> ConceptosTramite = new List<cConcepto>();
                //Si el tipo de tramite es diferente a predial o Servicios se debe validar que exista el tramite en el la tabla de tramites.....
                if (IdTramite != 5 && IdTramite != 6)
                {
                    //tramite = new tTramiteBL().GettTramiteByPredio(predio.Id, IdTramite);
                    ltramite = new tTramiteBL().GetTiposTramiteIdPredio(predio.Id, IdTramite, "A");

                    if (ltramite.Count <= 0)
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.TramiteInexistente), ModalPopupMensaje.TypeMesssage.Alert);
                    }
                    else
                    {
                        if (IdTramite == 2)
                        {
                            ////DataTable DTtram = new DataTable();
                            //ConceptosTramite = null;
                            //DTConceptos.Columns.Add("Id", typeof(int));
                            //DTConceptos.Columns.Add("Estado", typeof(string));
                            //DTConceptos.Columns.Add("FechaFin", typeof(DateTime));
                            //DTConceptos.Columns.Add("Porcentaje", typeof(Decimal));
                            //DTConceptos.Columns.Add("Estado", typeof(string));
                            //DTConceptos.Columns.Add("Cri", typeof(string));
                            //DTConceptos.Columns.Add("Nombre", typeof(string));
                            //Table tab = new Table();
                            //foreach (tTramite t in ltramite)
                            //{

                            //}
                        }
                        else
                        {
                            tramite = ltramite.FirstOrDefault();
                            if (tramite.IdTipoTramite == 3 && tramite.Observacion == null)// si es convenio y sin desglosado, toma los conceptosd e la tabla cConcepto
                            {
                                //ConceptosTramite = new cConceptoBL().GetListForDescuentos(tramite.IdTipoTramite, Convert.ToInt32(DateTime.Today.Year));//Conceptos para el tramite...
                                ConceptosTramite = conceptosPorTramite(tramite.Id, Convert.ToInt32(DateTime.Today.Year));
                            }
                            else
                            {
                                ViewState["IdTipoTramite"] = tramite.IdTipoTramite;
                                //ConceptosTramite = new tTramiteDetalleBL().GetAllIdTramite(tramite.Id).Where(c => c.cConcepto.SinDescuento = true).Select(c => c.cConcepto).ToList();
                                //ConceptosTramite = new cConceptoBL().GetListForDescuentos(tramite.IdTipoTramite, Convert.ToInt32(DateTime.Today.Year));//Conceptos para el tramite...
                                ConceptosTramite = conceptosPorTramite(tramite.Id, Convert.ToInt32(DateTime.Today.Year));
                            }
                        }
                    }
                }
                if (IdTramite != 4)
                {
                    if (IdTramite==5)
                        ConceptosTramite = new cConceptoBL().GetListForDescuentos(IdTramite, Convert.ToInt32(DateTime.Today.Year));//Conceptos para el tramite...

                }                 
                string Excluir = "";
                if (IdTramite ==5)
                    Excluir = new cParametroSistemaBL().GetValorByClave("IMPUESTOIP");
                if (IdTramite == 6)
                    Excluir = new cParametroSistemaBL().GetValorByClave("IMPUESTOSM");

                List<tPrediosDescuento> Descuentos = new tPrediosDescuentoBL().GetForDescuentos(predio.Id, tramite.Id);//Descuentos registrados...
                if (ConceptosTramite.Count() > 0)
                {
                    DTConceptos = ConvertirListaToDataTable(ConceptosTramite);
                    if (Excluir != "")
                    {
                        Excluir = "Cri!='" + Excluir+"'";
                        DTConceptos.Select(Excluir);
                    }

                    DTConceptos.Columns.Add("FechaFin", typeof(DateTime));
                    DTConceptos.Columns.Add("Porcentaje", typeof(Decimal));
                    DTConceptos.Columns.Add("Estado", typeof(string));


                    if (Descuentos.Count() > 0)
                    {
                        foreach (tPrediosDescuento p in Descuentos)
                        {
                            for (int i = 0; i < DTConceptos.Rows.Count; i++)
                            {
                                if (DTConceptos.Rows[i]["Id"].ToString() == p.IdConcepto.ToString()) // if id==2
                                {
                                    //DTConceptos.Rows[i]["FechaInicio"] = Convert.ToString(p.FechaInicio);
                                    DTConceptos.Rows[i]["FechaFin"] = p.FechaFin;//p.FechaFin.ToString("dd/MM/aaaa") ;
                                    DTConceptos.Rows[i]["Porcentaje"] = Convert.ToString(p.Porcentaje);
                                    DTConceptos.Rows[i]["Estado"] = Convert.ToString(p.Activo);

                                    i = DTConceptos.Rows.Count;
                                }
                            }

                        }
                    }
                   
                }
                grd.DataSource = DTConceptos;
                grd.DataBind();
                grd.Visible = true;
                grdISABI.Visible = false;
            }
        }

        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd.PageIndex = e.NewPageIndex;
            llenaGrid(null);
        }

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                cUsuarios U = new cUsuarios();
                U = (cUsuarios)Session["usuario"];
                int id = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = grd.Rows[id];
                string[] aux = { e.CommandName, grd.DataKeys[id].Values[0].ToString() };
                ViewState["aux"] = aux;
                int idconcepto = Convert.ToInt32(grd.DataKeys[id].Values[0].ToString()); //(grd.Rows[id].Cells[1].ToString());
                cPredio predio = new cPredioBL().GetByClavePredial(txtCve.Text);
                cConcepto Concepto = new cConceptoBL().GetByConstraint(idconcepto);//Conceptos para el tramite...
                switch (e.CommandName)
                {
                    case "NuevoRegistro":
                        lbl_titulo.Text = "Nuevo Descuento";
                        limpiaCampos();
                        habilitaCampos(true);
                        lblConcepto.Text = "Concepto:" + Concepto.Descripcion;
                        txtPorcentaje.Focus();
                        pnl_Modal.Show();
                        break;
                    case "ActualizarRegistro":
                        //string[] aux = { e.CommandName, grd.DataKeys[id].Values[0].ToString() };
                        lbl_titulo.Text = "Modifica Descuento";
                        lblConcepto.Text = "Concepto:" + Concepto.Descripcion;
                        txtPorcentaje.Text = grd.DataKeys[id].Values[2].ToString();
                        txtFechaFin.Text = grd.DataKeys[id].Values[3].ToString();
                        pnl_Modal.Show();
                        break;
                    //case "ConsultarRegistro":

                    //    lbl_titulo.Text = "Consulta Descuento";
                    //    //ViewState["Tipo"] = e.CommandName;
                    //    lblConcepto.Text += Concepto.Descripcion;
                    //    txtPorcentaje.Text = grd.DataKeys[id].Values[2].ToString();
                    //    txtPorcentaje.Enabled = true;
                    //    txtFechaFin.Text = grd.DataKeys[id].Values[3].ToString();
                    //    txtFechaFin.Enabled = true;
                    //    btn_Guardar.Visible = false;
                    //    pnl_Modal.Show();
                    //    break;
                    case "EliminarRegistro":
                        //int id = Convert.ToInt32(e.CommandArgument);
                        ViewState["idMod"] = grd.DataKeys[id].Values[0].ToString();
                        vtnModal.DysplayCancelar = true;
                        vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar), ModalPopupMensaje.TypeMesssage.Confirm);
                        break;
                    case "ActivarRegistro":
                        //int id = Convert.ToInt32(e.CommandArgument);
                        ViewState["idMod"] = grd.DataKeys[id].Values[0].ToString();
                        vtnModal.DysplayCancelar = true;
                        vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro), ModalPopupMensaje.TypeMesssage.Confirm);

                       
                        break;
                }
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

            llenaGrid(null);
        }

        protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string estado = grd.DataKeys[e.Row.RowIndex].Values[1].ToString();
                
                if (string.IsNullOrEmpty(estado))
                {
                    ImageButton imgEditar = (ImageButton)e.Row.FindControl("imgEditar");
                    imgEditar.Visible = false;
                    ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                    imgActivar.Visible = false;
                    ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                    imgDelete.Visible = false;
                    //ImageButton imgConsulta = (ImageButton)e.Row.FindControl("imgConsulta");
                    //imgConsulta.Visible = false;
                }
                if (estado == "True")
                {
                    ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                    imgActivar.Visible = false;
                    ImageButton imgNuevo = (ImageButton)e.Row.FindControl("imgNuevo");
                    imgNuevo.Visible = false;
                }
                if(estado=="False")
                {
                    ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                    imgDelete.Visible = false;
                    ImageButton imgNuevo = (ImageButton)e.Row.FindControl("imgNuevo");
                    imgNuevo.Visible = false;
                }
            }

        }

        private void limpiaCampos()
        {
            txtPorcentaje.Text = "";
            txtFechaFin.Text = "";
        }

        private void habilitaCampos(bool activa)
        {
            txtPorcentaje.Enabled = activa;
            txtFechaFin.Enabled = activa;
        }

        protected void guardanormal()
        {
            string[] aux = (string[])ViewState["aux"];
            string tipo = aux[0];
            tTramite tramite = new tTramite();
            int idConcepto = Convert.ToInt32(aux[1]);
            cPredio pred = new cPredioBL().GetByClavePredial(lblPred.Text);
            cUsuarios U = (cUsuarios)Session["usuario"];
            tPrediosDescuento desc = new tPrediosDescuento();
            
            int IdTramite = Convert.ToInt32(ddlTramite.SelectedValue);
            //Si el tipo de tramite es diferente a predial o Servicios se debe validar que exista el tramite en el la tabla de tramites.....
            if (IdTramite != 5 && IdTramite != 6)
            {
                //tramite = new tTramiteBL().GettTramiteByPredio(pred.Id, IdTramite);
                tramite = new tTramiteBL().GetTiposTramiteIdPredio(pred.Id, IdTramite, "A").FirstOrDefault();

            }
            if (idConcepto > 0)
            {
                MensajesInterfaz insert = new MensajesInterfaz();
                if (tipo == "NuevoRegistro")
                {
                    desc.IdConcepto = idConcepto;//Convert.ToInt32(grd.DataKeys[0].Values[0].ToString());
                    desc.IdPredio = pred.Id;
                    if (ViewState["IdTramiteClaveGenerica"] == null)
                    {
                        if (tramite.Id != 0)
                            desc.IdTramite = tramite.Id;
                    }
                    else
                    {
                        desc.IdTramite = Convert.ToInt32(ViewState["IdTramiteClaveGenerica"]);
                    }
                    desc.Concepto = ddlTramite.SelectedItem.Text;//ddlTramite.DataTextField;
                    desc.FechaInicio = DateTime.Now;
                    desc.FechaFin = Convert.ToDateTime(txtFechaFin.Text).AddDays(1);
                    desc.Porcentaje = Convert.ToDecimal(txtPorcentaje.Text);
                    desc.Activo = true;
                    desc.IdUsuario = U.IdUsuario;
                    desc.FechaModificacion = DateTime.Today;
                    insert = new tPrediosDescuentoBL().Insert(desc);
                }
                if (tipo == "ActualizarRegistro")
                {
                    if (ViewState["IdTramiteClaveGenerica"] == null)
                    {
                        desc = new tPrediosDescuentoBL().GetForDescuento(idConcepto, pred.Id, tramite.Id);
                    }
                    else
                    {
                        desc = new tPrediosDescuentoBL().GetForDescuento(idConcepto, pred.Id, Convert.ToInt32(ViewState["IdTramiteClaveGenerica"]));
                    }
                    desc.Concepto = ddlTramite.SelectedItem.Text;//ddlTramite.DataTextField;
                    desc.FechaInicio = DateTime.Now;
                    desc.FechaFin = Convert.ToDateTime(txtFechaFin.Text).AddDays(1);
                    desc.Porcentaje = Convert.ToDecimal(txtPorcentaje.Text);
                    desc.Activo = true;
                    desc.IdUsuario = U.IdUsuario;
                    desc.FechaModificacion = DateTime.Today;
                    insert = new tPrediosDescuentoBL().Update(desc);
                }
                
                if (insert == MensajesInterfaz.Ingreso)
                {
                    limpiaCampos();
                }
                else
                { vtnModal.ShowPopup(new Utileria().GetDescription(insert), ModalPopupMensaje.TypeMesssage.Error); }             
            }
        }

        protected void guardaISABI()
        {
            string sid = claveid.Value.ToString();
            int i = 0; int iddesc = 0; int idtramite = 0; int idcon = 0; string tipo = "";
            Char d = '-';
            String[] ls = sid.Split(d);
            foreach (var x in ls)
            {
                i = i + 1;
                if (i == 1)
                    iddesc = Convert.ToInt32(x);
                if (i == 2)
                    idtramite = Convert.ToInt32(x);
                if (i == 3)
                    idcon = Convert.ToInt32(x);
                if (i == 4)
                    tipo = x;
            }

            cUsuarios U = (cUsuarios)Session["usuario"];
            cPredio pred = new cPredioBL().GetByClavePredial(lblPred.Text);
            tTramite tramite = new tTramiteBL().GetByConstraint(idtramite);
            tPrediosDescuento desc = new tPrediosDescuentoBL().GetByConstraint(iddesc);
            cConcepto con = new cConceptoBL().GetByConstraint(idcon);
            MensajesInterfaz insert = new MensajesInterfaz();
            if (desc==null)
            {
                if (tipo == "NuevoRegistro")
                {
                    desc = new tPrediosDescuento();
                    desc.IdConcepto = con.Id;//Convert.ToInt32(grd.DataKeys[0].Values[0].ToString());
                    desc.IdPredio = pred.Id;
                    if (tramite.Id != 0)
                        desc.IdTramite = tramite.Id;
                    desc.Concepto = ddlTramite.SelectedItem.Text;//ddlTramite.DataTextField;
                    desc.FechaInicio = DateTime.Now.Date;
                    desc.FechaFin = Convert.ToDateTime(txtFechaFin.Text).AddDays(1);
                    desc.Porcentaje = Convert.ToDecimal(txtPorcentaje.Text);
                    desc.Activo = true;
                    desc.IdUsuario = U.IdUsuario;
                    desc.FechaModificacion = DateTime.Now;
                    insert = new tPrediosDescuentoBL().Insert(desc);
                }
            }
            if (desc!=null)
            {
                if (tipo == "ActualizarRegistro")
                {
                    desc.Concepto = ddlTramite.SelectedItem.Text;//ddlTramite.DataTextField;
                    desc.FechaInicio = DateTime.Now.Date;
                    desc.FechaFin = Convert.ToDateTime(txtFechaFin.Text).AddDays(1);
                    desc.Porcentaje = Convert.ToDecimal(txtPorcentaje.Text);
                    desc.Activo = true;
                    desc.IdUsuario = U.IdUsuario;
                    desc.FechaModificacion = DateTime.Now;
                    insert = new tPrediosDescuentoBL().Update(desc);
                }
                if (insert == MensajesInterfaz.Ingreso)
                {
                    limpiaCampos();
                }
                else
                { vtnModal.ShowPopup(new Utileria().GetDescription(insert), ModalPopupMensaje.TypeMesssage.Error); }
                
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int IdTramite = Convert.ToInt32(ddlTramite.SelectedValue);
            if (IdTramite != 2)
            {
                if (ViewState["IdTramiteClaveGenerica"] == null)
                {
                    guardanormal();
                    llenaGrid(null);
                }
                else
                {
                    guardanormal();                    
                    llenagridClvegenerica(Convert.ToInt32(ViewState["IdTramiteClaveGenerica"]));
                    grdClaveGenerica.Visible = true;
                    grd.Visible = true;
                }                
            }
            else
            {
                guardaISABI();
                llenaGridISABI(null);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ViewState["aux"] = null;
            limpiaCampos();
            pnl_Modal.Hide();
        }

        protected void aceptanorm()
        {
            cUsuarios U = (cUsuarios)Session["usuario"];
            int idConcepto = Convert.ToInt32(ViewState["idMod"]);
            cPredio pred = new cPredioBL().GetByClavePredial(lblPred.Text);
            tTramite tramite = new tTramite();
            int IdTramite = Convert.ToInt32(ddlTramite.SelectedValue);
            //Si el tipo de tramite es diferente a predial o Servicios se debe validar que exista el tramite en el la tabla de tramites.....
            if (IdTramite != 5 && IdTramite != 6)
            {
                tramite = new tTramiteBL().GettTramiteByPredio(pred.Id, IdTramite);
               
            }
            tPrediosDescuento desc;
            if (ViewState["IdTramiteClaveGenerica"] == null)
            {
                 desc = new tPrediosDescuentoBL().GetForDescuento(idConcepto, pred.Id, tramite.Id);
            }
            else
            {
                desc = new tPrediosDescuentoBL().GetForDescuento(idConcepto, pred.Id, Convert.ToInt32(ViewState["IdTramiteClaveGenerica"].ToString()));
            }

            //if (ViewState["IdTramiteClaveGenerica"] != null)
            //{
            //    grdClaveGenerica.Visible = false;
            //}

            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                desc.Activo = false;
                desc.IdUsuario = U.Id;
                desc.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new tPrediosDescuentoBL().Delete(desc);
                vtnModal.DysplayCancelar = false;
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = null;
                llenaGrid(null);
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                desc.Activo = true;
                desc.IdUsuario = U.Id;
                desc.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new tPrediosDescuentoBL().Update(desc);
                vtnModal.DysplayCancelar = false;
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = null;
                llenaGrid(null);
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Ingreso) ||
              vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Actualizacion))
            {
                llenaGrid(null);
                limpiaCampos();
            }
          
        }

        protected void aceptaISABI()
        {
            cUsuarios U = (cUsuarios)Session["usuario"];
            int iddesc = Convert.ToInt32(ViewState["idMod"]);
            cPredio pred = new cPredioBL().GetByClavePredial(lblPred.Text);
            tPrediosDescuento desc = new tPrediosDescuentoBL().GetByConstraint(iddesc);

            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                desc.Activo = false;
                desc.IdUsuario = U.Id;
                desc.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new tPrediosDescuentoBL().Delete(desc);
                vtnModal.DysplayCancelar = false;
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = null;
                llenaGrid(null);
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                desc.Activo = true;
                desc.IdUsuario = U.Id;
                desc.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new tPrediosDescuentoBL().Update(desc);
                vtnModal.DysplayCancelar = false;
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = null;
                llenaGrid(null);
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Ingreso) ||
              vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Actualizacion))
            {
                llenaGrid(null);
                limpiaCampos();
            }
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            int IdTramite = Convert.ToInt32(ddlTramite.SelectedValue);
            if (IdTramite != 2)
            {
                if (ViewState["IdTramiteClaveGenerica"] == null)
                {
                    aceptanorm();
                    llenaGrid(null);
                }
                else
                {
                    aceptanorm();
                    llenagridClvegenerica(Convert.ToInt32(ViewState["IdTramiteClaveGenerica"]));
                    grd.Visible = true;
                    grdClaveGenerica.Visible = true;
                }
               
            }
            else
            {
                aceptaISABI();
                llenaGridISABI(null);
            }
        }
       
        public DataTable ConvertirListaToDataTable<T>(IList<T> data)
        {

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties) table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties) row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        #region ISABI

        private void llenaGridISABI(cPredio predio)
        {
            DataTable DTConceptos = new DataTable();
            if (predio == null)
                predio = new cPredioBL().GetByClavePredial(txtCve.Text);
            cTipoTramite tipotram = new cTipoTramiteBL().GetByConstraint(Convert.ToInt32(ddlTramite.SelectedValue));
            tTramite tramite = new tTramite();
            List<tTramite> ltramite = new List<tTramite>();
            int IdTramite = Convert.ToInt32(ddlTramite.SelectedValue);
            if (IdTramite == 0)
            {
                grd.Visible = false;
                grdISABI.Visible = false;
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.Seleccionatramite), ModalPopupMensaje.TypeMesssage.Alert);
            }
            else
            {
                List<cConcepto> ConceptosTramite = new cConceptoBL().ListConceptobyDescuentoTramiteISABI(DateTime.Now.Year);
                if (ConceptosTramite != null)
                {
                    tPrediosDescuento pdes=null;
                    DTConceptos.Columns.Add("Id");
                    DTConceptos.Columns.Add("Adquiriente");
                    DTConceptos.Columns.Add("Cri");
                    DTConceptos.Columns.Add("Concepto");
                    DTConceptos.Columns.Add("Porcentaje");
                    DTConceptos.Columns.Add("FechaFin");

                    
                    ltramite = new tTramiteBL().GetTiposTramiteIdPredio(predio.Id, IdTramite, "A");
                    foreach(tTramite t in ltramite)
                    {
                        foreach (cConcepto conx in ConceptosTramite)
                        {
                            pdes = new tPrediosDescuentoBL().GetbyIdConceptoPredioISABI(conx.Id, Convert.ToInt32( t.IdPredio), t.Id);
                            DataRow workRow = DTConceptos.NewRow();
                            if (pdes != null)
                            {
                                workRow[0] = pdes.Id.ToString() + "-" + t.Id.ToString() + "-" + conx.Id.ToString();
                                workRow[1] = t.NombreAdquiriente;
                                workRow[2] = conx.Cri;
                                workRow[3] = pdes.Concepto;
                                workRow[4] = pdes.Porcentaje;
                                workRow[5] = pdes.FechaFin;
                                DTConceptos.Rows.Add(workRow);
                            }
                            else
                            {   
                                workRow[0] = 0.ToString() + "-" + t.Id.ToString() + "-" + conx.Id.ToString();
                                workRow[1] = t.NombreAdquiriente;
                                workRow[2] = conx.Cri;
                                workRow[3] = conx.Nombre;
                                workRow[4] = "";
                                workRow[5] = "";
                                DTConceptos.Rows.Add(workRow);
                            }
                        }
                    }
                }
              }

                grdISABI.DataSource = DTConceptos;
                grdISABI.DataBind();
                grdISABI.Visible = true;
                grd.Visible = false;
        }

        protected void grd_PageIndexChangingISABI(object sender, GridViewPageEventArgs e)
        {
            grdISABI.PageIndex = e.NewPageIndex;
            llenaGridISABI(null);
        }

        protected void grd_RowCommandISABI(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                claveid.Value = "";
                //desc-tramite-concepto
                cUsuarios U = new cUsuarios();
                U = (cUsuarios)Session["usuario"];
                int p= Convert.ToInt32(e.CommandArgument.ToString());
                string sid = grdISABI.DataKeys[Convert.ToInt32(p)].Values[0].ToString();
                claveid.Value = sid + "-" + e.CommandName;
                int i = 0; int iddesc = 0; int idtramite = 0; int idcon = 0;
                Char d = '-';
                String[] ls = sid.Split(d);
                foreach (var x in ls)
                {
                    i = i + 1;
                    if (i == 1)
                        iddesc = Convert.ToInt32(x);
                    if (i == 2)
                        idtramite = Convert.ToInt32(x);
                    if (i == 3)
                        idcon = Convert.ToInt32(x);
                }



                //int id = Convert.ToInt32(sid);
                //GridViewRow row = grdISABI.Rows[id];
                //string[] aux = { e.CommandName, grd.DataKeys[id].Values[0].ToString() };
                //ViewState["aux"] = aux;
                //Int16 idconcepto = Convert.ToInt32(grd.DataKeys[id].Values[0].ToString()); //(grd.Rows[id].Cells[1].ToString());
                cPredio predio = new cPredioBL().GetByClavePredial(txtCve.Text);
                cConcepto Concepto = new cConceptoBL().GetByConstraint(idcon);//Conceptos para el tramite...
                switch (e.CommandName)
                {
                    case "NuevoRegistro":
                        lbl_titulo.Text = "Nuevo Descuento";
                        limpiaCampos();
                        habilitaCampos(true);
                        lblConcepto.Text = "Concepto:" + Concepto.Descripcion;
                        txtPorcentaje.Focus();
                        pnl_Modal.Show();
                        break;
                    case "ActualizarRegistro":
                        ViewState["idMod"] = iddesc;
                        tPrediosDescuento des = new tPrediosDescuentoBL().GetByConstraint(iddesc);
                        lbl_titulo.Text = "Modifica Descuento";
                        lblConcepto.Text = "Concepto:" + Concepto.Descripcion;
                        txtPorcentaje.Text = des.Porcentaje.ToString();
                        txtFechaFin.Text = des.FechaFin.ToString();
                        pnl_Modal.Show();
                        break;
                    //case "ConsultarRegistro":

                    //    lbl_titulo.Text = "Consulta Descuento";
                    //    //ViewState["Tipo"] = e.CommandName;
                    //    lblConcepto.Text += Concepto.Descripcion;
                    //    txtPorcentaje.Text = grd.DataKeys[id].Values[2].ToString();
                    //    txtPorcentaje.Enabled = true;
                    //    txtFechaFin.Text = grd.DataKeys[id].Values[3].ToString();
                    //    txtFechaFin.Enabled = true;
                    //    btn_Guardar.Visible = false;
                    //    pnl_Modal.Show();
                    //    break;
                    case "EliminarRegistro":
                        //int id = Convert.ToInt32(e.CommandArgument);
                        ViewState["idMod"] = iddesc;
                        vtnModal.DysplayCancelar = true;
                        vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar), ModalPopupMensaje.TypeMesssage.Confirm);
                        break;
                    case "ActivarRegistro":
                        //int id = Convert.ToInt32(e.CommandArgument);
                        ViewState["idMod"] = iddesc;
                        vtnModal.DysplayCancelar = true;
                        vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro), ModalPopupMensaje.TypeMesssage.Confirm);
                        break;
                }
            }
        }

        protected void grd_SortingISABI(object sender, GridViewSortEventArgs e)
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

            llenaGridISABI(null);
        }

        protected void grd_RowDataBoundISABI(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string estado = grdISABI.DataKeys[e.Row.RowIndex].Values[0].ToString();

                int i = 0; int iddesc = 0; int idtramite = 0; int idcon = 0;
                Char d = '-';
                String[] ls = estado.Split(d);
                foreach (var x in ls)
                {
                    i = i + 1;
                    if (i == 1)
                        iddesc = Convert.ToInt32(x);
                    if (i == 2)
                        idtramite = Convert.ToInt32(x);
                    if (i == 3)
                        idcon = Convert.ToInt32(x);
                }


                if (iddesc==0)
                {
                    ImageButton imgEditar = (ImageButton)e.Row.FindControl("imgEditar");
                    imgEditar.Visible = false;
                    ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                    imgActivar.Visible = false;
                    ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                    imgDelete.Visible = false;
                    //ImageButton imgConsulta = (ImageButton)e.Row.FindControl("imgConsulta");
                    //imgConsulta.Visible = false;
                }
                if (iddesc != 0)
                {
                    //estado = new tPrediosDescuentoBL().GetByConstraint(Convert.ToInt32(estado)).Activo.ToString();
                    if (new tPrediosDescuentoBL().GetByConstraint(iddesc).Activo == true)
                    {
                        ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                        imgActivar.Visible = false;
                        ImageButton imgNuevo = (ImageButton)e.Row.FindControl("imgNuevo");
                        imgNuevo.Visible = false;
                    }
                    if (new tPrediosDescuentoBL().GetByConstraint(iddesc).Activo == false)
                    {
                        ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                        imgDelete.Visible = false;
                        ImageButton imgNuevo = (ImageButton)e.Row.FindControl("imgNuevo");
                        imgNuevo.Visible = false;
                    }
                }
            }

        }

        #endregion

        protected List<cConcepto> conceptosPorTramite(Int32 idTramite, Int32 ejercicio)
        {
            List<cConcepto> lc = new List<cConcepto>();
            cConcepto c = new cConcepto();

            List<tTramiteDetalle> ltDet = new tTramiteDetalleBL().GetAllIdTramite(idTramite);
 
            ///lc = new tTramiteDetalleBL().GetAllIdTramite(idTramite).Where(o => o.cConcepto.SinDescuento = true).Select(o => o.cConcepto).ToList();
            //lc = new cConceptoBL().GetListForDescuentos(tramite.IdTipoTramite, Convert.ToInt32(DateTime.Today.Year));//Conceptos para el tramite...
            foreach (tTramiteDetalle ltd in ltDet)
            {
                c = new cConceptoBL().GetForDescuentos(ltd.IdConcepto, ejercicio);
                if (c != null)
                {
                    lc.Add(c); }
            }

            Int32 idTipoTramite = Convert.ToInt32(ViewState["IdTipoTramite"]);
            if (idTipoTramite == 3)
            {
                String convenioRecargo = new cParametroSistemaBL().GetValorByClave("CONVENIO_RECARGO");
                if (convenioRecargo != null)
                {
                    c = new cConceptoBL().getConceptoByCriEjercicio(convenioRecargo, ejercicio, idTipoTramite);
                    if (c != null)
                    {
                        lc.Add(c);
                    }
                }

            } 
            return lc;
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
            }            
            grdClaveGenerica.DataSource = tramites;
            grdClaveGenerica.DataBind();

        }

        protected void grdClaveGenerica_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ConsultarRegistro")
            {
                int id = Convert.ToInt32(e.CommandArgument.ToString());
                ViewState["IdTramiteClaveGenerica"] = id;
                llenagridClvegenerica(id);
            }
        }
        private void llenagridClvegenerica(int IdTramite)
        {
            
            int IdPredio = Convert.ToInt32(ViewState["IdPredio"].ToString());
            cPredio predio = new cPredioBL().GetByConstraint(IdPredio);
            lblPred.Text = predio.ClavePredial;
            cContribuyente contribuyente = new cContribuyenteBL().GetByConstraint(predio.IdContribuyente);
            lblPropietario.Text = contribuyente.ApellidoPaterno + " " + contribuyente.ApellidoMaterno + " " + contribuyente.Nombre;

            DataTable DTConceptos = new DataTable();
            List<cConcepto> ConceptosTramite = new List<cConcepto>();
            ConceptosTramite = conceptosPorTramite(IdTramite, Convert.ToInt32(DateTime.Today.Year));
            DTConceptos = ConvertirListaToDataTable(ConceptosTramite);
            DTConceptos.Columns.Add("FechaFin", typeof(DateTime));
            DTConceptos.Columns.Add("Porcentaje", typeof(Decimal));
            DTConceptos.Columns.Add("Estado", typeof(string));

            List<tPrediosDescuento> Descuentos = new tPrediosDescuentoBL().GetForDescuentos(IdPredio, IdTramite);//Descuentos registrados...


            if (Descuentos.Count() > 0)
            {
                foreach (tPrediosDescuento p in Descuentos)
                {
                    for (int i = 0; i < DTConceptos.Rows.Count; i++)
                    {
                        if (DTConceptos.Rows[i]["Id"].ToString() == p.IdConcepto.ToString()) // if id==2
                        {
                            //DTConceptos.Rows[i]["FechaInicio"] = Convert.ToString(p.FechaInicio);
                            DTConceptos.Rows[i]["FechaFin"] = p.FechaFin;//p.FechaFin.ToString("dd/MM/aaaa") ;
                            DTConceptos.Rows[i]["Porcentaje"] = Convert.ToString(p.Porcentaje);
                            DTConceptos.Rows[i]["Estado"] = Convert.ToString(p.Activo);

                            i = DTConceptos.Rows.Count;
                        }
                    }

                }
            }
            grd.DataSource = DTConceptos;
            grd.DataBind();
            grd.Visible = true;
        }
    }
}