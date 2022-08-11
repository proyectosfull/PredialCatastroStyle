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
    public partial class DescuentoPredio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //List<cDescuento> descuentos = new cDescuentoBL().GetAll().OrderBy(d => d.Porcentaje).ToList();
                //Session["descuentos"] = descuentos;
                llenaTramite();
            }
        }

        private void llenaTramite()
        {
            ddlTramite.DataValueField = "Id";
            ddlTramite.DataTextField = "Tramite";
            ddlTramite.DataSource = new cTipoTramiteBL().GetAll();
            ddlTramite.DataBind();
            //ddlTramite.Items.Insert(0, new ListItem("Todos", ""));
        }
        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            cPredio predio = new cPredioBL().GetByClavePredial(txtCve.Text);
            if (predio != null)
            {
                lblPred.Text = predio.ClavePredial;
                cContribuyente contribuyente = new cContribuyenteBL().GetByConstraint(predio.IdContribuyente);
                lblPropietario.Text = contribuyente.Nombre + " " + contribuyente.ApellidoPaterno + " " + contribuyente.ApellidoMaterno;
                ddlTramite.Enabled = true;
                llenaGrid();
                btnGuardar.Visible = true;
                
                //grdDetalle.Visible = true;
            }

            else
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.PredioInexistente), ModalPopupMensaje.TypeMesssage.Alert);
                txtCve.Text = "";
                //ddlTramite.Enabled = false;
            }
        }

        private void llenaGrid()
        {
            string tipo = ddlTramite.SelectedValue.ToString();
            string mesa = tipo == "IP" ? "PREDIAL" : "SERMUN";
            List<cParametroSistema> listparam = new cParametroSistemaBL().GetFilterFin("Clave", tipo, "TRUE", "Id", "");
            if(listparam!= null)
            {
                List<cConcepto> conceptos = new List<cConcepto>();
                cConcepto c = new cConcepto();
                cMesa m = new cMesaBL().GetByCampo("Nombre",mesa,"TRUE");
                for(int i=0;i < listparam.Count();i++)
                {
                    c = new cConceptoBL().GetByCampo("Cri", listparam[i].Valor.ToString(),m.Id, "TRUE", "Cri", "");
                    if (c!= null)
                        conceptos.Add(c);
                }
                grdDetalle.DataSource = conceptos;
                grdDetalle.DataBind();
               
            }
            
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            cPredio pred = new cPredioBL().GetByClavePredial(lblPred.Text);
            
            cUsuarios U = (cUsuarios)Session["usuario"];
            foreach(GridViewRow gr in grdDetalle.Rows)
            {
                TextBox Descuento = (TextBox)gr.FindControl("txtDscto");
                string r = Descuento.Text;
                
                if ((String.IsNullOrEmpty(Descuento.Text))==false && Convert.ToDecimal(Descuento.Text) > 0)
                {
                    tPrediosDescuento desc = new tPrediosDescuento();
                    desc.IdConcepto = Convert.ToInt32(grdDetalle.DataKeys[gr.RowIndex].Values[0].ToString()); 
                    desc.IdPredio = pred.Id;

                    desc.IdTramite = null;
                    desc.Concepto = ddlTramite.SelectedValue;

                    desc.IdTramite = null;
                    desc.Concepto = " ";// ddlTipo.SelectedValue;

                    desc.FechaInicio = DateTime.Now;
                    TextBox FechaFin = (TextBox)gr.FindControl("txtFechaFin");
                    desc.FechaFin = Convert.ToDateTime(FechaFin.Text);
                    desc.Porcentaje = Convert.ToDecimal(Descuento.Text);
                    desc.Activo = true;
                    desc.IdUsuario = U.IdUsuario;
                    desc.FechaModificacion = DateTime.Now;
                    MensajesInterfaz insert = new tPrediosDescuentoBL().Insert(desc);
                    if (insert == MensajesInterfaz.Ingreso)
                    {
                        //limpiaCampos();
                    }
                    else
                        vtnModal.ShowPopup(new Utileria().GetDescription(insert), ModalPopupMensaje.TypeMesssage.Error);
                }
            }
            
        }
    }
}