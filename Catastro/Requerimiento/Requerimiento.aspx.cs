using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Clases;
<<<<<<< HEAD:Catastro/Catastro/Rezagos/Requerimiento.aspx.cs
using Clases.BL;
using Clases.Utilerias;
using iTextSharp.text.pdf;
using System.IO;
using Catastro.Controles;
using System.Transactions;

namespace Catastro
{
    public partial class Requerimiento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["sortCampo"] = "Nombre";
                ViewState["sortOrden"] = "asc";
                //Llenado del dropdownlist de año
                Int32 Anio = DateTime.Today.Year;
                ddlAnio.Items.Insert(0, new ListItem("Año", "0"));
                ddlAnio.Items.Insert(1, new ListItem(Anio.ToString(), Anio.ToString()));
                for (Int32 i = 1; i <= 5; i++)
                {
                    ddlAnio.Items.Insert(i + 1, new ListItem((Anio - i).ToString(), (Anio - 1).ToString()));
                }
                //Se muestra el grid de Seleccionados vacío
                DataTable dt = new DataTable();
                dt.Columns.Add("ClavePredial");
                dt.Columns.Add("Tipo");
                dt.Columns.Add("Periodo");
                dt.Columns.Add("IdPredio");
                dt.Columns.Add("IdTipoPredio");
                DataRow row = dt.NewRow();
                row["IdPredio"] = "";
                row["IdTipoPredio"] = "";
                row["ClavePredial"] = "";
                row["Tipo"] = "";
                row["Periodo"] = "";
                dt.Rows.Add(row);
                grdResultados.DataSource = dt;
                grdResultados.DataBind();
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
                ddlAsignar.DataSource = dtAgente;
                ddlAsignar.DataTextField = "Nombre";
                ddlAsignar.DataValueField = "Id";
                ddlAsignar.DataBind();
                //Se asgina la fecha de hoy como default al cuadro de texto fecha
                txtFecha.Text = DateTime.Today.ToShortDateString();
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }
        /// <summary>
        /// Muestra la modal para buscar contribuyente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            llenaFiltroContribuyente();
            pnl_Modal.Show();
        }
        /// <summary>
        /// Llena el fil
        /// </summary>
        private void llenaFiltroContribuyente()
        {
            ddlFiltroContribuyente.Items.Clear();
            ddlFiltroContribuyente.Items.Insert(0, new ListItem("Todos", ""));
            ddlFiltroContribuyente.Items.Insert(1, new ListItem("Nombre", "Nombre"));
            ddlFiltroContribuyente.Items.Insert(2, new ListItem("Apellido Paterno", "ApellidoPaterno"));
            ddlFiltroContribuyente.Items.Insert(3, new ListItem("Apellido Materno", "ApellidoMaterno"));
            ddlFiltroContribuyente.Items.Insert(4, new ListItem("CURP", "Curp"));
        }
        /// <summary>
        /// Realiza la búsquedas de contribuyentes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbBuscarContribuyenteFiltro_Click(object sender, ImageClickEventArgs e)
        {
            string[] filtro = new string[] { ddlFiltroContribuyente.SelectedValue, txtFiltroContribuyente.Text, "true" };
            ViewState["filtro"] = filtro;
            llenagrid();
        }
        /// <summary>
        /// Llena el grid de búsqueda de Contribuyente
        /// </summary>
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
            pnl_Modal.Show();
        }
        /// <summary>
        /// Código de indexación o paginación del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd.PageIndex = e.NewPageIndex;
            llenagrid();
        }
        /// <summary>
        /// Cierra la modal de Búsqueda de Contribuyente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnl_Modal.Hide();
        }
        /// <summary>
        /// Obtiene los predios que coinciden con los criterios dados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            String[] param = { ddlAnio.SelectedValue, ddlBimestre.SelectedValue, chkBaldios.Checked.ToString(), chkExentos.Checked.ToString(), chkPExentos.Checked.ToString(), chkUBaldios.Checked.ToString(), txtRangoDe.Text, txtRangoA.Text, hdfIdCon.Value, txtClave.Text };
            cPredioBL pBL = new cPredioBL();
            grdResultados.DataSource = pBL.ObtenerPrediosRequerimientos(param);
            grdResultados.DataBind();
        }
        /// <summary>
        /// Código para lo botones del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SeleccionarPersona")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;
                cContribuyenteBL cBL = new cContribuyenteBL();
                cContribuyente contribuyente = cBL.GetByConstraint(id);
                txtContribuyente.Text = contribuyente.Nombre + " " + contribuyente.ApellidoPaterno + " " + contribuyente.ApellidoMaterno;
                hdfIdCon.Value = contribuyente.Id.ToString();
                pnl_Modal.Hide();
            }

        }
        /// <summary>
        /// Código de ordenado del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            List<List<string>> parametros = new List<List<string>>();

            for (int i = 0; i < grdResultados.Rows.Count; i++)
            {
                List<string> param = new List<string>();
                String[] periodo = grdResultados.Rows[i].Cells[2].Text.Split('-');
                Int32 EIni = Convert.ToInt32(periodo[0]);
                Int32 BIni = Convert.ToInt32(periodo[1]);

                param.Add(grdResultados.DataKeys[i].Values["IdPredio"].ToString());
                param.Add("NO");
                param.Add(BIni.ToString());
                param.Add(EIni.ToString());
                param.Add(ddlBimestre.SelectedValue.ToString());
                param.Add(ddlAnio.SelectedValue.ToString());
                param.Add("-1");
                param.Add(txtFecha.Text);
                parametros.Add(param);

                //Impuesto impuesto = new SaldosC().CalculaCobro(Convert.ToInt32(grdResultados.DataKeys[i].Values["IdPredio"]), "NO", BIni, EIni, Convert.ToInt32(ddlBimestre.SelectedValue), Convert.ToInt32(ddlAnio.SelectedValue),0);
            }
            DataTable dtEdosCta = new tRequerimientoBL().GenerarEdoCuenta(parametros);
            grdEdoCta.DataSource = dtEdosCta;
            grdEdoCta.DataBind();
            TabContainerRequerimiento.ActiveTabIndex = 2;
        }

        protected void grdResultados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            cTipoPredioBL tpBL = new cTipoPredioBL();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Text = e.Row.Cells[1].Text == "&nbsp;" || e.Row.Cells[1].Text == "" ? "" : tpBL.GetByConstraint(Convert.ToInt32(e.Row.Cells[1].Text)).Descripcion;
            }
        }

        protected void btnRequerir_Click(object sender, EventArgs e)
        {
            Calcula();
            vtnModal.ShowPopup("¿Se imprimieron correctamente los requerimientos?", ModalPopupMensaje.TypeMesssage.Confirm);
        }
        protected void Calcula()
        {

            string Path = Server.MapPath("~/");
            foreach (GridViewRow row in grdEdoCta.Rows)
            {
                Boolean bConsiderar = true;
                if (txtRRangoA.Text != "" && txtRRanggoB.Text != "")
                {
                    long RA = Convert.ToInt64(txtRRangoA.Text);
                    long RB = Convert.ToInt64(txtRRanggoB.Text);
                    long cve = Convert.ToInt64(row.Cells[0].Text);
                    if (!(cve >= RA && cve <= RB))
                        bConsiderar = false;

                }

                if (bConsiderar)
                {
                    string formOriginal = Path + "/Documentos/Requerimiento.pdf";

                    string ruta = row.Cells[0].Text + "-" + DateTime.Now.Millisecond.ToString() + ".pdf";
                    ViewState["pdf"] = ruta;
                    string formImprimir = Path + "/Temporales/" + ruta;

                    PdfReader reader = new PdfReader(formOriginal);
                    PdfStamper stamper = new PdfStamper(reader, new FileStream(formImprimir, FileMode.Create));

                    AcroFields fields = stamper.AcroFields;
                    fields.SetField("Contribuyente", row.Cells[2].Text);
                    fields.SetField("Domicilio", row.Cells[1].Text);
                    fields.SetField("ClaveCatastral", row.Cells[0].Text);
                    cPredio predio = new cPredioBL().GetByClavePredial(row.Cells[0].Text);
                    fields.SetField("SuperficeTerreno", predio.SuperficieTerreno.ToString());
                    fields.SetField("ValorTerreno", predio.ValorTerreno.ToString());
                    fields.SetField("SuperficieConstruccion", predio.SuperficieConstruccion.ToString());
                    fields.SetField("ValorConstruccion", predio.ValorConstruccion.ToString());
                    fields.SetField("BaseGravable", "0");
                    fields.SetField("Tasa", "0");

                    fields.SetField("Periodo1", row.Cells[3].Text);

                    fields.SetField("Impuesto", row.Cells[5].Text);//VALOR.ToString("N", CultureInfo.CurrentCulture));
                    fields.SetField("Diferencia", row.Cells[9].Text);
                    fields.SetField("Rezago", row.Cells[7].Text);
                    fields.SetField("Adicional", row.Cells[6].Text);
                    fields.SetField("Recargo", row.Cells[8].Text);
                    fields.SetField("Multa", row.Cells[13].Text);
                    Decimal Subtotal = Convert.ToDecimal(row.Cells[5].Text) + Convert.ToDecimal(row.Cells[9].Text) + Convert.ToDecimal(row.Cells[7].Text) + Convert.ToDecimal(row.Cells[6].Text) + Convert.ToDecimal(row.Cells[8].Text) + Convert.ToDecimal(row.Cells[13].Text);
                    fields.SetField("Subtotal", Subtotal.ToString());

                    fields.SetField("Periodo2", row.Cells[15].Text);

                    fields.SetField("Infraestructura", grdEdoCta.DataKeys[row.RowIndex].Values["Infraestructura"].ToString());
                    fields.SetField("LimpiezaFrente", grdEdoCta.DataKeys[row.RowIndex].Values["Limpieza"].ToString());
                    fields.SetField("DAP", grdEdoCta.DataKeys[row.RowIndex].Values["DAP"].ToString());
                    fields.SetField("RecoleccionResiduos", grdEdoCta.DataKeys[row.RowIndex].Values["Recoleccion"].ToString());

                    fields.SetField("RezagoSM", row.Cells[18].Text);
                    fields.SetField("AdicionalSM", row.Cells[17].Text);
                    fields.SetField("RecargoSM", row.Cells[19].Text);

                    fields.SetField("MultaSM", row.Cells[21].Text);
                    fields.SetField("GastoEjecucionSM", row.Cells[20].Text);
                    Decimal SubtotalSM = Convert.ToDecimal(row.Cells[18].Text==""?"0": row.Cells[18].Text) + Convert.ToDecimal(row.Cells[17].Text) + Convert.ToDecimal(row.Cells[19].Text) + Convert.ToDecimal(row.Cells[21].Text) + Convert.ToDecimal(row.Cells[20].Text) + Convert.ToDecimal(grdEdoCta.DataKeys[row.RowIndex].Values["Infraestructura"].ToString()) + Convert.ToDecimal(grdEdoCta.DataKeys[row.RowIndex].Values["Limpieza"].ToString()) + Convert.ToDecimal(grdEdoCta.DataKeys[row.RowIndex].Values["DAP"].ToString()) + Convert.ToDecimal(grdEdoCta.DataKeys[row.RowIndex].Values["Recoleccion"].ToString());
                    fields.SetField("SubtotalSM", SubtotalSM.ToString());

                    fields.SetField("Total", (Subtotal + SubtotalSM).ToString());

                    fields.SetField("Fecha", "Cuernavaca, Morelos, México a " + row.Cells[24].Text);
                    fields.SetField("Titular", "Titular");
                    fields.SetField("Puesto", "Puesto");

                    // flatten form fields and close document
                    stamper.FormFlattening = true;
                    stamper.Close();

                    string urlpath = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;
                    urlpath = urlpath + "/Temporales/" + ViewState["pdf"].ToString();
                    string parametros = "','" + ViewState["pdf"].ToString().Replace(".pdf", "") + "','height=600,width=550,toolbar=no,directories=no,menubar=no,scrollbars=no,resizable=no'";
                    String Clientscript = "window.open('" + urlpath + parametros + ",'_blank')";

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), ViewState["pdf"].ToString().Replace(".pdf",""), Clientscript, true);
                }
            }
        }
=======
using Clases.BL;
using Clases.Utilerias;
using iTextSharp.text.pdf;
using System.IO;
using Catastro.Controles;
using System.Transactions;
namespace Catastro
{
    public partial class Requerimiento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["sortCampo"] = "Nombre";
                ViewState["sortOrden"] = "asc";
                //Llenado del dropdownlist de año
                Int32 Anio = DateTime.Today.Year;
                ddlAnio.Items.Insert(0, new ListItem("Año", "0"));
                ddlAnio.Items.Insert(1, new ListItem(Anio.ToString(), Anio.ToString()));
                for (Int32 i = 1; i <= 5; i++)
                {
                    ddlAnio.Items.Insert(i + 1, new ListItem((Anio - i).ToString(), (Anio - 1).ToString()));
                }
                //Se muestra el grid de Seleccionados vacío
                DataTable dt = new DataTable();
                dt.Columns.Add("ClavePredial");
                dt.Columns.Add("Tipo");
                dt.Columns.Add("Periodo");
                dt.Columns.Add("IdPredio");
                dt.Columns.Add("IdTipoPredio");
                DataRow row = dt.NewRow();
                row["IdPredio"] = "";
                row["IdTipoPredio"] = "";
                row["ClavePredial"] = "";
                row["Tipo"] = "";
                row["Periodo"] = "";
                dt.Rows.Add(row);
                grdResultados.DataSource = dt;
                grdResultados.DataBind();
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
                ddlAsignar.DataSource = dtAgente;
                ddlAsignar.DataTextField = "Nombre";
                ddlAsignar.DataValueField = "Id";
                ddlAsignar.DataBind();
                //Se asgina la fecha de hoy como default al cuadro de texto fecha
                txtFecha.Text = DateTime.Today.ToShortDateString();
                //Se llena el dropdownlist de documentos
                ddlDocumento.DataSource = new cTipoFaseBL().GetAll();
                ddlDocumento.DataTextField = "Descripcion";
                ddlDocumento.DataValueField = "Id";
                ddlDocumento.DataBind();
                ddlDocumento.Items.Insert(0, new ListItem("Todos", "0"));
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }
        /// <summary>
        /// Muestra la modal para buscar contribuyente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            llenaFiltroContribuyente();
            pnl_Modal.Show();
        }
        /// <summary>
        /// Llena el fil
        /// </summary>
        private void llenaFiltroContribuyente()
        {
            ddlFiltroContribuyente.Items.Clear();
            ddlFiltroContribuyente.Items.Insert(0, new ListItem("Todos", ""));
            ddlFiltroContribuyente.Items.Insert(1, new ListItem("Nombre", "Nombre"));
            ddlFiltroContribuyente.Items.Insert(2, new ListItem("Apellido Paterno", "ApellidoPaterno"));
            ddlFiltroContribuyente.Items.Insert(3, new ListItem("Apellido Materno", "ApellidoMaterno"));
            ddlFiltroContribuyente.Items.Insert(4, new ListItem("CURP", "Curp"));
        }
        /// <summary>
        /// Realiza la búsquedas de contribuyentes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbBuscarContribuyenteFiltro_Click(object sender, ImageClickEventArgs e)
        {
            string[] filtro = new string[] { ddlFiltroContribuyente.SelectedValue, txtFiltroContribuyente.Text, "true" };
            ViewState["filtro"] = filtro;
            llenagrid();
        }
        /// <summary>
        /// Llena el grid de búsqueda de Contribuyente
        /// </summary>
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
            pnl_Modal.Show();
        }
        /// <summary>
        /// Código de indexación o paginación del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd.PageIndex = e.NewPageIndex;
            llenagrid();
        }
        /// <summary>
        /// Cierra la modal de Búsqueda de Contribuyente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnl_Modal.Hide();
        }
        /// <summary>
        /// Obtiene los predios que coinciden con los criterios dados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            String[] param = { ddlAnio.SelectedValue, ddlBimestre.SelectedValue, chkBaldios.Checked.ToString(), chkExentos.Checked.ToString(), chkPExentos.Checked.ToString(), chkUBaldios.Checked.ToString(), txtRangoDe.Text, txtRangoA.Text, hdfIdCon.Value, txtClave.Text, ddlDocumento.SelectedValue };
            cPredioBL pBL = new cPredioBL();
            grdResultados.DataSource = pBL.ObtenerPrediosRequerimientos(param);
            grdResultados.DataBind();
        }
        /// <summary>
        /// Código para lo botones del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SeleccionarPersona")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;
                cContribuyenteBL cBL = new cContribuyenteBL();
                cContribuyente contribuyente = cBL.GetByConstraint(id);
                txtContribuyente.Text = contribuyente.Nombre + " " + contribuyente.ApellidoPaterno + " " + contribuyente.ApellidoMaterno;
                hdfIdCon.Value = contribuyente.Id.ToString();
                pnl_Modal.Hide();
            }

        }
        /// <summary>
        /// Código de ordenado del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            List<List<string>> parametros = new List<List<string>>();

            for (int i = 0; i < grdResultados.Rows.Count; i++)
            {
                List<string> param = new List<string>();
                String[] periodo = grdResultados.Rows[i].Cells[2].Text.Split('-');
                Int32 EIni = Convert.ToInt32(periodo[0]);
                Int32 BIni = Convert.ToInt32(periodo[1]);

                param.Add(grdResultados.DataKeys[i].Values["IdPredio"].ToString());
                param.Add("NO");
                param.Add(BIni.ToString());
                param.Add(EIni.ToString());
                param.Add(ddlBimestre.SelectedValue.ToString());
                param.Add(ddlAnio.SelectedValue.ToString());
                param.Add("-1");
                param.Add(txtFecha.Text);
                param.Add("Rezago");
                parametros.Add(param);

                //Impuesto impuesto = new SaldosC().CalculaCobro(Convert.ToInt32(grdResultados.DataKeys[i].Values["IdPredio"]), "NO", BIni, EIni, Convert.ToInt32(ddlBimestre.SelectedValue), Convert.ToInt32(ddlAnio.SelectedValue), -1);
            }
            DataTable dtEdosCta = new tRequerimientoBL().GenerarEdoCuenta(parametros);
            grdEdoCta.DataSource = dtEdosCta;
            grdEdoCta.DataBind();
            TabContainerRequerimiento.ActiveTabIndex = 2;
        }

        protected void grdResultados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            cTipoPredioBL tpBL = new cTipoPredioBL();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Text = e.Row.Cells[1].Text == "&nbsp;" || e.Row.Cells[1].Text == "" ? "" : tpBL.GetByConstraint(Convert.ToInt32(e.Row.Cells[1].Text)).Descripcion;
            }
        }

        protected void btnRequerir_Click(object sender, EventArgs e)
        {
            int cont = Calcula();
            if (cont!=0)
            { 
                vtnModal.ShowPopup("¿Se imprimieron correctamente los requerimientos?", ModalPopupMensaje.TypeMesssage.Confirm);
                txtTarjetas.Text = cont.ToString();
            }
            else
            {
                vtnModal.DysplayCancelar = false;
                vtnModal.ShowPopup("No existen registros que coincidan con el rango proporcionado", ModalPopupMensaje.TypeMesssage.Confirm);
                vtnModal.DysplayCancelar = true;
            }
        }
        protected int Calcula()
        {
            int cont = 0;
            string Path = Server.MapPath("~/");

            foreach (GridViewRow row in grdEdoCta.Rows)
            {
                Boolean bConsiderar = true;
                if (txtRRangoA.Text != "" && txtRRanggoB.Text != "")
                {
                    long RA = Convert.ToInt64(txtRRangoA.Text);
                    long RB = Convert.ToInt64(txtRRanggoB.Text);
                    long cve = Convert.ToInt64(row.Cells[0].Text);
                    if (!(row.RowIndex + 1 >= RA && row.RowIndex + 1 <= RB))
                        bConsiderar = false;                                            
                }

                if (bConsiderar)
                {
                    string formOriginal = Path + "/Documentos/Requerimiento.pdf";

                    string ruta = row.Cells[0].Text + "-" + DateTime.Now.Millisecond.ToString() + ".pdf";
                    ViewState["pdf"] = ruta;
                    string formImprimir = Path + "/Temporales/" + ruta;

                    PdfReader reader = new PdfReader(formOriginal);
                    PdfStamper stamper = new PdfStamper(reader, new FileStream(formImprimir, FileMode.Create));

                    AcroFields fields = stamper.AcroFields;
                    fields.SetField("Contribuyente", row.Cells[2].Text);
                    fields.SetField("Domicilio", row.Cells[1].Text);
                    fields.SetField("ClaveCatastral", row.Cells[0].Text);
                    cPredio predio = new cPredioBL().GetByClavePredial(row.Cells[0].Text);
                    fields.SetField("SuperficeTerreno", predio.SuperficieTerreno.ToString());
                    fields.SetField("ValorTerreno", predio.ValorTerreno.ToString());
                    fields.SetField("SuperficieConstruccion", predio.SuperficieConstruccion.ToString());
                    fields.SetField("ValorConstruccion", predio.ValorConstruccion.ToString());
                    fields.SetField("BaseGravable", "0");
                    fields.SetField("Tasa", "0");

                    fields.SetField("Periodo1", row.Cells[3].Text);

                    fields.SetField("Impuesto", row.Cells[5].Text);//VALOR.ToString("N", CultureInfo.CurrentCulture));
                    fields.SetField("Diferencia", row.Cells[9].Text);
                    fields.SetField("Rezago", row.Cells[7].Text);
                    fields.SetField("Adicional", row.Cells[6].Text);
                    fields.SetField("Recargo", row.Cells[8].Text);
                    fields.SetField("Multa", row.Cells[13].Text);
                    Decimal Subtotal = Convert.ToDecimal(row.Cells[5].Text) + Convert.ToDecimal(row.Cells[9].Text) + Convert.ToDecimal(row.Cells[7].Text) + Convert.ToDecimal(row.Cells[6].Text) + Convert.ToDecimal(row.Cells[8].Text) + Convert.ToDecimal(row.Cells[13].Text);
                    fields.SetField("Subtotal", Subtotal.ToString());

                    fields.SetField("Periodo2", row.Cells[15].Text);

                    fields.SetField("Infraestructura", grdEdoCta.DataKeys[row.RowIndex].Values["Infraestructura"].ToString());
                    fields.SetField("LimpiezaFrente", grdEdoCta.DataKeys[row.RowIndex].Values["Limpieza"].ToString());
                    fields.SetField("DAP", grdEdoCta.DataKeys[row.RowIndex].Values["DAP"].ToString());
                    fields.SetField("RecoleccionResiduos", grdEdoCta.DataKeys[row.RowIndex].Values["Recoleccion"].ToString());

                    fields.SetField("RezagoSM", row.Cells[18].Text);
                    fields.SetField("AdicionalSM", row.Cells[17].Text);
                    fields.SetField("RecargoSM", row.Cells[19].Text);

                    fields.SetField("MultaSM", row.Cells[21].Text);
                    fields.SetField("GastoEjecucionSM", row.Cells[20].Text);
                    Decimal SubtotalSM = Convert.ToDecimal(row.Cells[18].Text == "" ? "0" : row.Cells[18].Text) + Convert.ToDecimal(row.Cells[17].Text) + Convert.ToDecimal(row.Cells[19].Text) + Convert.ToDecimal(row.Cells[21].Text) + Convert.ToDecimal(row.Cells[20].Text) + Convert.ToDecimal(grdEdoCta.DataKeys[row.RowIndex].Values["Infraestructura"].ToString()) + Convert.ToDecimal(grdEdoCta.DataKeys[row.RowIndex].Values["Limpieza"].ToString()) + Convert.ToDecimal(grdEdoCta.DataKeys[row.RowIndex].Values["DAP"].ToString()) + Convert.ToDecimal(grdEdoCta.DataKeys[row.RowIndex].Values["Recoleccion"].ToString());
                    fields.SetField("SubtotalSM", SubtotalSM.ToString());

                    fields.SetField("Total", (Subtotal + SubtotalSM).ToString());

                    fields.SetField("Fecha", "Cuernavaca, Morelos, México a " + row.Cells[24].Text);
                    fields.SetField("Titular", "Titular");
                    fields.SetField("Puesto", "Puesto");

                    // flatten form fields and close document
                    stamper.FormFlattening = true;
                    stamper.Close();

                    string urlpath = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;
                    urlpath = urlpath + "/Temporales/" + ViewState["pdf"].ToString();
                    string parametros = "','" + ViewState["pdf"].ToString().Replace(".pdf", "") + "','height=600,width=550,toolbar=no,directories=no,menubar=no,scrollbars=no,resizable=no'";
                    String Clientscript = "window.open('" + urlpath + parametros + ",'_blank')";
                    cont++;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), ViewState["pdf"].ToString().Replace(".pdf", ""), Clientscript, true);
                }
            }
            return cont;
        }
>>>>>>> 776e7d9a0a962f8626f41905875e95cd0c8d8209:Catastro/Catastro/Requerimientos/Requerimiento.aspx.cs
        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            if (vtnModal.Mensaje == "¿Se imprimieron correctamente los requerimientos?")
<<<<<<< HEAD:Catastro/Catastro/Rezagos/Requerimiento.aspx.cs
            {
                cUsuarios U = new cUsuarios();
                U = (cUsuarios)Session["usuario"];
                tRequerimientoBL rBL = new tRequerimientoBL();
                using (TransactionScope scope = new TransactionScope())
                {
                    MensajesInterfaz resul = MensajesInterfaz.Ingreso;
                    foreach (GridViewRow row in grdEdoCta.Rows)
                    {
                        Boolean bConsiderar = true;
                        if (txtRRangoA.Text != "" && txtRRanggoB.Text != "")
                        {
                            long RA = Convert.ToInt64(txtRRangoA.Text);
                            long RB = Convert.ToInt64(txtRRanggoB.Text);
                            long cve = Convert.ToInt64(row.Cells[0].Text);
                            if (!(cve >= RA && cve <= RB))
                                bConsiderar = false;

                        }

                        if (bConsiderar)
                        {
                            tRequerimiento req;
                            Int32 Folio = Convert.ToInt32(txtFolio.Text);
                            if (row.Cells[3].Text != "")
                            {
                                req = new tRequerimiento();
                                req.IdPredio = Convert.ToInt32(grdEdoCta.DataKeys[row.RowIndex].Values["IdPredio"].ToString());
                                req.Folio = Folio;
                                req.TipoImpuesto = "IP";
                                req.FechaEmision = Convert.ToDateTime(row.Cells[24].Text);
                                req.TipoFase = 1;
                                req.FechaLimite = Convert.ToDateTime(row.Cells[24].Text);
                                String[] periodos = row.Cells[3].Text.Split('-');
                                String[] periodo = periodos[0].TrimStart().Split(' ');
                                req.BimestreInicial = Convert.ToInt32(periodo[0]);
                                req.EjercicioInicial = Convert.ToInt32(periodo[1]);
                                periodo = periodos[1].TrimStart().Split(' ');
                                req.BimestreFinal = Convert.ToInt32(periodo[0]);
                                req.EjercicioFinal = Convert.ToInt32(periodo[1]);
                                req.Diferencia = null;
                                req.Impuesto = Convert.ToDecimal(row.Cells[5].Text);
                                req.Adicional = Convert.ToDecimal(row.Cells[6].Text);
                                req.Recargo = Convert.ToDecimal(row.Cells[8].Text);
                                req.Rezago = Convert.ToDecimal(row.Cells[7].Text);
                                req.Diferencia = Convert.ToDecimal(row.Cells[9].Text);
                                req.RecargoDiferencia = Convert.ToDecimal(row.Cells[10].Text);
                                req.Ejecucion = Convert.ToDecimal(row.Cells[11].Text);
                                req.Multa = Convert.ToDecimal(row.Cells[13].Text);
                                req.Importe = Convert.ToDecimal(row.Cells[14].Text);
                                req.Status = "A";
                                req.StatusEnvio = "I";
                                req.Activo = true;
                                req.IdUsuario = U.Id;
                                req.FechaModificacion = DateTime.Today;
                                resul = rBL.Insert(req);
                                if (resul == MensajesInterfaz.Ingreso)
                                    ((HiddenField)row.FindControl("hdfIdIP")).Value = req.Id.ToString();
                            }
                            if (resul == MensajesInterfaz.Ingreso)
                            {
                                if (row.Cells[15].Text != "")
                                {
                                    req = new tRequerimiento();
                                    req.IdPredio = Convert.ToInt32(grdEdoCta.DataKeys[row.RowIndex].Values["IdPredio"].ToString());
                                    req.Folio = Folio;
                                    req.TipoImpuesto = "SM";
                                    req.FechaEmision = Convert.ToDateTime(row.Cells[24].Text);
                                    req.TipoFase = 1;
                                    req.FechaLimite = Convert.ToDateTime(row.Cells[24].Text);
                                    String[] periodos = row.Cells[15].Text.Split('-');
                                    String[] periodo = periodos[0].TrimStart().Split(' ');
                                    req.BimestreInicial = Convert.ToInt32(periodo[0]);
                                    req.EjercicioInicial = Convert.ToInt32(periodo[1]);
                                    periodo = periodos[1].TrimStart().Split(' ');
                                    req.BimestreFinal = Convert.ToInt32(periodo[0]);
                                    req.EjercicioFinal = Convert.ToInt32(periodo[1]);
                                    req.Diferencia = null;
                                    req.Impuesto = Convert.ToDecimal(row.Cells[16].Text);
                                    req.Adicional = Convert.ToDecimal(row.Cells[17].Text);
                                    req.Recargo = Convert.ToDecimal(row.Cells[19].Text);
                                    req.Rezago = Convert.ToDecimal(row.Cells[18].Text);
                                    req.Diferencia = 0;
                                    req.RecargoDiferencia = 0;
                                    req.Ejecucion = Convert.ToDecimal(row.Cells[20].Text);
                                    req.Multa = Convert.ToDecimal(row.Cells[21].Text);
                                    req.Importe = 0;
                                    req.Status = "A";
                                    req.StatusEnvio = "I";
                                    req.Activo = true;
                                    req.IdUsuario = U.Id;
                                    req.FechaModificacion = DateTime.Today;
                                    resul = rBL.Insert(req);
                                    if (resul == MensajesInterfaz.Ingreso)
                                        ((HiddenField)row.FindControl("hdfIdSM")).Value = req.Id.ToString();
                                }
                            }
                            if (resul == MensajesInterfaz.Ingreso)
                            {
                                row.Cells[23].Text = Folio.ToString();
                                Folio++;
                            }
                        }
                    }
                    if (resul == MensajesInterfaz.Ingreso)
                    {
                        scope.Complete();
                        TabContainerEdoCta.TabIndex = 2;
                    }

                }
            }
        }

        protected void btnAsignar_Click(object sender, EventArgs e)
        {
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            tRequerimientoBL rBL = new tRequerimientoBL();
            using (TransactionScope scope = new TransactionScope())
            {
                MensajesInterfaz resul = MensajesInterfaz.Actualizacion;
                foreach (GridViewRow row in grdEdoCta.Rows)
                {
                    if (row.Cells[23].Text != "")
                    {
                        if (((HiddenField)row.FindControl("hdfIdIP")).Value != "")
                        {
                            tRequerimiento req = rBL.GetByConstraint(Convert.ToInt32(((HiddenField)row.FindControl("hdfIdIP")).Value));
                            req.IdAgenteFiscal = Convert.ToInt32(ddlAsignar.SelectedValue);
                            req.IdUsuario = U.IdUsuario;
                            req.FechaModificacion = DateTime.Today;
                            resul = rBL.Update(req);
                        }
                        if (((HiddenField)row.FindControl("hdfIdSM")).Value != "")
                        {
                            tRequerimiento req = rBL.GetByConstraint(Convert.ToInt32(((HiddenField)row.FindControl("hdfIdSM")).Value));
                            req.IdAgenteFiscal = Convert.ToInt32(ddlAsignar.SelectedValue);
                            req.IdUsuario = U.IdUsuario;
                            req.FechaModificacion = DateTime.Today;
                            resul = rBL.Update(req);
                        }
                        if (resul == MensajesInterfaz.Actualizacion)
                            row.Cells[22].Text = ddlAsignar.SelectedItem.Text;
                    }
                }
                if (resul == MensajesInterfaz.Actualizacion)
                    scope.Complete();
            }
        }
    }
=======
            {
                cUsuarios U = new cUsuarios();
                U = (cUsuarios)Session["usuario"];
                tRequerimientoBL rBL = new tRequerimientoBL();
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        MensajesInterfaz resul = MensajesInterfaz.Ingreso;
                        foreach (GridViewRow row in grdEdoCta.Rows)
                        {
                            Boolean bConsiderar = true;
                            if (txtRRangoA.Text != "" && txtRRanggoB.Text != "")
                            {
                                long RA = Convert.ToInt64(txtRRangoA.Text);
                                long RB = Convert.ToInt64(txtRRanggoB.Text);
                                if (!(row.RowIndex + 1 >= RA && row.RowIndex + 1 <= RB))
                                    bConsiderar = false;

                            }

                            if (bConsiderar)
                            {
                                tRequerimiento req;
                                Int32 Folio = Convert.ToInt32(txtFolio.Text);
                                cPredio predio = new cPredioBL().GetByConstraint(Convert.ToInt32(grdEdoCta.DataKeys[row.RowIndex].Values["IdPredio"].ToString()));
                                if (row.Cells[3].Text != "")
                                {
                                    req = new tRequerimiento();
                                    req.IdPredio = Convert.ToInt32(grdEdoCta.DataKeys[row.RowIndex].Values["IdPredio"].ToString());
                                    req.Folio = Folio;
                                    req.TipoImpuesto = "IP";
                                    req.FechaEmision = Convert.ToDateTime(row.Cells[24].Text);
                                    cTipoFase fase = new cTipoFaseBL().GetFilter("Fase", predio.cTipoFase.FaseSiguiente.ToString(), "true", "FaseSiguiente", "asc").FirstOrDefault();
                                    req.TipoFase = fase.Id;
                                    req.FechaLimite = Convert.ToDateTime(row.Cells[24].Text);
                                    String[] periodos = row.Cells[3].Text.Split('-');
                                    String[] periodo = periodos[0].TrimStart().Split(' ');
                                    req.BimestreInicial = Convert.ToInt32(periodo[0]);
                                    req.EjercicioInicial = Convert.ToInt32(periodo[1]);
                                    periodo = periodos[1].TrimStart().Split(' ');
                                    req.BimestreFinal = Convert.ToInt32(periodo[0]);
                                    req.EjercicioFinal = Convert.ToInt32(periodo[1]);
                                    req.Diferencia = null;
                                    req.Impuesto = Convert.ToDecimal(row.Cells[5].Text);
                                    req.Adicional = Convert.ToDecimal(row.Cells[6].Text);
                                    req.Recargo = Convert.ToDecimal(row.Cells[8].Text);
                                    req.Rezago = Convert.ToDecimal(row.Cells[7].Text);
                                    req.Diferencia = Convert.ToDecimal(row.Cells[9].Text);
                                    req.RecargoDiferencia = Convert.ToDecimal(row.Cells[10].Text);
                                    req.Ejecucion = Convert.ToDecimal(row.Cells[11].Text);
                                    req.Multa = Convert.ToDecimal(row.Cells[13].Text);
                                    req.Importe = Convert.ToDecimal(row.Cells[14].Text);
                                    req.Status = "A";
                                    req.StatusEnvio = "I";
                                    req.Activo = true;
                                    req.IdUsuario = U.Id;
                                    req.FechaModificacion = DateTime.Today;
                                    predio.IdTipoFaseIp = fase.Id;
                                    resul = rBL.Insert(req);
                                    if (resul == MensajesInterfaz.Ingreso)
                                        ((HiddenField)row.FindControl("hdfIdIP")).Value = req.Id.ToString();
                                }
                                if (resul == MensajesInterfaz.Ingreso)
                                {
                                    if (row.Cells[15].Text != "")
                                    {
                                        req = new tRequerimiento();
                                        req.IdPredio = Convert.ToInt32(grdEdoCta.DataKeys[row.RowIndex].Values["IdPredio"].ToString());
                                        req.Folio = Folio;
                                        req.TipoImpuesto = "SM";
                                        req.FechaEmision = Convert.ToDateTime(row.Cells[24].Text);
                                        cTipoFase fase = new cTipoFaseBL().GetFilter("Fase", predio.cTipoFase1.FaseSiguiente.ToString(), "true", "FaseSiguiente", "asc").FirstOrDefault();
                                        req.TipoFase = fase.Id;
                                        req.FechaLimite = Convert.ToDateTime(row.Cells[24].Text);
                                        String[] periodos = row.Cells[15].Text.Split('-');
                                        String[] periodo = periodos[0].TrimStart().Split(' ');
                                        req.BimestreInicial = Convert.ToInt32(periodo[0]);
                                        req.EjercicioInicial = Convert.ToInt32(periodo[1]);
                                        periodo = periodos[1].TrimStart().Split(' ');
                                        req.BimestreFinal = Convert.ToInt32(periodo[0]);
                                        req.EjercicioFinal = Convert.ToInt32(periodo[1]);
                                        req.Diferencia = null;
                                        req.Impuesto = Convert.ToDecimal(row.Cells[16].Text);
                                        req.Adicional = Convert.ToDecimal(row.Cells[17].Text);
                                        req.Recargo = Convert.ToDecimal(row.Cells[19].Text);
                                        req.Rezago = Convert.ToDecimal(row.Cells[18].Text);
                                        req.Diferencia = 0;
                                        req.RecargoDiferencia = 0;
                                        req.Ejecucion = Convert.ToDecimal(row.Cells[20].Text);
                                        req.Multa = Convert.ToDecimal(row.Cells[21].Text);
                                        req.Importe = 0;
                                        req.Status = "A";
                                        req.StatusEnvio = "I";
                                        req.Activo = true;
                                        req.IdUsuario = U.Id;
                                        req.FechaModificacion = DateTime.Today;
                                        predio.IdTipoFaseSm = fase.Id;
                                        resul = rBL.Insert(req);
                                        if (resul == MensajesInterfaz.Ingreso)
                                            ((HiddenField)row.FindControl("hdfIdSM")).Value = req.Id.ToString();
                                    }
                                }
                                if (resul == MensajesInterfaz.Ingreso)
                                {
                                    predio.FechaModificacion = DateTime.Today;
                                    predio.IdUsuario = U.Id;
                                    resul = new cPredioBL().Update(predio);
                                    if (resul == MensajesInterfaz.Actualizacion)
                                    {
                                        row.Cells[23].Text = Folio.ToString();
                                        Folio++;
                                        resul = MensajesInterfaz.Ingreso;
                                    }
                                }
                            }
                        }
                        if (resul == MensajesInterfaz.Ingreso)
                        {
                            scope.Complete();
                            TabContainerEdoCta.TabIndex = 2;
                        }
                    }
                    catch(Exception ex)
                    {
                        vtnModal.DysplayCancelar = false;
                        vtnModal.ShowPopup("Error al intentar actulizar información", ModalPopupMensaje.TypeMesssage.Error);
                        vtnModal.DysplayCancelar = true;
                    }
                }
            }
        }

        protected void btnAsignar_Click(object sender, EventArgs e)
        {
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            tRequerimientoBL rBL = new tRequerimientoBL();
            using (TransactionScope scope = new TransactionScope())
            {
                MensajesInterfaz resul = MensajesInterfaz.Actualizacion;
                foreach (GridViewRow row in grdEdoCta.Rows)
                {
                    if (row.Cells[23].Text != "")
                    {
                        if (((HiddenField)row.FindControl("hdfIdIP")).Value != "")
                        {
                            tRequerimiento req = rBL.GetByConstraint(Convert.ToInt32(((HiddenField)row.FindControl("hdfIdIP")).Value));
                            req.IdAgenteFiscal = Convert.ToInt32(ddlAsignar.SelectedValue);
                            req.IdUsuario = U.IdUsuario;
                            req.FechaModificacion = DateTime.Today;
                            resul = rBL.Update(req);
                        }
                        if (((HiddenField)row.FindControl("hdfIdSM")).Value != "")
                        {
                            tRequerimiento req = rBL.GetByConstraint(Convert.ToInt32(((HiddenField)row.FindControl("hdfIdSM")).Value));
                            req.IdAgenteFiscal = Convert.ToInt32(ddlAsignar.SelectedValue);
                            req.IdUsuario = U.IdUsuario;
                            req.FechaModificacion = DateTime.Today;
                            resul = rBL.Update(req);
                        }
                        if (resul == MensajesInterfaz.Actualizacion)
                            row.Cells[22].Text = ddlAsignar.SelectedItem.Text;
                    }
                }
                if (resul == MensajesInterfaz.Actualizacion)
                    scope.Complete();
            }
        }
    }
>>>>>>> 776e7d9a0a962f8626f41905875e95cd0c8d8209:Catastro/Catastro/Requerimientos/Requerimiento.aspx.cs
}