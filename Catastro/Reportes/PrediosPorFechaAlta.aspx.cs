using Clases;
using Clases.BL;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Catastro.Reportes
{
    public partial class PrediosPorFechaAlta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlReport.Visible = false;
                llenaDdls();
            }
        }
        protected void llenaDdls()
        {
            Int32 Anio = DateTime.Today.Year;
            ddlAnio.Items.Insert(0, new ListItem("Todos", "0"));
            ddlAnio.Items.Insert(1, new ListItem(Anio.ToString(), Anio.ToString()));
            for (Int32 i = 1; i <= 5; i++)
            {
                ddlAnio.Items.Insert(i + 1, new ListItem((Anio - i).ToString(), (Anio - 1).ToString()));
            }

            //foreach (int? anio in new vVistasBL().ObtienePadronAnios(0, 0, 0))
            //{
            //    ListItem li = new ListItem(anio.ToString(), anio.ToString());
            //    ddlAnio.Items.Add(li);
            //}
            //ddlAnio.DataBind();
            ddlAnio.Items.Insert(0, new ListItem("Todos", "0"));            
        }
        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            pnlReport.Visible = true;
            List<cParametroSistema> listConfiguraciones = new cParametroSistemaBL().GetAll();
            string NombreMunicipio = listConfiguraciones.FirstOrDefault(c => c.Clave.ToUpper() == "NOMBRE_MUNICIPIO").Valor;
            string Dependencia = listConfiguraciones.FirstOrDefault(c => c.Clave.ToUpper() == "DEPENDENCIA").Valor;
            string Area = listConfiguraciones.FirstOrDefault(c => c.Clave.ToUpper() == "AREA").Valor;
            string UrlLogo = Server.MapPath("~") + "\\" + listConfiguraciones.FirstOrDefault(c => c.Clave.ToUpper() == "LOGO").Valor;
            FileStream fS = new FileStream(UrlLogo, FileMode.Open, FileAccess.Read);
            byte[] LogoByte = new byte[fS.Length];
            fS.Read(LogoByte, 0, (int)fS.Length);
            fS.Close();

            cUsuarios user = ((cUsuarios)Session["usuario"]);

            DataTable ConfGral = new DataTable("ConfGral");
            ConfGral.Columns.Add("NombreMunicipio");
            ConfGral.Columns.Add("Dependencia");
            ConfGral.Columns.Add("Area");
            ConfGral.Columns.Add("Logo", typeof(Byte[]));

            ConfGral.Rows.Add(NombreMunicipio, Dependencia, Area, LogoByte);

            List<vPadronPredio> litado = new List<vPadronPredio> ();
            if (ddlFiltro.SelectedValue == "fecha")
                litado = new vVistasBL().ObtienePadronByFechaAlta(int.Parse(ddlMes.SelectedValue), int.Parse(ddlAnio.SelectedValue));
            else
                litado = new vVistasBL().ObtienePadronByRangoFechaAlta(DateTime.Parse(txtFechaInicio.Text), DateTime.Parse(txtFechaFin.Text));


            //INICIA REPORTE
            this.rpvtPredios.ProcessingMode = ProcessingMode.Local;
            rpvtPredios.LocalReport.DisplayName = "Padrón de predios";
            rpvtPredios.LocalReport.ReportPath = "Reportes/PrediosFechaAlta.rdlc";
            rpvtPredios.LocalReport.EnableExternalImages = true;
            rpvtPredios.LocalReport.DataSources.Clear();
            rpvtPredios.LocalReport.DataSources.Add(new ReportDataSource("ConfGral", ConfGral));
            rpvtPredios.LocalReport.DataSources.Add(new ReportDataSource("Padron", litado));
            string fullMonthName = "TODOS";
            string AnioName = "TODOS";
            if (ddlMes.SelectedValue != "0")
            {
                fullMonthName = ddlFiltro.SelectedValue == "fecha" ? new DateTime(2015, int.Parse(ddlMes.SelectedValue), 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper() : txtFechaInicio.Text;
            }
            if (ddlAnio.SelectedValue != "0")
            {
                AnioName = ddlFiltro.SelectedValue == "fecha" ?  ddlAnio.SelectedValue : txtFechaFin.Text;
            }
            fullMonthName = ddlFiltro.SelectedValue == "fecha" ? " DEL MES DE " + fullMonthName : " DEL " + fullMonthName;
            AnioName = ddlFiltro.SelectedValue == "fecha" ? " AÑO " + AnioName : " AL " + AnioName;

            ReportParameter parameterSetUp = new ReportParameter("Mes", fullMonthName);
            rpvtPredios.LocalReport.SetParameters(parameterSetUp);
            ReportParameter parameterAnio = new ReportParameter("Anio", AnioName);
            rpvtPredios.LocalReport.SetParameters(parameterAnio);
            rpvtPredios.LocalReport.Refresh();
        }

        protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFiltro.SelectedValue == "fecha")
            {
                Label3.Visible = true;
                Label4.Visible = true;
                txtFechaInicio.Visible = false;
                txtFechaFin.Visible = false;
                txtFechaFin_RequiredFieldValidator.Enabled = false;
                txtFechaInicio_RequiredFieldValidator.Enabled = false;
                ddlAnio.Visible = true;
                ddlMes.Visible = true;
            }
            else
            {
                Label3.Visible = false;
                Label4.Visible = false;
                txtFechaInicio.Visible = true;
                txtFechaFin.Visible = true;
                txtFechaFin_RequiredFieldValidator.Enabled = true;
                txtFechaInicio_RequiredFieldValidator.Enabled = true;
                ddlAnio.Visible = false;
                ddlMes.Visible = false;
            }
        }
    }
}