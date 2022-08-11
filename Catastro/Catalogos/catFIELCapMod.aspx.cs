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
    public partial class catFIELCapMod : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (Session["parametro"] != null)
                {
                    Dictionary<string, string> parametros = (Dictionary<string, string>)Session["parametro"];
                    if (parametros["tipoPantalla"] == "M")
                    {

                        int id = Convert.ToInt32(parametros["idFIEL"]);
                        cFIEL FIEL = new cFIELBL().GetByConstraint(id);
                        limpiaCampos();
                        txtNombre.Text = FIEL.Nombre;
                        txtRFC.Text=FIEL.RFC;
                        txtCalle.Text = FIEL.Calle;
                        txtNoInterior.Text = FIEL.NoInterior;
                        txtNoExterior.Text = FIEL.NoExterior;
                        txtColonia.Text = FIEL.Colonia;
                        txtCodigoPostal.Text = FIEL.CodigoPostal;
                        txtLocalidad.Text = FIEL.Localidad;
                        txtMunicipio.Text = FIEL.Municipio;
                        txtEstado.Text = FIEL.Estado;
                        txtPais.Text = FIEL.Pais;
                        txtReferencia.Text = FIEL.Pais;
                        txtKeyPass.Text = FIEL.KeyPass;
                        //txtKeyPass.Visible = false;
                        //lblKeyPass.Visible = false;

                        habilitaCampos(true);  
                        
                    }
                    if (parametros["tipoPantalla"] == "C")
                    {

                        int id = Convert.ToInt32(parametros["idFIEL"]);
                        cFIEL FIEL = new cFIELBL().GetByConstraint(id);

                        limpiaCampos();
                        txtNombre.Text = FIEL.Nombre;
                        txtRFC.Text = FIEL.RFC;
                        txtCalle.Text = FIEL.Calle;
                        txtNoInterior.Text = FIEL.NoInterior;
                        txtNoExterior.Text = FIEL.NoExterior;
                        txtColonia.Text = FIEL.Colonia;
                        txtCodigoPostal.Text = FIEL.CodigoPostal;
                        txtLocalidad.Text = FIEL.Localidad;
                        txtMunicipio.Text = FIEL.Municipio;
                        txtEstado.Text = FIEL.Estado;
                        txtPais.Text = FIEL.Pais;
                        txtReferencia.Text = FIEL.Pais;
                        txtKeyPass.Text = FIEL.KeyPass;
                        txtKeyPass.Visible = false;
                        lblKeyPass.Visible = false;

                        habilitaCampos(false);
                        btnGuardar.Visible = false;
                    }
                }
                vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
            }
            
        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            Dictionary<string, string> parametros = (Dictionary<string, string>)Session["parametro"];


            if (parametros==null)
            {
                cFIEL FIEL = new cFIEL();

                FIEL.KeyFile = fileKey.FileBytes;
                FIEL.CerFile = fileCer.FileBytes;
                FIEL.Logo = fileLogo.FileBytes;
                FIEL.KeyPass = new GeneraRecibo.utilerias().Encrypt(txtKeyPass.Text);
                FIEL.RFC = txtRFC.Text;
                FIEL.Calle = txtCalle.Text;
                FIEL.Municipio = txtMunicipio.Text;
                FIEL.Estado = txtEstado.Text;
                FIEL.Pais = txtPais.Text;
                FIEL.CodigoPostal = txtCodigoPostal.Text;
                FIEL.Nombre = txtNombre.Text;
                FIEL.NoExterior = txtNoExterior.Text;
                FIEL.NoInterior = txtNoInterior.Text;
                FIEL.Colonia = txtColonia.Text;
                FIEL.Localidad = txtLocalidad.Text;
                FIEL.Referencia = txtReferencia.Text;

               
                FIEL.IdUsuario = (short)U.Id;
                FIEL.Activo = true;
                FIEL.FechaModificacion = DateTime.Now;

                

                MensajesInterfaz msg = new cFIELBL().Insert(FIEL);
                vtnModal.DysplayCancelar = false;
                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Confirm);
            }
            else
            {
                cFIEL FIEL = new cFIELBL().GetByConstraint(Convert.ToInt32(parametros["idFIEL"]));

                FIEL.KeyFile = fileKey.FileBytes;
                FIEL.CerFile = fileCer.FileBytes;
                FIEL.Logo = fileLogo.FileBytes;
                //FIEL.KeyPass = txtKeyPass.Text;
                FIEL.RFC = txtRFC.Text;
                FIEL.Calle = txtCalle.Text;
                FIEL.Municipio = txtMunicipio.Text;
                FIEL.Estado = txtEstado.Text;
                FIEL.Pais = txtPais.Text;
                FIEL.CodigoPostal = txtCodigoPostal.Text;
                FIEL.Nombre = txtNombre.Text;
                FIEL.NoExterior = txtNoExterior.Text;
                FIEL.NoInterior = txtNoInterior.Text;
                FIEL.Colonia = txtColonia.Text;
                FIEL.Localidad = txtLocalidad.Text;
                FIEL.Referencia = txtReferencia.Text;

                FIEL.IdUsuario = (short)U.Id;
                FIEL.FechaModificacion = DateTime.Now;
                MensajesInterfaz msg = new cFIELBL().Update(FIEL);
                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
            }


        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("catFIEL.aspx");
        }

             private void limpiaCampos()
        {
            

            ViewState["idMod"] = null;
        }
        private void habilitaCampos(bool activa)
        {

            fileKey.Enabled=activa;
            fileCer.Enabled=activa;
            fileLogo.Enabled = activa;
            txtKeyPass.Enabled = activa;
            txtRFC.Enabled = activa;
            txtCalle.Enabled = activa;
            txtMunicipio.Enabled = activa;
            txtEstado.Enabled = activa;
            txtPais.Enabled = activa;
            txtCodigoPostal.Enabled = activa;
            txtNombre.Enabled = activa;
            txtNoExterior.Enabled = activa;
            txtNoInterior.Enabled = activa;
            txtColonia.Enabled = activa;
            txtLocalidad.Enabled = activa;
            txtReferencia.Enabled = activa;

            fileCer.Enabled = activa;
            fileKey.Enabled = activa;
            fileLogo.Enabled = activa;

        }

        
        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Ingreso) ||
              vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Actualizacion))
            {
                Response.Redirect("catFIEL.aspx");
            }
        }
    }
}