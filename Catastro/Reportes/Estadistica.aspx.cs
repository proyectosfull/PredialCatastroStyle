using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using Clases.BL;

namespace Catastro.Reportes
{
    public partial class Estadistica : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CalculaEstadistica()
        {
            List<vPadronPredio> listado = new List<vPadronPredio>();

            //listado = new vVistasBL().ObtienePadron();//int.Parse(ddlStatus.SelectedValue), int.Parse(ddlAnio.SelectedValue), int.Parse(ddlBimestre.SelectedValue), int.Parse(ddlTipo.SelectedValue), clv, RemoveSpecialCharacters(txtClave.Text), txtContribuyente.Text.Trim(), hdfIdCondominio.Value, txtColonia.Text, txtInicioClave.Text, txtFinClave.Text);
        }
    }
}