<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PrediosPorFechaAlta.aspx.cs" Inherits="Catastro.Reportes.PrediosPorFechaAlta" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="Label1" runat="server" Text="Padrón de predios por fecha de alta" CssClass="letraTitulo"></asp:Label></td>
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
                                    <asp:DropDownList ID="ddlFiltro" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" >
                                        <asp:ListItem Text="Fecha" Value="fecha" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Rango de fechas" Value="rango"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Mes:"></asp:Label>&nbsp;&nbsp;&nbsp;
                                    &nbsp;&nbsp;-<asp:DropDownList ID="ddlMes" runat="server"  class="ddlMediano">
                                        <asp:ListItem Text="Todos" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Enero" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Febrero" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Marzo" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Abril" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Mayo" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="Junio" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="Julio" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="Agosto" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="Septiembre" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="Octubre" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="Noviembre" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="Diciembre" Value="12"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textMediano" placeholder="Fecha Inicio." Visible="false"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="txtFechaInicio_MaskedEditExtender" runat="server" BehaviorID="txtFechaInicio_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaInicio" />
                                    <ajaxToolkit:CalendarExtender ID="txtFechaInicio_CalendarExtender" runat="server" BehaviorID="txtFechaInicio_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFechaInicio" Format="dd/MM/yyyy" />
                                    <asp:RequiredFieldValidator ID="txtFechaInicio_RequiredFieldValidator" runat="server" CssClass="valida" ErrorMessage="*" ControlToValidate="txtFechaInicio" Enabled="false"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="Año:"></asp:Label>&nbsp;&nbsp;&nbsp;<asp:DropDownList ID="ddlAnio" runat="server"  class="ddlMediano" />
                                    <asp:TextBox ID="txtFechaFin" runat="server" CssClass="textMediano" placeholder="Fecha Final." Visible="false"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="txtFechaFin_MaskedEditExtender" runat="server" BehaviorID="txtFechaFin_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaFin" />
                                    <ajaxToolkit:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" BehaviorID="txtFechaFin_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFechaFin" Format="dd/MM/yyyy" />
                                    <asp:RequiredFieldValidator ID="txtFechaFin_RequiredFieldValidator" runat="server" CssClass="valida" ErrorMessage="*" ControlToValidate="txtFechaFin" Enabled="false"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="true"
                                        ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" />
                                </td>
                                <td>&nbsp;</td>
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
                                    <input type="button" id="printreport" value="imprimir" onclick="printReport('<%=rpvtPredios.ClientID %>')" />
                                </div>
                            </div>
                        </asp:Panel>
                        <rsweb:reportviewer id="rpvtPredios" runat="server" height="500px" width="900px" showprintbutton="true"></rsweb:reportviewer>
                    </td>
                </tr>
            </table>

            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>



