using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catastro.ModelosFactura
{
    public class Constantes
    {
        //Modo pruebas true , produccion false
        public static bool modoPruebas = true;

        //DEV
        //https://dev.timbradorxpress.mx/ws/servicio.do?wsdl
        //PROD
        //https://app.facturaloplus.com/ws/servicio.do?wsdl

        // ######################### PREDIAL #################################
        public static string codigo = "sipred";
        public static string siglasSistema = "s i p r e d";
        public static string nombreSistema = "MUNICIPIO DE EMILIANO ZAPATA, MOR. ";
        public static string nombreOficina = "DIRECCIÓN DE PREDIAL Y CATASTRO";
        public static string correoSistema = "recaudacionezapata@gmail.com";
        public static string telefonoSistema = "777-246-06-28, 777-246-06-27 ";

        public static string correoEmail = "facturacionsipred@hotmail.com";
        public static string contraEmail = "@EZApata2022";
        //El primer key es de pruebas : el segundo es el productivo., cuando subamos a produccion tenemos que cambiar la bandera modopruebas(false)
        public static string apikey = (modoPruebas) ? "0f19e26ab63b4e7495ae26fb0379cadf" : "fbfb032ebdb443c09c747bfe458af0b3";
        // ######################### PREDIAL #################################

        public static long cpSistema = 62760;
        public static string direccionSistema = "PLAZA 10 DE ABRIL Num. S/N CENTRO, EMILIANO ZAPATA, MORELOS C.P. 62760";
        public static string formatoFechaArchivos = "yyyy-MM-ddT HH-mm-ss";

        public static string pw = (modoPruebas) ? "12345678a" :  "2011LFGD";
        public static string cerName = (modoPruebas) ? "CSD_EKU9003173C9.cer.pem" : "00001000000503627557.cer.pem";
        public static string keyName = (modoPruebas) ? "CSD_EKU9003173C9.key.pem" : "CSD_MUNICIPIO_DE_EMILIANO_ZAPATA_MEZ940101K26_20200323_141819.key.pem";

        public static string rfcEmisor = (modoPruebas) ? "EKU9003173C9" : "MEZ940101K26";
        public static long regimenFiscalEmisor = 603;
        public static string nombreEmisor = "MUNICIPIO DE EMILIANO ZAPATA";
        public static string noCertificadoEmisor = (modoPruebas) ? "30001000000400002434" : "00001000000503627557";

        public static string archivosSistemaFolder = System.Environment.CurrentDirectory;
        public static string jsonEnviado = @"C:\sipred-files\jsonEnviado.txt";
        public static string jsonRespuesta = @"C:\sipred-files\jsonRespuesta.txt";

        public static string recibosSistemaFolder = "/sipred-files/Recibos expedidos/";

        public static string cortesSistemaFolder = @"C:\sipred-files\Cortes de caja";
        public static string ordenesTrabajoSistemaFolder = @"C:\sipred-files\Ordenes de trabajo";
        public static string notificacionSistemaFolder = @"C:\sipred-files\Notificación";

        public static string recibosReimpresosSistemaFolder = "/sipred-files/Recibos expedidos/Reimpresos/";
        public static string xmlSistemaFolder = "/sipred-files/Recibos expedidos/xml/";

        public static string catalogosSistemaFolder = @"C:\sipred-files\Catalogos";
        public static string reportesSistemaFolder = @"C:\sipred-files\Reportes";
        public static string historicoPagosSistemaFolder = @"C:\sipred-files\Historico Pagos";
        public static string historicoOrdenesSistemaFolder = @"C:\sipred-files\Ordenes de trabajo";
        public static string tomanuevaSistemaFolder = @"C:\sipred-files\Ordenes de trabajo\Tomas nuevas";
        public static string reinstalacionSistemaFolder = @"C:\sipred-files\Ordenes de trabajo\Reinstalación";
        public static string convenioSistemaFolder = @"C:\sipred-files\Convenios";
        public static string contratosSistemaFolder = @"C:\sipred-files\Contratos";


    }
}