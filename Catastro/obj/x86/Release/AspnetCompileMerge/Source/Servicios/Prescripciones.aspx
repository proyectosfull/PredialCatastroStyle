<%@ Page Title="Prescripciones" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Prescripciones.aspx.cs" Inherits="Catastro.Catalogos.prescripciones" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="lblTitulo" runat="server" CssClass="textModalTitulo2" Text="Prescripciones"></asp:Label>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 70%">
                        <table style="width: 100%">                           
                            <tr style="background-color: #b4b4b4">
                                <td  >
                                    <asp:Label ID="Label2" runat="server" Text="Clave Catastral:" CssClass="letraMediana"></asp:Label>                                    
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtClvCastatral" runat="server" CssClass="textGrande" MaxLength="12" placeholder="Clave Catastral" ></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender runat="server" ID="meeFiltro" Mask="9999-99-999-999" MaskType="Number" InputDirection="LeftToRight" TargetControlID="txtClvCastatral" />
                                    <%--<ajaxToolkit:MaskedEditExtender  runat="server" ID="meeFiltro" Mask="9999-99-999-999" MaskType="Number"  InputDirection="LeftToRight" TargetControlID="txtClvCastatral" />--%>
                                    
                                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="buscarClaveCatastral" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="txtClvCastatral" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                   <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^\d+$" ControlToValidate="txtClvCastatral" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1">
                                    <asp:Label ID="Label5" runat="server" Text="Propietario" CssClass="letraMediana" Enabled="true"></asp:Label>
                                    </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtContruyente" runat="server" CssClass="textExtraGrande" ReadOnly="true" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    <asp:Label ID="Label8" runat="server" CssClass="letraMediana" Text="Ubicación:"></asp:Label>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtCalle" runat="server" Width="700px" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    <asp:Label ID="Label3" runat="server" CssClass="letraMediana" Text="Detalles:"></asp:Label>
                                </td>
                                <td >
                                    <asp:Label ID="Label4" runat="server" CssClass="letraMediana" Text="Superficie:"></asp:Label><br />
                                    <asp:TextBox ID="txtSuperficie" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                    <asp:Label ID="Label13" runat="server" CssClass="letraMediana" Text="m"></asp:Label>&#178;
                                </td>
                                <td >
                                    <asp:Label ID="Label6" runat="server" CssClass="letraMediana" Text="Frente:"></asp:Label><br />
                                    <asp:TextBox ID="txtFrente" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label7" runat="server" CssClass="letraMediana" Text="Uso de Suelo:"></asp:Label><br />
                                    <asp:TextBox ID="txtUso" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label21" runat="server" CssClass="letraMediana" Text="Estatus:"></asp:Label><br />
                                    <asp:TextBox ID="txtEstatus" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <br />
                                </td>
                            </tr>
                            <tr style="background-color: #b4b4b4">
                                <td colspan="6">
                                    <asp:Label ID="Label14" runat="server" Text="Periodo de Pagos" CssClass="letraMediana" Font-Size="Medium"></asp:Label>                                    
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    <asp:Label ID="Label15" runat="server" CssClass="letraMediana" Text="Impuesto Predial:"></asp:Label>
                                </td>
                                <td >
                                    <asp:Label ID="Label18" runat="server" CssClass="letraMediana" Text="Bimestre:"></asp:Label><br />
                                    <asp:DropDownList ID="ddlBimestreIP" runat="server">
                                        <asp:ListItem Text="SELECCIONAR BIMESTRE" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="1 - Enero-Febrero" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2 - Marzo-Abril" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3 - Mayo-Junio" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4 - Julio-Agosto" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5 - Septiembre-Octubre" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6 - Noviembre-Diciembre" Value="6"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="valida" ControlToValidate="ddlBimestreIP" ErrorMessage="*" SetFocusOnError="True" InitialValue="0" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="Label16" runat="server" CssClass="letraMediana" Text="Ultimo Año:"></asp:Label><br />
                                    <asp:TextBox ID="txtaaFinalIP" runat="server" CssClass="textMediano" MaxLength="4"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator19" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^\d+$" ControlToValidate="txtaaFinalIP" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="Label17" runat="server" CssClass="letraMediana" Text="Tipo Fase:"></asp:Label><br />
                                    <asp:DropDownList ID="ddlTipoFaseIP" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    <asp:Label ID="Label19" runat="server" CssClass="letraMediana" Text="SERVICIOS MUNICIPALES:" Visible="False"></asp:Label>
                                </td>
                                <td >
                                    <asp:Label ID="Label23" runat="server" CssClass="letraMediana" Text="Bimestre:" Visible="False"></asp:Label><br />
                                    <asp:DropDownList ID="ddlBimestreSM" runat="server" Visible="False">
                                        <asp:ListItem Text="SELECCIONAR BIMESTRE" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="1 - Enero-Febrero" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2 - Marzo-Abril" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3 - Mayo-Junio" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4 - Julio-Agosto" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5 - Septiembre-Octubre" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6 - Noviembre-Diciembre" Value="6"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" ControlToValidate="ddlBimestreSM" ErrorMessage="*" SetFocusOnError="True" InitialValue="0" ValidationGroup="guardar" Visible="False"></asp:RequiredFieldValidator>
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="Label20" runat="server" CssClass="letraMediana" Text="Ultimo Año:" Visible="False"></asp:Label><br />
                                    <asp:TextBox ID="txtaaFinalSM" runat="server" CssClass="textMediano" MaxLength="4" Visible="False"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^\d+$" ControlToValidate="txtaaFinalSM" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small" Visible="False"></asp:RegularExpressionValidator>
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="Label22" runat="server" CssClass="letraMediana" Text="Tipo Fase:" Visible="False"></asp:Label><br />
                                    <asp:DropDownList ID="ddlTipoFaseSM" runat="server" Visible="False"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="text-align:center">
                                    <asp:Button ID="btnGuardar" runat="server" CausesValidation="False" Text="Guardar" OnClick="btnGuardar_Click"/>
                                </td>
                                <td colspan="3" style="text-align:center">
                                    <asp:Button ID="btnLimpiar" runat="server" CausesValidation="False" Text="Limpiar" OnClick="btnLimpiar_Click"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />

            <asp:Literal ID="Literal1" runat="server"></asp:Literal>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
