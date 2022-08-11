<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BusquedaBitacora.aspx.cs" Inherits="Catastro.Catalogos.BusquedaBitacora" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 30px;"></td>
                </tr>
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="Label1" runat="server" Text="Bitacora de operación" CssClass="letraTitulo"></asp:Label></td>
                </tr>
                <tr>
                    <td style="height: 2px">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="padding: 0px 10px 10px 10px;">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td style="width: 230px">
                                    <asp:Label ID="Label2" runat="server" Text="Usuario:" CssClass="letraTitulo"></asp:Label>

                                </td>
                                <td style="width: 350px">
                                    <asp:DropDownList ID="ddlUsuarios" runat="server" Width="250px">
                                    </asp:DropDownList>
                                </td>
                                <td rowspan="4" style="vertical-align: top;">
                                    <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="True"
                                        ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" ValidationGroup="buscar" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Interfaz:" CssClass="letraTitulo"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlVentana" runat="server" CssClass="ddlGrande" Width="250px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="Clave castral:" CssClass="letraTitulo"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtClave" runat="server" CssClass="textGrande"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender runat="server" ID="meeFiltro" Mask="9999-99-999-999" TargetControlID="txtClave" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Rango de Fechas:" CssClass="letraTitulo"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textMediano" placeholder="Fecha Inicio." Width="120px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvFechaInicio" runat="server" ControlToValidate="txtFechaInicio" CssClass="valida" ErrorMessage="*" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                                    <ajaxToolkit:MaskedEditExtender ID="txtFechaInicio_MaskedEditExtender" runat="server" BehaviorID="txtFechaInfra_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaInicio" />
                                    <ajaxToolkit:CalendarExtender ID="txtFechaInicio_CalendarExtender" runat="server" BehaviorID="txtFechaInfra_CalendarExtender" CssClass="CalendarCSS" Format="dd/MM/yyyy" TargetControlID="txtFechaInicio" />
                                    <asp:TextBox ID="txtFechaFin" runat="server" CssClass="textMediano" placeholder="Fecha Final."></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="txtFechaFin_MaskedEditExtender" runat="server" BehaviorID="txtFechaFin_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaFin" />
                                    <asp:RequiredFieldValidator ID="rfvFechaFin" runat="server" ControlToValidate="txtFechaFin" CssClass="valida" ErrorMessage="*" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="txtFechaInfra_CalendarExtender" CssClass="CalendarCSS" Format="dd/MM/yyyy" TargetControlID="txtFechaFin" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="padding: 0px 10px 10px 10px;">
                        <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False"
                            CssClass="grd" DataKeyNames="Id,IdTabla,NombreTabla,Tabla,Usuario,ClavePredial" AllowPaging="True"
                            AllowSorting="True" BorderStyle="None" ShowFooter="True" OnSorting="grd_Sorting" OnPageIndexChanging="grd_PageIndexChanging" PageSize="50" OnRowCommand="grd_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="ventana" HeaderText="Ventana" SortExpression="ventana" />
                                <asp:BoundField DataField="ClavePredial" HeaderText="Clave Predial" SortExpression="ClavePredial" />
                                <asp:BoundField DataField="Tabla" HeaderText="Tabla" SortExpression="Tabla" />
                                <asp:BoundField DataField="Movimiento" HeaderText="Movimiento" SortExpression="Movimiento" />
                                <asp:BoundField DataField="Usuario" HeaderText="Usuario" SortExpression="Usuario" />
                                <asp:BoundField DataField="FechaModificacion" HeaderText="Fecha de Modificación" SortExpression="FechaModificacion" DataFormatString="{0:dd-MM-yyyy hh:mm:ss tt}" />
                                <asp:BoundField DataField="MaquinaIP" HeaderText="Maquina" SortExpression="MaquinaIP" />
                                <asp:ButtonField ButtonType="Image" CommandName="consultar" ImageUrl="~/Img/consultar.fw.png" Text="Registro" HeaderText="Registro" />
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmptySearch" runat="server" CssClass="letraTituloEmptyGrid">No se encontraron resultados</asp:Label>
                            </EmptyDataTemplate>
                            <FooterStyle CssClass="grdFooter" />
                            <HeaderStyle CssClass="grdHead" />
                            <RowStyle CssClass="grdRowPar" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnl" runat="server" class="formPanel">
                <asp:Label ID="lbl_titulo" runat="server" CssClass="textModalTitulo2" Text="Consulta Registro"></asp:Label>
                <asp:Panel ID="panelgrid" runat="server" Height="600px" ScrollBars="Vertical" Width="700px">
                    <asp:GridView ID="gdrRegistro" runat="server" AutoGenerateColumns="False" CssClass="grd" BorderStyle="None" Width="500px">
                        <Columns>
                            <asp:BoundField DataField="Campo" HeaderText="Campo" />
                            <asp:BoundField DataField="Valor" HeaderText="Valor" />
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmptySearch" runat="server" CssClass="letraTituloEmptyGrid">No se encontraron resultados</asp:Label>
                        </EmptyDataTemplate>
                        <FooterStyle CssClass="grdFooter" />
                        <HeaderStyle CssClass="grdHead" />
                        <RowStyle CssClass="grdRowPar" />
                    </asp:GridView>
                </asp:Panel>
            <asp:Button ID="btnCerrar" runat="server" CausesValidation="False" Text="Cerrar" OnClick="btnCerrar_Click" />
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="pnl_Modal" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnl" TargetControlID="btnPnl">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnPnl" runat="server" />

            <uc1:ModalPopupMensaje ID="ModalPopupMensaje1" runat="server" DysplayAceptar="True" DysplayCancelar="True" />



            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

