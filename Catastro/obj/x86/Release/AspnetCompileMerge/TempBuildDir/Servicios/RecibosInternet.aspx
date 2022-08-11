<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RecibosInternet.aspx.cs" Inherits="Catastro.Servicios.RecibosInternet" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

      <script type="text/javascript">
        function printPdf() {
            var PDF = document.getElementById("MainContent_frameRecibo");
            PDF.focus();
            PDF.contentWindow.print();
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="Label1" runat="server" Text="Recibos Pendientes" CssClass="letraTitulo"></asp:Label></td>
                </tr>
                <tr>
                    <td style="height: 2px" colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="padding: 0px 10px 10px 10px;" colspan="6">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlFiltro" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" />
                                    <asp:TextBox ID="txtFiltro" runat="server" CssClass="textGrande"></asp:TextBox>
                                    <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="False"
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
                    <td style="padding: 0px 10px 10px 10px;" colspan="6">
                        <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False"
                            CssClass="grd" DataKeyNames="id,Estado" AllowPaging="True"
                            AllowSorting="True" BorderStyle="None" ShowFooter="True" PageSize="5"
                            OnRowCommand="grd_RowCommand" OnSorting="grd_Sorting" OnPageIndexChanging="grd_PageIndexChanging" OnRowDataBound="grd_RowDataBound">
                            <Columns>                                
                                <asp:BoundField DataField="IdPredio" HeaderText="Clave Predial" SortExpression="IdPredio" />
                                <asp:BoundField DataField="FechaPago" HeaderText="Fecha de Pago" SortExpression="FechaPago" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="Ejercicio" HeaderText="Ejercicio" SortExpression="Ejercicio" />
                                <asp:BoundField DataField="ImportePagado" HeaderText="Importe Pagado" SortExpression="ImportePagado" />
                                <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="135px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgConsulta" runat="server" ToolTip="Consultar!"
                                            ImageUrl="~/img/consultar.fw.png"
                                            CssClass="imgButtonGrid"
                                            CommandName="ConsultarRegistro"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                                    </ItemTemplate>
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
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr style="background-color: #b4b4b4">
                        <td  colspan="6">
                            <asp:Label ID="Label14" runat="server" Text="Detalles del Predio" CssClass="letraMediana" Font-Size="Medium"></asp:Label>
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
                            <asp:Label ID="Label2" runat="server" CssClass="letraMediana" Text="Localidad:"></asp:Label>
                            &nbsp;
                                    <asp:Label ID="txtLocalidad" runat="server" Text="" CssClass="letraMediana" Enabled="true"></asp:Label>
                        </td>
                    </tr>
                    <tr style="background-color: #b4b4b4">
                        <td  colspan="6">
                            <asp:Label ID="Label3" runat="server" Text="Detalles del Cobro" CssClass="letraMediana" Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td  colspan="6">
                            <asp:GridView ID="grdCobros" runat="server" AutoGenerateColumns="False"
                                CssClass="grd" DataKeyNames="id" Visible="false" OnRowDataBound="grdCobro_RowDataBound"
                                AllowSorting="True" BorderStyle="None" ShowFooter="True">
                                <Columns>
                                    <asp:BoundField DataField="IdMesa" HeaderText="Mesa" SortExpression="IdMesa" />
                                    <asp:BoundField DataField="Descripcion" HeaderText="Concepto" SortExpression="Descripcion" />
                                    <asp:BoundField DataField="ImporteNeto" HeaderText="Costo" SortExpression="ImporteNeto" />
                                    <asp:BoundField DataField="ImporteDescuento" HeaderText="Descuento" SortExpression="ImporteDescuento" />
                                    <asp:BoundField DataField="ImporteTotal" HeaderText="Importe" SortExpression="ImporteTotal" />
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
                        <td></td>
                        <td colspan="1" style="text-align: right">
                            <asp:Button ID="btnReciboIpMs" runat="server" Text="Generar Ambos Recibos" OnClick="btnReciboIpMs_Click" />
                        </td>
                        <td colspan="1" style="text-align: right">
                            <asp:Button ID="btnReciboPredial" runat="server" Text="Generar Recibo Predial" OnClick="btnReciboPredial_Click" />
                        </td>
                        <td colspan="1" style="text-align: right">
                            <asp:Button ID="btnReciboServicios" runat="server" Text="Generar Recibo Servicios" OnClick="btnReciboServicios_Click" />
                        </td>
                        <td colspan="1" style="text-align: right">
                            <asp:Label ID="Label16" runat="server" CssClass="textModalTitulo2" Text="TOTAL"></asp:Label>
                        </td>
                        <td colspan="1" style="text-align: center">
                            <asp:Label ID="lblTotal" runat="server" CssClass="textModalTitulo2" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </table>
            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />
            <asp:Panel ID="pnlRecibo" runat="server" Width="800px" Height="500px" BackColor="White">
                <div class="width:100%;margin:1px;">
                    <asp:ImageButton ID="imCerrarRecibo" runat="server" ImageUrl="~/Img/eliminar.png" ImageAlign="Right" OnClick="imCerrarRecibo_Click" CausesValidation="False" />
                </div>
                <iframe id="frameReciboIP" runat="server" src="" width="100%" height="50%" style="border: none;" />
                <iframe id="frameReciboSM" runat="server" src="" width="100%" height="50%" style="border: none;" />
                <iframe id="frameRecibo" runat="server" src="" width="100%" height="100%" style="border: none;" />
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="modalRecibo" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnlRecibo" TargetControlID="btnRecibo">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnRecibo" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

