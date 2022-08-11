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
                lbltituloErrores.Visible = false;
                //Llenado del dropdownlist de año
                Int32 Anio = DateTime.Today.Year;
                ddlAnio.Items.Insert(0, new ListItem("Año", "0"));
                ddlAnio.Items.Insert(1, new ListItem(Anio.ToString(), Anio.ToString()));
                for (Int32 i = 1; i <= 5; i++)
                {
                    ddlAnio.Items.Insert(i + 1, new ListItem((Anio - i).ToString(), (Anio - 1).ToString()));
                }
                //ddlERequerir.Items.Insert(0, new ListItem("Año", "0"));
                //ddlERequerir.Items.Insert(1, new ListItem(Anio.ToString(), Anio.ToString()));
                //ddlAnio.Items.Insert(0, new ListItem("Año", "0"));
                //ddlAnio.Items.Insert(1, new ListItem(Anio.ToString(), Anio.ToString()));
                //for (Int32 i = 1; i <= 5; i++)
                //{
                //    //ddlERequerir.Items.Insert(i + 1, new ListItem((Anio - i).ToString(), (Anio - 1).ToString()));
                //    ddlAnio.Items.Insert(i + 1, new ListItem((Anio - i).ToString(), (Anio - 1).ToString()));
                //}
                llenarGridBusqueda(0);                
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
                //ddlDocumento.Items.Insert(0, new ListItem("Todos", "0"));
                ddlDocumento.SelectedValue = new cTipoFaseBL().GetFilter("Descripcion", "SIN DOCUMENTO", "true", "Descripcion", "asc").FirstOrDefault() != null ? new cTipoFaseBL().GetFilter("Descripcion", "SIN DOCUMENTO", "true", "Descripcion", "asc").FirstOrDefault().Id.ToString() : "0";

                DateTime d = DateTime.Now;
                if (d.Month > 2)
                {
                    ddlAnio.SelectedValue = DateTime.Now.Year.ToString();
                }
                else
                {
                    int y= DateTime.Now.Year-1;
                    ddlAnio.SelectedValue = y.ToString();
                }                             
    
               //INGRESA EL BIMESTRE ACTUAL O ANTERIOR PARA REQUERIR
                double bActual = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(d.Month / 2.0)));

                if ( (d.Month % 2.0)  == 0 )                    
                    ddlBimestre.SelectedValue = bActual.ToString();
                else
                    ddlBimestre.SelectedValue = (bActual - 1).ToString();
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);           

        }

        /// <summary>
        /// Muestra la modal para buscar contribuyente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["sortCampo"] = "Nombre";
            lblbuscarContribuyente.Text = "Buscar Contribuyente";
            llenaFiltroContribuyente();
            pnl_Modal.Show();
        }

        /// <summary>
        /// Muestra la modal para buscar condominios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbCondominio_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["sortCampo"] = "Descripcion";
            lblbuscarContribuyente.Text = "Buscar Condominio";
            llenaFiltroCondominio();
            pnl_Modal.Show();
        }

        /// <summary>
        /// Muestra la modal para buscar colonia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbColonia_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["sortCampo"] = "NombreColonia";
            lblbuscarContribuyente.Text = "Buscar Colonia";
            llenaFiltroColonia();
            pnl_Modal.Show();
        }

        /// <summary>
        /// Llena el filtro de condominios
        /// </summary>
        private void llenaFiltroCondominio()
        {
            ddlFiltroContribuyente.Items.Clear();
            ddlFiltroContribuyente.Items.Insert(0, new ListItem("Todos", ""));
            ddlFiltroContribuyente.Items.Insert(1, new ListItem("Nombre", "Descripcion"));            
        }

        /// <summary>
        /// Llena el filtro de colonia
        /// </summary>
        private void llenaFiltroColonia()
        {
            ddlFiltroContribuyente.Items.Clear();
            ddlFiltroContribuyente.Items.Insert(0, new ListItem("Todos", ""));
            ddlFiltroContribuyente.Items.Insert(1, new ListItem("Nombre", "NombreColonia"));
        }

        /// <summary>
        /// Llena el filtro de contribuyentes
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
            if(lblbuscarContribuyente.Text.Contains("Contribuyente"))
                llenagrid();
            else if (lblbuscarContribuyente.Text.Contains("Condominio"))
                llenagridCondominio();
            else
                llenagridColonia();
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
            grd.Visible = true;
            pnl_Modal.Show();
        }

        /// <summary>
        /// Llena el grid de búsqueda de Condominios
        /// </summary>
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

        /// <summary>
        /// Llena el grid de búsqueda de Condominios
        /// </summary>
        private void llenagridColonia()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grdCol.DataSource = new cColoniaBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOrden"].ToString());
                grdCol.DataBind();
            }
            else
            {
                grdCol.DataSource = new cColoniaBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOrden"].ToString());
                grdCol.DataBind();

            }
            grdCol.Visible = true;
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
        /// Código de indexación o paginación del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdCon_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCon.PageIndex = e.NewPageIndex;
            llenagridCondominio();
        }

        /// <summary>
        /// Código de indexación o paginación del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdCol_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCon.PageIndex = e.NewPageIndex;
            llenagridColonia();
        }

        /// <summary>
        /// Cierra la modal de Búsqueda de Contribuyente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Obtiene los predios que coinciden con los criterios dados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ViewState["Tabla"] = null;
            llenarGridBusqueda(1);
        }

        protected void llenarGridBusqueda(int band)
        {
            if ((band == 1) && (ViewState["Tabla"] == null))
            {
                
                String[] param = { ddlAnio.SelectedValue, ddlBimestre.SelectedValue, chkBaldios.Checked.ToString(), chkExentos.Checked.ToString(), chkPExentos.Checked.ToString(), chkUBaldios.Checked.ToString(), txtRangoDe.Text, txtRangoA.Text, hdfIdCon.Value, RemoveSpecialCharacters(txtClave.Text), ddlDocumento.SelectedValue, hdfIdCondominio.Value, hdfIdColonia.Value  };
                cPredioBL pBL = new cPredioBL();
                DataTable dt = new vVistasBL().ObtienePadronRezagoBusq(param); // pBL.ObtenerPrediosRequerimientos(param);
                ViewState["Tabla"] = dt;
                grdResultados.DataSource = dt;
                grdResultados.DataBind();
                lblTotal2.Text = dt.Rows.Count.ToString();
                ViewState["dt"] = dt;
            }
            else if ((band == 1) && (ViewState["Tabla"] != null))
            {
                DataTable dt = (DataTable)ViewState["Tabla"];
                grdResultados.DataSource = dt;
                grdResultados.DataBind();
                lblTotal2.Text = dt.Rows.Count.ToString();
            }
            else
            {
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
            }
            btnGenerar.Enabled = true;
        }

        protected void llenarGridError(string claveP, string error)
        {
            DataTable dt = null;
           if (ViewState["TablaE"] != null)
            {
                 dt = (DataTable)ViewState["TablaE"];
            }
            else
            {
                //Se muestra el grid de Seleccionados vacío
                dt = new DataTable();
                dt.Columns.Add("ClavePredial");
                dt.Columns.Add("Error");
               
            }
            if (claveP.Trim().Length > 0)
            {
                DataRow row = dt.NewRow();
                row["ClavePredial"] = claveP;
                row["Error"] = error;
                dt.Rows.Add(row);
            }
            ViewState["TablaE"] = dt;
            grdError.DataSource = dt;
            grdError.DataBind();
            if (dt.Rows.Count > 0)
                grdError.Visible = true;
            else
                grdError.Visible = false;
            lbltituloErrores.Visible = grdError.Visible;
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
                txtContribuyente.Text =  contribuyente.ApellidoPaterno + " " + contribuyente.ApellidoMaterno +" "+ contribuyente.Nombre ;
                hdfIdCon.Value = contribuyente.Id.ToString();
                txtFiltroContribuyente.Text = "";
                grd.DataSource = null;
                grd.DataBind();
                grd.Visible = false;
                pnl_Modal.Hide();
            }

        }

        /// <summary>
        /// Código para lo botones del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdCon_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SeleccionarPersona")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;
                cCondominioBL cBL = new cCondominioBL();
                cCondominio condominio = cBL.GetByConstraint(id);
                txtCondominio.Text = condominio.Descripcion;
                hdfIdCondominio.Value = condominio.Id.ToString();
                txtFiltroContribuyente.Text = "";
                grdCon.DataSource = null;
                grdCon.DataBind();
                grdCon.Visible = false;
                pnl_Modal.Hide();
            }
        }

        /// <summary>
        /// Código para lo botones del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Código de ordenado del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Código de ordenado del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            ViewState["TablaE"] = null;
            grdError.DataSource = null;
            grdError.DataBind();
            grdError.Visible = false;
            lbltituloErrores.Visible = false;
            int idTipoFase = Convert.ToInt32(ddlDocumento.SelectedValue);
            List <List<string>> parametros = new List<List<string>>();
            DataTable dt = (DataTable)ViewState["dt"];            
            for (int i = 0; i < dt.Rows.Count; i++)
            {               
                List<string> param = new List<string>();
                String[] periodo = dt.Rows[i]["Periodo"].ToString().Split('-');
                Int32 EIni = Convert.ToInt32(periodo[0]);
                Int32 BIni = Convert.ToInt32(periodo[1]);

                param.Add(dt.Rows[i]["IdPredio"].ToString());
                param.Add("NO");
                param.Add(BIni.ToString());
                param.Add(EIni.ToString());
                //param.Add(ddlBRequerir.SelectedValue.ToString());
                //param.Add(ddlERequerir.SelectedValue.ToString());
                param.Add(ddlBimestre.SelectedValue.ToString());
                param.Add(ddlAnio.SelectedValue.ToString());
                param.Add("-1");
                param.Add(txtFecha.Text);
                param.Add("");
                parametros.Add(param);
            }

            cTipoFase fase = new cTipoFaseBL().GetByConstraint(idTipoFase);
            if (fase == null)
            {
                vtnModal.Dispose();
                vtnModal.ShowPopup(new Utileria().GetDescription("No se ha definido la cobranza de la fase: Honorarios o Gastos de Ejecución"), ModalPopupMensaje.TypeMesssage.Alert);
                return;
            }
            cTipoFase faseSig = new cTipoFaseBL().GetByFase(fase.FaseSiguiente);

            cConcepto conEjecucion = null, conMulta = null, conEjecucionSM = null, conMultaSM = null, conHonorarios = null, conHonorariosSM = null;
            String cri = "";

            string mesa = new cParametroSistemaBL().GetValorByClave("MESAIP");
            if (mesa == "")
            {
                vtnModal.Dispose();
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.DefinirMesaIP), ModalPopupMensaje.TypeMesssage.Alert);
                return;
            }            
            else
            {
                //HONORARIOS
                if (faseSig.Id == 2)  //1 Honorarios       2 Gastos de ejecución
                {
                    cri = new cParametroSistemaBL().GetValorByClave("HONORARIOS");
                    if (cri == "")//validar si la mesa esta definida en la tabla cParametroSistema
                    {
                        vtnModal.Dispose();
                        vtnModal.ShowPopup(new Utileria().GetDescription("Concepto Honorarios no esta definido en Parametros Sistema"), ModalPopupMensaje.TypeMesssage.Alert);
                        return;
                    }
                    conHonorarios = new cConceptoBL().RegistroByCri(cri, DateTime.Today.Year, new cMesaBL().GetByNombre(mesa));
                    if (conHonorarios == null)
                    {
                        vtnModal.Dispose();
                        vtnModal.ShowPopup(new Utileria().GetDescription("Concepto Honorarios no esta definido en Catálogos Conceptos"), ModalPopupMensaje.TypeMesssage.Alert);
                        return;
                    }
                }
                else
                {
                    //EJECUCION
                    cri = new cParametroSistemaBL().GetValorByClave("EJECUCIONIP");
                    if (cri == "")//validar si la mesa esta definida en la tabla cParametroSistema
                    {
                        vtnModal.Dispose();
                        vtnModal.ShowPopup(new Utileria().GetDescription("Concepto Ejecución no esta definido en Parametros Sistema"), ModalPopupMensaje.TypeMesssage.Alert);
                        return;
                    }
                    conEjecucion = new cConceptoBL().RegistroByCri(cri, DateTime.Today.Year, new cMesaBL().GetByNombre(mesa));
                    if (conEjecucion == null)
                    {
                        vtnModal.Dispose();
                        vtnModal.ShowPopup(new Utileria().GetDescription("Concepto Ejecución no esta definido en Catálogos Conceptos"), ModalPopupMensaje.TypeMesssage.Alert);
                        return;
                    }
                }//end cobranza
                
                //MULTAS
                cri = new cParametroSistemaBL().GetValorByClave("MULTASIP");
                if (cri == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    vtnModal.Dispose();
                    vtnModal.ShowPopup(new Utileria().GetDescription("Concepto MultasIP no esta definido en Parametros Sistema"), ModalPopupMensaje.TypeMesssage.Alert);
                    return;
                }
                conMulta  = new cConceptoBL().RegistroByCri(cri, DateTime.Today.Year, new cMesaBL().GetByNombre(mesa));
                if (conMulta == null)
                {
                    vtnModal.Dispose();
                    vtnModal.ShowPopup(new Utileria().GetDescription("Concepto Multa no esta definido en Catálogos Conceptos"), ModalPopupMensaje.TypeMesssage.Alert);
                    return;
                }                
            }
            mesa = new cParametroSistemaBL().GetValorByClave("MESASM");
            if (mesa == "")
            {
                vtnModal.Dispose();
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.DefinirMesaSM), ModalPopupMensaje.TypeMesssage.Alert);
                return;
            }
            else
            {
                //HONORARIOS
                if (faseSig.Cobranza == 1)  //1 Honorarios       2 Gastos de ejecución
                {
                    cri = new cParametroSistemaBL().GetValorByClave("HONORARIOSSM");
                    if (cri == "")//validar si la mesa esta definida en la tabla cParametroSistema
                    {
                        vtnModal.Dispose();
                        vtnModal.ShowPopup(new Utileria().GetDescription("Concepto Honorarios SM no esta definido en Parametros Sistema"), ModalPopupMensaje.TypeMesssage.Alert);
                        return;
                    }
                    conHonorariosSM = new cConceptoBL().RegistroByCri(cri, DateTime.Today.Year, new cMesaBL().GetByNombre(mesa));
                    if (conHonorariosSM == null)
                    {
                        vtnModal.Dispose();
                        vtnModal.ShowPopup(new Utileria().GetDescription("Concepto Gastos de Honorarios no esta definido en Catálogos Conceptos"), ModalPopupMensaje.TypeMesssage.Alert);
                        return;
                    }
                }
                else
                {
                    //EJECUCION
                    cri = new cParametroSistemaBL().GetValorByClave("EjecucionSM");
                    if (cri == "")//validar si la mesa esta definida en la tabla cParametroSistema
                    {
                        vtnModal.Dispose();
                        vtnModal.ShowPopup(new Utileria().GetDescription("Concepto Gastos Ejecución SM no esta definido en Parametros Sistema"), ModalPopupMensaje.TypeMesssage.Alert);
                        return;
                    }
                    conEjecucionSM = new cConceptoBL().RegistroByCri(cri, DateTime.Today.Year, new cMesaBL().GetByNombre(mesa));
                    if (conEjecucionSM == null)
                    {
                        vtnModal.Dispose();
                        vtnModal.ShowPopup(new Utileria().GetDescription("Concepto Gastos de Ejecucion no esta definido en Catálogos Conceptos"), ModalPopupMensaje.TypeMesssage.Alert);
                        return;
                    }
                }
                //MULTAS
                cri = new cParametroSistemaBL().GetValorByClave("MULTASSM");
                if (cri == "")//validar si la mesa esta definida en la tabla cParametroSistema
                {
                    vtnModal.Dispose();
                    vtnModal.ShowPopup(new Utileria().GetDescription("Concepto de Multas de SM no esta definido en Parametros Sistema"), ModalPopupMensaje.TypeMesssage.Alert);
                    return;
                }
                conMultaSM = new cConceptoBL().RegistroByCri(cri, DateTime.Today.Year, new cMesaBL().GetByNombre(mesa));
                if (conMultaSM == null)
                {
                    vtnModal.Dispose();
                    vtnModal.ShowPopup(new Utileria().GetDescription("Concepto Multas SM no esta definido en Catálogos Conceptos"), ModalPopupMensaje.TypeMesssage.Alert);
                    return;
                }
            }
            cSalarioMinimo sm = new cSalarioMinimoBL().GetByEjercicio(DateTime.Today.Year);            

            DataTable dtEdosCta = new tRequerimientoBL().GenerarEdoCuenta(parametros,conHonorarios,conEjecucion, conMulta, conHonorariosSM, conEjecucionSM, conMultaSM,sm.Importe, idTipoFase);
            DataTable dtClone = dtEdosCta.Clone();
            int consecutivo = 0;
            foreach(DataRow row in dtEdosCta.Rows)
            {
                if (row["Error"].ToString() == "")
                {
                    consecutivo++;
                    row["Consecutivo"] = consecutivo;
                    dtClone.ImportRow(row);
                }
                else
                {
                    llenarGridError(row["ClavePredial"].ToString(), row["Error"].ToString());
                }                                
            }
            grdEdoCta.DataSource = dtClone;
            grdEdoCta.DataBind();
            txtTarjetas.Text = "";
            if (!grdError.Visible)
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
            long RA = Convert.ToInt64(txtRRangoA.Text);
            long RB = Convert.ToInt64(txtRRanggoB.Text);

            //vallida que exista el formato siguiente
            string Path = Server.MapPath("~/");            
            string paramMunicipio = new cParametroSistemaBL().GetByClave("MUNICIPIO_COBRO").Valor;
            cTipoFase fase = new cTipoFaseBL().GetByConstraint(Convert.ToInt32(ddlDocumento.SelectedValue));
            cTipoFase fasesig = new cTipoFaseBL().GetByFase(fase.FaseSiguiente);
            
            string formOriginal = Path + "/Documentos/" + fasesig.DoctoImprimir + "_" + paramMunicipio + ".pdf";

            if (!File.Exists(formOriginal))
            {                
                vtnModal.ShowPopup("No existe sig. formato " + fasesig.DoctoImprimir + "_" + paramMunicipio + ".pdf", ModalPopupMensaje.TypeMesssage.Confirm);
                vtnModal.DysplayCancelar = true;
                return;
            }

            if ((RB - RA) <= 99)
            {
                int cont = 0;
                try
                {
                    cont = Calcula(formOriginal);
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
                    txtTarjetas.Text = cont.ToString();
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
            else
            {
                vtnModal.ShowPopup("El rango maximo para generar la impresion es de 100", ModalPopupMensaje.TypeMesssage.Confirm);
                vtnModal.DysplayCancelar = false;
            }       
        }

        protected int Calcula(string formOriginal)
        {
            int cont = 0;
            string Path = Server.MapPath("~/");
            List<string> Lista = new List<string>();
            Int32 Folio = Convert.ToInt32(txtFolio.Text);
            string paramMunicipio = new cParametroSistemaBL().GetByClave("MUNICIPIO_COBRO").Valor;

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
                    string ruta = HttpUtility.HtmlDecode(row.Cells[1].Text.Trim()) + "-" + DateTime.Now.Millisecond.ToString() + ".pdf";
                    ViewState["pdf"] = ruta;
                    string formImprimir = Path + "/Temporales/" + ruta;

                    PdfReader reader = new PdfReader(formOriginal);
                    PdfStamper stamper = new PdfStamper(reader, new FileStream(formImprimir, FileMode.Create));

                    AcroFields fields = stamper.AcroFields;
                    fields.SetField("Contribuyente", (HttpUtility.HtmlDecode(row.Cells[3].Text).ToString()));
                    fields.SetField("Domicilio", (HttpUtility.HtmlDecode(row.Cells[2].Text).ToString()));
                    fields.SetField("Folio", Folio.ToString());
                    fields.SetField("ClaveCatastral", HttpUtility.HtmlDecode(row.Cells[1].Text));
                    string clavens = HttpUtility.HtmlDecode(row.Cells[1].Text);
                    string clave = clavens.Replace("-", "");
                    cPredio predio = new cPredioBL().GetByClavePredial(clave);
                    fields.SetField("SuperficieTerreno", predio.SuperficieTerreno.ToString());
                    fields.SetField("ValorTerreno", Convert.ToDecimal( predio.ValorTerreno).ToString("C") );
                    fields.SetField("SuperficieConstruccion", predio.SuperficieConstruccion.ToString());
                    fields.SetField("ValorConstruccion", predio.ValorConstruccion.ToString("C"));
                    fields.SetField("BaseGravable", predio.ValorCatastral.ToString("C"));
                    decimal tasa = new cBaseImpuestoBL().GetByEjercicio(DateTime.Today.Year);
                    fields.SetField("Tasa", tasa.ToString("C"));

                    fields.SetField("Periodo", HttpUtility.HtmlDecode(row.Cells[4].Text));
                    fields.SetField("Impuesto",   Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[6].Text)).ToString("C"));
                    fields.SetField("Diferencia", Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[10].Text)).ToString("C"));
                    fields.SetField("Rezago", Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[8].Text)).ToString("C"));
                    fields.SetField("Adicional", Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[7].Text)).ToString("C"));
                    Decimal recargo = Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[9].Text)) + Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[11].Text));
                    fields.SetField("Recargo", recargo.ToString("C"));
                    fields.SetField("Multa", Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[14].Text)).ToString("C"));                    
                    fields.SetField("Honorarios", Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[12].Text)).ToString("C"));//revisar
                    fields.SetField("Ejecucion", Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[13].Text)).ToString("C"));//revisar
                    fields.SetField("Subtotal", Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[15].Text)).ToString("C"));
                    fields.SetField("BimApagar", HttpUtility.HtmlDecode(row.Cells[31].Text));

            
                    fields.SetField("PeriodoSM", HttpUtility.HtmlDecode(row.Cells[17].Text));
                    fields.SetField("Infraestructura", Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[18].Text)).ToString("C"));
                    fields.SetField("Limpieza", Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[27].Text)).ToString("C"));//grdEdoCta.DataKeys[row.RowIndex].Values["Limpieza"]).ToString("C"));
                    fields.SetField("DAP", Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[25].Text)).ToString("C"));
                    fields.SetField("Recoleccion", Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[26].Text)).ToString("C"));
                    fields.SetField("RezagoSM", Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[20].Text)).ToString("C"));
                    fields.SetField("AdicionalSM", Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[19].Text)).ToString("C"));
                    fields.SetField("RecargoSM", Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[21].Text)).ToString("C"));
                    fields.SetField("EjecucionSM", Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[23].Text)).ToString("C"));//revisar
                    fields.SetField("HonorariosSM", Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[22].Text)).ToString("C"));//revisar
                    fields.SetField("MultaSM", Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[24].Text)).ToString("C"));
                    //fields.SetField("SM", Convert.ToDecimal(row.Cells[17].Text).ToString("C"));
                    fields.SetField("SubtotalSM", Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[28].Text)).ToString("C"));
                    fields.SetField("Total", (Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[15].Text)) + Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[28].Text))).ToString("C"));

                    string texto = new cParametroSistemaBL().GetValorByClave("NOMBRE_MUNICIPIO");
                    fields.SetField("Fecha", texto + ", MORELOS, A " + (HttpUtility.HtmlDecode(row.Cells[30].Text)).Substring(0,2) +" DE " +  Convert.ToDateTime(HttpUtility.HtmlDecode(row.Cells[30].Text)).ToString("MMMM").ToUpper()+" DE "+(HttpUtility.HtmlDecode(row.Cells[30].Text)).Substring(6,4));
                    fields.SetField("Titular", (HttpUtility.HtmlDecode(new cParametroSistemaBL().GetValorByClave("FIRMAREQUERIMIENTO")).ToString()));
                    fields.SetField("Puesto", (HttpUtility.HtmlDecode(new cParametroSistemaBL().GetValorByClave("PUESTOREQUERIMIENTO")).ToString() ));
                    fields.SetField("NOTIFICADORES", (HttpUtility.HtmlDecode(new cParametroSistemaBL().GetValorByClave("NOTIFICADORES")).ToString()));

                    // flatten form fields and close document
                    stamper.FormFlattening = true;
                    stamper.Close();
                    row.Cells[29].Text = Folio.ToString();
                    Folio++;
                    Lista.Add(ViewState["pdf"].ToString());

                    cont++;                  
                }
            }
            joinPDF(Lista, Path + "/Temporales/");

            return cont;
        }

        protected void joinPDF(List<string> Lista, string ruta)
        {

            iTextSharp.text.Document document = new iTextSharp.text.Document();          
            MemoryStream ms = new MemoryStream();  
            PdfCopy pdfCopy = new PdfCopy(document, ms);

            document.Open();

            foreach (string item in Lista)
            {
                PdfReader pdfReader = new PdfReader(ruta+item);
                int n = pdfReader.NumberOfPages;
                for (var page = 0; page < n;)
                {
                    pdfCopy.AddPage(pdfCopy.GetImportedPage(pdfReader, ++page));
                }
                pdfCopy.FreeReader(pdfReader);
            }
            pdfCopy.Flush();

            document.Close();
            ViewState["pdf"] = "requerimientos-" + DateTime.Now.Millisecond.ToString() + ".pdf";
          

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

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            if (vtnModal.Mensaje == "¿Se imprimieron correctamente los requerimientos?")
            {
                cUsuarios U = new cUsuarios();
                U = (cUsuarios)Session["usuario"];
                tRequerimientoBL rBL = new tRequerimientoBL();
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        MensajesInterfaz resul = MensajesInterfaz.Ingreso;
                        //Int32 Folio = Convert.ToInt32(txtFolio.Text);
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
                                int idFaseSig = 1;
                                cTipoFase FaseSig = new cTipoFase();

                                cTipoFase FaseActual = new cTipoFaseBL().GetByFase(ddlDocumento.SelectedValue);
                                if (FaseActual != null)
                                    FaseSig= new cTipoFaseBL().GetByFase(FaseActual.FaseSiguiente);

                                if (FaseSig != null)
                                    idFaseSig = FaseSig.Id;

                                tRequerimiento req;                                
                                cPredio predio = new cPredioBL().GetByConstraint(Convert.ToInt32(grdEdoCta.DataKeys[row.RowIndex].Values["IdPredio"].ToString()));
                                if (row.Cells[3].Text != "" && row.Cells[3].Text!= "&nbsp;")
                                {
                                    req = new tRequerimiento();
                                    req.IdPredio = Convert.ToInt32(grdEdoCta.DataKeys[row.RowIndex].Values["IdPredio"].ToString());
                                    req.Folio = Convert.ToInt32(row.Cells[23].Text);
                                    req.TipoImpuesto = "IP";
                                    req.FechaEmision = DateTime.Now;
                                    cTipoFase fase = new cTipoFaseBL().GetFilter("Fase", predio.cTipoFase.Fase.ToString(), "true", "FaseSiguiente", "asc").FirstOrDefault();
                                    req.TipoFase = idFaseSig; // fase.Id;
                                    req.FechaLimite = Convert.ToDateTime(row.Cells[24].Text);
                                    String[] periodos = row.Cells[3].Text.Split('-');
                                    String[] periodo = periodos[0].TrimStart().Split(' ');
                                    req.BimestreInicial = Convert.ToInt32(periodo[0]);
                                    req.EjercicioInicial = Convert.ToInt32(periodo[1]);
                                    periodo = periodos[1].TrimStart().Split(' ');
                                    req.BimestreFinal = Convert.ToInt32(periodo[0]);
                                    req.EjercicioFinal = Convert.ToInt32(periodo[1]);
                                    req.Diferencia = null;
                                    req.Impuesto = Convert.ToDecimal(row.Cells[6].Text);
                                    req.Adicional = Convert.ToDecimal(row.Cells[7].Text);
                                    req.Recargo = Convert.ToDecimal(row.Cells[9].Text);
                                    req.Rezago = Convert.ToDecimal(row.Cells[8].Text);
                                    req.Diferencia = Convert.ToDecimal(row.Cells[10].Text);
                                    req.RecargoDiferencia = Convert.ToDecimal(row.Cells[11].Text);
                                    req.Ejecucion = Convert.ToDecimal(row.Cells[13].Text);
                                    req.Multa = Convert.ToDecimal(row.Cells[15].Text);
                                    req.Importe = Convert.ToDecimal(row.Cells[16].Text);
                                    req.Status = "A";
                                    req.StatusEnvio = "I";
                                    req.Activo = true;
                                    req.IdUsuario = U.Id;
                                    req.FechaModificacion = DateTime.Now;
                                    predio.IdTipoFaseIp =  idFaseSig; ;
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
                                        req.Folio = Convert.ToInt32(row.Cells[23].Text);
                                        req.TipoImpuesto = "SM";
                                        req.FechaEmision = DateTime.Now;
                                        req.TipoFase =  idFaseSig;
                                        req.FechaLimite = Convert.ToDateTime(row.Cells[24].Text);
                                        String[] periodos = row.Cells[29].Text.Split('-');
                                        String[] periodo = periodos[0].TrimStart().Split(' ');
                                        req.BimestreInicial = Convert.ToInt32(periodo[0]);
                                        req.EjercicioInicial = Convert.ToInt32(periodo[1]);
                                        periodo = periodos[1].TrimStart().Split(' ');
                                        req.BimestreFinal = Convert.ToInt32(periodo[0]);
                                        req.EjercicioFinal = Convert.ToInt32(periodo[1]);
                                        req.Diferencia = null;
                                        req.Impuesto = Convert.ToDecimal(row.Cells[18].Text);
                                        req.Limpieza = Convert.ToDecimal(row.Cells[27].Text);
                                        req.Recoleccion = Convert.ToDecimal(row.Cells[6].Text);
                                        req.Dap = Convert.ToDecimal(row.Cells[25].Text);
                                        req.Adicional = Convert.ToDecimal(row.Cells[19].Text);
                                        req.Recargo = Convert.ToDecimal(row.Cells[21].Text);
                                        req.Rezago = Convert.ToDecimal(row.Cells[20].Text);
                                        req.Diferencia = 0;
                                        req.RecargoDiferencia = 0;
                                        req.Honorarios = Convert.ToDecimal(row.Cells[22].Text);
                                        req.Ejecucion = Convert.ToDecimal(row.Cells[23].Text);
                                        req.Multa = Convert.ToDecimal(row.Cells[24].Text);
                                        req.Importe = Convert.ToDecimal(row.Cells[28].Text); 
                                        req.Status = "A";
                                        req.StatusEnvio = "I";
                                        req.Activo = true;
                                        req.IdUsuario = U.Id;
                                        req.FechaModificacion = DateTime.Now;
                                        predio.IdTipoFaseSm = idFaseSig;
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
                                        //row.Cells[23].Text = Folio.ToString();
                                        //Folio++;
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
                        else
                        {
                            vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                        }
                    }
                    catch (Exception ex)
                    {
                        new Utileria().logError("Requerimiento , vtnMOdal", ex);
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

        protected void chkPExentos_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPExentos.Checked)
                chkExentos.Enabled = false;
            else
                chkExentos.Enabled = true;
        }

        protected void chkExentos_CheckedChanged(object sender, EventArgs e)
        {
            if(chkExentos.Checked)
                chkPExentos.Enabled = false;
            else
                chkPExentos.Enabled = true;
        }

        protected void chkBaldios_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBaldios.Checked)
                chkUBaldios.Enabled = false;
            else
                chkUBaldios.Enabled = true;
        }

        protected void chkUBaldios_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUBaldios.Checked)
                chkBaldios.Enabled = false;
            else
                chkBaldios.Enabled = true;
        }

        protected void grdResultados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdResultados.PageIndex = e.NewPageIndex;
            llenarGridBusqueda(1);            
        }

        protected void grdError_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdError.PageIndex = e.NewPageIndex;
            llenarGridError("","");
        }

        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            tRequerimientoBL rBL = new tRequerimientoBL();
            tRequerimiento valida = new tRequerimiento();
            DateTime fechaGeneracion;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    MensajesInterfaz resul = MensajesInterfaz.Ingreso;
                    //Int32 Folio = Convert.ToInt32(txtFolio.Text);
                    foreach (GridViewRow row in grdEdoCta.Rows)
                    {
                        Boolean bConsiderar = true;
                        fechaGeneracion = DateTime.Now;
                        if (txtRRangoA.Text != "" && txtRRanggoB.Text != "")
                        {
                            long RA = Convert.ToInt64(txtRRangoA.Text);
                            long RB = Convert.ToInt64(txtRRanggoB.Text);
                            if (!(row.RowIndex + 1 >= RA && row.RowIndex + 1 <= RB))
                                bConsiderar = false;
                        }

                        if (bConsiderar)
                        {
                            int idFaseSig = 1;
                            cTipoFase FaseSig = new cTipoFase();

                            cTipoFase FaseActual = new cTipoFaseBL().GetByConstraint(Convert.ToInt32(ddlDocumento.SelectedValue));
                            if (FaseActual != null)
                                FaseSig = new cTipoFaseBL().GetByFase(FaseActual.FaseSiguiente);

                            if (FaseSig != null)
                                idFaseSig = FaseSig.Id;
                            tRequerimiento req;
                            cPredio predio = new cPredioBL().GetByConstraint(Convert.ToInt32(grdEdoCta.DataKeys[row.RowIndex].Values["IdPredio"].ToString()));
                            if (HttpUtility.HtmlDecode(row.Cells[4].Text).Trim() != "")
                            {
                                req = new tRequerimiento();
                                req.IdPredio = Convert.ToInt32(grdEdoCta.DataKeys[row.RowIndex].Values["IdPredio"].ToString());
                                req.Folio = Convert.ToInt32(HttpUtility.HtmlDecode(row.Cells[29].Text));
                                req.TipoImpuesto = "IP";
                                req.FechaEmision = fechaGeneracion;// Convert.ToDateTime(HttpUtility.HtmlDecode(row.Cells[30].Text));
                                //cTipoFase fase = new cTipoFaseBL().GetFilter("Fase", predio.cTipoFase.Fase.ToString(), "true", "FaseSiguiente", "asc").FirstOrDefault();                               
                                req.TipoFase = idFaseSig;
                                req.FechaLimite = Convert.ToDateTime(HttpUtility.HtmlDecode(row.Cells[30].Text));
                                String[] periodos = HttpUtility.HtmlDecode(row.Cells[4].Text).Split('-');
                                String[] periodo = periodos[0].TrimStart().Split(' ');
                                req.BimestreInicial = Convert.ToInt32(periodo[0]);
                                req.EjercicioInicial = Convert.ToInt32(periodo[1]);
                                periodo = periodos[1].TrimStart().Split(' ');
                                req.BimestreFinal = Convert.ToInt32(periodo[0]);
                                req.EjercicioFinal = Convert.ToInt32(periodo[1]);
                                //req.Diferencia = Convert.ToDecimal(row.Cells[10].Text);
                                req.Impuesto = Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[6].Text));
                                req.Adicional = Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[7].Text));
                                req.Recargo = Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[9].Text));
                                req.Rezago = Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[8].Text));
                                req.Diferencia = Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[10].Text));
                                req.RecargoDiferencia = Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[11].Text));
                                req.Honorarios =  Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[12].Text));
                                req.Ejecucion = Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[13].Text));
                                req.Multa = Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[14].Text));
                                req.Importe = Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[15].Text));
                                req.Status = "A";
                                req.StatusEnvio = "I";
                                req.Activo = true;
                                req.IdUsuario = U.Id;
                                req.FechaModificacion = fechaGeneracion;
                                predio.IdTipoFaseIp = idFaseSig;

                                if ( FaseActual.Id == 2)
                                {
                                    resul  = new tRequerimientoBL().CambiaRequerimientoAfaseSig(req.IdPredio, FaseActual.Id, "IP", U.Id);
                                    if (resul == MensajesInterfaz.Actualizacion)
                                        ((HiddenField)row.FindControl("hdfIdIP")).Value = req.Id.ToString();
                                    resul = new tRequerimientoBL().CambiaRequerimientoAfaseSig(req.IdPredio, FaseActual.Id, "SM", U.Id);
                                    if (resul == MensajesInterfaz.Actualizacion)
                                        ((HiddenField)row.FindControl("hdfIdSM")).Value = req.Id.ToString();
                                    valida = null;
                                }           
                                                    
                                
                                    //validar que no exista un requerimiento activo de la misma fase
                                valida = new tRequerimientoBL().obtenerRequerimientoporIdPredio(req.IdPredio, "IP");
                                if (valida != null)
                                {
                                    row.Cells[32].Text = "Predio con documento activo, se debe cancelar antes de generar otro documento.";
                                    resul = MensajesInterfaz.NoActualizar;
                                }
                                else
                                {

                                    resul = rBL.Insert(req);
                                    if (resul == MensajesInterfaz.Ingreso)
                                        ((HiddenField)row.FindControl("hdfIdIP")).Value = req.Id.ToString();
                                }
                               
                              
                            }
                            if (resul == MensajesInterfaz.Ingreso && valida == null)
                            {
                                if (HttpUtility.HtmlDecode(row.Cells[17].Text).Trim() != "")
                                {
                                    req = new tRequerimiento();
                                    req.IdPredio = Convert.ToInt32(grdEdoCta.DataKeys[row.RowIndex].Values["IdPredio"].ToString());
                                    req.Folio = Convert.ToInt32(HttpUtility.HtmlDecode(row.Cells[29].Text)); 
                                    req.TipoImpuesto = "SM";
                                    req.FechaEmision = fechaGeneracion;// Convert.ToDateTime(HttpUtility.HtmlDecode(row.Cells[30].Text));
                                    ///cTipoFase fase = new cTipoFaseBL().GetFilter("Fase", predio.cTipoFase.Fase.ToString(), "true", "FaseSiguiente", "asc").FirstOrDefault();
                                    //Arturo// req.TipoFase = fase.Id;
                                    req.TipoFase = idFaseSig;
                                    req.FechaLimite = Convert.ToDateTime(HttpUtility.HtmlDecode(row.Cells[30].Text));                                    
                                    String[] periodos = row.Cells[17].Text.Split('-'); // row.Cells[15].Text.Split('-');
                                    String[] periodo = periodos[0].TrimStart().Split(' ');// periodos[0].TrimStart().Split(' ');
                                    req.BimestreInicial = Convert.ToInt32(periodo[0]);
                                    req.EjercicioInicial = Convert.ToInt32(periodo[1]);
                                    periodo = periodos[1].TrimStart().Split(' ');
                                    req.BimestreFinal = Convert.ToInt32(periodo[0]);
                                    req.EjercicioFinal = Convert.ToInt32(periodo[1]);
                                    req.Diferencia = null;
                                    req.Impuesto = Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[18].Text));
                                    req.Limpieza = Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[27].Text));
                                    req.Recoleccion = Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[26].Text));
                                    req.Dap = Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[25].Text));
                                    req.Adicional = Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[19].Text));
                                    req.Recargo = Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[21].Text));
                                    req.Rezago = Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[20].Text));
                                    req.Diferencia = 0;
                                    req.RecargoDiferencia = 0;
                                    req.Honorarios = Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[22].Text));
                                    req.Ejecucion = Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[23].Text));
                                    req.Multa = Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[24].Text));
                                    req.Importe = Convert.ToDecimal(HttpUtility.HtmlDecode(row.Cells[28].Text));                                    
                                    req.Status = "A";
                                    req.StatusEnvio = "I";
                                    req.Activo = true;
                                    req.IdUsuario = U.Id;
                                    req.FechaModificacion = fechaGeneracion;
                                    predio.IdTipoFaseSm = idFaseSig;
                                    //predio.IdTipoFaseIp = 2;
                                    //resul = rBL.Insert(req);
                                    //if (resul == MensajesInterfaz.Ingreso)
                                    //    ((HiddenField)row.FindControl("hdfIdSM")).Value = req.Id.ToString();

                                    if (FaseActual.Id > 2)
                                    {
                                        resul = new tRequerimientoBL().CambiaRequerimientoAfaseSig(req.IdPredio, FaseActual.Id, "SM", U.Id);
                                        if (resul == MensajesInterfaz.Actualizacion)
                                            ((HiddenField)row.FindControl("hdfIdIP")).Value = req.Id.ToString();
                                    }
                                    else
                                    {
                                        resul = rBL.Insert(req);
                                        if (resul == MensajesInterfaz.Ingreso)
                                            ((HiddenField)row.FindControl("hdfIdIP")).Value = req.Id.ToString();
                                    }
                                }
                            }
                            if (resul == MensajesInterfaz.Ingreso && valida == null)
                            {
                                predio.FechaModificacion = DateTime.Today;
                                predio.IdUsuario = U.Id;
                                resul = new cPredioBL().Update(predio);
                                if (resul == MensajesInterfaz.Actualizacion)
                                {                                   
                                    resul = MensajesInterfaz.Ingreso;
                                }
                            }
                        }
                    }
                    if (resul == MensajesInterfaz.Ingreso && valida == null)
                    {
                        scope.Complete();
                        TabContainerEdoCta.TabIndex = 2;
                        vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                    }
                    else
                    {
                        vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                    }
                }
                catch (Exception ex)
                {
                    new Utileria().logError("Requerimiento , btnSiguiente", ex);
                    vtnModal.DysplayCancelar = false;
                    vtnModal.ShowPopup("Error al intentar actualizar información", ModalPopupMensaje.TypeMesssage.Error);
                    vtnModal.DysplayCancelar = true;
                }
            }
        }

        protected void imgBorrarCon_Click(object sender, ImageClickEventArgs e)
        {
            txtContribuyente.Text = "";
            hdfIdCon.Value = "";
        }

        protected void imbBorrarCondominio_Click(object sender, ImageClickEventArgs e)
        {
            txtCondominio.Text = "";
            hdfIdCondominio.Value = "";
        }

        protected void imbBorrarColonia_Click(object sender, ImageClickEventArgs e)
        {
            txtColonia.Text = "";
            hdfIdColonia.Value = "";
        }
        
        //protected void btnGenerar_Click(object sender, EventArgs e)
        //{
        //    List<List<string>> parametros = new List<List<string>>();
        //    DataTable dt = (DataTable)ViewState["dt"];
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        List<string> param = new List<string>();
        //        String[] periodo = dt.Rows[i]["Periodo"].ToString().Split('-');
        //        Int32 EIni = Convert.ToInt32(periodo[0]);
        //        Int32 BIni = Convert.ToInt32(periodo[1]);

        //        param.Add(dt.Rows[i]["IdPredio"].ToString());
        //        param.Add("NO");
        //        param.Add(BIni.ToString());
        //        param.Add(EIni.ToString());
        //        param.Add(ddlBimestre.SelectedValue.ToString());
        //        param.Add(ddlAnio.SelectedValue.ToString());
        //        param.Add("-1");
        //        param.Add(txtFecha.Text);
        //        param.Add("Rezago");
        //        parametros.Add(param);

        //        //Impuesto impuesto = new SaldosC().CalculaCobro(Convert.ToInt32(grdResultados.DataKeys[i].Values["IdPredio"]), "NO", BIni, EIni, Convert.ToInt32(ddlBimestre.SelectedValue), Convert.ToInt32(ddlAnio.SelectedValue), -1);
        //    }

        //    //for (int i = 0; i < grdResultados.Rows.Count; i++)
        //    //{
        //    //    List<string> param = new List<string>();
        //    //    String[] periodo = grdResultados.Rows[i].Cells[2].Text.Split('-');
        //    //    Int32 EIni = Convert.ToInt32(periodo[0]);
        //    //    Int32 BIni = Convert.ToInt32(periodo[1]);

        //    //    param.Add(grdResultados.DataKeys[i].Values["IdPredio"].ToString());
        //    //    param.Add("NO");
        //    //    param.Add(BIni.ToString());
        //    //    param.Add(EIni.ToString());
        //    //    param.Add(ddlBimestre.SelectedValue.ToString());
        //    //    param.Add(ddlAnio.SelectedValue.ToString());
        //    //    param.Add("-1");
        //    //    param.Add(txtFecha.Text);
        //    //    param.Add("Rezago");
        //    //    parametros.Add(param);

        //    //    //Impuesto impuesto = new SaldosC().CalculaCobro(Convert.ToInt32(grdResultados.DataKeys[i].Values["IdPredio"]), "NO", BIni, EIni, Convert.ToInt32(ddlBimestre.SelectedValue), Convert.ToInt32(ddlAnio.SelectedValue), -1);
        //    //}

        //    DataTable dtEdosCta = new tRequerimientoBL().GenerarEdoCuenta(parametros);
        //    grdEdoCta.DataSource = dtEdosCta;
        //    grdEdoCta.DataBind();
        //    txtTarjetas.Text = "";
        //    TabContainerRequerimiento.ActiveTabIndex = 2;
        //}

        public string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') ||  c == ',' )
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }


}