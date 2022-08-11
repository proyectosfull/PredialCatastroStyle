using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Core;
using Clases;
using System.Data;
using Clases.Utilerias;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;

namespace Clases.BL
{
    public class tConvenioBL
    {
        PredialEntities Predial;
        /// <summary>
        /// 
        /// </summary>
        public tConvenioBL()
        {
            Predial = new PredialEntities();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Insert(tConvenio obj)
        {
            MensajesInterfaz Insert;
            try
            {
                Predial.tConvenio.Add(obj);
                Predial.SaveChanges();
                Insert = MensajesInterfaz.Ingreso;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("tConvenioBL.Insert.DbUpdateException", ex);
                Insert = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("tConvenioBL.Insert.DataException", ex);
                Insert = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConvenioBL.Insert.Exception", ex);
                Insert = MensajesInterfaz.ErrorGeneral;
            }
            return Insert;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Update(tConvenio obj)
        {
            MensajesInterfaz Update;
            try
            {
                tConvenio objOld = Predial.tConvenio.FirstOrDefault(c => c.Id == obj.Id);
                Utilerias.Utileria.Compare(obj, objOld);
                objOld.IdConvenioEdoCta = obj.IdConvenioEdoCta;
                objOld.Status = obj.Status;
                objOld.NoParcialidades = obj.NoParcialidades;
                objOld.PorcentajeConvenio = obj.PorcentajeConvenio;
                objOld.ImporteTotal = obj.ImporteTotal;
                objOld.FechaIni = obj.FechaIni;
                objOld.FechaFin = obj.FechaFin;
                objOld.Email = obj.Email;
                objOld.Telefono = obj.Telefono;
                objOld.Celular = obj.Celular;
                objOld.NoIdentificacion = obj.NoIdentificacion;
                objOld.Activo = obj.Activo;
                objOld.IdUsuario = obj.IdUsuario;
                objOld.FechaModificacion = obj.FechaModificacion;
                Predial.SaveChanges();
                Update = MensajesInterfaz.Actualizacion;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("tConvenioBL.Update.DbUpdateException", ex);
                Update = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("tConvenioBL.Update.DataException", ex);
                Update = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConvenioBL.Update.Exception", ex);
                Update = MensajesInterfaz.ErrorGeneral;
            }
            return Update;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tConvenio GetByConstraint(int id)
        {
            tConvenio obj = null;
            try
            {
                obj = Predial.tConvenio.FirstOrDefault(o => o.Id == id);
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConvenioBL.GetByConstraint.Exception", ex , "--Parámetros id:" + id);
            }
            return obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MensajesInterfaz Delete(tConvenio obj)
        {
            MensajesInterfaz Delete;
            try
            {
                tConvenio objOld = Predial.tConvenio.FirstOrDefault(c => c.Id == obj.Id);
                objOld.Activo = obj.Activo;
                objOld.IdUsuario = obj.IdUsuario;
                objOld.FechaModificacion = obj.FechaModificacion;
                Predial.SaveChanges();
                Delete = MensajesInterfaz.Actualizacion;
            }
            catch (DbUpdateException ex)
            {
                new Utileria().logError("tConvenioBL.Delete.DbUpdateException", ex);
                Delete = MensajesInterfaz.ErrorGuardar;
            }
            catch (DataException ex)
            {
                new Utileria().logError("tConvenioBL.Delete.DataException", ex);
                Delete = MensajesInterfaz.ErrorDB;
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConvenioBL.Delete.Exception", ex);
                Delete = MensajesInterfaz.ErrorGeneral;
            }
            return Delete;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public List<tConvenio> GetFilter(string campoFiltro, string valorFiltro, string activos, string campoSort, string tipoSort)
        {
            List<tConvenio> objList = null;
            try
            {
                if (campoFiltro == string.Empty)
                {
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.tConvenio.SqlQuery("Select Id,IdConvenioEdoCta,Status,NoParcialidades,PorcentajeConvenio,ImporteTotal,FechaIni,FechaFin,Activo,IdUsuario,FechaModificacion,folio, ConDetalle,Email,Telefono,Celular,NoIdentificacion from tConvenio where activo=1 order by " + campoSort + " " + tipoSort).ToList();
                    else
                        objList = Predial.tConvenio.SqlQuery("Select Id,IdConvenioEdoCta,Status,NoParcialidades,PorcentajeConvenio,ImporteTotal,FechaIni,FechaFin,Activo,IdUsuario,FechaModificacion,folio, ConDetalle,Email,Telefono,Celular,NoIdentificacion from tConvenio where activo= 0 order by " + campoSort + " " + tipoSort).ToList();
                }
                else if(campoFiltro == "IdPredio")
                {
                    valorFiltro = "%" + valorFiltro + "%";
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.tConvenio.SqlQuery("Select C.Id,IdConvenioEdoCta,C.Status,NoParcialidades,PorcentajeConvenio,ImporteTotal,FechaIni,FechaFin,C.folio,C.Activo,C.IdUsuario,C.FechaModificacion, ConDetalle , Email,Telefono,Celular,NoIdentificacion from tConvenio as C inner join tConvenioEdoCta on tConvenioEdoCta.id= C.IdConvenioEdoCta where C.activo=1 and tConvenioEdoCta." + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                    else
                        objList = Predial.tConvenio.SqlQuery("Select C.Id,IdConvenioEdoCta,C.Status,NoParcialidades,PorcentajeConvenio,ImporteTotal,FechaIni,FechaFin,C.folio,C.Activo,C.IdUsuario,C.FechaModificacion,ConDetalle,Email,Telefono,Celular,NoIdentificacion from tConvenio as C inner join tConvenioEdoCta on tConvenioEdoCta.id= C.IdConvenioEdoCta where C.activo=1 and tConvenioEdoCta." + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                }
                else
                {//Folio
                    valorFiltro = "%" + valorFiltro + "%";
                    if (activos.ToUpper() == "TRUE")
                        objList = Predial.tConvenio.SqlQuery("Select Id,IdConvenioEdoCta,Status,NoParcialidades,PorcentajeConvenio,ImporteTotal,FechaIni,FechaFin,Activo,IdUsuario,FechaModificacion,folio,ConDetalle,Email,Telefono,Celular,NoIdentificacion from tConvenio where activo=1 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                    else
                        objList = Predial.tConvenio.SqlQuery("Select Id,IdConvenioEdoCta,Status,NoParcialidades,PorcentajeConvenio,ImporteTotal,FechaIni,FechaFin,Activo,IdUsuario,FechaModificacion,folio,ConDetalle , Email,Telefono,Celular,NoIdentificacion from tConvenio where activo=0 and " + campoFiltro + " like  @p order by " + campoSort + " " + tipoSort, new SqlParameter("@p", valorFiltro)).ToList();
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConvenioBL.GetFilter.Exception", ex);
            }
            return objList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<tConvenio> GetAll()
        {
            List<tConvenio> objList = null;
            try
            {
                objList = Predial.tConvenio.Where(o => o.Activo == true).ToList();
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConvenioBL.GetAll.Exception", ex);
            }
            return objList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<string> ListaCampos()
        {
            List<string> propertyList = new List<string>();
            try
            {
                tConvenio pObject = new tConvenio();
                if (pObject != null)
                {
                    foreach (var prop in pObject.GetType().GetProperties())
                    {
                        if (prop.Name.ToUpper() != "ID" && prop.Name.ToUpper() != "IDUSUARIO" && prop.Name.ToUpper() != "STATUS" && prop.Name.ToUpper() != "ACTIVO" && prop.Name.ToUpper() != "FECHAMODIFICACION" && prop.Name.Substring(0, 1) != "c" && prop.Name.Substring(0, 1) != "t" && prop.Name.Substring(0, 1) != "m" && prop.Name.Substring(0, 1) != "I")
                            propertyList.Add(prop.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConvenioBL.ListaCampos.Exception", ex);
            }
           
            return propertyList;
        }

        public tConvenio GetByIdConvenioEstadoCta(int idEstado)
        {
            tConvenio obj = null;
            try
            {
                obj = Predial.tConvenio.FirstOrDefault(o => o.IdConvenioEdoCta == idEstado && o.Status != "C"  && o.Activo == true);
            }
            catch (Exception ex)
            {
                new Utileria().logError("tConvenioBL.GetByIdConvenioEstadoCta.Exception", ex , "--Parámetros idEstado:" + idEstado);
            }
            return obj;
        }

        public Int32 ObtenerConvenioPorIdPredio(Int32 idPredio)
        {

            List<tConvenioEdoCta> objList = new List<tConvenioEdoCta>();
            string status = string.Empty;
#pragma warning disable CS0219 // La variable 'con' está asignada pero su valor nunca se usa
            int con = 0;
#pragma warning restore CS0219 // La variable 'con' está asignada pero su valor nunca se usa

            try
            {
                objList = Predial.tConvenioEdoCta.Where(o =>  o.IdPredio == idPredio && o.Activo == true).ToList();
                
                if (objList.Count() > 0)
                {
                    foreach (tConvenioEdoCta prop in objList)
                     {
                        tConvenio c = new tConvenioBL().GetByIdConvenioEstadoCta(prop.Id);
                        if (c != null && (c.Status == "X" || c.Status == "A"))                           
                            return c.Id;
                        //else
                        //    return 0;
                     }                  
                    
                }

            }
            catch (Exception ex)
            {
                new Utileria().logError("tRequerimiento.GetAll.Exception", ex);
            }
            return 0;
        }

        public Int32 MaxFolioConvenio()
        {
            Int32 max = 1;

#pragma warning disable CS0219 // La variable 'objList' está asignada pero su valor nunca se usa
            List <tConvenio> objList = null;
#pragma warning restore CS0219 // La variable 'objList' está asignada pero su valor nunca se usa
            try
            {
                //objList = Predial.tConvenio.Where(o => SqlFunctions.PatIndex("%[0-9]%", o.Folio) > 0)).ma;
                //if (objList.Count() > 0)
                // max = objList.Max(o => Int32.Parse(o.Folio));
                max = Convert.ToInt32(Predial.tConvenio.Where(o => SqlFunctions.PatIndex("%[0-9]%", o.Folio) > 0).Max(o => o.Folio));
                
            }               
            catch (Exception ex)
            {
                new Utileria().logError("tConvenioBL.GetByConstraint.Exception", ex, "--Parámetros id:" + max);
            }

            return max+1;
        }
    }
}

