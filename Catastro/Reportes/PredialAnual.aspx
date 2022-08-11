<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PredialAnual.aspx.cs" Inherits="Catastro.Reportes.PredialAnual" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 60px;">
                <asp:Label ID="Label1" runat="server" Text="Reporte Anual." CssClass="letraTitulo"></asp:Label></td>
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
                        <td  width="200px">
                             <asp:Label ID="lblCajero" runat="server" Text="Año"></asp:Label>  &nbsp;                           
                             <asp:DropDownList ID="ddlanio" runat="server"></asp:DropDownList>
                            &nbsp;&nbsp;
                             <rsweb:ReportViewer ID="ReportViewer1" runat="server"></rsweb:ReportViewer>
                        </td>
                        <td>
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
                <%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
                <asp:Panel ID="pnlReport" runat="server" Visible="false">
                    <div class="row">
                        <div class="col-md-12">
                            <input type="button" id="printreport" value="imprimir" onclick="printReport('<%=rpt.ClientID %>    ')" />
                        </div>
                    </div>
                </asp:Panel>
                <rsweb:reportviewer ID="rpt" runat="server" Height="500px" Width="900px" ShowPrintButton="true"></rsweb:reportviewer>
        </tr>
    </table>
</asp:Content>
