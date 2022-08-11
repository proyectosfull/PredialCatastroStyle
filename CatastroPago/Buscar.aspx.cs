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


namespace CatastroPago
{
    public partial class Buscar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cParametroSistema municipio = new cParametroSistemaBL().GetByClave("MUNICIPIO_COBRO");
                if (municipio.Valor == "ZAPATA")
                {
                    ImagenLogo.ImageUrl = "~/Img/logoZapata.jpg";
                    ViewState["colordiv"] = "#BA5049";
                    ImagenLogo.Height = 102;
                    ImagenLogo.Width = 318;
                }
                else
                {
                    ImagenLogo.ImageUrl = "~/Img/logoZapata.jpg";
                    ViewState["colordiv"] = "#BA5049";
                    ImagenLogo.Height = 102;
                    ImagenLogo.Width = 318;
                    
                }

                //txtClavePredial.Focus();
            }
            //vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        public string colorDiv { get { return ViewState["colordiv"].ToString(); } }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {           
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
       
            //pnl_Modal.Hide();
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
        }

        protected void btnInicio_Click(object sender, EventArgs e)
        {
            if (txtClavePredial.Text.Length != 12)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.FormatoPredio), ModalPopupMensaje.TypeMesssage.Alert);
                txtClavePredial.Text = "";
                return;
            }
            cPredio predio = new cPredioBL().GetByClavePredial(txtClavePredial.Text);
            if(predio == null)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("No se encuentra registradA la clave catastral."), ModalPopupMensaje.TypeMesssage.Alert);
                txtClavePredial.Text = "";
                return;
            }

            if (predio.cStatusPredio.Descripcion != "A")
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("El predio no genero estado de cuenta, favor de pasar a la Dirección de Predial y Catastro"), ModalPopupMensaje.TypeMesssage.Alert);
                txtClavePredial.Text = "";
                return;
            }

            double mesActual = Utileria.Redondeo(DateTime.Today.Month / 2.0);
            int bActual = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(DateTime.Today.Month / 2.0)));
            int ejercicioAct = DateTime.Today.Year;

            //valida si ya se cobra año anticipado
            int mesCobroAnt = Convert.ToInt32(new cParametroCobroBL().GetByClave("MesPagoAnticipado"));
            if (DateTime.Today.Month >= mesCobroAnt && mesCobroAnt > 0)
            {
                int EjercicioAnticipado = Convert.ToInt32(new cParametroCobroBL().GetByClave("EjercicioAnticipado"));
                ejercicioAct = EjercicioAnticipado;
            }
            //valida siel predio ya se encuentrra pagado
            if (predio.AaFinalIp * 10 + predio.BimestreFinIp >= ejercicioAct * 10 + 6)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("El predio ya se encuentra pagado. Gracias por su Pago!"), ModalPopupMensaje.TypeMesssage.Alert);
                txtClavePredial.Text = "";
                return;
            }
            //valida que el predio tenga una fecha de avaluo mayor a 2 años a la fecha actual
            string avaluo = new cParametroSistemaBL().GetByClave("FECHAAVALUOINTERNET").Valor;
            if (avaluo == "SI")
            {
                DateTime fecha = DateTime.Now.AddMonths(-24);
                //se elimina la validacion de Base gravable
                //cBaseGravable baseg = new cBaseGravableBL().GetByPredAnio(predio.Id, DateTime.Today.Year);

                //if (baseg != null)
                //{
                    if ( predio.FechaAvaluo < fecha )
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription("Favor de pasar a actualizar sus datos a la Dirección de Predial y Catastro del Municipio;  para poder realizar su pago por esté medio, gracias."), ModalPopupMensaje.TypeMesssage.Alert);
                        txtClavePredial.Text = "";
                        return;
                    }
                //}
            }

            if ((txtIniciales.Text.Trim().ToUpper() != "prb4t") && (txtIniciales.Text.Trim().ToUpper() != "PRB4T"))
            {
                string nombre = predio.cContribuyente.ApellidoPaterno + " " + predio.cContribuyente.ApellidoMaterno + " " + predio.cContribuyente.Nombre;

                if (InicialesUser(nombre, true).ToUpper().Trim() != InicialesUser(txtIniciales.Text, false).ToUpper().Trim() )
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription("Las iniciales no corresponden al titular del predio"), ModalPopupMensaje.TypeMesssage.Alert);
                    txtClavePredial.Text = "";
                    return;
                }
            }

            Session["idPredio"] = predio.Id;
            Response.Redirect("~/EdoPredial.aspx", false);
        }

        private string InicialesUser(string as_IniUser, bool ab_bandera)
        {
            as_IniUser.Trim();
            string ls_iniciales_user = "", ls_letra_anterior;
            string[] stringArray = new string[as_IniUser.Length];

            if (ab_bandera == true)
            {
                ls_letra_anterior = as_IniUser.Substring(0, 1);
                stringArray[0] = as_IniUser.Substring(0, 1);
                for (int li_i = 1; li_i < as_IniUser.Length; li_i++)
                {
                    if (as_IniUser.Substring(li_i, 1) != " " && ls_letra_anterior == " ")
                        stringArray[li_i] = as_IniUser.Substring(li_i, 1);
                    ls_letra_anterior = as_IniUser.Substring(li_i, 1);
                }
            }
            else
            {
                for (int li_i = 0; li_i < as_IniUser.Length; li_i++)
                {
                    stringArray[li_i] = as_IniUser.Substring(li_i, 1);
                }
            }
            Array.Sort(stringArray);
            for (int li_i = 0; li_i < as_IniUser.Length; li_i++)
            {
                ls_iniciales_user += stringArray[li_i];
            }
            return ls_iniciales_user;
        }

        protected void btFacturacion_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Recibos/Facturacion.aspx", false); 
        }
    }
}