<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReporteRequerimientos.aspx.cs" Inherits="Catastro.Recibos.ReporteRequerimientos" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 60px;">
                <asp:Label ID="Label1" runat="server" Text="Reporte de Requerimientos." CssClass="letraTitulo"></asp:Label></td>
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
                        <td colspan="2"> 
                            <asp:Label ID="lblEstado" runat="server" Text="Estado"></asp:Label>  &nbsp;&nbsp;&nbsp;&nbsp;                           
                            <asp:DropDownList ID="ddlEstado" runat="server">
                                <asp:ListItem Value="%">Todos</asp:ListItem>
                                <asp:ListItem Value="ACTIVO">ACTIVO</asp:ListItem>
                                <asp:ListItem Value="PAGADO">PAGADO</asp:ListItem>
                                <asp:ListItem Value="CANCELADO">CANCELADO</asp:ListItem>
                            </asp:DropDownList>    
                             &nbsp;&nbsp;                    
                                                     
                          <asp:Label ID="lblCondominio" runat="server" Text="Condominio"></asp:Label>                               
                          <asp:DropDownList ID="ddlCondominio" runat="server"></asp:DropDownList>                        
                         
                        &nbsp; &nbsp;  Agente Fiscal<asp:DropDownList ID="ddlAgente" runat="server"></asp:DropDownList>                        
                         
                        </td>
                    </tr>
                    <tr>
                        <td > <asp:RadioButtonList ID="RadioButtonFecha" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RadioButtonFecha_SelectedIndexChanged" RepeatDirection="Horizontal" >
                                <asp:ListItem Selected="True" Value="Emision"> Fecha Emisión </asp:ListItem>
                                <asp:ListItem Value="Pago"> Fecha Pago </asp:ListItem>  
                              </asp:RadioButtonList> 
                                                                         
                            <asp:Label ID="lblFecha" runat="server" Text="Fecha de Emisión"></asp:Label>  &nbsp;
                            <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textMediano" placeholder="Fecha Inicio."  Width="120px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvFechaInicio" runat="server" ControlToValidate="txtFechaInicio" CssClass="valida" ErrorMessage="*" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                            <ajaxToolkit:MaskedEditExtender ID="txtFechaInicio_MaskedEditExtender" runat="server" BehaviorID="txtFechaInfra_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaInicio" />
                            <ajaxToolkit:CalendarExtender ID="txtFechaInicio_CalendarExtender" runat="server" BehaviorID="txtFechaInfra_CalendarExtender" CssClass="CalendarCSS" Format="dd/MM/yyyy" TargetControlID="txtFechaInicio" />
                            <asp:TextBox ID="txtFechaFin" runat="server" CssClass="textMediano" placeholder="Fecha Final."  Width="120px"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="txtFechaFin_MaskedEditExtender" runat="server" BehaviorID="txtFechaFin_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaFin" />
                            <asp:RequiredFieldValidator ID="rfvFechaFin" runat="server" ControlToValidate="txtFechaFin" CssClass="valida" ErrorMessage="*" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="txtFechaInfra_CalendarExtender" CssClass="CalendarCSS" Format="dd/MM/yyyy" TargetControlID="txtFechaFin" />
                       
                            <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="true" ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" ValidationGroup="buscar" />
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
                <asp:Panel ID="pnlReport" runat="server" Visible="false">
                    <div class="row">
                        <div class="col-md-12">
                            <input type="button" id="printreport" value="imprimir" onclick="printReport('<%=rpt.ClientID %>    ')" />
                        </div>
                    </div>
                </asp:Panel>
                <rsweb:ReportViewer ID="rpt" runat="server" Height="500px" Width="900px" ShowPrintButton="true"></rsweb:ReportViewer>
        </tr>
    </table>

</asp:Content>
