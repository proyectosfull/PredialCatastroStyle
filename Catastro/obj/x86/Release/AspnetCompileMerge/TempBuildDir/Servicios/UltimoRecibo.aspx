<%@ Page Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="UltimoRecibo.aspx.cs" Inherits="Catastro.Servicios.UltimoRecibo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="lblTitulo" runat="server" CssClass="textModalTitulo2" Text="Reimpresión último recibo"></asp:Label>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 70%">
                        <table style="width: 100%">                           
                            <tr style="background-color: #b4b4b4">
                                <td  >
                                    <asp:Label ID="Label2" runat="server" Text="Folio Recibo:" CssClass="letraMediana"></asp:Label>                                    
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtFolio" runat="server" CssClass="textGrande" MaxLength="12" placeholder="Folio recibo" AutoPostBack="true"  ></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="txtFolio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                     <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Ingresar solo números enteros" ValidationExpression="^\d+$" ControlToValidate="txtFolio" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1">
                                    <asp:Label ID="Label18" runat="server" CssClass="letraMediana" Text="Motivo Cancelación:"></asp:Label>
                                    </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtObservacion" runat="server" CssClass="textMultiExtraGrande" Height="84px" MaxLength="200" placeholder="Motivo de cancelación." TextMode="MultiLine" Width="599px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="3" style="text-align:center">
                                    <asp:Button ID="btnLimpiar" runat="server" CausesValidation="False" Text="Recargar"/>
                                </td>
                                <td colspan="3" style="text-align:center">
                                    <asp:Button ID="btnGuardar" runat="server" CausesValidation="False" Text="Cancelar"/>
                                </td>
                            </tr>
                        </table>
                           
                            <tr>
                                <td colspan="6">
                                    <br />
                                </td>
                            </tr>
                            <tr style="background-color: #b4b4b4">
                                <td colspan="6">
                                    &nbsp;</td>
                            </tr>    
                    </td>
                </tr>
            </table>

            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />

            <asp:Literal ID="Literal1" runat="server"></asp:Literal>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
