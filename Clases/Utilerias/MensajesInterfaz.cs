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
        ///¿Está seguro que quiere eliminar el registro? 
        /// </summary>
        [Description("¿Está seguro que quiere eliminar el registro?")]
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
        ///Usurio o Contraseña Incorrecta
        /// </summary>
        [Description("Usuario o Contraseña Incorrecta, Favor de verificar.")]
        ErrorUsuario = 11,
        /// <summary>
        ///Ocurrio un problema, ya existe información.
        /// </summary>
        [Description("Ocurrio un problema, ya existe información.")]
        ErrorExists = 12,
        /// <summary>
        /// No elemento no puede ser actualizado.
        /// </summary>
        [Description("Los datos no pueden ser modificados.")]
        NoActualizar = 13,    
        /// <summary>
        /// Regresar
        /// </summary>
        [Description("Los cambios sin guardar se perderán, ¿Estas seguro de continuar?.")]
        RegresarMSG = 14,
        /// <summary>
        /// Regresar
        /// </summary>
        [Description("¿Está seguro que quiere activar el registro?")]
        ActivarRegistro = 15,
        /// <summary>
        /// 
        /// </summary>
        [Description("No se puede registrar el salario mínimo, debido a que actualmente cuenta con otro activo, por favor delo de baja para poder continuar.")]
        SalarioVigente = 16,
         /// <summary>
        /// 
        /// </summary>
        [Description("El usuario se encuentra dado de baja, favor se comunicarse con el administrador del sistema.")]
        UsuarioInactivo = 17,
        /// <summary>
        /// 
        /// </summary>
        [Description(" Usuario y/o Contraseña Invalidas.")]
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
        [Description("No esta definida la aplicación de la Prescripción ingresar el valor de SI/NO. Consulte con el administrador del sistema")]
        AplicarPrescripcion = 25,

        [Description("Corregir este mesaje de interfaz, por favor   -seleccionaEjercicioFinal-")]
        seleccionaEjercicioFinal = 26,

        [Description("El Indice Nacional de Precios debe actualizarse al mes anterior, después del día 10 del mes. Informe al administrador.")]
        INPactual = 27,

        [Description("El Indice Nacional de Precios del año anterior no esta definido. Informe al administrador.")]
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

        [Description("¿Está seguro que quiere Guardar los Cambios?")]
        ConfirmacionGuardado = 34,

        [Description("El ejercicio de cobro esta incorrecto, favor de verificarlo")]
        EjercicioIncorrecto = 35,
        
        [Description("No se puede registrar el FIEL, debido a que actualmente cuenta con otro activo, por favor delo de baja para poder continuar.")]
        FIELVigente = 36,

        [Description("El Salario Mínimo no esta definido para el año: ")]
        SalarioMinimo = 37,

        [Description("La Base Gravable no esta definido para el año: ")]
        BaseGravable = 38,

        [Description("Base de los primeros metros no esta definido para el año: ")]
        BaseDelImpuesto = 39,

        [Description("No estan definidos los años para aplicar la prescripción")]
        AniosPrescripcion = 40,

        [Description("No se ha definido la tarifa por tipo de predio para el año: ")]
        CuotaTipoPredio = 41,

        [Description("Impuesto por bimestre erroneo, consulte con el administrado")]
        ImpuestoPorBimestre = 42,

        [Description("No se ha definido la tarifa de la Zona para el año: ")]
        TarifaZona = 43,

        [Description("No se ha definido la tarifa de Limpieza para el año: ")]
        TarifaLimpieza = 44,

        [Description("No se ha definido la tarifa de Recolección de Residuos para el año: ")]
        TarifaRecoleccion = 45,

        [Description("No se ha definido la tarifa de DAP para el año: ")]
        TarifaDAP = 46,

        [Description("No se ha definido el porcentaje de Recargos para el año: ")]
        TarifaRecargos = 47,

        [Description("No se ha definido la serie del recibo, consulte al administrador: ")]
        DefinirSerie = 48,

        [Description("No se ha definido la mesa de Impuesto Predial  (Parámetros Sistema), consulte al administrador: ")]
        DefinirMesaIP = 49,

        [Description("No se ha definido la mesa de Servicios Municipales  (Parámetros Sistema), consulte al administrador: ")]
        DefinirMesaSM = 50,

        [Description("No se ha definido la mesa (Catálogo Mesa), consulte al administrador: ")]
        DefinirMesaCatalogo = 51,

        [Description("Error en el Tipo de Pago, consulte al administrador (Catálogo Tipo Pago)")]
        TipoPago =52,

        [Description("El predio se encuentra dado de baja, favor se comunicarse con el administrador del sistema.")]
        PredioInactivo = 53,

        [Description("Selecciona el método de pago")]
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

        [Description("¿Desea cerrar esta caja?")]
        CerrarCaja = 58,
        /// </summary>
        [Description("Debe escribir el número de un empleado.")]
        NoEmpleadoVacio = 59,
        [Description("Los datos seleccionados ya existen en una caja abierta.")]
        CajeroCajaAbierta = 60,
        [Description("No cuentas con una caja asignada, para realizar cobros")]
        SinCajaActiva = 61,
        [Description("La Clave del predio no tiene asignado algún plano, Favor de verificarla .")]
        PredioSinPlano = 62,
        [Description("La Clave del predio debe contener por lo menos 8 caractéres.")]
        ClaveMinOcho = 63,
        [Description("El nombre del contribuyente debe contener por lo menos 3 caractéres.")]
        NombreMinTres = 64,
        [Description("No esta definido el parámetro AplicarActualizacionINP ( Parámetro Sistema).")]
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

        [Description("El predio cuenta con una base gravable para ese ejercicio, ¿desea guardar los cambios?")]
        BaseRepetida = 74,
        [Description("¿Está seguro que quiere Cancelar el registro?")]
        ConfirmacionCancelar = 75,
        [Description("Imposible generar el tramite, Favor de pasar a catastro por notificación catastral...")]
        NoAGenerarTramite = 76,
        [Description("Favor de pasar a Catastro, para verificar la información del predio")]
        FechaAvaluo = 77,
        [Description("La fecha de Avaluo es menor de 2 años, el tramite no puede ser generado.")]
        FechaAvaluoMenorDosAnios = 78,
        [Description("La colonia no es válida.")]
        ColoniaNoValida = 79,
        [Description("Factura Enviada por correo exitosamente.")]
        FacturaCorreo = 80,
        [Description("La factura no pudo ser enviada por correo.")]
        FacturaCorreoError = 81,
        [Description("Disculpe las molestias se presento un problema al generar su recibo oficial del Ayuntamiento, favor de comunirse a la Dirección de Ingresos.")]
        PagoEnLineaFailDeposito = 82,
        [Description("Ok")]
        PagoEnLineaOK = 83,
        [Description("Clave de descuento no encontrada")]    
        Clave = 85,
        [Description("Pago en línea presento un problema, ya se le notifico al administrador. Intente más tarde.")]
        PagoEnLineaFail = 86,
        [Description("Periodo de pago incorrecto. El año inicial es mayor al año final, favor de verificar.")]
        PeriodoIncorrecto = 87,
        [Description("Periodo de diferencias incorrecto. El año inicial es mayor al año final, favor de verificar.")]
        PeriodoIncorrectoDif = 88,
        [Description("Error al generar el Recibo Digital")]
        ReciboDigitalError = 89,
        [Description("No se genero el listado de Conceptos para el recibo Digital")]
        ConceptoDigitalError = 90,
        [Description("Ok")]
        TramiteGuardado = 91,
        [Description("¿Está seguro que quiere cancelar la factura?")]
        ConfirmacionCancelaFactura = 92,
        [Description("No se ha definido el tipo de cobro, consulte al administrador ")]
        DefinirTipoCobro = 93,
        [Description("No se ha actualizado el incide nacional de precios, consulte al administrador ")]
        INPnoActualizado = 94,
        [Description("La clave catastral ya se encuentra registrada.")]
        ClaveResgistrada = 95,
        [Description("Imposible Generar el Recibo, el estado de internet cambio, consulte al administrador")]
        InternetEstado = 96,
        [Description("¿Esta seguro que desea continuar con la generación del recibo?.")]
        ReciboInternetConfirmacion = 97,
        [Description("El convenio tiene conceptos desglosados")]
        ListConvenioDesglozado = 98,
        [Description("El contribuyente es invalido. Verifique la información.")]
        ContribuyenteNoValido = 99,
        [Description("El contribuyente ha sido guardado, continue con la captura.")]
        ContribuyenteAgregado = 100,
        [Description("La clave catastral, ya esta pagada.")]
        ClavePagada = 101,
        [Description("Ocurrio un problema al tratar de Replicar todas las BG, contacta al aDministrador o vuelve a intentarlo mas tarde.")]
        ErrorBGReplicar = 102,
        [Description("La cuenta de predial no existe.")]
        CuentaInexistente = 103,
        [Description("El recibo será facturado. ¿Desea continuar?")]
        ConfirmacionFacturaPendiente = 104,
        [Description("El recibo fue facturado correctamente.")]
        ExitoFacturaPendiente = 105,
        [Description("El recibo no pudo ser facturado.")]
        ErrorFacturaPendiente = 106,
        [Description("Plano generado.")]
        PlanoDownload = 107,
    }
}
    