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
    public partial class catContribuyente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["parametro"] != null)
                {
                    Dictionary<string, string> parametros = (Dictionary<string, string>)Session["parametro"];
                    if (parametros.ContainsKey("idContribuyente"))
                    {
                        llenaConfiguracion(Convert.ToInt32(parametros["idContribuyente"]));
                        if (parametros.ContainsKey("tipoPantalla"))
                        {
                            if (parametros["tipoPantalla"] == "C")
                            {
                                habilitaCampos(false);
                                lblTitulo.Text = "Consulta Contribuyente";

                            }
                            else {
                                hdfId.Value = parametros["idContribuyente"];
                                lblTitulo.Text = "Edición de Contribuyente";
                            }
                        }
                    }
                }
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            vtnModal.DysplayCancelar = false;
            cContribuyente Contribuyente = new cContribuyente();
            MensajesInterfaz msg = new MensajesInterfaz();

            if (!(hdfId.Value == string.Empty || hdfId.Value == "0"))
            {
                Contribuyente = new cContribuyenteBL().GetByConstraint(Convert.ToInt32(hdfId.Value));
            }
            Contribuyente.Nombre = txtNombre.Text;
            Contribuyente.ApellidoPaterno = txtApellidoP.Text;
            Contribuyente.ApellidoMaterno = txtApellidoM.Text;
            Contribuyente.RazonSocial = txtRazon.Text;
            Contribuyente.Calle = txtCalle.Text;
            Contribuyente.Numero = txtNumero.Text;
            Contribuyente.Colonia = txtColonia.Text;
            Contribuyente.Localidad = txtLocalidad.Text;
            Contribuyente.Municipio = txtMunicipio.Text;
            Contribuyente.Estado = txtEstado.Text;
            Contribuyente.CP = txtCP.Text;
            Contribuyente.Email = txtEmail.Text;
            Contribuyente.Telefono = txtTelefono.Text;
            Contribuyente.Curp = txtCurp.Text;
            Contribuyente.IdUsuario = U.Id;
            Contribuyente.PorcCoPropietario = 0;
            Contribuyente.IdPropietarioTitular = 0;//idpredio
            Contribuyente.Activo = true;
            Contribuyente.FechaModificacion = DateTime.Now;
            Contribuyente.AdultoMayor = chbAdultoMayor.Checked ? "S": "N";
            Contribuyente.Referencia = txtReferencia.Text;
            
            if (hdfId.Value == string.Empty || hdfId.Value == "0")
            {
                msg = new cContribuyenteBL().Insert(Contribuyente);
            }
            else
            {
                msg = new cContribuyenteBL().Update(Contribuyente);
            }
            vtnModal.DysplayCancelar = false;
            vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Confirm);
            
        } 

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Session["parametro"] = null;
            Response.Redirect("BusquedaContribuyentes.aspx");
        }      

        private void llenaConfiguracion(int Id)
        {
            cContribuyente contri = new cContribuyenteBL().GetByConstraint(Id);
            txtNombre.Text = contri.Nombre;
            txtApellidoP.Text = contri.ApellidoPaterno;
            txtApellidoM.Text = contri.ApellidoMaterno;
            txtCalle.Text = contri.Calle;
            txtNumero.Text = contri.Numero;
            txtColonia.Text = contri.Colonia;
            txtLocalidad.Text = contri.Localidad;
            txtMunicipio.Text = contri.Municipio;
            txtEstado.Text = contri.Estado;
            txtCP.Text = contri.CP;
            txtEmail.Text = contri.Email;
            txtTelefono.Text = contri.Telefono;
            txtCurp.Text = contri.Curp;
            txtRazon.Text = contri.RazonSocial;
            chbAdultoMayor.Checked = contri.AdultoMayor == null ? false : contri.AdultoMayor.Equals("S") ? true : false;
            txtReferencia.Text = contri.Referencia;
        }
        private void habilitaCampos(bool activa)
        {
            rbltipoPersona.Enabled = activa;
            txtNombre.Enabled = activa;
            txtApellidoP.Enabled = activa;
            txtApellidoM.Enabled = activa;
            txtCalle.Enabled = activa;
            txtNumero.Enabled = activa;
            txtColonia.Enabled = activa;
            txtLocalidad.Enabled = activa;
            txtMunicipio.Enabled = activa;
            txtEstado.Enabled = activa;
            txtCP.Enabled = activa;
            txtEmail.Enabled = activa;
            txtTelefono.Enabled = activa;
            txtCurp.Enabled = activa;
            btnGuardar.Visible = activa;
            chbAdultoMayor.Enabled = activa;
            txtRazon.Enabled = activa;
            txtReferencia.Enabled = activa;
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {

            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Ingreso) ||
              vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Actualizacion))
            {
                //Session["parametro"] = null;
                Response.Redirect("BusquedaContribuyentes.aspx");
            }
        }

        protected void rbltipoPersona_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbltipoPersona.SelectedValue == "Fisica")
            {
                //rfvApellidoPaterno.Enabled = true;
                //rvfCurp.Enabled = true;
                chbAdultoMayor.Enabled = true;
            }
            else
            {
                //rfvApellidoPaterno.Enabled = false;
                //rvfCurp.Enabled = false;                
                chbAdultoMayor.Checked = false;
                chbAdultoMayor.Enabled = false;
            }
        }
    }
}