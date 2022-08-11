<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="catConceptoOmisionPago.aspx.cs" Inherits="Catastro.Catalogos.catConceptoOmisionPago" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="Label1" runat="server" Text="Concepto Omisión de Pago" CssClass="letraTitulo"></asp:Label></td>
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
                                <td colspan="6">
                                    <asp:TextBox ID="txtClvCastatral" runat="server" CssClass="textGrande" MaxLength="12" AutoPostBack="true" OnTextChanged="buscarClaveCatastral" placeholder="no. folio"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtClvCastatral" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="agregarConcepto"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1">
                                    <asp:Label ID="Label3" runat="server" CssClass="letraMediana" Text="Concepto:"></asp:Label>
                                </td>
                                <td colspan="1">
                                    <asp:DropDownList ID="ddlConcepto" runat="server" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlConcepto" CssClass="valida" InitialValue="%" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="agregarConcepto"></asp:RequiredFieldValidator>
                                </td>
                                <td colspan="1">
                                    <asp:Label ID="Label11" runat="server" Text="  Fecha Inicio: " CssClass="letraMediana"></asp:Label>
                                </td>
                                <td colspan="1">
                                    <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textMediano" MaxLength="50" placeholder="Fecha Inicio"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="txtFecha_MaskedEditExtender1" runat="server" BehaviorID="txtFecha_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaInicio" />
                                    <ajaxToolkit:CalendarExtender ID="txtFecha_CalendarExtender1" runat="server" BehaviorID="txtFecha_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFechaInicio" Format="dd/MM/yyyy" />
                                    <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1" ControlToValidate="txtFechaInicio" IsValidEmpty="false" MinimumValue="02/01/1900" MinimumValueMessage="" ValidationGroup="agregarConcepto"></ajaxToolkit:MaskedEditValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" CssClass="valida" ControlToValidate="txtFechaInicio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="agregarConcepto"></asp:RequiredFieldValidator>
                                </td>
                                <td colspan="1">
                                    <asp:Label ID="Label4" runat="server" Text="  Fecha Final: " CssClass="letraMediana"></asp:Label>
                                </td>
                                <td colspan="1">
                                    <asp:TextBox ID="txtFechaFin" runat="server" CssClass="textMediano" MaxLength="50" placeholder="Fecha Final"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" BehaviorID="txtFecha_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaFin" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="txtFecha_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFechaFin" Format="dd/MM/yyyy" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="valida" ControlToValidate="txtFechaFin" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="agregarConcepto"></asp:RequiredFieldValidator>
                                </td>
                                <td colspan="1">
                                    <asp:Button ID="btnAgregarConcepto" runat="server" Text="Agregar Concepto" ValidationGroup="agregarConcepto" OnClick="btnAgregarConcepto_Click" />
                                    <asp:Button ID="btnCancelarConcepto" runat="server" Text="Cancelar Edición"  OnClick="btnCancelarConcepto_Click" Visible="false"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="padding: 0px 10px 10px 10px;">
                        <asp:GridView ID="grd" runat="server" DataKeyNames="Id,Activo,Status" AutoGenerateColumns="false" CssClass="grd"
                            AllowSorting="True" BorderStyle="None" ShowFooter="True" OnRowCommand="grd_RowCommand" OnRowDataBound="grd_RowDataBound" OnSorting="grd_Sorting" >
                            <Columns>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText ="Clave" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClave" runat="server" Text='<%# Eval("Clave") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText ="Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText ="Status" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText ="Activo" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActivo" runat="server" Text='<%# Eval("Activo") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText ="Cambio" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCambio" runat="server" Text='<%# Eval("Cambio") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText ="Concepto">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescripcion" runat="server" Text='<%# Eval("Descripcion") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID ="lblCambios" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText ="Fecha Inicio">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFechaInicio" runat="server" Text='<%# Eval("FechaInicio") %>'  ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText ="Fecha Fin">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFechaFin" runat="server" Text='<%# Eval("FechaFin") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:CommandField ShowDeleteButton="true" ButtonType="Button" />--%>
                                <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="135px">
                                    <ItemTemplate>                                       
                                        <asp:ImageButton ID="imgUpdate" runat="server" ToolTip="Modificar!"
                                            ImageUrl="~/img/modificar.png"
                                            CssClass="imgButtonGrid"
                                            CommandName="ModificarRegistro"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                                        <asp:ImageButton ID="imgDelete" runat="server" ToolTip="Eliminar!"
                                            ImageUrl="~/img/eliminar.png"
                                            CssClass="imgButtonGrid"
                                            CommandName="EliminarRegistro"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                                        <asp:ImageButton ID="imgActivar" runat="server" ToolTip="Activar!"
                                            ImageUrl="~/img/Activar.png"
                                            CssClass="imgButtonGrid"
                                            CommandName="ActivarRegistro"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />                                     
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
                    <td colspan="7">
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CausesValidation="False" OnClick="btnGuardar_Click" />
                        <asp:Button ID="btnRecargar" runat="server" CausesValidation="False" Text="Recargar" OnClick="btnRecargar_Click" />
                    </td>
                </tr>
            </table>
            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
