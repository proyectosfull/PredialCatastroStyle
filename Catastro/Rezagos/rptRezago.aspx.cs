using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using Clases.BL;
using Clases.Utilerias;
using System.Data;
using System.Text;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Threading;

namespace Catastro.Rezagos
{
    public partial class rptRezago : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //lblCalculando.Visible = false;
                ViewState["sortCampo"] = "Nombre";
                ViewState["sortOrden"] = "asc";
                //Llenado del dropdownlist de año
                int mesCobroAnt = Convert.ToInt32(new cParametroCobroBL().GetByClave("MesPagoAnticipado"));              

                Int32 Anio = DateTime.Today.Year;
                Int32 ini = 1;
                ddlAnio.Items.Insert(0, new ListItem("Año", "0"));
                if (mesCobroAnt <= DateTime.Today.Month)
                {
                    ddlAnio.Items.Insert(ini, new ListItem((Anio+1).ToString(), (Anio+1).ToString()));
                    ini = 2;
                }
               
                ddlAnio.Items.Insert(ini, new ListItem(Anio.ToString(), Anio.ToString()));
                Int32 j = 1;
                for (Int32 i = ini; i <= 5; i++)
                {
                    ddlAnio.Items.Insert(i + 1, new ListItem((Anio - j).ToString(), (Anio - 1).ToString()));
                    j++;
                }

                DateTime d = DateTime.Now;
                if (d.Month > 2)
                {
                    ddlAnio.SelectedValue = DateTime.Now.Year.ToString();
                }
                else
                {
                    int y = DateTime.Now.Year - 1;
                    ddlAnio.SelectedValue = y.ToString();
                }

                ddlBimestre.SelectedValue = "6";

                ddlCondominio.DataValueField = "Id";
                ddlCondominio.DataTextField = "Descripcion";
                ddlCondominio.DataSource = new cCondominioBL().GetAll();
                ddlCondominio.DataBind();
                ddlCondominio.Items.Insert(0, new ListItem("TODOS", "0"));
            }

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //gridResultado.DataSource =  dtReporteRezago();
            //gridResultado.DataBind();
            lblCalculando.Text = "Procesando...";
            Int64 rangoDe = 0;
            Int64 rangoA = 0;
            if  (txtRangoDe.Text != "")
                rangoDe = Convert.ToInt64(txtRangoDe.Text);
            if (txtRangoA.Text != "")
                rangoA = Convert.ToInt64(txtRangoA.Text);
            int bimFinal = Convert.ToInt32( ddlBimestre.Text );
            int ejFinal = Convert.ToInt32(ddlAnio.Text);
            //string texto = ddlCondominio.Text;
            int idCondominio = Convert.ToInt32(ddlCondominio.SelectedValue);
            int idColonia = hdfIdColonia.Value==string.Empty?0:Convert.ToInt32(hdfIdColonia.Value);

            DataTable dt = dtReporteRezago(ejFinal, bimFinal, false, false, false, false, rangoDe,rangoA , 0, 0, idCondominio, idColonia, txtClave.Text);
            //(int ejFinal, int bimFinal, bool sinBaldio, bool soloExento, bool sinExento, bool soloBaldio, Int64 rangoDe, Int64 rangoA, int idContribuyente, int idFase, int idCondominio, int idColonia, string claves)
            lblCalculando.Text = "Procesando..." + dt.Rows.Count.ToString();

            List<cParametroSistema> listConfiguraciones = new cParametroSistemaBL().GetAll();
            string NombreMunicipio = listConfiguraciones.FirstOrDefault(c => c.Clave.ToUpper() == "NOMBRE_MUNICIPIO").Valor;
            string Dependencia = listConfiguraciones.FirstOrDefault(c => c.Clave.ToUpper() == "DEPENDENCIA").Valor;
            string Area = listConfiguraciones.FirstOrDefault(c => c.Clave.ToUpper() == "AREA").Valor;
            string UrlLogo = Server.MapPath("~") + "\\" + listConfiguraciones.FirstOrDefault(c => c.Clave.ToUpper() == "LOGO").Valor;
            FileStream fS = new FileStream(UrlLogo, FileMode.Open, FileAccess.Read);
            byte[] LogoByte = new byte[fS.Length];
            fS.Read(LogoByte, 0, (int)fS.Length);
            fS.Close();

            DataTable ConfGral = new DataTable("ConfGral");
            ConfGral.Columns.Add("NombreMunicipio");
            ConfGral.Columns.Add("Dependencia");
            ConfGral.Columns.Add("Area");
            ConfGral.Columns.Add("Logo", typeof(Byte[]));
            ConfGral.Rows.Add(NombreMunicipio, Dependencia, Area, LogoByte);

            // INICIA REPORTE
            this.rpCalcula.ProcessingMode = ProcessingMode.Local;
            rpCalcula.LocalReport.DisplayName = "Padrón de Rezago";
            rpCalcula.LocalReport.ReportPath = "Rezagos/CalculoRezago.rdlc";
            rpCalcula.LocalReport.EnableExternalImages = true;
            rpCalcula.LocalReport.DataSources.Clear();
            rpCalcula.LocalReport.DataSources.Add(new ReportDataSource("ConfGral", ConfGral));
            rpCalcula.LocalReport.DataSources.Add(new ReportDataSource("CalculoRezago",dt ));
            rpCalcula.LocalReport.Refresh();

        }

        protected DataTable dtReporteRezago(int ejFinal, int bimFinal, bool sinBaldio, bool soloExento, bool sinExento, bool soloBaldio, Int64 rangoDe, Int64 rangoA, int idContribuyente, int idFase, int idCondominio, int idColonia, string claves)
        {
            #region
            /// 0-Año Periodo, 1-Bimestre Periodo, 
            /// 2-Ignorar Baldíos
            /// 3-Solo Exento
            /// 4-Ignorar Exento
            /// 5-Baldios
            /// 6-Rango Incial, 7-Rango Final
            /// 8-Contribuyente
            /// 9-Claves</param>
            /// 10-IdFase
            /// 11-IdCondominio
            /// 12-Idcolonia</param>
            DataTable dt = new DataTable();
            Impuesto i = new Impuesto();

            dt.Columns.Add("ClavePredial");
            dt.Columns.Add("Propietario");
            dt.Columns.Add("Domicilio");
            dt.Columns.Add("Condominio");
            dt.Columns.Add("Periodo");
            dt.Columns.Add("SuperTerreno");
            dt.Columns.Add("SuperConstruccion");
            dt.Columns.Add("ValorTerreno");
            dt.Columns.Add("ValorConstruccion");                    
            dt.Columns.Add("ImpuestoAnt");
            dt.Columns.Add("AdicionalAnt");
            dt.Columns.Add("Impuesto");
            dt.Columns.Add("Adicional");
            dt.Columns.Add("Diferencias");
            dt.Columns.Add("RecDiferencias");
            dt.Columns.Add("Rezagos");
            dt.Columns.Add("Recargos");
            dt.Columns.Add("Honorarios");
            dt.Columns.Add("Ejecucion");
            dt.Columns.Add("Multas");
            dt.Columns.Add("Descuentos");
            dt.Columns.Add("Importe");
            //dt.Columns.Add("IdPredio");
            #endregion
                       
            int j = 1;
            lblCalculando.Visible = true;

            // String[] param = { "2017", "6", "false", "false", "false", "false", rangoDe, rangoA, "", "", "0" };
            
            
            List<cPredio> lPredio = new cPredioBL().ObtenerPrediosCalculoRezago( ejFinal, bimFinal, sinBaldio, soloExento, sinExento, soloBaldio, rangoDe, rangoA, idContribuyente, claves, idFase, idCondominio, idColonia);
            
            ////hilos List<cPredio>
            ////se crea lista agrupada de 1000
            //List<List<cPredio>> llPredio = cPredioAgrupado( lPredio);

            //SaldosRezago r = new SaldosRezago();
            ////new Thread(() => NewThread(name, lastname));
            //Thread hilo = new Thread(() => new SaldosRezago().GeneraRezago(llPredio[0], ejFinal, bimFinal));                 
            //hilo.Start();
            //hilo.Join();
            //dt = r.RezagoResultado(); 
            ////**


            foreach (cPredio p in lPredio)
            {
                i = new SaldosC().InicializaIP(i);

                //lblCalculando.Text = " Calculando... " + lPredio.Count() + " / " + j.ToString();
                if ( ( p.AaFinalIp *10 )+ p.BimestreFinIp  <= (ejFinal *10)+bimFinal )
                    i = new SaldosC().CalculaCobro(p.Id, "NO", p.BimestreFinIp, p.AaFinalIp, bimFinal, ejFinal, 0, 0, "rptRezago");

                #region llena datatable
                if ((i.TextError == null || i.TextError == "") &&  i.mensaje == 0 )
                {
                    DataRow dr = dt.NewRow();
                    dr["ClavePredial"] = p.ClavePredial;
                    dr["Propietario"] =  p.cContribuyente.ApellidoPaterno + " " + p.cContribuyente.ApellidoMaterno + " " + p.cContribuyente.Nombre;
                    dr["Domicilio"] = p.Calle + " NÚM. " + p.Numero + " COL. " + p.cColonia.NombreColonia + " C.P. " + p.CP + " LOCALIDAD " + p.Localidad; ;
                    dr["Condominio"] = p.cCondominio.Descripcion;                    
                    dr["Periodo"] = i.Estado.PeriodoGral;
                    dr["SuperTerreno"] = p.SuperficieTerreno;
                    dr["SuperConstruccion"] = p.SuperficieConstruccion;
                    dr["ValorTerreno"] = p.ValorTerreno;
                    dr["ValorConstruccion"] = p.ValorConstruccion;   
                    dr["ImpuestoAnt"] = i.Estado.AntImpuesto > 0 ? i.Estado.AntImpuesto : 0 ;
                    dr["AdicionalAnt"] = i.Estado.AntAdicional > 0 ? i.Estado.AntAdicional :0;
                    dr["Impuesto"] = i.Estado.Impuesto > 0 ? i.Estado.Impuesto: 0;
                    dr["Adicional"] = i.Estado.Adicional> 0 ? i.Estado.Adicional : 0;
                    dr["Diferencias"] = i.Estado.Diferencias;
                    dr["RecDiferencias"] = i.Estado.RecDiferencias;
                    dr["Rezagos"] = i.Estado.Rezagos;
                    dr["Recargos"] = i.Estado.Recargos;
                    dr["Honorarios"] = i.Estado.Honorarios;
                    dr["Ejecucion"] = i.Estado.Ejecucion > 0 ?i.Estado.Ejecucion : 0;
                    dr["Multas"] = i.Estado.Multas;
                    dr["Descuentos"] = i.Estado.Descuentos;
                    dr["Importe"] = i.Estado.Importe;

                    dt.Rows.Add(dr);
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    dr["ClavePredial"] = p.ClavePredial;
                    dr["Propietario"] = i.TextError;
                    dr["Domicilio"] = "-";// p.Calle + " NÚM. " + p.Numero + " COL. " + p.cColonia.NombreColonia + " C.P. " + p.CP + " LOCALIDAD " + p.Localidad; ;
                    dr["Condominio"] = "-";// p.cCondominio.Descripcion;
                    dr["Periodo"] = "-";// p.BimestreFinIp.ToString() + " " + p.AaFinalIp.ToString();
                    dr["SuperConstruccion"] = 0;// p.SuperficieConstruccion;
                    dr["ValorTerreno"] = 0;// p.ValorTerreno;
                    dr["ValorConstruccion"] = 0;// p.ValorConstruccion;                   
                    dr["ImpuestoAnt"] = 0;
                    dr["AdicionalAnt"] = 0 ;
                    dr["Impuesto"] = 0;
                    dr["Adicional"] = 0;
                    dr["Diferencias"] = 0;
                    dr["RecDiferencias"] = 0;
                    dr["Rezagos"] = 0;
                    dr["Recargos"] = 0;
                    dr["Honorarios"] = 0;
                    dr["Ejecucion"] = 0;
                    dr["Multas"] = 0;
                    dr["Descuentos"] = 0;
                    dr["Importe"] = 0;
                    dt.Rows.Add(dr);
                }
                j++;
            }
            #endregion

            return dt;
        }

        private void llenagrid()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new cContribuyenteBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOrden"].ToString());
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new cContribuyenteBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOrden"].ToString());
                grd.DataBind();

            }
            grd.Visible = true;
            pnl_Modal.Show();
        }

        private void llenagridCondominio()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grdCon.DataSource = new cCondominioBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOrden"].ToString());
                grdCon.DataBind();
            }
            else
            {
                grdCon.DataSource = new cCondominioBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOrden"].ToString());
                grdCon.DataBind();

            }
            grdCon.Visible = true;
            pnl_Modal.Show();
        }

        private void llenagridColonia()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grdCol.DataSource = new cColoniaBL().GetFilter("", "", "TRUE", "NombreColonia", ViewState["sortOrden"].ToString());
                grdCol.DataBind();
            }
            else
            {
                grdCol.DataSource = new cColoniaBL().GetFilter(filtro[0], filtro[1], filtro[2], "NombreColonia", ViewState["sortOrden"].ToString());
                grdCol.DataBind();

            }
            grdCol.Visible = true;
            pnl_Modal.Show();
        }

        protected void imgBorrarCon_Click(object sender, ImageClickEventArgs e)
        {
            txtContribuyente.Text = "";
            hdfIdCon.Value = "";
        }

        //protected void imbBorrarCondominio_Click(object sender, ImageClickEventArgs e)
        //{
        //    txtCondominio.Text = "";
        //    hdfIdCondominio.Value = "";
        //}

        protected void imbBorrarColonia_Click(object sender, ImageClickEventArgs e)
        {
            txtColonia.Text = "";
            hdfIdColonia.Value = "";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtFiltroContribuyente.Text = "";
            grd.DataSource = null;
            grd.DataBind();
            grd.Visible = false;
            grdCon.DataSource = null;
            grdCon.DataBind();
            grdCon.Visible = false;
            grdCol.DataSource = null;
            grdCol.DataBind();
            grdCol.Visible = false;
            pnl_Modal.Hide();
        }

        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["sortCampo"] = "Nombre";
            lblbuscarContribuyente.Text = "Buscar Contribuyente";
            llenaFiltroContribuyente();
            pnl_Modal.Show();
        }

        protected void imbCondominio_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["sortCampo"] = "Descripcion";
            lblbuscarContribuyente.Text = "Buscar Condominio";
            llenaFiltroCondominio();
            pnl_Modal.Show();
        }

        protected void imbColonia_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["sortCampo"] = "NombreColonia";
            lblbuscarContribuyente.Text = "Buscar Colonia";
            llenaFiltroColonia();
            pnl_Modal.Show();
        }

        private void llenaFiltroCondominio()
        {
            ddlFiltroContribuyente.Items.Clear();
            ddlFiltroContribuyente.Items.Insert(0, new ListItem("Todos", ""));
            ddlFiltroContribuyente.Items.Insert(1, new ListItem("Nombre", "Descripcion"));
        }

        private void llenaFiltroColonia()
        {
            ddlFiltroContribuyente.Items.Clear();
            ddlFiltroContribuyente.Items.Insert(0, new ListItem("Todos", ""));
            ddlFiltroContribuyente.Items.Insert(1, new ListItem("Nombre", "NombreColonia"));
        }

        private void llenaFiltroContribuyente()
        {
            ddlFiltroContribuyente.Items.Clear();
            ddlFiltroContribuyente.Items.Insert(0, new ListItem("Todos", ""));
            ddlFiltroContribuyente.Items.Insert(1, new ListItem("Nombre", "Nombre"));
            //ddlFiltroContribuyente.Items.Insert(2, new ListItem("Apellido Paterno", "ApellidoPaterno"));
            //ddlFiltroContribuyente.Items.Insert(3, new ListItem("Apellido Materno", "ApellidoMaterno"));
            //ddlFiltroContribuyente.Items.Insert(4, new ListItem("CURP", "Curp"));
        }

        protected void imbBuscarContribuyenteFiltro_Click(object sender, ImageClickEventArgs e)
        {
            string[] filtro = new string[] { ddlFiltroContribuyente.SelectedValue, txtFiltroContribuyente.Text, "true" };
            ViewState["filtro"] = filtro;
            if (lblbuscarContribuyente.Text.Contains("Contribuyente"))
                llenagrid();
            else if (lblbuscarContribuyente.Text.Contains("Condominio"))
                llenagridCondominio();
            else
                llenagridColonia();
        }

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SeleccionarPersona")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;
                cContribuyenteBL cBL = new cContribuyenteBL();
                cContribuyente contribuyente = cBL.GetByConstraint(id);
                txtContribuyente.Text =  contribuyente.ApellidoPaterno + " " + contribuyente.ApellidoMaterno + " " +contribuyente.Nombre ;
                hdfIdCon.Value = contribuyente.Id.ToString();
                txtFiltroContribuyente.Text = "";
                grd.DataSource = null;
                grd.DataBind();
                grd.Visible = false;
                pnl_Modal.Hide();
            }

        }

        protected void grdCon_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SeleccionarPersona")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;
                cCondominioBL cBL = new cCondominioBL();
                cCondominio condominio = cBL.GetByConstraint(id);
                //txtCondominio.Text = condominio.Descripcion;
                //hdfIdCondominio.Value = condominio.Id.ToString();
                txtFiltroContribuyente.Text = "";
                grdCon.DataSource = null;
                grdCon.DataBind();
                grdCon.Visible = false;
                pnl_Modal.Hide();
            }
        }

        protected void grdCol_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SeleccionarPersona")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;
                cColoniaBL cBL = new cColoniaBL();
                cColonia colonia = cBL.GetByConstraint(id);
                txtColonia.Text = colonia.NombreColonia;
                hdfIdColonia.Value = colonia.Id.ToString();
                txtFiltroContribuyente.Text = "";
                grdCol.DataSource = null;
                grdCol.DataBind();
                grdCol.Visible = false;
                pnl_Modal.Hide();
            }

        }

        protected void grd_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["sortCampo"] == null)
            {
                ViewState["sortCampo"] = e.SortExpression.ToString();
                ViewState["sortOrden"] = "asc";
            }
            else
            {
                if (e.SortExpression.ToString() == ViewState["sortCampo"].ToString())
                {
                    if (ViewState["sortOrden"].ToString() == "asc")
                        ViewState["sortOrden"] = "desc";
                    else
                        ViewState["sortOrden"] = "asc";
                }
                else
                {
                    ViewState["sortCampo"] = e.SortExpression.ToString();
                    ViewState["sortOrden"] = "asc";
                }
            }

            llenagrid();
        }

        protected void grdCon_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["sortCampo"] == null)
            {
                ViewState["sortCampo"] = e.SortExpression.ToString();
                ViewState["sortOrden"] = "asc";
            }
            else
            {
                if (e.SortExpression.ToString() == ViewState["sortCampo"].ToString())
                {
                    if (ViewState["sortOrden"].ToString() == "asc")
                        ViewState["sortOrden"] = "desc";
                    else
                        ViewState["sortOrden"] = "asc";
                }
                else
                {
                    ViewState["sortCampo"] = e.SortExpression.ToString();
                    ViewState["sortOrden"] = "asc";
                }
            }

            llenagridCondominio();
        }

        protected void grdCol_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["sortCampo"] == null)
            {
                ViewState["sortCampo"] = e.SortExpression.ToString();
                ViewState["sortOrden"] = "asc";
            }
            else
            {
                if (e.SortExpression.ToString() == ViewState["sortCampo"].ToString())
                {
                    if (ViewState["sortOrden"].ToString() == "asc")
                        ViewState["sortOrden"] = "desc";
                    else
                        ViewState["sortOrden"] = "asc";
                }
                else
                {
                    ViewState["sortCampo"] = e.SortExpression.ToString();
                    ViewState["sortOrden"] = "asc";
                }
            }

            llenagridColonia();
        }

        protected void grdResultados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            cTipoPredioBL tpBL = new cTipoPredioBL();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Text = e.Row.Cells[1].Text == "&nbsp;" || e.Row.Cells[1].Text == "" ? "" : tpBL.GetByConstraint(Convert.ToInt32(e.Row.Cells[1].Text)).Descripcion;
            }
        }

        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd.PageIndex = e.NewPageIndex;
            llenagrid();
        }

        protected void grdCon_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCon.PageIndex = e.NewPageIndex;
            llenagridCondominio();
        }

        protected void grdCol_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCon.PageIndex = e.NewPageIndex;
            llenagridColonia();
        }

        public List<List<cPredio>> cPredioAgrupado(List<cPredio> lpredio)
        {
            List<List<cPredio>> llp = new List<List<cPredio>>();
            List<cPredio> p = new List<cPredio>();
            int i = 0;
            int j = 3;
            foreach (cPredio item in lpredio)
            {
                if (i == lpredio.Count() - 1)
                {
                    j = i;
                }
                p.Add(item);
                if (i == j)
                {
                    llp.Add(p);
                    p = new List<cPredio>();
                    j = j + 3;
                }
                i++;
            }
            return llp;
        }


    }
}