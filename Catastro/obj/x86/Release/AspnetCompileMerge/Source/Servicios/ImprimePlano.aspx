<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImprimePlano.aspx.cs" Inherits="Catastro.Servicios.ImprimePlano" %>
<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
        <asp:PostBackTrigger ControlID="btnImprimir" />
        </Triggers>
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="Label1" runat="server" Text="Impresión de plano" CssClass="letraTitulo"></asp:Label></td>
                </tr>
                <tr>
                    <td style="height: 2px">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="padding: 0px 10px 10px 10px; background-color: #b4b4b4">
                        <table cellpadding="0" cellspacing="0" width="100%">
                           
                            <tr>
                                <td colspan="1">
                                    <asp:Label ID="Label2" runat="server" CssClass="letraMediana" Text="Clave Catastral:"></asp:Label>
                                </td>
                                <td colspan="1">
                                    <asp:TextBox ID="txtClvCastatral" runat="server" CssClass="textGrande" MaxLength="12"  placeholder="Clave Catastral" OnTextChanged="buscarClaveCatastral" AutoPostBack="true"></asp:TextBox>                                    
                                </td>
                                 <td colspan="1">
                                    <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="False" ImageUrl="~/img/consultar.fw.png" OnClick="buscarClaveCatastral" />                                   
                                </td>
                                <td colspan="1">                                    
                                </td>
                                <td colspan="1">                                    
                                </td>                               
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblMunicipioL" runat="server" CssClass="letraMediana" Text="Municipio"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblLocalidadL" runat="server" CssClass="letraMediana" Text="Localidad Colonia"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblUbicacionL" runat="server" CssClass="letraMediana" Text="Ubicación del predio"></asp:Label>
                                </td>                                
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblMunicipio" runat="server" CssClass="textGrande" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblLocalidad" runat="server" CssClass="textGrande" Text=""></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblUbicacion" runat="server" CssClass="textGrande" Text=""></asp:Label>
                                </td> 
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblNombreCausanteL" runat="server" CssClass="letraMediana" Text="Nombre del causante"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblDomicilioCausanteL" runat="server" CssClass="letraMediana" Text="Domicilio del causante"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblNombreCausante" runat="server" CssClass="textGrande" Text=""></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblDomicilioCausante" runat="server" CssClass="textGrande" Text=""></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Label ID="lblSuperficieTerrenoL" runat="server" CssClass="letraMediana" Text="Superficie terreno"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblValorTerrenoL" runat="server" CssClass="letraMediana" Text="Valor terreno"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblSuperficieConstruccionL" runat="server" CssClass="letraMediana" Text="Superficie construcción"></asp:Label>
                                </td>
                                 <td>
                                    <asp:Label ID="lblValorConstruccionL" runat="server" CssClass="letraMediana" Text="Valor construcción"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblValorTotalL" runat="server" CssClass="letraMediana" Text="Valor total"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSuperficieTerreno" runat="server" CssClass="textGrande" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblValorTerreno" runat="server" CssClass="textGrande" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblSuperficieConstruccion" runat="server" CssClass="textGrande" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblValorConstruccion" runat="server" CssClass="textGrande" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblValorTotal" runat="server" CssClass="textGrande" Text=""></asp:Label>
                                </td>
                            </tr>
                            <%--<tr>
                                <td>
                                    <asp:Label ID="lblTPrivativo" runat="server" CssClass="textGrande" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblTComun" runat="server" CssClass="textGrande" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCPrivativa" runat="server" CssClass="textGrande" Text=""></asp:Label>
                                </td>
                                 <td>
                                    <asp:Label ID="lblCComun" runat="server" CssClass="textGrande" Text=""></asp:Label>
                                </td>
                                <td>
                                    
                                </td>
                            </tr>--%>
                            <tr>
                                <td colspan="2" style="text-align: left">
                                    <br />
                                    <asp:Label ID="lblMensajeTPendiente" runat="server"
                                        CssClass="letraBannerLinks" ForeColor="Blue" Visible="False"></asp:Label></td>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" OnClick="btnImprimir_Click" Visible="false" /></td>
                                <td>
                                    <asp:Button ID="btnGenera" runat="server" CausesValidation="False" OnClick="btnGenera_Click" Text="Generar Tramite" Visible="False" /></td>
                            </tr>
                        </table>
                    </td>
                    
                    </tr>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <asp:Panel ID="pnlMapa" runat="server" Visible="false">   
    	        <asp:RadioButtonList ID="rblMapTools" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="0">Acercar</asp:ListItem>
                    <asp:ListItem Value="1">Alejar</asp:ListItem>
                    <asp:ListItem Value="2" Selected="True">Mover</asp:ListItem>
                </asp:RadioButtonList>
                <asp:ImageButton Width="500" Height="300" ID="imgMap" runat="server" OnClick="imgMap_Click" style="border: 1px solid #000;" />
                <asp:GridView ID="gv1" runat="server">
                </asp:GridView>
            </asp:Panel>
            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
