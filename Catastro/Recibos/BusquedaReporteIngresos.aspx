<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BusquedaReporteIngresos.aspx.cs" Inherits="Catastro.Recibos.BusquedaReporteIngresos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
     <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td  style="width: 716px">
                        <asp:Label ID="lblTitulo" runat="server" Text="Buscar Prepoliza de ingresos" CssClass="letraTitulo"></asp:Label>
                     </td>
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
                    <asp:Label ID="lblFechaInicio" runat="server" Text="Fecha de Inicio:" CssClass="letraMediana"></asp:Label>
                 </td>
                                <td>
                    <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textMediano" placeholder="Fecha inicio."></asp:TextBox>
                    
                    <ajaxToolkit:MaskedEditExtender ID="txtFechaInicio_MaskedEditExtender" runat="server" BehaviorID="txtFechaInfra_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaInicio" />
                    <ajaxToolkit:CalendarExtender ID="txtFechaInicio_CalendarExtender" runat="server" BehaviorID="txtFechaInfra_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFechaInicio" Format="dd/MM/yyyy" />
                 </td>
                                <td>
                    <asp:Label ID="lblFechaFin" runat="server" Text="Fecha fin:" CssClass="letraMediana"></asp:Label>
                 </td>
                                <td>
                    <asp:TextBox ID="txtFechaFin" runat="server" CssClass="textMediano" placeholder="Fecha Fin."></asp:TextBox>
                    <ajaxToolkit:MaskedEditExtender ID="txtFechaFin_MaskedEditExtender" runat="server" BehaviorID="txtFechaFin_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaFin" />
                    
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="txtFechaInfra_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFechaFin" Format="dd/MM/yyyy" />
                 </td>
                                <td>
                                    <asp:ImageButton ID="imbBuscar" runat="server" ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" CausesValidation="true" ValidationGroup="buscar" />
                    
                 </td>
                                <td>                  
                    <asp:Button  ID="imbNuevo" runat="server" OnClick="imbNuevo_Click"  Text="Nuevo Reporte"  />
                 </td>
                                <td>
                    <asp:CompareValidator ID ="cpv" runat="server" ValidationGroup="buscar" CssClass="valida" ControlToCompare="txtFechaInicio" ControlToValidate="txtFechaFin" Display="Dynamic" ErrorMessage="La fecha de fin debe de ser igual o mayor que la de inicio" Operator="GreaterThanEqual" Type="Date" />
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
                    <asp:GridView ID="grdReporte" runat="server" AllowPaging="True" AutoGenerateColumns="False" BorderStyle="None" CssClass="grd" DataKeyNames="id" OnRowCommand="grdReporte_RowCommand" OnSorting="grdReporte_Sorting" OnPageIndexChanging="grdReporte_PageIndexChanging" ShowFooter="True">
                            <Columns>
                                <asp:BoundField DataField="FechaPrePoliza" HeaderText="Fecha de  creación" SortExpression="FechaPrePoliza" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion" />
                                <%--<asp:BoundField DataField="Importe" HeaderText="Importe" SortExpression="Importe" />--%>
                                <asp:BoundField DataField="Elaboro" HeaderText="Elaboró" SortExpression="Elaboró" />
                                <asp:TemplateField HeaderText="Herramientas" ItemStyle-CssClass="" ItemStyle-Width="180px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgVerReporte" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' CommandName="VerReporte" CssClass="imgButtonGrid" ImageUrl="~/img/consultar.png" ToolTip="Consultar!" />
                                        <asp:ImageButton ID="imgDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' CommandName="EliminarRegistro" CssClass="imgButtonGrid" ImageUrl="~/img/eliminar.png" OnClientClick="return Alert_Confirmar(this.id,
                                            '¡Está seguro que quiere eliminar la asignación de folios?', 'Eliminación cancelada.');" ToolTip="Eliminar!" />
                                    </ItemTemplate>
                                    <ItemStyle Width="180px" />
                                </asp:TemplateField>
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
            <uc1:ModalPopupMensaje runat="server" ID="vtnModal" />       
</asp:Content>
