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
using Microsoft.Reporting.WebForms;

namespace Catastro.Recibos
{
    public partial class ReporteRecibos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlReport.Visible = false;
                llenaListas();
            }
        }
        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            pnlReport.Visible = true;
            //CARGA DATOS GENERALES y se crea datatable
            List<cParametroSistema> listConfiguraciones = new cParametroSistemaBL().GetAll();
            string NombreMunicipio = listConfiguraciones.FirstOrDefault(c => c.Clave == "NOMBRE_MUNICIPIO").Valor;
            string Dependencia = listConfiguraciones.FirstOrDefault(c => c.Clave == "DEPENDENCIA").Valor;
            string Area = listConfiguraciones.FirstOrDefault(c => c.Clave == "AREA").Valor;
            string UrlLogo = Server.MapPath("~") + listConfiguraciones.FirstOrDefault(c => c.Clave == "LOGO").Valor;
            FileStream fS = new FileStream(UrlLogo, FileMode.Open, FileAccess.Read);
            byte[] LogoByte = new byte[fS.Length];
            fS.Read(LogoByte, 0, (int)fS.Length);
            fS.Close();          

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
            cUsuarios U = (cUsuarios)Session["usuario"];          
            string nombre = U.Nombre + " " + U.ApellidoPaterno + " " + U.ApellidoMaterno;
            ConfGral.Rows.Add(NombreMunicipio, Dependencia, Area, LogoByte, "", "", nombre, "", "");

            DateTime fin = Convert.ToDateTime(txtFechaFin.Text + " 23:59:59");
            DateTime inicio = Convert.ToDateTime(txtFechaInicio.Text);
            List<vReciboReporteDet> listRecibos= new vVistasBL().ObtieneReciboReporteDet(Convert.ToInt32(ddlCajero.SelectedValue),ddlEstado.SelectedValue,Convert.ToInt32(ddlTipoTramite.SelectedValue),inicio, fin);

            List<vReciboReporteDet> listRecibosFinal = new List<vReciboReporteDet>();
            if (ddlEstado.SelectedItem.Value.ToString() == "%" && ddlTipoTramite.SelectedItem.Value == "0" && ddlCajero.SelectedItem.Value == "0")
            {
                if (chkInhabilitados.Checked == true)
                {

                    int rMin;
                    int rMax = listRecibos.Max(x => x.Id);
                    for (rMin = listRecibos.Min(x => x.Id); rMin <= rMax; rMin++)
                    {
                        List<vReciboReporteDet> listRec = listRecibos.Where(lrec => lrec.Id == rMin).ToList();
                        if (listRec.Count < 1)
                        {
                            vReciboReporteDet rnew = new vReciboReporteDet();
                            rnew.Id = rMin;
                            rnew.ClavePredial = "-";
                            rnew.Tramite = "-";
                            rnew.Cajero = "-";
                            rnew.ImporteDescuento = 0;
                            rnew.ImporteTotal = 0;
                            rnew.ImporteNeto = 0;
                            rnew.EstadoRecibo = "I";
                            rnew.Nombre = "INHABILITADO POR EL SISTEMA";
                            listRecibosFinal.Add(rnew);
                        }
                        else
                        {
                            foreach (vReciboReporteDet r in listRec)
                            {
                                listRecibosFinal.Add(r);
                            }
                        }
                    }
                }
                else
                {
                    listRecibosFinal = listRecibos;
                }
                ////INICIA REPORTE
                rpt.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                rpt.LocalReport.DisplayName = "Reporte Ingresos";
                rpt.LocalReport.ReportPath = "Reportes/RecibosRpt.rdlc";
                //rpt.LocalReport.ReportPath = "Reportes/RecibosListRpt.rdlc";
                rpt.LocalReport.EnableExternalImages = true;
                rpt.LocalReport.DataSources.Clear();
                rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ConfGral", ConfGral));
                rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("Recibos", listRecibosFinal));
                List<ReportParameter> paramList = new List<ReportParameter>();
                paramList.Add(new ReportParameter("fInicio", inicio.ToString("dd/MM/yyyy"), false));
                paramList.Add(new ReportParameter("fFin", fin.ToString("dd/MM/yyyy"), false));
                this.rpt.LocalReport.SetParameters(paramList);
                rpt.LocalReport.Refresh();
            }
            else
            {

                ////INICIA REPORTE
                rpt.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                rpt.LocalReport.DisplayName = "Reporte Ingresos";
                rpt.LocalReport.ReportPath = "Reportes/RecibosRpt.rdlc";
                rpt.LocalReport.EnableExternalImages = true;
                rpt.LocalReport.DataSources.Clear();
                rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ConfGral", ConfGral));
                rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("Recibos", listRecibos));
                List<ReportParameter> paramList = new List<ReportParameter>();
                paramList.Add(new ReportParameter("fInicio", inicio.ToString("dd/MM/yyyy"), false));
                paramList.Add(new ReportParameter("fFin", fin.ToString("dd/MM/yyyy"), false));
                this.rpt.LocalReport.SetParameters(paramList);
                rpt.LocalReport.Refresh();
            }
            
            
        }

        private void llenaListas()
        {
            ddlTipoTramite.DataValueField = "Id";
            ddlTipoTramite.DataTextField = "Tramite";
            ddlTipoTramite.DataSource = new cTipoTramiteBL().GetAll();
            ddlTipoTramite.DataBind();
            ddlTipoTramite.Items.Insert(0, new ListItem("TODOS", "0"));

            ddlCajero.DataValueField = "IdCajero";
            ddlCajero.DataTextField = "Usuario";
            ddlCajero.DataSource = new vVistasBL().cajeros();
            ddlCajero.DataBind();
            ddlCajero.Items.Insert(0, new ListItem("TODOS", "0"));


        }     
    }
}