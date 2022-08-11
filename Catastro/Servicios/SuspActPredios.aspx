<%@ Page Title="Suspensión - Activación de predios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SuspActPredios.aspx.cs" Inherits="Catastro.Catalogos.SuspActPredios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="lblTitulo" runat="server" CssClass="textModalTitulo2" Text="Suspensión, Activación y Comentarios de predios"></asp:Label>
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
                                    <asp:Button ID="btnBuscarClave" OnClick="buscarClaveCatastral" runat="server" Text="Buscar" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="txtClvCastatral" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
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
                                    <asp:TextBox ID="txtCalle" runat="server" CssClass="textExtraGrande" Width="100%" Enabled="false"></asp:TextBox>
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
                                    <asp:Label ID="Label14" runat="server" Text="Modificación de estatus y Comentarios" CssClass="letraMediana" Font-Size="Medium"></asp:Label>                                    
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    <asp:Label ID="Label15" runat="server" CssClass="letraMediana" Text="Estatus:"></asp:Label>
                                </td>
                                <td >
                                    <asp:Label ID="Label16" runat="server" CssClass="letraMediana" Text="Estatus Anterior:"></asp:Label><br />
                                    <asp:TextBox ID="txtEstatusAnt" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="Label17" runat="server" CssClass="letraMediana" Text="Nuevo Estatus:"></asp:Label><br />
                                    <asp:DropDownList ID="ddlEstatusNu" runat="server"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="valida" ErrorMessage="*" ControlToValidate="ddlEstatusNu" SetFocusOnError="true" ValidationGroup="guardar" InitialValue="0"></asp:RequiredFieldValidator>
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="Label18" runat="server" CssClass="letraMediana" Text="Comentarios:"></asp:Label><br />
                                    <asp:TextBox ID="txtObservacion" TextMode="MultiLine" runat="server" CssClass="textMultiExtraGrande" Width="330px" MaxLength="200" placeholder="Observación."></asp:TextBox>
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
