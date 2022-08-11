<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModalPopupMensaje.ascx.cs" Inherits="Catastro.Controles.ModalPopupMensaje" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:UpdatePanel ID="UpdatePanelPopup" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlMensaje" runat="server" Width="474PX" CssClass="zindexModal">
            <table border="0" cellspacing="0" width="474px" align="center">
                <tr>
                    <td class="topModal" colspan="3">
                        <table border="0" cellspacing="0" width="100%" align="center" style="height: 99%">
                            <tr valign="bottom">
                                <td class="tdTituloModal">
                                    <asp:Label ID="lblAviso" runat="server" CssClass="textModalTitulo" Text="Aviso"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                
                <tr>
                    <td class="leftModal"></td>
                    <td class="mainModal">
                        <br />
                        <asp:Label ID="lblMensaje" runat="server" CssClass="textModalDescrip"></asp:Label><br />
                        <br />

                        <asp:Button ID="btnAceptarMensaje" runat="server"
                            OnClick="btnAceptarMensaje_Click" Text="Aceptar" CausesValidation="False" />
                        &nbsp; &nbsp;&nbsp;
                    <asp:Button ID="btnCancelarMensaje" runat="server"
                        OnClick="btnCancelarMensaje_Click" Text="Cancelar" CausesValidation="False" />
                        <br />
                    </td>
                    <td class="rigthModal"></td>
                </tr>
                <tr>
                    <td class="bottomModal" colspan="3"></td>
                </tr>
            </table>
            <ajaxToolkit:ModalPopupExtender ID="mpeMensaje" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnlMensaje" TargetControlID="btnMensaje">                
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnMensaje" runat="server" />

        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
