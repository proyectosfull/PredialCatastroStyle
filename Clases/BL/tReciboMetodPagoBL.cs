using Clases.Utilerias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases.BL
{
    public class tReciboMetodoPagoBL
    {
        PredialEntities Predial;
        /// <summary>
        /// 
        /// </summary>
        public tReciboMetodoPagoBL()
        {
            Predial = new PredialEntities();
        }

        public MensajesInterfaz Insert(tReciboMetodoPago obj)
        {
            MensajesInterfaz Insert;
            try
            {
                Predial.tReciboMetodoPago.Add(obj);
                Predial.SaveChanges();
                Insert = MensajesInterfaz.Ingreso;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("tReciboMetodoPago.Insert.DbUpdateException", ex);
                Insert = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("tReciboMetodoPago.Insert.DataException", ex);
                Insert = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tReciboMetodoPago.Insert.Exception", ex);
                Insert = MensajesInterfaz.ErrorGeneral;
            }
            return Insert;
        }



    }
}
