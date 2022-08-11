<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConfigAnticipado.aspx.cs" Inherits="Catastro.Catalogos.ConfigAnticipado" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            


            <asp:Label ID="lblTitulo" runat="server" Text="-------------" CssClass="letraTitulo"></asp:Label>
            <hr />
            <asp:Label ID="lbltextoActualizacion" runat="server" Text="------------" CssClass="letraMediana"></asp:Label>


            <br />
            <table style="width:100%;">
                <tr>
                    <td>
                        <asp:Button ID="btnRealizarMasivo" runat="server" OnClick="btnRealizarMasivo_Click" Text="Masivo Base Gravable" Width="204px" />
                    </td>
                    <td>Actualiza el padrón de bases gravables para el cobro del año anticipado.</td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnConceptos" runat="server" EnableViewState="False" Text="Conceptos Cobro" Width="204px" OnClick="btnConceptos_Click" />
                    </td>
                    <td>Genera los conceptos de cobro del siguiente ejercicio.</td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnUMA" runat="server" Text="UMA" Width="205px" OnClick="btnUMA_Click" />
                    </td>
                    <td>G<span>enera la Unida de Medida A (UMA).</span></td>
                </tr>
                <tr>
                    <td style="height: 20px">
                        <asp:Button ID="btnBaseImpuesto" runat="server" Text="Base Impuesto" Width="205px" OnClick="btnBaseImpuesto_Click" />
                    </td>
                    <td style="height: 20px">Base de Impuesto para la primera cantidad de la base gravable establecida en la Ley de Ingresos.</td>
                </tr>
                <tr>
                    <td style="height: 22px">
                        <asp:Button ID="btnCuotasPredio" runat="server" Text="Cuotas Tipo Predio" Width="205px" OnClick="btnCuotasPredio_Click" />
                    </td>
                    <td style="height: 22px">Cuotas por Tipo de Predio (urbano, baldío, rústico, etc.) Esta determina el cobro mínimo de acuerdo al tipo de predio.</td>
                </tr>
                <tr>
                    <td style="height: 22px">
                        <asp:Button ID="btnZonas" runat="server" Text="Tarifas Zona" Width="205px" OnClick="btnZonas_Click" />
                    </td>
                    <td style="height: 22px">Tarifa de Zonas, estos valores son para el cobro de los Servicios Municipales (infraestructura o derechos)</td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnRecoleccion" runat="server" Text="Tarifa Recolección" Width="205px" OnClick="btnRecoleccion_Click" />
                    </td>
                    <td>Tarifa para la Recolección de Residuos Solidos o Recolección de Basura</td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnLimpieza" runat="server" Text="Tarifa Limpieza" Width="205px" OnClick="btnLimpieza_Click" />
                    </td>
                    <td>Tarifa Limpieza de Frente Baldios</td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnDap" runat="server" Text="Tarifa DAP" Width="205px" OnClick="btnDap_Click" />
                    </td>
                    <td>Tarifa de cobro de Derecho de Alumbrado Público DAP</td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnRecargos" runat="server" Text="Tarifa Recargos" Width="205px" OnClick="btnRecargos_Click" />
                    </td>
                    <td>Tarifa de Recargos para los siguientes meses.</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                 <tr>
                    <td>&nbsp;</td>
                    <td>Cambiar el <span style="color: #6600CC; font-size: large">Mes inicial para cobro anticipado</span>&nbsp;, en menú Configuración -> Parametros Cobro.</td>
                </tr>
                 <tr>
                    <td>&nbsp;</td>
                    <td>Cambiar el <span style="color: #6600CC; font-size: large">Ejercicio anticipado</span> para el cobro Anticipado, en menú&nbsp; Configuracion -&gt; Parametros Cobro.</td>
                </tr>
                <tr>
                    <td style="height: 27px"></td>
                    <td style="height: 27px">Configurar la clave de <span style="color: #6600CC; font-size: large">Descuento global de Predial</span> para cobro Anticipado, en menú &nbsp; Configuración -&gt; Parametros Sistema.</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>Configurar la clave de <span style="color: #6600CC; font-size: large">Descuento global de Servicios</span> para cobro Anticipado, en menú&nbsp; Configuracion -&gt; Parametros Sistema.<br />
                        </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>Configurar el importe de <span style="color: #6600CC; font-size: large">Descuento global de Predial </span>para cobro Anticipado&nbsp;en Menú Configuración -&gt; Descuentos Globales.</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>Configurar el importe de <span style="color: #6600CC; font-size: large">Descuento global de Servicios</span> para el cobro anticipado, en menú Configuración -&gt; Descuentos Globales.</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>Configurar el importe de&nbsp; <span style="color: #6600CC; font-size: large">Descuento de campaña Jubilados, Pensionados y 3ra Edad</span> para cobro Anticipado, en menú Configuración -&gt; Parametros Sistema.<br />
                        </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>

            </table>


            <asp:Panel ID="pnl" runat="server" class="formPanel">
                Realizando Acciones....
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="pnl_Modal" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnl" TargetControlID="btnPnl">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnPnl" runat="server" />

            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
