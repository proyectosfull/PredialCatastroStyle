using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Clases.BL;
using Catastro.Controles;
using Clases;
using Clases.Utilerias;
using System.Web.UI.WebControls;

namespace Catastro.Catalogos
{
    public partial class prescripciones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                llenafase();
                limpiacampos();
                oculta(false);
                txtClvCastatral.Focus();
            }
        }

        protected void llenafase()
        {
            ddlTipoFaseIP.DataValueField = "Id";
            ddlTipoFaseIP.DataTextField = "Descripcion";
            ddlTipoFaseIP.DataSource = new cTipoFaseBL().GetAll();
            ddlTipoFaseIP.DataBind();
            ddlTipoFaseIP.Items.Insert(0, new ListItem("Seleccionar Fase", "0"));
            ddlTipoFaseSM.DataValueField = "Id";
            ddlTipoFaseSM.DataTextField = "Descripcion";
            ddlTipoFaseSM.DataSource = new cTipoFaseBL().GetAll();
            ddlTipoFaseSM.DataBind();
            ddlTipoFaseSM.Items.Insert(0, new ListItem("Seleccionar Fase", "0"));
        }
        protected void limpiacampos()
        {
            ViewState["idP"] = null;

            txtClvCastatral.Text = "";

            txtContruyente.Text = "";
            txtCalle.Text = "";
            txtSuperficie.Text = "";
            txtFrente.Text = "";
            txtUso.Text = "";
            txtEstatus.Text = "";

            txtaaFinalIP.Text = "";
            ddlTipoFaseIP.SelectedValue = "0";
            ddlBimestreIP.SelectedValue = "1";
            txtaaFinalSM.Text = "";
            ddlTipoFaseSM.SelectedValue = "0";
            ddlBimestreSM.SelectedValue = "1";
        }
        protected void oculta(bool t) 
        {
            txtaaFinalIP.Enabled = t;
            ddlTipoFaseIP.Enabled =t;
            ddlBimestreIP.Enabled =t;
            txtaaFinalSM.Enabled= t;
            ddlTipoFaseSM.Enabled = t;
            ddlBimestreSM.Enabled = t;
        }
        protected void llenaPant(cPredio Predio)
        {
            ViewState["idP"] = Predio.Id.ToString();
            txtContruyente.Text = Predio.cContribuyente.ApellidoPaterno + " " + Predio.cContribuyente.ApellidoMaterno + " " + Predio.cContribuyente.Nombre + " " + Predio.cContribuyente.RazonSocial;
            txtCalle.Text = Predio.Calle + " No." + Predio.Numero + " Colonia " + Predio.cColonia.NombreColonia + ", " + Predio.Localidad + "  C.P. " + Predio.CP;
            txtSuperficie.Text = Predio.SuperficieTerreno.ToString();
            txtFrente.Text = Predio.MetrosFrente.ToString();
            txtUso.Text = Predio.cUsoSuelo.Clave;
            txtEstatus.Text = Predio.cStatusPredio.Descripcion;
            
            txtaaFinalIP.Text = Predio.AaFinalIp.ToString();
            ddlTipoFaseIP.SelectedValue = Predio.IdTipoFaseIp.ToString();
            ddlBimestreIP.SelectedValue = Predio.BimestreFinIp.ToString();
            txtaaFinalSM.Text = Predio.AaFinalSm.ToString();
            ddlTipoFaseSM.SelectedValue = Predio.IdTipoFaseSm.ToString();
            ddlBimestreSM.SelectedValue = Predio.BimestreFinSm.ToString();
        }
        protected void buscarClaveCatastral(object sender, System.EventArgs e)
        {
            if (txtClvCastatral.Text.Length == 12) {                
                cPredio Predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);
                if (Predio != null)
                {
                    llenaPant(Predio);
                    oculta(true);
                }
                else
                {
                    limpiacampos();
                    oculta(false);
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.PredioInexistente), ModalPopupMensaje.TypeMesssage.Alert);
                }
            }
            else
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.FormatoPredio), ModalPopupMensaje.TypeMesssage.Alert);
                txtClvCastatral.Text = "";
            }
        }
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            llenafase();
            limpiacampos();
            oculta(false);
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            MensajesInterfaz msg = new MensajesInterfaz();
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            cPredio predio = new cPredioBL().GetByConstraint(Convert.ToInt32(ViewState["idP"]));
            predio.AaFinalIp = Convert.ToInt32(txtaaFinalIP.Text);
            predio.IdTipoFaseIp = Convert.ToInt32(ddlTipoFaseIP.SelectedValue);
            predio.BimestreFinIp = Convert.ToInt32(ddlBimestreIP.SelectedValue);
            predio.AaFinalSm = Convert.ToInt32(txtaaFinalSM.Text);
            predio.IdTipoFaseSm = Convert.ToInt32(ddlTipoFaseSM.SelectedValue);
            predio.BimestreFinSm = Convert.ToInt32(ddlBimestreSM.SelectedValue);
            predio.IdUsuario = U.Id;
            predio.FechaModificacion = DateTime.Now;
            msg = new cPredioBL().Update(predio);
            vtnModal.DysplayCancelar = false;
            vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Actualizacion))
            {
                limpiacampos();
                oculta(false);
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Ingreso) ||
              vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Actualizacion))
            {
            }
        }
    }
}