using System;
/// <summary>
/// Summary description for MensajesInterfaz
/// </summary>
/// 

namespace Clases.Utilerias
{
    public enum MensajesInterfaz
    {
        /// <summary>
        /// No existen datos para el elemento buscado
        /// </summary>
        [Description("No existen datos para el elemento buscado.")]
        NoEncontrados = 1,
        /// <summary>
        /// Los datos han sido ingresados satisfactoriamente.
        /// </summary>
        [Description("Los datos han sido ingresados satisfactoriamente.")]
        Ingreso = 2,
        /// <summary>
        /// Los datos han sido modificados correctamente.
        /// </summary>
        [Description("Los datos han sido modificados correctamente.")]
        Actualizacion = 3,
        /// <summary>
        /// Los datos han sido eliminados satisfactoriamente.
        /// </summary>
        [Description("Los datos han sido eliminados satisfactoriamente.")]
        Eliminacion = 4,
        /// <summary>
        ///�Est� seguro que quiere eliminar el registro? 
        /// </summary>
        [Description("�Est� seguro que quiere eliminar el registro?")]
        ConfimacionEliminar = 5,
        /// <summary>
        ///Error al tratar de guardar el registro
        /// </summary>
        [Description("Ocurrio un problema al tratar de guardar el registro")]
        ErrorGuardar = 6,
        /// <summary>
        ///Error al tratar de borrar el registro
        /// </summary>
        [Description("Ocurrio un problema al tratar de eliminar el registro.")]
        ErrorBorrar = 7,
        /// <summary>
        ///Ocurrio un problema al tratar de borrar el registro
        /// </summary>
        [Description("Uno o varios campos no pueden ser modificados.")]
        ErrorActualizar = 8,
        /// <summary>
        ///Error al tratar de borrar el registro
        /// </summary>
        [Description("Ocurrio un problema al tratar de conectarse a la base de datos.")]
        ErrorDB = 9,
        /// <summary>
        ///Error al tratar de borrar el registro
        /// </summary>
        [Description("Ocurrio un problema no controlado.")]
        ErrorGeneral = 10,
        /// <summary>
        ///Usurio o Contrase�a Incorrecta
        /// </summary>
        [Description("Usuario o Contrase�a Incorrecta, Favor de verificar.")]
        ErrorUsuario = 11,
        /// <summary>
        ///Ocurrio un problema, ya existe informaci�n.
        /// </summary>
        [Description("Ocurrio un problema, ya existe informaci�n.")]
        ErrorExists = 12,
        /// <summary>
        /// No elemento no puede ser actualizado.
        /// </summary>
        [Description("Los datos no pueden ser modificados.")]
        NoActualizar = 13,    
        /// <summary>
        /// Regresar
        /// </summary>
        [Description("Los cambios sin guardar se perder�n, �Estas seguro de continuar?.")]
        RegresarMSG = 14,
        /// <summary>
        /// Regresar
        /// </summary>
        [Description("�Est� seguro que quiere activar el registro?")]
        ActivarRegistro = 15,
        /// <summary>
        /// 
        /// </summary>
        [Description("No se puede registrar el salario m�nimo, debido a que actualmente cuenta con otro activo, por favor delo de baja para poder continuar.")]
        SalarioVigente = 16,
         /// <summary>
        /// 
        /// </summary>
        [Description("El usuario se encuentra dado de baja, favor se comunicarse con el administrador del sistema.")]
        UsuarioInactivo = 17,
        /// <summary>
        /// 
        /// </summary>
        [Description(" Usuario y/o Contrase�a Invalidas.")]
        UsuarioNoExiste = 18,
        [Description("El nombre de usuario o el numero de empleado ya existe.")]
        UsuarioExiste = 19,
        /// <summary>
        /// 
        /// </summary>
        [Description("La clave catastral no existe.")]
        PredioInexistente = 20,

        [Description("La Clave del predio debe de ser 12 caracteres, Favor de verificarla .")]
        FormatoPredio = 21,
        /// <summary>
        /// 
        /// </summary>
        [Description("Cobro Anticipado habilitado, la base del impuesto anticipado no esta definida. Consulte con el administrador del sistema.")]
        BaseImpuestoAnticipado = 22,
        /// <summary>
        /// 
        /// </summary>
        [Description("No esta definido el Ejercicio o Bimestre inicial de Cobro para cobro de Impuesto Predial. Pasar a Catastro.")]
        EjercicioIniFin = 23,
        /// <summary>
        /// 
        /// </summary>
        [Description("Clave de predial con problemas, para generar el calculo. Consulte con el administrador del sistema")]
        CalculoImpuesto = 24,
        /// <summary>
        /// 
        /// </summary>
        [Description("No esta definida la aplicaci�n de la Prescripci�n ingresar el valor de SI/NO. Consulte con el administrador del sistema")]
        AplicarPrescripcion = 25,

        [Description("Corregir este mesaje de interfaz, por favor   -seleccionaEjercicioFinal-")]
        seleccionaEjercicioFinal = 26,

        [Description("El Indice Nacional de Precios debe actualizarse al mes anterior, despu�s del d�a 10 del mes. Informe al administrador.")]
        INPactual = 27,

        [Description("El Indice Nacional de Precios del a�o anterior no esta definido. Informe al administrador.")]
        INPanterior = 28,

        [Description("No se ha definido el Porcentaje de Adicional, consulte con el administrador")]
        PorcAdicional = 29,

        [Description("No se ha definido el Porcentaje de los Primeros 70,000 de la Base Gravable, consulte con el administrador")]
        DosMillar = 30,

        [Description("No se ha definido el Porcentaje por el Excedente de la Base Gravable, consulte con el administrador")]
        TresMillar = 31,

        [Description("No se ha definido el Ejercicio Inicial para el cobro de rezago, consulte con el administrador")]
        EjercicioInicialCobro = 32,

        [Description("Revisar los UMA's Anualizados, consulte con el administrador")]
        SalarioMinumoAnualizado = 33,

        [Description("�Est� seguro que quiere Guardar los Cambios?")]
        ConfirmacionGuardado = 34,

        [Description("El ejercicio de cobro esta incorrecto, favor de verificarlo")]
        EjercicioIncorrecto = 35,
        
        [Description("No se puede registrar el FIEL, debido a que actualmente cuenta con otro activo, por favor delo de baja para poder continuar.")]
        FIELVigente = 36,

        [Description("El Salario M�nimo no esta definido para el a�o: ")]
        SalarioMinimo = 37,

        [Description("La Base Gravable no esta definido para el a�o: ")]
        BaseGravable = 38,

        [Description("Base de los primeros metros no esta definido para el a�o: ")]
        BaseDelImpuesto = 39,

        [Description("No estan definidos los a�os para aplicar la prescripci�n")]
        AniosPrescripcion = 40,

        [Description("No se ha definido la tarifa por tipo de predio para el a�o: ")]
        CuotaTipoPredio = 41,

        [Description("Impuesto por bimestre erroneo, consulte con el administrado")]
        ImpuestoPorBimestre = 42,

        [Description("No se ha definido la tarifa de la Zona para el a�o: ")]
        TarifaZona = 43,

        [Description("No se ha definido la tarifa de Limpieza para el a�o: ")]
        TarifaLimpieza = 44,

        [Description("No se ha definido la tarifa de Recolecci�n de Residuos para el a�o: ")]
        TarifaRecoleccion = 45,

        [Description("No se ha definido la tarifa de DAP para el a�o: ")]
        TarifaDAP = 46,

        [Description("No se ha definido el porcentaje de Recargos para el a�o: ")]
        TarifaRecargos = 47,

        [Description("No se ha definido la serie del recibo, consulte al administrador: ")]
        DefinirSerie = 48,

        [Description("No se ha definido la mesa de Impuesto Predial  (Par�metros Sistema), consulte al administrador: ")]
        DefinirMesaIP = 49,

        [Description("No se ha definido la mesa de Servicios Municipales  (Par�metros Sistema), consulte al administrador: ")]
        DefinirMesaSM = 50,

        [Description("No se ha definido la mesa (Cat�logo Mesa), consulte al administrador: ")]
        DefinirMesaCatalogo = 51,

        [Description("Error en el Tipo de Pago, consulte al administrador (Cat�logo Tipo Pago)")]
        TipoPago =52,

        [Description("El predio se encuentra dado de baja, favor se comunicarse con el administrador del sistema.")]
        PredioInactivo = 53,

        [Description("Selecciona el m�todo de pago")]
        SeleccionTipoPago = 54,

        [Description("Ingresa la cantidad de efectivo para elpago")]
        IngresaEfectivo = 55,

        /// <summary>
        /// 
        /// </summary>
        [Description("La caja se ha cerrado corectamente")]
        CajaCerrada = 56,
        /// <summary>
        /// 
        /// </summary>
        [Description("El empleado que busca no fue encontrado.")]
        NoEmpleado = 57,

        [Description("�Desea cerrar esta caja?")]
        CerrarCaja = 58,
        /// </summary>
        [Description("Debe escribir el n�mero de un empleado.")]
        NoEmpleadoVacio = 59,
        [Description("Los datos seleccionados ya existen en una caja abierta.")]
        CajeroCajaAbierta = 60,
        [Description("No cuentas con una caja asignada, para realizar cobros")]
        SinCajaActiva = 61,
        [Description("La Clave del predio no tiene asignado alg�n plano, Favor de verificarla .")]
        PredioSinPlano = 62,
        [Description("La Clave del predio debe contener por lo menos 8 caract�res.")]
        ClaveMinOcho = 63,
        [Description("El nombre del contribuyente debe contener por lo menos 3 caract�res.")]
        NombreMinTres = 64,
        [Description("No esta definido el par�metro AplicarActualizacionINP ( Par�metro Sistema).")]
        AplicarAtualizacionINP = 65,
        [Description("Imposible consultar el predio, se encuentra en estado Suspendido.")]
        sTatusPredioSuspendido = 66,
        [Description("Imposible consultar el predio, se encuentra en estado de Baja.")]
        sTatusPredioBaja = 67,

        [Description("El predio no cuenta con un tramite registrado. No es posible realizar los descuentos a este tipo de tramite para el predio indicado.")]
        TramiteInexistente = 68,
        [Description("No se ha definido el tipo de cobro para el Catastro, consulte al administrador. ")]
        cobroTipoCatastro = 69,
        /// <summary>
        /// Los datos han sido ingresados satisfactoriamente.
        /// </summary>
        [Description("EL RFC ha sido ingresados satisfactoriamente.")]
        IngresoRFC = 70,

        /// <summary>
        /// Los datos han sido modificados correctamente.
        /// </summary>
        [Description("El RFC ha sido modificado correctamente.")]
        ActualizacionRFC = 71,

        [Description("Debe seleccionar el tipo de tramite e ingresar la clave del predio para asignar los descuentos.")]
        Seleccionatramite = 72,

        [Description("El predio cuenta con un convenio, no se pueden generar tramites.")]
        PredioEnConvenio = 73,

        [Description("El predio cuenta con una base gravable para ese ejercicio, �desea guardar los cambios?")]
        BaseRepetida = 74,
        [Description("�Est� seguro que quiere Cancelar el registro?")]
        ConfirmacionCancelar = 75,
        [Description("Imposible generar el tramite, Favor de pasar a catastro por notificaci�n catastral...")]
        NoAGenerarTramite = 76,
        [Description("Favor de pasar a Catastro, para verificar la informaci�n del predio")]
        FechaAvaluo = 77,
        [Description("La fecha de Avaluo es menor de 2 a�os, el tramite no puede ser generado.")]
        FechaAvaluoMenorDosAnios = 78,
        [Description("La colonia no es v�lida.")]
        ColoniaNoValida = 79,
        [Description("Factura Enviada por correo exitosamente.")]
        FacturaCorreo = 80,
        [Description("La factura no pudo ser enviada por correo.")]
        FacturaCorreoError = 81,
        [Description("Disculpe las molestias se presento un problema al generar su recibo oficial del Ayuntamiento, favor de comunirse a la Direcci�n de Ingresos.")]
        PagoEnLineaFailDeposito = 82,
        [Description("Ok")]
        PagoEnLineaOK = 83,
        [Description("Clave de descuento no encontrada")]    
        Clave = 85,
        [Description("Pago en l�nea presento un problema, ya se le notifico al administrador. Intente m�s tarde.")]
        PagoEnLineaFail = 86,
        [Description("Periodo de pago incorrecto. El a�o inicial es mayor al a�o final, favor de verificar.")]
        PeriodoIncorrecto = 87,
        [Description("Periodo de diferencias incorrecto. El a�o inicial es mayor al a�o final, favor de verificar.")]
        PeriodoIncorrectoDif = 88,
        [Description("Error al generar el Recibo Digital")]
        ReciboDigitalError = 89,
        [Description("No se genero el listado de Conceptos para el recibo Digital")]
        ConceptoDigitalError = 90,
        [Description("Ok")]
        TramiteGuardado = 91,
        [Description("�Est� seguro que quiere cancelar la factura?")]
        ConfirmacionCancelaFactura = 92,
        [Description("No se ha definido el tipo de cobro, consulte al administrador ")]
        DefinirTipoCobro = 93,
        [Description("No se ha actualizado el incide nacional de precios, consulte al administrador ")]
        INPnoActualizado = 94,
        [Description("La clave catastral ya se encuentra registrada.")]
        ClaveResgistrada = 95,
        [Description("Imposible Generar el Recibo, el estado de internet cambio, consulte al administrador")]
        InternetEstado = 96,
        [Description("�Esta seguro que desea continuar con la generaci�n del recibo?.")]
        ReciboInternetConfirmacion = 97,
        [Description("El convenio tiene conceptos desglosados")]
        ListConvenioDesglozado = 98,
        [Description("El contribuyente es invalido. Verifique la informaci�n.")]
        ContribuyenteNoValido = 99,
        [Description("El contribuyente ha sido guardado, continue con la captura.")]
        ContribuyenteAgregado = 100,
        [Description("La clave catastral, ya esta pagada.")]
        ClavePagada = 101,
        [Description("Ocurrio un problema al tratar de Replicar todas las BG, contacta al aDministrador o vuelve a intentarlo mas tarde.")]
        ErrorBGReplicar = 102,
        [Description("La cuenta de predial no existe.")]
        CuentaInexistente = 103,
        [Description("El recibo ser� facturado. �Desea continuar?")]
        ConfirmacionFacturaPendiente = 104,
        [Description("El recibo fue facturado correctamente.")]
        ExitoFacturaPendiente = 105,
        [Description("El recibo no pudo ser facturado.")]
        ErrorFacturaPendiente = 106,
        [Description("Plano generado.")]
        PlanoDownload = 107,
    }
}
    