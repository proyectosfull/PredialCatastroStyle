<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="catFIELCapMod.aspx.cs" Inherits="Catastro.Catalogos.catFIELCapMod" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lblTitulo" runat="server" Text="Alta Fiel" CssClass="letraTitulo"></asp:Label>

                    </td>
                </tr>
                <tr>
                    <td style="height: 2px">
                        <hr />
                        <asp:HiddenField ID="hdfId" runat="server" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <table>
                    <tr>
                        <td>
                            <asp:Label Text="Nombre:" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label Text="RFC:" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label Text="Calle:" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRFC" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox>

                        </td>
                        <td>
                            <asp:TextBox ID="txtCalle" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox><br />
                            </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="txtRFC" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" CssClass="valida" ControlToValidate="txtCalle" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td
                    </tr>
                    <tr>
                        <td>
                            <asp:Label Text="No Exterior:" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label Text="No Interior:" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label Text="Colonia:" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtNoExterior" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoInterior" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtColonia" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label Text="Código Postal:" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label Text="Localidad:" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label Text="Municipio:" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtCodigoPostal" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtLocalidad" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMunicipio" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox>
                       </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" CssClass="valida" ControlToValidate="txtCodigoPostal" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                        <td></td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="valida" ControlToValidate="txtMunicipio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label Text="Estado:" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label Text="Páis:" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label Text="Referencia:" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtEstado" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtPais" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtReferencia" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                            <tr>
                                <td>
                                    
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="valida" ControlToValidate="txtEstado" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" CssClass="valida" ControlToValidate="txtPais" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                </td>
                                <td></td>
                            </tr>
                    <tr>
                        <td>
                         <asp:Label Text="Archivo KEY:" runat="server"></asp:Label>
                        </td>
                        <td>
                               
                        </td>
                        <td>
                               <asp:Label ID="lblKeyPass" Text="KeyPass" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                       
                        <td colspan="2">
                            <asp:FileUpload ID="fileKey" runat="server" AllowMultiple="false" />

<asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="fileKey"
    runat="server" Display="Dynamic" CssClass="valida" ValidationGroup="guardar" />
<asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.key|.KEY)$"
    ControlToValidate="fileKey" runat="server" CssClass="valida" ErrorMessage="*Solo se permiten archivos KEY."
    Display="Dynamic" ValidationGroup="guardar"/>
                        </td>
                         <td>
                        <asp:TextBox ID="txtKeyPass" runat="server" CssClass="textMediano" MaxLength="50" Width="250px" TextMode="Password"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtKeyPass" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            <asp:Label Text="Archivo CER:" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:FileUpload ID="fileCer" runat="server" AllowMultiple="false" />
                            <br />
<asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="fileCer"
    runat="server" Display="Dynamic" CssClass="valida" ValidationGroup="guardar"/>
<asp:RegularExpressionValidator ID="RegularExpressionValidator2" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.cer|.CER)$"
    ControlToValidate="fileCer" runat="server" CssClass="valida" ErrorMessage="*Solo se permiten archivos CER."
    Display="Dynamic" ValidationGroup="guardar"/>
                        </td>

                    </tr>

                    <tr>
                        <td>
                            <asp:Label Text="Logo:" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:FileUpload ID="fileLogo" runat="server" AllowMultiple="false" />
                            <br />
<asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="fileLogo"
    runat="server" Display="Dynamic" CssClass="valida" ValidationGroup="guardar"/>
<asp:RegularExpressionValidator ID="RegularExpressionValidator3" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.png|.PNG)$"
    ControlToValidate="fileLogo" runat="server" CssClass="valida" ErrorMessage="*Solo se permiten archivos de png."
    Display="Dynamic" ValidationGroup="guardar" />
                        </td>
                    </tr>
                        <caption>
                            <br />
                            <tr>
                                <td>
                                    <asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" Text="Guardar" ValidationGroup="guardar" />
                                    <asp:Button ID="btnCancelar" runat="server" CausesValidation="False" OnClick="btnCancelar_Click" Text="Cancelar" />
                                </td>
                            </tr>
                        </caption>
                            
            </table>
            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />

            <asp:Panel ID="pnl" runat="server" class="formPanel">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="height: 60px;">
                            <asp:Label ID="Label1" runat="server" Text="FIEL" CssClass="letraTitulo"></asp:Label></td>
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
                                    <td>
                                        <%--<asp:DropDownList ID="ddlFiltro" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" />--%>
                                    &nbsp;<asp:TextBox ID="txtFiltro" runat="server" CssClass="textGrande"></asp:TextBox>
                                        &nbsp;&nbsp;&nbsp;
                                 <%--  <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="False"
                                        ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" /> --%>
                                        <%--<asp:Button ID="btnCancelar" runat="server" OnClick="btnCancelarContribuyente_Click"
                                        Text="Cancelar" CausesValidation="False" />                --%>                   
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="pnl_Modal" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnl" TargetControlID="btnPnl">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnPnl" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger  ControlID="btnGuardar"/>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
