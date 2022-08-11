using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Clases;
using Clases.BL;


namespace Catastro.Recibos
{
    public partial class TabularIngresosGral : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ExportExcel.Visible = false;
        }

        protected void imbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            List<GridDetalle> lts = new List<GridDetalle>();
            DateTime fin = Convert.ToDateTime(txtFechaFin.Text + " 23:59:59");
            DateTime inicio = Convert.ToDateTime(txtFechaInicio.Text);

            llenagrid();
        }

        private void llenagrid()
        {
            DateTime fin = Convert.ToDateTime(txtFechaFin.Text + " 23:59:59");
            DateTime inicio = Convert.ToDateTime(txtFechaInicio.Text);
            List<vTabularDetalleConta> listado = new List<vTabularDetalleConta>();
            listado = new vVistasBL().TabularDetalleConta(inicio, fin);
            grdv.DataSource = listado;
            grdv.DataBind();
            grdv.Visible = true;
            if (listado.Count > 0) ExportExcel.Visible = true;
        }


        protected void grdv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ConsultaPago")
            {
            }
        }

        protected void grdv_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void grdv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdv.PageIndex = e.NewPageIndex;
            llenagrid();
        }

        protected void grdv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView grd2 = (GridView)e.Row.FindControl("grd2");

                List<vReciboDetalle> rd = new List<vReciboDetalle>();
                Int32 recibo = int.Parse(e.Row.Cells[0].Text);
                rd = new vVistasBL().ObtieneReciboDetalle(recibo);
                if (rd != null)
                {
                    grd2.DataSource = rd;
                    grd2.DataBind();
                }
               
                e.Row.Cells[9].Text  = Convert.ToDecimal(e.Row.Cells[9].Text).ToString("N2");
                e.Row.Cells[10].Text = Convert.ToDecimal(e.Row.Cells[10].Text).ToString("N2");
                e.Row.Cells[11].Text = Convert.ToDecimal(e.Row.Cells[11].Text).ToString("N2");
            }
        }

        protected void grd2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal importeNeto = 0;
            decimal importeDescuento = 0;
            decimal importePagado = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                importeNeto = Convert.ToDecimal(e.Row.Cells[2].Text);
                importeDescuento = Convert.ToDecimal(e.Row.Cells[3].Text);
                importePagado = Convert.ToDecimal(e.Row.Cells[4].Text);

                e.Row.Cells[2].Text = importeNeto.ToString("N2");
                e.Row.Cells[3].Text = importeDescuento.ToString("N2");
                e.Row.Cells[4].Text = importePagado.ToString("N2");

                Session["importeNeto"] = importeNeto;
                Session["importeDescuento"] = importeDescuento;
                Session["importePagado"] = importePagado;

            }
        }
        public partial class GridDetalle
        {
            public int Recibo { get; set; }
            public int idPredio { get; set; }
            public int idConcepto { get; set; }
            public string ClavePredial { get; set; }
            public string ClaveAnterior { get; set; }
            public string Contribuyente { get; set; }
            public DateTime FechaPago { get; set; }
            public string EstatusRecibo { get; set; }
            public DateTime FechaCancelacion { get; set; }
            public string TipoTramite { get; set; }
            public string clave_descto { get; set; }
            public string descuento { get; set; }
            public decimal ImportePagado { get; set; }
            public decimal ImporteNeto { get; set; }
            public decimal ImporteDescuento { get; set; }
        }

        private void exportar()
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", "Student.xls"));
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter(); ;
            HtmlTextWriter htm = new HtmlTextWriter(sw);
            grdv.AllowPaging = false;
            grdv.RenderControl(htm);
            Response.Write(sw.ToString());
            Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void ExportExcel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.AddHeader("content-disposition", "attachment;  filename=IngresosPorConcepto"+ DateTime.Now.ToString() +".xlsx");
            Response.AddHeader("content-disposition", "attachment;filename=IngresosPorConcepto" + DateTime.Now.ToString() + ".xls");
            Response.Charset = "";
            //Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                grdv.AllowPaging = false;
                llenagrid();

                grdv.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in grdv.HeaderRow.Cells)
                {
                    cell.BackColor = grdv.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grdv.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = grdv.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = grdv.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }
                grdv.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());

                Response.OutputStream.Flush();
                Response.OutputStream.Close();
                Response.Flush();
                Response.Close();
            }
        }     
       
    }
}