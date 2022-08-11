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
                //llenaListas();
            }
        }

        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            cPredio predio = new cPredioBL().GetByClavePredial(txtCve.Text);
            if (predio != null)
            {
                lblPred.Text = predio.ClavePredial;
                cContribuyente contribuyente = new cContribuyenteBL().GetByConstraint(predio.IdContribuyente);
                lblPropietario.Text = contribuyente.Nombre + " " + contribuyente.ApellidoPaterno + " " + contribuyente.ApellidoMaterno;
                ddltipo.Enabled = true;
                llenaGrid();
                //grdDetalle.Visible = true;
            }

            else
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.PredioInexistente), ModalPopupMensaje.TypeMesssage.Alert);
                txtCve.Text = "";
                ddltipo.Enabled = false;
            }
        }

        private void llenaGrid()
        {
            string tipo = ddltipo.SelectedValue.ToString();
            string mesa = tipo == "IP" ? "PREDIAL" : "SERMUN";
            List<cParametroSistema> listparam = new cParametroSistemaBL().GetFilter("Clave", tipo, "TRUE", "Id", "");
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
            DateTime fecha = DateTime.Now;
            cUsuarios U = (cUsuarios)Session["usuario"];
            foreach(GridView gr in grdDetalle.Rows)
            {
                int IdConcepto = Convert.ToInt32(grdDetalle.DataKeys[gr.RowIndex].Values[0].ToString());
                double Descuento = Convert.ToDouble(gr.FindControl("txtDscto"));
                if (Descuento != 00.00)
                {
                    tPrediosDescuento descuento = new tPrediosDescuento();
                    descuento.idConcepto = IdConcepto;
                    descuento.IdPredio = Convert.ToInt32(lblPred.Text);
                    descuento.idtramite = null;
                    descuento.Conepto = "IP";
                    descuento.FechaInicio = DateTime();
                    descuento.FechaFin = DateTime();
                }
            }
            
        }
    }
}