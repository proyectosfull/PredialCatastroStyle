using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using Clases.BL;
using Clases.Utilerias;
using Catastro.Controles;
using iTextSharp.text.pdf;

namespace Catastro.Recibos
{
    public partial class CorteCaja : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlCajeros.DataValueField = "Id";
                ddlCajeros.DataTextField = "Usuario";
                ddlCajeros.DataSource = new cUsuariosBL().GetAll();
                ddlCajeros.DataBind();
                ddlCajeros.Items.Insert(0, new ListItem("Seleccionar Usuario", "%"));
                activaCampos(false);
                //pnlReport.Visible = false;
            }
        }
        //protected void ddlCajeros_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlCajeros.SelectedValue != "%")
        //    {
        //        ddlMesa.DataValueField = "IdMesa";
        //        ddlMesa.DataTextField = "Mesa";
        //        ddlMesa.DataSource = new vVistasBL().ObtieneMesaUsuario(Convert.ToInt32(ddlCajeros.SelectedValue));
        //        ddlMesa.DataBind();
        //        ddlMesa.Items.Insert(0, new ListItem("Seleccionar Mesa", "%"));
        //        limpiaCampos();
        //        btnGuardar.Visible = false;
        //    }
        //}
        protected void btnGenerarCorte_Click(object sender, EventArgs e)
        {
            int IdCajero = Convert.ToInt32(ddlCajeros.SelectedValue);
            DateTime FechaI = Convert.ToDateTime(txtFechaInicio.Text);
            DateTime FechaF = Convert.ToDateTime(txtFechaFin.Text + " 23:59:59");
            //string[] filtro = new string[] { IdCajero.ToString(), FechaI.ToString(), FechaF.ToString() };
            //ViewState["filtro"] = filtro;

            List<vReciboMetodoPago>  listMetodo = new vVistasBL().vReciboMetodoPago(FechaI, FechaF, "", IdCajero);

            if (listMetodo == null) return;

            grd.DataSource = listMetodo;
            grd.DataBind();

            List<tRecibo> listRecibo = new tReciboBL().GetRecibosCajero(IdCajero, FechaI, FechaF);
            if (listRecibo == null) return;
            //grd.DataSource = listRecibo;
            //grd.DataBind();

            var suma = listRecibo.Where(l => l.EstadoRecibo == "P").Sum(l => l.ImportePagado);

            //listMetodo = new vVistasBL().vReciboMetodoPago( FechaI, FechaF, "", IdCajero);

           // 1   01  EFECTIVO   2  02  CHEQUE  3  03  TRANSFERENCIA  4   04  TARJETAS DE CRÉDITO    9   28  TARJETA DE DÉBITO

            decimal cheque = Convert.ToDecimal( listMetodo.Where(l => l.CveMetodoPagoFinal == "02").Sum(l => l.ImporteMetodo) ) ;
            decimal transferencia = Convert.ToDecimal( listMetodo.Where(l => l.CveMetodoPagoFinal == "03").Sum(l => l.ImporteMetodo) );
            decimal credito = Convert.ToDecimal( listMetodo.Where(l => l.CveMetodoPagoFinal == "04").Sum(l => l.ImporteMetodo) );
            decimal debito = Convert.ToDecimal( listMetodo.Where(l => l.CveMetodoPagoFinal == "28").Sum(l => l.ImporteMetodo) );

            txtCheque.Text = cheque.ToString("N");
            lblCheque.Text = String.Format("{0:C2}",cheque);
            txtCredito.Text = credito.ToString("N");
            lblCredito.Text = String.Format("{0:C2}", credito);
            txtTarjeta.Text = debito.ToString("N");
            lblTarjeta.Text = String.Format("{0:C2}",debito);
            txtTransferencia.Text = transferencia.ToString("N");
            lblTransferencia.Text = String.Format("{0:C2}", transferencia);

            lblTotalImporte.Text = String.Format("{0:C2}", suma);

            var cobrados = listRecibo.Where(l => l.EstadoRecibo == "P").Count();
            lblCobrados.Text = cobrados.ToString();
            Session["cobrados"] = cobrados.ToString();

            var cancelados = listRecibo.Where(l => l.EstadoRecibo == "C").Count();
            lblCancelados.Text = cancelados.ToString();
            Session["cancelados"] = cancelados.ToString();

            activaCampos(true);
            calculoCorte();
            //rpt.LocalReport.DataSources.Clear();
            //rpt.LocalReport.Refresh();
        }

        private void calculoCorte()
        {
            decimal totalDenominacion;
            decimal Imoneda = Convert.ToDecimal(lblMoneda.Text.Replace("$", ""));
            decimal I20 = Convert.ToDecimal(lbl20.Text.Replace("$", ""));
            decimal I50 = Convert.ToDecimal(lbl50.Text.Replace("$", ""));
            decimal I100 = Convert.ToDecimal(lbl100.Text.Replace("$", ""));
            decimal I200 = Convert.ToDecimal(lbl200.Text.Replace("$", ""));
            decimal I500 = Convert.ToDecimal(lbl500.Text.Replace("$", ""));
            decimal I1000 = Convert.ToDecimal(lbl1000.Text.Replace("$", ""));
            decimal ICheque = Convert.ToDecimal(lblCheque.Text.Replace("$", ""));
            decimal ITarjeta = Convert.ToDecimal(lblTarjeta.Text.Replace("$", ""));
            decimal ICredito = Convert.ToDecimal(lblCredito.Text.Replace("$", ""));
            decimal ITransferencia = Convert.ToDecimal(lblTransferencia.Text.Replace("$", ""));

            totalDenominacion = Imoneda + I20 + I50 + I100 + I200 + I500 + I1000 + ICheque + ITarjeta + ICredito + ITransferencia;
            decimal totalRecibos = Convert.ToDecimal(lblTotalImporte.Text.Replace("$", ""));
            decimal diff = totalDenominacion - totalRecibos;
            lblvefectivo.Text = (Imoneda + I20 + I50 + I100 + I200 + I500 + I1000).ToString("C");
            lblvttarjetas.Text = (ICheque + ITarjeta + ICredito).ToString("C");

            lblDifImporte.Text = String.Format("{0:C2}", diff);
            lblImporteCorte.Text = String.Format("{0:C2}", totalDenominacion);

            if (diff != 0)
            {
                lblDifImporte.ForeColor = System.Drawing.Color.Red;
                btnGuardar.Visible = false;
            }
            else
            {
                lblDifImporte.ForeColor = System.Drawing.Color.Green;
                btnGuardar.Visible = true;
            }
        }
        protected void txtMoneda_TextChanged(object sender, EventArgs e)
        {
            if (txtMoneda.Text != "")
            {
                Decimal valor;
                if (Decimal.TryParse(txtMoneda.Text, out valor))
                {
                    //parsing successful 
                    lblMoneda.Text = String.Format("{0:C2}", valor);
                    lblVmoneda.Visible = false;
                    txt20.Focus();
                }
                else
                {
                    //parsing failed. 
                    lblVmoneda.Visible = true;
                    txtMoneda.Focus();
                    lblMoneda.Text = String.Format("{0:C2}", valor);
                }
            }
            else
            {
                lblMoneda.Text = String.Format("{0:C2}", 0);
                lblVmoneda.Visible = false;
            }
            calculoCorte();
        }
        protected void txt20_TextChanged(object sender, EventArgs e)
        {
            if (txt20.Text != "")
            {
                int valor = 0;
                if (int.TryParse(txt20.Text, out valor))
                {
                    //parsing successful 
                    lbl20.Text = String.Format("{0:C2}", valor * 20);
                    lblV20.Visible = false;
                    txt50.Focus();
                }
                else
                {
                    //parsing failed.                 
                    txt20.Focus();
                    lbl20.Text = String.Format("{0:C2}", valor);
                    lblV20.Visible = true;
                }
            }
            else
            {
                lbl20.Text = String.Format("{0:C2}", 0);
                lblV20.Visible = false;
            }
            calculoCorte();
        }
        protected void txt50_TextChanged(object sender, EventArgs e)
        {
            if (txt50.Text != "")
            {
                int valor = 0;
                if (int.TryParse(txt50.Text, out valor))
                {
                    //parsing successful 
                    lbl50.Text = String.Format("{0:C2}", valor * 50);
                    lblV50.Visible = false;
                    txt100.Focus();
                }
                else
                {
                    //parsing failed.                 
                    txt50.Focus();
                    lbl50.Text = String.Format("{0:C2}", valor);
                    lblV50.Visible = true;
                }
            }
            else
            {
                lbl50.Text = String.Format("{0:C2}", 0);
                lblV50.Visible = false;
            }
            calculoCorte();
        }
        protected void txt100_TextChanged(object sender, EventArgs e)
        {
            if (txt100.Text != "")
            {
                int valor = 0;
                if (int.TryParse(txt100.Text, out valor))
                {
                    //parsing successful 
                    lbl100.Text = String.Format("{0:C2}", valor * 100);
                    lblV100.Visible = false;
                    txt200.Focus();
                }
                else
                {
                    //parsing failed.                 
                    txt100.Focus();
                    lbl100.Text = String.Format("{0:C2}", valor);
                    lblV100.Visible = true;
                }
            }
            else
            {
                lbl100.Text = String.Format("{0:C2}", 0);
                lblV100.Visible = false;
            }
            calculoCorte();
        }
        protected void txt200_TextChanged(object sender, EventArgs e)
        {
            if (txt200.Text != "")
            {
                int valor = 0;
                if (int.TryParse(txt200.Text, out valor))
                {
                    //parsing successful 
                    lbl200.Text = String.Format("{0:C2}", valor * 200);
                    lblV200.Visible = false;
                    txt500.Focus();
                }
                else
                {
                    //parsing failed.                 
                    txt200.Focus();
                    lbl200.Text = String.Format("{0:C2}", valor);
                    lblV200.Visible = true;
                }
            }
            else
            {
                lbl200.Text = String.Format("{0:C2}", 0);
                lblV200.Visible = false;
            }
            calculoCorte();
        }
        protected void txt500_TextChanged(object sender, EventArgs e)
        {
            if (txt500.Text != "")
            {
                int valor = 0;
                if (int.TryParse(txt500.Text, out valor))
                {
                    //parsing successful 
                    lbl500.Text = String.Format("{0:C2}", valor * 500);
                    lblV500.Visible = false;
                    txt1000.Focus();
                }
                else
                {
                    //parsing failed.                 
                    txt500.Focus();
                    lbl500.Text = String.Format("{0:C2}", valor);
                    lblV500.Visible = true;
                }
            }
            else
            {
                lbl500.Text = String.Format("{0:C2}", 0);
                lblV500.Visible = false;
            }
            calculoCorte();
        }
        protected void txt1000_TextChanged(object sender, EventArgs e)
        {
            if (txt1000.Text != "")
            {
                int valor = 0;
                if (int.TryParse(txt1000.Text, out valor))
                {
                    //parsing successful 
                    lbl1000.Text = String.Format("{0:C2}", valor * 1000);
                    lblV1000.Visible = false;
                    txtCheque.Focus();
                }
                else
                {
                    //parsing failed.                 
                    txt1000.Focus();
                    lbl1000.Text = String.Format("{0:C2}", valor);
                    lblV1000.Visible = true;
                }
            }
            else
            {
                lbl1000.Text = String.Format("{0:C2}", 0);
                lblV1000.Visible = false;
            }
            calculoCorte();
        }
        protected void txtCheque_TextChanged(object sender, EventArgs e)
        {
            if (txtCheque.Text != "")
            {
                Decimal valor;
                if (Decimal.TryParse(txtCheque.Text, out valor))
                {
                    //parsing successful 
                    lblCheque.Text = String.Format("{0:C2}", valor);
                    lblVcheque.Visible = false;
                    txtTarjeta.Focus();
                }
                else
                {
                    //parsing failed. 
                    lblVcheque.Visible = true;
                    txtCheque.Focus();
                    lblCheque.Text = String.Format("{0:C2}", valor);
                }
            }
            else
            {
                lblCheque.Text = String.Format("{0:C2}", 0);
                lblVcheque.Visible = false;
            }
            calculoCorte();
        }
        protected void txtTarjeta_TextChanged(object sender, EventArgs e)
        {
            if (txtTarjeta.Text != "")
            {
                Decimal valor;
                if (Decimal.TryParse(txtTarjeta.Text, out valor))
                {
                    //parsing successful 
                    lblTarjeta.Text = String.Format("{0:C2}", valor);
                    lblVtarjeta.Visible = false;
                }
                else
                {
                    //parsing failed. 
                    lblVtarjeta.Visible = true;
                    txtTarjeta.Focus();
                    lblTarjeta.Text = String.Format("{0:C2}", valor);
                }
            }
            else
            {
                lblTarjeta.Text = String.Format("{0:C2}", 0);
                lblVtarjeta.Visible = false;
            }
            calculoCorte();
        }
        protected void txtCredito_TextChanged(object sender, EventArgs e)
        {
            if (txtCredito.Text != "")
            {
                Decimal valor;
                if (Decimal.TryParse(txtCredito.Text, out valor))
                {
                    //parsing successful 
                    lblCredito.Text = String.Format("{0:C2}", valor);
                    lblVCredito.Visible = false;
                }
                else
                {
                    //parsing failed. 
                    lblVCredito.Visible = true;
                    txtCredito.Focus();
                    lblCredito.Text = String.Format("{0:C2}", valor);
                }
            }
            else
            {
                lblCredito.Text = String.Format("{0:C2}", 0);
                lblVCredito.Visible = false;
            }
            calculoCorte();
        }
        protected void txtTransferencia_TextChanged(object sender, EventArgs e)
        {
            if (txtTransferencia.Text != "")
            {
                Decimal valor;
                if (Decimal.TryParse(txtTransferencia.Text, out valor))
                {
                    //parsing successful 
                    lblTransferencia.Text = String.Format("{0:C2}", valor);
                    lblVTransferencia.Visible = false;
                }
                else
                {
                    //parsing failed. 
                    lblVTransferencia.Visible = true;
                    txtTransferencia.Focus();
                    lblTransferencia.Text = String.Format("{0:C2}", valor);
                }
            }
            else
            {
                lblTransferencia.Text = String.Format("{0:C2}", 0);
                lblVTransferencia.Visible = false;
            }
            calculoCorte();
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DateTime fecha = DateTime.Now;
            cUsuarios usuario = (cUsuarios)Session["usuario"];
            tConfiguracionMesa cMesa = new tConfiguracionMesaBL().GetByIdUsuario(Convert.ToInt32(ddlCajeros.SelectedValue));
            tCorteCaja corte = new tCorteCaja();
            corte.Fecha = Convert.ToDateTime(txtFechaInicio.Text);  //DateTime.Now;
            corte.NoCaja = cMesa.IdCaja;
            corte.IdUsuarioCajero = Convert.ToInt32(ddlCajeros.SelectedValue);
            corte.IdMesa = 1;// Convert.ToInt32(ddlMesa.SelectedValue);
            corte.Equipo = "";// (System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName);
            corte.ImporteTotal = Convert.ToDecimal(lblTotalImporte.Text.Replace("$", ""));
            corte.RecibosCobrados = Convert.ToInt32(lblCobrados.Text);
            corte.RecibosCancelados = Convert.ToInt32(lblCancelados.Text);
            corte.Turno = cMesa.Turno;
            corte.FechaInicial = Convert.ToDateTime(txtFechaInicio.Text);  //fecha;// Convert.ToDateTime(txtFechaInicio.Text);
            corte.FechaFinal = Convert.ToDateTime(txtFechaFin.Text );  //fecha;// Convert.ToDateTime(txtFechaFin.Text + " 23:59:59");
            corte.Activo = true;
            corte.IdUsuario = usuario.Id;
            corte.FechaModificacion = fecha;

            #region Denominacion
            tCorteCajaDenominacion ccdMoneda = new tCorteCajaDenominacion();
            ccdMoneda.Denominacion = "Moneda fraccionaria";
            ccdMoneda.Cantidad = txtMoneda.Text == "" ? 0 : Convert.ToDecimal(txtMoneda.Text);
            ccdMoneda.Importe = Convert.ToDecimal(lblMoneda.Text.Replace("$", ""));
            ccdMoneda.Activo = true;
            ccdMoneda.IdUsuario = usuario.Id;
            ccdMoneda.FechaModificacion = fecha;
            corte.tCorteCajaDenominacion.Add(ccdMoneda);

            tCorteCajaDenominacion ccd20 = new tCorteCajaDenominacion();
            ccd20.Denominacion = "20";
            ccd20.Cantidad = txt20.Text == "" ? 0 : Convert.ToInt32(txt20.Text);
            ccd20.Importe = Convert.ToDecimal(lbl20.Text.Replace("$", ""));
            ccd20.Activo = true;
            ccd20.IdUsuario = usuario.Id;
            ccd20.FechaModificacion = fecha;
            corte.tCorteCajaDenominacion.Add(ccd20);

            tCorteCajaDenominacion ccd50 = new tCorteCajaDenominacion();
            ccd50.Denominacion = "50";
            ccd50.Cantidad = txt50.Text == "" ? 0 : Convert.ToInt32(txt50.Text);
            ccd50.Importe = Convert.ToDecimal(lbl50.Text.Replace("$", ""));
            ccd50.Activo = true;
            ccd50.IdUsuario = usuario.Id;
            ccd50.FechaModificacion = fecha;
            corte.tCorteCajaDenominacion.Add(ccd50);

            tCorteCajaDenominacion ccd100 = new tCorteCajaDenominacion();
            ccd100.Denominacion = "100";
            ccd100.Cantidad = txt100.Text == "" ? 0 : Convert.ToInt32(txt100.Text);
            ccd100.Importe = Convert.ToDecimal(lbl100.Text.Replace("$", ""));
            ccd100.Activo = true;
            ccd100.IdUsuario = usuario.Id;
            ccd100.FechaModificacion = fecha;
            corte.tCorteCajaDenominacion.Add(ccd100);

            tCorteCajaDenominacion ccd200 = new tCorteCajaDenominacion();
            ccd200.Denominacion = "200";
            ccd200.Cantidad = txt200.Text == "" ? 0 : Convert.ToInt32(txt200.Text);
            ccd200.Importe = Convert.ToDecimal(lbl200.Text.Replace("$", ""));
            ccd200.Activo = true;
            ccd200.IdUsuario = usuario.Id;
            ccd200.FechaModificacion = fecha;
            corte.tCorteCajaDenominacion.Add(ccd200);

            tCorteCajaDenominacion ccd500 = new tCorteCajaDenominacion();
            ccd500.Denominacion = "500";
            ccd500.Cantidad = txt500.Text == "" ? 0 : Convert.ToInt32(txt500.Text);
            ccd500.Importe = Convert.ToDecimal(lbl500.Text.Replace("$", ""));
            ccd500.Activo = true;
            ccd500.IdUsuario = usuario.Id;
            ccd500.FechaModificacion = fecha;
            corte.tCorteCajaDenominacion.Add(ccd500);

            tCorteCajaDenominacion ccd1000 = new tCorteCajaDenominacion();
            ccd1000.Denominacion = "1000";
            ccd1000.Cantidad = txt1000.Text == "" ? 0 : Convert.ToInt32(txt1000.Text);
            ccd1000.Importe = Convert.ToDecimal(lbl1000.Text.Replace("$", ""));
            ccd1000.Activo = true;
            ccd1000.IdUsuario = usuario.Id;
            ccd1000.FechaModificacion = fecha;
            corte.tCorteCajaDenominacion.Add(ccd1000);

            tCorteCajaDenominacion ccdCheque = new tCorteCajaDenominacion();
            ccdCheque.Denominacion = "Cheque";
            ccdCheque.Cantidad = txtCheque.Text == "" ? 0 : Convert.ToDecimal(txtCheque.Text);
            ccdCheque.Importe = Convert.ToDecimal(lblCheque.Text.Replace("$", ""));
            ccdCheque.Activo = true;
            ccdCheque.IdUsuario = usuario.Id;
            ccdCheque.FechaModificacion = fecha;
            corte.tCorteCajaDenominacion.Add(ccdCheque);

            tCorteCajaDenominacion ccdTarjeta = new tCorteCajaDenominacion();
            ccdTarjeta.Denominacion = "Tarjeta débito";
            ccdTarjeta.Cantidad = txtTarjeta.Text == "" ? 0 : Convert.ToDecimal(txtTarjeta.Text);
            ccdTarjeta.Importe = Convert.ToDecimal(lblTarjeta.Text.Replace("$", ""));
            ccdTarjeta.Activo = true;
            ccdTarjeta.IdUsuario = usuario.Id;
            ccdTarjeta.FechaModificacion = fecha;
            corte.tCorteCajaDenominacion.Add(ccdTarjeta);

            tCorteCajaDenominacion ccdCredito = new tCorteCajaDenominacion();
            ccdCredito.Denominacion = "Tarjeta crédito";
            ccdCredito.Cantidad = txtCredito.Text == "" ? 0 : Convert.ToDecimal(txtCredito.Text);
            ccdCredito.Importe = Convert.ToDecimal(lblCredito.Text.Replace("$", ""));
            ccdCredito.Activo = true;
            ccdCredito.IdUsuario = usuario.Id;
            ccdCredito.FechaModificacion = fecha;
            corte.tCorteCajaDenominacion.Add(ccdCredito);

            tCorteCajaDenominacion ccdTransferencia = new tCorteCajaDenominacion();
            ccdTransferencia.Denominacion = "Transferencia";
            ccdTransferencia.Cantidad = txtTransferencia.Text == "" ? 0 : Convert.ToDecimal(txtTransferencia.Text);
            ccdTransferencia.Importe = Convert.ToDecimal(lblTransferencia.Text.Replace("$", ""));
            ccdTransferencia.Activo = true;
            ccdTransferencia.IdUsuario = usuario.Id;
            ccdTransferencia.FechaModificacion = fecha;
            corte.tCorteCajaDenominacion.Add(ccdTransferencia);

            #endregion

            //agrega detalle corte recibos
            foreach (GridViewRow gr in grd.Rows)
            {
                if (gr.RowType == DataControlRowType.DataRow)
                {
                    tCorteCajaDetalle ccd = new tCorteCajaDetalle();
                    ccd.IdRecibo = Convert.ToInt32(grd.DataKeys[gr.RowIndex].Values[0].ToString());
                    ccd.Activo = true;
                    ccd.IdUsuario = usuario.Id;
                    ccd.FechaModificacion = fecha;
                    corte.tCorteCajaDetalle.Add(ccd);
                }
            }
            MensajesInterfaz respuesta = new tCorteCajaBL().Insert(corte);
            if (respuesta == MensajesInterfaz.Ingreso)
            {
                reporte(corte);
                //activaCampos(false);
                btnGuardar.Visible = false;
            }
            else
            {
                mgs.ShowPopup(new Utileria().GetDescription(respuesta), ModalPopupMensaje.TypeMesssage.Alert);
            }
        }
        private void limpiaCampos()
        {
            txtFechaInicio.Text = "";
            txtFechaFin.Text = "";
            lblCobrados.Text = "0";
            lblCancelados.Text = "0";
            grd.DataBind();
            grd.DataSource = null;
            txtMoneda.Text = "";
            lblMoneda.Text = "$0.00";
            txt20.Text = "";
            lbl20.Text = "$0.00";
            txt50.Text = "";
            lbl50.Text = "$0.00";
            txt100.Text = "";
            lbl100.Text = "$0.00";
            txt200.Text = "";
            lbl200.Text = "$0.00";
            txt500.Text = "";
            lbl500.Text = "$0.00";
            txt1000.Text = "";
            lbl1000.Text = "$0.00";
            txtCheque.Text = "";
            lblCheque.Text = "$0.00";
            txtTarjeta.Text = "$0.00";
            txtCredito.Text = "$0.00";
            lblTransferencia.Text = "$0.00";
            lblImporteCorte.Text = "$0.00";
            lblTotalImporte.Text = "$0.00";
            lblDifImporte.Text = "$0.00";
        }
        private void activaCampos(bool activo)
        {
            txtMoneda.Enabled = activo;
            txt20.Enabled = activo;
            txt50.Enabled = activo;
            txt100.Enabled = activo;
            txt200.Enabled = activo;
            txt500.Enabled = activo;
            txt1000.Enabled = activo;
            txtCheque.Enabled = activo;
            txtTarjeta.Enabled = activo;
            txtCredito.Enabled = activo;
            txtTransferencia.Enabled = activo;
        }
        private void reporte(tCorteCaja corte)
        {
            tbPrincipal.Visible = false;
            //pnlReport.Visible = true;
            cUsuarios U = (cUsuarios)Session["usuario"];
            //CARGA DATOS GENERALES y se crea datatable
            List<cParametroSistema> listConfiguraciones = new cParametroSistemaBL().GetAll();
            string NombreMunicipio = listConfiguraciones.FirstOrDefault(c => c.Clave == "NOMBRE_MUNICIPIO").Valor;
            string Dependencia = listConfiguraciones.FirstOrDefault(c => c.Clave == "DEPENDENCIA").Valor;
            string Area = listConfiguraciones.FirstOrDefault(c => c.Clave == "AREA").Valor;
            string UrlLogo = Server.MapPath("~") + listConfiguraciones.FirstOrDefault(c => c.Clave == "LOGO").Valor;
            string MUNICIPIO_COBRO = listConfiguraciones.FirstOrDefault(c => c.Clave == "MUNICIPIO_COBRO").Valor;
            FileStream fS = new FileStream(UrlLogo, FileMode.Open, FileAccess.Read);
            byte[] LogoByte = new byte[fS.Length];
            fS.Read(LogoByte, 0, (int)fS.Length);
            fS.Close();
            string RecibioCajaGeneral = listConfiguraciones.FirstOrDefault(c => c.Clave == "RecibioCajaGeneral").Valor;
            string VoBo = listConfiguraciones.FirstOrDefault(c => c.Clave == "VoBo").Valor;
            DataTable ConfGral = new DataTable("ConfGral");
            ConfGral.Columns.Add("NombreMunicipio");
            ConfGral.Columns.Add("Dependencia");
            ConfGral.Columns.Add("Area");
            ConfGral.Columns.Add("Logo", typeof(Byte[]));
            ConfGral.Columns.Add("Mesa");
            ConfGral.Columns.Add("Cajero");
            ConfGral.Columns.Add("Entrego");
            ConfGral.Columns.Add("RecibioCajaGeneral");
            ConfGral.Columns.Add("VoBo");
            //ConfGral.Rows.Add(NombreMunicipio, Dependencia, Area, LogoByte, ddlMesa.SelectedItem.Text, U.Usuario, U.Nombre + " " + U.ApellidoPaterno + " " + U.ApellidoMaterno, RecibioCajaGeneral, VoBo);
            ConfGral.Rows.Add(NombreMunicipio, Dependencia, Area, LogoByte, "", U.Usuario, U.Nombre + " " + U.ApellidoPaterno + " " + U.ApellidoMaterno, RecibioCajaGeneral, VoBo);
            //lista tCorteCaja
            List<tCorteCaja> listcorte = new List<tCorteCaja>();
            listcorte.Add(corte);

            DataTable MetodoPagoDTS = new DataTable("MetodoPagoDTS");
            MetodoPagoDTS.Columns.Add("Metodo");
            MetodoPagoDTS.Columns.Add("Numero");
            MetodoPagoDTS.Columns.Add("Autorizacion");
            MetodoPagoDTS.Columns.Add("Importe", System.Type.GetType("System.Decimal"));
            MetodoPagoDTS.Columns.Add("ClavePredial");
            decimal totalEfectivo = 0;
            if (MUNICIPIO_COBRO == "OTRO MPIO") // GeneraRecibo listado con contribuyente e importe
            {
                //se llena lista de recibos
                List<vRecibo> listvRecibos = new List<vRecibo>();
                foreach (tCorteCajaDetalle ccd in corte.tCorteCajaDetalle)
                {
                    vRecibo r = new vVistasBL().ObtieneRecibo(ccd.IdRecibo);
                    if (r.EstadoRecibo == "CANCELADO")
                    {
                        r.ImportePagado = 0;//Convert.ToDecimal("0.00");
                    }
                    listvRecibos.Add(r);
                    if (r.IdTipoPago != 2 && r.EstadoRecibo != "CANCELADO")
                    {
                        MetodoPagoDTS.Rows.Add(r.Nombre, r.NoTipoPago, r.NoAutorizacion, r.ImportePagado, r.ClavePredial);
                    }
                    else if (r.IdTipoPago == 2 && r.EstadoRecibo != "CANCELADO")
                    {
                        totalEfectivo = totalEfectivo + r.ImportePagado;
                    }
                }
                MetodoPagoDTS.Rows.Add("EFECTIVO", "", "", totalEfectivo, "");

                List<tCorteCajaDenominacion> listCorteCajaDenominacion = corte.tCorteCajaDenominacion.Where(cc => cc.Denominacion != "Cheque" && cc.Denominacion != "Tarjeta").ToList();

                ////INICIA REPORTE
                //rpt.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                //rpt.LocalReport.DisplayName = "Corte de Caja";
                //rpt.LocalReport.ReportPath = "Reportes/CorteCajaRecibosRpt.rdlc";
                //rpt.LocalReport.EnableExternalImages = true;
                //rpt.LocalReport.DataSources.Clear();
                //rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ConfGral", ConfGral));
                //rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("tCorteCaja", listcorte));
                //rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("vRecibo", listvRecibos));
                //rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("MetodoPagoDTS", MetodoPagoDTS));
                //rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("tCorteCajaDenominacion", corte.tCorteCajaDenominacion));
                //rpt.LocalReport.Refresh();
            }
            else
            {
                //se llena lista de recibos
                //List<vRecibo> listvRecibos = new List<vRecibo>();
                foreach (tCorteCajaDetalle ccd in corte.tCorteCajaDetalle)
                {
                    vRecibo r = new vVistasBL().ObtieneRecibo(ccd.IdRecibo);
                    //if (r.EstadoRecibo == "CANCELADO")
                    //    r.ImportePagado =  0;//Convert.ToDecimal("0.00");
                    //listvRecibos.Add(r);
                    if (r.IdTipoPago != 2 && r.EstadoRecibo != "CANCELADO")
                    {
                        MetodoPagoDTS.Rows.Add(r.Nombre, r.NoTipoPago, r.NoAutorizacion, r.ImportePagado, r.ClavePredial);
                    }
                    else if (r.IdTipoPago == 2 && r.EstadoRecibo != "CANCELADO")
                    {
                        totalEfectivo = totalEfectivo + r.ImportePagado;
                    }
                }

                //MetodoPagoDTS.Rows.Add("EFECTIVO", "", "", totalEfectivo,"");
                ////List<tCorteCajaDenominacion> listCorteCajaDenominacion = corte.tCorteCajaDenominacion.Where(cc => cc.Denominacion != "Cheque" && cc.Denominacion != "Tarjeta").ToList();
                ////INICIA REPORTE
                //rpt.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                //rpt.LocalReport.DisplayName = "Corte de Caja";
                //rpt.LocalReport.ReportPath = "Reportes/CorteCajaRpt.rdlc";
                ////rpt.LocalReport.ReportPath = "Reportes/CorteCajaDetRpt.rdlc"; reporte de recibos detallado 
                //rpt.LocalReport.EnableExternalImages = true;
                //rpt.LocalReport.DataSources.Clear();
                //rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ConfGral", ConfGral));
                //rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("tCorteCaja", listcorte));
                //rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("tCorteCaja", listcorte));
                ////rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("vRecibo", listvRecibos));
                //rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("MetodoPagoDTS", MetodoPagoDTS));
                //rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("tCorteCajaDenominacion", corte.tCorteCajaDenominacion));
                //rpt.LocalReport.Refresh();

                fillPDF();
            }
        }

        protected void fillPDF()
        {

            String path = Server.MapPath("~/");
            String formOriginal = path + "Documentos/Caja/" + "CorteCaja_ZAPATA.pdf";
            String ruta = "CorteDeCaja_" + DateTime.Now.ToString("dd-MM-yyyy") + "_" + DateTime.Now.Second + ".pdf";
            String formImprimir = path + "/Temporales/" + ruta;
            ViewState["pdf"] = ruta;

            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];

            PdfReader reader = new PdfReader(formOriginal);
            PdfStamper stam = new PdfStamper(reader, new FileStream(formImprimir, FileMode.Create));

            stam.AcroFields.SetField("fechainicial", (Convert.ToDateTime(txtFechaInicio.Text).ToString("dd") + " de " + (Convert.ToDateTime(txtFechaInicio.Text).ToString("MMMM")).ToLower() +
                            " de " + Convert.ToDateTime(txtFechaInicio.Text).ToString("yyyy")));
            stam.AcroFields.SetField("fechafinal", (Convert.ToDateTime(txtFechaFin.Text).ToString("dd") + " de " + (Convert.ToDateTime(txtFechaFin.Text).ToString("MMMMM")).ToLower() +
                            " de " + Convert.ToDateTime(txtFechaFin.Text).ToString("yyyy")));
            cUsuarios c = new cUsuariosBL().GetByConstraint( Convert.ToInt32(ddlCajeros.SelectedValue)) ;

            stam.AcroFields.SetField("cajero", c.Usuario);
            stam.AcroFields.SetField("fechaelaboracion", (Convert.ToDateTime(txtFechaInicio.Text).ToString("dd") + " de " + (Convert.ToDateTime(txtFechaInicio.Text).ToString("MMMM")).ToLower() +
                            " de " + Convert.ToDateTime(txtFechaInicio.Text).ToString("yyyy")));

            stam.AcroFields.SetField("cmonedas", txtMoneda.Text);
            stam.AcroFields.SetField("tmonedas", lblMoneda.Text);
            stam.AcroFields.SetField("c20", txt20.Text);
            stam.AcroFields.SetField("t20", lbl20.Text);
            stam.AcroFields.SetField("c50", txt50.Text);
            stam.AcroFields.SetField("t50", lbl50.Text);
            stam.AcroFields.SetField("c100", txt100.Text);
            stam.AcroFields.SetField("t100", lbl100.Text);
            stam.AcroFields.SetField("c200", txt200.Text);
            stam.AcroFields.SetField("t200", lbl200.Text);
            stam.AcroFields.SetField("c500", txt500.Text);
            stam.AcroFields.SetField("t500", lbl500.Text);
            stam.AcroFields.SetField("c1000", txt1000.Text);
            stam.AcroFields.SetField("t1000", lbl1000.Text);

            stam.AcroFields.SetField("ccheque", txtCheque.Text);
            stam.AcroFields.SetField("tcheque", lblCheque.Text);
            stam.AcroFields.SetField("cdebito", txtTarjeta.Text);
            stam.AcroFields.SetField("tdebito", lblTarjeta.Text);
            stam.AcroFields.SetField("ccredito", txtCredito.Text);
            stam.AcroFields.SetField("tcredito", lblCredito.Text);

            stam.AcroFields.SetField("ctransferencia", txtTransferencia.Text);
            stam.AcroFields.SetField("ttransferencia", lblTransferencia.Text);

            stam.AcroFields.SetField("tefectivo", lblvefectivo.Text);
            stam.AcroFields.SetField("ttarjeta", lblvttarjetas.Text);
            //stam.AcroFields.SetField("ctotal", txtTransferencia.Text);
            stam.AcroFields.SetField("ttotal", lblTotalImporte.Text);

            stam.AcroFields.SetField("cobrados", Session["cobrados"].ToString());
            stam.AcroFields.SetField("cancelados", Session["cancelados"].ToString());
            stam.AcroFields.SetField("totalRecibos", ( Convert.ToDecimal(Session["cobrados"]) + Convert.ToDecimal(Session["cancelados"])).ToString()   );

            //DateTime fecha = DateTime.Now;
            //stam.AcroFields.SetField("Fecha", (Convert.ToDateTime(fecha).ToString("dd") + " de " + (Convert.ToDateTime(fecha).ToString("MMMM")).ToUpper() +
            //                " de " + Convert.ToDateTime(fecha).ToString("yyyy")));

            stam.AcroFields.SetField("titular", HttpUtility.HtmlDecode(new cParametroSistemaBL().GetValorByClave("DirectorPredial")));
            stam.AcroFields.SetField("puestotitular", HttpUtility.HtmlDecode(new cParametroSistemaBL().GetValorByClave("PUESTOPREDIAL")));
            stam.AcroFields.SetField("elaboro", HttpUtility.HtmlDecode(U.Nombre + " " + U.ApellidoPaterno + " " + U.ApellidoMaterno)); // + " " + DateTime.Today.ToString()));
            //stam.AcroFields.SetField("{{fechaActual}}","A "+DateTime.Now.ToString("dd")+" DE "+ DateTime.Now.ToString("MMMM")+" DE "+DateTime.Now.ToString("yyyy") );  
            //stam.AcroFields.SetField("{{puestoPredio}}", new cParametroSistemaBL().GetValorByClave("PuestoPredial"));
            stam.FormFlattening = true;
            stam.Close();

            frameRecibo.Src = "~/Temporales/" + ViewState["pdf"].ToString();
            modalRecibo.Show();
            //frameRecibo.Attributes["src"] = ConfigurationManager.AppSettings["NombreSitioWeb"] + "/Temporales/" + ViewState["pdf"].ToString();
            String Clientscript = "printPdf();";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newWindow", Clientscript, true);
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            tbPrincipal.Visible = true;
            limpiaCampos();
            //pnlReport.Visible = false;
        }

        protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //if (e.Row.Cells[1].Text.ToString() == "PAGADO")
                //{
                //    e.Row.Cells[1].Text = "PAGADO";
                //}
                //else
                //{
                //    e.Row.Cells[1].Text = "CANCELADO";
                //}
            }
        }

        //protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    grd.PageIndex = e.NewPageIndex;
        //    string[] filtro = (string[])ViewState["filtro"];
        //    List<tRecibo> listRecibo = new tReciboBL().GetRecibosCajero(Convert.ToInt32(filtro[0]), Convert.ToDateTime(filtro[1]), Convert.ToDateTime(filtro[2]));
        //    grd.DataSource = listRecibo;
        //    grd.DataBind();
        //}

        protected void imCerrarRecibo_Click(object sender, ImageClickEventArgs e)
        {
            modalRecibo.Hide();
            tbPrincipal.Visible = true;
            limpiaCampos();
        }




    }
}