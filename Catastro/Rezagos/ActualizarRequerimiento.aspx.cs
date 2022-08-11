using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Clases;
using Clases.BL;
using Clases.Utilerias;
using iTextSharp.text.pdf;
using System.IO;
using Catastro.Controles;
using System.Transactions;
using System.Globalization;
using System.Data.Entity.Infrastructure;
using System.Text;

namespace Catastro.Requerimientos
{
    public partial class ActualizarRequerimiento : System.Web.UI.Page
    {
        //req.Limpieza = Convert.ToDecimal(row.Cells[25].Text);
        //req.Recoleccion = Convert.ToDecimal(row.Cells[24].Text);
        //req.dap = Convert.ToDecimal(row.Cells[23].Text);

        protected int Calcula(String formOriginal)
        {            
            string Path = Server.MapPath("~/");
            Int32 bimcob = 0;
            string ruta = HttpUtility.HtmlDecode(txtClvCastatral.Text.Trim()) + "-" + DateTime.Now.Millisecond.ToString() + ".pdf";
            ViewState["pdf"] = ruta;
            string formImprimir = Path + "/Temporales/" + ruta;


            PdfReader reader = new PdfReader(formOriginal);
            PdfStamper stamper = new PdfStamper(reader, new FileStream(formImprimir, FileMode.Create));

            AcroFields fields = stamper.AcroFields;
            fields.SetField("Contribuyente", (HttpUtility.HtmlDecode(txtContribuyente.Text)));
            fields.SetField("Domicilio", (HttpUtility.HtmlDecode(txtUbicacion.Text)));
            fields.SetField("Folio", txtFolioOficio.Text);
            fields.SetField("ClaveCatastral",txtClvCastatral.Text);
            string clavens = txtClvCastatral.Text;
            string clave = clavens.Replace("-", "");
            cPredio predio = new cPredioBL().GetByClavePredial(clave);
            fields.SetField("SuperficieTerreno", predio.SuperficieTerreno.ToString());
            fields.SetField("ValorTerreno", Convert.ToDecimal(predio.ValorTerreno).ToString("C"));
            fields.SetField("SuperficieConstruccion", predio.SuperficieConstruccion.ToString());
            fields.SetField("ValorConstruccion", predio.ValorConstruccion.ToString("C"));
            fields.SetField("BaseGravable", predio.ValorCatastral.ToString("C"));
            decimal tasa = new cBaseImpuestoBL().GetByEjercicio(DateTime.Today.Year);
            fields.SetField("Tasa", tasa.ToString("C"));

            fields.SetField("Periodo","Del "+ txtPIIP.Text+" al "+txtPFIP.Text);
            fields.SetField("Impuesto", Convert.ToDecimal(txtImpuestoIP.Text) > 0 ? Convert.ToDecimal(txtImpuestoIP.Text).ToString("C") : "0");
            fields.SetField("Diferencia", Convert.ToDecimal(txtDiferenciasIP.Text) > 0 ? Convert.ToDecimal(txtDiferenciasIP.Text).ToString("C") : "0");
            fields.SetField("Rezago", Convert.ToDecimal(txtRezagoIP.Text) > 0 ? Convert.ToDecimal(txtRezagoIP.Text).ToString("C") : "0");
            fields.SetField("Adicional", Convert.ToDecimal(txtAdicionalesIP.Text) > 0 ? Convert.ToDecimal(txtAdicionalesIP.Text).ToString("C") : "0");
            Decimal recargo = Convert.ToDecimal(txtRecargoIP.Text) > 0 ? Convert.ToDecimal(txtRecargoIP.Text) :0;
            fields.SetField("Recargo", recargo.ToString("C"));
            fields.SetField("Multa", Convert.ToDecimal(txtMultaIP.Text) > 0 ?Convert.ToDecimal(txtMultaIP.Text).ToString("C") :"0");
            fields.SetField("Honorarios", Convert.ToDecimal(txtHonorarioIP.Text) > 0 ? Convert.ToDecimal(txtHonorarioIP.Text).ToString("C") : "0");//revisar
            fields.SetField("Ejecucion", Convert.ToDecimal(txtEjecucionIP.Text) > 0 ? Convert.ToDecimal(txtEjecucionIP.Text).ToString("C") : "0");//revisar

            fields.SetField("Subtotal", Convert.ToDecimal(txtImporteIP.Text).ToString("C"));

            //1 2016 - 6 2016       .
            MensajesInterfaz msg = new MensajesInterfaz();
            if (txtPIIP.Text != "" && txtPFIP.Text != "")
            {
                int bimI = Convert.ToInt32(txtPIIP.Text.Substring(0, 1));
                int aaI = Convert.ToInt32(txtPIIP.Text.Substring(4, 4));
                int bimF = Convert.ToInt32(txtPFIP.Text.Substring(0, 1));
                int aaF = Convert.ToInt32(txtPFIP.Text.Substring(4, 4));                
                bimcob = new SaldosC().CuentaBimestre(bimI, aaI, bimF, aaF, ref msg);
            }
            fields.SetField("BimApagar", Convert.ToString(bimcob));
            if (txtPISM.Text != "" && txtPFSM.Text != "" )
                 fields.SetField("PeriodoSM", "Del "+txtPISM+" al "+txtPFSM);
            fields.SetField("Infraestructura", txtImpuestoSM.Text != "" ? Convert.ToDecimal(txtImpuestoSM.Text).ToString("C"):"0");
            fields.SetField("Limpieza", txtLimpiezaSM.Text != "" ? Convert.ToDecimal(txtLimpiezaSM.Text).ToString("C") : "0");//grdEdoCta.DataKeys[row.RowIndex].Values["Limpieza"]).ToString("C"));
            fields.SetField("DAP", txtDAP.Text != "" ? Convert.ToDecimal(txtDAP.Text).ToString("C"): "0");
            fields.SetField("Recoleccion", txtRecoleccionSM.Text != "" ? Convert.ToDecimal(txtRecoleccionSM.Text).ToString("C") : "0");//Pendiente
            fields.SetField("RezagoSM", txtRezagoSM.Text != "" ? Convert.ToDecimal(txtRezagoSM.Text).ToString("C"): "0");
            fields.SetField("AdicionalSM", txtAdicionalesSM.Text != "" ? Convert.ToDecimal(txtAdicionalesSM.Text).ToString("C") : "0");
            fields.SetField("RecargoSM", txtRecargosSM.Text != "" ? Convert.ToDecimal(txtRecargosSM.Text).ToString("C") : "0");
            fields.SetField("HonorariosSM", txtHonorarioSM.Text != "" ? Convert.ToDecimal(txtHonorarioSM.Text).ToString("C") : "0");//revisar
            fields.SetField("EjecucionSM", txtEjecucionSM.Text != "" ? Convert.ToDecimal(txtEjecucionSM.Text).ToString("C"): "0");//revisar
            fields.SetField("MultaSM", txtMultaSM.Text != "" ? Convert.ToDecimal(txtMultaSM.Text).ToString("C") : "0");
                    //fields.SetField("SM", Convert.ToDecimal(0.0).ToString("C"));
                  
            fields.SetField("SubtotalSM", txtImporteSM.Text != "" ? Convert.ToDecimal(txtImporteSM.Text).ToString("C") :"0");
            fields.SetField("Total", (Convert.ToDecimal(txtImporteIP.Text != "" ? txtImporteIP.Text : "0") + Convert.ToDecimal(txtImporteSM.Text != "" ? txtImporteSM.Text : "0")).ToString("C") );
            string texto = new cParametroSistemaBL().GetValorByClave("NOMBRE_MUNICIPIO");
            fields.SetField("Fecha", texto + ", MORELOS, A " + (txtFechaEmision.Text).Substring(0, 2) + " DE " + Convert.ToDateTime(txtFechaEmision.Text).ToString("MMMM").ToUpper() + " DE " + (txtFechaEmision.Text).Substring(6, 4));
            fields.SetField("Titular", (HttpUtility.HtmlDecode(new cParametroSistemaBL().GetValorByClave("FIRMAREQUERIMIENTO")).ToString()));
            fields.SetField("Puesto", (HttpUtility.HtmlDecode(new cParametroSistemaBL().GetValorByClave("PUESTOREQUERIMIENTO")).ToString()));
            fields.SetField("NOTIFICADORES", (HttpUtility.HtmlDecode(new cParametroSistemaBL().GetValorByClave("NOTIFICADORES")).ToString()));

            // flatten form fields and close document
            stamper.FormFlattening = true;
            stamper.Close();
            joinPDF(formImprimir);
            return 1;
         }
           

        protected void joinPDF( string ruta)
        {

           iTextSharp.text.Document document = new iTextSharp.text.Document();          
            MemoryStream ms = new MemoryStream();  
            PdfCopy pdfCopy = new PdfCopy(document, ms);

            document.Open();

            
                PdfReader pdfReader = new PdfReader(ruta);
                int n = pdfReader.NumberOfPages;
                for (var page = 0; page < n;)
                {
                    pdfCopy.AddPage(pdfCopy.GetImportedPage(pdfReader, ++page));
                }
                pdfCopy.FreeReader(pdfReader);
            
            pdfCopy.Flush();

            document.Close();
            //ViewState["pdf"] = "requerimientos-" + DateTime.Now.Millisecond.ToString() + ".pdf";
          

            using (FileStream fs = new FileStream(ruta + ViewState["pdf"].ToString(), FileMode.OpenOrCreate))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(ms.ToArray());
                }
            }

            string urlpath = "~/Temporales/" + ViewState["pdf"].ToString(); 
            //Response.Redirect(urlpath, false);

            frameRecibo.Src = urlpath;



            //string urlpath = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;
            //urlpath = urlpath + "/Catastro/Temporales/" + ViewState["pdf"].ToString();
            //string parametros = "','" + ViewState["pdf"].ToString().Replace(".pdf", "") + "','height=600,width=550,toolbar=no,directories=no,menubar=no,scrollbars=no,resizable=no'";
            //String Clientscript = "window.open('" + urlpath + parametros + ",'_blank')";
           
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), ViewState["pdf"].ToString().Replace(".pdf", ""), Clientscript, true);


            ////print
            //pdf.Src = ruta + ViewState["pdf"].ToString();
        }

        #region Grid
        private void llenagrid(int idPredio)
        {
            if (idPredio == 0)
                idPredio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text.Trim()).Id;
            string[] filtro = (string[])ViewState["filtro"];

            grd.DataSource = null;
            grd.DataBind();

            grd.DataSource = tRequerimientoL(new tRequerimientoBL().GetAllbyIdPredio(idPredio));
            grd.DataBind();
            
        }

        private DataTable tRequerimientoL(List<tRequerimiento> liReq)
        {
            cUsuarios ur = (cUsuarios)Session["usuario"];
            DataTable datos = new DataTable("DTSorting");
            datos.Columns.Add("Id");            
            datos.Columns.Add("Folio");
            datos.Columns.Add("FechaEmision");
            datos.Columns.Add("Inicial");
            datos.Columns.Add("Final");
            datos.Columns.Add("Importe");
            datos.Columns.Add("Status");
            datos.Columns.Add("Activo");
            datos.Columns.Add("IdPredio");
            int folio = 0; bool p = true; int c = 0; tRequerimiento b2 = null; 
            if (liReq != null)
            {
                List<tRequerimiento> liureq = liReq;
                DataRow workRow = datos.NewRow();
                foreach (tRequerimiento b in liReq)
                {
                    c = c + 1;
                    if (p)
                    {
                        workRow = datos.NewRow();
                        String estado = "";
                        switch (b.Status)
                        {
                            case "A": estado = "Activo"; break;
                            case "C": estado = "Cancelado"; break;
                            case "S": estado = "Suspendido"; break;
                            case "P": estado = "Pagado"; break;
                            case "I": estado = "Impreso"; break;
                            default: estado = b.Status; break;
                        }
                        workRow[0] = b.Id;
                        workRow[1] = b.Folio;
                        folio = b.Folio;
                        workRow[2] = b.FechaEmision;
                        workRow[3] = b.BimestreInicial + " - " + b.EjercicioInicial;
                        workRow[4] = b.BimestreFinal + " - " + b.EjercicioFinal;
                        workRow[5] = b.Importe;
                        workRow[6] = estado;
                        workRow[7] = b.Activo;
                        workRow[8] = b.cPredio.Id;                            
                        b2 = liureq.FirstOrDefault(o => o.Folio == folio && o.Id != b.Id && o.FechaEmision == b.FechaEmision);
                        if (b2 != null)
                        {
                            p = false;
                            workRow[0] = b.Id + "-" + b2.Id;
                            workRow[5] = b.Importe + b2.Importe;
                            datos.Rows.Add(workRow);
                            b2 = null;
                            folio = 0;
                        }
                        else
                        {
                            p = true;
                            datos.Rows.Add(workRow);
                        }
                    }
                    if(c==2)
                    {
                        c = 0;
                        p = true;
                    }
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

        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd.PageIndex = e.NewPageIndex;
            llenagrid(0);
        }

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ConsultarRegistro")
            {
                Int32 id = Convert.ToInt32(e.CommandArgument.ToString());//ROW ID
                activarElementos(true);
                ViewState["ActivarChecked"] = 1;
                if (rbnUnificado.Checked)
                {
                    rbnUnificado_CheckedChanged(null,null);
                }

                if (rbtIPSM.Checked)
                {
                    rbtIPSM_CheckedChanged(null, null);
                }
                Int32 folio = Convert.ToInt32(grd.Rows[id].Cells[0].Text);
                DateTime fechaEmision  = Convert.ToDateTime(grd.Rows[id].Cells[1].Text);
                detallesRequerimientos(folio, fechaEmision);                              
            }
        }

        protected void grd_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["sortCampo"] == null)
            {
                ViewState["sortCampo"] = e.SortExpression.ToString();
                ViewState["sortOnden"] = "asc";
            }
            else
            {
                if (e.SortExpression.ToString() == ViewState["sortCampo"].ToString())
                {
                    if (ViewState["sortOnden"].ToString() == "asc")
                        ViewState["sortOnden"] = "desc";
                    else
                        ViewState["sortOnden"] = "asc";
                }
                else
                {
                    ViewState["sortCampo"] = e.SortExpression.ToString();
                    ViewState["sortOnden"] = "asc";
                }
            }

            llenagrid(0);
        }

        protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //string activo = grd.DataKeys[e.Row.RowIndex].Values[1].ToString();
                //if (activo.ToUpper() == "TRUE")
                //{
                //    //ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                //    //imgActivar.Visible = false;
                //    ImageButton imgConsulta = (ImageButton)e.Row.FindControl("imgConsulta");
                //    imgConsulta.Visible = false;
                //}
                //else
                //{
                //    ImageButton imgUpdate = (ImageButton)e.Row.FindControl("imgUpdate");
                //    imgUpdate.Visible = false;
                //    //ImageButton imgConsulta = (ImageButton)e.Row.FindControl("imgConsulta");
                //    //imgConsulta.Visible = false;
                //    //ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                //    //imgDelete.Visible = false;
                //}
            }
        }

        #endregion
        
        protected void generaReq(int idIP, int idSM)
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtClvCastatral.Focus();
            if (!IsPostBack)
            {
                ViewState["ActivarChecked"] = 0;
                divDer.Visible = false;
                divDAP.Visible = true;
                lblStatusPredial.Text = "Estatus";
                lblStatusSM.Visible = false;
                ddlStatusServicios.Visible = false;
                lblTipoDoc.Visible = false;
                ddlTipoDocumento.Visible = false;
                //Se llena el dropdownlist de agentes
                List<cAgenteFiscal> agentes = new cAgenteFiscalBL().GetFilter("", "", "TRUE", "Id", "asc");
                DataTable dtAgente = new DataTable();
                dtAgente.Columns.Add("Id");
                dtAgente.Columns.Add("Nombre");
                foreach (cAgenteFiscal agente in agentes)
                {
                    DataRow r = dtAgente.NewRow();
                    r["Id"] = agente.Id;
                    r["Nombre"] = agente.Nombre + " " + agente.ApellidoPaterno + " " + agente.ApellidoMaterno;
                    dtAgente.Rows.Add(r);
                }
                ddlAgente.DataSource = dtAgente;
                ddlAgente.DataTextField = "Nombre";
                ddlAgente.DataValueField = "Id";
                ddlAgente.DataBind();
                ddlAgente.Items.Insert(0, new ListItem("Sin Agente...", "0"));
                bloquearCampos(true);
                activarElementos(false);
                llenaTipoDocumento();
            }
        }

        protected void llenaTipoDocumento()
        {
            ddlTipoDocumento.Items.Clear();
            ddlTipoDocumento.DataValueField = "Id";
            ddlTipoDocumento.DataTextField = "Descripcion";
            ddlTipoDocumento.DataSource = new cTipoFaseDocBL().GetAll();
            ddlTipoDocumento.DataBind();
            ddlTipoDocumento.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar...", "%"));
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //limpiarCampos();
            //rbnUnificado.Enabled = true;
            //rbtIPSM.Enabled = true;            
            Response.Redirect("ActualizarRequerimiento.aspx");
        }

        protected void limpiarCampos()
        {
            txtContribuyente.Text = "";
            txtFechaEmision.Text = "";
            txtUbicacion.Text = "";
            txtFechaLimite.Text = "";
            txtDocumento.Text = "";
            txtFechaRecepcion.Text = "";
            txtFolioOficio.Text = "";
            txtPIIP.Text = "";
            txtPFIP.Text = "";
            txtPISM.Text = "";
            txtPFSM.Text = "";
            txtFase.Text = "";
            txtImpuestoIP.Text = "";
            txtImpuestoSM.Text = "";
            ddlStatusPredial.SelectedIndex = 0;
            ddlStatusServicios.SelectedIndex = 0;
            ddlTipoDocumento.SelectedIndex = 0;
            txtAdicionalesIP.Text = "";
            txtAdicionalesSM.Text = "";
            txtRecargoIP.Text = "";
            txtRecargosSM.Text = "";
            ddlAgente.SelectedIndex = 0;
            txtRezagoIP.Text = "";
            txtRezagoSM.Text = "";
            txtOficio.Text = "";
            txtDiferenciasIP.Text = "";
            txtDAP.Text = "";
            txtDAP0.Text = "";
            txtLimpiezaSM.Text = "";
            txtRecoleccionSM.Text = "";
            txtFechaRecepcion.Text = "";
            txtRecIP.Text = "";
            ddlFirmante.SelectedValue = "Personal";//.Text = "";
            txtEjecucionIP.Text = "";
            txtEjecucionSM.Text = "";
            txtHonorarioIP.Text = "";
            txtHonorarioSM.Text = "";
            txtMultaIP.Text = "";
            txtMultaSM.Text = "";
            txtObservaciones.Text = "";
            txtImporteIP.Text = "";
            txtImporteSM.Text = "";
            txtNumPagoIP.Text = "";
            txtNumPagoSM.Text = "";
            txtFechaPagoIP.Text = "";
            txtFechaPagoSM.Text = "";
            txtReciboIP.Text = "";
            txtReciboSM.Text = "";
            txtFechaEstrado.Text = "";
            txtClvCastatral.Text = "";
            txtClvCastatral.Enabled = true;
            ddlStatusPredial.Enabled = true;
            ddlStatusServicios.Enabled = true;
            ddlTipoDocumento.Enabled = true;
            btnBuscar.Visible = true;
            activarElementos(false);
            ViewState["ActivarChecked"] = 0;
            grd.DataSource = null;
            grd.DataBind();
            grd.Visible = false;
        }

        protected void bloquearCampos(Boolean bBloquear)
        {
            ddlStatusServicios.Enabled = !bBloquear;
            ddlStatusPredial.Enabled = !bBloquear;
            ddlTipoDocumento.Enabled = !bBloquear;
            txtFechaRecepcion.Enabled = !bBloquear;
            //txtFolioOficio.Enabled = !bBloquear;
            txtFechaRecepcion.Enabled = !bBloquear;
            ddlAgente.Enabled = !bBloquear;
            txtOficio.Enabled = !bBloquear;
            ddlFirmante.Enabled = !bBloquear;
            txtObservaciones.Enabled = !bBloquear;
            btnBuscar.Visible = bBloquear;         
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            MensajesInterfaz msg = new MensajesInterfaz();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    //tRequerimiento req = new tRequerimientoBL().obtenerRequerimientoporIdPredio(Convert.ToInt32(hdfIdPredio.Value), "IP");
                    tRequerimiento req = new tRequerimientoBL().obtenerRequerimientoIdPredio(Convert.ToInt32(hdfIdPredio.Value), Convert.ToInt32(txtFolioOficio.Text), "IP");
                    Boolean bBand = false;
                    if (req != null)
                    {
                        if (req.Status == "C")
                        {
                            if (!(new tRequerimientoBL().validarRequerimientoActivo(Convert.ToInt32(hdfIdPredio.Value), "IP", req.FechaEmision)))
                            {
                                throw new System.ArgumentException(new Utileria().GetDescription("Imposible actualizar el requerimiento, se encuentra activo otro requerimiento"));
                            }
                        }
                                               
                        if (ddlStatusPredial.SelectedValue == "C" && req.Status == "A")
                        {
                            req.FechaCancelacion = DateTime.Today;
                            req.Activo = false;
                            //req.Activo = false;
                            cPredio predio = new cPredioBL().GetByConstraint(Convert.ToInt32(hdfIdPredio.Value));
                            predio.IdTipoFaseIp = 1;
                            //predio.IdTipoFaseSm = 1;
                            predio.IdUsuario = U.Id;
                            predio.FechaModificacion = DateTime.Now;
                            msg = new cPredioBL().Update(predio);

                            msg = new tRequerimientoBL().CancelAnteriorPorPredioPagado(req.IdPredio, req.TipoImpuesto, req.Id);
                            if (msg != MensajesInterfaz.Actualizacion)
                                throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");

                        }
                        if (ddlStatusPredial.SelectedValue == "A" && req.Status == "C")
                        {
                            req.FechaCancelacion = null;
                            req.Activo = true;
                            cPredio predio = new cPredioBL().GetByConstraint(Convert.ToInt32(hdfIdPredio.Value));
                            predio.IdTipoFaseIp = req.TipoFase;
                            //predio.IdTipoFaseSm = req.TipoFase;
                            predio.IdUsuario = U.Id;
                            predio.FechaModificacion = DateTime.Now;
                            msg = new cPredioBL().Update(predio);
                                                        
                            if (msg != MensajesInterfaz.Actualizacion)
                                throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                        }
                        req.Status = ddlStatusPredial.SelectedValue;

                        if (txtFechaRecepcion.Text != "")
                            req.FechaRecepcion = Convert.ToDateTime(txtFechaRecepcion.Text);
                        else
                            req.FechaRecepcion = null;
                        if (txtFechaEstrado.Text != "")
                            req.FechaOficio = Convert.ToDateTime(txtFechaEstrado.Text);
                        else
                            req.FechaOficio = null;

                        req.CaracterFirmante = ddlFirmante.SelectedValue;//.Text;
                        req.Observaciones = txtObservaciones.Text;
                        req.Oficio = txtOficio.Text;
                        if ( ddlAgente.SelectedItem.Text.Substring(1,1) == "(" )
                            throw new System.ArgumentException("Agente dado de baja", "Error");

                        if (ddlAgente.SelectedValue != "0")
                            req.IdAgenteFiscal = Convert.ToInt32(ddlAgente.SelectedValue);
                        else
                            req.IdAgenteFiscal = null;

                        if (ddlTipoDocumento.SelectedValue != "%")
                            req.IdTipoFaseDoc = Convert.ToInt32(ddlTipoDocumento.SelectedValue);
                        else
                            req.IdTipoFaseDoc = null;

                        //req.FechaModificacion = DateTime.Today;
                        req.IdUsuario = U.Id;
                        req.FechaModificacion = DateTime.Now;
                        //req.Activo = true;
                        MensajesInterfaz resul = new tRequerimientoBL().Update(req);
                        if ((resul == MensajesInterfaz.Actualizacion))
                            bBand = true;
                    }
                    else
                        bBand = true;

                    if (bBand)//Servicios Municipales
                    {
                        bBand = false;
                        //req = new tRequerimientoBL().obtenerRequerimientoporIdPredio(Convert.ToInt32(hdfIdPredio.Value), "SM");
                        req = new tRequerimientoBL().obtenerRequerimientoIdPredio(Convert.ToInt32(hdfIdPredio.Value), Convert.ToInt32(txtFolioOficio.Text), "SM");
                        if (req != null)
                        {
                            if (rbnUnificado.Checked)
                            {
                                req.Status = ddlStatusPredial.SelectedValue;
                                if (ddlStatusPredial.SelectedValue == "C")
                                {
                                    req.FechaCancelacion = DateTime.Today;
                                    req.Activo = false;

                                    cPredio predio = new cPredioBL().GetByConstraint(Convert.ToInt32(hdfIdPredio.Value));
                                    predio.IdTipoFaseSm = 1;
                                    predio.IdUsuario = U.Id;
                                    predio.FechaModificacion = DateTime.Now;
                                    msg = new cPredioBL().Update(predio);

                                    msg = new tRequerimientoBL().CancelAnteriorPorPredioPagado(req.IdPredio, req.TipoImpuesto, req.Id);
                                    if (msg != MensajesInterfaz.Actualizacion)
                                        throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                                }
                            }
                            else
                            {
                                req.Status = ddlStatusServicios.SelectedValue;
                                if (ddlStatusServicios.SelectedValue == "C")
                                {
                                    req.FechaCancelacion = DateTime.Today;
                                    req.Activo = false;
                                }

                            }
                            if (txtFechaRecepcion.Text != "")
                                req.FechaRecepcion = Convert.ToDateTime(txtFechaRecepcion.Text);
                            else
                                req.FechaRecepcion = null;
                            if (txtFechaEstrado.Text != "")
                                req.FechaOficio = Convert.ToDateTime(txtFechaEstrado.Text);
                            else
                                req.FechaOficio = null;
                            req.CaracterFirmante = ddlFirmante.SelectedValue;//.Text;
                            req.Observaciones = txtObservaciones.Text;
                            req.Oficio = txtOficio.Text;
                            if (ddlAgente.SelectedValue != "0")
                                req.IdAgenteFiscal = Convert.ToInt32(ddlAgente.SelectedValue);
                            else
                                req.IdAgenteFiscal = null;

                            if (ddlTipoDocumento.SelectedValue != "%")
                                req.IdTipoFaseDoc = Convert.ToInt32(ddlTipoDocumento.SelectedValue);
                            else
                                req.IdTipoFaseDoc = null;

                            req.FechaModificacion = DateTime.Today;
                            req.IdUsuario = U.Id;
                            req.FechaModificacion = DateTime.Now;
                            MensajesInterfaz resul = new tRequerimientoBL().Update(req);
                            if ((resul == MensajesInterfaz.Actualizacion))// && (resul == msg))
                                bBand = true;
                        }
                        else
                            bBand = true;
                    }
                    if (bBand)
                    {
                        //limpiarCampos();
                        //bloquearCampos(true);
                        //rbnUnificado.Enabled = true;
                        //rbtIPSM.Enabled = true;
                        //ddlTipoDocumento.Visible = false;
                        scope.Complete();
                        vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.Actualizacion), ModalPopupMensaje.TypeMesssage.Alert);
                        btnCancelar_Click(null, null);
                    }
                    else
                        vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ErrorActualizar), ModalPopupMensaje.TypeMesssage.Alert);
                }
            }
            catch (Exception error)
            {
                if (error.Message != "Subproceso anulado.")
                {
                    new Utileria().logError("ActualizarRequerimiento.btnGuardar_Click.Exception", error);
                    vtnModal.ShowPopup(new Utileria().GetDescription(error.Message), ModalPopupMensaje.TypeMesssage.Alert);
                }
            }
        }

        protected void txtClvCastatral_TextChanged(object sender, EventArgs e)
        {
            //if (txtClvCastatral.Text.Length == 12)
            //{
            //    cPredio Predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);
            //    if (Predio != null && Predio.Activo==true && (Predio.cStatusPredio.Descripcion=="A" || Predio.cStatusPredio.Descripcion == "S"))
            //    {
            //        Boolean bBand = false;
            //        txtContribuyente.Text = Predio.cContribuyente.Nombre + " " + Predio.cContribuyente.ApellidoPaterno + " " + Predio.cContribuyente.ApellidoMaterno;
            //        txtUbicacion.Text = Predio.Calle + " NÚM. " + Predio.Numero + " COL. " + Predio.cColonia.NombreColonia + " C.P. " + Predio.CP + " LOCALIDAD " + Predio.Localidad;
            //        hdfIdPredio.Value = Predio.Id.ToString();
            //        txtDocumento.Text = Predio.IdTipoFaseIp != null ? Predio.cTipoFase.DoctoImprimir : Predio.cTipoFase1.DoctoImprimir;
            //        tRequerimiento req = new tRequerimientoBL().obtenerRequerimientoporIdPredio(Predio.Id, "IP");
            //        if (req != null)
            //        {
            //            bBand = true;
            //            txtFechaEmision.Text = req.FechaEmision.ToShortDateString();
            //            txtFechaLimite.Text = req.FechaLimite.ToShortDateString();
            //            txtFolioOficio.Text = req.Folio.ToString();
            //            txtFase.Text = new cTipoFaseBL().GetByConstraint(req.TipoFase).Descripcion;
            //            txtFechaEstrado.Text = req.FechaOficio==null?"":Convert.ToDateTime(req.FechaOficio).ToShortDateString();
            //            //txtFechaRecepcion.Text = req.FechaOficio.ToString();
            //            txtFechaRecepcion.Text = req.FechaRecepcion.ToString();
            //            txtFirmante.Text = req.CaracterFirmante;
            //            txtObservaciones.Text = req.Observaciones;
            //            txtOficio.Text = req.Oficio;
            //            ddlAgente.SelectedValue = req.IdAgenteFiscal == null ? "0" : req.IdAgenteFiscal.ToString();
            //            ddlStatusPredial.SelectedValue = req.Status;
            //            //ddlStatusServicios.Enabled = false;
            //            txtPIIP.Text = req.BimestreInicial.ToString() + " - " + req.EjercicioInicial.ToString();
            //            txtPFIP.Text = req.BimestreFinal.ToString() + " - " + req.EjercicioFinal.ToString();
            //            txtImpuestoIP.Text = req.Impuesto.ToString();
            //            txtAdicionalesIP.Text = req.Adicional.ToString();
            //            txtRecargoIP.Text = req.Recargo.ToString();
            //            txtRezagoIP.Text = req.Rezago.ToString();
            //            txtDiferenciasIP.Text = req.Diferencia.ToString();
            //            txtRecIP.Text = req.RecargoDiferencia.ToString();
            //            txtEjecucionIP.Text = req.Ejecucion.ToString();
            //            txtMultaIP.Text = req.Multa.ToString();
            //            txtImporteIP.Text = req.Importe.ToString();
            //            txtFechaPagoIP.Text = req.FechaPago.ToString();
            //            txtReciboIP.Text = req.Recibo.ToString();
            //            if (txtFechaPagoIP.Text != "")
            //                txtNumPagoIP.Text = "1";
            //        }
            //        req = new tRequerimientoBL().obtenerRequerimientoporIdPredio(Predio.Id, "SM");
            //        if (req != null)
            //        {
            //            if (!bBand)
            //            {
            //                txtFechaEmision.Text = req.FechaEmision.ToShortDateString();
            //                txtFechaLimite.Text = req.FechaLimite.ToShortDateString();
            //                txtFechaEstrado.Text = Convert.ToDateTime(req.FechaOficio).ToShortDateString();
            //                txtFolioOficio.Text = req.Folio.ToString();
            //                txtFase.Text = new cTipoFaseBL().GetByConstraint(req.TipoFase).Descripcion;
            //                //txtFechaRecepcion.Text = req.FechaOficio.ToString();
            //                txtFechaRecepcion.Text = req.FechaRecepcion.ToString();
            //                txtFirmante.Text = req.CaracterFirmante;
            //                txtObservaciones.Text = req.Observaciones;
            //                txtOficio.Text = req.Oficio;
            //                ddlAgente.SelectedValue = req.IdAgenteFiscal == null ? "0" : req.IdAgenteFiscal.ToString();
            //                ddlStatusPredial.Enabled = false;
            //                bBand = true;
            //            }
            //            ddlStatusServicios.SelectedValue = req.Status;
            //            txtPISM.Text = req.BimestreInicial.ToString() + " - " + req.EjercicioInicial.ToString();
            //            txtPFSM.Text = req.BimestreFinal.ToString() + " - " + req.EjercicioFinal.ToString();
            //            txtImpuestoSM.Text = req.Impuesto.ToString();
            //            txtAdicionalesSM.Text = req.Adicional.ToString();
            //            txtRecargosSM.Text = req.Recargo.ToString();
            //            txtRezagoSM.Text = req.Rezago.ToString();
            //            txtDAP.Text = req.Diferencia.ToString();
            //            txtEjecucionSM.Text = req.Ejecucion.ToString();
            //            txtMultaSM.Text = req.Multa.ToString();
            //            txtImporteSM.Text = req.Importe.ToString();
            //            txtFechaPagoSM.Text = req.FechaPago.ToString();
            //            txtReciboSM.Text = req.Recibo.ToString();
            //            if(txtFechaPagoSM.Text!="")
            //                txtNumPagoSM.Text = "1";
            //        }
            //        if (!bBand)
            //        {
            //            vtnModal.ShowPopup("No hay información de requerimiento para esa clave predial", ModalPopupMensaje.TypeMesssage.Alert);
            //            txtClvCastatral.Text = "";
            //        }
            //        else
            //        {
            //            bloquearCampos(false);
            //            txtClvCastatral.Enabled = false;
            //        }

            //    }
            //    else
            //    {
            //        vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.PredioInexistente), ModalPopupMensaje.TypeMesssage.Alert);
            //        txtClvCastatral.Text = "";
            //    }
            //}
            //else
            //{
            //    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.FormatoPredio), ModalPopupMensaje.TypeMesssage.Alert);
            //    txtClvCastatral.Text = "";
            //}

        }

        protected void detallesRequerimientos( Int32 folio,DateTime fechaEmision)
        {
            Boolean bBand = false;
            
            tRequerimiento req = new tRequerimientoBL().obtenerRequerimientoIdPredioFecha(Convert.ToInt32(hdfIdPredio.Value), folio, "IP",fechaEmision);
            if (req != null)
            {
                bBand = true;
                txtFechaEmision.Text = req.FechaEmision.ToShortDateString();
                txtFechaLimite.Text = req.FechaLimite.ToShortDateString();
                txtFolioOficio.Text = req.Folio.ToString();
                txtFase.Text = new cTipoFaseBL().GetByConstraint(req.TipoFase).Descripcion;
                txtFechaEstrado.Text = req.FechaOficio == null ? "" : Convert.ToDateTime(req.FechaOficio).ToShortDateString();
                //txtFechaRecepcion.Text = req.FechaOficio.ToString();
                txtFechaRecepcion.Text = req.FechaRecepcion.ToString();
                if (req.CaracterFirmante != "" && req.CaracterFirmante != null)
                {
                    ddlFirmante.SelectedValue = req.CaracterFirmante;
                }
                txtObservaciones.Text = req.Observaciones;
                txtOficio.Text = req.Oficio;

                //buscar agente
                //ddlAgente.Items.Insert(0, new ListItem("Sin Agente...", "0"));
                if (ddlAgente.Items.IndexOf (ddlAgente.Items.FindByValue(req.IdAgenteFiscal == null ? "0" : req.IdAgenteFiscal.ToString())) >= 0)
                    ddlAgente.SelectedValue = req.IdAgenteFiscal == null ? "0" : req.IdAgenteFiscal.ToString();
                else
                {
                    cAgenteFiscal af = new cAgenteFiscalBL().GetByConstraint(req.IdAgenteFiscal.Value);
                    ddlAgente.Items.Insert( (ddlAgente.Items.Count), new ListItem("(" + af.Nombre +" "+af.ApellidoPaterno+" "+af.ApellidoMaterno + ")" , af.Id.ToString()));
                    ddlAgente.SelectedValue = req.IdAgenteFiscal == null ? "0" : req.IdAgenteFiscal.ToString();
                }
                ddlStatusPredial.SelectedValue = req.Status;
                txtPIIP.Text = req.BimestreInicial.ToString() + " - " + req.EjercicioInicial.ToString();
                txtPFIP.Text = req.BimestreFinal.ToString() + " - " + req.EjercicioFinal.ToString();
                txtImpuestoIP.Text = req.Impuesto == null ? "0.00": req.Impuesto.ToString();
                txtAdicionalesIP.Text = req.Adicional == null? "0.00": req.Adicional.ToString();
                txtRecargoIP.Text = req.Recargo == null ? "0.00":req.Recargo.ToString();
                txtRezagoIP.Text = req.Rezago == null ? "0.00":req.Rezago.ToString();
                txtDiferenciasIP.Text = req.Diferencia == null ? "0.0":req.Diferencia.ToString();
                txtRecIP.Text = req.RecargoDiferencia == null ? "0.00": req.RecargoDiferencia.ToString();
                txtHonorarioIP.Text = req.Honorarios == null ? "0.00":req.Honorarios.ToString();
                txtEjecucionIP.Text = req.Ejecucion == null ? "0.00":req.Ejecucion.ToString();                
                txtMultaIP.Text = req.Multa == null ? "0.00": req.Multa.ToString();
                txtImporteIP.Text = req.Importe== null ? "0.00" :req.Importe.ToString();
                txtFechaPagoIP.Text = req.FechaPago.ToString();
                txtReciboIP.Text = req.Recibo.ToString();
                if (txtFechaPagoIP.Text != "")
                    txtNumPagoIP.Text = "1";
            }
            req = new tRequerimientoBL().obtenerRequerimientoIdPredioFecha(Convert.ToInt32(hdfIdPredio.Value), folio,"SM",fechaEmision);
            if (req != null)
            {
                if (!bBand)
                {
                    txtFechaEmision.Text = req.FechaEmision.ToShortDateString();
                    txtFechaLimite.Text = req.FechaLimite.ToShortDateString();
                    txtFechaEstrado.Text = Convert.ToDateTime(req.FechaOficio).ToShortDateString();
                    txtFolioOficio.Text = req.Folio.ToString();
                    txtFase.Text = new cTipoFaseBL().GetByConstraint(req.TipoFase).Descripcion;
                    //txtFechaRecepcion.Text = req.FechaOficio.ToString();
                    txtFechaRecepcion.Text = req.FechaRecepcion.ToString();
                    ddlFirmante.SelectedValue = req.CaracterFirmante;
                    txtObservaciones.Text = req.Observaciones;
                    txtOficio.Text = req.Oficio;
                    ddlAgente.SelectedValue = req.IdAgenteFiscal == null ? "0" : req.IdAgenteFiscal.ToString();
                    ddlStatusPredial.Enabled = false;
                    bBand = true;
                }
                ddlStatusServicios.SelectedValue = req.Status;
                txtPISM.Text = req.BimestreInicial.ToString() + " - " + req.EjercicioInicial.ToString();
                txtPFSM.Text = req.BimestreFinal.ToString() + " - " + req.EjercicioFinal.ToString();
                txtImpuestoSM.Text = req.Impuesto == null ? "0.00":req.Impuesto.ToString();
                txtAdicionalesSM.Text = req.Adicional == null ? "0.00":req.Adicional.ToString();
                txtRecargosSM.Text = req.Recargo == null ? "0.00" : req.Recargo.ToString();
                txtRezagoSM.Text = req.Rezago == null ? "0.00":req.Rezago.ToString();
                if (req.IdTipoFaseDoc != null)
                {
                    ddlTipoDocumento.SelectedValue =req.IdTipoFaseDoc.ToString();
                }
                if (rbtIPSM.Checked)
                    txtDAP.Text = req.Dap== null ? "0.00": req.Dap.ToString();
                else
                    txtDAP0.Text = req.Diferencia == null ? "0.00": req.Diferencia.ToString();

                txtRecoleccionSM.Text = req.Recoleccion == null ? "0.00":req.Recoleccion.ToString();
                txtLimpiezaSM.Text = req.Limpieza == null ? "0.00": req.Limpieza.ToString();
                txtHonorarioSM.Text = req.Honorarios == null ? "0.00" : req.Honorarios.ToString();
                txtEjecucionSM.Text = req.Ejecucion == null ? "0.00" : req.Ejecucion.ToString();
                txtMultaSM.Text = req.Multa == 0 ? "0.00": req.Multa.ToString();
                txtImporteSM.Text = req.Importe == null ? "0.00":req.Importe.ToString();
                txtFechaPagoSM.Text = req.FechaPago.ToString();
                txtReciboSM.Text = req.Recibo.ToString();
                if (txtFechaPagoSM.Text != "")
                    txtNumPagoSM.Text = "1";
            }
            if (!bBand)
            {
                vtnModal.ShowPopup("No hay información de requerimiento para esa clave predial", ModalPopupMensaje.TypeMesssage.Alert);
                txtClvCastatral.Text = "";
            }
            else
            {
                bloquearCampos(false);
                rbnUnificado.Enabled = false;
                rbtIPSM.Enabled = false;
                txtClvCastatral.Enabled = false;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtClvCastatral.Text.Length == 12)
            {
                cPredio Predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);
                
                if (Predio != null && Predio.Activo == true && (Predio.cStatusPredio.Descripcion == "A" || Predio.cStatusPredio.Descripcion == "S"))
                {                    
                    
                    txtContribuyente.Text = Predio.cContribuyente.Nombre + " " + Predio.cContribuyente.ApellidoPaterno + " " + Predio.cContribuyente.ApellidoMaterno;
                    txtUbicacion.Text = Predio.Calle + " NÚM. " + Predio.Numero + " COL. " + Predio.cColonia.NombreColonia + " C.P. " + Predio.CP + " LOCALIDAD " + Predio.Localidad;
                    hdfIdPredio.Value = Predio.Id.ToString();
#pragma warning disable CS0472 // El resultado de la expresión siempre es 'true' porque un valor del tipo 'int' nunca es igual a 'NULL' de tipo 'int?'
                    if (Predio.IdTipoFaseIp != null)
                    {
                        cTipoFase TF = new cTipoFaseBL().GetByConstraint(Predio.IdTipoFaseIp);
                        txtDocumento.Text = TF.DoctoImprimir.ToString();
                    }
#pragma warning restore CS0472 // El resultado de la expresión siempre es 'true' porque un valor del tipo 'int' nunca es igual a 'NULL' de tipo 'int?'
                    llenagrid(Predio.Id);
                }
                else
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.PredioInexistente), ModalPopupMensaje.TypeMesssage.Alert);
                    txtClvCastatral.Text = "";
                }
            }
            else
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.FormatoPredio), ModalPopupMensaje.TypeMesssage.Alert);
                txtClvCastatral.Text = "";
            }
        }

        protected void rbnUnificado_CheckedChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt16(ViewState["ActivarChecked"]) == 1)
            {
                //rbtIPSM.Checked = false;
                divDer.Visible = false;
                divDAP.Visible = true;
                lblStatusPredial.Text = "Estatus";
                lblStatusSM.Visible = false;
                ddlStatusServicios.Visible = false;
                lblTipoDoc.Visible = true;
                ddlTipoDocumento.Visible = true;
            }
           

        }

        protected void rbtIPSM_CheckedChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt16(ViewState["ActivarChecked"]) == 1)
            {
                //rbnUnificado.Checked = false;
                divDer.Visible = true;
                divDAP.Visible = false;
                lblStatusPredial.Text = "Estatus Predial";
                lblStatusSM.Visible = true;
                ddlStatusServicios.Visible = true;
                lblTipoDoc.Visible = true;
                ddlTipoDocumento.Visible = true;
            }
            
        }

        protected void activarElementos(Boolean activa)
        {
            lblPeridos.Visible = activa;
            lblFase.Visible = activa;
            divDAP.Visible = activa;
            divIzq.Visible = activa;
            btnGuardar.Visible = activa;
            btnReimprimir.Visible = activa;
            txtFase.Visible = activa;
            lblStatusPredial.Visible = activa;
            lblTipoDoc.Visible = activa;
            ddlStatusPredial.Visible = activa;
            lblAgente.Visible = activa;
            ddlAgente.Visible = activa;
            lblOficio.Visible = activa;
            txtOficio.Visible = activa;
            lblFecha.Visible = activa;
            txtFechaRecepcion.Visible = activa;
            lblFirmante.Visible = activa;
            ddlFirmante.Visible = activa;
            lblObservaciones.Visible = activa;
            txtObservaciones.Visible = activa;
            lblFolioOficio.Visible = activa;
            txtFolioOficio.Visible = activa;
            lblFechaEstrado.Visible = activa;
            txtFechaEstrado.Visible = activa;
            btnCancelar.Visible = activa;
            lblResultado.Visible = activa;                       
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            modalRecibo.Hide();
        }

        protected void btnReimprimir_Click(object sender, EventArgs e)
        {
            
            //vallida que exista el formato siguiente
            string Path = Server.MapPath("~/");            
            string paramMunicipio = new cParametroSistemaBL().GetByClave("MUNICIPIO_COBRO").Valor;
            cTipoFase fase = new cTipoFaseBL().GetByConstraint(new tRequerimientoBL().obtenerRequerimientoIdPredio(Convert.ToInt32(hdfIdPredio.Value), Convert.ToInt32(txtFolioOficio.Text), "IP").TipoFase);
                       
            string formOriginal = Path + "/Documentos/" + fase.DoctoImprimir + "_" + paramMunicipio + ".pdf";
            //string formOriginal = Path + "/Documentos/Requerimiento_" + paramMunicipio + ".pdf";

            if (!File.Exists(formOriginal))
            {
                vtnModal.ShowPopup("No existe sig. formato " + formOriginal, ModalPopupMensaje.TypeMesssage.Confirm);
                vtnModal.DysplayCancelar = true;
                return;
            }



                int cont = 0;
                try
                {
                    cont =  Calcula(formOriginal);
                }
                catch (Exception ex)
                {
                    new Utileria().logError("Requerimiento , btnRequerir", ex);
                    cont = -1;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "verMsg", "hideGenerar();", true);
                    vtnModal.DysplayCancelar = false;
                    vtnModal.ShowPopup("Error al generar los archivos", ModalPopupMensaje.TypeMesssage.Confirm);
                    vtnModal.DysplayCancelar = true;
                }

                if (cont > 0)
                {
                    modalRecibo.Show();
                    //txtTarjetas.Text = cont.ToString();

                }
                else
                {
                    modalRecibo.Hide();
                    vtnModal.DysplayCancelar = false;
                    vtnModal.ShowPopup("No existen registros que coincidan con el rango proporcionado", ModalPopupMensaje.TypeMesssage.Confirm);
                    vtnModal.DysplayCancelar = true;
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "verMsg", "hideGenerar();", true);
                
        }
    }
}