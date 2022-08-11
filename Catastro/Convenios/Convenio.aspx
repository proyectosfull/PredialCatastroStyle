<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Convenio.aspx.cs" Inherits="Catastro.Convenios.Convenio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
       
        function fnRecargo() {
            var par = document.getElementById("MainContent_txtParcialidades").value;
            if (par > 1 && par <= 12)
                document.getElementById("MainContent_txtRecargo").value = "1.0";
            else if ((par >= 13 && par < 24))
                document.getElementById("MainContent_txtRecargo").value = "1.25";
            else if ((par >= 24 && par <= 36))
                document.getElementById("MainContent_txtRecargo").value = "1.50";
            else
                document.getElementById("MainContent_txtRecargo").value = "";
        }


    </script>
    <style type="text/css"> 
    .ajax__calendar_inactive  {color:#00ffff;}
        .auto-style1 {
            width: 796px;
        }
        .auto-style2 {
            width: 263px;
            text-align: left;
        }
        .auto-style4 {
            width: 409px;
        }
        .auto-style6 {
            width: 158px;
        }
        .auto-style7 {
            font-size: 12px;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
         <Triggers>
        <asp:PostBackTrigger ControlID="btnImprime" />
        </Triggers>
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="lblTitulo" runat="server" CssClass="textModalTitulo2" Text="Captura Convenio"></asp:Label>
                        <asp:HiddenField ID="hdfId" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 2px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 70%">
                        <table style="width: 100%">
                            <tr style="background-color: #b4b4b4">
                                <td class="auto-style6">
                                    <asp:Label ID="lblClave" runat="server" Text="Clave Catastral:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>&nbsp;</td>
                                <td class="auto-style4">
                                    <asp:TextBox ID="txtClvCastatral" runat="server" CssClass="textGrande" MaxLength="12" placeholder="Clave Catastral"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="txtClvCastatral" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" CssClass="valida" ControlToValidate="txtClvCastatral" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" ValidationGroup="buscar" OnClick="btnBuscar_Click" Visible="False" />
                                    <asp:HiddenField ID="hdfIdCovenioEdoCta" runat="server" />
                                    <asp:HiddenField ID="hdfIdPredio" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblNombre" runat="server" Text="Nombre Contribuyente:" CssClass="letraMediana"></asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtNombreAdquiriente" runat="server" CssClass="auto-style7" MaxLength="100" placeholder="Nombre Contribuyente" Enabled="False" Width="663px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" ControlToValidate="txtNombreAdquiriente" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblTipoImpuesto" runat="server" Text="Tipo Impuesto:" CssClass="letraMediana"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblMonto" runat="server" CssClass="letraMediana" Text="Importe Total:"></asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtTipoImpuesto" runat="server" CssClass="textMediano" MaxLength="100" placeholder="Nombre Adquiriente" Enabled="False"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" CssClass="valida" ControlToValidate="txtTipoImpuesto" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:TextBox ID="txtMonto" runat="server" CssClass="textMediano" Enabled="false" MaxLength="8" placeholder="Importe Total"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtMonto" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                    <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtMonto" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d+(\.\d{2})?|\.\d{2}" ValidationGroup="guardar"></asp:RegularExpressionValidator>--%>
                                    &nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style6">
                                    <asp:Label ID="lblTelefono" runat="server" CssClass="letraMediana" Text="No. Teléfono"></asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtTelefono" runat="server" MaxLength="10" Width="265px"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" CssClass="valida" ControlToValidate="txtTelefono" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                                    <ajaxToolkit:MaskedEditExtender runat="server" ID="MaskedEditExtender3" Mask="9999999999" TargetControlID="txtTelefono" MaskType="Number" InputDirection="RightToLeft" />                                   
                                </td>
                                <td>
                                     <asp:Label ID="lblCelular" runat="server" CssClass="letraMediana" Text="No. Celular"></asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtCelular" runat="server" MaxLength="10" Width="265px"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" CssClass="valida" ControlToValidate="txtCelular" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                                    <ajaxToolkit:MaskedEditExtender runat="server" ID="MaskedEditExtender4" Mask="9999999999" TargetControlID="txtCelular" MaskType="Number" InputDirection="RightToLeft" />                                   
                                </td>                               
                            </tr
                             <tr>
                                <tr>
                                    <td class="auto-style6">
                                        <asp:Label ID="lblNoIdentificacion" runat="server" CssClass="letraMediana" Text="No. Identificación"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="txtNoIdentificacion" runat="server" MaxLength="18" Width="265px"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" CssClass="valida" ControlToValidate="txtNoIdentificacion" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                                    <td>
                                        <asp:Label ID="lblEmail" runat="server" CssClass="letraMediana" Text="Email"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" Width="265px"></asp:TextBox>
                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtEmail" CssClass="valida" ErrorMessage="Formato de correo incorrecto" Font-Size="Small" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="guardar"></asp:RegularExpressionValidator></td>--%>
                                    <td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblConvenio" runat="server" CssClass="letraMediana" Text="Folio del convenio"></asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtFolio" runat="server" MaxLength="20" Width="265px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtFolio" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style6">
                                    <asp:Label ID="lblFechaIncial" runat="server" CssClass="letraMediana" Text="Fecha Incial:"></asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtFechaInicial" runat="server" CssClass="textMediano" MaxLength="50" placeholder="Fecha Inicial"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" BehaviorID="txtFechaFinal_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaInicial" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="txtFechaInicial_CalendarExtender" CssClass="CalendarCSS" Format="dd/MM/yyyy" TargetControlID="txtFechaInicial" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtFechaInicial" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator62" runat="server" ControlToValidate="txtTipoImpuesto" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                    &nbsp;</td>
                                <td>
                                    <asp:Label ID="lblFechaFinal" runat="server" CssClass="letraMediana" Text="Fecha Final:"></asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtFechaFinal" runat="server" CssClass="textMediano" MaxLength="50" placeholder="Fecha Final"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" BehaviorID="txtFechaFinal_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaFinal" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" BehaviorID="txtFechaFinal_CalendarExtender" CssClass="CalendarCSS" Format="dd/MM/yyyy" TargetControlID="txtFechaFinal" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtFechaFinal" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblParcialidades" runat="server" CssClass="letraMediana" Text="Parcialidades:"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblRecargo" runat="server" CssClass="letraMediana" Text="Tasa de Recargo:"></asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtParcialidades" runat="server" CssClass="textMediano" MaxLength="2" OnBlur="fnRecargo();" placeholder="Parcialidades"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtParcialidades" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtRecargo" CssClass="valida" ErrorMessage="Parcialidades de 2 a 36" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtRecargo" runat="server" CssClass="textMediano" Enabled="False" MaxLength="5" placeholder="Tasa de Recargo"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="chkRecargo" runat="server" AutoPostBack="True" OnCheckedChanged="chkRecargo_CheckedChanged" Text="No Aplicar Recargo" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            </tr>
                            </tr>
                            <tr>
                                <td class="auto-style6">
                                    <asp:Button ID="btnCalcular" runat="server" OnClick="btnCalcular_Click" Text="Calcular Pagos" ValidationGroup="guardar" />
                                    <asp:Button ID="btnCancelar" runat="server" CausesValidation="False" OnClick="btnCancelar_Click" Text="Regresar" />
                                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" OnClick="btnCancel_Click" Text="Cancelar" Visible="False" />
                                </td>
                               
                            </tr>
                            <tr>
                                <td class="auto-style6">
                                    <br />
                                </td>
                              
                            </tr>
                            <tr>
                                <td class="auto-style6">
                                    <br />
                                    </td>
                            </tr>
                        <tr>
                            <td class="auto-style6">
                               
                                <asp:GridView ID="grdPagos" runat="server" AllowSorting="True" AutoGenerateColumns="False" BorderStyle="None" CssClass="grd" DataKeyNames="id,idPredio,estatus" OnRowDataBound="grdPagos_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="noPago" HeaderText="No. Pago">
                                        <ItemStyle Width="80px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Fecha de Pago">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFecha" runat="server" AutoPostBack="true" CssClass="textMediano" MaxLength="50" OnTextChanged="txtFecha_TextChanged" placeholder="Fecha de Pago" Text='<%# Eval("fecha") %>'></asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" BehaviorID="txtFecha_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFecha" />
                                                <ajaxToolkit:CalendarExtender ID="ceFecha" runat="server" BehaviorID="txtFecha_CalendarExtender" CssClass="CalendarCSS" Format="dd/MM/yyyy" TargetControlID="txtFecha" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtFecha" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                                <asp:RangeValidator ID="rgvFecha" runat="server" ControlToValidate="txtFecha" CssClass="mensajeValidador" ErrorMessage="Fuera de rango" MaximumValue="01/01/9999" MinimumValue="01/01/1900" SetFocusOnError="True" Type="Date" ValidationGroup="guardar"></asp:RangeValidator>
                                            </ItemTemplate>
                                            <ItemStyle Width="320px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="saldo" DataFormatString="{0:N0}" HeaderText="Saldo">
                                        <ItemStyle Width="150px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Parcialidad">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtParcialidad" runat="server" AutoPostBack="true" CssClass="textMediano" MaxLength="50" OnTextChanged="txtParcialidad_TextChanged" placeholder="Saldo" Text='<%# Eval("parcialidad","{0:N0}") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtParcialidad" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                            </ItemTemplate>
                                            <ItemStyle Width="200px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Recargo">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRecargo" runat="server" AutoPostBack="true" CssClass="textMediano" MaxLength="50" OnTextChanged="txtRecargo_TextChanged" placeholder="Saldo" Text='<%# Eval("recargo","{0:N0}") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator61" runat="server" ControlToValidate="txtRecargo" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                            </ItemTemplate>
                                            <ItemStyle Width="200px" />
                                        </asp:TemplateField>
                                        <%--                                <asp:BoundField DataField="recargo" HeaderText="Recargo" DataFormatString="{0:N0}" >
                                <HeaderStyle Width="250px" />
                                </asp:BoundField>--%>
                                        <asp:BoundField DataField="pagoTardio" DataFormatString="{0:N0}" HeaderText="Pago Tardio">
                                        <ItemStyle Width="200px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="mensualidad" DataFormatString="{0:N0}" HeaderText="Mensualidad">
                                        <ItemStyle Width="200px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Pagado">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkPago" runat="server" Checked='<%# Eval("pago") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptySearch" runat="server" CssClass="letraTituloEmptyGrid">No se encontraron resultados</asp:Label>
                                    </EmptyDataTemplate>
                                    <FooterStyle CssClass="grdFooter" />
                                    <HeaderStyle CssClass="grdHead" />
                                    <RowStyle CssClass="grdRowPar" />
                                </asp:GridView>
                                   
                                <table>
                                    <tr>
                                        <td class="auto-style1" style="text-align:right; padding-right:80px">
                                            <asp:Label ID="lblTotal" runat="server" CssClass="letraMediana" Text="Total"></asp:Label>
                                        </td>
                                        <td class="auto-style2">
                                            <asp:Label ID="lblTparcialidad" runat="server" CssClass="letraMediana" Text=""></asp:Label>
                                        </td>
                                        <td style="width:400px;  text-align: left;">
                                            <asp:Label ID="lblTRecargo" runat="server" CssClass="letraMediana" Text=""></asp:Label>
                                        </td>
                                        <td style="width:300px; text-align: left;">
                                            <asp:Label ID="lblPagoTardio" runat="server" CssClass="letraMediana" Text=""></asp:Label>
                                        </td>
                                        <td style="width:250px">
                                            <asp:Label ID="lblTMensualidad" runat="server" CssClass="letraMediana" Text=""></asp:Label>
                                        </td>
                                        <td style="width:200px"></td>
                                    </tr>
                                </table>
                            </td>
                </tr>
                <tr>
                    <td class="auto-style6">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style6">
                        <asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" Text="Guardar" ValidationGroup="guardar" Visible="False" />
                        <asp:Button ID="btnModificar" runat="server" CausesValidation="False" OnClick="btnModificar_Click" Text="Modificar" Visible="False" />
                        <asp:Button ID="btnFinalizar" runat="server" CausesValidation="False" OnClick="btnFinalizar_Click" Text="Finalizar Convenio" ValidationGroup="guardar" Visible="False" />
                        <%--<asp:Button ID="btnImprime" runat="server" OnClick="btnImprime_Click" Text="Imprime Convenio" />--%>
                        <asp:Button ID="btnAtras" runat="server" CausesValidation="False" OnClick="btnAtras_Click" Text="Regresar" Visible="False" />
                    </td>
                </tr>
                        </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="btnImprime" runat="server" OnClick="btnImprime_Click" Text="Imprime Convenio" />
    <uc1:ModalPopupMensaje runat="server" ID="vtnModal" DysplayAceptar="True" DysplayCancelar="False" />
</asp:Content>

