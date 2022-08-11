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


namespace Catastro.Catalogos
{
    public partial class catConcepto1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ViewState["tr"] = "0";
            if (!IsPostBack)
            {
                //ActivaControles(false);
                lbl_titulo.Text = "Alta Concepto";
                
                //limpiaCampos();
                //habilitaCampos(true);
                btn_Guardar.Visible = true;
                txtNombre.Focus();
                 Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros = (Dictionary<string, string>)Session["parametro"];
                if (parametros != null)
                {
                    if (parametros.ContainsKey("tipoPantalla"))
                    {
                         if (parametros["tipoPantalla"] == "C")
                         {
                            if (parametros.ContainsKey("idConcepto"))
                            {
                                llenaMesa();
                                llenaTipoTramite();
                                lbl_titulo.Text = "Consulta Concepto";
                                int idConcepto=Convert.ToInt32(parametros["idConcepto"]);
                                CargarDatos(idConcepto);
                                habilitaCampos(false);                             
                                ViewState["tipoPantalla"] = "C";
                            }
                         }
                         else if (parametros["tipoPantalla"] == "M")
                         {
                            if (parametros.ContainsKey("idConcepto"))
                            {
                                llenaMesa();
                                llenaTipoTramite();
                                lbl_titulo.Text = "Modifica Concepto";
                                int idConcepto=Convert.ToInt32(parametros["idConcepto"]);
                                habilitaCampos(true);
                                CargarDatos(idConcepto);
                                ViewState["tipoPantalla"] = "M";
                                ViewState["idConcepto"] = idConcepto.ToString();
                            }
                         }
                        else if (parametros["tipoPantalla"] == "N")
                         {
                            lbl_titulo.Text = "Nuevo Concepto";                           
                            llenaMesa();
                            llenaTipoTramite();
                            ViewState["tipoPantalla"] = "N";
                            habilitaCampos(true);
                            ddlTipoTramite.SelectedValue = "%";
                            ddlTipoCobro.SelectedValue = "0";
                            cbxActivo.Checked = true;
                            ddlTipoCobro_SelectedIndexChanged(null, null);
                         }
                    }
                }

                          
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
            
        }

        private void CargarDatos(int idConcepto)
        {          
            llenaMesa();
            cConcepto concepto = new cConceptoBL().GetByConstraint(idConcepto);
            limpiaCampos();
            txtCri.Text = concepto.Cri;
            txtNombre.Text = concepto.Nombre;
            txtDescripcion.Text = concepto.Descripcion;          

            if (concepto.TipoCobro == "S")
            {
                
                ddlTipoCobro.SelectedValue = "1";
                ddlTipoCobro_SelectedIndexChanged(null, null);
                //rblEsAdicional_SelectedIndexChanged(null, null);
            }
            else if (concepto.TipoCobro == "I")
            {
               
                ddlTipoCobro.SelectedValue = "2";
                ddlTipoCobro_SelectedIndexChanged(null, null);
                //rblEsAdicional_SelectedIndexChanged(null, null);
            }
          

            txtSalarioMin.Text = Convert.ToString(concepto.SalarioMin);
            txtSalarioMax.Text = Convert.ToString(concepto.SalarioMax);
            if(concepto.EsAdicional==true)
                cbesAdicional.Checked = true;
            if (concepto.Adicional == true)
                cbAdicional.Checked = true;
            txtImporte.Text = Convert.ToString(concepto.Importe);
            txtEjercicio.Text = concepto.Ejercicio.ToString();          
            ddlIdMesa.SelectedValue = Convert.ToString(concepto.IdMesa);
#pragma warning disable CS0219 // La variable 'h' está asignada pero su valor nunca se usa
            int h = 0;
#pragma warning restore CS0219 // La variable 'h' está asignada pero su valor nunca se usa
            foreach(ListItem i in ddlTipoTramite.Items)
            {
                if (i.Value == Convert.ToString(concepto.IdTipoTramite))
                {
                    ddlTipoTramite.SelectedValue = Convert.ToString(concepto.IdTipoTramite);
                    h = 1;
                }
            }
            ddlTipoCobro.SelectedValue = Convert.ToString(concepto.TipoCobro);
            ddlTipoCobro_SelectedIndexChanged(null,null);
            if(concepto.Activo)
                cbxActivo.Checked = true;
            if (concepto.SinDescuento)
                chkDescuento.Checked = true;
            hdfProdServ.Value = concepto.cProdServ.Id.ToString();
            txtxProdServ.Text=concepto.cProdServ.ClaveProdServ + " " +concepto.cProdServ.Descripcion;
            hdfUnidadMedida.Value = concepto.cUnidadMedida.Id.ToString();
            txtUnidadMedidia.Text = concepto.cUnidadMedida.ClaveUnidad + " " + concepto.cUnidadMedida.Nombre;
        }
        private void limpiaCampos()
        {
            txtCri.Text = "";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtSalarioMax.Text = "";
            txtSalarioMin.Text = "";
            txtImporte.Text = "";
            txtEjercicio.Text = "";
            ddlIdMesa.SelectedIndex = 0;
            //chkAdicional.Checked = false;
            chkDescuento.Checked = false;
            //chkAgrava.Checked = false;
            //txtEjercicio.Text = "";
            cbesAdicional.Checked = false;
            cbAdicional.Checked = false;
          
        }

        private void habilitaCampos(bool activa)
        {
            txtCri.Enabled = activa;
            txtNombre.Enabled = activa;
            txtDescripcion.Enabled = activa;          
            chkDescuento.Enabled = activa;
            ddlTipoCobro.Enabled = activa;
            ddlIdMesa.Enabled = activa;
            txtEjercicio.Enabled = activa;
            //chkAdicional.Enabled = activa;           
            //txtEjercicio.Enabled = activa;
            ddlTipoTramite.Enabled = activa;
            txtSalarioMax.Enabled = activa;
            txtSalarioMin.Enabled = activa;
            txtImporte.Enabled = activa;
            txtxProdServ.Enabled = activa;
            txtUnidadMedidia.Enabled = activa;
            cbesAdicional.Enabled = activa;
            cbAdicional.Enabled = activa;

            btn_Guardar.Visible = activa;

        }
    
        #region Combos

        private void llenaMesa()
        {
            ddlIdMesa.DataValueField = "Id";
            ddlIdMesa.DataTextField = "Nombre";
            ddlIdMesa.DataSource = new cMesaBL().GetAll();
            ddlIdMesa.DataBind();
            ddlIdMesa.Items.Insert(0, new ListItem("Seleccionar Mesa", "%"));
        }
        private void llenaTipoTramite()
        {
            ddlTipoTramite.DataValueField = "Id";
            ddlTipoTramite.DataTextField = "Tramite";
            ddlTipoTramite.DataSource = new cTipoTramiteBL().GetAll();
            ddlTipoTramite.DataBind();
            ddlTipoTramite.Items.Insert(0, new ListItem("Seleccionar Tipo Tramite", "%"));
        }
        #endregion
    
        protected void ddlTipoCobro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoCobro.SelectedItem.Value == "0")
            {
                lblSalarioMin.Enabled = false;
                lblSalarioMax.Enabled = false;
                txtSalarioMin.Enabled = false;
                rfvSalarioMin.Enabled = false;
                revSalarioMin.Enabled = false;
                txtSalarioMin.Text = "";
                txtSalarioMax.Enabled = false;
                rfvSalarioMax.Enabled = false;
                revSalarioMax.Enabled = false;
                txtSalarioMax.Text = "";

                lblImporte.Enabled = false;
                txtImporte.Enabled = false;
                rfvImporte.Enabled = false;
                revImporte.Enabled = false;
                //chkAdicional.Checked = false;
                txtImporte.Text = "";
            }
            if (ddlTipoCobro.SelectedItem.Value == "S")
            {
                lblSalarioMin.Enabled = true;
                lblSalarioMax.Enabled = true;
                txtSalarioMin.Enabled = true;
                rfvSalarioMin.Enabled = true;
                revSalarioMin.Enabled = true;
                txtSalarioMax.Enabled = true;
                rfvSalarioMax.Enabled = true;
                revSalarioMax.Enabled = true;

                lblImporte.Enabled = false;
                txtImporte.Enabled = false;
                rfvImporte.Enabled = false;
                revImporte.Enabled = false;
               // chkAdicional.Checked = false;
                txtImporte.Text = "";
            }
            else if (ddlTipoCobro.SelectedItem.Value == "P" || ddlTipoCobro.SelectedValue=="I" || ddlTipoCobro.SelectedValue=="T")
            {
                lblSalarioMin.Enabled = false;
                lblSalarioMax.Enabled = false;
                rfvSalarioMin.Enabled = false;
                revSalarioMin.Enabled = false;
                txtSalarioMax.Enabled = false;
                txtSalarioMin.Enabled = false;
                rfvSalarioMax.Enabled = false;
                revSalarioMax.Enabled = false;

                lblImporte.Enabled = true;
                txtImporte.Enabled = true;
                rfvImporte.Enabled = true;
                revImporte.Enabled = true;
               // chkAdicional.Checked = false;
                txtSalarioMin.Text = "";
                txtSalarioMax.Text = "";
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];
            vtnModal.DysplayCancelar = false;
          
                cConcepto conceptoOld = new cConcepto();
                bool duplicado=false;
                string tipo = ViewState["tipoPantalla"].ToString();
                //if (ViewState["tipoPantalla"].ToString() == "M")
                //{
                //    tipo = "M";
                //    conceptoOld = new cConceptoBL().GetByConstraint(Convert.ToInt32(ViewState["idConcepto"].ToString()));
                //    //conceptoOld = new cConceptoBL().GetByCri(txtCri.Text.Trim());
                //    if (conceptoOld.Cri == txtCri.Text.Trim() && conceptoOld.Ejercicio == Convert.ToInt32(txtEjercicio.Text))
                //        duplicado = false;
                //    else
                //    {
                //        //cConcepto conceptoNuevo = new cConcepto();
                //        //conceptoNuevo = new cConceptoBL().GetByCriEjercicio(txtCri.Text.Trim(), Convert.ToInt32(ddlGrupoAnio.SelectedItem.Text));
                //        //if (conceptoNuevo != null)
                //        //    duplicado = true;
                //    }
                //}
                //else if (ViewState["tipoPantalla"].ToString() == "N")
                //{
                //    tipo = "N";
                //    conceptoOld = new cConceptoBL().getConceptoByCriEjercicio(txtCri.Text.Trim(), Convert.ToInt32(txtEjercicio.Text));
                //    if (conceptoOld != null)
                //        duplicado = true;
                //}
                
                if (!duplicado)
                {
                    cConcepto concepto = new cConcepto();
                    if (tipo == "M")
                    {
                        int idConcepto=Convert.ToInt32(ViewState["idConcepto"].ToString());
                        concepto = new cConceptoBL().GetByConstraint(idConcepto);
                       // concepto.Id = idConcepto;
                    }
                    concepto.Adicional = true;
                    if (!cbAdicional.Checked)
                        concepto.Adicional = false;
                    concepto.EsAdicional = true;
                    if (!cbesAdicional.Checked)
                        concepto.EsAdicional = false;
                    concepto.SinDescuento = chkDescuento.Checked;
                    if(ddlTipoTramite.SelectedValue!="%")
                    concepto.IdTipoTramite = Convert.ToInt32(ddlTipoTramite.SelectedValue);
                    concepto.Cri = txtCri.Text;
                    concepto.Nombre = txtNombre.Text;
                    concepto.Descripcion = txtDescripcion.Text;
                    if(ddlTipoCobro.SelectedValue=="S")
                    {
                        //concepto.TipoCobro = Convert.ToString(new cTipoCobroBL().GetCodigo(ddlTipoCobro.SelectedValue).Id);
                        concepto.TipoCobro = "S";
                        concepto.SalarioMin = Convert.ToInt32(txtSalarioMin.Text);
                        concepto.SalarioMax = Convert.ToInt32(txtSalarioMax.Text);
                        concepto.Importe = 0;
                    }
                    if (ddlTipoCobro.SelectedValue == "I" || ddlTipoCobro.SelectedValue == "P" || ddlTipoCobro.SelectedValue == "T")
                    {
                        //concepto.TipoCobro = Convert.ToString(new cTipoCobroBL().GetCodigo(ddlTipoCobro.SelectedValue).Id);
                        concepto.TipoCobro = ddlTipoCobro.SelectedValue;
                        concepto.Importe = Convert.ToDecimal(txtImporte.Text);
                        concepto.SalarioMin = 0;
                        concepto.SalarioMax = 0;
                    }
                    concepto.IdMesa = Convert.ToInt16(ddlIdMesa.SelectedValue);                
                    concepto.Ejercicio = Convert.ToInt16(txtEjercicio.Text);
                    concepto.IdUsuario = U.Id;
                    concepto.Activo = false;
                    if(cbxActivo.Checked)
                        concepto.Activo = true;
                    concepto.FechaModificacion = DateTime.Now;
                    if (ddlTipoCobro.SelectedItem.Value == "1")
                        concepto.TipoCobro = "S";
                    else if (ddlTipoCobro.SelectedItem.Value == "2")
                        concepto.TipoCobro = "I";
                concepto.IdProdServ = Convert.ToInt32(hdfProdServ.Value);
                concepto.IdUnidadMedida = Convert.ToInt32(hdfUnidadMedida.Value);
                MensajesInterfaz msg=new MensajesInterfaz();
                    if (tipo == "N")
                        msg = new cConceptoBL().Insert(concepto);
                    else if (tipo == "M")
                        msg= new cConceptoBL().Update(concepto);

                    vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Confirm);
                }
                else
                {
                    vtnModal.DysplayCancelar = false;
                    vtnModal.ShowPopup("Ya existe un Concepto con CRI : " + txtCri.Text.Trim() + " en el Ejercicio: " + txtEjercicio.Text, ModalPopupMensaje.TypeMesssage.Alert);
                    //ViewState["tr"]="1";
                }
         }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Session["parametro"] = null;
            Response.Redirect("~/Catalogos/catBusquedaConcepto.aspx");
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Ingreso) ||
              vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Actualizacion))
            {
                Response.Redirect("~/Catalogos/catBusquedaConcepto.aspx");
            }

        }

      

        
    }
}