using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using Clases.BL;
using Clases.Utilerias;
using Catastro.Controles;
using Excel;
using System.IO;
using System.Data;
using System.Text;
using System.Transactions;

namespace Catastro.Catalogos
{
    public partial class catPrediosMasivo : System.Web.UI.Page
    {
        int validacarga = 0;
        //protected void Page_Init(object sender, EventArgs e)
        //{
        //    UpdatePanel up = (Page.Master.FindControl("UpdatePanel1")) as UpdatePanel;
        //    AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();
        //    trigger.ControlID = UploadButton.UniqueID;
        //    trigger.EventName = "Click";
        //    up.Triggers.Add(trigger);
        //    up.UpdateMode = UpdatePanelUpdateMode.Conditional;
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                trcarga.Visible = false;
                trgrid.Visible = false;
            }
            //UpdatePanel updatePanel = (UpdatePanel)Page.Master.FindControl("UpdatePanel1");
            //UpdatePanelControlTrigger trigger = new PostBackTrigger();
            //////UpdatePanelControlTrigger trigger = new AsyncPostBackTrigger();
            //trigger.ControlID = UploadButton.UniqueID;
            //updatePanel.Triggers.Add(trigger);
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }
        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Ingreso))
            {
                Response.Redirect("~/Catalogos/catPrediosMasivo", false);
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Actualizacion))
            {
                Response.Redirect("~/Catalogos/catPrediosMasivo", false);
            }

        }
        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            if (txtClavePredial.Text != string.Empty)
            {
                cPredio predio = new cPredioBL().GetByClavePredial(txtClavePredial.Text);
                if (predio != null)
                {
                    ViewState["IdPredio"] = txtClavePredial.Text;
                    txtPropietario.Text = predio.cContribuyente.ApellidoPaterno + ' ' + predio.cContribuyente.ApellidoMaterno + ' ' + predio.cContribuyente.Nombre;
                    txtUbicacion.Text = predio.Calle + ' ' + predio.Numero + ' ' + predio.cColonia.NombreColonia + ' ' + predio.Localidad;
                    trcarga.Visible = true;
                    trgrid.Visible = false;
                }
                else
                {
                    txtPropietario.Text = "";
                    txtUbicacion.Text = "";
                    trcarga.Visible = false;
                    trgrid.Visible = false;
                }
            }
            else
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.PredioInexistente), ModalPopupMensaje.TypeMesssage.Alert);
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (fuExcel.HasFile)
            {
                try
                {
                    string[] strExtencsionImagen = new string[] { ".xls", ".xlsx" };
                    string fileExtension = System.IO.Path.GetExtension(fuExcel.FileName).ToLower();

                    if (strExtencsionImagen.Contains(fileExtension))
                    {
                        string path = Server.MapPath("~/Temporales/");//El folder debe estar ya creado

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        string nombrePath = path + fuExcel.FileName + DateTime.Now.Millisecond.ToString();
                        fuExcel.SaveAs(nombrePath);

                        FileStream stream = File.Open(nombrePath, FileMode.Open, FileAccess.Read);
                        DataSet result;

                        if (fileExtension == ".xls")
                        {
                            //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                            IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                            excelReader.IsFirstRowAsColumnNames = true;
                            result = excelReader.AsDataSet();
                            excelReader.Close();
                        }
                        else
                        {
                            //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                            excelReader.IsFirstRowAsColumnNames = true;
                            result = excelReader.AsDataSet();
                            excelReader.Close();
                        }

                        DataTable dt = result.Tables[0];
                        StringBuilder fallas = new StringBuilder();
                        StringBuilder fallasRecorrido = new StringBuilder();

                        double validavalores = 0;
                        int validarecibo = 0;
                        int fila = 0;
                        if (validacarga == 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                validavalores = 0;
                                fila = fila + 1;
                                string clavecatastral = "";
                                clavecatastral = dr["ClaveCatastral"].ToString() == "" || dr["ClaveCatastral"].ToString() == null ? "" : dr["ClaveCatastral"].ToString();
                                if (clavecatastral.Length == 12)
                                {//VALIDA SI YA EXISTE LA CLAVE, si es plano se debe omitir 
                                    if (rblTipo.SelectedValue != "Planos")
                                    {
                                        if (new cPredioBL().GetByClavePredial(dr["ClaveCatastral"].ToString() == "" || dr["ClaveCatastral"].ToString() == null ? "" : dr["ClaveCatastral"].ToString().Trim()) != null)
                                        {
                                            fallasRecorrido.AppendLine("Fila: " + fila + ", La clave catastral: " + dr["ClaveCatastral"].ToString() + " ya existe;");
                                        }
                                    }
                                }
                                else
                                {
                                    fallasRecorrido.AppendLine("Fila: " + fila + ", La clave catastral: " + dr["ClaveCatastral"].ToString() + " debe contener 12 digitos;");
                                }
                                if (dr["Calle"].ToString().Length > 150)
                                {
                                    fallasRecorrido.AppendLine("Fila: " + fila + ", La calle de la clave catastral: " + dr["ClaveCatastral"].ToString() + ", contiene mas de 150 caracteres;");
                                }
                                if (dr["Numero"].ToString().Length > 20)
                                {
                                    fallasRecorrido.AppendLine("Fila: " + fila + ", El Numero del domicilio, de la clave catastral: " + dr["ClaveCatastral"].ToString() + ", contiene mas de 20 caracteres;");
                                }
                                if (!Double.TryParse(dr["SuperficieTerreno"].ToString(), out validavalores))
                                {
                                    fallasRecorrido.AppendLine("Fila: " + fila + ", La superficie terreno de la clave catastral: " + dr["ClaveCatastral"].ToString() + ", no es un numero valido;");
                                }
                                if (!Double.TryParse(dr["TerrenoPrivativo"].ToString(), out validavalores))
                                {
                                    fallasRecorrido.AppendLine("Fila: " + fila + ", El terreno privativo de la clave catastral: " + dr["ClaveCatastral"].ToString() + ", no es un numero valido;");
                                }
                                if (!Double.TryParse(dr["TerrenoComun"].ToString(), out validavalores))
                                {
                                    fallasRecorrido.AppendLine("Fila: " + fila + ", El terreno común de la clave catastral: " + dr["ClaveCatastral"].ToString() + ", no es un numero valido;");
                                }
                                if (!Double.TryParse(dr["ValorTerreno"].ToString(), out validavalores))
                                {
                                    fallasRecorrido.AppendLine("Fila: " + fila + ", El valor de terreno de la clave catastral: " + dr["ClaveCatastral"].ToString() + ", no es un numero valido;");
                                }
                                if (!Double.TryParse(dr["SuperficieConstruccion"].ToString(), out validavalores))
                                {
                                    fallasRecorrido.AppendLine("Fila: " + fila + ", La superficie de construcción de la clave catastral: " + dr["ClaveCatastral"].ToString() + ", no es un numero valido;");
                                }
                                if (!Double.TryParse(dr["ConstruccionPrivativa"].ToString(), out validavalores))
                                {
                                    fallasRecorrido.AppendLine("Fila: " + fila + ", La construcción privativa de la clave catastral: " + dr["ClaveCatastral"].ToString() + ", no es un numero valido;");
                                }
                                if (!Double.TryParse(dr["ConstruccionComun"].ToString(), out validavalores))
                                {
                                    fallasRecorrido.AppendLine("Fila: " + fila + ", La construcción común de la clave catastral: " + dr["ClaveCatastral"].ToString() + ", no es un numero valido;");
                                }
                                if (!Double.TryParse(dr["ValorConstruccion"].ToString(), out validavalores))
                                {
                                    fallasRecorrido.AppendLine("El valor de construcción de la clave catastral: " + dr["ClaveCatastral"].ToString() + ", no es un numero valido;");
                                }
                                //if (!Double.TryParse(dr["ValorConstruccionComun"].ToString(), out validavalores))
                                //{
                                //    fallasRecorrido.AppendLine("El valor de construcción común de la clave catastral: " + dr["ClaveCatastral"].ToString() + ", no es un numero valido;");
                                //}
                                //if (!Double.TryParse(dr["ValorConstruccionPrivativa"].ToString(), out validavalores))
                                //{
                                //    fallasRecorrido.AppendLine("El valor de construcción privativa de la clave catastral: " + dr["ClaveCatastral"].ToString() + ",no es un numero valido;");
                                //}
                                if (!Double.TryParse(dr["MetrosFrente"].ToString(), out validavalores))
                                {
                                    fallasRecorrido.AppendLine("Fila: " + fila + ", Los metros de frente de la clave catastral: " + dr["ClaveCatastral"].ToString() + ", no es un numero valido;");
                                }
                                if (!Int32.TryParse(dr["ReciboAlta"].ToString(), out validarecibo))
                                {
                                    fallasRecorrido.AppendLine("Fila: " + fila + ", El numero de recibo de alta, de la clave catastral: " + dr["ClaveCatastral"].ToString() + ", no es un numero valido;");
                                }
                                if (dr["Observaciones"].ToString().Length > 800)
                                {
                                    fallasRecorrido.AppendLine("Fila: " + fila + ", Las observaciones de la clave catastral: " + dr["ClaveCatastral"].ToString() + ", contiene mas de 800 caracteres;");
                                }
                                if (dr["Referencias"].ToString().Length > 800)
                                {
                                    fallasRecorrido.AppendLine("Fila: " + fila + ", Las observaciones de la clave catastral: " + dr["ClaveCatastral"].ToString() + ", contiene mas de 800 caracteres;");
                                }
                            }

                        }
                        else
                        {
                            vtnModal.ShowPopup(new Utileria().GetDescription("Revisar elarchiv de errores"), ModalPopupMensaje.TypeMesssage.Confirm);
                        }

                        if (fallasRecorrido.Length == 0)
                        {
                            trcarga.Visible = false;
                            trgrid.Visible = true;
                            btnTramitePlanos.Visible = false;
                            grdMasivo.DataSource = dt;
                            grdMasivo.DataBind();

                            if (rblTipo.SelectedValue == "Planos")
                            {
                                btnTramitePlanos.Visible = true;
                                btnGuardar.Visible = false;
                            }
                        }
                        else
                        {
                            Response.Clear();
                            Response.Buffer = true;
                            Response.ContentType = "text/plain";
                            Response.AppendHeader("content-disposition", "attachment;filename=Bitacora.txt");
                            Response.Write(fallasRecorrido.ToString());
                            Response.End();
                        }
                    }
                    else
                    {
                        vtnModal.DysplayCancelar = false;
                        vtnModal.ShowPopup("Por favor seleccione un archivo de excel.", ModalPopupMensaje.TypeMesssage.Confirm);
                    }
                }
                catch (Exception ex)
                {
                    vtnModal.DysplayCancelar = false;
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ErrorGeneral), ModalPopupMensaje.TypeMesssage.Confirm);
                    new Utileria().logError("cargar excel", ex, "");
                }
            }
            else
            {
                vtnModal.DysplayCancelar = false;
                vtnModal.ShowPopup("Por favor seleccione un archivo de excel.", ModalPopupMensaje.TypeMesssage.Confirm);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            cPredio pBase = new cPredioBL().GetByClavePredial(ViewState["IdPredio"].ToString());
            cBaseGravable bgBase = new cBaseGravableBL().GetUltByIdPredioAct(pBase.Id);
            cPredioBL predioBL = new cPredioBL();

            MensajesInterfaz msg = new MensajesInterfaz();
            DateTime fecha = DateTime.Now;
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (GridViewRow row in grdMasivo.Rows)
                {
                    string ClaveCatastral = string.Empty;
                    string Calle = string.Empty;
                    string Numero = string.Empty;

                    //double SuperficieTerreno = 0, TerrenoPrivativo = 0, TerrenoComun = 0, ValorTerreno = 0, SuperficieConstruccion = 0, ConstruccionPrivativa = 0, ConstruccionComun = 0, ValorConstruccion = 0, ValorConstruccionComun = 0, ValorConstruccionPrivativa = 0, MetrosFrente = 0;

                    cPredio predio = new cPredio();
                    predio.ClavePredial = row.Cells[0].Text == "" || row.Cells[0].Text == null ? "" : row.Cells[0].Text.Trim();
                    predio.Calle = Server.HtmlDecode(row.Cells[1].Text == "" || row.Cells[1].Text == null ? "" : row.Cells[1].Text.Trim()); //calle
                    predio.Numero = Server.HtmlDecode(row.Cells[2].Text == "" || row.Cells[2].Text == null ? "" : row.Cells[2].Text.Trim()); //;
                    predio.IdColonia = pBase.IdColonia;
                    predio.CP = pBase.CP;
                    predio.Localidad = pBase.Localidad;
                    predio.SuperficieTerreno = Convert.ToDouble(Server.HtmlDecode(row.Cells[3].Text == "" || row.Cells[3].Text == null ? "" : row.Cells[3].Text.Trim())); // SuperficieTerreno;
                    predio.TerrenoPrivativo = Convert.ToDouble(Server.HtmlDecode(row.Cells[4].Text == "" || row.Cells[4].Text == null ? "" : row.Cells[4].Text.Trim())); //TerrenoPrivativo;
                    predio.TerrenoComun = Convert.ToDouble(Server.HtmlDecode(row.Cells[5].Text == "" || row.Cells[5].Text == null ? "" : row.Cells[5].Text.Trim())); //TerrenoComun;
                    predio.ValorTerreno = Convert.ToDouble(Server.HtmlDecode(row.Cells[6].Text == "" || row.Cells[6].Text == null ? "" : row.Cells[6].Text.Trim())); //ValorTerreno;
                    predio.SuperficieConstruccion = Convert.ToDouble(Server.HtmlDecode(row.Cells[7].Text == "" || row.Cells[7] == null ? "" : row.Cells[7].Text.Trim())); //SuperficieConstruccion;
                    predio.ConstruccionPrivativa = Convert.ToDouble(Server.HtmlDecode(row.Cells[8].Text == "" || row.Cells[8].Text == null ? "" : row.Cells[8].Text.Trim())); //ConstruccionPrivativa;
                    predio.ConstruccionComun = Convert.ToDouble(Server.HtmlDecode(row.Cells[9].Text == "" || row.Cells[9].Text == null ? "" : row.Cells[9].Text.Trim())); //ConstruccionComun;
                    predio.ValorConstruccion = Convert.ToDouble(Server.HtmlDecode(row.Cells[10].Text == "" || row.Cells[10].Text == null ? "" : row.Cells[10].Text.Trim())); // ValorConstruccion;
                    predio.ValorCatastral = Convert.ToDouble(predio.ValorTerreno + predio.ValorConstruccion);
                    predio.FechaAlta = fecha;
                    predio.FechaAvaluo = fecha;
                    predio.FechaTraslado = fecha;
                    predio.Zona = pBase.Zona;
                    predio.MetrosFrente = Convert.ToDouble(Server.HtmlDecode(row.Cells[11].Text == "" || row.Cells[11].Text == null ? "" : row.Cells[11].Text.Trim())); //MetrosFrente;
                    predio.IdUsoSuelo = pBase.IdUsoSuelo;
                    predio.IdExentoPago = pBase.IdExentoPago;
                    predio.IdStatusPredio = pBase.IdStatusPredio;
                    //predio.FechaBaja = pBase.FechaBaja;
                    predio.IdTipoPredio = pBase.IdTipoPredio;
                    predio.IdContribuyente = pBase.IdContribuyente;
                    predio.IdTipoFaseIp = pBase.IdTipoFaseIp;
                    predio.Nivel = pBase.Nivel;
                    predio.UbicacionExpediente = pBase.UbicacionExpediente;
                    predio.BimestreFinIp = pBase.BimestreFinIp;
                    predio.AaFinalIp = pBase.AaFinalIp;
                    predio.IdTipoFaseSm = pBase.IdTipoFaseSm;
                    predio.BimestreFinSm = pBase.BimestreFinSm;
                    predio.AaFinalSm = pBase.AaFinalSm;
                    predio.IdTipoMovAvaluo = pBase.IdTipoMovAvaluo;
                    predio.IdCondominio = pBase.IdCondominio;
                    predio.IdCartografia = pBase.IdCartografia;
                    predio.Referencia = Server.HtmlDecode(row.Cells[14].Text == "" || row.Cells[14].Text == null ? "" : row.Cells[14].Text.Trim());
                    predio.IdUsuario = U.Id;
                    predio.FechaModificacion = fecha;
                    predio.Activo = true;

                    cBaseGravable BaseGravable = null;
                    if ((Convert.ToInt32(txtAaFinalIPV.Text) == DateTime.Now.Year - 1) && (ddlBimestreIP.SelectedValue == "6"))
                    {
                        BaseGravable = new cBaseGravable();
                        BaseGravable.Ejercicio = DateTime.Now.Year;
                        BaseGravable.Bimestre = bgBase.Bimestre;
                        BaseGravable.FechaAvaluo = fecha;
                        BaseGravable.SuperficieTerreno = Convert.ToDouble(Server.HtmlDecode(row.Cells[3].Text == "" || row.Cells[3].Text == null ? "" : row.Cells[3].Text.Trim())); // SuperficieTerreno;
                        BaseGravable.TerrenoPrivativo = Convert.ToDouble(Server.HtmlDecode(row.Cells[4].Text == "" || row.Cells[4].Text == null ? "" : row.Cells[4].Text.Trim())); //TerrenoPrivativo;
                        BaseGravable.TerrenoComun = Convert.ToDouble(Server.HtmlDecode(row.Cells[5].Text == "" || row.Cells[5].Text == null ? "" : row.Cells[5].Text.Trim())); //TerrenoComun;
                        BaseGravable.ValorTerreno = Convert.ToDouble(Server.HtmlDecode(row.Cells[6].Text == "" || row.Cells[6].Text == null ? "" : row.Cells[6].Text.Trim())); //ValorTerreno;
                        BaseGravable.SuperficieConstruccion = Convert.ToDouble(Server.HtmlDecode(row.Cells[7].Text == "" || row.Cells[7].Text == null ? "" : row.Cells[7].Text.Trim())); //SuperficieConstruccion;
                        BaseGravable.ConstruccionPrivativa = Convert.ToDouble(Server.HtmlDecode(row.Cells[8].Text == "" || row.Cells[8].Text == null ? "" : row.Cells[8].Text.Trim())); //ConstruccionPrivativa;
                        BaseGravable.ConstruccionComun = Convert.ToDouble(Server.HtmlDecode(row.Cells[9].Text == "" || row.Cells[9].Text == null ? "" : row.Cells[9].Text.Trim())); //ConstruccionComun;;
                        BaseGravable.ValorConstruccion = Convert.ToDouble(Server.HtmlDecode(row.Cells[10].Text == "" || row.Cells[10].Text == null ? "" : row.Cells[10].Text.Trim())); // ValorConstruccion;;
                        BaseGravable.Valor = Convert.ToDecimal(BaseGravable.ValorConstruccion + BaseGravable.ValorTerreno);
                        //BaseGravable.ValorConstruccionPrivativa = ValorConstruccionPrivativa;
                        //BaseGravable.ValorConstruccionComun = ValorConstruccionComun;
                        //BaseGravable.Prototipo = txtPrototipo.Text == "" ? null : (double?)Convert.ToDouble(txtPrototipo.Text);
                        BaseGravable.IdUsuario = U.Id;
                        BaseGravable.Activo = true;
                        BaseGravable.FechaModificacion = fecha;
                        predio.cBaseGravable.Add(BaseGravable);
                    }
                    else
                    {
                        for (int i = Convert.ToInt32(txtAaFinalIPV.Text); i <= DateTime.Now.Year; i++)
                        {
                            BaseGravable = new cBaseGravable();
                            BaseGravable.Ejercicio = i;
                            BaseGravable.Bimestre = 6;
                            BaseGravable.FechaAvaluo = fecha;
                            BaseGravable.SuperficieTerreno = Convert.ToDouble(Server.HtmlDecode(row.Cells[3].Text == "" || row.Cells[3].Text == null ? "" : row.Cells[3].Text.Trim())); // SuperficieTerreno;
                            BaseGravable.TerrenoPrivativo = Convert.ToDouble(Server.HtmlDecode(row.Cells[4].Text == "" || row.Cells[4].Text == null ? "" : row.Cells[4].Text.Trim())); //TerrenoPrivativo;
                            BaseGravable.TerrenoComun = Convert.ToDouble(Server.HtmlDecode(row.Cells[5].Text == "" || row.Cells[5].Text == null ? "" : row.Cells[5].Text.Trim())); //TerrenoComun;
                            BaseGravable.ValorTerreno = Convert.ToDouble(Server.HtmlDecode(row.Cells[6].Text == "" || row.Cells[6].Text == null ? "" : row.Cells[6].Text.Trim())); //ValorTerreno;
                            BaseGravable.SuperficieConstruccion = Convert.ToDouble(Server.HtmlDecode(row.Cells[7].Text == "" || row.Cells[7].Text == null ? "" : row.Cells[7].Text.Trim())); //SuperficieConstruccion;
                            BaseGravable.ConstruccionPrivativa = Convert.ToDouble(Server.HtmlDecode(row.Cells[8].Text == "" || row.Cells[8].Text == null ? "" : row.Cells[8].Text.Trim())); //ConstruccionPrivativa;
                            BaseGravable.ConstruccionComun = Convert.ToDouble(Server.HtmlDecode(row.Cells[9].Text == "" || row.Cells[9].Text == null ? "" : row.Cells[9].Text.Trim())); //ConstruccionComun;;
                            BaseGravable.ValorConstruccion = Convert.ToDouble(Server.HtmlDecode(row.Cells[10].Text == "" || row.Cells[10].Text == null ? "" : row.Cells[10].Text.Trim())); // ValorConstruccion;;
                            BaseGravable.Valor = Convert.ToDecimal(BaseGravable.ValorConstruccion + BaseGravable.ValorTerreno);
                            //BaseGravable.ValorConstruccionPrivativa = ValorConstruccionPrivativa;
                            //BaseGravable.ValorConstruccionComun = ValorConstruccionComun;
                            //BaseGravable.Prototipo = txtPrototipo.Text == "" ? null : (double?)Convert.ToDouble(txtPrototipo.Text);
                            BaseGravable.IdUsuario = U.Id;
                            BaseGravable.Activo = true;
                            BaseGravable.FechaModificacion = fecha;
                            predio.cBaseGravable.Add(BaseGravable);
                        }
                    }


                    cPredioObservacion observacion = new cPredioObservacion();
                    observacion.Observacion = Server.HtmlDecode(row.Cells[13].Text == "" || row.Cells[13].Text == null ? "" : row.Cells[13].Text.Trim());//observaciones
                    observacion.IdUsuario = U.Id;
                    observacion.Activo = true;
                    observacion.FechaModificacion = fecha;

                    predio.cPredioObservacion.Add(observacion);

                    msg = predioBL.Insert(predio);
                    if (msg != MensajesInterfaz.Ingreso)
                    {
                        vtnModal.DysplayCancelar = false;
                        vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Confirm);
                        break;
                    }
                    else
                    {
                        int recibo = Convert.ToInt32(Server.HtmlDecode(row.Cells[12].Text));//recibo
                        if (recibo != 0)
                        {
                            tReciboBL rBL = new tReciboBL();
                            tRecibo r = rBL.GetByConstraint(recibo);
                            r.tTramite.IdPredio = predio.Id;
                            msg = rBL.Update(r);
                        }
                    }
                }
                //FIN DE LA TRANSACCION 
                scope.Complete();

                vtnModal.DysplayCancelar = false;
                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Confirm);

            }
        }

        protected void rblTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblTipo.SelectedValue == "Predios")
            {
                tbPredios.Visible = true;
                tbCondominios.Visible = false;
                tbColonias.Visible = false;
                btnTramitePlanos.Visible = false;
            }
            else if (rblTipo.SelectedValue == "Planos")
            {
                tbPredios.Visible = true;
                tbCondominios.Visible = false;
                tbColonias.Visible = false;
                btnTramitePlanos.Visible = false;
            }
            else if (rblTipo.SelectedValue == "Condominios")
            {
                tbCondominios.Visible = true;
                tbPredios.Visible = false;
                tbColonias.Visible = false;
                ddlCondominio.DataValueField = "Id";
                ddlCondominio.DataTextField = "Descripcion";
                ddlCondominio.DataSource = new cCondominioBL().GetAll();
                ddlCondominio.DataBind();
                ddlCondominio.Items.Insert(0, new ListItem("Seleccionar Condominio", ""));
            }
            if (rblTipo.SelectedValue == "Colonias")
            {
                tbColonias.Visible = true;
                tbCondominios.Visible = false;
                tbPredios.Visible = false;
                ddlColonias.DataValueField = "Id";
                ddlColonias.DataTextField = "NombreColonia";
                ddlColonias.DataSource = new cColoniaBL().GetAll();
                ddlColonias.DataBind();
                ddlColonias.Items.Insert(0, new ListItem("Seleccionar Colonia", ""));
            }
        }

        public string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || c == ',')
                {
                    sb.Append(c);
                }
            }
            StringBuilder claves = new StringBuilder();
            bool bandera = false;
            foreach (string clave in sb.ToString().Split(','))
            {
                if (bandera == false)
                {
                    claves.Append("'" + clave + "'");
                    bandera = true;
                }
                else
                {
                    claves.Append(",'" + clave + "'");
                }

            }
            return claves.ToString();
        }

        protected void btnGuardarCondominio_Click(object sender, EventArgs e)
        {
            string claves = RemoveSpecialCharacters(txtMasivoCondominio.Text);
            int result = new pProcedimientos().actulizaMasivo("cCondominio", ddlCondominio.SelectedValue, claves);
            vtnModal.DysplayCancelar = false;
            vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.Actualizacion), ModalPopupMensaje.TypeMesssage.Confirm);
        }

        protected void btnGuardarColonias_Click(object sender, EventArgs e)
        {
            string claves = RemoveSpecialCharacters(txtMasivocolonias.Text);
            int result = new pProcedimientos().actulizaMasivo("cColonia", ddlColonias.SelectedValue, claves);
            vtnModal.DysplayCancelar = false;
            vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.Actualizacion), ModalPopupMensaje.TypeMesssage.Confirm);
        }

        protected void TramitePlanos_Click(object sender, EventArgs e)
        {
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];

            MensajesInterfaz msg = new MensajesInterfaz();
            DateTime fecha = DateTime.Now;
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (GridViewRow row in grdMasivo.Rows)
                {

                    //***************** plano                   
                    U = (cUsuarios)Session["usuario"];
                    tTramite Tramite = new tTramite();
                    tRecibo r = new tRecibo();
                    cPredio predio = new cPredioBL().GetByClavePredial(row.Cells[0].Text.Trim());
                    string validacionFechaAvaluo = new cParametroSistemaBL().GetValorByClave("VALIDACIONFECHAAVALUO");
                    if (validacionFechaAvaluo == "SI")
                    {
                        DateTime zeroTime = new DateTime(1, 1, 1);
                        TimeSpan span = DateTime.Now - predio.FechaAvaluo;
                        int years = (zeroTime + span).Year - 1;
                        if (years < 2)
                        {
                            MensajesInterfaz msgA = MensajesInterfaz.FechaAvaluoMenorDosAnios;
                            vtnModal.ShowPopup(new Utileria().GetDescription(msgA), ModalPopupMensaje.TypeMesssage.Alert);
                            return;
                        }
                    }
                    int recibo = Convert.ToInt32(Server.HtmlDecode(row.Cells[12].Text));//recibo
                    if (recibo != 0)
                    {
                        r = new tReciboBL().GetByConstraint(recibo);
                    }
                    else
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription("El recibo no esta registrado como pagado."), ModalPopupMensaje.TypeMesssage.Alert);
                        return;
                    }

                    Tramite.Fecha = DateTime.Now;
                    Tramite.IdPredio = predio.Id;
                    Tramite.IdTipoTramite = Convert.ToInt32(new cParametroSistemaBL().GetValorByClave("Plano"));
                    Tramite.Status = "P";
                    Tramite.IdUsuario = U.Id;
                    Tramite.Activo = true;
                    Tramite.FechaModificacion = DateTime.Now;
                    Tramite.FechaOperacion = DateTime.Now;
                    Tramite.SuperficieConstruccion = predio.SuperficieConstruccion;
                    Tramite.SuperficieTerreno = predio.SuperficieTerreno;
                    Tramite.Tipo = "IP";
                    Tramite.Observacion = "PLANO DERIVADO DE ALTA MASIVA DE CLAVE ORIGEN " + txtClavePredial.Text + ", RECIBO " + r.Id;
                    Tramite.Activo = true;
                    Tramite.IdUsuario = U.Id;
                    Tramite.FechaModificacion = DateTime.Now;

                    msg = new tTramiteBL().Insert(Tramite);
                    if (msg != MensajesInterfaz.Ingreso)
                    {
                        vtnModal.DysplayCancelar = false;
                        vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Confirm);
                        break;
                    }

                    cPredioObservacion observacion = new cPredioObservacion();
                    observacion.IdPredio = predio.Id;
                    observacion.Observacion = "PLANO DERIVADO DE ALTA MASIVA DE CLAVE ORIGEN " + txtClavePredial.Text + ", RECIBO " + r.Id; ;//observaciones
                    observacion.IdUsuario = U.Id;
                    observacion.Activo = true;
                    observacion.FechaModificacion = fecha;

                    msg = new cPredioObservacionBL().Insert(observacion);
                    if (msg != MensajesInterfaz.Ingreso)
                    {
                        vtnModal.DysplayCancelar = false;
                        vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Confirm);
                        break;
                    }



                }
                //FIN DE LA TRANSACCION 
                scope.Complete();

                vtnModal.DysplayCancelar = false;
                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Confirm);

            }

        }



    }
}
