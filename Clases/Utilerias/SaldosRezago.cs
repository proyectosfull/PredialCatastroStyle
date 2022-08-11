using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;

namespace Clases.Utilerias
{
    public class SaldosRezago
    {   
#pragma warning disable CS0649 // El campo 'SaldosRezago.dt' nunca se asigna y siempre tendrá el valor predeterminado null
        DataTable dt ;
#pragma warning restore CS0649 // El campo 'SaldosRezago.dt' nunca se asigna y siempre tendrá el valor predeterminado null

        
        //public  RezagoInicio(List<cPredio> lPredio, int ejFinal, int bimFinal, DataTable dt)
        //{
        //    this.lPredio = lPredio;
        //    this.ejFinal = ejFinal;
        //    this.bimFinal = bimFinal;
        //    this.dt = dt;

        //}
        public void GeneraRezago(List<cPredio> lPredio, int ejFinal, int bimFinal)
        {
            Impuesto i = new Impuesto();
            int j = 1;

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

            foreach (cPredio p in lPredio)
            {
                i = new SaldosC().InicializaIP(i);

                //lblCalculando.Text = " Calculando... " + lPredio.Count() + " / " + j.ToString();
                if ((p.AaFinalIp * 10) + p.BimestreFinIp <= (ejFinal * 10) + bimFinal)
                    i = new SaldosC().CalculaCobro(p.Id, "NO", p.BimestreFinIp, p.AaFinalIp, bimFinal, ejFinal, 0, 0, "rptRezago");

                #region llena datatable
                if (i.TextError == null || i.TextError == "")
                {
                    DataRow dr = dt.NewRow();
                    dr["ClavePredial"] = p.ClavePredial;
                    dr["Propietario"] = p.cContribuyente.ApellidoPaterno + " " + p.cContribuyente.ApellidoMaterno + " " + p.cContribuyente.Nombre;
                    dr["Domicilio"] = p.Calle + " NÚM. " + p.Numero + " COL. " + p.cColonia.NombreColonia + " C.P. " + p.CP + " LOCALIDAD " + p.Localidad; ;
                    dr["Condominio"] = p.cCondominio.Descripcion;
                    dr["Periodo"] = i.Estado.PeriodoGral;
                    dr["SuperTerreno"] = p.SuperficieTerreno;
                    dr["SuperConstruccion"] = p.SuperficieConstruccion;
                    dr["ValorTerreno"] = p.ValorTerreno;
                    dr["ValorConstruccion"] = p.ValorConstruccion;
                    dr["ImpuestoAnt"] = i.Estado.AntImpuesto > 0 ? i.Estado.AntImpuesto : 0;
                    dr["AdicionalAnt"] = i.Estado.AntAdicional > 0 ? i.Estado.AntAdicional : 0;
                    dr["Impuesto"] = i.Estado.Impuesto > 0 ? i.Estado.Impuesto : 0;
                    dr["Adicional"] = i.Estado.Adicional > 0 ? i.Estado.Adicional : 0;
                    dr["Diferencias"] = i.Estado.Diferencias;
                    dr["RecDiferencias"] = i.Estado.RecDiferencias;
                    dr["Rezagos"] = i.Estado.Rezagos;
                    dr["Recargos"] = i.Estado.Recargos;
                    dr["Honorarios"] = i.Estado.Honorarios;
                    dr["Ejecucion"] = i.Estado.Ejecucion > 0 ? i.Estado.Ejecucion : 0;
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
                    dr["AdicionalAnt"] = 0;
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
                    #endregion

                    j++;
                }              
               
            }           
        }


        public DataTable RezagoResultado()
        {
            return dt;
        }
       


        //public List<List<cPredio>> cPredioAgrupado(List<cPredio> lpredio)
        //{
        //    List<List<cPredio>> llp = new List<List<cPredio>>();
        //    List<cPredio> p = new List<cPredio>();
        //    int i = 0;
        //    int j = 1000;
        //    foreach (cPredio item in lpredio)
        //    {
        //        if (i == lpredio.Count()-1)
        //        {
        //            j = i;
        //        }
        //        p.Add(item);
        //        if (i == j)
        //        {
        //            llp.Add(p);
        //            p = new List<cPredio>();
        //            j = j + 1000;                    
        //        }             
        //        i++;
        //    }
        //    return llp;
        //}

    }//public class  SaldosRezago
}
