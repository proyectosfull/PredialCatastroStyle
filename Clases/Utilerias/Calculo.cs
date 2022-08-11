using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clases.BL;
using Clases;
using Clases.Utilerias;

namespace Clases.Utilerias
{
    public class Calculo
    {

        public Dictionary<string, decimal> dCobro = new Dictionary<string, decimal>();

        //obtiene parametros de cobro cParametrosCobro (Clave)
        //obtiene datos del predio cPredio (ClaveCatastral)
        //obtiene descuentos
        //  Cdescuento Global  // descuento general
        //  descuento  x clave
        //obtiene periodo de pago
        //obtiene base del impuesto  GetByPeriodo(int EjercicioInicial, int EjercicioFinal)
        //obtiene bases gravables GetByPeriodo(int EjercicioInicial, int EjercicioFinal)
        //obtiene salario minimo getbyEjercicio //getByperiodo(ini,fin)
        //Ontiene tarifa recargos GetByPeriodo(int EjercicioInicial, int EjercicioFinal)

        //sacar el bimestre actual y el año actual para comparar
        //DateTime hoy;
        //hoy = DateTime.Today;
        //li_bim_actual = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(hoy.Month / 2)));
        //if (Convert.ToInt32(ls_periodo_pag) < Convert.ToInt32(hoy.Year.ToString() + li_bim_actual.ToString()))
        //{
        //    estatus = "R";
        //}
        public int CuentaBimestre (int bimestreInicial, int ejercicioInicial, int bimestreFinal, int ejercicioFinal)
        {
            int bimestres = 0;
            Boolean flag = true;

            if (bimestreInicial == bimestreFinal && ejercicioInicial == ejercicioFinal)
            {
                bimestres = 0;
                return 0;
            }

            if (ejercicioInicial > ejercicioFinal && bimestreInicial > bimestreFinal)
            {
                bimestres = 0;
                return 0;
            }

            bimestres++;
            do
            {
                if (bimestreInicial == bimestreFinal && ejercicioInicial == ejercicioFinal)
                    break;
                else
                {
                    bimestreInicial++;

                    if (bimestreInicial == 7)
                    {
                        bimestreInicial = 1;
                        ejercicioInicial++;
                    }
                    bimestres++;
                }

            } while (flag == true);//(bimestreInicial <= bimestreFinal && ejercicioInicial <= ejercicioFinal); 

            return bimestres;        
        }

       
        public decimal ImpuestoPorBimestre(decimal baseGravable, decimal baseImpuesto, decimal SM, int tipoPredio,int ejercicio)
        {
            decimal impuestoAnual = 0;
            decimal smAnualizado = 0;
            object par = new cParametroCobroBL().GetDiccionaryValor();
           
            cCuotasPredio c = new cCuotasPredioBL().GetByTipoPredio(ejercicio, tipoPredio);
            smAnualizado = (dCobro["SM_ANUALIZADO"] * SM) * c.CuotasCobro;
                            
            switch (tipoPredio)
            {
                case 1:
                case 3:
                case 4:
                case 5:
                   if (baseGravable > baseImpuesto)
                   {
                       impuestoAnual = (baseImpuesto * dCobro["PORC_BASE_IMPUESTO"]) + ((baseGravable - baseImpuesto) * dCobro["PORC_EXC_IMPUESTO"]);                           
                   }                                   
                   if ( impuestoAnual < smAnualizado  ) impuestoAnual = smAnualizado;                                        
                   
                   break;
                case 2: //rustico
                   impuestoAnual = smAnualizado;
                //    ad_imp70 = ld_ImpuestoAA;
                //    ad_impExc = 0;
                    break;      
            }
            return impuestoAnual / 6 ;
        }


        

    }//End class IP
} //End  Catastro.Proceso