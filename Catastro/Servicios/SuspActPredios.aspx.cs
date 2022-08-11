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
    public partial class SuspActPredios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                llenaddl();
                limpiacampos();
                oculta(false);
                txtClvCastatral.Focus();
            }
        }

        protected void llenaddl()
        {
            List<cStatusPredio> lest = new cStatusPredioBL().GetAll();
            foreach( cStatusPredio est in lest)
            {
                switch (est.Descripcion)
                {
                    case "A":
                        ddlEstatusNu.Items.Insert(0, new ListItem("ACTIVO", Convert.ToString(est.Id)));
                        break;
                    case "B":
                        ddlEstatusNu.Items.Insert(0, new ListItem("BAJA", Convert.ToString(est.Id)));
                        break;
                    case "S":
                        ddlEstatusNu.Items.Insert(0, new ListItem("SUSPENDIDO", Convert.ToString(est.Id)));
                        break;
                    default:
                        ddlEstatusNu.Items.Insert(0, new ListItem(est.Descripcion, Convert.ToString(est.Id)));
                        break;
                }
            }
            ddlEstatusNu.Items.Insert(0, new ListItem("Seleccionar Estatus Nuevo", "0"));
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

            txtEstatusAnt.Text = "";
            ddlEstatusNu.SelectedValue = "0";
            txtObservacion.Text = "";
        }
        protected void oculta(bool t) 
        {
            ddlEstatusNu.Enabled =t;
            txtObservacion.Enabled =t;
        }
        protected void llenaPant(cPredio Predio)
        {
            ViewState["idP"] = Predio.Id.ToString();
            txtContruyente.Text =  Predio.cContribuyente.ApellidoPaterno + " " + Predio.cContribuyente.ApellidoMaterno +" "+ Predio.cContribuyente.Nombre ;
            txtCalle.Text = Predio.Calle + " No." + Predio.Numero + " Colonia " + Predio.cColonia.NombreColonia + ", " + Predio.Localidad + "  C.P. " + Predio.CP;
            txtSuperficie.Text = Predio.SuperficieTerreno.ToString();
            txtFrente.Text = Predio.MetrosFrente.ToString();
            txtUso.Text = Predio.cUsoSuelo.Clave;
            txtObservacion.Text = Predio.Observacion;

            txtEstatusAnt.Text = Predio.cStatusPredio.Descripcion;
            try { ddlEstatusNu.SelectedValue = Predio.cStatusPredio.Id.ToString(); }catch { ddlEstatusNu.SelectedValue = "0"; }
            //txtObservacion.Text = Predio.ToString();
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
            llenaddl();
            limpiacampos();
            oculta(false);
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            MensajesInterfaz msg = new MensajesInterfaz();
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            cPredio predio = new cPredioBL().GetByConstraint(Convert.ToInt32(ViewState["idP"]));
            predio.IdStatusPredio = Convert.ToInt32(ddlEstatusNu.SelectedValue);
            predio.Observacion = txtObservacion.Text;
            predio.IdUsuario = U.Id;
            predio.FechaModificacion = DateTime.Now;
            msg = new cPredioBL().Update(predio);
            vtnModal.DysplayCancelar = false;
            vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
            if (txtObservacion.Text.Trim() != "")
            {
                if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Actualizacion))
                {
                    cPredioObservacion obs = new cPredioObservacion();
                    obs.IdPredio = Convert.ToInt32(ViewState["idP"].ToString());
                    obs.Observacion = txtObservacion.Text;
                    obs.Activo = true;
                    obs.IdUsuario = U.Id;
                    obs.FechaModificacion = DateTime.Now;
                    obs.Origen = "PRESCRIPCION";
                    msg = new cPredioObservacionBL().Insert(obs);
                    
                    vtnModal.DysplayCancelar = false;
                    vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);

                    msg = new cPredioBL().Update(predio);

                    vtnModal.DysplayCancelar = false;
                    vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
                    limpiacampos();
                    oculta(false);
                }
            }
            else
            {
                limpiacampos();
                oculta(false);
            }
        }
    }
}