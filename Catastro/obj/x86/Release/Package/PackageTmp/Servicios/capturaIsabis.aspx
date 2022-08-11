<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="capturaIsabis.aspx.cs" Inherits="Catastro.Catalogos.capturaIsabis" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="lblTitulo" runat="server" CssClass="textModalTitulo2" Text="Captura Isabis"></asp:Label>
                        <hr />
                    </td>
                </tr>
                 <tr>
                    <td style="height: 2px"><asp:HiddenField ID="hdfId" runat="server" />
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 70%">
                        <table style="width: 100%">                           
                            <tr style="background-color: #b4b4b4">
                                <td  >
                                    <asp:Label ID="Label2" runat="server" Text="Clave Catastral:" CssClass="letraMediana"></asp:Label>                                    
                                    <br />
                                    <br />
                                    <asp:Label ID="lblCuentaPredial" runat="server" CssClass="letraMediana" Text="Cuenta Predial:"></asp:Label>
                                </td>
                                <td colspan="1">
                                    <asp:TextBox ID="txtClvCastatral" runat="server" CssClass="textGrande"  MaxLength="15" placeholder="Clave Catastral." ></asp:TextBox>
                                    <%--<ajaxToolkit:MaskedEditExtender  runat="server" ID="meeFiltro" Mask="9999-99-999-999" MaskType="Number"  InputDirection="LeftToRight" TargetControlID="txtClvCastatral" />--%>
                                    <ajaxToolkit:MaskedEditExtender runat="server" ID="meeFiltro" Mask="9999-99-999-999" TargetControlID="txtClvCastatral" MaskType="Number" InputDirection="RightToLeft" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="txtClvCastatral" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>   <br />
                                    <%--<ajaxToolkit:MaskedEditExtender  ID="txtTelefono_MaskedEditExtender" runat="server" InputDirection="LeftToRight" Mask="9999-99-999-999" PromptCharacter="_" TargetControlID="txtClvCastatral" /> --%>
                                    <asp:Label ID="lblCuentaPredialTxt" runat="server" CssClass="letraMediana" Text=""></asp:Label>
                                <td colspan="1">
                                    <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="False" ImageUrl="~/img/consultar.fw.png" ValidationGroup="buscarClave" OnClick="buscarClaveCatastral" />                                   
                                </td>
                                <td colspan="1">
                                <asp:CheckBox ID="chkNotariaForanea" runat="server" Text="Notaria Foránea" /><br />
                                <asp:CheckBox ID="chkNoCausa" runat="server" Text="No Causa" AutoPostBack="true" OnCheckedChanged="chkNoCausa_CheckedChanged" /><br />
                                    <asp:CheckBox ID="chkCoret" runat="server" AutoPostBack="true" Text="Insus" OnCheckedChanged="chkCoret_CheckedChanged" />
                                    </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblNombre" runat="server" Text="Nombre Adquiriente:" CssClass="letraMediana"></asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtNombreAdquiriente" runat="server" CssClass="textExtraGrande" MaxLength="100" placeholder="Nombre Adquiriente."></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" ControlToValidate="txtNombreAdquiriente" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="Label3" runat="server" Text="Tipo Avaluo:" CssClass="letraMediana"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="ddlConceptoPago" runat="server"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlConceptoPago" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar" InitialValue="%" CssClass="valida"></asp:RequiredFieldValidator>
                                </td>
                               
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="Label4" runat="server" Text="Nombre Valuador:" CssClass="letraMediana"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="ddlNombreValuador" runat="server"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlNombreValuador" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar" InitialValue="%" CssClass="valida"></asp:RequiredFieldValidator>
                                </td>
                                 <td > 
                                    <asp:Label ID="lblFechaRegistro" runat="server" CssClass="letraMediana" Text="Fecha de registro:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaRegistro" runat="server" CssClass="textMediano" MaxLength="50" placeholder="Fecha registro:"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="txtFechaRegistro_MaskedEditExtender1" runat="server" BehaviorID="txtFechaRegistro_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaRegistro" />
                                    <ajaxToolkit:CalendarExtender ID="txtFechaRegistro_CalendarExtender1" runat="server" BehaviorID="txtFechaRegistro_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFechaRegistro" Format="dd/MM/yyyy" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" CssClass="valida" ControlToValidate="txtFechaRegistro" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="1">
                                    <asp:Label ID="Label5" runat="server" Text="QUE HACE AL C.:" CssClass="letraMediana"></asp:Label>
                                    </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtContruyente" runat="server" CssClass="textExtraGrande" ReadOnly="true" placeholder="Contribuyente."></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1">
                                    <asp:Label ID="Label6" runat="server" Text="Según número de escritura: " CssClass="letraMediana"></asp:Label>
                                </td>
                                <td colspan="1">
                                    <asp:TextBox ID="txtEscritura" runat="server" CssClass="textGrande" MaxLength="100" placeholder="Escritura"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" CssClass="valida" ControlToValidate="txtEscritura" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                </td>
                                <td colspan="1">
                                    <asp:Label ID="Label7" runat="server" Text="  de Fecha Operación: " CssClass="letraMediana"></asp:Label>
                                </td>
                                <td colspan="1">                                    
                                    <asp:TextBox ID="txtFecha" runat="server" CssClass="textMediano" MaxLength="50" placeholder="Fecha de Operación:"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="txtFecha_MaskedEditExtender1" runat="server" BehaviorID="txtFecha_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFecha" />
                                    <ajaxToolkit:CalendarExtender ID="txtFecha_CalendarExtender1" runat="server" BehaviorID="txtFecha_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFecha" Format="dd/MM/yyyy" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="valida" ControlToValidate="txtFecha" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1">
                                    <asp:Label ID="Label8" runat="server" CssClass="letraMediana" Text="Valor del Inmueble"></asp:Label>
                                </td>
                                <td colspan="1">
                                    <asp:Label ID="Label9" runat="server" CssClass="letraMediana" Text="Catastro:" ></asp:Label><br />
                                    <asp:TextBox ID="txtCatastro" runat="server" CssClass="textMediano" MaxLength="11" placeholder="Catastro" AutoPostBack="true" OnTextChanged="validaValorInmueble" ></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" CssClass="valida" ControlToValidate="txtCatastro" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator> <br/>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Maximo 2 decimales." ValidationExpression="\d+(\.\d{2})?|\.\d{2}" ControlToValidate="txtCatastro" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator> <br />
                                    <asp:RangeValidator ID="validadorCatastro" runat="server" ErrorMessage="Ingresar valor mayor a 0 y menor a 100,000,000.00" ControlToValidate="txtCatastro" MinimumValue="0.01" MaximumValue="99999999.99" Display="Dynamic" ValidationGroup="guardar" Type="Double"></asp:RangeValidator>
                                </td>
                                <td colspan="1">
                                    <asp:Label ID="Label10" runat="server" CssClass="letraMediana" Text="Comercial:"></asp:Label><br />
                                    <asp:TextBox ID="txtComercial" runat="server" CssClass="textMediano" MaxLength="11" placeholder="Comercial" AutoPostBack="true"  OnTextChanged="validaValorInmueble"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" CssClass="valida" ControlToValidate="txtComercial" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator><br/>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Maximo 2 decimales." ValidationExpression="\d+(\.\d{2})?|\.\d{2}" ControlToValidate="txtComercial" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator><br />
                                    <asp:RangeValidator ID="validadorComercial" runat="server" ErrorMessage="Ingresar valor mayor a 0 y menor a 100,000,000.00" ControlToValidate="txtComercial" MinimumValue="0.0" MaximumValue="99999999.99" Display="Dynamic" ValidationGroup="guardar" Type="Double"></asp:RangeValidator>
                                </td>
                               <%-- <td colspan="1">
                                    <asp:Label ID="Label11" runat="server" CssClass="letraMediana" Text="Fiscal:"></asp:Label><br />
                                    <asp:TextBox ID="txtFiscal" runat="server" CssClass="textMediano" placeholder="Fiscal" MaxLength="8" AutoPostBack="true" OnTextChanged="validaValorInmueble"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" CssClass="valida" ControlToValidate="txtFiscal" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator><br/>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Maximo 2 decimales." ValidationExpression="\d+(\.\d{2})?|\.\d{2}" ControlToValidate="txtFiscal" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                </td>--%>
                                <td colspan="1">
                                    <asp:Label ID="Label12" runat="server" CssClass="letraMediana" Text="Operación:"></asp:Label><br />
                                    <asp:TextBox ID="txtOperacion" runat="server" CssClass="textMediano" placeholder="Operación" MaxLength="11" AutoPostBack="true"  OnTextChanged="validaValorInmueble"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" CssClass="valida" ControlToValidate="txtOperacion" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator><br/>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="Maximo 2 decimales." ValidationExpression="\d+(\.\d{2})?|\.\d{2}" ControlToValidate="txtOperacion" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator><br />
                                    <asp:RangeValidator ID="validadorOperacion" runat="server" ErrorMessage="Ingresar valor mayor a 0 y menor a 100,000,000.00" ControlToValidate="txtOperacion" MinimumValue="0.1" MaximumValue="99999999.99" Display="Dynamic" ValidationGroup="guardar" Type="Double"></asp:RangeValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="Label20" runat="server" Text="Notaria Solicitante:" CssClass="letraMediana"></asp:Label><br />
                                    <asp:TextBox ID="txtNombreSolicitante" runat="server" CssClass="textExtraGrande" MaxLength="30" placeholder="Nombre Solicitante."></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" CssClass="valida" ControlToValidate="txtNombreSolicitante" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="Label21" runat="server" Text="Observaciones:" CssClass="letraMediana"></asp:Label><br />
                                    <asp:TextBox ID="txtObservaciones" TextMode="MultiLine" runat="server" CssClass="textGrande" MaxLength="100" placeholder="Observaciones. " Width="476px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" CssClass="valida" ControlToValidate="txtObservaciones" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>                                       
                                </td>                               
                            </tr>
                             <tr>
                        <td colspan="2" style="text-align: left">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="guardar" OnClick="btnGuardar_Click" />
                            </td>
                        <td colspan="2" style="text-align: right">
                            <asp:Button ID="btnCancelar" runat="server" CausesValidation="False" Text="Regresar" OnClick="btnCancelar_Click" />
                        </td>
                    </tr>
                        </table>
                    </td>
                    <td >
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label13" runat="server" CssClass="letraMediana" Text="Impuesto:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtImpuesto" runat="server" CssClass="textGrande" ReadOnly="true"></asp:TextBox>
                                </td>                                
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label14" runat="server" CssClass="letraMediana" Text="Descuento:"></asp:Label>
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtDescuento" runat="server" CssClass="textGrande" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label15" runat="server" CssClass="letraMediana" Text="Adicionales:"></asp:Label>
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtAdicionales" runat="server" CssClass="textGrande" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label16" runat="server" CssClass="letraMediana" Text="Descuento:"></asp:Label>
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtDescuento01" runat="server" CssClass="textGrande" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label17" runat="server" CssClass="letraMediana" Text="Recargos:"></asp:Label>
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtRecargos" runat="server" CssClass="textGrande" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label18" runat="server" CssClass="letraMediana" Text="Descuento:"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtDescuento02" runat="server" CssClass="textGrande" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label19" runat="server" CssClass="letraMediana" Text="Importe:"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtImporte" runat="server" CssClass="textGrande" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>                
            </table>
            <br />
            <table style="width:100%;" runat="server" Visible="False" >
                <tr style="background-color: #b4b4b4 " >
                    <td style="width:50%;">
                        <asp:Label ID="lblImpuestoAnual" runat="server" Text="Impuesto Anual" CssClass="letraGrande" Font-Size="Medium" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width:50%;">
                        <asp:Label ID="lblImpuestoBimestral" runat="server" Text="Impuesto Bimestral:" CssClass="letraGrande" Font-Size="Medium" Font-Bold="true" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:50%;">
                        <asp:GridView ID="grdAno" runat="server" AutoGenerateColumns="False" CssClass="grd" AllowPaging="True"
                            AllowSorting="True" BorderStyle="None" ShowFooter="True">
                            <Columns>
                                <asp:BoundField DataField="ClavePredial" HeaderText="ClavePredial" SortExpression="ClavePredial" />
                                <asp:BoundField DataField="Ano" HeaderText="Año" SortExpression="Ano" />
                                <asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" />
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmptySearch" runat="server" CssClass="letraTituloEmptyGrid">No se encontraron resultados</asp:Label>
                            </EmptyDataTemplate>
                            <FooterStyle CssClass="grdFooter" />
                            <HeaderStyle CssClass="grdHead" />
                            <RowStyle CssClass="grdRowPar" />
                        </asp:GridView>
                    </td>
                    <td style="width:50%;">
                        <asp:GridView ID="grdBimestre" runat="server" AutoGenerateColumns="False" CssClass="grd" AllowPaging="True"
                            AllowSorting="True" BorderStyle="None" ShowFooter="True">
                            <Columns>
                                <asp:BoundField DataField="ClavePredial" HeaderText="ClavePredial" SortExpression="ClavePredial" />
                                <asp:BoundField DataField="Ano" HeaderText="Año" SortExpression="Ano" />
                                <asp:BoundField DataField="Bimestre" HeaderText="Bimestre" SortExpression="Bimestre" />
                                <asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" />
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmptySearch" runat="server" CssClass="letraTituloEmptyGrid">No se encontraron resultados</asp:Label>
                            </EmptyDataTemplate>
                            <FooterStyle CssClass="grdFooter" />
                            <HeaderStyle CssClass="grdHead" />
                            <RowStyle CssClass="grdRowPar" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>


            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
