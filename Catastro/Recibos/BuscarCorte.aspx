<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BuscarCorte.aspx.cs" Inherits="Catastro.Recibos.BuscarCorte" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 60px;">
                <asp:Label ID="Label1" runat="server" Text="Corte de Caja" CssClass="letraTitulo"></asp:Label></td>
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
                            
                           
                            <asp:Label ID="lblTipoBusqueda" runat="server" Text="Tipo búsqueda:"></asp:Label>
                            
                           
                        </td>
                        
                        <td>
                             <asp:DropDownList ID="ddlBusqueda" runat="server" CssClass="textMediano" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                <asp:ListItem Value="2">Folio</asp:ListItem>
                                <asp:ListItem Value="1">Fecha</asp:ListItem>
                            </asp:DropDownList>
                                                        
                            </td>
                        
                        <td>
                            <asp:Label ID="lblFiltro" runat="server" Visible="False">Folio: </asp:Label> 
                        </td>
                        
                        <td>
                        <asp:TextBox ID="txtBusqueda" runat="server" CssClass="textGrande" Width="240px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvBusqueda" runat="server" ControlToValidate="txtBusqueda" CssClass="valida" ErrorMessage="*" ValidationGroup="buscar"></asp:RequiredFieldValidator>                            
                            <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textMediano" placeholder="Fecha Infraccion." Visible="False" Width="120px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFechaInicio" runat="server" ControlToValidate="txtFechaInicio" CssClass="valida" ErrorMessage="*" ValidationGroup="buscar" Visible="False" Enabled="False"></asp:RequiredFieldValidator>
                        <ajaxToolkit:MaskedEditExtender ID="txtFechaInicio_MaskedEditExtender" runat="server" BehaviorID="txtFechaInfra_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaInicio" />
                        <ajaxToolkit:CalendarExtender ID="txtFechaInicio_CalendarExtender" runat="server" BehaviorID="txtFechaInfra_CalendarExtender" CssClass="CalendarCSS" Format="dd/MM/yyyy" TargetControlID="txtFechaInicio" />
                        <asp:TextBox ID="txtFechaFin" runat="server" CssClass="textMediano" placeholder="Fecha Final." Visible="False"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender ID="txtFechaFin_MaskedEditExtender" runat="server" BehaviorID="txtFechaFin_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaFin" />
                        <asp:RequiredFieldValidator ID="rfvFechaFin" runat="server" ControlToValidate="txtFechaFin" CssClass="valida" ErrorMessage="*" ValidationGroup="buscar" Enabled="False"></asp:RequiredFieldValidator>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="txtFechaInfra_CalendarExtender" CssClass="CalendarCSS" Format="dd/MM/yyyy" TargetControlID="txtFechaFin" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Solo Numeros" ControlToValidate="txtBusqueda" ValidationGroup="buscar" CssClass="valida" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                        </td>
                            
                        <td>
                            <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="true" ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" ValidationGroup="buscar" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                 <asp:GridView ID="grdCorte" runat="server" AllowPaging="True" AutoGenerateColumns="False" BorderStyle="None" CssClass="grd" DataKeyNames="id" OnRowCommand="grdCorte_RowCommand" ShowFooter="True" OnPageIndexChanging="grdCorte_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Folio" SortExpression="Id" />
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha Corte" SortExpression="Fecha" />
                                <asp:BoundField DataField="Cajero" HeaderText="Cajero" SortExpression="Cajero" />
                                <asp:BoundField DataField="ImporteTotal" HeaderText="Importe" SortExpression="ImporteTotal" />
                                <asp:BoundField DataField="FechaInicial" HeaderText="Fecha Inicio" SortExpression="FechaInicial" />
                                <asp:BoundField DataField="FechaFinal" HeaderText="Fecha Fin" SortExpression="FechaFinal" />
                                <asp:TemplateField HeaderText="Herramientas" ItemStyle-CssClass="" ItemStyle-Width="180px">
                                    <ItemTemplate>
                                        
                                        <asp:ImageButton ID="imgConsulta" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' CommandName="ConsultarRegistro" CssClass="imgButtonGrid" ImageUrl="~/img/consultar.fw.png" ToolTip="Consultar!" />
                                        
                                        <%--<asp:ImageButton ID="imgDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' CommandName="EliminarRegistro" CssClass="imgButtonGrid" ImageUrl="~/img/eliminar.png" OnClientClick="return Alert_Confirmar(this.id,
                                            'Está seguro que quiere cancelar la Infraccion', 'Eliminación cancelada.');" ToolTip="Cancelar!" />--%>
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
        <tr>
            <td style="padding: 0px 10px 10px 10px;">
                <asp:Panel ID="pnlReport" runat="server" Visible="false">
                <div class ="row">                
                    <div class="col-md-12">
                        <input type="button" id="printreport" value="imprimir" onclick="printReport('<%=rpt.ClientID %>')"/>               
                    </div>
                </div>
            </asp:Panel>
                <rsweb:ReportViewer ID="rpt" runat="server" Height="500px" Width="900px" ShowPrintButton="true"></rsweb:ReportViewer>
        </tr>
    </table>
    
</asp:Content>
