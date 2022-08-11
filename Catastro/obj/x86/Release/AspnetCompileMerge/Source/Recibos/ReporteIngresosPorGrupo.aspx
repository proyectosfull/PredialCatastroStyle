<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReporteIngresosPorGrupo.aspx.cs" Inherits="Catastro.Recibos.ReporteIngresosPorGrupo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td colspan="3">
                <asp:Label ID="lblTitulo" runat="server" Text="Generar Prepoliza de Ingresos" CssClass="letraTitulo"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 2px" colspan="3">
                <hr />
            </td>
        </tr>
        <tr>
            <td style="padding: 0px 10px 10px 10px;" colspan="3">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblFechaInicio" runat="server" Text="Fecha de Inicio:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textMediano" placeholder="Fecha inicio."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvFechaInicio" runat="server" ControlToValidate="txtFechaInicio" CssClass="valida" ErrorMessage="*" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                            
                            <ajaxToolkit:MaskedEditExtender ID="txtFechaInicio_MaskedEditExtender" runat="server" BehaviorID="txtFechaInfra_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaInicio" />
                            <ajaxToolkit:CalendarExtender ID="txtFechaInicio_CalendarExtender" runat="server" BehaviorID="txtFechaInfra_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFechaInicio" Format="dd/MM/yyyy" />
                        </td>
                        <td>
                            <asp:Label ID="lblFechaFin" runat="server" Text="Fecha fin:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFechaFin" runat="server" CssClass="textMediano" placeholder="Fecha Fin."></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="txtFechaFin_MaskedEditExtender" runat="server" BehaviorID="txtFechaFin_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaFin" />
                            <asp:RequiredFieldValidator ID="rfvFechaFin" runat="server" ControlToValidate="txtFechaFin" CssClass="valida" ErrorMessage="*" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="txtFechaInfra_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFechaFin" Format="dd/MM/yyyy" />
                        </td>
                        <td>
                            <asp:CompareValidator ID="cpv" runat="server" ValidationGroup="buscar" CssClass="valida" ControlToCompare="txtFechaInicio" ControlToValidate="txtFechaFin" Display="Dynamic" ErrorMessage="La fecha de fin debe de ser igual o mayor que la de inicio" Operator="GreaterThanEqual" Type="Date" />                            
                        </td>
                        <td>
                           <asp:ImageButton ID="imbBuscar" runat="server" ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" CausesValidation="true" ValidationGroup="buscar" /> 
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding: 0px 10px 10px 10px;" colspan="3">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>                            
                            <asp:Label ID="lblDescripcion" runat="server" Text="Descripción del reporte: " Visible="False"></asp:Label>                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescripcion" runat="server" CssClass="textExtraGrande" placeholder="Descripción." Visible="false"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="frvDesc" runat="server" ControlToValidate="txtDescripcion" CssClass="valida" ErrorMessage="*" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>                      
                        <td>
                            <asp:Button ID="btnGenerar" runat="server" Text="Generar" ValidationGroup="guardar" OnClick="btnGenerar_Click" Visible="false" />
                        </td>
                        <td style="text-align:right">
                            <asp:Button ID="btnRegresar" runat="server" Text="Regresar" OnClick="btnRegresar_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding: 0px 10px 10px 10px;" colspan="3">
                <asp:GridView ID="grdReporte" runat="server" AutoGenerateColumns="False" BorderStyle="None" CssClass="grd" DataKeyNames="IdConcepto,Total" ShowFooter="True" OnRowDataBound="grdReporte_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" />
                        <asp:BoundField DataField="Descripcion" HeaderText="Concepto" SortExpression="Descripcion" />
                        <asp:BoundField DataField="Recibos" HeaderText="Recibos" SortExpression="Recibos" />
                        <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString="{0:c}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
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
  
    <asp:Panel ID="pnlReport" runat="server" Visible="false">
        <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td><input type="button" id="printreport" value="imprimir" onclick="printReport('<%=rpt.ClientID %>    ')" /></td>
                      <%--  <td>
                            <asp:Button ID="btnReporte" runat="server" Text="Ver reporte" OnClick="btnReporte_Click" Visible="false" />
                        </td>
                        <td>
                            <asp:Button ID="btnVerDetalle" runat="server" Text="Ver detalle" OnClick="btnVerDetalle_Click" Visible="false" />
                        </td>--%>
                    </tr>
                </table>                
    </asp:Panel>
    <rsweb:ReportViewer ID="rpt" runat="server" Height="500px" Width="900px" ShowPrintButton="true"></rsweb:ReportViewer>
      <uc1:ModalPopupMensaje runat="server" ID="vtnModal" />
</asp:Content>

