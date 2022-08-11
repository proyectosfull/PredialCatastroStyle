using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases.BL;
using Catastro.Controles;
using Clases;
using Clases.Utilerias;
using System.Transactions;

namespace Transito.Catalogos
{
    public partial class catIndicePrecio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["sortCampo"] = "Ejercicio";
                ViewState["sortOnden"] = "desc";
                chkInactivo.Checked = true;
                llenagrid();
                llenaFiltro();
                txtFiltro.Enabled = false;
                validadorEjercicio.MaximumValue = DateTime.Now.Year.ToString();
                validadorEjercicio.ErrorMessage = "Ingresar Ejercicio menor o igual a " + DateTime.Now.Year;

            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }


        private void llenaFiltro()
        {
            //ddlFiltro.DataSource = new cIndicePrecioBL().ListaCampos();
            //ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
            ddlFiltro.Items.Insert(1, new ListItem("Ejercicio", "Ejercicio"));
        }

        private void llenagrid()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new cIndicePrecioBL().obtenerAniosCatalgos(true, "");
                grd.DataBind();
            }
            else
            {
                if (filtro[0].ToString().ToUpper() == "")
                    grd.DataSource = new cIndicePrecioBL().obtenerAniosCatalgos(Convert.ToBoolean(filtro[2]), "");
                else
                    grd.DataSource = new cIndicePrecioBL().obtenerAniosCatalgos(Convert.ToBoolean(filtro[2]), filtro[1]);
                grd.DataBind();
            }
        }

        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd.PageIndex = e.NewPageIndex;
            llenagrid();
        }

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ConsultarRegistro")
            {
                lbl_titulo.Text = "Consulta Índice de Precios";
                pnl_Modal.Show();
                int ejercicio = Convert.ToInt32(e.CommandArgument);
                List<cIndicePrecio> lista = new cIndicePrecioBL().GetFilter("Ejercicio", ejercicio.ToString(), "true", "Mes", "asc");
                limpiaCampos();
                habilitaCampos(false);
                if (lista != null)
                {
                    txtEjercicio.Text = lista[0].Ejercicio.ToString();
                    llenarMeses(lista);
                    configurarReqs(1);
                }
                btnGuardar.Visible = false;
            }
            else if (e.CommandName == "ModificarRegistro")
            {
                lbl_titulo.Text = "Modifica Índice de Precios";
                int ejercicio = Convert.ToInt32(e.CommandArgument);
                List<cIndicePrecio> lista = new cIndicePrecioBL().GetFilter("Ejercicio", ejercicio.ToString(), "true", "Mes", "asc");
                limpiaCampos();
                habilitaCampos(true);
                if (lista != null)
                {
                    txtEjercicio.Text = lista[0].Ejercicio.ToString();
                    llenarMeses(lista);
                }
                btnGuardar.Visible = true;
                txtEjercicio.Enabled = false;
                ViewState["idMod"] = ejercicio;
                configurarReqs(Convert.ToInt32(txtEjercicio.Text));
                pnl_Modal.Show();
            }
            else if (e.CommandName == "EliminarRegistro")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;
                vtnModal.DysplayCancelar = true;
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar), ModalPopupMensaje.TypeMesssage.Confirm);
            }
            else if (e.CommandName == "ActivarRegistro")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["idMod"] = id;
                vtnModal.DysplayCancelar = true;
                vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro), ModalPopupMensaje.TypeMesssage.Confirm);
            }

        }

        protected void llenarMeses(List<cIndicePrecio> lista)
        {
            //int limite = DateTime.Now.Month;
            //if (lista[0].Ejercicio == DateTime.Now.Year)
            //{
            //    if (limite != 12)
            //        limite = limite + 1;
            //    else
            //        limite = 1;
            //    configurarReqs(limite, lista[0].Ejercicio);
            //}
            configurarReqs(lista[0].Ejercicio);
            foreach (cIndicePrecio indice in lista)
            {
                switch (indice.Mes)
                {
                    case 1:

                        txtEnero.Text = indice.Valor.ToString();
                        break;
                    case 2:

                        txtFebrero.Text = indice.Valor.ToString();
                        break;
                    case 3:
                        txtMarzo.Text = indice.Valor.ToString();
                        break;
                    case 4:
                        txtAbril.Text = indice.Valor.ToString();
                        break;
                    case 5:
                        txtMayo.Text = indice.Valor.ToString();
                        break;
                    case 6:
                        txtJunio.Text = indice.Valor.ToString();
                        break;
                    case 7:
                        txtJulio.Text = indice.Valor.ToString();
                        break;
                    case 8:
                        txtAgosto.Text = indice.Valor.ToString();
                        break;
                    case 9:
                        txtSeptiembre.Text = indice.Valor.ToString();
                        break;
                    case 10:
                        txtOctubre.Text = indice.Valor.ToString();
                        break;
                    case 11:
                        txtNoviembre.Text = indice.Valor.ToString();
                        break;
                    case 12:
                        txtDiciembre.Text = indice.Valor.ToString();
                        break;
                }
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

            llenagrid();
        }

        protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string activo = grd.DataKeys[e.Row.RowIndex].Values[0].ToString();
                if (activo.ToUpper() == "TRUE")
                {
                    ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
                    imgActivar.Visible = false;
                }
                else
                {
                    ImageButton imgConsulta = (ImageButton)e.Row.FindControl("imgConsulta");
                    imgConsulta.Visible = false;
                    ImageButton imgUpdate = (ImageButton)e.Row.FindControl("imgUpdate");
                    imgUpdate.Visible = false;
                    ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                    imgDelete.Visible = false;
                }
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            limpiaCampos();
            habilitaCampos(true);
            configurarReqs(0);
            lbl_titulo.Text = "Alta de Nuevo Índice de Precios";
            pnl_Modal.Show();
            btnGuardar.Visible = true;
            txtEjercicio.Focus();
        }

        protected Decimal toDecimal(string o)
        {
            return Convert.ToDecimal(o.Replace(".", "."));
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {            
            cUsuarios U = new cUsuarios();
            U = (cUsuarios)Session["usuario"];
            pnl_Modal.Hide();
            vtnModal.DysplayCancelar = false;

            MensajesInterfaz msg =new MensajesInterfaz();
            cIndicePrecio Indice = new cIndicePrecio();
            cIndicePrecioBL ibl=new cIndicePrecioBL();
            DateTime t=DateTime.Now;
            int ejercicio= Convert.ToInt32(txtEjercicio.Text.Trim());
            decimal d=0;
            decimal val=0;
            Int32 idEjercicio = Convert.ToInt32(ViewState["idMod"]);
            List<int> mesp=new List<int>();
           

            if (idEjercicio == 0)
            {
                if (ibl.getIsEjercicio(ejercicio))
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription("Imposible registrar el Indice Precio,  ya se encuentra dado de alta."), ModalPopupMensaje.TypeMesssage.Alert);
                    return;
                }
            }

#region reqs
            if(ReqEnero.Enabled==true)
                mesp.Add(1);
            if(ReqFebrero.Enabled==true)
                mesp.Add(2);
            if(ReqMarzo.Enabled==true)
                mesp.Add(3);
            if(ReqAbril.Enabled==true)
                mesp.Add(4);
            if(ReqMayo.Enabled==true)
                mesp.Add(5);
            if(ReqJunio.Enabled==true)
                mesp.Add(6);
            if(ReqJulio.Enabled==true)
                mesp.Add(7);
            if(ReqAgosto.Enabled==true)
                mesp.Add(8);
            if(ReqSeptiembre.Enabled==true)
                mesp.Add(9);
            if(ReqOctubre.Enabled==true)
                mesp.Add(10);
            if(ReqNoviembre.Enabled==true)
                mesp.Add(11);
            if(ReqDiciembre.Enabled==true)
                mesp.Add(12);
#endregion

            foreach(int m in mesp)
            {
                d = 0;
                Indice=null;
                d=ibl.GetByMesEjercicio(m,ejercicio);
#region mesesVal
                if(m==1)
                    val=Convert.ToDecimal(txtEnero.Text.Trim());
                else if(m==2)
                    val=Convert.ToDecimal(txtFebrero.Text.Trim());
                else if(m==3)
                    val=Convert.ToDecimal(txtMarzo.Text.Trim());
                else if(m==4)
                    val=Convert.ToDecimal(txtAbril.Text.Trim());
                else if(m==5)
                    val=Convert.ToDecimal(txtMayo.Text.Trim());
                else if(m==6)
                    val=Convert.ToDecimal(txtJunio.Text.Trim());
                else if(m==7)
                    val=Convert.ToDecimal(txtJulio.Text.Trim());
                else if(m==8)
                    val=Convert.ToDecimal(txtAgosto.Text.Trim());
                else if(m==9)
                    val=Convert.ToDecimal(txtSeptiembre.Text.Trim());
                else if(m==10)
                    val=Convert.ToDecimal(txtOctubre.Text.Trim());
                else if(m==11)
                    val=Convert.ToDecimal(txtNoviembre.Text.Trim());
                else if(m==12)
                    val=Convert.ToDecimal(txtDiciembre.Text.Trim());
#endregion
                if(d!=0)
                {
                    if(d!=val)
                    {
                        Indice=ibl.obtenerAniosMes(ejercicio,m);
                        Indice.Valor=val;
                        Indice.FechaModificacion=t;
                        Indice.IdUsuario=U.Id;
                        //Indice.Activo = false;
                        msg=ibl.Update(Indice);
                    }
                }
                else
                {
                    new cIndicePrecioBL().InactivaEjercicioMes(ejercicio, m);

                    Indice = new cIndicePrecio();
                    Indice.Activo=true;
                    Indice.Ejercicio=ejercicio;
                    Indice.FechaModificacion=t;
                    Indice.IdUsuario=U.Id;
                    Indice.Mes=m;
                    Indice.Valor=val;
                    msg=ibl.Insert(Indice);
                }
            }

            vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);
            limpiaCampos();
            llenagrid();

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ViewState["idMod"] = 0;
            limpiaCampos();
            pnl_Modal.Hide();
        }

        private void limpiaCampos()
        {
            txtEjercicio.Text = "";
            txtEnero.Text = "";
            txtFebrero.Text = "";
            txtMarzo.Text = "";
            txtAbril.Text = "";
            txtMayo.Text = "";
            txtJunio.Text = "";
            txtJulio.Text = "";
            txtAgosto.Text = "";
            txtSeptiembre.Text = "";
            txtOctubre.Text = "";
            txtNoviembre.Text = "";
            txtDiciembre.Text = "";
            ViewState["idMod"] = 0;
        }

        private void habilitaCampos(bool activa)
        {
            txtEjercicio.Enabled = activa;
            txtEjercicio.Enabled = activa;
            txtEnero.Enabled = activa;
            txtFebrero.Enabled = activa;
            txtMarzo.Enabled = activa;
            txtAbril.Enabled = activa;
            txtMayo.Enabled = activa;
            txtJunio.Enabled = activa;
            txtJulio.Enabled = activa;
            txtAgosto.Enabled = activa;
            txtSeptiembre.Enabled = activa;
            txtOctubre.Enabled = activa;
            txtNoviembre.Enabled = activa;
            txtDiciembre.Enabled = activa;
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];

            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                List<cIndicePrecio> lista = new cIndicePrecioBL().GetFilter("Ejercicio", ViewState["idMod"].ToString(), "true", "Mes", "asc");
                Boolean bBand = true;
                MensajesInterfaz resul=MensajesInterfaz.Eliminacion;
                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (cIndicePrecio indice in lista)
                    {
                        indice.Activo = false;
                        indice.IdUsuario = U.Id;
                        indice.FechaModificacion = DateTime.Now;
                        resul = new cIndicePrecioBL().Delete(indice);
                        if ( resul != MensajesInterfaz.Actualizacion)
                        {
                            bBand = false;
                            break;
                        }
                    }
                    if (bBand)
                        scope.Complete();
                }                

                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                List<cIndicePrecio> lista = new cIndicePrecioBL().GetFilter("Ejercicio", ViewState["idMod"].ToString(), "false", "Mes", "asc");
                Boolean bBand = true;
                MensajesInterfaz resul = MensajesInterfaz.Eliminacion;
                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (cIndicePrecio indice in lista)
                    {
                        indice.Activo = true;
                        indice.IdUsuario = U.Id;
                        indice.FechaModificacion = DateTime.Now;
                        resul = new cIndicePrecioBL().Delete(indice);
                        if (resul != MensajesInterfaz.Actualizacion)
                        {
                            bBand = false;
                            break;
                        }
                    }
                    if (bBand)
                        scope.Complete();
                }
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Ingreso) ||
              vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Actualizacion))
            {
                llenagrid();
                limpiaCampos();
            }
        }

        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            string[] filtro = new string[] { ddlFiltro.SelectedValue, txtFiltro.Text, chkInactivo.Checked.ToString() };
            ViewState["filtro"] = filtro;
            llenagrid();
        }

        protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFiltro.Text = "";
            if (ddlFiltro.SelectedValue.ToString() == "")
            {
                txtFiltro.Enabled = false;
            }
            else
            {
                txtFiltro.Enabled = true;
            }
        }

        //protected void configurarReqs(int Meslimit, int eaño)
        protected void configurarReqs(int eje)
        {

            Int32 ejercicio = DateTime.Now.Year;
            Int32 mes = DateTime.Now.Month;
            Boolean[] mesActivo = new Boolean[12];

            for (int x = 0; x < 12; x++)
            {
                if (eje == ejercicio || eje==0)
                {
                    if (x < mes-1)
                    {
                        mesActivo[x] = true;
                    }
                    else
                    {
                        mesActivo[x] = false;
                    }
                }
                else
                {
                    if (eje < ejercicio && eje != 1)
                    {
                        mesActivo[x] = true;
                    }
                    else
                    {
                        mesActivo[x] = false;
                    }
                }
            }
                ReqEnero.Enabled = mesActivo[0];
                ReqFebrero.Enabled = mesActivo[1];
                ReqMarzo.Enabled = mesActivo[2];
                ReqAbril.Enabled = mesActivo[3];
                ReqMayo.Enabled = mesActivo[4];
                ReqJunio.Enabled = mesActivo[5];
                ReqJulio.Enabled = mesActivo[6];
                ReqAgosto.Enabled = mesActivo[7];
                ReqSeptiembre.Enabled = mesActivo[8];
                ReqOctubre.Enabled = mesActivo[9];
                ReqNoviembre.Enabled = mesActivo[10];
                ReqDiciembre.Enabled = mesActivo[11];
               if (eje != 1  )
                {
                    txtEnero.Enabled = mesActivo[0];
                    txtFebrero.Enabled = mesActivo[1];
                    txtMarzo.Enabled = mesActivo[2];
                    txtAbril.Enabled = mesActivo[3];
                    txtMayo.Enabled = mesActivo[4];
                    txtJunio.Enabled = mesActivo[5];
                    txtJulio.Enabled = mesActivo[6];
                    txtAgosto.Enabled = mesActivo[7];
                    txtSeptiembre.Enabled = mesActivo[8];
                    txtOctubre.Enabled = mesActivo[9];
                    txtNoviembre.Enabled = mesActivo[10];
                    txtDiciembre.Enabled = mesActivo[11];
                }               
       
        }
    }
}