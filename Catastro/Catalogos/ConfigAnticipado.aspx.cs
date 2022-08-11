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
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Drawing;

namespace Catastro.Catalogos
{
    public partial class ConfigAnticipado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int anoAct = DateTime.Now.Year;
            string sact = Convert.ToString(anoAct);
            string snue = Convert.ToString(anoAct + 1);

            if (!IsPostBack)
            {
               
                //int ultimoEjercicio =  Convert.ToInt32(new cParametroSistemaBL().GetByClave("ULT_EJERCICIO_BG").Valor);
                //if (anoAct == ultimoEjercicio)
                //{
                    
                    lblTitulo.Text = "Configuración de Cobro del " + sact + " al " + snue + "";
                    lbltextoActualizacion.Text = "Con esta actualización podrás realizar un masivo que replique los catálogos de cobro de " + sact + " al " + snue +
                        " de manera rápida y sencilla. Esta acción debe ser realizado únicamente por la persona autorizada, 1 o 2 días antes del inicio del cobro anticipado. " +
                        " <br />  <hr />";
                //}
                

                validaBotones(snue);
            }
            
            vtnModal.eventoAceptar += new EventHandler(vtnModal_eventoAceptar);
            validaBotones(snue);
        }

        protected void vtnModal_eventoAceptar(object sender, EventArgs e)
        {
            cUsuarios U = (cUsuarios)Session["usuario"];
            
            if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ConfimacionEliminar))
            {
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.ActivarRegistro))
            {
            }
            else if (vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Ingreso) ||
              vtnModal.Mensaje == new Utileria().GetDescription(MensajesInterfaz.Actualizacion))
            {
            }
        }              

        protected void validaBotones(string snue)
        {

            if (executaComando("cBaseGravable", snue) > 0)
            {
                btnRealizarMasivo.Enabled = false;
                btnRealizarMasivo.BackColor = Color.Gray;
            }
            if (executaComando("cConcepto", snue) > 0)
            {
                btnConceptos.Enabled = false;
                btnConceptos.BackColor = Color.Gray;
            }
            if (executaComando("cSalarioMinimo", snue) > 0)
            {
                btnUMA.Enabled = false;
                btnUMA.BackColor = Color.Gray;
            }
            if (executaComando("cBaseImpuesto", snue) > 0)
            {
                btnBaseImpuesto.Enabled = false;
                btnBaseImpuesto.BackColor = Color.Gray;
            }
            if (executaComando("cCuotasPredio", snue) > 0)
            {
                btnCuotasPredio.Enabled = false;
                btnCuotasPredio.BackColor = Color.Gray;
            }
            if (executaComando("cTarifaZona", snue) > 0)
            {
                btnZonas.Enabled = false;
                btnZonas.BackColor = Color.Gray;
            }
            if (executaComando("cTarifaRecoleccion", snue) > 0)
            {
                btnRecoleccion.Enabled = false;
                btnRecoleccion.BackColor = Color.Gray;
            }
            if (executaComando("cTarifaLimpieza", snue) > 0)
            {
                btnLimpieza.Enabled = false;
                btnLimpieza.BackColor = Color.Gray;
            }
            if (executaComando("cTarifaDap", snue) > 0)
            {
                btnDap.Enabled = false;
                btnDap.BackColor = Color.Gray;
            }
            if (executaComando("cTarifaRecargo", snue) > 0)
            {
                btnRecargos.Enabled = false;
                btnRecargos.BackColor = Color.Gray;
            }
        }

        private int executaComando(string tabla, string anio)
        {
            int resul = 0;

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand(); // Create a object of SqlCommand class
            cmd.Connection = con; //Pass the connection object to Command

            cmd.CommandText = "SELECT COUNT(1) FROM " + tabla + " WHERE Ejercicio= " + anio + ";";
            cmd.CommandType = CommandType.Text;
            resul = (int)cmd.ExecuteScalar();
            cmd.Dispose();
            con.Close();
            return resul;
        }

        protected void btnRealizarMasivo_Click(object sender, EventArgs e)
        {
            vtnModal.DysplayCancelar = false;
            validaBotones((DateTime.Now.Year +1).ToString());

            try
            {
                if (executaComando("cBaseGravable", (DateTime.Now.Year + 1).ToString() ) > 0)
                {
                    vtnModal.ShowPopup(new Utileria().GetDescription("Bases gravables del "+ (DateTime.Now.Year + 1).ToString()+ " generadas"), Catastro.Controles.ModalPopupMensaje.TypeMesssage.Alert);
                    return;
                }

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand(); // Create a object of SqlCommand class
                cmd.Connection = con; //Pass the connection object to Command

                cmd.CommandText = "INSERT INTO [dbo].[cBaseGravable] " +
                                  " ([IdPredio],[Ejercicio],[Bimestre],[Valor],[FechaAvaluo],[SuperficieTerreno],[TerrenoPrivativo] " +
                                  " ,[TerrenoComun],[ValorTerreno],[SuperficieConstruccion],[ConstruccionPrivativa],[ConstruccionComun] " +
                                  " ,[ValorConstruccion],[Activo],[IdUsuario],[FechaModificacion]) " +
                                  "  ( select b.[IdPredio], year(getdate())+ 1 as ejercicio, b.[Bimestre], b.[Valor], cast((year(getdate())+1)as varchar)+'-01-01' as FechaAvaluo, b.[SuperficieTerreno], b.[TerrenoPrivativo] " +
                                  " , b.[TerrenoComun], b.[ValorTerreno], b.[SuperficieConstruccion], b.[ConstruccionPrivativa], b.[ConstruccionComun] " +
                                  " , b.[ValorConstruccion], b.[Activo], b.[IdUsuario], b.[FechaModificacion] " +
                                  "  from cBaseGravable b " +
                                  "  join cPredio on cpredio.Id = b.IdPredio and IdStatusPredio != 2 " +
                                  "  where b.Ejercicio = year(getdate()) and b.Activo= 1)";
                cmd.CommandType = CommandType.Text;


                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();

                pnl_Modal.Hide();
            }
            catch (Exception ex)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("Problemas para actualizar las Bases Gravables"), Catastro.Controles.ModalPopupMensaje.TypeMesssage.Alert);
                new Utileria().logError("Actualización de Bases Gravables", ex);
                return;
            }
        }

        protected void btnConceptos_Click(object sender, EventArgs e)
        {
            vtnModal.DysplayCancelar = false;
            if (executaComando("cConcepto", (DateTime.Now.Year + 1).ToString()) > 0)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("Conceptos de cobro del " + (DateTime.Now.Year + 1).ToString() + " generados"), Catastro.Controles.ModalPopupMensaje.TypeMesssage.Alert);
                return;
            }
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand(); // Create a object of SqlCommand class
                cmd.Connection = con; //Pass the connection object to Command

                cmd.CommandText = " INSERT INTO cConcepto (IdTipoTramite, IdMesa,IdUnidadMedida,IdProdServ, Ejercicio, Cri, Nombre, Descripcion, " +
                                  "  SalarioMin, SalarioMax, Importe, TipoCobro, Adicional, EsAdicional, SinDescuento, Activo,   " +
                                  "  IdUsuario, FechaModificacion, IdGrupo)  " +
                                  "  select IdTipoTramite, IdMesa,IdUnidadMedida,IdProdServ, year(getdate()) + 1, Cri, Nombre, Descripcion, SalarioMin, SalarioMax,   " +
                                  "  Importe, TipoCobro, Adicional, EsAdicional, SinDescuento, Activo, 1, getdate(), IdGrupo  " +
                                  "  from cConcepto where ejercicio = year(getdate()) and activo = 1; ";

                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();

                pnl_Modal.Hide();
            }
            catch (Exception ex)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("Problemas para actualizar los Conceptos"), Catastro.Controles.ModalPopupMensaje.TypeMesssage.Alert);
                new Utileria().logError("Actualización de Conceptos", ex);
                return;
            }
        }

        protected void btnUMA_Click(object sender, EventArgs e)
        {
            vtnModal.DysplayCancelar = false;
            if (executaComando("cSalarioMinimo", (DateTime.Now.Year + 1).ToString()) > 0)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("UMA de cobro de " + (DateTime.Now.Year + 1).ToString() + " generados"), Catastro.Controles.ModalPopupMensaje.TypeMesssage.Alert);
                return;
            }
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand(); // Create a object of SqlCommand class
                cmd.Connection = con; //Pass the connection object to Command

                cmd.CommandText = " INSERT INTO [dbo].[cSalarioMinimo] " +
                                  " ([Ejercicio],[Importe],[Descripcion],[Activo],[IdUsuario],[FechaModificacion]) " +
                                  " (select year(getdate()) +1,[Importe],[Descripcion],[Activo],[IdUsuario],[FechaModificacion] " +
                                  " from[dbo].[cSalarioMinimo] " +
                                  " where ejercicio = year(getdate()) and activo = 1);";

                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();

                pnl_Modal.Hide();
            }
            catch (Exception ex)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("Problemas para actualizar los cSalarioMinimo"), Catastro.Controles.ModalPopupMensaje.TypeMesssage.Alert);
                new Utileria().logError("Actualización de cSalarioMinimo", ex);
                return;
            }

        }

        protected void btnBaseImpuesto_Click(object sender, EventArgs e)
        {
            vtnModal.DysplayCancelar = false;
            if (executaComando("cBaseImpuesto", (DateTime.Now.Year + 1).ToString()) > 0)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("Base de Impuesto de " + (DateTime.Now.Year + 1).ToString() + " generados"), Catastro.Controles.ModalPopupMensaje.TypeMesssage.Alert);
                return;
            }
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand(); // Create a object of SqlCommand class
                cmd.Connection = con; //Pass the connection object to Command

                cmd.CommandText = "INSERT INTO  [dbo].[cBaseImpuesto] "+
                                   " (Ejercicio,[Valor],[Activo],[IdUsuario],[FechaModificacion]) " +
                                   " (select year(getdate()) + 1,[Valor],[Activo],[IdUsuario],[FechaModificacion] " +
                                   " from[dbo].[cBaseImpuesto] where[Ejercicio] = year(getdate()) and activo = 1); ";

                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();

                pnl_Modal.Hide();
            }
            catch (Exception ex)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("Problemas para actualizar los cBaseImpuesto"), Catastro.Controles.ModalPopupMensaje.TypeMesssage.Alert);
                new Utileria().logError("Actualización de cBaseImpuesto", ex);
                return;
            }

        }

        protected void btnCuotasPredio_Click(object sender, EventArgs e)
        {
            vtnModal.DysplayCancelar = false;
            if (executaComando("cCuotasPredio", (DateTime.Now.Year + 1).ToString()) > 0)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("Cuotas por Tipo de Predio de " + (DateTime.Now.Year + 1).ToString() + " generados"), Catastro.Controles.ModalPopupMensaje.TypeMesssage.Alert);
                return;
            }
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand(); // Create a object of SqlCommand class
                cmd.Connection = con; //Pass the connection object to Command

                cmd.CommandText = "INSERT INTO [dbo].[cCuotasPredio] " +
                                  " ([Ejercicio],[IdTipoPredio],[CuotasCobro],[IdTipoCobro],[Activo],[IdUsuario],[FechaModificacion]) " +
                                  " (select year(getdate())+1,[IdTipoPredio],[CuotasCobro],[IdTipoCobro],[Activo],[IdUsuario],[FechaModificacion] " +
                                  " from[dbo].[cCuotasPredio] " +
                                  " where[Ejercicio] = year(getdate()) and activo = 1); ";

                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();

                pnl_Modal.Hide();
            }
            catch (Exception ex)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("Problemas para actualizar los cCuotasPredio"), Catastro.Controles.ModalPopupMensaje.TypeMesssage.Alert);
                new Utileria().logError("Actualización de cCuotasPredio", ex);
                return;
            }

        }

        protected void btnZonas_Click(object sender, EventArgs e)
        {
            vtnModal.DysplayCancelar = false;
            if (executaComando("cTarifaZona", (DateTime.Now.Year + 1).ToString()) > 0)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(" Tarifas de Zonas de" + (DateTime.Now.Year + 1).ToString() + " generados"), Catastro.Controles.ModalPopupMensaje.TypeMesssage.Alert);
                return;
            }
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand(); // Create a object of SqlCommand class
                cmd.Connection = con; //Pass the connection object to Command

                cmd.CommandText = "INSERT INTO [dbo].[cTarifaZona] " +
                                  " ([Zona] ,[Ejercicio] ,[Tarifa],[IdTipoCobro],[Activo],[IdUsuario],[FechaModificacion]) "+
                                  " ( select[Zona] ,year(getdate()) +1 ,[Tarifa],[IdTipoCobro],[Activo],[IdUsuario],[FechaModificacion]  " +
                                  "  from[dbo].[cTarifaZona]  " +
                                  " where[Ejercicio] = year(getdate()) and activo = 1); ";

                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();

                pnl_Modal.Hide();
            }
            catch (Exception ex)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("Problemas para actualizar los cTarifaZona"), Catastro.Controles.ModalPopupMensaje.TypeMesssage.Alert);
                new Utileria().logError("Actualización de cTarifaZona", ex);
                return;
            }
        }

        protected void btnRecoleccion_Click(object sender, EventArgs e)
        {
            vtnModal.DysplayCancelar = false;
            if (executaComando("cTarifaRecoleccion", (DateTime.Now.Year + 1).ToString()) > 0)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(" Tarifas de Recolección de" + (DateTime.Now.Year + 1).ToString() + " generados"), Catastro.Controles.ModalPopupMensaje.TypeMesssage.Alert);
                return;
            }
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand(); // Create a object of SqlCommand class
                cmd.Connection = con; //Pass the connection object to Command

                cmd.CommandText = "INSERT INTO [dbo].[cTarifaRecoleccion] " +
                                  " ([Ejercicio],[Tarifa],[IdTipoCobro],[Activo],[IdUsuario],[FechaModificacion]) " +
		                          " ( select year(getdate())+1,[Tarifa],[IdTipoCobro],[Activo],[IdUsuario],[FechaModificacion] " +
                                  "  from[dbo].[cTarifaRecoleccion]  " +
                                  "  where Ejercicio = year(getdate()) and activo = 1);  ";

                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();

                pnl_Modal.Hide();
            }
            catch (Exception ex)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("Problemas para actualizar los cTarifaRecoleccion"), Catastro.Controles.ModalPopupMensaje.TypeMesssage.Alert);
                new Utileria().logError("Actualización de cTarifaRecoleccion", ex);
                return;
            }
        }

        protected void btnLimpieza_Click(object sender, EventArgs e)
        {
            vtnModal.DysplayCancelar = false;
            if (executaComando("cTarifaLimpieza", (DateTime.Now.Year + 1).ToString()) > 0)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(" Tarifas de Limpieza de" + (DateTime.Now.Year + 1).ToString() + " generados"), Catastro.Controles.ModalPopupMensaje.TypeMesssage.Alert);
                return;
            }
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand(); // Create a object of SqlCommand class
                cmd.Connection = con; //Pass the connection object to Command

                cmd.CommandText = "INSERT INTO [dbo].[cTarifaLimpieza] " +
                                  " ([Ejercicio],[Tarifa],[TipoCobro],[Activo],[IdUsuario] ,[FechaModificacion]) " +                                
                                  "  ( select year(getdate())+1,[Tarifa],[TipoCobro],[Activo],[IdUsuario] ,[FechaModificacion] " + 
                                  "  from[dbo].[cTarifaLimpieza] " +
                                  "  where Ejercicio = year(getdate()) and activo = 1) ";

                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();

                pnl_Modal.Hide();
            }
            catch (Exception ex)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("Problemas para actualizar los cTarifaLimpieza"), Catastro.Controles.ModalPopupMensaje.TypeMesssage.Alert);
                new Utileria().logError("Actualización de cTarifaLimpieza", ex);
                return;
            }
        }

        protected void btnDap_Click(object sender, EventArgs e)
        {
            vtnModal.DysplayCancelar = false;
            if (executaComando("cTarifaDap", (DateTime.Now.Year + 1).ToString()) > 0)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(" Tarifas de DAP de" + (DateTime.Now.Year + 1).ToString() + " generados"), Catastro.Controles.ModalPopupMensaje.TypeMesssage.Alert);
                return;
            }
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand(); // Create a object of SqlCommand class
                cmd.Connection = con; //Pass the connection object to Command

                cmd.CommandText = " INSERT INTO [cTarifaDap] "+
                                  " ([Ejercicio],[Tarifa],[TipoCobro],[Activo],[IdUsuario],[FechaModificacion]) " +
                                  "( select year(getdate()) +1,[Tarifa],[TipoCobro],[Activo],[IdUsuario],[FechaModificacion] " +
                                   " from [cTarifaDap] " +
                                   " where Ejercicio = year(getdate()) and activo = 1) ";

                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();

                pnl_Modal.Hide();
            }
            catch (Exception ex)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("Problemas para actualizar los cTarifaDap"), Catastro.Controles.ModalPopupMensaje.TypeMesssage.Alert);
                new Utileria().logError("Actualización de cTarifaDap", ex);
                return;
            }
        }

        protected void btnRecargos_Click(object sender, EventArgs e)
        {
            vtnModal.DysplayCancelar = false;
            if (executaComando("cTarifaRecargo", (DateTime.Now.Year + 1).ToString()) > 0)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription(" Tarifas de Recargos de" + (DateTime.Now.Year + 1).ToString() + " generados"), Catastro.Controles.ModalPopupMensaje.TypeMesssage.Alert);
                return;
            }
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand(); // Create a object of SqlCommand class
                cmd.Connection = con; //Pass the connection object to Command

                cmd.CommandText = " INSERT INTO [dbo].[cTarifaRecargo] "+
                                  " ([Ejercicio],[Bimestre],[Porcentaje],[Activo],[IdUsuario],[FechaModificacion])	 " +
                                  " (select year(getdate())+1,[Bimestre],0,[Activo],[IdUsuario],[FechaModificacion] " +
                                  " from[cTarifaRecargo] where Ejercicio = year(getdate()) and activo = 1)";

                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();

                pnl_Modal.Hide();
            }
            catch (Exception ex)
            {
                vtnModal.ShowPopup(new Utileria().GetDescription("Problemas para actualizar los cTarifaRecargo"), Catastro.Controles.ModalPopupMensaje.TypeMesssage.Alert);
                new Utileria().logError("Actualización de cTarifaRecargo", ex);
                return;
            }
        }
    }
}