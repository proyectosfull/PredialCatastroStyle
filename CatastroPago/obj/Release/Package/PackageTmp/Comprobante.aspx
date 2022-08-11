<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Comprobante.aspx.cs" Inherits="CatastroPago.Comprobante" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title><%: Page.Title %> Pago en Línea</title>

    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>
    <webopt:BundleReference runat="server" Path="~/Content/css" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />

    <script type="text/javascript">
        function printPdf() {
            var PDF = document.getElementById("frameIP");
            PDF.focus();
            PDF.contentWindow.print();
        }
    </script>
</head>
<body >
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server">
            <Scripts>
                <%--To learn more about bundling scripts in ScriptManager see http://go.microsoft.com/fwlink/?LinkID=301884 --%>
                <%--Framework Scripts--%>
                <asp:ScriptReference Name="MsAjaxBundle" />
                <asp:ScriptReference Name="jquery" />
                <asp:ScriptReference Name="bootstrap" />
                <asp:ScriptReference Name="respond" />
                <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                <asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
                <asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
                <asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
                <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
                <asp:ScriptReference Name="WebFormsBundle" />
                <%--Site Scripts--%>
            </Scripts>
        </asp:ScriptManager>
        <div>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                  <div class="container">
                  <div style="text-align: center;">
                        <asp:Panel ID="PanelResultado" runat="server" Visible="true" CssClass="pricing-table-content" BorderStyle="None">
                            <div style="text-align: center"><asp:Image ID="ImagenLogo" runat="server" ImageUrl="~/Img/logo_yaute.jpg" Height="143px" Width="239px" /></div>
                            <div id="barra" style="height:12px; width:100%; background-color: <%=this.colorDiv%>; text-align: center;"> </div>

                            <table align="center" >
                               <%-- <tr>
                                    <td style="text-align: center" class="header-full" colspan="2">
                                        <asp:Image ID="ImagenLogo" runat="server" Height="76px" ImageUrl="~/Img/logo_yaute.jpg" style="text-align: center" Width="187px" />
                                    </td>
                                     <div id="barra" style="height:12px; width:100%; background-color: <%=this.colorDiv%>; text-align: center;"> </div>
                                </tr>--%>
                                <tr>
                                    <td style="text-align: right" class="alert-border">
                                        Estado :</td>
                                    <td class="auto-style5">
                                        <asp:Label ID="lblEstado" runat="server" CssClass="letraMediana" Width="239px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" class="alert-border">
                                        Folio Transacción:</td>
                                    <td class="auto-style7">
                                        <asp:Label ID="lblFolio" runat="server" CssClass="letraMediana" Width="239px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" class="alert-border">
                                        Fecha/hora de transacción:</td>
                                    <td class="auto-style6">
                                        <asp:Label ID="lblFechaHora" runat="server" CssClass="letraMediana" Width="239px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" class="alert-border">
                                        Clave catastral procesada:</td>
                                    <td class="auto-style6">
                                        <asp:Label ID="lblCvecatProcesada" runat="server" CssClass="letraMediana" Width="239px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" class="alert-border">
                                        Importe:</td>
                                    <td class="auto-style6">
                                        <asp:Label ID="lblImporteTotal" runat="server" CssClass="letraMediana" Width="239px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;" class="alert-border">
                                        Número de Autorización:</td>
                                    <td class="auto-style6">
                                        <asp:Label ID="lblClaveAutorizacion" runat="server" CssClass="letraMediana" Width="239px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div id="barra" style="height:1px; width:100%; background-color: <%=this.colorDiv%>; text-align: center;"> </div>
                        </asp:Panel>
                   </div>

                    <asp:Panel ID="pnlRecibo" runat="server"  BackColor="White" Visible="False" CssClass="" HorizontalAlign="center" Height="500px" Width="100%">
                        <br />
                        <asp:Label ID="lbIP" runat="server"  Text="Comprobante de  Impuesto Predial" 
                            CssClass="letraMediana"></asp:Label>
                        <br />   
                       
                        <iframe id="frameIP" runat="server" src="" width="100%" height="45%" style="border: none;" title="Recibo de pago de Impuesto Predial" /> </iframe>
                        <br />
                        <asp:Label ID="lbSM" runat="server"  Text="Comprobante de Servicios Municipales" 
                            CssClass="letraMediana"></asp:Label>
                        <br />
                        <iframe id="frameSM" runat="server" src="" width="100%" height="45%" style="border-style: none; border-color: inherit; border-width: medium;"/>
                        </iframe>
                    </asp:Panel>
                    <br />
                    <div class="row" style="text-align: center;" >
                            <div  style="text-align: center;">
                               <asp:Button ID="btnConsulta" runat="server" OnClick="btnConsulta_Click" Text="Nueva Consulta" Width="159px" CssClass="btn-social foursquare solid" />
                            </div>
                                
                    </div>

                 </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </form>
</body>
</html>