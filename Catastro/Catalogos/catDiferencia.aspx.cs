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
using System.Data;
//using Clases.Utilerias;

namespace Catastro.Catalogos
{
    public partial class catDiferencia : System.Web.UI.Page
    {
        Impuesto i = new Impuesto();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Session["parametro"] != null)
                {
                    Dictionary<string, string> parametros = (Dictionary<string, string>)Session["parametro"];
                    if (parametros.ContainsKey("idDiferencia"))
                    {
                        txtClvCastatral.Enabled = false;
                        imbBuscar.Visible = false;
                        llenaConfiguracion(Convert.ToInt32(parametros["idDiferencia"]));
                        if (parametros.ContainsKey("tipoPantalla"))
                        {
                            if (parametros["tipoPantalla"] == "C")
                            {
                                habilitaCampos(false);
                                lbl_titulo.Text = "Consulta Diferencia";
                                btnGuardar.Visible = false;

                            }
                            else
                            {
                                hdfId.Value = parametros["idDiferencia"];
                                lbl_titulo.Text = "Edición de Diferencia";
                                btnGuardar.Visible = true;

                            }
                        }
                    }
                }
                else
                {
                    llenarEjercicio();
                    llenarStatus();
                    llenarBimestres();            
                }
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        private void llenarBimestres()
        {

            Dictionary<string, string> bimestre = new Dictionary<string, string>() { };

            for (int i = 1; i <= 6; i++)
            {

                bimestre.Add(i.ToString(), i.ToString());
            }

            var bimestres = bimestre.Select(x => new { Id = x.Key, Nombre = x.Value });

            ddlAvaluoBI.Items.Clear();
            ddlAvaluoBI.DataValueField = "Id";
            ddlAvaluoBI.DataTextField = "Nombre";
            ddlAvaluoBI.DataSource = bimestres;
            ddlAvaluoBI.DataBind();
            ddlAvaluoBI.Items.Insert(0, new ListItem("Bim", "%"));

            ddlAvaluoBF.Items.Clear();
            ddlAvaluoBF.DataValueField = "Id";
            ddlAvaluoBF.DataTextField = "Nombre";
            ddlAvaluoBF.DataSource = bimestres;
            ddlAvaluoBF.DataBind();
            ddlAvaluoBF.Items.Insert(0, new ListItem("Bim", "%"));

            ddlTDominioBI.Items.Clear();
            ddlTDominioBI.DataValueField = "Id";
            ddlTDominioBI.DataTextField = "Nombre";
            ddlTDominioBI.DataSource = bimestres;
            ddlTDominioBI.DataBind();
            ddlTDominioBI.Items.Insert(0, new ListItem("Bim", "%"));

            ddlTDominioBF.Items.Clear();
            ddlTDominioBF.DataValueField = "Id";
            ddlTDominioBF.DataTextField = "Nombre";
            ddlTDominioBF.DataSource = bimestres;
            ddlTDominioBF.DataBind();
            ddlTDominioBF.Items.Insert(0, new ListItem("Bim", "%"));

            ddlConstruccionBI.Items.Clear();
            ddlConstruccionBI.DataValueField = "Id";
            ddlConstruccionBI.DataTextField = "Nombre";
            ddlConstruccionBI.DataSource = bimestres;
            ddlConstruccionBI.DataBind();
            ddlConstruccionBI.Items.Insert(0, new ListItem("Bim", "%"));

            ddlConstruccionBF.Items.Clear();
            ddlConstruccionBF.DataValueField = "Id";
            ddlConstruccionBF.DataTextField = "Nombre";
            ddlConstruccionBF.DataSource = bimestres;
            ddlConstruccionBF.DataBind();
            ddlConstruccionBF.Items.Insert(0, new ListItem("Bim", "%"));


        }

        private void llenarStatus()
        {
            ddlStatus.Items.Clear();
            ddlStatus.DataValueField = "Id";
            ddlStatus.DataTextField = "Nombre";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new ListItem("Seleccionar Status", "%"));
            ddlStatus.Items.Insert(1, new ListItem("Activa", "A"));
            ddlStatus.Items.Insert(2, new ListItem("Pagado", "P"));
            ddlStatus.Items.Insert(3, new ListItem("Cancelado", "C"));

        }

        private void llenarEjercicio()
        {

            int EjercicioAnticipado = Convert.ToInt32(new cParametroCobroBL().GetByClave("EjercicioAnticipado"));
            int anioF = DateTime.Now.Year;
            int anioFd = DateTime.Now.Year;

            if (EjercicioAnticipado > DateTime.Now.Year)
            {
                anioF = EjercicioAnticipado;
                anioFd = EjercicioAnticipado;
            }

            Dictionary<string, string> anio = new Dictionary<string, string>() { };
            Dictionary<string, string> aniod = new Dictionary<string, string>() { };

            for (int y = 10; 0 <= y; y--)
            {
                int anioI = anioF - y;
                anio.Add(anioI.ToString(), anioI.ToString());
            }
            

            var anios = anio.Select(x => new { Id = x.Key, Nombre = x.Value });
            //var aniosd = aniod.Select(x => new { Id = x.Key, Nombre = x.Value });

            ddlAvaluoEI.Items.Clear();
            ddlAvaluoEI.DataValueField = "Id";
            ddlAvaluoEI.DataTextField = "Nombre";
            ddlAvaluoEI.DataSource = anios;
            ddlAvaluoEI.DataBind();
            ddlAvaluoEI.Items.Insert(0, new ListItem("Ejercicio", "%"));

            ddlAvaluoEF.Items.Clear();
            ddlAvaluoEF.DataValueField = "Id";
            ddlAvaluoEF.DataTextField = "Nombre";
            ddlAvaluoEF.DataSource = anios;
            ddlAvaluoEF.DataBind();
            ddlAvaluoEF.Items.Insert(0, new ListItem("Ejercicio", "%"));

            ddlTDominioEI.Items.Clear();
            ddlTDominioEI.DataValueField = "Id";
            ddlTDominioEI.DataTextField = "Nombre";
            ddlTDominioEI.DataSource = anios; //aniosd;
            ddlTDominioEI.DataBind();
            ddlTDominioEI.Items.Insert(0, new ListItem("Ejercicio", "%"));

            ddlTDominioEF.Items.Clear();
            ddlTDominioEF.DataValueField = "Id";
            ddlTDominioEF.DataTextField = "Nombre";
            ddlTDominioEF.DataSource = anios; //aniosd;
            ddlTDominioEF.DataBind();
            ddlTDominioEF.Items.Insert(0, new ListItem("Ejercicio", "%"));

            ddlConstruccionEI.Items.Clear();
            ddlConstruccionEI.DataValueField = "Id";
            ddlConstruccionEI.DataTextField = "Nombre";
            ddlConstruccionEI.DataSource = anios;
            ddlConstruccionEI.DataBind();
            ddlConstruccionEI.Items.Insert(0, new ListItem("Ejercicio", "%"));

            ddlConstruccionEF.Items.Clear();
            ddlConstruccionEF.DataValueField = "Id";
            ddlConstruccionEF.DataTextField = "Nombre";
            ddlConstruccionEF.DataSource = anios;
            ddlConstruccionEF.DataBind();
            ddlConstruccionEF.Items.Insert(0, new ListItem("Ejercicio", "%"));

        }

        protected void buscarClaveCatastral(object sender, System.EventArgs e)
        {
            //buscaCLaveCatastral();
            //llenaGrid(txtClvCastatral.Text.Trim());
        }

        private void llenaGrid(string clavecatastral)
        {
            int idpredio = new cPredioBL().GetByClavePredial(clavecatastral).Id;
            List<cBaseGravable> lbg = new cBaseGravableBL().GetListByIdPredio(idpredio);
            grdcreditos.DataSource = sorting(lbg);
            grdcreditos.DataBind();
        }

        private DataTable sorting(List<cBaseGravable> lBg)
        {
            i = new Impuesto();

            DataTable datos = new DataTable("DTSorting");
            datos.Columns.Add("Fecha");
            datos.Columns.Add("UsuarioMod");
            datos.Columns.Add("Ejercicio");
            datos.Columns.Add("ValorTerreno");
            datos.Columns.Add("ValorConstruccion");
            datos.Columns.Add("ValorPredio");
            datos.Columns.Add("CreditoFiscal");
            datos.Columns.Add("ImpuestoBimestral");

            if (lBg != null)
            {
                //int c = 1;
                foreach (cBaseGravable bg in lBg)
                {
                    double restante = 0;
                    double res = 0;
                    DataRow workRow = datos.NewRow();
                    workRow[0] = bg.FechaModificacion;
                    workRow[1] = "Migración de Sistema";
                    if(bg.IdUsuario!=1)
                        workRow[1] = new cUsuariosBL().GetByConstraint(bg.IdUsuario).Usuario;
                    workRow[2] = bg.Ejercicio;
                    workRow[3] = bg.ValorTerreno;
                    workRow[4] = bg.ValorConstruccion;
                    workRow[5] = bg.Valor;
                    if (bg.Valor <= 70000)
                    {
                        workRow[6] = 140;
                        workRow[7] = 23.33;
                    }
                    else
                    {
                        restante = Convert.ToDouble(bg.Valor) - 70000;
                        res = restante * .003;
                        workRow[6] = Utileria.Redondeo(res + 140);
                        workRow[7]= Utileria.Redondeo((res+140)/6);
                    }
                    //c = c + 1;
                    datos.Rows.Add(workRow);
                }

                if (ViewState["sortCampo"] != null && ViewState["sortOnden"] != null)
                {
                    if (ViewState["sortCampo"].ToString() != String.Empty && ViewState["sortOnden"].ToString() != String.Empty)
                    {
                        datos.DefaultView.Sort = string.Format("{0} {1}", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                    }
                }
            }
            return datos;
        }

        private void buscaCLaveCatastral()
        {
            if (txtClvCastatral.Text.Length == 12)
            {
               
                cPredio Predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);
                if (Predio == null)
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.PredioInexistente), ModalPopupMensaje.TypeMesssage.Alert);
                    txtClvCastatral.Text = "";
                }
                else
                {
                    cDiferencia dif = new cDiferenciaBL().GetByClaveCatastral(Predio.Id);
                    if (dif != null)
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription("El predio ya tiene diferencias registradas"), ModalPopupMensaje.TypeMesssage.Alert);
                        txtClvCastatral.Text = "";
                        return;
                    }

                    Label1.Visible = true;
                    txtUltPerPag.Visible = true;
                    txtUltPerPag.Text = Predio.BimestreFinIp.ToString() + " - " + Predio.AaFinalIp.ToString();
                    if (Predio.ClavePredial.Substring(0, 1) == "0") //solo si se refiere a una cuenta, la clave empieza con 0
                    {                      
                        lblCuentaPredial.Visible = true;                      
                        lblCuentaPredial.Text = "Cuenta Predial:   "+ Predio.ClaveAnterior.Substring(0, 4) + "-" + Predio.ClaveAnterior.Substring(4, 2) + "-" + Predio.ClaveAnterior.Substring(6, 3) + "-" + Predio.ClaveAnterior.Substring(9, 3);
                    }
                    cDiferencia diferencias = Predio.cDiferencia.Where(x => x.IdPredio == Predio.Id && x.Status == "A").FirstOrDefault();
                    if (diferencias!=null)
                    {
                        //string mensaje = "Imposible capturar la Diferencia ya existe una diferencia activa para la clave catastral Ingresada.";
                        //vtnModal.ShowPopup(new Utileria().GetDescription(mensaje), ModalPopupMensaje.TypeMesssage.Alert);
                        //txtClvCastatral.Text = "";
                        llenaConfiguracion(diferencias.Id);
                    }
                }
            }
            else
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.FormatoPredio), ModalPopupMensaje.TypeMesssage.Alert);
                txtClvCastatral.Text = "";
            }

        }

        private void limpiaCampos()
        {
            llenarStatus();
            llenarBimestres();
            llenarEjercicio();
            txtClvCastatral.Text = "";
            txtAvaluoDiferencia.Text = "";
            txtTDominioDiferencia.Text = "";
            txtConstruccionDiferencia.Text = "";
            txtFecha.Text = "";
            ViewState["idMod"] = null;
        }

        protected Dictionary<string, string> validadorGuardar()
        {
            Dictionary<string, string> respuesta = new Dictionary<string, string>() { };
            string isGuardado = "true";
            string mensaje = "continuar";

            if (txtAvaluoDiferencia.Text == "" && txtConstruccionDiferencia.Text == "" && txtTDominioDiferencia.Text == ""){
                isGuardado="false"; 
                mensaje= "Debe de Agregar por lo menos un concepto";
                respuesta.Add(isGuardado, mensaje);
            }

            if (txtAvaluoDiferencia.Text == "0.00")
                txtAvaluoDiferencia.Text = "";
            if (txtAvaluoDiferencia.Text != "")
            {
                if (ddlAvaluoBI.SelectedValue == "%" || ddlAvaluoBF.SelectedValue == "%" || ddlAvaluoEF.SelectedValue == "%" || ddlAvaluoEI.SelectedValue == "%")
                {
                    isGuardado = "false";
                    mensaje = "Debe de Agregar el Periodo de Vigencia del Concepto de avalúo";
                    respuesta.Add(isGuardado, mensaje);
                }
                else
                {
                    if (Convert.ToInt32(ddlAvaluoEI.SelectedValue) == Convert.ToInt32(ddlAvaluoEF.SelectedValue))
                    {
                        mensaje = validaPeriodoVigencia(Convert.ToInt32(ddlAvaluoBI.SelectedValue), Convert.ToInt32(ddlAvaluoEI.SelectedValue),
                                   Convert.ToInt32(ddlAvaluoBF.SelectedValue), Convert.ToInt32(ddlAvaluoEF.SelectedValue));
                    }
                    else if (Convert.ToInt32(ddlAvaluoEI.SelectedValue) > Convert.ToInt32(ddlAvaluoEF.SelectedValue))
                    {
                        mensaje = "El Periodo Final no puede ser menor que el Inicial";
                    }
                    else
                    {
                        mensaje = "true";
                    }
                    if (mensaje != "true")
                    {
                        isGuardado = "false";
                        mensaje = mensaje + " del Concepto de avalúo";
                        respuesta.Add(isGuardado, mensaje);
                    }
                }
            }
            if (txtConstruccionDiferencia.Text == "0.00")
                txtConstruccionDiferencia.Text = "";
            if (txtConstruccionDiferencia.Text != "")
            {
                if (ddlConstruccionBF.SelectedValue == "%" || ddlConstruccionBI.SelectedValue == "%" || ddlConstruccionEF.SelectedValue == "%" || ddlConstruccionEI.SelectedValue == "%")
                {
                    isGuardado = "false";
                    mensaje = "Debe de Agregar el Periodo de Vigencia del Concepto de Construcción";
                    respuesta.Add(isGuardado, mensaje);
                }
                else
                {
                    if (Convert.ToInt32(ddlConstruccionEI.SelectedValue) == Convert.ToInt32(ddlConstruccionEF.SelectedValue))
                    {
                        mensaje = validaPeriodoVigencia(Convert.ToInt32(ddlConstruccionBI.SelectedValue), Convert.ToInt32(ddlConstruccionEF.SelectedValue),
                                  Convert.ToInt32(ddlConstruccionBF.SelectedValue), Convert.ToInt32(ddlConstruccionEF.SelectedValue));
                    }
                    else if (Convert.ToInt32(ddlConstruccionEI.SelectedValue) > Convert.ToInt32(ddlConstruccionEF.SelectedValue))
                    {
                        mensaje = "El Periodo Final no puede ser menor que el Inicial";
                    }
                    else
                    {
                        mensaje = "true";
                    }
                     if (mensaje != "true")
                     {
                         isGuardado = "false";
                         mensaje = mensaje + " del Concepto de Construcción";
                         respuesta.Add(isGuardado, mensaje);
                    }
                
                }
            }
            if (txtTDominioDiferencia.Text == "0.00")
                txtTDominioDiferencia.Text = "";
            if (txtTDominioDiferencia.Text != "")
            {
                if (ddlTDominioBF.SelectedValue == "%" || ddlTDominioBI.SelectedValue == "%" || ddlTDominioEF.SelectedValue == "%" || ddlTDominioEI.SelectedValue == "%")
                {
                    isGuardado = "false";
                    mensaje = "Debe de Agregar el Periodo de Vigencia del Concepto de Tdo. de Dominio";
                    respuesta.Add(isGuardado, mensaje);
                }
                else
                {
                    if (Convert.ToInt32(ddlTDominioEI.SelectedValue) == Convert.ToInt32(ddlTDominioEF.SelectedValue))
                    {
                        mensaje = validaPeriodoVigencia(Convert.ToInt32(ddlTDominioBI.SelectedValue), Convert.ToInt32(ddlTDominioEI.SelectedValue),
                                 Convert.ToInt32(ddlTDominioBF.SelectedValue), Convert.ToInt32(ddlTDominioEF.SelectedValue));
                    }
                    else if (Convert.ToInt32(ddlTDominioEI.SelectedValue) > Convert.ToInt32(ddlTDominioEF.SelectedValue))
                    {
                            mensaje = "El Periodo Final no puede ser menor que el Inicial";
                    }
                    else
                    {
                        mensaje = "true";
                    }
                    if (mensaje != "true")
                    {
                        isGuardado = "false";
                        mensaje = mensaje + " del Concepto de Tdo. de Dominio";
                        respuesta.Add(isGuardado, mensaje);
                    }
                    
                }
            }
           
            respuesta.Add(isGuardado, mensaje);

            return respuesta;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            
            Dictionary<string, string> respuesta = validadorGuardar();
            String res= respuesta.Select(x=>x.Key).FirstOrDefault();
            if (res.Equals( "false"))
            {
                String mensaje = respuesta.Select(x => x.Value).FirstOrDefault();
                vtnModal.ShowPopup(new Utileria().GetDescription(mensaje), ModalPopupMensaje.TypeMesssage.Alert);                
            }
            else
            {
                cUsuarios U = new cUsuarios();
                U = (cUsuarios)Session["usuario"];

                cDiferencia Diferencia = new cDiferencia();
                MensajesInterfaz msg = new MensajesInterfaz();
                if (!(hdfId.Value == string.Empty || hdfId.Value == "0"))
                {
                    Diferencia = new cDiferenciaBL().GetByConstraint(Convert.ToInt32(hdfId.Value));
                }

                cPredio Predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);
                Diferencia.IdPredio = Predio.Id;
                if(txtAvaluoDiferencia.Text != "")   Diferencia.Avaluo = Convert.ToDecimal(txtAvaluoDiferencia.Text);
                if(ddlAvaluoBF.SelectedValue != "%") Diferencia.AvaluoBFinal =  Convert.ToInt16(ddlAvaluoBF.SelectedValue);
                if(ddlAvaluoBI.SelectedValue != "%") Diferencia.AvaluoBInicial = Convert.ToInt16(ddlAvaluoBI.SelectedValue) ;
                if(ddlAvaluoEF.SelectedValue != "%") Diferencia.AvaluoEjercicioFinal =  Convert.ToInt32(ddlAvaluoEF.SelectedValue);
                if(ddlAvaluoEI.SelectedValue != "%") Diferencia.AvaluoEjercicioInicial = Convert.ToInt32(ddlAvaluoEI.SelectedValue);

                if(txtConstruccionDiferencia.Text != "")   Diferencia.Construccion = Convert.ToDecimal(txtConstruccionDiferencia.Text);
                if(ddlConstruccionBF.SelectedValue != "%") Diferencia.ConstruccionBFinal = Convert.ToInt32(ddlConstruccionBF.SelectedValue);
                if(ddlConstruccionBI.SelectedValue != "%") Diferencia.ConstruccionBInicial = Convert.ToInt32(ddlConstruccionBI.SelectedValue);
                if(ddlConstruccionEF.SelectedValue != "%") Diferencia.ConstruccionEjercicioFinal = Convert.ToInt32(ddlConstruccionEF.SelectedValue) ;
                if(ddlConstruccionEI.SelectedValue != "%") Diferencia.ConstruccionEjercicioInicial = Convert.ToInt32(ddlConstruccionEI.SelectedValue);

                if(txtTDominioDiferencia.Text != "")   Diferencia.Traslado = Convert.ToDecimal(txtTDominioDiferencia.Text);
                if(ddlTDominioBF.SelectedValue != "%") Diferencia.TrasladoBFinal = Convert.ToInt32(ddlTDominioBF.SelectedValue) ;
                if(ddlTDominioBI.SelectedValue != "%") Diferencia.TrasladoBInicial =  Convert.ToInt32(ddlTDominioBI.SelectedValue);
                if(ddlTDominioEF.SelectedValue != "%") Diferencia.TrasladoEjercicioFinal = Convert.ToInt32(ddlTDominioEF.SelectedValue);
                if(ddlTDominioEI.SelectedValue != "%") Diferencia.TrasladoEjercicioInicial = Convert.ToInt32(ddlTDominioEI.SelectedValue) ;
                Diferencia.FechaAplicacion = Convert.ToDateTime(txtFecha.Text);
                Diferencia.Status = Convert.ToString(ddlStatus.SelectedValue);
                Diferencia.IdUsuario = U.Id;
                Diferencia.Activo = true;
                Diferencia.FechaModificacion = DateTime.Now;

                if (hdfId.Value == string.Empty || hdfId.Value == "0")
                {
                    msg = new cDiferenciaBL().Insert(Diferencia);
                }
                else
                {
                    msg = new cDiferenciaBL().Update(Diferencia);
                }

                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Confirm);

                limpiaCampos();
            }
        }

        private void llenaConfiguracion(int Id)
        {
            cDiferencia Diferencia = new cDiferenciaBL().GetByConstraint(Id);
            cPredio predio = new cPredioBL().GetByConstraint(Diferencia.IdPredio);
            Label1.Visible = true;
            txtUltPerPag.Visible = true;
            txtUltPerPag.Text = predio.BimestreFinIp.ToString() + " - " + predio.AaFinalIp.ToString();
            llenarEjercicio();
            llenarStatus();
            llenarBimestres();
            txtClvCastatral.Text = Diferencia.cPredio.ClavePredial;
            if(Diferencia.Avaluo !=null)
                if (Diferencia.Avaluo != 0)
                    txtAvaluoDiferencia.Text = Diferencia.Avaluo.ToString();
            if (Diferencia.AvaluoBInicial != null)
                if (Diferencia.AvaluoBInicial != 0)
                    ddlAvaluoBI.SelectedValue = Diferencia.AvaluoBInicial.ToString();
            if (Diferencia.AvaluoEjercicioInicial != null)
                if (Diferencia.AvaluoEjercicioInicial != 0)
                    ddlAvaluoEI.SelectedValue = Diferencia.AvaluoEjercicioInicial.ToString();
            if (Diferencia.AvaluoBFinal != null)
                if (Diferencia.AvaluoBFinal != 0)
                    ddlAvaluoBF.Text = Diferencia.AvaluoBFinal.ToString();
            if (Diferencia.AvaluoEjercicioFinal != null)
                if (Diferencia.AvaluoEjercicioFinal != 0)
                    ddlAvaluoEF.SelectedValue = Diferencia.AvaluoEjercicioFinal.ToString();
            if (Diferencia.Traslado != null)
                if (Diferencia.Traslado != 0)
                    txtTDominioDiferencia.Text = Diferencia.Traslado.ToString();
            if (Diferencia.TrasladoBInicial != null)
                if (Diferencia.TrasladoBInicial != 0)
                    ddlTDominioBI.SelectedValue = Diferencia.TrasladoBInicial.ToString();
            if (Diferencia.TrasladoEjercicioInicial != null)
                if (Diferencia.TrasladoEjercicioInicial != 0)
                ddlTDominioEI.SelectedValue = Diferencia.TrasladoEjercicioInicial.ToString();
            if (Diferencia.TrasladoBFinal != null)
                if (Diferencia.TrasladoBFinal!= 0)
                    ddlTDominioBF.SelectedValue = Diferencia.TrasladoBFinal.ToString();
            if (Diferencia.TrasladoEjercicioFinal != null)
                if (Diferencia.TrasladoEjercicioFinal != 0)
                    ddlTDominioEF.SelectedValue = Diferencia.TrasladoEjercicioFinal.ToString();
            if (Diferencia.Construccion != null)
                if (Diferencia.Construccion != 0)
                    txtConstruccionDiferencia.Text = Diferencia.Construccion.ToString();
            if (Diferencia.ConstruccionBInicial != null)
                if (Diferencia.ConstruccionBInicial != 0)
                    ddlConstruccionBI.SelectedValue = Diferencia.ConstruccionBInicial.ToString();
            if (Diferencia.ConstruccionEjercicioInicial != null)
                if (Diferencia.ConstruccionEjercicioInicial != 0)
                    ddlConstruccionEI.SelectedValue = Diferencia.ConstruccionEjercicioInicial.ToString();
            if (Diferencia.ConstruccionBFinal != null)
                if (Diferencia.ConstruccionBFinal != 0)
                    ddlConstruccionBF.SelectedValue = Diferencia.ConstruccionBFinal.ToString();
            if (Diferencia.ConstruccionEjercicioFinal != null)
                if (Diferencia.ConstruccionEjercicioFinal != 0)
                    ddlConstruccionEF.SelectedValue = Diferencia.ConstruccionEjercicioFinal.ToString();
            txtFecha.Text = Diferencia.FechaAplicacion.ToString("dd/MM/yyyyy");
            ddlStatus.SelectedValue = Diferencia.Status;            
        }

        private void habilitaCampos(bool activa)
        {
            txtClvCastatral.Enabled = activa;
            txtAvaluoDiferencia.Enabled = activa;
            ddlAvaluoBI.Enabled = activa;
            ddlAvaluoEI.Enabled = activa;
            ddlAvaluoBF.Enabled = activa;
            ddlAvaluoEF.Enabled = activa;
            txtTDominioDiferencia.Enabled = activa;
            ddlTDominioBI.Enabled = activa;
            ddlTDominioEI.Enabled = activa;
            ddlTDominioBF.Enabled = activa;
            ddlTDominioEF.Enabled = activa;
            txtConstruccionDiferencia.Enabled = activa;
            ddlConstruccionBI.Enabled = activa;
            ddlConstruccionEI.Enabled = activa;
            ddlConstruccionBF.Enabled = activa;
            ddlConstruccionEF.Enabled = activa;
            txtFecha.Enabled = activa;            
            ddlStatus.Enabled = activa;
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Ingreso) ||
                vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Actualizacion) ||
                vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.RegresarMSG))
            {
                //Session["parametro"] = null;
                Response.Redirect("BusquedaDiferencia.aspx");
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Session["parametro"] = null;
            //Response.Redirect("BusquedaDiferencia.aspx");
            Dictionary<string, string> parametros = (Dictionary<string, string>)Session["parametro"];
            if (parametros != null)
            {
                if (parametros.ContainsKey("tipoPantalla"))
                {
                    if (parametros["tipoPantalla"] == "M")
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.RegresarMSG), ModalPopupMensaje.TypeMesssage.Confirm);
                    }
                    else
                    {
                       //Session["parametro"] = null;
                        Response.Redirect("BusquedaDiferencia.aspx");
                    }
                }
            }
            else
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.RegresarMSG), ModalPopupMensaje.TypeMesssage.Confirm);
            }
        }

        protected String validaPeriodoVigencia(Int32 BI, Int32 EI, Int32 BF, Int32 EF) {
            String mensaje = "true";
            if (EI == EF)
            {
                if (BF < BI)
                    mensaje = "El Bimestre Final no Puede ser menor que el Inical";
            }
            else if (EF < EI)
                mensaje = "El Periodo Final no puede ser menor que el Inicial";                

            return mensaje;
        }

        protected void buscarClaveCatastral(object sender, ImageClickEventArgs e)
        {
            buscaCLaveCatastral();
            if (txtClvCastatral.Text != "")
                llenaGrid(txtClvCastatral.Text.Trim());
        }

        protected void ValidadorImpuesto(object sender, System.EventArgs e)
        {
            decimal anual = 0;
            cPredio Predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);
            decimal ant = 0;
            decimal act = 0;
            ant = txtBaseAnt.Text == "" ? 0 : Convert.ToDecimal(txtBaseAnt.Text);
            act = txtBaseNew.Text == "" ? 0 : Convert.ToDecimal(txtBaseNew.Text);
            txtBaseAnt.Text = ant.ToString("N");
            txtBaseNew.Text = act.ToString("N");
            if (act >= ant)
            {
                txtBaseDif.Text = (act - ant).ToString("N");
                anual = CalculaImpuestoAnual(Predio.Id, DateTime.Now.Year, (act - ant));
                txtImpuesto.Text = anual.ToString("N");
                txtBimestre.Text = (anual / 6).ToString("N");
                txtBaseAnt.Text = ant.ToString("N");
                txtBaseNew.Text = act.ToString("N");
            }
        }

        protected decimal CalculaImpuestoAnual(int idPredio,int ejercicio, decimal diferencia )
        {
            decimal imp = 0;
            decimal bi = 0, bg=0, sm=0 ;
            string descto = string.Empty, error = string.Empty;
            SaldosC s = new SaldosC();
            //i = s.CalculaCobro(idPredio, "NO", 1, ejercicio, 6, ejercicio, 0, 0, "CalculaPredial");
            bi = new cBaseImpuestoBL().GetByEjercicio(ejercicio);
            //bg = new cBaseGravableBL().GetByBasePredEjercicio(idPredio,  ejercicio);
            sm = new cSalarioMinimoBL().GetSMbyEjercicio(ejercicio);
            cPredio predio = new cPredioBL().GetByConstraint(idPredio);

            imp = s.ImpuestoPorBimestreCuota(diferencia, bi, sm, predio.cTipoPredio.Id , ejercicio, ref error);

            if (error != "")
                imp = 0;
            else
                imp = imp * 6;

            return imp;
        }

    }
}