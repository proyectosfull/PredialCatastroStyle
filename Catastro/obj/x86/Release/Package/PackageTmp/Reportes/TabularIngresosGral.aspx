<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TabularIngresosGral.aspx.cs" Inherits="Catastro.Recibos.TabularIngresosGral" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
         <Triggers>
            <asp:PostBackTrigger ControlID="ExportExcel" />
        </Triggers>
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 60px; font-family: 'Trebuchet MS'; font-size: 25px; color: #575655;">
                        Ingresos Por Concepto</td>
                </tr>
                <tr>
                    <td style="height: 2px">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="padding: 0px 10px 10px 10px;">
                        <table cellpadding="0" cellspacing="0" width="100%" style="height: 200px">
                            <tr>
                                <td style="width: 371px; height: 53px;">
                                    <asp:Label ID="lblFecha0" runat="server" Text="Fecha" CssClass="letraMediana"></asp:Label>
                                    <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textMediano" placeholder="Fecha Inicio." Width="120px"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="txtFechaInicio_MaskedEditExtender" runat="server" BehaviorID="txtFechaInfra_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaInicio" />
                                    <ajaxToolkit:CalendarExtender ID="txtFechaInicio_CalendarExtender" runat="server" BehaviorID="txtFechaInfra_CalendarExtender" CssClass="CalendarCSS" Format="dd/MM/yyyy" TargetControlID="txtFechaInicio" />
                                    <asp:RequiredFieldValidator ID="rfvFechaInicio0" runat="server" ControlToValidate="txtFechaInicio" CssClass="valida" ErrorMessage="*" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtFechaFin" runat="server" CssClass="textMediano" placeholder="Fecha Final."></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="txtFechaFin_MaskedEditExtender" runat="server" BehaviorID="txtFechaFin_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaFin" />
                                    <ajaxToolkit:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" BehaviorID="txtFechaInfra_CalendarExtender" CssClass="CalendarCSS" Format="dd/MM/yyyy" TargetControlID="txtFechaFin" />
                                    <asp:RequiredFieldValidator ID="rfvFechaFin0" runat="server" ControlToValidate="txtFechaFin" CssClass="valida" ErrorMessage="*" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                                </td>
                                <td style="height: 53px; width: 275px">
                                    <asp:Label ID="Label16" runat="server" Text="Tipo tramite:" CssClass="letraMediana" Visible="False"></asp:Label>
                                    <asp:DropDownList ID="ddlTipoTramite" runat="server" Visible="False" />
                                </td>
                                <td style="height: 53px">
                                    <asp:Label ID="Label15" runat="server" Text="Estatus del recibo:" CssClass="letraMediana"></asp:Label>
                                    <asp:DropDownList ID="ddlStatus" runat="server" class="ddlMediano">
                                        <asp:ListItem Text="TODOS" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="PAGADO" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="CANCELADO" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="Label4" runat="server" CssClass="letraMediana" style="font-size: small" Text="Tipo de predio:" Visible="False"></asp:Label>
                                    <asp:DropDownList ID="ddlTipo" runat="server" Visible="False" />
                                    <asp:ImageButton ID="imbBuscar" runat="server" Height="45px" ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" ValidationGroup="buscar" Width="46px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 371px; height: 52px;">
                                    <asp:Label ID="lblColonia" runat="server" CssClass="letraMediana" Text="Colonia:" Visible="False"></asp:Label>
                                    <asp:TextBox ID="txtColonia" runat="server" CssClass="textExtraGrande" MaxLength="200" placeholder="Colonia" Width="315px" Visible="False"></asp:TextBox>
                                </td>
                                <td style="width: 275px; height: 52px;">
                                    &nbsp;</td>
                                <td style="height: 52px">
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
          
            <br />
            <br />
            <asp:GridView ID="grdv" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" BorderStyle="None" CssClass="grd" 
                DataKeyNames="Recibo" OnPageIndexChanging="grdv_PageIndexChanging" OnRowCommand="grdv_RowCommand" OnRowDataBound="grdv_RowDataBound" OnSorting="grdv_Sorting" PageSize="10" ShowFooter="True">
                <Columns>
                    <asp:BoundField DataField="Recibo" HeaderText="Recibo" SortExpression="Recibo" />                   
                    <asp:BoundField DataField="ClavePredial" HeaderText="Clave Predial" />
                    <asp:BoundField DataField="ClaveAnterior" HeaderText="Clave Anterior" />                    
                    <asp:BoundField DataField="Contribuyente" HeaderText="Contribuyente" />                   
                    <asp:BoundField DataField="EstatusRecibo" HeaderText="Estatus Recibo" />
                    <asp:BoundField DataField="FechaPago" HeaderText="Fecha Pago" />
                    <asp:BoundField DataField="TipoTramite" HeaderText="Tramite" />
                    <asp:BoundField DataField="clave_descto" HeaderText="Clave Descto" />
                    <asp:BoundField DataField="descuento" HeaderText="Descuento" />
                    <asp:BoundField DataField="ImporteNeto" HeaderText="Importe Neto" />
                    <asp:BoundField DataField="ImporteDescuento" HeaderText="Importe Descuento" />
                    <asp:BoundField DataField="ImportePagado" HeaderText="Importe Pagado" />
                    <asp:TemplateField HeaderText="Conceptos">
                        <ItemTemplate>
                            <asp:GridView ID="grd2" runat="server" CssClass="grd"  AutoGenerateColumns="False" OnRowDataBound="grd2_RowDataBound" ShowHeader="False">
                                <Columns>
                                    <asp:BoundField DataField="Cri" HeaderText="Cuenta" />
                                    <asp:BoundField DataField="Nombre" HeaderText="Descripción" />
                                    <asp:BoundField DataField="ImporteNeto" HeaderText="Importe" />
                                    <asp:BoundField DataField="ImporteDescuento" HeaderText="Descuento" />
                                    <asp:BoundField DataField="ImporteTotal" HeaderText="Total" />
                                </Columns>
                            </asp:GridView>
                        </ItemTemplate>
                   </asp:TemplateField>
                   <%--<asp:TemplateField HeaderText="Herramientas" ItemStyle-CssClass="" ItemStyle-Width="135px">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgConsultaPago" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Recibo")%>' CommandName="ConsultaPago" CssClass="imgButtonGrid" ImageUrl="~/img/pagos.png" ToolTip="Consulta Pago" />
                        </ItemTemplate>
                   </asp:TemplateField>--%>
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="lblEmptySearchTramite" runat="server" CssClass="letraTituloEmptyGrid">No se encontraron resultados</asp:Label>
                </EmptyDataTemplate>
                <FooterStyle CssClass="grdFooter" />
                <HeaderStyle CssClass="grdHead" />
                <RowStyle CssClass="grdRowPar" />
            </asp:GridView>
            <br />
            <br />
           
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
           
            <asp:ImageButton ID="ExportExcel" runat="server" OnClick="ExportExcel_Click" Height="57px" ImageUrl="~/Img/descarga.jpeg" Width="59px" />
            <br />
            <br />

            <%--<ajaxToolkit:ModalPopupExtender ID="pnl_Modal" runat="server" BackgroundCssClass="ventanaBackground" PopupControlID="pnl" TargetControlID="btnPnl">
                        </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnPnl" runat="server" />

             <asp:Panel ID="pnlReport" runat="server" Visible="true">
                <div class="row">
                    <div class="col-md-12">
                      <%--<input type="button" id="printreport" value="imprimir" onclick="printReport('<%=rpvtPredios.ClientID %>')" />
                    </div>
               </div>
            </asp:Panel>--%>

            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


