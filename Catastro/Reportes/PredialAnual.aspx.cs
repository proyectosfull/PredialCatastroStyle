using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases.BL;
using Clases;
using System.Data;
using System.IO;
using Microsoft.Reporting.WebForms;

namespace Catastro.Reportes
{
    public partial class PredialAnual : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<vAnioPredialAnual> anios = new vVistasBL().vAnioPredialAnual();
                ddlanio.DataSource = anios;
                ddlanio.DataTextField = "EjercicioH";
                ddlanio.DataValueField = "EjercicioH";
                ddlanio.DataBind();
            }
        }

        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            int anio = Convert.ToInt32(ddlanio.SelectedItem.Text);
            List<pAnualPredialP1_Result> p1 = new pProcedimientos().pAnualPredialP1(anio);
            if (p1 == null) { p1 = new List<pAnualPredialP1_Result>(); }
            List<pAnualPredialP2_Result> p2 = new pProcedimientos().pAnualPredialP2(anio);
            if (p2 == null) { p2 = new List<pAnualPredialP2_Result>(); }
            List<pAnualPredialP3_Result> p3 = new pProcedimientos().pAnualPredialP3(anio);
            if (p3 == null) { p3 = new List<pAnualPredialP3_Result>(); }
            List<pAnualPredialP4_Result> p4 = new pProcedimientos().pAnualPredialP4(anio);
            if (p4 == null) { p4 = new List<pAnualPredialP4_Result>(); }
            List<pAnualPredialP5_Result> p5 = new pProcedimientos().pAnualPredialP5(anio);
            if (p5 == null) { p5 = new List<pAnualPredialP5_Result>(); }
            List<spCalculaImpuestoHactual_Result> p51 = new pProcedimientos().spCalculaImpuestoHactual(anio);
            List<spCalculaImpuestoH_Result> p52 = new pProcedimientos().spCalculaImpuestoH(anio-1);
            double ImporteSumaTotales=0;
            double DescuentoSumaTotales=0;
            
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

            DataTable conceptoAnualP1 = new DataTable("conceptoAnualP1");
            conceptoAnualP1.Columns.Add("Descripcion");
            conceptoAnualP1.Columns.Add("Importe");
            conceptoAnualP1.Columns.Add("Descuento");
            foreach(pAnualPredialP1_Result p in p1)
            {
                conceptoAnualP1.Rows.Add(p.CONCEPTO,p.RECAUDACION,p.DESCUENTO);
                ImporteSumaTotales = ImporteSumaTotales + Convert.ToDouble(p.RECAUDACION);
                DescuentoSumaTotales = DescuentoSumaTotales + Convert.ToDouble(p.DESCUENTO);
            }
            DataTable conceptoAnualP2 = new DataTable("conceptoAnualP2");
            conceptoAnualP2.Columns.Add("Descripcion");
            conceptoAnualP2.Columns.Add("Importe");
            conceptoAnualP2.Columns.Add("Descuento");
            foreach (pAnualPredialP2_Result p in p2)
            {
                conceptoAnualP2.Rows.Add(p.CONCEPTO, p.ImporteNeto, p.ImporteDescuento);
                ImporteSumaTotales = ImporteSumaTotales + Convert.ToDouble(p.ImporteNeto);
                DescuentoSumaTotales = DescuentoSumaTotales + Convert.ToDouble(p.ImporteDescuento);
            }
            DataTable conceptoAnualP3 = new DataTable("conceptoAnualP3");
            conceptoAnualP3.Columns.Add("Descripcion");
            conceptoAnualP3.Columns.Add("Total");            
            foreach (pAnualPredialP3_Result p in p3)
            {
                conceptoAnualP3.Rows.Add(p.Descripcion, p.N_PREDIOS);
            }
            DataTable conceptoAnualP4 = new DataTable("conceptoAnualP4");
            conceptoAnualP4.Columns.Add("Descripcion");
            conceptoAnualP4.Columns.Add("Importe");            
            foreach (pAnualPredialP4_Result p in p4)
            {
                conceptoAnualP4.Rows.Add(p.CONCEPTO, p.RECAUDACION);
            }
            DataTable conceptoAnualP5 = new DataTable("conceptoAnualP5");
            conceptoAnualP5.Columns.Add("Descripcion");
            conceptoAnualP5.Columns.Add("Total");            
            foreach (pAnualPredialP5_Result p in p5)
            {
                conceptoAnualP5.Rows.Add(p.DESCRIPCION,p.PADRON);
            }
            foreach (spCalculaImpuestoHactual_Result p in p51)
            {
                conceptoAnualP5.Rows.Add("(18) RECAUDACIÓN POTENCIAL DEL EJERCICIO (LEY DE INGRESOS)", String.Format("{0:C}", p.actual));
            }
            foreach (spCalculaImpuestoH_Result p in p52)
            {
                conceptoAnualP5.Rows.Add("(19) RECAUDACIÓN POTENCIAL EJERCICIOS ANTERIORES (REZAGO, LEY DE INGRESOS)", String.Format("{0:C}", p.Rezago));
            }

            ////INICIA REPORTE
            rpt.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            rpt.LocalReport.DisplayName = "Reporte Predial Anual";
            rpt.LocalReport.ReportPath = "Reportes/PredialAnual.rdlc";
            rpt.LocalReport.EnableExternalImages = true;
            rpt.LocalReport.DataSources.Clear();
            rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ConfGral", ConfGral));
            rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("conceptoAnualP1", conceptoAnualP1));
            rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("conceptoAnualP2", conceptoAnualP2));
            rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("conceptoAnualP3", conceptoAnualP3));
            rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("conceptoAnualP4", conceptoAnualP4));
            rpt.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("conceptoAnualP5", conceptoAnualP5));

            string aniosAnt = (anio - 1).ToString() + ", " + (anio - 2).ToString() + ", " + (anio - 3).ToString() + ", " + (anio - 4).ToString() + ", " + (anio - 5).ToString();
            List<ReportParameter> paramList = new List<ReportParameter>();
            paramList.Add(new ReportParameter("anio", anio.ToString(), false));
            paramList.Add(new ReportParameter("anioAnt", aniosAnt, false));
            paramList.Add(new ReportParameter("ImporteSumaTotales", ImporteSumaTotales.ToString(), false));
            paramList.Add(new ReportParameter("DescuentoSumaTotales", DescuentoSumaTotales.ToString(), false));
            this.rpt.LocalReport.SetParameters(paramList);
            rpt.LocalReport.Refresh();
        }
    }
}