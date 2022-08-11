<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="catastroTramite.aspx.cs" Inherits="Catastro.Servicios.catastroTramite" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="Label1" runat="server" Text="Catastro" CssClass="letraTitulo"></asp:Label></td>
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
                                    <%--<ajaxToolkit:MaskedEditExtender runat="server" ID="meeFiltro" Mask="9999-99-999-999" MaskType="Number"  InputDirection="LeftToRight" TargetControlID="txtClvCastatral" />--%>
                                    <ajaxToolkit:MaskedEditExtender runat="server" ID="meeFiltro" Mask="9999-99-999-999" TargetControlID="txtClvCastatral" MaskType="Number" InputDirection="RightToLeft" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtClvCastatral" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="agregarConcepto"></asp:RequiredFieldValidator>
                                </td>
                                <td colspan="2">
                                    <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="False" ImageUrl="~/img/consultar.fw.png" OnClick="buscarClaveCatastral" />
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblCuentaPredial" runat="server" CssClass="letraMediana" Text=""></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="lblCuentaPredialTxt" runat="server" CssClass="letraMediana" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1">
                                    <asp:Label ID="Label5" runat="server" Text="Propietario" CssClass="letraMediana" Enabled="true"></asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:Label ID="txtContruyente" runat="server" Text="" CssClass="letraMediana" Enabled="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label8" runat="server" CssClass="letraMediana" Text="Ubicación:"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label9" runat="server" CssClass="letraMediana" Text="Calle:"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="txtCalle" runat="server" Text="" CssClass="letraMediana" Enabled="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label10" runat="server" CssClass="letraMediana" Text="Numero:"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="txtNumero" runat="server" Text="" CssClass="letraMediana" Enabled="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label11" runat="server" CssClass="letraMediana" Text="Colonia:"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="txtColonia" runat="server" Text="" CssClass="letraMediana" Enabled="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label12" runat="server" CssClass="letraMediana" Text="Codigo Postal:"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="txtCP" runat="server" Text="" CssClass="letraMediana" Enabled="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" CssClass="letraMediana" Text="Localidad:"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="txtLocalidad" runat="server" Text="" CssClass="letraMediana" Enabled="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1"> 
                                    <asp:Label ID="lblFechaEntrega" runat="server" CssClass="letraMediana" Text="Fecha de entrega:"></asp:Label>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtFechaEntrega" runat="server" CssClass="textMediano" MaxLength="50" placeholder="Fecha entrega:"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="txtFechaEntrega_MaskedEditExtender1" runat="server" BehaviorID="txtFechaEntrega_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaEntrega" />
                                    <ajaxToolkit:CalendarExtender ID="txtFechaEntrega_CalendarExtender1" runat="server" BehaviorID="txtFechaEntrega_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFechaEntrega" Format="dd/MM/yyyy" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" CssClass="valida" ControlToValidate="txtFechaEntrega" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="agregarConcepto"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1">
                                    <asp:Label ID="Label3" runat="server" CssClass="letraMediana" Text="Concepto:"></asp:Label>
                                </td>
                                <td colspan="4">
                                    <asp:DropDownList ID="ddlConcepto" runat="server" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlConcepto" CssClass="valida" InitialValue="%" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="agregarConcepto"></asp:RequiredFieldValidator>
                                </td>
                                <td colspan="1">
                                    <asp:Button ID="btnAgregarConcepto" runat="server" Text="Agregar Concepto" ValidationGroup="agregarConcepto" OnClick="btnAgregarConcepto_Click" />
                                </td>
                                <td colspan="1">
                                    <asp:Button ID="btnCancelarTramite" runat="server" Text="Cancelar Tramite" OnClick="btnCancelarTramite_Click" Visible="false" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="padding: 0px 10px 0px 10px;">
                        <table runat="server" id="tablaColumnaGrid" visible="false" cellpadding="0" cellspacing="0" width="100%">
                            <tr style="background-color: #c3c3c3; height: 25px;">
                                <td style="width: 140px">
                                    <asp:Label ID="lblFechaColumna" runat="server" CssClass="letraMediana" ForeColor="#FFFFFF" Text=" Fecha"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblContribuyenteColumna" runat="server" CssClass="letraMediana" ForeColor="#FFFFFF" Text="Contribuyente"></asp:Label>
                                </td>
                                <td style="width: 140px">
                                    <asp:Label ID="lblHerramientaColumna" runat="server" CssClass="letraMediana" ForeColor="#FFFFFF" Text="Herramientas"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="padding: 0px 10px 10px 10px;">
                        <div runat="server" id="divGridAlta" visible="false" style="overflow: auto; height: 250px; align: left;">
                            <asp:GridView ID="grdAlta" runat="server" DataKeyNames="Id,Activo,Status,TipoCobro,Clave" AutoGenerateColumns="false" CssClass="grd"
                                AllowSorting="True" BorderStyle="None" ShowFooter="false" OnRowCommand="grdAlta_RowCommand" OnRowDataBound="grdAlta_RowDataBound" OnSorting="grdAlta_Sorting" ShowHeader="false">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Clave" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClave" runat="server" Text='<%# Eval("Clave") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Status" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="TipoCobro" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTipoCobro" runat="server" Text='<%# Eval("TipoCobro") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Activo" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblActivo" runat="server" Text='<%# Eval("Activo") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Cambio" Visible="false">
                                        <ItemTemplate>
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="text-align: left;">
                                                        <asp:Label ID="lblCambio" runat="server" Text='<%# Eval("Cambio") %>' Visible="false"></asp:Label>
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="lblTotalText" runat="server" Text="Total"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="135px" HeaderText="Fecha">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Eval("Descripcion") %>'></asp:Label>
                                        </ItemTemplate>
                                        <%-- <FooterTemplate>
                                        <asp:Label ID="lblCambios" runat="server"></asp:Label>
                                    </FooterTemplate>--%>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Contribuyente">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNombreAdquiriente" runat="server" Text='<%# Eval("NombreAdquiriente") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" Visible="false" HeaderText="Importe">
                                        <ItemTemplate>
                                            <asp:Label ID="lblImporte" runat="server" Visible="false" Text='<%# Eval("Importe") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotal" runat="server" Text="0" CssClass="valida"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="135px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgDelete" runat="server" ToolTip="Eliminar Tramite!"
                                                ImageUrl="~/img/eliminar.png"
                                                CssClass="imgButtonGrid"
                                                CommandName="EliminarRegistro"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                                            <asp:ImageButton ID="imgActivar" runat="server" ToolTip="Activar!"
                                                ImageUrl="~/img/Activar.png"
                                                CssClass="imgButtonGrid"
                                                CommandName="ActivarRegistro"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                                            <asp:ImageButton ID="imgUpdate" runat="server" ToolTip="Editar!"
                                                ImageUrl="~/img/modificar.png"
                                                CssClass="imgButtonGrid"
                                                CommandName="EditarImporte"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle CssClass="grdFooter" />
                                <HeaderStyle CssClass="grdHead" />
                                <RowStyle CssClass="grdRowPar" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="padding: 0px 10px 10px 10px;">
                        <table runat="server" id="tablaDatosAltas" visible="false" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td style="padding: 0px 0px 20px 0px;">
                                    <asp:Label ID="lblCambiosFooter" CssClass="grdFooter" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1">
                                    <asp:Label ID="lblContribuyente" runat="server" CssClass="letraMediana" Text="Contribuyente:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtContribuyente" runat="server" CssClass="textExtraGrande" MaxLength="100" placeholder="Nombre Adquiriente." AutoPostBack="true" OnTextChanged="txtContribuyente_TextChanged"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="revContribuyente" runat="server" ControlToValidate="txtContribuyente" CssClass="valida" Enabled="false" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblNTramites" runat="server" CssClass="letraMediana" Text="Número de Tramites :"></asp:Label>
                                </td>
                                <td colspan="1">
                                    <asp:TextBox ID="txtNTramites" runat="server" CssClass="textGrande" MaxLength="3" placeholder="Número de Tramites." Text="1"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvNTramites" runat="server" ControlToValidate="txtNTramites" CssClass="valida" Enabled="false" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revNTramites" runat="server" ControlToValidate="txtNTramites" CssClass="valida" Enabled="false" ErrorMessage="Ingresar solo numeros enteros" Font-Size="Small" SetFocusOnError="True" ValidationExpression="^\d+$" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <tr>
                        <td style="padding: 0px 10px 10px 10px;">
                            <asp:GridView ID="grd" runat="server" AllowSorting="True" AutoGenerateColumns="false" BorderStyle="None" CssClass="grd" DataKeyNames="Id,Activo,Status,TipoCobro,Clave" OnRowCommand="grd_RowCommand" OnRowDataBound="grd_RowDataBound" OnSorting="grd_Sorting" ShowFooter="True">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Clave" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClave" runat="server" Text='<%# Eval("Clave") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Status" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="TipoCobro" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTipoCobro" runat="server" Text='<%# Eval("TipoCobro") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Activo" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblActivo" runat="server" Text='<%# Eval("Activo") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Cambio" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCambio" runat="server" Text='<%# Eval("Cambio") %>' Visible="false"></asp:Label>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Concepto">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Eval("Descripcion") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>

                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="text-align: left;">
                                                        <asp:Label ID="lblCambios" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="lblTotalText" runat="server" Text="Total"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Nombre Adquiriente" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNombreAdquiriente" runat="server" Text='<%# Eval("NombreAdquiriente") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Importe">
                                        <ItemTemplate>
                                            <asp:Label ID="lblImporte" runat="server" Text='<%# Eval("Importe") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotal" runat="server" Text="0" CssClass="valida"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Herramientas" ItemStyle-CssClass="" ItemStyle-Width="135px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' CommandName="EliminarRegistro" CssClass="imgButtonGrid" ImageUrl="~/img/eliminar.png" ToolTip="Eliminar Concepto!" />
                                            <asp:ImageButton ID="imgActivar" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' CommandName="ActivarRegistro" CssClass="imgButtonGrid" ImageUrl="~/img/Activar.png" ToolTip="Activar!" />
                                            <asp:ImageButton ID="imgUpdate" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' CommandName="EditarImporte" CssClass="imgButtonGrid" ImageUrl="~/img/modificar.png" ToolTip="Editar!" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle CssClass="grdFooter" />
                                <HeaderStyle CssClass="grdHead" />
                                <RowStyle CssClass="grdRowPar" />
                            </asp:GridView>
                        </td>
                    </tr>
                      <tr>
                          <td colspan="7" style="vertical-align:text-top!important">
                              <asp:Label ID="Label13" runat="server" CssClass="letraMediana" Text="Observaciones:"></asp:Label>                          
                            <asp:TextBox ID="txtObservaciones" runat="server" CssClass="textMultiExtraGrande" MaxLength="500" placeholder="Observaciones"></asp:TextBox>
                        </td>
                       </tr>
                    <tr>
                        <td colspan="7">
                            <asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" Text="Guardar" ValidationGroup="guardar" />
                            &nbsp;
                            <asp:Button ID="btnRecargar" runat="server" CausesValidation="False" OnClick="btnRecargar_Click" Text="Recargar" />
                            &nbsp;
                            <asp:Button ID="btnEstado" runat="server" CausesValidation="False" OnClick="btnEstado_Click" Text="Estado de Cuenta" Width="163px" Visible="False" />
                        </td>
                    </tr>
                </tr>
            </table>
            <asp:Panel ID="pnl" runat="server" class="formPanel">
                <table>
                    <tr style="background-color: #b4b4b4">
                        <td colspan="4">
                            <asp:Label ID="lblTituloConceptoId" runat="server" CssClass="textModalTitulo2" Text="Cobros"></asp:Label>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="lblUma" runat="server" Text="Número de UMA's." CssClass="letraMediana"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtUma" runat="server" CssClass="textGrande" placeholder="Número de UMA's."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredValidadorSalario" runat="server" CssClass="valida" ControlToValidate="txtUma" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="agregarUma"></asp:RequiredFieldValidator>
                            <br />
                            <asp:RangeValidator ID="validadorSalario" runat="server" ErrorMessage="Ingresar solo números mayor a 0 y menor a 100" ControlToValidate="txtUma" MinimumValue="0" MaximumValue="10" Display="Dynamic" ValidationGroup="agregarCobro" Type="Integer"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="lblImporte" runat="server" Text="Importe:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtImporte" runat="server" CssClass="textGrande" placeholder="Importe."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="valida" ControlToValidate="txtImporte" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="agregarImporte"></asp:RequiredFieldValidator>
                            <br />
                            <asp:RangeValidator ID="validadorEfectivo" runat="server" ErrorMessage="Ingresar solo números mayor a 0 y menor a 99999999.99" ControlToValidate="txtImporte" MinimumValue="0.01" MaximumValue="99999999.99" Display="Dynamic" Type="Double"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <br />
                            <asp:Button ID="btnAceptarUma" runat="server" Text="Aceptar" ValidationGroup="agregarUma" OnClick="btnAceptarCobro_Click" />
                        </td>
                        <td style="text-align: right;">
                            <br />
                            <asp:Button ID="btnAceptarImporte" runat="server" Text="Aceptar" ValidationGroup="agregarImporte" OnClick="btnAceptarCobro_Click" />
                        </td>
                        <td style="text-align: right;">
                            <br />
                            <asp:Button ID="btnCancelarCobro" runat="server" Text="Cancelar" OnClick="btnCancelarCobro_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="pnl_Modal" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnl" TargetControlID="btnPnl">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnPnl" runat="server" />
            <br />
            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />
            
                <%--<input type="button" id="printreport" runat="server" value="imprimir" visible="false" onclick="printReport('<%=rpt.ClientID %>    ')" />--%>
            <asp:Panel ID="pnlReport" runat="server" Visible="false">
                    <div class="row">
                        <div class="col-md-12">
                            <input type="button" id="printreport" value="imprimir" onclick="printReport('<%=rpt.ClientID %>    ')" />
                        </div>
                    </div>
                </asp:Panel>    
            <rsweb:reportviewer id="rpt" runat="server" height="500px" width="800px" showprintbutton="true"></rsweb:reportviewer>
           
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
