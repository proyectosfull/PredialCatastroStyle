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


namespace Catastro.Catalogos
{
    public partial class catRolPermisos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["sortCampo"] = "Rol";
                ViewState["sortOnden"] = "asc";
                llenaFiltro();
                llenaListas();
                llenagrid();
                txtFiltro.Enabled = false;
            }
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
        }
        private void llenaFiltro()
        {
            ddlFiltro.DataSource = new cVentanaBL().ListaCampos();
            ddlFiltro.DataBind();
            ddlFiltro.Items.Insert(0, new ListItem("Todos", ""));
        }
        private void llenaListas()
        {
            ddlRol.DataValueField = "Id";
            ddlRol.DataTextField = "Rol";
            ddlRol.DataSource = new cRolBL().GetAll();
            ddlRol.DataBind();
            ddlRol.Items.Insert(0, new ListItem("Seleccionar Rol", "%"));

            List<cVentana> ventanas = new cVentanaBL().GetAll();
            List<cVentana> ventanasPadre = ventanas.Where(vp => vp.IdPapa == 0).ToList();

            List<cBoton> botones = new cBotonBL().GetAll();
            foreach (cVentana v in ventanasPadre)
            {
                TreeNode nodeVentanaPadre = new TreeNode(v.Ventana, v.Id.ToString());
                nodeVentanaPadre.ToolTip = v.Descripcion;
                nodeVentanaPadre.ShowCheckBox = false;
                trVentanas.Nodes.Add(nodeVentanaPadre);
                List<cVentana> vhijos = ventanas.Where(vp => vp.IdPapa == v.Id).ToList();
                foreach (cVentana vh in vhijos)
                {
                    TreeNode nodeVentanaHijo = new TreeNode(vh.Ventana, vh.Id.ToString());
                    nodeVentanaHijo.ToolTip = v.Descripcion;
                    nodeVentanaHijo.ShowCheckBox = false;
                    nodeVentanaPadre.ChildNodes.Add(nodeVentanaHijo);
                    List<cBoton> botonesVentana = botones.Where(b => b.IdVentana == vh.Id).ToList();
                    foreach (cBoton b in botonesVentana)
                    {
                        TreeNode nodeBoton = new TreeNode(b.Boton, b.Id.ToString());
                        nodeBoton.ToolTip = v.Descripcion;
                        nodeVentanaHijo.ChildNodes.Add(nodeBoton);
                    }
                }

            }



        }

        #region Grid
        private void llenagrid()
        {
            string[] filtro = (string[])ViewState["filtro"];

            if (filtro == null)
            {
                grd.DataSource = new cRolBL().GetFilter("", "", "TRUE", ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
                grd.DataBind();
            }
            else
            {
                grd.DataSource = new cRolBL().GetFilter(filtro[0], filtro[1], filtro[2], ViewState["sortCampo"].ToString(), ViewState["sortOnden"].ToString());
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
                lbl_titulo.Text = "Consulta Permisos";
                pnl_Modal.Show();
                int id = Convert.ToInt32(e.CommandArgument);
                cRol rol = new cRolBL().GetByConstraint(id);
                limpiaCampos();
                ddlRol.SelectedValue = rol.Id.ToString();
                List<mRolBot> botones = new mRolBotBL().GetAllByIdRoll(id);
                RecorrerTreeViewActiva(trVentanas.Nodes, ref botones);
                habilitaCampos(false);


                btn_Guardar.Visible = false;
            }
            else if (e.CommandName == "ModificarRegistro")
            {
                lbl_titulo.Text = "Modifica Permisos";
                int id = Convert.ToInt32(e.CommandArgument);
                cRol rol = new cRolBL().GetByConstraint(id);
                limpiaCampos();
                ddlRol.SelectedValue = rol.Id.ToString();
                List<mRolBot> botones = new mRolBotBL().GetAllByIdRoll(id);
                RecorrerTreeViewActiva(trVentanas.Nodes, ref botones);
                habilitaCampos(true);
                btn_Guardar.Visible = true;
                pnl_Modal.Show();
            }
            //else if (e.CommandName == "EliminarRegistro")
            //{
            //    int id = Convert.ToInt32(e.CommandArgument);
            //    ViewState["idMod"] = id;
            //    vtnModal.DysplayCancelar = true;
            //    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar), ModalPopupMensaje.TypeMesssage.Informacion);
            //}
            //else if (e.CommandName == "ActivarRegistro")
            //{
            //    int id = Convert.ToInt32(e.CommandArgument);
            //    ViewState["idMod"] = id;
            //    vtnModal.DysplayCancelar = true;
            //    vtnModal.ShowPopup(new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro), ModalPopupMensaje.TypeMesssage.Informacion);
            //}

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
        //protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        string activo = grd.DataKeys[e.Row.RowIndex].Values[1].ToString();

        //        if (activo.ToUpper() == "TRUE")
        //        {
        //            ImageButton imgActivar = (ImageButton)e.Row.FindControl("imgActivar");
        //            imgActivar.Visible = false;
        //        }
        //        else
        //        {
        //            ImageButton imgConsulta = (ImageButton)e.Row.FindControl("imgConsulta");
        //            imgConsulta.Visible = false;
        //            ImageButton imgUpdate = (ImageButton)e.Row.FindControl("imgUpdate");
        //            imgUpdate.Visible = false;
        //            ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
        //            imgDelete.Visible = false;
        //        }
        //    }

        //}
        #endregion

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            lbl_titulo.Text = "Alta Permiso";
            pnl_Modal.Show();
            limpiaCampos();
            habilitaCampos(true);
            btn_Guardar.Visible = true;

        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            
            if (ddlRol.SelectedIndex != 0)
            {
                cUsuarios U = (cUsuarios)Session["usuario"];
                pnl_Modal.Hide();
                vtnModal.DysplayCancelar = false;
                List<mRolBot> botones = new List<mRolBot>();
                DateTime fecha = DateTime.Now;
                RecorrerTreeViewGuardar(trVentanas.Nodes, ref botones, U.Id, fecha);
                MensajesInterfaz msg = new mRolBotBL().InsertLista(botones, Convert.ToInt32(ddlRol.SelectedValue), U.Id, fecha);
                vtnModal.ShowPopup(new Utileria().GetDescription(msg), ModalPopupMensaje.TypeMesssage.Alert);

                limpiaCampos();
                llenagrid();
            }

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ViewState["idMod"] = 0;
            limpiaCampos();
            pnl_Modal.Hide();
        }
        private void limpiaCampos()
        {
            ddlRol.SelectedIndex = 0;
            RecorrerTreeViewLimpiar(trVentanas.Nodes);
        }
        private void habilitaCampos(bool activa)
        {
            ddlRol.Enabled = activa;
            trVentanas.Enabled = activa;
        }
        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
                cVentana Ventana = new cVentanaBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                Ventana.Activo = false;
                Ventana.IdUsuario = U.Id;
                Ventana.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cVentanaBL().Delete(Ventana);
                vtnModal.DysplayCancelar = false;
                vtnModal.ShowPopup(new Utileria().GetDescription(resul), ModalPopupMensaje.TypeMesssage.Alert);
                ViewState["idMod"] = 0;
                llenagrid();
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
                cVentana Ventana = new cVentanaBL().GetByConstraint(Convert.ToInt32(ViewState["idMod"]));
                Ventana.Activo = true;
                Ventana.IdUsuario = U.Id;
                Ventana.FechaModificacion = DateTime.Now;
                MensajesInterfaz resul = new cVentanaBL().Update(Ventana);
                vtnModal.DysplayCancelar = false;
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
            string[] filtro = new string[] { ddlFiltro.SelectedValue, txtFiltro.Text, "true" };
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

        protected void ddlRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRol.SelectedIndex != 0)
            {
                List<mRolBot> botones = new mRolBotBL().GetAllByIdRoll(Convert.ToInt32(ddlRol.SelectedValue));
                RecorrerTreeViewLimpiar(trVentanas.Nodes);
                RecorrerTreeViewActiva(trVentanas.Nodes, ref botones);

            }
            pnl_Modal.Show();
        }


        private void RecorrerTreeViewActiva(TreeNodeCollection Nodos, ref  List<mRolBot> botones)
        {
            foreach (TreeNode Nodo in Nodos)
            {
                if (Nodo.ChildNodes.Count == 0)
                {
                    mRolBot boton = botones.FirstOrDefault(b => b.IdBoton == Convert.ToInt32(Nodo.Value));
                    if (boton != null)
                        Nodo.Checked = true;
                    //Nodo.Text = "Hijo";
                }
                else
                {
                    //Nodo.Text = "Padre";
                    RecorrerTreeViewActiva(Nodo.ChildNodes, ref botones);
                }
            }
        }

        private void RecorrerTreeViewLimpiar(TreeNodeCollection Nodos)
        {
            foreach (TreeNode Nodo in Nodos)
            {
                if (Nodo.ChildNodes.Count == 0)
                {
                    Nodo.Checked = false;
                    //Nodo.Text = "Hijo";
                }
                else
                {
                    //Nodo.Text = "Padre";
                    RecorrerTreeViewLimpiar(Nodo.ChildNodes);
                }
            }
        }

        private void RecorrerTreeViewGuardar(TreeNodeCollection Nodos, ref List<mRolBot> botones, int idUsuario, DateTime fecha)
        {
            foreach (TreeNode Nodo in Nodos)
            {
                if (Nodo.ChildNodes.Count == 0)
                {
                    if (Nodo.Checked)
                    {
                        mRolBot boton = new mRolBot();
                        boton.IdBoton = Convert.ToInt32(Nodo.Value);
                        boton.IdRol = Convert.ToInt32(ddlRol.SelectedValue);
                        boton.Activo = true;
                        boton.IdUsuario = idUsuario;
                        boton.FechaModificacion = fecha;
                        botones.Add(boton);
                    }
                    //Nodo.Text = "Hijo";
                }
                else
                {
                    //Nodo.Text = "Padre";
                    RecorrerTreeViewGuardar(Nodo.ChildNodes, ref botones, idUsuario, fecha);
                }
            }
        }

    }
}