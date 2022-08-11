using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using Clases.BL;
using Clases.Utilerias;
using Catastro.Controles;

namespace Catastro.Recibos
{
    public partial class ReporteIngresosPorGrupo : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["parametro"] != null)
                {
                    Dictionary<string, string> parametros = (Dictionary<string, string>)Session["parametro"];
                    if (parametros.ContainsKey("idPrepoliza"))
                    {
                        if (parametros.ContainsKey("tipoPantalla"))
                        {
                            if (parametros["tipoPantalla"] == "C")
                                activaCampos(false);
                            else
                                ViewState["IdMod"] = parametros["idFolio"];
                        }
                        llenaPrepoliza(Convert.ToInt32(parametros["idPrepoliza"]));
                    }
                }
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }

        private void llenaPrepoliza(int idPrepoliza)
        {
            tPrepoliza prepoliza = new tPrepolizaBL().GetByConstraint(idPrepoliza);
            txtFechaInicio.Text = prepoliza.FechaInicio.ToString("dd/MM/yyyy");
            txtFechaFin.Text = prepoliza.FechaFin.ToString("dd/MM/yyyy");
            txtDescripcion.Text = prepoliza.Descripcion;
            ViewState["prepoliza"] = prepoliza.Id;
            generaReporte();
        }

        private void activaCampos(bool activa)
        {
            btnGenerar.Visible = activa;
            imbBuscar.Visible = activa;
            txtFechaInicio.Enabled = activa;
            txtFechaFin.Enabled = activa;
            txtDescripcion.Enabled = activa;
            pnlReport.Visible = !activa;
            //btnVerDetalle.Visible = !activa;
            //btnReporte.Visible = !activa;
        }

        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            DateTime fin = Convert.ToDateTime(txtFechaFin.Text + " 23:59:59");
            DateTime inicio = Convert.ToDateTime(txtFechaInicio.Text);
            string MUNICIPIO_COBRO = new cParametroSistemaBL().GetValorByClave("MUNICIPIO_COBRO");

            object p;
            if (MUNICIPIO_COBRO == "TLALTIZAPAN")
            {
                //List<pReporteIngresosPorConcepto_Result> p = new pProcedimientos().ObtieneReporteIngresoPorConcepto(inicio, fin);
                p = new pProcedimientos().ObtieneReporteIngresoPorConcepto(inicio, fin);
            }
            else
            {
                //List<pReporteIngresosPorConceptoAgrupado_Result> p = new pProcedimientos().ObtieneReporteIngresoPorConceptoAgrupado(inicio, fin);
                p = new pProcedimientos().ObtieneReporteIngresoPorConceptoAgrupado(inicio, fin);
            }

            grdReporte.DataSource = p;
            grdReporte.DataBind();

            if (grdReporte.Rows.Count > 0)
            {
                btnGenerar.Visible = true;
                txtDescripcion.Visible = true;
                lblDescripcion.Visible = true;
            }
            else
            {
                btnGenerar.Visible = false;
                txtDescripcion.Visible = false;
                lblDescripcion.Visible = false;
            }

        }

        protected void btnReporte_Click(object sender, EventArgs e)
        {
            generaReporte();
            //if (!btnVerDetalle.Visible)
            //{
            //    btnGenerar.Visible = true;
            //    txtDescripcion.Visible = true;
            //}
        }

        private void generaReporte()
        {
            //CARGA DATOS GENERALES y se crea datatable
            List<cParametroSistema> listConfiguraciones = new cParametroSistemaBL().GetAll();
            string NombreMunicipio = listConfiguraciones.FirstOrDefault(c => c.Clave == "NOMBRE_MUNICIPIO").Valor;
            string Dependencia = listConfiguraciones.FirstOrDefault(c => c.Clave == "DEPENDENCIA").Valor;
            string MUNICIPIO_COBRO = listConfiguraciones.FirstOrDefault(c => c.Clave == "MUNICIPIO_COBRO").Valor;

            string Area = listConfiguraciones.FirstOrDefault(c => c.Clave == "AREA").Valor;
            string UrlLogo = Server.MapPath("~") + "\\" + listConfiguraciones.FirstOrDefault(c => c.Clave == "LOGO").Valor;
            FileStream fS = new FileStream(UrlLogo, FileMode.Open, FileAccess.Read);
            byte[] LogoByte = new byte[fS.Length];
            fS.Read(LogoByte, 0, (int)fS.Length);
            fS.Close();
            string VoBo = listConfiguraciones.FirstOrDefault(c => c.Clave == "VoBo").Valor;
            DataTable ConfGral = new DataTable("ConfGral");
            ConfGral.Columns.Add("NombreMunicipio");
            ConfGral.Columns.Add("Dependencia");
            ConfGral.Columns.Add("Area");
            ConfGral.Columns.Add("Logo", typeof(Byte[]));
            ConfGral.Columns.Add("Usuario");
            ConfGral.Columns.Add("Elaboro");
            ConfGral.Columns.Add("VoBo");
            List<tPrepoliza> listaPoliza = new List<tPrepoliza>();
            tPrepoliza p = new tPrepolizaBL().GetByConstraint(int.Parse(ViewState["prepoliza"].ToString()));
            listaPoliza.Add(p);

            ConfGral.Rows.Add(NombreMunicipio, Dependencia, Area, LogoByte, p.cUsuarios.Nombre + " " + p.cUsuarios.ApellidoPaterno + " " + p.cUsuarios.ApellidoMaterno, p.cUsuarios.Nombre + " " + p.cUsuarios.ApellidoPaterno + " " + p.cUsuarios.ApellidoMaterno, VoBo);
            //       ObtieneReporteIngresoGrupoPrepoliza   
            //List<pReporteIngresosPorGrupo_Result> listaDetalle = new pProcedimientos().ObtieneReporteIngresoPorGrupoPrepoliza(int.Parse(ViewState["prepoliza"].ToString()));
            //List<pReporteIngresosPrepolizaConsulta_Result> listaDetalle = new pProcedimientos().ReporteIngresoPrepolizaConsulta(int.Parse(ViewState["prepoliza"].ToString()));

            List<tPrepolizaDetalle> listaDetalle = p.tPrepolizaDetalle.ToList();

            List<tPrepolizaDetalle> listaDetalleSinDesc = listaDetalle.Where(l => l.Importe > 0).ToList();
            List<tPrepolizaDetalle> listaDetalleDesc = listaDetalle.Where(l => l.Importe < 0).ToList();
            tPrepolizaDetalle listaSubtotal = new tPrepolizaDetalle();
            listaSubtotal.descripcion = "SUBTOTAL";
            listaSubtotal.Importe = listaDetalleSinDesc.Sum(l => l.Importe);
            listaSubtotal.Recibos = listaDetalleSinDesc.Count();

            List<tPrepolizaDetalle> listaDetalleFinal = listaDetalleSinDesc;
            listaDetalleFinal.Add(listaSubtotal);
            listaDetalleFinal.AddRange(listaDetalleDesc);


            //INICIA REPORTE
            rpt.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            rpt.LocalReport.DisplayName = "Reporte de Ingresos";
            if (MUNICIPIO_COBRO == "YAUTEPEC")
            {
                rpt.LocalReport.ReportPath = "Reportes/IngresosPorGrupoSinFirma.rdlc";
            }
            else
            {
                rpt.LocalReport.ReportPath = "Reportes/IngresosPorGrupo.rdlc";
            }
            rpt.LocalReport.EnableExternalImages = true;
            rpt.LocalReport.DataSources.Clear();
            rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ConfGral", ConfGral));
            rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("Prepoliza", listaPoliza));
            rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("reporteGrupo", listaDetalleFinal));
            rpt.LocalReport.Refresh();

        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("BusquedaReporteIngresos");
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            tPrepoliza p = new tPrepoliza();
            p.Activo = true;
            p.FechaInicio = Convert.ToDateTime(txtFechaInicio.Text);
            p.FechaFin = Convert.ToDateTime(txtFechaFin.Text);
            p.FechaPrePoliza = DateTime.Now;
            p.FechaModificacion = DateTime.Now;
            p.IdUsuario = ((cUsuarios)Session["usuario"]).Id;
            //p.Importe = Convert.ToDecimal(lblTotal.Text.Replace("$", ""));
            //p.ImporteDescuentoTotal = Convert.ToDecimal(lblTotalDescuento.Text.Replace("$", ""));
            cParametroSistema DESCUENTO_ANT = new cParametroSistemaBL().GetByClave("DESCUENTO_ANT");
            cParametroSistema DESCUENTO = new cParametroSistemaBL().GetByClave("DESCUENTO");
            string MUNICIPIO_COBRO = new cParametroSistemaBL().GetValorByClave("MUNICIPIO_COBRO");

            foreach (GridViewRow row in grdReporte.Rows)
            {
                if (row.Cells[0].Text == DESCUENTO_ANT.Valor)
                {
                    p.ImporteDescuentoAntTotal = Convert.ToDecimal(grdReporte.DataKeys[row.RowIndex].Values[1].ToString());
                }
                else if (row.Cells[0].Text == DESCUENTO.Valor)
                {
                    p.ImporteDescuentoTotal = Convert.ToDecimal(grdReporte.DataKeys[row.RowIndex].Values[1].ToString());
                }
            }
            //GridViewRow footerRow = grdReporte.FooterRow;
            p.Importe = Convert.ToDecimal(ViewState["Total"]);
            p.Descripcion = txtDescripcion.Text;
            DateTime fin = Convert.ToDateTime(txtFechaFin.Text + " 23:59:59");
            DateTime inicio = Convert.ToDateTime(txtFechaInicio.Text);
            List<int?> recibos = new pProcedimientos().ObtieneReporteIngresoRecibos(inicio, fin);

            foreach (int? recibo in recibos)
            {
                tPrepolizaRecibo tpr = new tPrepolizaRecibo();
                tpr.Activo = true;
                tpr.IdUsuario = ((cUsuarios)Session["usuario"]).Id;
                tpr.FechaModificacion = DateTime.Now;
                tpr.IdRecibo = ((int)recibo);
                p.tPrepolizaRecibo.Add(tpr);
            }

            if (MUNICIPIO_COBRO == "TLALTIZAPAN")
            {
                List<pReporteIngresosPorConcepto_Result> pList = new pProcedimientos().ObtieneReporteIngresoPorConcepto(inicio, fin);
                foreach (pReporteIngresosPorConcepto_Result grd in pList)
                {
                    tPrepolizaDetalle tpd = new tPrepolizaDetalle();
                    tpd.Activo = true;
                    tpd.IdUsuario = ((cUsuarios)Session["usuario"]).Id;
                    tpd.FechaModificacion = DateTime.Now;
                    tpd.IdConcepto = grd.IdConcepto;// (int.Parse(grdReporte.DataKeys[grd.RowIndex].Value.ToString()));
                    tpd.Orden = grd.orden;
                    tpd.Clave = grd.Clave;
                    tpd.descripcion = grd.Descripcion;
                    tpd.Recibos = (Int32)grd.Recibos;
                    tpd.Importe = (decimal)grd.Total;//Convert.ToDecimal(grd.Cells[2].Text.Replace("$", ""));
                    p.tPrepolizaDetalle.Add(tpd);
                }
            }
            else
            {
                List<pReporteIngresosPorConceptoAgrupado_Result> pList = new pProcedimientos().ObtieneReporteIngresoPorConceptoAgrupado(inicio, fin);
                foreach (pReporteIngresosPorConceptoAgrupado_Result grd in pList)
                {
                    tPrepolizaDetalle tpd = new tPrepolizaDetalle();
                    tpd.Activo = true;
                    tpd.IdUsuario = ((cUsuarios)Session["usuario"]).Id;
                    tpd.FechaModificacion = DateTime.Now;
                    tpd.IdConcepto = grd.IdConcepto;// (int.Parse(grdReporte.DataKeys[grd.RowIndex].Value.ToString()));
                    tpd.Orden = grd.orden;
                    tpd.Clave = grd.Clave;
                    tpd.descripcion = grd.Descripcion;
                    tpd.Recibos = (Int32)grd.Recibos;
                    tpd.Importe = (decimal)grd.Total;//Convert.ToDecimal(grd.Cells[2].Text.Replace("$", ""));
                    p.tPrepolizaDetalle.Add(tpd);
                }
            }

            MensajesInterfaz msg = new MensajesInterfaz();
            msg = new tPrepolizaBL().Insert(p);
            vtnModal.DysplayCancelar = false;
            ViewState["prepoliza"] = p.Id;
            vtnModal.DysplayCancelar = false;
            vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Confirm);
            btnGenerar.Visible = false;
            txtDescripcion.Enabled = false;
            pnlReport.Visible = true;
            // btnReporte.Visible = true;
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Ingreso))
            {
                pnlReport.Visible = true;
                //btnVerDetalle.Visible = true;
                //btnReporte.Visible = true;
                generaReporte();
            }
        }
        //private void generaReporteDetalle()
        //{
        //    //CARGA DATOS GENERALES y se crea datatable
        //    List<cParametroSistema> listConfiguraciones = new cParametroSistemaBL().GetAll();
        //    string NombreMunicipio = listConfiguraciones.FirstOrDefault(c => c.Clave == "NOMBRE_MUNICIPIO").Valor;
        //    string Dependencia = listConfiguraciones.FirstOrDefault(c => c.Clave == "DEPENDENCIA").Valor;
        //    string Area = listConfiguraciones.FirstOrDefault(c => c.Clave == "AREA").Valor;
        //    string UrlLogo = Server.MapPath("~") + "\\" + listConfiguraciones.FirstOrDefault(c => c.Clave == "LOGO").Valor;
        //    FileStream fS = new FileStream(UrlLogo, FileMode.Open, FileAccess.Read);
        //    byte[] LogoByte = new byte[fS.Length];
        //    fS.Read(LogoByte, 0, (int)fS.Length);
        //    fS.Close();
        //    string VoBo = listConfiguraciones.FirstOrDefault(c => c.Clave == "VoBo").Valor;
        //    DataTable ConfGral = new DataTable("ConfGral");
        //    ConfGral.Columns.Add("NombreMunicipio");
        //    ConfGral.Columns.Add("Dependencia");
        //    ConfGral.Columns.Add("Area");
        //    ConfGral.Columns.Add("Logo", typeof(Byte[]));
        //    ConfGral.Columns.Add("Usuario");
        //    ConfGral.Columns.Add("Elaboro");
        //    ConfGral.Columns.Add("VoBo");
        //    tPrepoliza p = new tPrepolizaBL().GetByConstraint(int.Parse(ViewState["prepoliza"].ToString()));

        //    ConfGral.Rows.Add(NombreMunicipio, Dependencia, Area, LogoByte, p.cUsuarios.Nombre + " " + p.cUsuarios.ApellidoPaterno + " " + p.cUsuarios.ApellidoMaterno, p.cUsuarios.Nombre + " " + p.cUsuarios.ApellidoPaterno + " " + p.cUsuarios.ApellidoMaterno, VoBo);

        //    List<tPrepoliza> listaPoliza = new List<tPrepoliza>();
        //    listaPoliza.Add(p);


        //    List<pReporteIngresos_Result> listaDetalle = new pProcedimientos().ObtieneReporteIngreso(int.Parse(ViewState["prepoliza"].ToString()));


        //    //INICIA REPORTE
        //    rpt.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //    rpt.LocalReport.DisplayName = "Reporte de Ingresos";
        //    rpt.LocalReport.ReportPath = "Reportes/Ingresos.rdlc";
        //    rpt.LocalReport.EnableExternalImages = true;
        //    rpt.LocalReport.DataSources.Clear();
        //    rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ConfGral", ConfGral));
        //    rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("Prepoliza", listaPoliza));
        //    rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("PrepolizaDetalle", listaDetalle));
        //    rpt.LocalReport.Refresh();
        //}
        //protected void btnVerDetalle_Click(object sender, EventArgs e)
        //{
        //    generaReporteDetalle();
        //}

        private Decimal total = 0;
        protected void grdReporte_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowType != DataControlRowType.EmptyDataRow)
                && (e.Row.RowType != DataControlRowType.Header))
            {
                string totalcadena = grdReporte.DataKeys[e.Row.RowIndex].Values[1].ToString();

                total += Convert.ToDecimal(totalcadena);

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Totales";
                e.Row.Cells[3].Text = total.ToString("C");
                ViewState["Total"] = total;
            }
        }
    }
}