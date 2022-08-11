using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Clases;
using Clases.BL;
using Clases.Utilerias;
using System.Data;
using Catastro.Controles;
using System.Transactions;
using FindAndReplace;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Globalization;

namespace Catastro.Convenios
{
    public partial class Convenio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["idConvenio"]))
                {
                    hdfId.Value = Request.QueryString["idConvenio"].ToString();
                    tConvenio convenio = new tConvenioBL().GetByConstraint(Convert.ToInt32(hdfId.Value));
                    llenarDatosPredio(convenio.IdConvenioEdoCta, false);
                    txtParcialidades.Text = convenio.NoParcialidades.ToString();
                    txtRecargo.Text = convenio.PorcentajeConvenio.ToString();
                    txtFechaFinal.Text = convenio.FechaFin.ToShortDateString();
                    txtFechaInicial.Text = convenio.FechaIni.ToShortDateString();
                    txtMonto.Text = convenio.ImporteTotal.ToString();
                    txtFolio.Text = convenio.Folio;
                    txtTelefono.Text = convenio.Telefono;
                    txtCelular.Text = convenio.Celular;
                    txtNoIdentificacion.Text = convenio.NoIdentificacion;
                    txtEmail.Text = convenio.Email;


                    //String strEstado = llenarGrid();
                    llenarGrid();
                    btnCalcular.Visible = false;
                    btnCancelar.Visible = false;
                    btnAtras.Visible = true;
                    txtClvCastatral.Enabled = false;
                    txtFechaFinal.Enabled = false;
                    txtFechaInicial.Enabled = false;
                    txtParcialidades.Enabled = false;
                    txtFolio.Enabled = false;
                    txtTelefono.Enabled = false;
                    txtCelular.Enabled = false;
                    txtNoIdentificacion.Enabled = false;
                    txtEmail.Enabled = false;
                    for (int i = 2; i <= grdPagos.Rows.Count; i++)
                    {
                        if (Convert.ToDecimal(((TextBox)grdPagos.Rows[i - 1].FindControl("txtRecargo")).Text) == 0)
                        {
                            chkRecargo.Checked = true;
                            break;
                        }
                    }

                    //chkRecargo.Enabled = false;
                    if (!String.IsNullOrEmpty(Request.QueryString["tipo"]))
                    {
                        if (Request.QueryString["tipo"].ToString() == "m")
                        {
                            if ((convenio.Status == "X") || (convenio.Status == "A"))
                            {
                                btnGuardar.Visible = false;
                                btnModificar.Visible = true;
                                btnFinalizar.Visible = false;
                            }
                            else
                                grdPagos.Enabled = false;
                        }
                        else
                            grdPagos.Enabled = false;
                    }
                }
                else if (!String.IsNullOrEmpty(Request.QueryString["Clave"]))
                {
                    txtClvCastatral.Text = Request.QueryString["Clave"].ToString().Trim();
                    llenarDatosPredio(-1, true);
                    btnGuardar.Visible = false;
                    btnModificar.Visible = false;
                    btnFinalizar.Visible = false;
                    btnImprime.Visible = false;
                    txtFolio.Enabled = false;
                    txtFolio.Text = "-";
                }
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        protected void llenarDatosPredio(int IdConvenioEdoCta, Boolean isNew)
        {
            if (isNew)
            {
                Buscar(isNew, 0);
            }
            else
            {
                List<tConvenioEdoCta> Predios = new tConvenioEdoCtaBL().GetByFolio(IdConvenioEdoCta);
                if (Predios != null)
                {
                    //tConvenioEdoCta Predio = Predios[0];
                    txtClvCastatral.Text = Predios[0].cPredio.ClavePredial.Trim();
                    Buscar(false, Predios[0].Id);
                    ////txtNombreAdquiriente.Text = Predio.cPredio.cContribuyente.ApellidoPaterno + " " + Predio.cPredio.cContribuyente.ApellidoMaterno + " "+Predio.cPredio.cContribuyente.Nombre ;
                    ////txtMonto.Text = Decimal.Round(Predio.Importe).ToString();
                    ////txtTipoImpuesto.Text = Predio.TipoImpuesto == "IP" ? "Impuesto Predial" : Predio.TipoImpuesto == "SM" ? "Servicios Municipales" : Predio.TipoImpuesto == "A" ? "Ambos" : "";
                    //////hdfIdCovenioEdoCta.Value = Predio.Id.ToString();  akpl
                    ////hdfIdPredio.Value = Predio.cPredio.Id.ToString();

                    ////hdfIdCovenioEdoCta.Value = Predio.Folio.ToString();
                }

            }
        }

        protected void btnCalcular_Click(object sender, EventArgs e)
        {
            DataTable dt = crearDataTable();
            DateTime fechaPago = Convert.ToDateTime(txtFechaInicial.Text);
            Decimal total = 0;
            Decimal resto = 0;
            Decimal parcialidad = 0;
            DateTime fechaTrienio = Convert.ToDateTime(new cParametroSistemaBL().GetValorByClave("FechaTrienio"));
            /*Se valida que la fecha el convenio no sea mayor a la fecha el trienio*/
            vtnModal.DysplayCancelar = false;

            if (fechaValida(fechaPago) || fechaValida(Convert.ToDateTime(txtFechaFinal.Text)))
                vtnModal.ShowPopup("La fecha inicial y final, deben de ser un hábil.", ModalPopupMensaje.TypeMesssage.Error);
            else
            {
                if (Convert.ToDateTime(txtFechaFinal.Text) < Convert.ToDateTime(txtFechaInicial.Text))
                    vtnModal.ShowPopup("La fecha final debe ser mayor a la fecha final.", ModalPopupMensaje.TypeMesssage.Error);
                /*Se valida que la fecha final sea mayor a la fecha inicial*/
                else if (Convert.ToDateTime(txtFechaFinal.Text) > fechaTrienio)
                    vtnModal.ShowPopup("La fecha final del convenio no puede ser Mayor a la fecha del trienio. Fecha de trieno : " + fechaTrienio.ToShortDateString() + ".", ModalPopupMensaje.TypeMesssage.Error);
                /*Se valida que el número de parcialidades no exceda la fecha final del trienio*/
                else
                {
                    TimeSpan diasTrienio = fechaTrienio - Convert.ToDateTime(txtFechaInicial.Text);
                    Int32 mesesTrienio = Convert.ToInt32(Decimal.Round(diasTrienio.Days / 30) + 1);
                    //if (Math.Abs((Convert.ToDateTime(txtFechaInicial.Text).Month - fechaTrienio.Month) + 12 * (Convert.ToDateTime(txtFechaInicial.Text).Year - fechaTrienio.Year)) < Convert.ToInt32(txtParcialidades.Text))
                    if (mesesTrienio < Convert.ToInt32(txtParcialidades.Text))
                        vtnModal.ShowPopup("El número de parcialidades excede el final de trienio. Número de parcialidades máximo:" + mesesTrienio.ToString() + ".", ModalPopupMensaje.TypeMesssage.Error);
                    /*Se valida que el número d parcialidades no exceda la fecha final del convenio*/
                    else
                    {
                        TimeSpan dias = Convert.ToDateTime(txtFechaFinal.Text) - Convert.ToDateTime(txtFechaInicial.Text);
                        Int32 meses = Convert.ToInt32(Decimal.Round(dias.Days / 30) + 1);
                        //if (Math.Abs((Convert.ToDateTime(txtFechaInicial.Text).Month - Convert.ToDateTime(txtFechaFinal.Text).Month) + 12 * (Convert.ToDateTime(txtFechaInicial.Text).Year - Convert.ToDateTime(txtFechaFinal.Text).Year)) < Convert.ToInt32(txtParcialidades.Text))
                        if (meses < Convert.ToInt32(txtParcialidades.Text))
                            vtnModal.ShowPopup("El número de parcialidades excede el final del convenio. Número de parcialidades máximo:" + meses.ToString() + ".", ModalPopupMensaje.TypeMesssage.Error);
                        else
                        {
                            /*Se valida que la fecha de pago no caiga en día feriado, sabado o domingo*/
                            while (fechaValida(fechaPago))
                            {
                                fechaPago = fechaPago.AddDays(1);
                                //if (fechaPago == Convert.ToDateTime(txtFechaFinal.Text).Date || bLimit == true)
                                //{
                                //    fechaPago = fechaPago.AddDays(-1);
                                //    bLimit = true;
                                //}
                                //else
                                //    fechaPago = fechaPago.AddDays(1);
                            }
                            resto = Decimal.Round(Convert.ToDecimal(txtMonto.Text));
                            for (int i = 1; i <= Convert.ToInt32(txtParcialidades.Text); i++)
                            {
                                DataRow row = dt.NewRow();
                                row["id"] = "0";
                                row["idPredio"] = "1";
                                row["fecha"] = fechaPago.ToShortDateString();
                                row["estatus"] = "A";
                                row["noPago"] = i.ToString();
                                row["saldo"] = resto.ToString();
                                if (i < Convert.ToInt32(txtParcialidades.Text))
                                {
                                    parcialidad = Decimal.Round(Convert.ToDecimal(txtMonto.Text) / Convert.ToInt32(txtParcialidades.Text));
                                    row["parcialidad"] = parcialidad.ToString();
                                    total = total + parcialidad;
                                }
                                else
                                    row["parcialidad"] = Decimal.Round(Convert.ToDecimal(txtMonto.Text) - total);
                                if (i == 1 || chkRecargo.Checked)
                                    row["recargo"] = "0";
                                else
                                    row["recargo"] = Decimal.Round(resto * Convert.ToDecimal(txtRecargo.Text) / 100).ToString();
                                resto = resto - parcialidad;
                                row["mensualidad"] = Decimal.Round(Convert.ToDecimal(row["parcialidad"].ToString()) + Convert.ToDecimal(row["recargo"].ToString()));
                                row["pago"] = false;
                                dt.Rows.Add(row);

                                /*Se valida que la fecha de pago no caiga en día feriado, sabado o domingo*/
                                //bLimit = false;
                                fechaPago = fechaPago.AddDays(60);
                                while (fechaValida(fechaPago))
                                {
                                    fechaPago = fechaPago.AddDays(1);
                                    //if (fechaPago == Convert.ToDateTime(txtFechaFinal.Text).Date)
                                    //    fechaPago = fechaPago.AddDays(-1);
                                    //else
                                    //    fechaPago = fechaPago.AddDays(1);
                                }
                            }
                            grdPagos.DataSource = dt;
                            grdPagos.DataBind();
                            txtClvCastatral.Enabled = false;
                            txtFechaFinal.Enabled = false;
                            txtFechaInicial.Enabled = false;
                            txtParcialidades.Enabled = false;
                            btnCalcular.Visible = false;
                            btnCancelar.Visible = false;
                            btnGuardar.Visible = true;
                            btnAtras.Visible = true;
                            btnModificar.Visible = false;
                            btnFinalizar.Visible = false;
                            //if (hdfId.Value != "")
                            //    btnFinalizar.Visible = true;
                            btnCancel.Visible = false;
                            grdPagos.Visible = true;
                            calcularSumas();

                        }
                    }
                }
            }
        }

        protected Boolean fechaValida(DateTime fecha)
        {
            cDiaFestivoBL festivoBL = new cDiaFestivoBL();
            //if (fecha.DayOfWeek == DayOfWeek.Saturday)
            //    return true;
            //else
            if (fecha.DayOfWeek == DayOfWeek.Sunday)
                return true;
            else if (festivoBL.GetFilter("Fecha", fecha.ToShortDateString(), "true", "Fecha", "asc").Count > 0)
                return true;
            else
                return false;
        }

        protected DataTable crearDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("idPredio");
            dt.Columns.Add("fecha");
            dt.Columns.Add("estatus");
            dt.Columns.Add("noPago");
            dt.Columns.Add("saldo", typeof(Decimal));
            dt.Columns.Add("parcialidad", typeof(Decimal));
            dt.Columns.Add("recargo", typeof(Decimal));
            dt.Columns.Add("pagoTardio", typeof(Decimal));
            dt.Columns.Add("mensualidad", typeof(Decimal));
            dt.Columns.Add("pago", typeof(Boolean));
            return dt;

        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            btnCalcular.Visible = true;
            //btnCancelar.Visible = true;
            btnGuardar.Visible = false;
            btnAtras.Visible = false;
            btnModificar.Visible = false;
            btnFinalizar.Visible = false;
            btnImprime.Visible = false;
            txtClvCastatral.Enabled = false;
            txtFechaFinal.Enabled = true;
            txtFechaInicial.Enabled = true;
            txtParcialidades.Enabled = true;
            txtTelefono.Enabled = true;
            txtCelular.Enabled = true;
            txtNoIdentificacion.Enabled = true;
            txtEmail.Enabled = true;
            //chkRecargo.Enabled = true;
            grdPagos.Visible = false;
            btnCancel.Visible = true;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /*Se valida que las fechas de los pagos no excedan la fecha final del convenio*/
            String strFilas = validarFechasPagos();
            if (!String.IsNullOrEmpty(strFilas))
                vtnModal.ShowPopup("Los siguientes pagos se encuentran fuera de las fechas del convenio. " + Environment.NewLine + "Números de pago: " + strFilas.Substring(0, strFilas.Length - 2) + ".", ModalPopupMensaje.TypeMesssage.Error);
            else
            {
                /*Decimal total = sumarParcialidades();
                if (total != Convert.ToDecimal(txtMonto.Text))
                    vtnModal.ShowPopup("La suma de pagos no cuadra con el monto total del convenio. Diferencia: " + (Convert.ToDecimal(txtMonto.Text) - total).ToString() + ".", ModalPopupMensaje.TypeMesssage.Error);
                else
                {*/

                string convDesglosado = new cParametroSistemaBL().GetValorByClave("COBROCONVENIODESGLOSADO");
                if (convDesglosado == "")
                    convDesglosado = "NO";
                Int32 idConvenio = 0;
                cUsuarios U = new cUsuarios();
                U = (cUsuarios)Session["usuario"];
                if (hdfId.Value == "")
                {//Ingreso
                    using (TransactionScope scope = new TransactionScope())
                    {
                        tConvenioEdoCta edoCta = new tConvenioEdoCtaBL().GetByConstraint(Convert.ToInt32(hdfIdCovenioEdoCta.Value));

                        tConvenio convenio = new tConvenio();
                        convenio.IdConvenioEdoCta = Convert.ToInt32(hdfIdCovenioEdoCta.Value);
                        convenio.NoParcialidades = Convert.ToInt32(txtParcialidades.Text);
                        convenio.Folio = new tConvenioBL().MaxFolioConvenio().ToString().PadLeft(3, '0');  // txtFolio.Text;
                        convenio.PorcentajeConvenio = Convert.ToDecimal(txtRecargo.Text);
                        convenio.FechaFin = Convert.ToDateTime(txtFechaFinal.Text);
                        convenio.FechaIni = Convert.ToDateTime(txtFechaInicial.Text);
                        convenio.ImporteTotal = Decimal.Round(Convert.ToDecimal(txtMonto.Text));
                        convenio.Status = "A";
                        convenio.Activo = true;
                        convenio.FechaModificacion = DateTime.Today;
                        convenio.IdUsuario = U.Id;
                        convenio.ConDetalle = convDesglosado == "NO" ? "N" : "S";  //S si- N no                        
                        convenio.Email = txtEmail.Text == "" ? "-----" : txtEmail.Text;
                        convenio.Telefono = txtTelefono.Text == "" ? "-----" : txtTelefono.Text;
                        convenio.Celular = txtCelular.Text == "" ? "-----" : txtCelular.Text;
                        convenio.NoIdentificacion = txtNoIdentificacion.Text;

                        MensajesInterfaz result = new tConvenioBL().Insert(convenio);

                        if (result == MensajesInterfaz.Ingreso)
                        {
                            #region ingreso
                            tTramite tramite;
                            Int32 fila = 0;
                            Boolean guardar = true;
                            while (fila < grdPagos.Rows.Count && guardar)
                            {
                                try
                                {
                                    tramite = new tTramite();
                                    tramite.IdConvenio = convenio.Id;
                                    tramite.IdPredio = Convert.ToInt32(hdfIdPredio.Value);
                                    tramite.Fecha = Convert.ToDateTime(((System.Web.UI.WebControls.TextBox)grdPagos.Rows[fila].FindControl("txtFecha")).Text);
                                    if (((System.Web.UI.WebControls.CheckBox)grdPagos.Rows[fila].FindControl("chkPago")).Checked)
                                        tramite.Status = "P";
                                    else
                                        tramite.Status = "A";
                                    tramite.IdTipoTramite = new cTipoTramiteBL().GetFilter("Tramite", "Convenio", "true", "Tramite", "asc").FirstOrDefault().Id;
                                    tramite.NoParcialidad = Convert.ToInt32(grdPagos.Rows[fila].Cells[0].Text);
                                    tramite.Mensualidad = Decimal.Round(Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdPagos.Rows[fila].FindControl("txtParcialidad")).Text));
                                    tramite.Interes = Decimal.Round(Convert.ToDecimal(((System.Web.UI.WebControls.TextBox)grdPagos.Rows[fila].FindControl("txtRecargo")).Text));
                                    //tramite.Interes = Convert.ToDecimal(grdPagos.Rows[fila].Cells[4].Text);
                                    tramite.Observacion = convDesglosado == "NO" ? "" : "DESGLOSADO"; ;
                                    tramite.Activo = true;
                                    tramite.IdUsuario = U.Id;
                                    tramite.FechaModificacion = DateTime.Today;
                                    idConvenio = convenio.Id;

                                    result = new tTramiteBL().Insert(tramite);
                                    if (result == MensajesInterfaz.Ingreso)
                                    {
                                        if (convDesglosado == "SI")
                                        {//guarda tTramiteDetalles si en cParametrosSistema = SI
                                            MensajesInterfaz msg = GuardaDetalle(edoCta, tramite, fila, edoCta.cPredio.ClavePredial, U, "ingreso");
                                            if (msg != MensajesInterfaz.Ingreso)
                                                guardar = false;
                                        }
                                        fila = fila + 1;
                                    }
                                    else
                                    {
                                        guardar = false;
                                    }

                                }

                                catch (Exception ex)
                                {
                                    new Utileria().logError("Convenio, btnGuardaConvenio", ex);
                                    guardar = false;
                                    result = MensajesInterfaz.ErrorGuardar;
                                }
                            }
                            if (guardar)
                            {
                                List<tConvenioEdoCta> Predios = new tConvenioEdoCtaBL().GetByClavePredial(txtClvCastatral.Text).Where(p => p.Status == "A").ToList();
                                if (Predios != null)
                                {
                                    fila = 0;
                                    //while (fila < Predios.Count && guardar)
                                    //{
                                    //    tConvenioEdoCta predio = Predios[fila];
                                    //    predio.Status = "P";
                                    //    result = new tConvenioEdoCtaBL().Update(predio);
                                    //    if (result == MensajesInterfaz.Actualizacion)
                                    //    {
                                    //        fila = fila + 1;
                                    //        result = MensajesInterfaz.Ingreso;
                                    //    }
                                    //    else
                                    //        guardar = false;
                                    //}
                                    hdfId.Value = convenio.Id.ToString();
                                    btnFinalizar.Visible = false;
                                    scope.Complete();


                                }
                                else
                                    result = MensajesInterfaz.ErrorGuardar;
                                //llenarGrid();
                            }




                            vtnModal.ShowPopup(new Utileria().GetDescription(result), ModalPopupMensaje.TypeMesssage.Alert);
                        }
                        else
                        {//si ocurror debe notificar
                            vtnModal.ShowPopup(new Utileria().GetDescription(result), ModalPopupMensaje.TypeMesssage.Alert);
                        }
                        #endregion ingreso
                    }
                    //tConvenio conv = new tConvenioBL().GetByConstraint(idConvenio);
                    //conv.Folio = new tConvenioBL().MaxFolioConvenio().ToString();
                    //MensajesInterfaz res = new tConvenioBL().Update(conv);
                    ////if (result == MensajesInterfaz.Actualizacion)
                    ////    guardar = true;


                }
                else
                { //Actualización
                    using (TransactionScope scope = new TransactionScope())
                    {
                        string claveCatastral = string.Empty;
                        tConvenioEdoCta edoCta = new tConvenioEdoCtaBL().GetByConstraint(Convert.ToInt32(hdfIdCovenioEdoCta.Value));
                        claveCatastral = edoCta.cPredio.ClavePredial.Trim();

                        tConvenio convenio = new tConvenioBL().GetByConstraint(Convert.ToInt32(hdfId.Value));
                        convenio.IdConvenioEdoCta = Convert.ToInt32(hdfIdCovenioEdoCta.Value);
                        convenio.NoParcialidades = Convert.ToInt32(txtParcialidades.Text);
                        convenio.PorcentajeConvenio = Convert.ToDecimal(txtRecargo.Text);
                        convenio.FechaFin = Convert.ToDateTime(txtFechaFinal.Text);
                        convenio.FechaIni = Convert.ToDateTime(txtFechaInicial.Text);
                        convenio.ImporteTotal = Decimal.Round(Convert.ToDecimal(txtMonto.Text));
                        convenio.Folio = txtFolio.Text;
                        convenio.Activo = true;
                        convenio.FechaModificacion = DateTime.Today;
                        convenio.IdUsuario = U.Id;
                        convenio.Email = txtEmail.Text == "" ? "-----" : txtEmail.Text;
                        convenio.Telefono = txtTelefono.Text == "" ? "-----" : txtTelefono.Text;
                        convenio.Celular = txtCelular.Text == "" ? "-----" : txtCelular.Text;
                        convenio.NoIdentificacion = txtNoIdentificacion.Text;
                        convenio.ConDetalle = "S";  //S si- N no

                        MensajesInterfaz result = new tConvenioBL().Update(convenio);
                        if (result == MensajesInterfaz.Actualizacion)
                        {
                            #region Actualizacion

                            tTramite tramite;
                            Int32 fila = 0;
                            Boolean guardar = true;
                            Boolean existe;
                            Int32 noRegistro = new tTramiteBL().GetFilter("IdConvenio", hdfId.Value, "true", "NoParcialidad", "asc").Count;
                            while (fila < grdPagos.Rows.Count && guardar)
                            {
                                try
                                {
                                    existe = true;
                                    tramite = new tTramiteBL().GetByNoPago(Convert.ToInt32(hdfId.Value), Convert.ToInt32(grdPagos.Rows[fila].Cells[0].Text));
                                    if (tramite == null)
                                    {
                                        tramite = new tTramite();
                                        existe = false;
                                    }

                                    tramite.IdConvenio = convenio.Id;
                                    tramite.IdPredio = Convert.ToInt32(hdfIdPredio.Value);
                                    tramite.Fecha = Convert.ToDateTime(((TextBox)grdPagos.Rows[fila].FindControl("txtFecha")).Text);
                                    if (((System.Web.UI.WebControls.CheckBox)grdPagos.Rows[fila].FindControl("chkPago")).Checked)
                                        tramite.Status = "P";
                                    else
                                        tramite.Status = "A";
                                    tramite.IdTipoTramite = new cTipoTramiteBL().GetFilter("Tramite", "Convenio", "true", "Tramite", "asc").FirstOrDefault().Id;
                                    tramite.NoParcialidad = Convert.ToInt32(grdPagos.Rows[fila].Cells[0].Text);
                                    tramite.Mensualidad = Decimal.Round(Convert.ToDecimal(((TextBox)grdPagos.Rows[fila].FindControl("txtParcialidad")).Text));
                                    tramite.Interes = Convert.ToDecimal(((TextBox)grdPagos.Rows[fila].FindControl("txtRecargo")).Text);
                                    tramite.Activo = true;
                                    tramite.IdUsuario = U.Id;
                                    tramite.FechaModificacion = DateTime.Now;

                                    if (existe)
                                    {
                                        result = new tTramiteBL().Update(tramite);
                                        if (!(result == MensajesInterfaz.Actualizacion))
                                            guardar = false;

                                        if (convDesglosado == "SI")
                                        {//guarda tTramiteDetalles si en cParametrosSistema = SI
                                            MensajesInterfaz msg = GuardaDetalle(edoCta, tramite, fila, claveCatastral, U, "actualiza");
                                            if (msg != MensajesInterfaz.Ingreso)
                                                guardar = false;
                                        }

                                    }
                                    else
                                    {
                                        result = new tTramiteBL().Insert(tramite);
                                        if (!(result == MensajesInterfaz.Ingreso))
                                            guardar = false;
                                        if (convDesglosado == "SI")
                                        {//guarda tTramiteDetalles si en cParametrosSistema = SI
                                            MensajesInterfaz msg = GuardaDetalle(edoCta, tramite, fila, claveCatastral, U, "ingreso");
                                            if (msg != MensajesInterfaz.Ingreso)
                                                guardar = false;
                                        }
                                    }
                                    fila = fila + 1;
                                }
                                catch (Exception ex)
                                {
                                    new Utileria().logError("convenio, btnGuardar", ex);
                                    guardar = false;
                                    result = MensajesInterfaz.ErrorActualizar;
                                }
                            }
                            if (guardar)
                            {
                                if (Convert.ToInt32(txtParcialidades.Text) < noRegistro)
                                {
                                    while (fila < noRegistro && guardar)
                                    {
                                        existe = true;
                                        tramite = new tTramiteBL().GetByNoPago(Convert.ToInt32(hdfId.Value), fila + 1);

                                        tramite.Activo = false;
                                        tramite.IdUsuario = U.Id;
                                        tramite.FechaModificacion = DateTime.Today;
                                        fila = fila + 1;
                                        result = new tTramiteBL().Update(tramite);
                                        if (!(result == MensajesInterfaz.Actualizacion))
                                            guardar = false;
                                        else
                                        {
                                            //si todo esta correcto se guarda el tTramiteDetalle
                                            MensajesInterfaz msg = GuardaDetalle(edoCta, tramite, fila, claveCatastral, U, "actualiza");
                                            //termina transaccion 
                                            scope.Complete();
                                        }
                                    }
                                }
                                else
                                {
                                    scope.Complete();
                                    btnImprime.Visible = true;
                                    btnModificar.Visible = true;
                                    btnFinalizar.Visible = false;
                                    btnGuardar.Visible = false;

                                }

                            }
                            #endregion actualizacion
                        }
                        btnFinalizar.Visible = false;
                        vtnModal.ShowPopup(new Utileria().GetDescription(result), ModalPopupMensaje.TypeMesssage.Alert);
                    }
                }
                //}
            }

        }

        protected String validarFechasPagos()
        {
            String strFilas = "";
            DateTime fechaPago;
            for (int i = 1; i <= grdPagos.Rows.Count; i++)
            {
                fechaPago = Convert.ToDateTime(((TextBox)grdPagos.Rows[i - 1].FindControl("txtFecha")).Text);
                if (fechaPago > Convert.ToDateTime(txtFechaFinal.Text) || fechaPago < Convert.ToDateTime(txtFechaInicial.Text))
                    strFilas = strFilas + i.ToString() + ", ";
            }
            return strFilas;
        }

        protected Decimal sumarParcialidades()
        {
            Decimal total = 0, parcialidad;
            for (int i = 1; i <= grdPagos.Rows.Count; i++)
            {
                parcialidad = Convert.ToDecimal(((TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad")).Text);
                total = total + parcialidad;
            }
            return total;
        }

        protected void txtFecha_TextChanged(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.TextBox txtFecha_Temp = (TextBox)sender;
            System.Web.UI.WebControls.TableCell tableCell = (System.Web.UI.WebControls.TableCell)txtFecha_Temp.Parent;
            GridViewRow row = (GridViewRow)tableCell.Parent;
            DateTime fechaPago = Convert.ToDateTime(txtFecha_Temp.Text);
            Boolean bLimit = false;
            while (fechaValida(fechaPago))
            {
                //fechaPago = fechaPago.AddDays(1);
                if (fechaPago == Convert.ToDateTime(txtFechaFinal.Text).Date || bLimit == true)
                {
                    fechaPago = fechaPago.AddDays(-1);
                    bLimit = true;
                }
                else
                    fechaPago = fechaPago.AddDays(1);
            }
            txtFecha_Temp.Text = fechaPago.ToShortDateString();
        }

        protected void llenarGrid()
        {
            DataTable dt = crearDataTable();
            List<tTramite> tramites = new tTramiteBL().GetFilter("IdConvenio", hdfId.Value, "true", "NoParcialidad", "asc");
            Decimal resto = Decimal.Round(Convert.ToDecimal(txtMonto.Text));
            foreach (tTramite tramite in tramites)
            {
                DataRow row = dt.NewRow();
                row["id"] = tramite.Id.ToString();
                row["idPredio"] = tramite.IdPredio.ToString();
                row["fecha"] = tramite.Fecha.ToShortDateString();
                row["estatus"] = tramite.Status;
                row["noPago"] = tramite.NoParcialidad.ToString();
                row["saldo"] = Decimal.Round(resto).ToString();
                row["parcialidad"] = Decimal.Round(tramite.Mensualidad.HasValue ? tramite.Mensualidad.Value : 0).ToString();
                row["recargo"] = Decimal.Round(tramite.Interes.HasValue ? tramite.Interes.Value : 0).ToString();
                row["pagoTardio"] = tramite.Status == "P" ? 0 : InteresPagoTardio(tramite.Id, tramite.Fecha);
                resto = resto - Convert.ToDecimal(tramite.Mensualidad);
                row["mensualidad"] = Decimal.Round(((tramite.Mensualidad.HasValue ? tramite.Mensualidad.Value : 0) + (tramite.Interes.HasValue ? tramite.Interes.Value : 0))).ToString();
                row["pago"] = tramite.Status == "P" ? true : false;
                dt.Rows.Add(row);
            }
            grdPagos.DataSource = dt;
            grdPagos.DataBind();
            calcularSumas();
            // return tramites[0].Status;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("BuscarConvenio.aspx?Clave=");
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            Response.Redirect("BuscarConvenio.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtClvCastatral.Enabled = false;
            txtFechaFinal.Enabled = false;
            txtFechaInicial.Enabled = false;
            txtParcialidades.Enabled = false;
            btnCalcular.Visible = false;
            btnCancelar.Visible = false;
            btnCancel.Visible = false;
            btnGuardar.Visible = true;
            btnAtras.Visible = true;
            if (hdfId.Value != "")
                btnFinalizar.Visible = true;
            btnModificar.Visible = true;
            grdPagos.Visible = true;
        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            vtnModal.DysplayCancelar = true;
            vtnModal.ShowPopup("¿Desea finalizar el Convenio?", ModalPopupMensaje.TypeMesssage.Confirm);
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            if (vtnModal.Mensaje == "¿Desea finalizar el Convenio?")
            {
                cUsuarios U = new cUsuarios();
                U = (cUsuarios)Session["usuario"];
                using (TransactionScope scope = new TransactionScope())
                {
                    Int32 fila = 0;
                    Boolean guardar = true;
                    MensajesInterfaz result = MensajesInterfaz.Actualizacion;
                    tConvenio convenio = new tConvenioBL().GetByConstraint(Convert.ToInt32(hdfId.Value));
                    convenio.Status = "C";
                    convenio.IdUsuario = U.Id;
                    convenio.Activo = false;
                    convenio.FechaModificacion = DateTime.Now;
                    result = new tConvenioBL().Update(convenio);
                    if (result == MensajesInterfaz.Actualizacion)
                    {
                        List<tTramite> tramites = new tTramiteBL().GetFilter("IdConvenio", hdfId.Value, "true", "NoParcialidad", "asc").ToList();
                        while (fila < tramites.Count && guardar)
                        {
                            try
                            {
                                if (((System.Web.UI.WebControls.CheckBox)grdPagos.Rows[Convert.ToInt32(tramites[fila].NoParcialidad) - 1].FindControl("chkPago")).Checked)
                                    tramites[fila].Status = "P";
                                else
                                    tramites[fila].Status = "C";
                                tramites[fila].IdUsuario = U.Id;
                                tramites[fila].FechaModificacion = DateTime.Today;

                                result = new tTramiteBL().Update(tramites[fila]);
                                fila = fila + 1;
                                if (!(result == MensajesInterfaz.Actualizacion))
                                    guardar = false;

                                if (guardar == true)
                                {
                                    //actualizar estado de cuenta
                                    tConvenioEdoCta tc = new tConvenioEdoCtaBL().GetByConstraint(convenio.IdConvenioEdoCta);
                                    if (tc != null)
                                    {
                                        tc.Status = "C";
                                        tc.Activo = false;
                                        tc.FechaModificacion = DateTime.Now;
                                        tc.IdUsuario = U.Id;
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                new Utileria().logError("Convenio, vtnModal", ex);
                                guardar = false;
                                result = MensajesInterfaz.ErrorActualizar;
                            }
                        }
                        if (guardar)
                        {
                            vtnModal.ShowPopup(new Utileria().GetDescription(result) + " Convenio Finalizado.", ModalPopupMensaje.TypeMesssage.Alert);
                            scope.Complete();
                            grdPagos.Enabled = false;
                            btnGuardar.Visible = false;
                            btnModificar.Visible = false;
                            btnFinalizar.Visible = false;
                        }
                        else
                            vtnModal.ShowPopup(new Utileria().GetDescription(result) + " No se ha podido Finalizar el Convenio.", ModalPopupMensaje.TypeMesssage.Alert);
                    }
                    else
                        vtnModal.ShowPopup(new Utileria().GetDescription(result) + " No se ha podido Finalizar el Convenio.", ModalPopupMensaje.TypeMesssage.Alert);
                }

            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Buscar(true);
        }

        protected void Buscar(Boolean isNew, Int32 idConvenio)
        {
            if (txtClvCastatral.Text.Length == 12)
            {
                List<tConvenioEdoCta> Predios = new List<tConvenioEdoCta>();

                if (isNew)
                    Predios = new tConvenioEdoCtaBL().GetByClavePredialByMonth(txtClvCastatral.Text, idConvenio).Where(p => p.Status == "A").Count() > 0 ? new tConvenioEdoCtaBL().GetByClavePredialByMonth(txtClvCastatral.Text, idConvenio).Where(p => p.Status == "A").ToList() : null;
                else
                    Predios = new tConvenioEdoCtaBL().GetByClavePredialByMonth(txtClvCastatral.Text, idConvenio).Count() > 0 ? new tConvenioEdoCtaBL().GetByClavePredialByMonth(txtClvCastatral.Text, idConvenio).ToList() : null;


                if (Predios != null)
                {
                    txtNombreAdquiriente.Text = Predios[0].cPredio.cContribuyente.ApellidoPaterno + " " + Predios[0].cPredio.cContribuyente.ApellidoMaterno + " " + Predios[0].cPredio.cContribuyente.Nombre;
                    //txtMonto.Text = Decimal.Round(Predio.Importe).ToString();
                    //txtTipoImpuesto.Text = Predio.TipoImpuesto == "IP" ? "Impuesto Predial" : Predio.TipoImpuesto == "SM" ? "Servicios Municipales" : "";
                    hdfIdCovenioEdoCta.Value = Predios[0].Folio.ToString();
                    hdfIdPredio.Value = Predios[0].cPredio.Id.ToString();
                    Decimal total = 0;
                    String strTipoImpuesto = "";
                    foreach (tConvenioEdoCta predio in Predios)
                    {
                        total = total + predio.Importe;
                        if (strTipoImpuesto == "")
                            strTipoImpuesto = predio.TipoImpuesto;
                        else if (strTipoImpuesto != predio.TipoImpuesto)
                            strTipoImpuesto = "A";
                    }
                    txtMonto.Text = Decimal.Round(total).ToString();
                    txtTipoImpuesto.Text = strTipoImpuesto == "IP" ? "Impuesto Predial" : strTipoImpuesto == "SM" ? "Servicios Municipales" : strTipoImpuesto == "A" ? "Ambos" : "";
                }
                else
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.PredioInexistente), ModalPopupMensaje.TypeMesssage.Alert);
                    txtClvCastatral.Text = "";
                    txtNombreAdquiriente.Text = "";
                    txtMonto.Text = "";
                    txtTipoImpuesto.Text = "";
                    hdfIdCovenioEdoCta.Value = "";
                }
            }
            else
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.FormatoPredio), ModalPopupMensaje.TypeMesssage.Alert);
                txtClvCastatral.Text = "";
            }

        }

        protected void calculoRecargo(Boolean bBand)
        {
            if (grdPagos.Rows.Count != 0)
            {
                if (bBand)
                {
                    for (int i = 2; i <= grdPagos.Rows.Count; i++)
                    {
                        if (grdPagos.DataKeys[i - 1].Values["estatus"].ToString() != "P")
                        {
                            grdPagos.Rows[i - 1].Cells[4].Text = "0";
                            TextBox txt = (TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad");
                            decimal rec = Utileria.Redondeo(Convert.ToDecimal(txt.Text));
                            grdPagos.Rows[i - 1].Cells[6].Text = rec.ToString();

                        }

                    }
                }
                else
                {
                    for (int i = 2; i <= grdPagos.Rows.Count; i++)
                    {
                        if (grdPagos.DataKeys[i - 1].Values["estatus"].ToString() != "P")
                        {
                            decimal rec = Convert.ToDecimal(grdPagos.Rows[i - 1].Cells[2].Text);
                            grdPagos.Rows[i - 1].Cells[4].Text = Utileria.Redondeo(Convert.ToDecimal(grdPagos.Rows[i - 1].Cells[2].Text) * Convert.ToDecimal(txtRecargo.Text) / 100).ToString();
                            TextBox txt = (TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad");
                            grdPagos.Rows[i - 1].Cells[6].Text = Decimal.Round(Convert.ToDecimal(txt.Text) + Convert.ToDecimal(grdPagos.Rows[i - 1].Cells[4].Text)).ToString();

                        }
                    }

                }
            }

        }

        protected void chkRecargo_CheckedChanged(object sender, EventArgs e)
        {
            calculoRecargo(chkRecargo.Checked);
            calcularSumas();
        }

        protected void calcularSumas()
        {

            Decimal parcialidad = 0, recargo = 0, tardio = 0, reca = 0;

            foreach (GridViewRow row in grdPagos.Rows)
            {
                parcialidad += Utileria.Redondeo(Convert.ToDecimal(((TextBox)row.FindControl("txtParcialidad")).Text));
                //string rec = (row.Cells[4].Text).ToString();
                string rec = (((TextBox)row.FindControl("txtRecargo")).Text);
                if (rec == null || rec == "")
                    reca = 0;
                else
                    reca = Convert.ToDecimal(rec);
                //recargo = recargo + Utileria.Redondeo(   (((TextBox)row.FindControl("txtRecargo")).Text)     );
                recargo += Utileria.Redondeo(reca);
                string tar = (row.Cells[5].Text).ToString();
                if (tar != "&nbsp;")
                    tardio = tardio + Utileria.Redondeo(Convert.ToDecimal(tar));

                //mensualidad +=  parcialidad + recargo +  tardio;// Convert.ToDecimal(row.Cells[5].Text);

            }
            lblTparcialidad.Text = parcialidad.ToString("N0");
            lblTRecargo.Text = recargo.ToString("N0");
            lblPagoTardio.Text = tardio.ToString("N0");
            lblTMensualidad.Text = (parcialidad + recargo + tardio).ToString("C");// mensualidad.ToString("N0");
        }

        protected void txtParcialidad_TextChanged(object sender, EventArgs e)
        {
            //Decimal resto = Convert.ToDecimal(txtMonto.Text), total = 0;
            //Decimal parcialidad = 0;
            //TextBox txt = (TextBox)sender;

            //GridViewRow row = (GridViewRow)txt.Parent.Parent;
            //int indice = row.RowIndex;
            //for (int i = 1; i <= grdPagos.Rows.Count; i++)
            //{

            //    if ((i - 1) <= indice)
            //    {
            //        resto -= Convert.ToDecimal(((TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad")).Text);
            //        if ((i - 1) == indice)
            //            grdPagos.Rows[i - 1].Cells[5].Text = Decimal.Round(Convert.ToDecimal(((TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad")).Text) + Convert.ToDecimal(((TextBox)grdPagos.Rows[i - 1].FindControl("txtRecargo")).Text)).ToString();
            //        //grdPagos.Rows[i - 1].Cells[5].Text = Decimal.Round(Convert.ToDecimal(((TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad")).Text) + Convert.ToDecimal(grdPagos.Rows[i - 1].Cells[4].Text)).ToString();
            //    }
            //    else
            //    {
            //        grdPagos.Rows[i - 1].Cells[2].Text = Decimal.Round((resto - total)).ToString();
            //        //parcialidad = Decimal.Round(Convert.ToDecimal(((TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad")).Text));
            //        if (i < Convert.ToInt32(txtParcialidades.Text))
            //        {
            //            parcialidad = Decimal.Round(resto / (Convert.ToInt32(txtParcialidades.Text) - (indice + 1)));
            //            ((TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad")).Text = parcialidad.ToString();
            //            total = total + parcialidad;
            //        }
            //        else
            //        {
            //            parcialidad = Decimal.Round(resto - total);
            //            ((TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad")).Text = Decimal.Round(parcialidad).ToString();
            //        }
            //        if (i == 1 || chkRecargo.Checked)
            //            ((TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad")).Text = "0";
            //        //grdPagos.Rows[i - 1].Cells[4].Text = "0";
            //        else
            //            ((TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad")).Text = Decimal.Round(Convert.ToDecimal(grdPagos.Rows[i - 1].Cells[2].Text) * Convert.ToDecimal(txtRecargo.Text) / 100).ToString();
            //        //grdPagos.Rows[i - 1].Cells[4].Text = Decimal.Round(Convert.ToDecimal(grdPagos.Rows[i - 1].Cells[2].Text) * Convert.ToDecimal(txtRecargo.Text) / 100).ToString();

            //        //resto = resto - parcialidad;                
            //        grdPagos.Rows[i - 1].Cells[5].Text = Decimal.Round(parcialidad + Convert.ToDecimal(((TextBox)grdPagos.Rows[i - 1].FindControl("txtRecargo")).Text)).ToString();
            //        //grdPagos.Rows[i - 1].Cells[5].Text = Decimal.Round(parcialidad + Convert.ToDecimal(grdPagos.Rows[i - 1].Cells[4].Text)).ToString();

            //    }
            //}
            //calcularSumas();
            Decimal resto = Convert.ToDecimal(txtMonto.Text), total = 0;
            Decimal parcialidad = 0;
            TextBox txt = (TextBox)sender;

            GridViewRow row = (GridViewRow)txt.Parent.Parent;
            int indice = row.RowIndex;
            for (int i = 1; i <= grdPagos.Rows.Count; i++)
            {

                if ((i - 1) <= indice)
                {
                    resto -= Convert.ToDecimal(((TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad")).Text);
                    decimal tardio = 0;
                    if ((i - 1) == indice)
                    {
                        string tar = grdPagos.Rows[i - 1].Cells[5].Text;
                        if (tar != "&nbsp;" && tar != "")
                            tardio = tardio + Utileria.Redondeo(Convert.ToDecimal(tar));

                        grdPagos.Rows[i - 1].Cells[6].Text = Decimal.Round(Convert.ToDecimal(((TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad")).Text) + Decimal.Round(Convert.ToDecimal(((TextBox)grdPagos.Rows[i - 1].FindControl("txtRecargo")).Text)) +
                                                             tardio).ToString("N0");
                    }
                }
                else
                {
                    decimal tardio = 0;
                    grdPagos.Rows[i - 1].Cells[2].Text = Decimal.Round((resto - total)).ToString("N0");
                    //parcialidad = Decimal.Round(Convert.ToDecimal(((TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad")).Text));
                    if (i < Convert.ToInt32(txtParcialidades.Text))
                    {
                        parcialidad = Decimal.Round(resto / (Convert.ToInt32(txtParcialidades.Text) - (indice + 1)));
                        ((TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad")).Text = parcialidad.ToString("N0");
                        total = total + parcialidad;
                    }
                    else
                    {
                        parcialidad = Decimal.Round(resto - total);
                        ((TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad")).Text = Decimal.Round(parcialidad).ToString("N0");
                    }
                    if (i == 1 || chkRecargo.Checked)
                        ((TextBox)grdPagos.Rows[i - 1].FindControl("txtRecargo")).Text = "0";
                    else
                        ((TextBox)grdPagos.Rows[i - 1].FindControl("txtRecargo")).Text = Decimal.Round(Convert.ToDecimal(grdPagos.Rows[i - 1].Cells[2].Text) * Convert.ToDecimal(txtRecargo.Text) / 100).ToString("N0");
                    //resto = resto - parcialidad;
                    string tar = grdPagos.Rows[i - 1].Cells[5].Text;
                    if (tar != "&nbsp;" && tar != "")
                        tardio = tardio + Utileria.Redondeo(Convert.ToDecimal(tar));

                    grdPagos.Rows[i - 1].Cells[6].Text = Decimal.Round(parcialidad + Convert.ToDecimal(((TextBox)grdPagos.Rows[i - 1].FindControl("txtRecargo")).Text) +
                                                         tardio).ToString("N0");
                }
            }
            calcularSumas();
        }

        protected void txtRecargo_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txt.Parent.Parent;

            row.Cells[5].Text = Decimal.Round(Convert.ToDecimal(((TextBox)row.FindControl("txtParcialidad")).Text) + Convert.ToDecimal(txt.Text)).ToString("N0");
            //Decimal resto = Convert.ToDecimal(txtMonto.Text), total = 0;
            //Decimal parcialidad = 0;
            //TextBox txt = (TextBox)sender;

            //GridViewRow row = (GridViewRow)txt.Parent.Parent;
            //int indice = row.RowIndex;
            //for (int i = 1; i <= grdPagos.Rows.Count; i++)
            //{

            //    if ((i - 1) <= indice)
            //    {
            //        resto -= Convert.ToDecimal(((TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad")).Text);
            //        if ((i - 1) == indice)
            //            grdPagos.Rows[i - 1].Cells[5].Text = Decimal.Round(Convert.ToDecimal(((TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad")).Text) + Convert.ToDecimal(((TextBox)grdPagos.Rows[i - 1].FindControl("txtRecargo")).Text)).ToString();
            //        //grdPagos.Rows[i - 1].Cells[5].Text = Decimal.Round(Convert.ToDecimal(((TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad")).Text) + Convert.ToDecimal(grdPagos.Rows[i - 1].Cells[4].Text)).ToString();
            //    }
            //    else
            //    {
            //        grdPagos.Rows[i - 1].Cells[2].Text = Decimal.Round((resto - total)).ToString();
            //        //parcialidad = Decimal.Round(Convert.ToDecimal(((TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad")).Text));
            //        if (i < Convert.ToInt32(txtParcialidades.Text))
            //        {
            //            parcialidad = Decimal.Round(resto / (Convert.ToInt32(txtParcialidades.Text) - (indice + 1)));
            //            ((TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad")).Text = parcialidad.ToString();
            //            total = total + parcialidad;
            //        }
            //        else
            //        {
            //            parcialidad = Decimal.Round(resto - total);
            //            ((TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad")).Text = Decimal.Round(parcialidad).ToString();
            //        }
            //        if (i == 1 || chkRecargo.Checked)
            //            ((TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad")).Text = "0";
            //        //grdPagos.Rows[i - 1].Cells[4].Text = "0";
            //        else
            //            ((TextBox)grdPagos.Rows[i - 1].FindControl("txtParcialidad")).Text = Decimal.Round(Convert.ToDecimal(grdPagos.Rows[i - 1].Cells[2].Text) * Convert.ToDecimal(txtRecargo.Text) / 100).ToString();
            //        //grdPagos.Rows[i - 1].Cells[4].Text = Decimal.Round(Convert.ToDecimal(grdPagos.Rows[i - 1].Cells[2].Text) * Convert.ToDecimal(txtRecargo.Text) / 100).ToString();

            //        //resto = resto - parcialidad;                
            //        grdPagos.Rows[i - 1].Cells[5].Text = Decimal.Round(parcialidad + Convert.ToDecimal(((TextBox)grdPagos.Rows[i - 1].FindControl("txtRecargo")).Text)).ToString();
            //        //grdPagos.Rows[i - 1].Cells[5].Text = Decimal.Round(parcialidad + Convert.ToDecimal(grdPagos.Rows[i - 1].Cells[4].Text)).ToString();

            //    }
            //}
            //calcularSumas();
        }

        protected void grdPagos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AjaxControlToolkit.CalendarExtender ceFecha = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("ceFecha");
                ceFecha.StartDate = Convert.ToDateTime(txtFechaInicial.Text).Date.AddDays(1);
                ceFecha.EndDate = Convert.ToDateTime(txtFechaFinal.Text).Date.AddDays(1);
                RangeValidator rgvFecha = (RangeValidator)e.Row.FindControl("rgvFecha");
                rgvFecha.MaximumValue = txtFechaFinal.Text;
                rgvFecha.MinimumValue = txtFechaInicial.Text;

            }
        }

        protected MensajesInterfaz GuardaDetalle(tConvenioEdoCta edo, tTramite tramite, int fila, string claveCatastral, cUsuarios u, string ingreso)
        {
            decimal porc = ((Convert.ToDecimal(((TextBox)grdPagos.Rows[fila].FindControl("txtParcialidad")).Text) / Convert.ToDecimal(txtMonto.Text)) * 100) / 100;

            Impuesto i = new Impuesto();
            SaldosC saldos = new SaldosC();
            MensajesInterfaz msg = new MensajesInterfaz();
            int idRef = 0;
            cConcepto con = new cConcepto();

            String convenioCeleb = new cParametroSistemaBL().GetValorByClave("CRI-CELEBRACIONCONVENIO");
            if (convenioCeleb != null)
            {
                con = new cConceptoBL().getConceptoByCriEjercicio(convenioCeleb, DateTime.Today.Year, 3);
                if (con == null)
                    throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
            }


            if (new tTramiteDetalleBL().InactivaPorIdTramite(tramite.Id) == MensajesInterfaz.NoEncontrados)
                return MensajesInterfaz.NoEncontrados;

            if (ingreso == "actualiza")
            {//se inactiva el registro de tTramiteDetalle y postreior se ingresa uno nuevo
                msg = new tTramiteDetalleBL().InactivaPorIdTramite(tramite.Id);
                if (msg != MensajesInterfaz.Actualizacion)
                    throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
            }

            try
            {
                decimal sumatoria = 0;
                Periodo perActual = new Periodo();
                Periodo perRezago = new Periodo();
                int bIni = 0;
                int aIni = 0;
                int bFin = 0;
                int aFin = 0;
                if (edo.PeriodoIP != null)
                {
                    bIni = Convert.ToInt32(edo.PeriodoIP.Substring(0, 1));
                    aIni = Convert.ToInt32(edo.PeriodoIP.Substring(2, 4));
                    bFin = Convert.ToInt32(edo.PeriodoIP.Substring(9, 1));
                    aFin = Convert.ToInt32(edo.PeriodoIP.Substring(11, 4));
                    perActual = new SaldosC().DevuelvePeriodoPago(bIni, aIni, bFin, aFin, "ACTUAL");
                    perRezago = new SaldosC().DevuelvePeriodoPago(bIni, aIni, bFin, aFin, "REZAGO");
                }
                if (perActual != null)
                {
                    i.PerActual = new Periodo();
                    i.PerActual = perActual;
                }
                if (perRezago != null)
                {
                    i.PerRezago = new Periodo();
                    i.PerRezago = perRezago;
                }
                i.ActImpuesto = Utileria.Redondeo(Convert.ToDecimal((edo.Impuesto - edo.DescImpuesto) * porc));
                i.Rezagos = Utileria.Redondeo(Convert.ToDecimal((edo.Rezago - edo.DescRezago) * porc));
                //i.Rezagos = Utileria.Redondeo(Convert.ToDecimal(((edo.Rezago - edo.DescRezago) + (edo.Impuesto - edo.DescImpuesto)) * porc));
                i.RezAdicional = Utileria.Redondeo(Convert.ToDecimal((edo.Adicional - edo.DescAdicional) * porc));
                i.RezRecargo = Utileria.Redondeo(Convert.ToDecimal((edo.Recargo - edo.DescRecargo) * porc));
                i.RezDiferencias = Utileria.Redondeo(Convert.ToDecimal((edo.Diferencia - edo.DescDiferencia) * porc));
                i.RezRecDiferencias = Utileria.Redondeo(Convert.ToDecimal((edo.RecargoDiferencia - edo.DescRecargoDiferencia) * porc));
                i.Honorarios = Utileria.Redondeo(Convert.ToDecimal((edo.Honorarios - edo.DescHonorarios) * porc));
                i.Ejecucion = Utileria.Redondeo(Convert.ToDecimal((edo.Ejecucion - edo.DescEjecucion) * porc));
                i.Multa = Utileria.Redondeo(Convert.ToDecimal((edo.Multa - edo.DescMulta) * porc));

                sumatoria = i.ActImpuesto + i.Rezagos + i.RezAdicional + i.RezRecargo + i.RezDiferencias + i.RezRecDiferencias + i.Honorarios + i.Ejecucion + i.Multa;

                Servicio s = new Servicio();
                if (edo.PeriodoSM != null)
                {
                    bIni = Convert.ToInt32(edo.PeriodoSM.Substring(0, 1));
                    aIni = Convert.ToInt32(edo.PeriodoSM.Substring(2, 4));
                    bFin = Convert.ToInt32(edo.PeriodoSM.Substring(9, 1));
                    aFin = Convert.ToInt32(edo.PeriodoSM.Substring(11, 4));
                    perActual = new SaldosC().DevuelvePeriodoPago(bIni, aIni, bFin, aFin, "ACTUAL");
                    perRezago = new SaldosC().DevuelvePeriodoPago(bIni, aIni, bFin, aFin, "REZAGO");
                }
                if (perActual != null)
                {
                    i.PerActual = new Periodo();
                    i.PerActual = perActual;
                }
                if (perRezago != null)
                {
                    s.PerRezago = new Periodo();
                    s.PerRezago = perRezago;
                }

                s.ActInfraestructura = Utileria.Redondeo(Convert.ToDecimal((edo.Infraestructura - edo.DescInfraestructura) * porc));
                s.ActRecoleccion = Utileria.Redondeo(Convert.ToDecimal((edo.Recoleccion - edo.DescRecoleccion) * porc));
                s.ActLimpieza = Utileria.Redondeo(Convert.ToDecimal((edo.Limpieza - edo.DescLimpieza) * porc));
                s.ActDap = Utileria.Redondeo(Convert.ToDecimal((edo.DAP - edo.DescDAP) * porc));
                //decimal suma = Convert.ToDecimal((edo.Infraestructura - edo.DescInfraestructura) + (edo.Recoleccion - edo.DescRecoleccion) +
                //                (edo.Limpieza - edo.DescLimpieza) + (edo.DAP - edo.DescDAP));
                s.RezRecargo = Utileria.Redondeo(Convert.ToDecimal((edo.RecargoSM - edo.DescRecargoSM) * porc));
                s.RezAdicional = Utileria.Redondeo(Convert.ToDecimal((edo.AdicionalSM - edo.DescAdicionalSM) * porc));
                s.Rezagos = Utileria.Redondeo(Convert.ToDecimal(((edo.RezagoSM - edo.DescRezagoSM)) * porc));
                s.Multa = Utileria.Redondeo(Convert.ToDecimal((edo.MultaSM - edo.DescMultaSM) * porc));
                s.Honorarios = Utileria.Redondeo(Convert.ToDecimal((edo.HonorariosSM - edo.HonorariosSM) * porc));
                s.Ejecucion = Utileria.Redondeo(Convert.ToDecimal((edo.EjecucionSM - edo.DescEjecucionSM) * porc));

                sumatoria += s.ActAdicInfraestructura + s.ActRecoleccion + s.ActLimpieza + s.ActDap + s.RezRecargo + s.RezAdicional +
                                    s.Rezagos + s.Multa + s.Honorarios + s.Ejecucion;

                if (sumatoria > tramite.Mensualidad)
                {
                    sumatoria = sumatoria - Convert.ToDecimal(tramite.Mensualidad);
                    if (i.Rezagos > 0 && i.Rezagos > sumatoria)
                        i.Rezagos = i.Rezagos - sumatoria;
                    else if (i.RezAdicional > 0 && i.RezAdicional > sumatoria)
                        i.RezAdicional = i.RezAdicional - sumatoria;
                    else if (i.RezRecargo > 0 && i.RezRecargo > sumatoria)
                        i.RezRecargo = i.RezRecargo - sumatoria;
                    else if (i.ActImpuesto > 0 && i.ActImpuesto > sumatoria)
                        i.ActImpuesto = i.ActImpuesto - sumatoria;
                    else if (s.Rezagos > 0 && s.Rezagos > sumatoria)
                        s.Rezagos = s.Rezagos - sumatoria;
                    else if (s.RezAdicional > 0 && s.RezAdicional > sumatoria)
                        s.RezAdicional = s.RezAdicional - sumatoria;
                    else if (s.RezRecargo > 0 && s.RezRecargo > sumatoria)
                        s.RezRecargo = s.RezRecargo - sumatoria;
                    else if (s.ActInfraestructura > 0 && s.ActInfraestructura > sumatoria)
                        s.ActInfraestructura = s.ActInfraestructura - sumatoria;
                    else if (s.ActRecoleccion > 0 && s.ActRecoleccion > sumatoria)
                        s.ActRecoleccion = s.ActRecoleccion - sumatoria;
                    else if (s.ActLimpieza > 0 && s.ActLimpieza > sumatoria)
                        s.ActLimpieza = s.ActLimpieza - sumatoria;
                    else if (s.ActDap > 0 && s.ActDap < sumatoria)
                        s.ActDap = s.ActDap - sumatoria;

                }
                else
                {
                    sumatoria = Convert.ToDecimal(tramite.Mensualidad) - sumatoria;
                    if (i.Rezagos > 0 && i.Rezagos > sumatoria)
                        i.Rezagos = i.Rezagos + sumatoria;
                    else if (i.RezAdicional > 0 && i.RezAdicional > sumatoria)
                        i.RezAdicional = i.RezAdicional + sumatoria;
                    else if (i.RezRecargo > 0 && i.RezRecargo > sumatoria)
                        i.RezRecargo = i.RezRecargo + sumatoria;
                    else if (i.ActImpuesto > 0 && i.ActImpuesto > sumatoria)
                        i.ActImpuesto = i.ActImpuesto + sumatoria;
                    else if (s.Rezagos > 0 && s.Rezagos < sumatoria)
                        s.Rezagos = s.Rezagos + sumatoria;
                    else if (s.RezAdicional > 0 && s.RezAdicional < sumatoria)
                        s.RezAdicional = s.RezAdicional + sumatoria;
                    else if (s.RezRecargo > 0 && s.RezRecargo < sumatoria)
                        s.RezRecargo = s.RezRecargo + sumatoria;
                    else if (s.ActInfraestructura > 0 && s.ActInfraestructura < sumatoria)
                        s.ActInfraestructura = s.ActInfraestructura + sumatoria;
                    else if (s.ActRecoleccion > 0 && s.ActRecoleccion < sumatoria)
                        s.ActRecoleccion = s.ActRecoleccion + sumatoria;
                    else if (s.ActLimpieza > 0 && s.ActLimpieza < sumatoria)
                        s.ActLimpieza = s.ActLimpieza + sumatoria;
                    else if (s.ActDap > 0 && s.ActDap < sumatoria)
                        s.ActDap = s.ActDap + sumatoria;
                }
                ConceptoGral cgral = saldos.ConceptosP(i, claveCatastral.Trim(), "");// edo.IdPredio);

                ////tTramiteDetalle  
                if (cgral != null)
                {
                    foreach (ConceptosRec conceptos in cgral.conceptos)
                    {
                        tTramiteDetalle detalle = new tTramiteDetalle();
                        detalle.IdConcepto = Convert.ToInt32(conceptos.IdCri);
                        detalle.IdTramite = tramite.Id;
                        detalle.Importe = Convert.ToDecimal(conceptos.Importe - conceptos.Descuento);
                        detalle.IdConceptoRef = conceptos.IdConceptoRef;
                        detalle.Activo = true;
                        detalle.FechaModificacion = DateTime.Now;
                        detalle.IdUsuario = u.Id;

                        msg = new tTramiteDetalleBL().Insert(detalle);
                        if (msg != MensajesInterfaz.Ingreso)
                            throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");

                        idRef = conceptos.IdConceptoRef;
                    }

                }

                ConceptoGral cgrales = saldos.ConceptosS(s, claveCatastral);// edo.IdPredio);

                ////tTramiteDetalle  
                if (cgrales != null)
                {
                    foreach (ConceptosRec conceptos in cgrales.conceptos)
                    {
                        tTramiteDetalle detalle = new tTramiteDetalle();
                        detalle.IdConcepto = Convert.ToInt32(conceptos.IdCri);
                        detalle.IdTramite = tramite.Id;
                        detalle.Importe = Convert.ToDecimal(conceptos.Importe - conceptos.Descuento);
                        detalle.IdConceptoRef = conceptos.IdConceptoRef;
                        detalle.Activo = true;
                        detalle.FechaModificacion = DateTime.Now;
                        detalle.IdUsuario = u.Id;

                        msg = new tTramiteDetalleBL().Insert(detalle);
                        if (msg != MensajesInterfaz.Ingreso)
                            throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                    }
                }

                ////se ingresa interes por pago tardio
                if (tramite.Interes > 0)
                {
                    if (con != null)
                    {
                        tTramiteDetalle detalle = new tTramiteDetalle();
                        detalle.IdConcepto = Convert.ToInt32(con.Id);
                        detalle.IdTramite = tramite.Id; ;
                        detalle.Importe = tramite.Interes;
                        detalle.IdConceptoRef = idRef;
                        detalle.Activo = true;
                        detalle.FechaModificacion = DateTime.Now;
                        detalle.IdUsuario = u.Id;

                        msg = new tTramiteDetalleBL().Insert(detalle);
                        if (msg != MensajesInterfaz.Ingreso)
                            throw new System.ArgumentException(new Utileria().GetDescription(msg), "Error");
                    }
                    else
                    {
                        throw new System.ArgumentException(new Utileria().GetDescription("CRI por celebración de convenio no esta habilitado"), "Error");
                    }
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("Convenio.GuardaDetalle.Exception", ex, " Parámetros-");
            }
            return msg;
        }

        protected decimal InteresPagoTardio(int idTramite, DateTime fechaParcialidad)
        {
            decimal interes = 0;

            //Calcula interes por pago Tardio
            if (fechaParcialidad < DateTime.Now)
            {
                interes = new tTramiteBL().interesFaltaPagoConvenioIdTramite(idTramite);
                if (interes.Equals(-1))
                {
                    //btnRecalculo.Enabled = false;
                    vtnModal.ShowPopup(new Utileria().GetDescription("Imposible calcular el interés por Falta de Pago"), ModalPopupMensaje.TypeMesssage.Alert);
                    return -1;
                }
            }
            return interes;
        }

        protected void btnImprime_Click(object sender, EventArgs e)
        {
            if (hdfId.Value != "")
            {
                tConvenio convenio = new tConvenioBL().GetByConstraint(Convert.ToInt32(hdfId.Value));
                tConvenioEdoCta edoCta = new tConvenioEdoCtaBL().GetByConstraint(convenio.IdConvenioEdoCta);
                cPredio predio = new cPredioBL().GetByConstraint(edoCta.IdPredio);

                if (predio != null)
                {
                    string Path = Server.MapPath("~/");

                    string inputFile = Path + "Documentos/Convenios/Convenio_" + new cParametroSistemaBL().GetByClave("MUNICIPIO_COBRO").Valor + ".docx";
                    string outputFile = Path + "/Temporales/" + convenio.Folio + "-" + predio.ClavePredial.Trim() + ".docx";
                    File.Copy(inputFile, outputFile, true);

                    #region reemplazar
                    using (var flatDocument = new FlatDocument(outputFile))
                    {
                        decimal importe = Convert.ToDecimal(edoCta.Impuesto + edoCta.Rezago + edoCta.Diferencia + edoCta.Adicional + edoCta.Recargo + edoCta.RecargoDiferencia + edoCta.Honorarios + edoCta.Ejecucion + edoCta.Multa);
                        // Search and replace document's text content.
                        //valida si es clave o cuenta

                        if (predio.ClavePredial == "" || predio.ClavePredial == null)
                        {
                            flatDocument.FindAndReplace("«clave»", predio.ClaveAnterior == "" || predio.ClaveAnterior == null ? "" : predio.ClaveAnterior.Substring(0, 4) + "-" + predio.ClaveAnterior.Substring(4, 2) + "-" + predio.ClaveAnterior.Substring(6, 3) + "-" + predio.ClavePredial.Substring(9, 3));
                            
                        }
                        else
                        {
                            if (predio.ClavePredial.Substring(0, 1) == "0") //cuentapredial
                            {
                                flatDocument.FindAndReplace("«clave»", predio.ClaveAnterior == "" || predio.ClaveAnterior == null ? "" : predio.ClaveAnterior.Substring(0, 4) + "-" + predio.ClaveAnterior.Substring(4, 2) + "-" + predio.ClaveAnterior.Substring(6, 3) + "-" + predio.ClavePredial.Substring(9, 3));
                               
                            }
                            else//clavecatastral
                            {
                                flatDocument.FindAndReplace("«clave»", predio.ClavePredial == "" || predio.ClavePredial == null ? "" : predio.ClavePredial.Substring(0, 4) + "-" + predio.ClavePredial.Substring(4, 2) + "-" + predio.ClavePredial.Substring(6, 3) + "-" + predio.ClavePredial.Substring(9, 3));
                            }
                        }

                        flatDocument.FindAndReplace("«propietario»", predio.cContribuyente.ApellidoPaterno.Trim() + " " + predio.cContribuyente.ApellidoMaterno.Trim() + " " + predio.cContribuyente.Nombre.Trim());
                        flatDocument.FindAndReplace("«convenio»", convenio.Folio);
                        //flatDocument.FindAndReplace("«clave»", predio.ClavePredial.Substring(0, 4) + "-" + predio.ClavePredial.Substring(4, 2) + "-" + predio.ClavePredial.Substring(6, 3) + "-" + predio.ClavePredial.Substring(9, 3));
                        flatDocument.FindAndReplace("«ife»", convenio.NoIdentificacion.ToString() == string.Empty ? "-" : convenio.NoIdentificacion);
                        flatDocument.FindAndReplace("«dircompleta»", predio.Calle.Trim() + " " + predio.Numero.Trim() + ", " + predio.cColonia.NombreColonia.Trim() + " " + predio.cCondominio.Descripcion.Trim());
                        flatDocument.FindAndReplace("«telcasa»", convenio.Telefono.ToString() == string.Empty ? "-" : convenio.Telefono);
                        flatDocument.FindAndReplace("«telcelular»", convenio.Celular.ToString() == string.Empty ? "-" : convenio.Celular);
                        flatDocument.FindAndReplace("«email»", convenio.Email.ToString() == string.Empty ? "-" : convenio.Email);
                        flatDocument.FindAndReplace("«directorpredial»", new cParametroSistemaBL().GetValorByClave("DIRECTORPREDIAL"));
                        flatDocument.FindAndReplace("«puestopredial»", new cParametroSistemaBL().GetValorByClave("PUESTOPREDIAL"));
                        flatDocument.FindAndReplace("«dia»", convenio.FechaIni.Day.ToString());


                        flatDocument.FindAndReplace("«mes»", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(convenio.FechaIni.Month))); // convenio.FechaIni.Month.ToString("MMMM"));
                        flatDocument.FindAndReplace("«anio»", convenio.FechaIni.Year.ToString());
                        flatDocument.FindAndReplace("«BimIniSt»", edoCta.PeriodoIP.Substring(0, 6));
                        flatDocument.FindAndReplace("«BimFinSt»", edoCta.PeriodoIP.Substring(9, 6));
                        flatDocument.FindAndReplace("«periodoip»", edoCta.PeriodoIP.ToString());

                        ////montos totales    
                        //1 2018 - 6 2019   
                        flatDocument.FindAndReplace("«impuestooriginal»", Convert.ToDouble(edoCta.Impuesto + edoCta.Rezago).ToString("C"));
                        flatDocument.FindAndReplace("«diferenciasoriginal»", Convert.ToDouble(edoCta.Diferencia).ToString("C"));
                        flatDocument.FindAndReplace("«adicionaloriginal»", Convert.ToDouble(edoCta.Adicional).ToString("C"));
                        flatDocument.FindAndReplace("«recargosoriginal»", Convert.ToDouble(edoCta.Recargo + edoCta.RecargoDiferencia).ToString("C"));
                        flatDocument.FindAndReplace("«honorariosoriginal»", Convert.ToDouble(edoCta.Honorarios).ToString("C"));
                        flatDocument.FindAndReplace("«ejecucionoriginal»", Convert.ToDouble(edoCta.Ejecucion).ToString("C"));
                        flatDocument.FindAndReplace("«multasoriginal»", Convert.ToDouble(edoCta.Multa).ToString("C"));
                        flatDocument.FindAndReplace("«totaloriginal»", Convert.ToDouble(importe).ToString("C"));
                        string importeLetras = new GeneraRecibo33.ReciboCFDI33().Letras(importe.ToString());
                        flatDocument.FindAndReplace("«totaloriginalletra»", importeLetras);

                        ////con descuento
                        //«cadcondonaciones»
                        flatDocument.FindAndReplace("«impuesto»", Convert.ToDouble((edoCta.Impuesto + edoCta.Rezago) - (edoCta.DescImpuesto + edoCta.DescRezago)).ToString("C"));
                        flatDocument.FindAndReplace("«diferencias»", Convert.ToDouble(edoCta.Diferencia - edoCta.DescDiferencia).ToString("C"));
                        flatDocument.FindAndReplace("«adicional»", Convert.ToDouble(edoCta.Adicional - edoCta.DescAdicional).ToString("C"));
                        flatDocument.FindAndReplace("«recargos»", Convert.ToDouble((edoCta.Recargo - edoCta.DescRecargo) + (edoCta.RecargoDiferencia - edoCta.DescRecargoDiferencia)).ToString("C"));
                        flatDocument.FindAndReplace("«honorarios»", Convert.ToDouble(edoCta.Honorarios - edoCta.DescHonorarios).ToString("C"));
                        flatDocument.FindAndReplace("«ejecucion»", Convert.ToDouble(edoCta.Ejecucion - edoCta.DescEjecucion).ToString("C"));
                        flatDocument.FindAndReplace("«multas»", Convert.ToDouble(edoCta.Multa - edoCta.DescMulta).ToString("C"));
                        flatDocument.FindAndReplace("«total»", Convert.ToDouble(edoCta.Importe).ToString("C"));
                        string edoImporteLetras = new GeneraRecibo33.ReciboCFDI33().Letras(edoCta.Importe.ToString());
                        flatDocument.FindAndReplace("«totalletra»", edoImporteLetras);
                    }
                    #endregion 


                    #region tabla

                    using (WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(outputFile, true))
                    {
                        MainDocumentPart mainPart = wordprocessingDocument.MainDocumentPart;
                        DocumentFormat.OpenXml.Wordprocessing.Table table = generaTabla();

                        //load bookmarks
                        var bookmarks = mainPart.Document.Body.Descendants<BookmarkStart>();

                        //find our bookmark
                        var bookmark =
                            from bookmarkIndex in bookmarks
                            where bookmarkIndex.Name == "tabla"
                            select bookmarkIndex;
                        //get to first element
                        OpenXmlElement elem = bookmark.First().Parent;
                        elem.InsertAfterSelf(table);
                        elem.Remove();

                        mainPart.Document.Save();
                    }
                    #endregion

                    Response.Redirect("~/Temporales/" + convenio.Folio + "-" + predio.ClavePredial.Trim() + ".docx");
                    Response.Close();
                }
            }
        }//fin de imprimir convenio

        public DocumentFormat.OpenXml.Wordprocessing.Table generaTabla()
        {
            DocumentFormat.OpenXml.Wordprocessing.Table table = null;
            DocumentFormat.OpenXml.Wordprocessing.TableRow row = null;

            //create table
            table = new DocumentFormat.OpenXml.Wordprocessing.Table();

            SetTableStyle(table);

            //add first row with title            
            row = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
            row.Append(CreateCell("NO. PAGOS"));
            row.Append(CreateCell("FECHA DE PAGO"));
            row.Append(CreateCell("SALDO"));
            row.Append(CreateCell("PARCIALIDAD"));
            row.Append(CreateCell("RECARGO"));
            row.Append(CreateCell("MENSUALIDAD"));

            table.Append(row);

            for (int rowNumber = 1; rowNumber <= grdPagos.Rows.Count; rowNumber++)
            {
                TextBox txFecha = (TextBox)grdPagos.Rows[rowNumber - 1].FindControl("txtFecha");
                TextBox txRecargo = (TextBox)grdPagos.Rows[rowNumber - 1].FindControl("txtRecargo");
                TextBox txParcialidad = (TextBox)(grdPagos.Rows[rowNumber - 1].FindControl("txtParcialidad"));

                row = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                row.Append(CreateCell(grdPagos.Rows[rowNumber - 1].Cells[0].Text.ToString()));//consecutivo
                row.Append(CreateCell(txFecha.Text.ToString()));
                row.Append(CreateCell(Convert.ToDecimal(grdPagos.Rows[rowNumber - 1].Cells[2].Text).ToString("C")));//saldo
                row.Append(CreateCell(Convert.ToDecimal(txParcialidad.Text).ToString("C"))); //parcialidad
                row.Append(CreateCell(Convert.ToDecimal(txRecargo.Text).ToString("C")));//recargo
                row.Append(CreateCell(Convert.ToDecimal(grdPagos.Rows[rowNumber - 1].Cells[6].Text).ToString("C")));

                table.Append(row);
            }

            return table;
        }

        private static DocumentFormat.OpenXml.Wordprocessing.TableCell CreateCell(string text)
        {
            return new DocumentFormat.OpenXml.Wordprocessing.TableCell(new Paragraph(new Run(new Text(text))));
        }

        private static void SetTableStyle(DocumentFormat.OpenXml.Wordprocessing.Table table)
        {
            TableProperties properties = new TableProperties();

            //table borders
            TableBorders borders = new TableBorders();

            borders.TopBorder = new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single) };
            borders.BottomBorder = new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single) };
            borders.LeftBorder = new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single) };
            borders.RightBorder = new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single) };
            borders.InsideHorizontalBorder = new InsideHorizontalBorder() { Val = BorderValues.Single };
            borders.InsideVerticalBorder = new InsideVerticalBorder() { Val = BorderValues.Single };

            properties.Append(borders);

            //set the table width to page width
            TableWidth tableWidth = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };
            properties.Append(tableWidth);

            //add properties to table
            table.Append(properties);
        }

    }
}