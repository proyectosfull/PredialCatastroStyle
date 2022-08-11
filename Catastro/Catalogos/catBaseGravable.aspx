<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="catBaseGravable.aspx.cs" Inherits="Catastro.Catalogos.catBaseGravable" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 40px;"></td>
                </tr>
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="lblTitulo" runat="server" CssClass="textModalTitulo2" Text="Alta de Base Gravable"></asp:Label>

                    </td>
                </tr>
                <tr>
                    <td style="height: 2px">
                        <asp:HiddenField ID="hdfId" runat="server" />
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="padding: 0px 10px 10px 10px;">
                        <table>
                            <tr>
                                <td style="font-size: 12px; color: #666666">
                                    <span style="font-weight: bold">Clave Catastral:</span></td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtClavePredial" runat="server" CssClass="textGrande" placeholder="Clave Predial." MaxLength="15"></asp:TextBox>
                                    <%--<ajaxToolkit:MaskedEditExtender  runat="server" ID="meeFiltro" Mask="9999-99-999-999" TargetControlID="txtClavePredial" />--%>
                                    <ajaxToolkit:MaskedEditExtender runat="server" ID="meeFiltro" Mask="9999-99-999-999" TargetControlID="txtClavePredial" MaskType="Number" InputDirection="RightToLeft" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" ControlToValidate="txtClavePredial" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                    <asp:ImageButton ID="imbBuscarPredio" runat="server" CausesValidation="False"
                                        ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscarPredio_Click" />
                                    &nbsp;
                                    <asp:Label ID="lblCuentaPredial" runat="server" CssClass="letraMediana"></asp:Label>
                                    &nbsp;&nbsp;
                                    <asp:Label ID="lblNombrePredio" runat="server" CssClass="letraMediana" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblFechaEvaluo" runat="server" Text="Fecha Evaluo:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaEvaluo" runat="server" CssClass="textGrande" placeholder="Fecha Evaluo."></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="txtFechaEvaluo_MaskedEditExtender" runat="server" BehaviorID="txtFechaEvaluo_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaEvaluo" />
                                    <ajaxToolkit:CalendarExtender ID="txtFechaEvaluo_CalendarExtender" runat="server" BehaviorID="txtFechaEvaluo_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFechaEvaluo" Format="dd/MM/yyyy" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" CssClass="valida" ControlToValidate="txtFechaEvaluo" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                </td>
                                <td colspan="2"></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblNombre" runat="server" Text="Ejercicio:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEjercicio" runat="server" CssClass="textGrande" MaxLength="4" placeholder="Ejercicio."></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" CssClass="valida" ControlToValidate="txtEjercicio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^\d+$" ControlToValidate="txtEjercicio" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                </td>
                                <td>
                                    <asp:Label ID="lblBimestre" runat="server" Text="Bimestre:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBimestre" runat="server">
                                        <asp:ListItem Text="1 - Enero-Febrero" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2 - Marzo-Abril" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3 - Mayo-Junio" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4 - Julio-Agosto" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5 - Septiembre-Octubre" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6 - Noviembre-Diciembre" Value="6" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSuperTerreno" runat="server" Text="Superficie del Terreno:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSuperTerreno" runat="server" CssClass="textGrande" placeholder="Superficie del Terreno." MaxLength="15"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" CssClass="valida" ControlToValidate="txtSuperTerreno" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="\d+(\.\d{1,2})?|\.\d{1,2}" ControlToValidate="txtSuperTerreno" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                </td>
                                <td>
                                    <asp:Label ID="lblSuperficeConstruccion" runat="server" Text="Superficie Construcción:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSuperficeConstruccion" runat="server" CssClass="textGrande" placeholder="Superficie Construcción." MaxLength="15"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" CssClass="valida" ControlToValidate="txtSuperficeConstruccion" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="\d+(\.\d{1,2})?|\.\d{1,2}" ControlToValidate="txtSuperficeConstruccion" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblTerrenoPrivativo" runat="server" Text="Terreno Privativo:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTerrenoPrivativo" runat="server" CssClass="textGrande" placeholder="Terreno Privativo." MaxLength="10"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" CssClass="valida" ControlToValidate="txtTerrenoPrivativo" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="\d+(\.\d{1,2})?|\.\d{1,2}" ControlToValidate="txtTerrenoPrivativo" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                </td>
                                <td>
                                    <asp:Label ID="lblConstruccionPrivativa" runat="server" Text="Construcción Privativa:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConstruccionPrivativa" runat="server" CssClass="textGrande" placeholder="Construcción Privativa." MaxLength="15"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" CssClass="valida" ControlToValidate="txtConstruccionPrivativa" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="\d+(\.\d{1,2})?|\.\d{1,2}" ControlToValidate="txtConstruccionPrivativa" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                </td>

                            </tr>
                            <tr>

                                <td>
                                    <asp:Label ID="lblTerrenoComun" runat="server" Text="Terreno Común:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTerrenoComun" runat="server" CssClass="textGrande" placeholder="Terreno Común." MaxLength="15"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" CssClass="valida" ControlToValidate="txtTerrenoComun" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="\d+(\.\d{1,2})?|\.\d{1,2}" ControlToValidate="txtTerrenoComun" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                </td>
                                <td>
                                    <asp:Label ID="lblConstruccionComun" runat="server" Text="Construcción Común:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConstruccionComun" runat="server" CssClass="textGrande" placeholder="Construcción Común." MaxLength="15"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" CssClass="valida" ControlToValidate="txtConstruccionComun" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="\d+(\.\d{1,2})?|\.\d{1,2}" ControlToValidate="txtConstruccionComun" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblValorTerreno" runat="server" Text="Valor Terreno:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtValorTerreno" runat="server" CssClass="textGrande" placeholder="Valor Terreno." MaxLength="15" OnTextChanged="txtValorTerreno_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" CssClass="valida" ControlToValidate="txtValorTerreno" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="\d+(\.\d{1,2})?|\.\d{1,2}" ControlToValidate="txtValorTerreno" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                </td>
                                <td>
                                    <asp:Label ID="lblValorConstruccion" runat="server" Text="Valor Construcción:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtValorConstruccion" runat="server" CssClass="textGrande" placeholder="Valor Construcción." MaxLength="15" OnTextChanged="txtValorTerreno_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="txtValorConstruccion" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="\d+(\.\d{1,2})?|\.\d{1,2}" ControlToValidate="txtValorConstruccion" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblValor" runat="server" Text="Base Gravable:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtValor" runat="server" CssClass="letraTituloEmptyGrid" placeholder="Valor." MaxLength="11" onkeydown="return false;" BorderStyle="None"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="valida" ControlToValidate="txtValor" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo ocho números enteros y dos decimales." ValidationExpression="\d{0,8}(\.\d{1,2})?|\.\d{1,2}" ControlToValidate="txtValor" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                --%></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblValorConstruccionComun" runat="server" Text="Valor Construcción Común:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtValorConstruccionComun" runat="server" CssClass="textGrande" placeholder="Valor Construcción Común." MaxLength="15"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" CssClass="valida" ControlToValidate="txtValorConstruccionComun" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="\d+(\.\d{1,2})?|\.\d{1,2}" ControlToValidate="txtValorConstruccionComun" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                </td>
                                <td>
                                    <asp:Label ID="lblValorConstruccionPrivativa" runat="server" Text="Valor Construcción Privativa:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtValorConstruccionPrivativa" runat="server" CssClass="textGrande" placeholder="Valor Construcción Privativa." MaxLength="15"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="valida" ControlToValidate="txtValorConstruccionPrivativa" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="\d+(\.\d{1,2})?|\.\d{1,2}" ControlToValidate="txtValorConstruccionPrivativa" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPrototipo" runat="server" Text="Prototipo:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPrototipo" runat="server" CssClass="textGrande" placeholder="Prototipo." MaxLength="10"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" CssClass="valida" ControlToValidate="txtPrototipo" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="\d+(\.\d{1,2})?|\.\d{1,2}" ControlToValidate="txtPrototipo" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                </td>
                            </tr>                            
                            <tr>
                                <td colspan="4">
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
    <script type="text/javascript">
        //$(document).ready(function () {
        //    $('#MainContent_txtValorConstruccion').change(function () {
        //        if ($.isNumeric($('#MainContent_txtValorConstruccion').val()) && $.isNumeric($('#MainContent_txtValorTerreno').val())) {
        //            var suma = parseFloat($('#MainContent_txtValorConstruccion').val()) + parseFloat($('#MainContent_txtValorTerreno').val());
        //            $('#MainContent_txtValor').val(suma);
        //        }
        //    });
        //    $('#MainContent_txtValorTerreno').change(function () {
        //        if ($.isNumeric($('#MainContent_txtValorConstruccion').val()) && $.isNumeric($('#MainContent_txtValorTerreno').val())) {
        //            var suma = parseFloat($('#MainContent_txtValorConstruccion').val()) + parseFloat($('#MainContent_txtValorTerreno').val());
        //            $('#MainContent_txtValor').val(suma);
        //        }
        //    });
        //});

    </script>
</asp:Content>
