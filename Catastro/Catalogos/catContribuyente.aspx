<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="catContribuyente.aspx.cs" Inherits="Catastro.Catalogos.catContribuyente" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
               
                <tr>
                    <td>
                        <asp:Label ID="lblTitulo" runat="server" Text="Alta contribuyente" CssClass="letraTitulo"></asp:Label>
                        <asp:RadioButtonList ID="rbltipoPersona" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rbltipoPersona_SelectedIndexChanged" RepeatDirection="Horizontal" style="margin-bottom: 0px" Visible="False">
                            <asp:ListItem Selected="True">Física</asp:ListItem>     <asp:ListItem>Moral</asp:ListItem>
                        </asp:RadioButtonList>  <asp:HiddenField ID="hdfId" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 2px">
                        <hr />                        
                    </td>
                </tr>

                <tr>
                    <td >
                        <table>
                            <tr>
                                <td style="width: 323px"><asp:Label ID="lblApellidoP" runat="server" Text="Apellido Paterno:" CssClass="letraMediana"></asp:Label></td>
                                <td style="width: 381px">
                                    <asp:Label ID="lblApellidoM" runat="server" CssClass="letraMediana" Text="Apellido Materno:"></asp:Label>
                                </td>
                                <td style="width: 316px">
                                    <asp:Label ID="lblNombre" runat="server" CssClass="letraMediana" Text="Nombre:"></asp:Label>
                                </td>
                            </tr>
                            <tr>                               
                                <td style="width: 323px">
                                    <asp:TextBox ID="txtApellidoP" runat="server" CssClass="textGrande" placeholder="Apellido Paterno:" MaxLength="100"></asp:TextBox>
                                </td>
                                <td style="width: 381px">
                                    <asp:TextBox ID="txtApellidoM" runat="server" CssClass="textGrande" placeholder="Apellido Paterno." MaxLength="100"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" CssClass="valida" ControlToValidate="txtApellidoM" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                                    
                                </td>
                                <td style="width: 316px">
                                     <asp:TextBox ID="txtNombre" runat="server" CssClass="textGrande" placeholder="Nombre." MaxLength="100"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" ControlToValidate="txtNombre" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                                </td>                                
                            </tr>
                            <tr>
                                <td colspan="3"  >
                                    <asp:Label ID="lblRazonSocial" runat="server" CssClass="letraMediana" Text="Razón Social:"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtRazon" runat="server" CssClass="textExtraGrande" placeholder="Razón Social" MaxLength="100" Width="842px"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="valida" ControlToValidate="txtRazon" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 323px">
                                    <asp:Label ID="lblCalle" runat="server" CssClass="letraMediana" Text="Calle:"></asp:Label>
                                </td>
                                <td style="width: 381px">
                                   
                                    <asp:Label ID="lblNumero" runat="server" CssClass="letraMediana" Text="Número:"></asp:Label>
                                   
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 323px">
                                    <asp:TextBox ID="txtCalle" runat="server" CssClass="textGrande" MaxLength="100" placeholder="Calle."></asp:TextBox>
                                </td>
                                <td style="width: 381px">
                                    <asp:TextBox ID="txtNumero" runat="server" CssClass="textGrande" MaxLength="50" placeholder="Número."></asp:TextBox>
                                </td>
                                <td style="width: 316px">

                                </td>
                            </tr>
                            <tr>
                                <td style="width: 323px">
                                    <asp:Label ID="lblColonia" runat="server" CssClass="letraMediana" Text="Colonia:"></asp:Label>
                                </td>
                                <td style="width: 381px">
                                    <asp:Label ID="lblLocalidad" runat="server" CssClass="letraMediana" Text="Localidad:"></asp:Label>
                                </td>
                                <td style="width: 316px">
                                    <asp:Label ID="lblMunicipio" runat="server" CssClass="letraMediana" Text="Municipio:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 323px; height: 63px;">
                                    <asp:TextBox ID="txtColonia" runat="server" CssClass="textGrande" MaxLength="100" placeholder="Colonia."></asp:TextBox>
                                </td>
                                <td style="height: 63px; width: 381px;">
                                    <asp:TextBox ID="txtLocalidad" runat="server" CssClass="textGrande" MaxLength="80" placeholder="Localidad."></asp:TextBox>
                                </td>
                                <td style="width: 316px; height: 63px;">
                                    <asp:TextBox ID="txtMunicipio" runat="server" CssClass="textGrande" MaxLength="80" placeholder="Municipio."></asp:TextBox>
                                </td>
                    
                            </tr>
                            <tr>
                                <td style="width: 323px">
                                    <asp:Label ID="lblEstado" runat="server" CssClass="letraMediana" Text="Estado:"></asp:Label>
                                </td>
                                <td style="width: 381px">
                                    <asp:Label ID="lblCP" runat="server" CssClass="letraMediana" Text="Código Postal:"></asp:Label>
                                </td>
                                <td>

                                    <asp:Label ID="lbTelefono" runat="server" CssClass="letraMediana" Text="Teléfono:"></asp:Label>

                                </td>
                            </tr>
                            <tr>
                                <td style="width: 323px">
                                    <asp:TextBox ID="txtEstado" runat="server" CssClass="textGrande" MaxLength="80" placeholder="Estado."></asp:TextBox>
                                </td>
                                <td style="width: 381px">
                                    <asp:TextBox ID="txtCP" runat="server" CssClass="textGrande" MaxLength="5" placeholder="Código Postal."></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender runat="server" ID="MaskedEditExtender2" Mask="99999" TargetControlID="txtCP" MaskType="Number" InputDirection="RightToLeft" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="textGrande" MaxLength="10" placeholder="Teléfono."></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                <td colspan="3"  >
                                    <asp:Label ID="Label2" runat="server" CssClass="letraMediana" Text="Referencia:"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtReferencia" runat="server" CssClass="textExtraGrande" placeholder="Referencia" MaxLength="100" Width="842px"></asp:TextBox>
                                 </td>
                            </tr>
                            <tr>
                                <td colspan="3" >
                                    <asp:Label ID="lblEmail" runat="server" CssClass="letraMediana" Text="Email:"></asp:Label>
                                
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="textExtraGrande" MaxLength="100" placeholder="Email."></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtEmail" CssClass="valida" ErrorMessage="Formato de correo incorrecto" Font-Size="Small" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 323px">
                                    <asp:Label ID="lblCurp" runat="server" CssClass="letraMediana" Text="CURP:"></asp:Label>
                                </td>
                                <td style="width: 381px">
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" CssClass="valida" ControlToValidate="txtCP" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                                    
                                    <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^\d+$" ControlToValidate="txtCP" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 323px">
                                    <asp:TextBox ID="txtCurp" runat="server" CssClass="textGrande" MaxLength="18" placeholder="CURP."></asp:TextBox>
                                </td>
                                <td style="width: 381px">
                                    <asp:CheckBox ID="chbAdultoMayor" runat="server" />
                                    <asp:Label ID="Label1" runat="server" CssClass="letraMediana" Text="Adulto Mayor:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 323px">
                                    &nbsp;</td>
                                <td style="width: 381px">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 323px">
                                    &nbsp;</td>
                                <td style="width: 381px">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 323px">
                                    &nbsp;</td>
                                <td style="width: 381px">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 323px">
                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="guardar" OnClick="btnGuardar_Click" />
                                    <asp:Button ID="btnCancelar" runat="server" CausesValidation="False" Text="Regresar" OnClick="btnCancelar_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>



            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
