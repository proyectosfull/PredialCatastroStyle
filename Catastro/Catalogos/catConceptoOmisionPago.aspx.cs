using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Transactions;
using System.Web.UI.WebControls;
using Clases.BL;
using Catastro.Controles;
using Clases;
using Clases.Utilerias;


namespace Catastro.Catalogos
{
    public partial class catConceptoOmisionPago : System.Web.UI.Page
    {       
        protected void Page_Load(object sender, EventArgs e)
        {                     
            if (!IsPostBack)
            {
                Session["parametro"] = null;                                               
                                           
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);                                  
        }

        private void llenarConcepto()
        {
            List<cParametroSistema> listParametroSistema = new cParametroSistemaBL().GetFilter("TipoCampo", "TablaOmision", "true", "Clave", "asc");

            if (listParametroSistema.Count > 0)
            {
                if (grd.Rows.Count > 0)
                {
                    foreach (GridViewRow gvr in grd.Rows)
                    {
                        int index = -1;
                        Label clave = gvr.FindControl("lblClave") as Label;
                        for (int i = 0; i < listParametroSistema.Count; i++)
                            if (clave.Text == listParametroSistema[i].Clave)
                            {
                                index = i;
                            }
                        if (index != -1)
                        {
                            listParametroSistema.RemoveAt(index);
                        }
                    }
                }
            }
            
                var lista = listParametroSistema.Select(l => new { Id = l.Clave, Nombre = l.Descripcion });
                ddlConcepto.Items.Clear();
                ddlConcepto.DataValueField = "Id";
                ddlConcepto.DataTextField = "Nombre";
                ddlConcepto.DataSource = lista;
                ddlConcepto.DataBind();
                ddlConcepto.Items.Insert(0, new ListItem("Selecciona el Concepto", "%"));
            
        }

        protected void guardarConceptoOmision()
        {
             try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (grd.Rows.Count > 0)
                    {

                        cUsuarios U = new cUsuarios();
                        U = (cUsuarios)Session["usuario"];
                        cPredio predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);
                        MensajesInterfaz msg = new MensajesInterfaz();
                        foreach (GridViewRow gvr in grd.Rows)
                        {
                            Label clave = gvr.FindControl("lblClave") as Label;
                            Label Descripcion = gvr.FindControl("lblDescripcion") as Label;
                            Label FechaInicio = gvr.FindControl("lblFechaInicio") as Label;
                            Label FechaFin = gvr.FindControl("lblFechaFin") as Label;
                            Label Id = gvr.FindControl("lblId") as Label;
                            Label Status = gvr.FindControl("lblStatus") as Label;
                            Label Activo = gvr.FindControl("lblActivo") as Label;
                            Label Cambio = gvr.FindControl("lblCambio") as Label;

                            cConceptoOmisionPago concepto = new cConceptoOmisionPago(); ;
                            if (Cambio.Text == "N")
                            {                                
                                concepto.Activo = true;
                            }
                            else
                            {
                                concepto = new cConceptoOmisionPagoBL().GetByConstraint(Convert.ToInt32(Id.Text));
                                concepto.Activo = Convert.ToBoolean(Activo.Text);
                            }

                            concepto.ClaveParametro = clave.Text;                            
                            concepto.FechaInicio = Convert.ToDateTime(FechaInicio.Text);
                            concepto.FechaFin = Convert.ToDateTime(FechaFin.Text);
                            concepto.IdUsuario = U.Id;
                            concepto.IdPredio = predio.Id;                            
                            concepto.FechaModificacion = DateTime.Now;

                            if (Cambio.Text == "N")
                            {
                                msg = new cConceptoOmisionPagoBL().Insert(concepto);
                            }
                            else
                            {
                                msg = new cConceptoOmisionPagoBL().Update(concepto);
                            }

                        }

                        vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);                        
                        buscarClaveCatastral(null,null);
                        scope.Complete();
                    }
                    else {
                        vtnModal.ShowPopup(new Utileria().GetDescription("Debe de Agregar un concepto"), ModalPopupMensaje.TypeMesssage.Alert);
                    }
              }
             }
            catch (Exception error)
            {
                new Utileria().logError("catConceptoOmisionPago, guardarConceptoOmision", error);
                vtnModal.ShowPopup(new Utileria().GetDescription(error.Message), ModalPopupMensaje.TypeMesssage.Alert);

            }                    
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            vtnModal.DysplayCancelar = true;
            vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfirmacionGuardado), ModalPopupMensaje.TypeMesssage.Confirm);
        }
        protected void btnRecargar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ViewState["mensajeCambio"]) > 0) {
                vtnModal.DysplayCancelar = true;
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.RegresarMSG), ModalPopupMensaje.TypeMesssage.Confirm);
            }
            else
            {
                Response.Redirect("catConceptoOmisionPago.aspx");
            }
           
            
        }
       

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ModificarRegistro")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                cConceptoOmisionPago conceptoOmision = new cConceptoOmisionPagoBL().GetByConstraint(id);
                txtFechaInicio.Text = conceptoOmision.FechaInicio.ToString("dd/MM/yyyy");
                txtFechaFin.Text = conceptoOmision.FechaFin.ToString("dd/MM/yyyy");
                ddlConcepto.Items.Clear();
                cParametroSistema parametroSistemas = new cParametroSistemaBL().GetFilter("Clave", conceptoOmision.ClaveParametro, "true", "Clave", "asc").FirstOrDefault();
                ddlConcepto.Items.Insert(0, new ListItem(parametroSistemas.Descripcion, parametroSistemas.Clave));
                ViewState["idMod"] = id;
                btnAgregarConcepto.Text = "Editar Concepto";
                btnCancelarConcepto.Visible = true;
            }
            else if (e.CommandName == "EliminarRegistro")
            {
                 string id = e.CommandArgument.ToString();

                 if (id.Substring(0, id.Length - 1) !="N")
                {
                    eliminarActivarConcepto(false, id);
                }
                else {
                    eliminarActivarConcepto(false, id);
                    llenarConcepto();
                }
                 if (grd.Rows.Count == 0) {
                     txtClvCastatral.ReadOnly = false;
                 }
            }
            else if (e.CommandName == "ActivarRegistro")
            {
                string id = e.CommandArgument.ToString();
                eliminarActivarConcepto(true, id);
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
        }

        protected void btnCancelarConcepto_Click(object sender, EventArgs e) {

            llenarConcepto();
            txtFechaInicio.Text = "";
            txtFechaFin.Text = "";
            ViewState["idMod"] = 0;
            btnCancelarConcepto.Visible = false;
            btnAgregarConcepto.Text = "Agregar Concepto";
        }
        protected void btnAgregarConcepto_Click(object sender, EventArgs e)
        {
            DateTime fechaInicioTxt = Convert.ToDateTime(txtFechaInicio.Text);
            DateTime fechaFinTxt = Convert.ToDateTime(txtFechaFin.Text);
         
            if (fechaInicioTxt > fechaFinTxt)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("Imposible agregar el Concepto, la Fecha Inicio no Puede ser mayor que la fecha fin"),
                    ModalPopupMensaje.TypeMesssage.Alert);
            }
            else
            {
                List<conceptoGrid> listConcepto = new List<conceptoGrid>();
                int index = 1;
                int idMod = 0;
                if (Convert.ToInt32(ViewState["idMod"])!=0)
                    idMod = Convert.ToInt32(ViewState["idMod"]);
                if (grd.Rows.Count > 0)
                {
                    foreach (GridViewRow gvr in grd.Rows)
                    {   
                        Label clave = gvr.FindControl("lblClave") as Label;
                        Label Descripcion = gvr.FindControl("lblDescripcion") as Label;
                        Label FechaInicio = gvr.FindControl("lblFechaInicio") as Label;
                        Label FechaFin = gvr.FindControl("lblFechaFin") as Label;
                        Label Id = gvr.FindControl("lblId") as Label;
                        Label Status = gvr.FindControl("lblStatus") as Label;
                        Label Activo = gvr.FindControl("lblActivo") as Label;
                        Label Cambio = gvr.FindControl("lblCambio") as Label;
                        conceptoGrid conceptoAux = new conceptoGrid();
                        conceptoAux.Clave = clave.Text;
                        conceptoAux.Descripcion = Descripcion.Text;
                        conceptoAux.Cambio = Cambio.Text;
                        conceptoAux.Activo = Convert.ToBoolean(Activo.Text);
                        if (Id.Text.Substring(0, Id.Text.Length - 1) == "N")
                        {
                            conceptoAux.Id = "N" + index;
                            index++;
                        }
                        else { 
                            conceptoAux.Id = Id.Text; 
                        }                       
                        conceptoAux.Status = Status.Text;
                        conceptoAux.FechaInicio = Convert.ToDateTime(FechaInicio.Text).ToString("dd/MM/yyyy");
                        conceptoAux.FechaFin = Convert.ToDateTime(FechaFin.Text).ToString("dd/MM/yyyy");
                        if (idMod != 0) {
                            if (idMod.ToString() != Id.Text) {
                                listConcepto.Add(conceptoAux);
                            }
                        }else{
                        listConcepto.Add(conceptoAux);
                         }
                    }
                }
                conceptoGrid concepto = new conceptoGrid();
                if (idMod != 0)
                {
                    cConceptoOmisionPago conceptoOmi = new cConceptoOmisionPagoBL().GetByConstraint(idMod);
                    concepto.Activo = true;
                    concepto.Status = conceptoOmi.cPredio.cStatusPredio.Descripcion;
                    concepto.Id = conceptoOmi.Id.ToString();
                    concepto.Cambio = "M";
                }
                else
                {
                    concepto.Activo = false;
                    concepto.Status = "A";
                    concepto.Id = "N" + index;
                    concepto.Cambio = "N";
                }
                concepto.Clave = ddlConcepto.SelectedValue;
                concepto.Descripcion = ddlConcepto.SelectedItem.Text;
                concepto.FechaInicio = Convert.ToDateTime(txtFechaInicio.Text).ToString("dd/MM/yyyy");
                concepto.FechaFin = Convert.ToDateTime(txtFechaFin.Text).ToString("dd/MM/yyyy");

                listConcepto.Add(concepto);
                grd.DataSource = listConcepto;
                grd.DataBind();
                txtClvCastatral.ReadOnly = true;
                llenarConcepto();
                txtFechaInicio.Text = "";
                txtFechaFin.Text = "";
                btnAgregarConcepto.Text = "Agregar Concepto";
                btnCancelarConcepto.Visible = false;
            }
        }     


        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {

            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.RegresarMSG))
            {
                Response.Redirect("catConceptoOmisionPago.aspx");
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfirmacionGuardado))
            {
                guardarConceptoOmision();
            }
           
        }
       

        protected void buscarClaveCatastral(object sender, System.EventArgs e)
        {
            if (txtClvCastatral.Text.Length == 12)
            {
                cPredio Predio = new cPredioBL().GetByClavePredial(txtClvCastatral.Text);
                if (Predio == null)
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.PredioInexistente), ModalPopupMensaje.TypeMesssage.Alert);
                    txtClvCastatral.Text = "";
                }
                else {
                   ViewState["idMod"] = 0;
                   ViewState["mensajeCambio"] = 0;
                   llenagrid(Predio.Id);
                   string fechaIncio =  getFechaInicio (Predio.BimestreFinSm , Predio.AaFinalSm);
                   MaskedEditValidator1.MinimumValue = fechaIncio;
                   MaskedEditValidator1.MinimumValueMessage = "La fecha incio minima es " + fechaIncio;                   
                   llenarConcepto();
                   txtFechaInicio.Text = "";
                   txtFechaFin.Text = "";                                      
                }
            }
            else
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.FormatoPredio), ModalPopupMensaje.TypeMesssage.Alert);
                txtClvCastatral.Text = "";
            }
        }

        public partial class conceptoGrid
        {
            public string Clave { get; set; }
            public string Descripcion { get; set; }
            public string FechaInicio { get; set; }
            public string FechaFin { get; set; }
            public string Status { get; set; }
            public Boolean Activo { get; set; }
            public string Id { get; set; }
            public string Cambio { get; set; }   

        }
        private void eliminarActivarConcepto(Boolean activo, string id)
        {

            List<conceptoGrid> listConcepto = new List<conceptoGrid>();
            if (grd.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in grd.Rows)
                {    Boolean continuar = true;
                    Label clave = gvr.FindControl("lblClave") as Label;
                    Label Descripcion = gvr.FindControl("lblDescripcion") as Label;
                    Label FechaInicio = gvr.FindControl("lblFechaInicio") as Label;
                    Label FechaFin = gvr.FindControl("lblFechaFin") as Label;
                    Label Id = gvr.FindControl("lblId") as Label;
                    Label Status = gvr.FindControl("lblStatus") as Label;
                    Label Activo = gvr.FindControl("lblActivo") as Label;
                    Label Cambio = gvr.FindControl("lblCambio") as Label;
                    conceptoGrid conceptoAux = new conceptoGrid();

                    if (id.Substring(0, id.Length - 1) == "N")
                    {
                        if (id == Id.Text) {
                            continuar = false;
                        }
                    }

                    if (continuar)
                    {
                        Boolean ActivoAux = Convert.ToBoolean(Activo.Text);
                        string CambioAux = Cambio.Text;
                        if (id.Substring(0, id.Length - 1) != "N")
                        {
                            if (id == Id.Text) { 
                                ActivoAux = activo;
                                CambioAux = "M";
                            }
                        }
                        conceptoAux.Clave = clave.Text;
                        conceptoAux.Descripcion = Descripcion.Text;
                        conceptoAux.Cambio = CambioAux;
                        conceptoAux.Activo = ActivoAux;
                        conceptoAux.Id = Id.Text;
                        conceptoAux.Status = Status.Text;
                        conceptoAux.FechaInicio = Convert.ToDateTime(FechaInicio.Text).ToString("dd/MM/yyyy");
                        conceptoAux.FechaFin = Convert.ToDateTime(FechaFin.Text).ToString("dd/MM/yyyy");
                        listConcepto.Add(conceptoAux);
                    }
                }
            }           
            grd.DataSource = listConcepto;
            grd.DataBind();           
            llenarConcepto();
           
        
        }

        private void llenagrid(int idPredio)
        {

            List<cConceptoOmisionPago> listConceptoOmision = new cConceptoOmisionPagoBL().GetAllIdPredio(idPredio);
            if (listConceptoOmision.Count> 0)
            {
                List<conceptoGrid> conceptos = new List<conceptoGrid>();
                foreach (cConceptoOmisionPago cCP in listConceptoOmision)
                { 
                conceptoGrid concepto = new conceptoGrid();
                cParametroSistema parametroSistemas = new cParametroSistemaBL().GetFilter("Clave", cCP.ClaveParametro, "true", "Clave", "asc").FirstOrDefault();
                concepto.Clave = cCP.ClaveParametro;
                concepto.Activo = cCP.Activo;
                concepto.Descripcion = parametroSistemas.Descripcion;
                concepto.FechaInicio = cCP.FechaInicio.ToString("dd/MM/yyyy");
                concepto.FechaFin = cCP.FechaFin.ToString("dd/MM/yyyy");
                concepto.Id = cCP.Id.ToString();
                concepto.Status = cCP.cPredio.cStatusPredio.Descripcion;
                concepto.Cambio = "G";
                conceptos.Add(concepto);
                }
                    
                grd.DataSource = conceptos;
                grd.DataBind();
            }                       
        }

        protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
#pragma warning disable CS0252 // Posible comparación de referencias no intencionada; para obtener una comparación de valores, convierta el lado de la izquierda en el tipo 'string'
                if (DataBinder.Eval(e.Row.DataItem, "Cambio") != "G")
                {

                    int cambios = Convert.ToInt32(ViewState["mensajeCambio"]);
                    cambios+=1;
                    ViewState["mensajeCambio"] = cambios;

                }
#pragma warning restore CS0252 // Posible comparación de referencias no intencionada; para obtener una comparación de valores, convierta el lado de la izquierda en el tipo 'string'
                string id =  grd.DataKeys[e.Row.RowIndex].Values[0].ToString();
                string activo = grd.DataKeys[e.Row.RowIndex].Values[1].ToString();
                string status = grd.DataKeys[e.Row.RowIndex].Values[2].ToString();
                if(status.ToUpper() == "S" || status.ToUpper() == "B"){
                    ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                    imgActivar.Visible = false;
                    ImageButton imgUpdate = (ImageButton)e.Row.FindControl("imgUpdate");
                    imgUpdate.Visible = false;
                    ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                    imgDelete.Visible = false;
                }else{
                if (activo.ToUpper() == "TRUE")
                {
                    ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                    imgActivar.Visible = false;
                }
                else
                {                    
                    if (id.Substring(0, id.Length - 1) == "N")
                    {
                        ImageButton imgUpdate = (ImageButton)e.Row.FindControl("imgUpdate");
                        imgUpdate.Visible = false;
                        ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                        imgActivar.Visible = false;
                    }
                    else
                    {
                        ImageButton imgUpdate = (ImageButton)e.Row.FindControl("imgUpdate");
                        imgUpdate.Visible = false;
                        ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                        imgDelete.Visible = false;
                    }
                }
            }
            
            }
            if (e.Row.RowType == DataControlRowType.Footer) {
                Label lbl = (Label)e.Row.FindControl("lblCambios");
                if (Convert.ToInt32(ViewState["mensajeCambio"]) > 0)
                {
                    lbl.Text = "Cambios pendientes por Guardar";
                }
                else {
                    lbl.Text = "";
                }
            }
        }
        protected string getFechaInicio(int bimestre, int ejercicio) {
            string fecha = "";
            if (bimestre <= 4) {
                fecha = "01/0" + ((bimestre * 2) + 1) + "/" + ejercicio;
            }
            else if (bimestre == 5)
            {
                fecha = "01/" + ((bimestre * 2) + 1) + "/" + ejercicio;
            }
            else {
                fecha = "01/01" + "/" + (ejercicio+1);
            }

            return fecha;
        }
    }
}