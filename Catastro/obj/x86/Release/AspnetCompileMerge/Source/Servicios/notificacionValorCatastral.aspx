<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="notificacionValorCatastral.aspx.cs" Inherits="Catastro.Servicios.notificacionValorCatastral" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
           function printPdf() {
               var PDF = document.getElementById("MainContent_frameRecibo");
               PDF.focus();
               PDF.contentWindow.print();
           }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="Label1" runat="server" Text="Notificación del Valor Catastral" CssClass="letraTitulo"></asp:Label></td>
                </tr>
                <tr>
                    <td style="height: 2px">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="padding: 0px 10px 10px 10px;">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr style="background-color: #b4b4b4">
                                <td colspan="1">
                                    <asp:Label ID="Label2" runat="server" CssClass="letraMediana" Text="Clave Catastral:"></asp:Label>
                                </td>
                                <td colspan="1">
                                    <asp:TextBox ID="txtClvCastatral" runat="server" CssClass="textGrande" MaxLength="12" AutoPostBack="true" placeholder="Clave Catastral"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender  runat="server" ID="meeFiltro" Mask="9999-99-999-999" MaskType="Number"  InputDirection="LeftToRight" TargetControlID="txtClvCastatral" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtClvCastatral" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="generarReporte"></asp:RequiredFieldValidator>
                                </td>
                                <td colspan="3">
                                    <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="False" ImageUrl="~/img/consultar.fw.png" OnClick="buscarClaveCatastral" />
                                </td>
                            </tr>
                            <tr><td colspan="5">&nbsp;</td></tr>
                            <tr>                                
                                <td colspan="5" style="border:groove; text-align:center">
                                    <asp:Label ID="Label3" runat="server" Text="DATOS DEL PROPIETARIO" CssClass="letraMediana" ForeColor="#000066" ></asp:Label>
                                </td>                                
                            </tr>  
                            <tr>                                
                                <td  style="border:groove; text-align:center">
                                    <asp:Label ID="Label4" runat="server" Text="CLAVE CATASTRAL" CssClass="letraMediana" ForeColor="#000066" ></asp:Label>
                                </td> 
                                <td  style="border:groove; text-align:center">
                                    <asp:Label ID="Label5" runat="server" Text="SUP. DEL TERRENO" CssClass="letraMediana" ForeColor="#000066" ></asp:Label>
                                </td>     
                                <td  style="border:groove; text-align:center">
                                    <asp:Label ID="Label6" runat="server" Text="VALOR DEL TERRRENO" CssClass="letraMediana" ForeColor="#000066" ></asp:Label>
                                </td>  
                                <td  style="border:groove; text-align:center">
                                    <asp:Label ID="Label7" runat="server" Text="SUP. CONSTRUCCIÓN" CssClass="letraMediana" ForeColor="#000066" ></asp:Label>
                                </td>  
                                <td  style="border:groove; text-align:center">
                                    <asp:Label ID="Label8" runat="server" Text="VALOR DE CONSTRUCCIÓN" CssClass="letraMediana" ForeColor="#000066" ></asp:Label>
                                </td>                          
                            </tr>  
                            <tr >                                
                                <td  style="border:groove; text-align:center">
                                    <asp:Label ID="lblClaveCastatral" runat="server" Text=" " CssClass="letraMediana" ReadOnly="true"></asp:Label>
                                </td> 
                                <td  style="border:groove; text-align:center">
                                    <asp:Label ID="lblSupTerreno" runat="server" Text=" " CssClass="letraMediana" ></asp:Label>
                                </td>     
                                <td  style="border:groove; text-align:center">
                                    <asp:Label ID="lblValorTerreno" runat="server" Text=" " CssClass="letraMediana" ></asp:Label>
                                </td>  
                                <td  style="border:groove; text-align:center">
                                    <asp:Label ID="lblSupConstruccion" runat="server" Text=" " CssClass="letraMediana" ></asp:Label>
                                </td>  
                                <td  style="border:groove; text-align:center">
                                    <asp:Label ID="lblValorConstruccion" runat="server" Text=" " CssClass="letraMediana" ></asp:Label>
                                </td>                          
                            </tr>    
                             <tr>                                
                                <td colspan="2" style="border:groove; text-align:center">
                                    <asp:Label ID="Label9" runat="server" Text="UBICACIÓN" CssClass="letraMediana" ForeColor="#000066" ></asp:Label>
                                </td> 
                                <td colspan="2" style="border:groove; text-align:center">
                                    <asp:Label ID="Label10" runat="server" Text="LOCALIDAD" CssClass="letraMediana" ForeColor="#000066" ></asp:Label>
                                </td>     
                                <td colspan="1"  style="border:groove; text-align:center">
                                    <asp:Label ID="Label11" runat="server" Text="MUNICIPIO" CssClass="letraMediana" ForeColor="#000066" ></asp:Label>
                                </td>                                                            
                            </tr>                                                             
                                <tr>
                                    <td colspan="2" style="border:groove; text-align:center">
                                        <asp:Label ID="lblUbicacion" runat="server" CssClass="letraMediana" Text=" "></asp:Label>
                                    </td>
                                    <td colspan="2" style="border:groove; text-align:center">
                                        <asp:Label ID="lblLocalidad" runat="server" CssClass="letraMediana" Text=" "></asp:Label>
                                    </td>
                                    <td colspan="1" style="border:groove; text-align:center">
                                        <asp:Label ID="lblMunicipio" runat="server" CssClass="letraMediana" Text=" "></asp:Label>
                                    </td>
                            </tr>
                            <tr>
                                <td colspan="5" style="border:groove; text-align:center">
                                    <asp:Label ID="lblReferencia" runat="server" CssClass="letraMediana" ForeColor="#000066" Text="REFERENCIA" Width="1083px"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="border:groove; text-align:center">
                                    <asp:Label ID="Label12" runat="server" CssClass="letraMediana" ForeColor="#000066" Text="NOMBRE DEL CONTRIBUYENTE"></asp:Label>
                                </td>
                                <td colspan="2" style="border:groove; text-align:center">
                                    <asp:Label ID="Label13" runat="server" CssClass="letraMediana" ForeColor="#000066" Text="DOMICILIO DEL CONTRIBUYUNTE"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="border:groove; text-align:center">
                                    <asp:Label ID="lblNombre" runat="server" CssClass="letraMediana" Text=" "></asp:Label>
                                </td>
                                <td colspan="2" style="border:groove; text-align:center">
                                    <asp:Label ID="lblDomicilio" runat="server" CssClass="letraMediana" Text=" "></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">&nbsp;</td>
                            </tr>
                            <tr style="border:groove;">
                                <td colspan="2" style=" text-align:center">
                                    <asp:Label ID="Label14" runat="server" CssClass="letraMediana" ForeColor="#000066" Text="VALOR CATASTRAL:"></asp:Label>
                                </td>
                                <td colspan="2" style="text-align:center">
                                    <asp:Label ID="Label15" runat="server" CssClass="letraMediana" ForeColor="#000066" Text="VALOR TOTAL"></asp:Label>
                                </td>
                                <td colspan="1" style="text-align:center">
                                    <asp:Label ID="lblValorCatastral" runat="server" CssClass="letraMediana" Text=" "></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="5" style=" text-align:left">
                                    <asp:Label ID="Label16" runat="server" CssClass="letraMediana" ForeColor="#000066" Text="OBSERVACIONES:"></asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtObservaciones" runat="server" CssClass="textMultiExtraGrande" placeholder="Observaciones" ReadOnly="true" TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtObservaciones" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="generarReporte"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="1" style=" text-align:center">
                                    <asp:Label ID="Label17" runat="server" CssClass="letraMediana" ForeColor="#000066" Text="VALOR AUMENTADO:"></asp:Label>
                                </td>
                                <td colspan="1" style=" text-align:center">
                                    <asp:TextBox ID="txtValorAumentado" runat="server" CssClass="textExtraGrande" placeholder="VALOR AUMENTADO" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td colspan="1" style=" text-align:right">
                                    <asp:Label ID="Label18" runat="server" CssClass="letraMediana" ForeColor="#000066" Text="SUP. DE AUMENTO. M2:"></asp:Label>
                                </td>
                                <td colspan="2" style=" text-align:center">
                                    <asp:TextBox ID="txtSubAumento" runat="server" CssClass="textExtraGrande" placeholder="SUP. DE AUMENTO. M2" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="1" style=" text-align:center">
                                    <asp:Label ID="Label19" runat="server" CssClass="letraMediana" ForeColor="#000066" Text="EN EL PODER DE:"></asp:Label>
                                </td>
                                <td colspan="1" style=" text-align:center">
                                    <asp:TextBox ID="txtEnPoder" runat="server" CssClass="textExtraGrande" placeholder="EN EL PODER DE " ReadOnly="true"></asp:TextBox>
                                </td>
                                <td colspan="1" style=" text-align:right">
                                    <asp:Label ID="Label20" runat="server" CssClass="letraMediana" ForeColor="#000066" Text=" QUIÉN DIJO SER:"></asp:Label>
                                </td>
                                <td colspan="2" style=" text-align:center">
                                    <asp:TextBox ID="txtQuienDijo" runat="server" CssClass="textExtraGrande" placeholder=" QUIÉN DIJO SER" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="1" style=" text-align:center">
                                    <asp:Label ID="Label21" runat="server" CssClass="letraMediana" ForeColor="#000066" Text="NOTIFICADOR:"></asp:Label>
                                </td>
                                <td colspan="1" style=" text-align:center">
                                    <asp:TextBox ID="txtNotificador" runat="server" CssClass="textExtraGrande" placeholder="NOTIFICADOR " ReadOnly="true"></asp:TextBox>
                                </td>
                                <td colspan="3">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="5">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: left">
                                    <asp:Button ID="btnGenerarReporte" runat="server" OnClick="btnGenerarReporte_Click" Text="Generar Notificación" ValidationGroup="generarReporte" />
                                </td>
                                <td colspan="3" style="text-align: right">
                                    <asp:Button ID="btnRecargar" runat="server" CausesValidation="False" OnClick="btnRecargar_Click" Text="Recargar" />
                                </td>
                            </tr>
                            </tr>   
                        </table>
             <asp:Panel ID="pnlRecibo" runat="server" Width="800px" Height="500px" BackColor="White">
                <div class="width:100%;margin:1px;">
                    <asp:ImageButton ID="imCerrarRecibo" runat="server" ImageUrl="~/Img/eliminar.png" ImageAlign="Right" OnClick="imCerrarRecibo_Click" />
                </div>
                <iframe id="frameRecibo" runat="server" src="" width="100%" height="100%" style="border: none;" />
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="modalRecibo" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnlRecibo" TargetControlID="btnRecibo">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnRecibo" runat="server" />
            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
