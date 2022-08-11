<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CorteCaja.aspx.cs" Inherits="Catastro.Recibos.CorteCaja"%>


<%--<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>--%>
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

    <div class="formCaptura">
          <asp:Label ID="lblTitulo" runat="server" Text="Corte de Caja" CssClass="letraTitulo"></asp:Label>
        <br />
        <table runat="server" id="tbPrincipal">
            <tr>
                <td>
                    <asp:Label ID="lblCajeros" runat="server" Text="Cajeros:" CssClass="letraMediana"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCajeros" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="ddlCajeros" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="corte" InitialValue="%"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:Label ID="lblFechaIni" runat="server" CssClass="letraMediana" Text="De la fecha:"></asp:Label>
                    <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textMediano" placeholder="Fecha Inicio."></asp:TextBox>
                    <ajaxToolkit:MaskedEditExtender ID="txtFechaInicio_MaskedEditExtender" runat="server" BehaviorID="txtFechaInicio_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaInicio" />
                    <ajaxToolkit:CalendarExtender ID="txtFechaInicio_CalendarExtender" runat="server" BehaviorID="txtFechaInfra_CalendarExtender" CssClass="CalendarCSS" Format="dd/MM/yyyy" TargetControlID="txtFechaInicio" />
                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtFechaInicio" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="corte"></asp:RequiredFieldValidator>--%>
                </td> 
                <td>
                     <asp:Label ID="lblFechaFin" runat="server" Text="A la fecha:" CssClass="letraMediana"></asp:Label>
                     &nbsp;
                     <asp:TextBox ID="txtFechaFin" runat="server" CssClass="textMediano" placeholder="Fecha Fin."></asp:TextBox>
                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" BehaviorID="txtFechaFin_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaInicio" />
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="txtFechaFin_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFechaFin" Format="dd/MM/yyyy" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" ControlToValidate="txtFechaFin" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="corte"></asp:RequiredFieldValidator>

                </td>               
                <td >
                    &nbsp;
                    <asp:Button ID="btnGenerarCorte" runat="server" Text="Iniciar Corte" OnClick="btnGenerarCorte_Click" ValidationGroup="corte" />
                </td>
            </tr>
            <%-- <asp:Panel ID="pnlRecibo" runat="server" Width="800px" Height="500px" BackColor="White">
                <div class="width:100%;margin:1px;">
                    <asp:ImageButton ID="imCerrarRecibo" runat="server" ImageUrl="~/Img/eliminar.png" ImageAlign="Right" OnClick="imCerrarRecibo_Click" />
                </div>
                <iframe id="frameRecibo" runat="server" src="" width="100%" height="100%" style="border: none;" />
        </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="modalRecibo" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnlRecibo" TargetControlID="btnRecibo">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnRecibo" runat="server" />--%>        
            <tr>
                <td colspan="5" style="height:10px"><hr /></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblTCobrados" runat="server" Text="Recibos Cobrados:" CssClass="letraMediana"></asp:Label>    
                </td>
                <td>
                    <asp:Label ID="lblCobrados" runat="server" Text="0" CssClass="letraMediana"></asp:Label>    </td>
                <td>
                    <asp:Label ID="lblTCancelados" runat="server" Text="Recibos Cancelados:" CssClass="letraMediana"></asp:Label>    
                </td>
                <td>
                    <asp:Label ID="lblCancelados" runat="server" Text="0" CssClass="letraMediana"></asp:Label>  
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="4" style="vertical-align:top;">
                    <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" BorderStyle="None" CssClass="grd" Width="100%" DataKeyNames="Id" OnRowDataBound="grd_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Recibo" SortExpression="Id" />
                            <asp:BoundField DataField="EstadoRecibo" HeaderText="Estado Recibo" SortExpression="EstadoRecibo" />                             
                            <asp:BoundField DataField="MetodoPagoFinal" HeaderText="Metodo Pago" SortExpression="MetodoPagoFinal" />
                            <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />                           
                            <asp:BoundField DataField="ImporteMetodo" HeaderText="Importe Pagado" SortExpression="ImporteMetodo" DataFormatString="{0:c}" >
                            <ItemStyle HorizontalAlign="Right" /> </asp:BoundField>
                            <asp:BoundField DataField="FechaPago" HeaderText="Fecha Pago" SortExpression="FechaPago" />
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmptySearch" runat="server" CssClass="letraTituloEmptyGrid">No se encontraron resultados</asp:Label>
                        </EmptyDataTemplate>
                        <FooterStyle CssClass="grdFooter" />
                        <HeaderStyle CssClass="grdHead" />
                        <RowStyle CssClass="grdRowPar" />
                    </asp:GridView>
                </td>
                <td style="vertical-align: top">
                    <table class="nav-justified">
                        <tr>
                            <td>
                                <asp:Label ID="lblDenominacion" runat="server" Text="Denominación" CssClass="letraSubTitulo"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCantidad" runat="server" Text="Cantidad" CssClass="letraSubTitulo"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblImporteDenomi" runat="server" Text="Importe" CssClass="letraSubTitulo"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblBMoneda" runat="server" Text="Moneda Fraccionada" CssClass="letraMediana"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMoneda" runat="server" CssClass="textMediano" placeholder="0.00" AutoPostBack="True" ValidationGroup="calculo" OnTextChanged="txtMoneda_TextChanged"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblMoneda" runat="server" Text="$0.00" CssClass="letraMediana"></asp:Label>
                                <asp:Label ID="lblVmoneda" runat="server" Text="Valor Invalido." CssClass="mensajeValidador" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblB20" runat="server" Text="Billetes de 20" CssClass="letraMediana"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt20" runat="server" CssClass="textMediano" placeholder="0" ValidationGroup="calculo" OnTextChanged="txt20_TextChanged" AutoPostBack="True"></asp:TextBox>                                
                            </td>
                            <td>
                                <asp:Label ID="lbl20" runat="server" Text="$0.00" CssClass="letraMediana"></asp:Label>
                                <asp:Label ID="lblV20" runat="server" Text="Valor Invalido." CssClass="mensajeValidador" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblB50" runat="server" Text="Billetes de 50" CssClass="letraMediana"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt50" runat="server" CssClass="textMediano" placeholder="0" ValidationGroup="calculo" OnTextChanged="txt50_TextChanged" AutoPostBack="True"></asp:TextBox>                                
                            </td>
                            <td>
                                <asp:Label ID="lbl50" runat="server" Text="$0.00" CssClass="letraMediana"></asp:Label>
                                <asp:Label ID="lblV50" runat="server" Text="Valor Invalido." CssClass="mensajeValidador" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblB100" runat="server" Text="Billetes de 100" CssClass="letraMediana"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt100" runat="server" CssClass="textMediano" placeholder="0" ValidationGroup="calculo" OnTextChanged="txt100_TextChanged" AutoPostBack="True"></asp:TextBox>                                
                            </td>
                            <td>
                                <asp:Label ID="lbl100" runat="server" Text="$0.00" CssClass="letraMediana"></asp:Label>
                                <asp:Label ID="lblV100" runat="server" Text="Valor Invalido." CssClass="mensajeValidador" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblB200" runat="server" Text="Billetes de 200" CssClass="letraMediana"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt200" runat="server" CssClass="textMediano" placeholder="0" ValidationGroup="calculo" OnTextChanged="txt200_TextChanged" AutoPostBack="True"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lbl200" runat="server" Text="$0.00" CssClass="letraMediana"></asp:Label>
                                <asp:Label ID="lblV200" runat="server" Text="Valor Invalido." CssClass="mensajeValidador" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblB500" runat="server" Text="Billetes de 500" CssClass="letraMediana"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt500" runat="server" CssClass="textMediano" placeholder="0" ValidationGroup="calculo" OnTextChanged="txt500_TextChanged" AutoPostBack="True"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lbl500" runat="server" Text="$0.00" CssClass="letraMediana"></asp:Label>
                                <asp:Label ID="lblV500" runat="server" Text="Valor Invalido." CssClass="mensajeValidador" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblB1000" runat="server" Text="Billetes de 1000" CssClass="letraMediana"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt1000" runat="server" CssClass="textMediano" placeholder="0" ValidationGroup="calculo" OnTextChanged="txt1000_TextChanged" AutoPostBack="True"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lbl1000" runat="server" Text="$0.00" CssClass="letraMediana"></asp:Label>
                                <asp:Label ID="lblV1000" runat="server" Text="Valor Invalido." CssClass="mensajeValidador" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbltefectivo" runat="server" Text="Total efectivo" CssClass="letraBannerLinks"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txttEfectivo" runat="server" CssClass="textMediano" placeholder="0" ValidationGroup="calculo" OnTextChanged="txt1000_TextChanged" AutoPostBack="True"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblvefectivo" runat="server" Text="$0.00" CssClass="letraBannerLinks" Enabled="False"></asp:Label>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblBCheques" runat="server" Text="Cheques" CssClass="letraMediana"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCheque" runat="server" CssClass="textMediano" placeholder="0.00" ValidationGroup="calculo" OnTextChanged="txtCheque_TextChanged" AutoPostBack="True" Enabled="False"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblCheque" runat="server" Text="$0.00" CssClass="letraMediana"></asp:Label>
                                <asp:Label ID="lblVcheque" runat="server" Text="Valor Invalido." CssClass="mensajeValidador" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblBTarjetas" runat="server" Text="Tarjetas débito" CssClass="letraMediana"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTarjeta" runat="server" CssClass="textMediano" placeholder="0.00" ValidationGroup="calculo" OnTextChanged="txtTarjeta_TextChanged" AutoPostBack="True"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblTarjeta" runat="server" Text="$0.00" CssClass="letraMediana"></asp:Label>
                                <asp:Label ID="lblVtarjeta" runat="server" Text="Valor Invalido." CssClass="mensajeValidador" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblBCredito" runat="server" Text="Tarjetas crédito" CssClass="letraMediana"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCredito" runat="server" CssClass="textMediano" placeholder="0.00" ValidationGroup="calculo"  AutoPostBack="True" OnTextChanged="txtCredito_TextChanged"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblCredito" runat="server" Text="$0.00" CssClass="letraMediana"></asp:Label>
                                <asp:Label ID="lblVCredito" runat="server" Text="Valor Invalido." CssClass="mensajeValidador" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblttarjetas" runat="server" Text="Total tarjetas" CssClass="letraBannerLinks"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtttarjetas" runat="server" CssClass="textMediano" placeholder="0.00" ValidationGroup="calculo"  AutoPostBack="True" Enabled="False" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblvttarjetas" runat="server" Text="$0.00" CssClass="letraBannerLinks" Enabled="False"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Label ID="lblBTransferencia" runat="server" Text="Transferencia" CssClass="letraMediana"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTransferencia" runat="server" CssClass="textMediano" placeholder="0.00" ValidationGroup="calculo" OnTextChanged="txtTransferencia_TextChanged" AutoPostBack="True"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblTransferencia" runat="server" Text="$0.00" CssClass="letraMediana"></asp:Label>
                                <asp:Label ID="lblVTransferencia" runat="server" Text="Valor Invalido." CssClass="mensajeValidador" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblTotal" runat="server" Text="Total:" CssClass="letraMediana"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblTotalImporte" runat="server" Text="" CssClass="letraMediana"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblDif" runat="server" Text="Diferencia:" CssClass="letraMediana"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblDifImporte" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="lblTotalCorte" runat="server" Text="Total:" CssClass="letraSubTitulo"></asp:Label>
                    <asp:Label ID="lblImporteCorte" runat="server" Text="$0.00" CssClass="letraMediana" Width="50px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align:right;" colspan="5">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar Corte" OnClick="btnGuardar_Click" Visible="False" CausesValidation="False" />
                </td>
            </tr>
        </table>


      <%--  <asp:Panel ID="pnlReport" runat="server" Visible="false">
            <div class ="row">                
                <div class="col-md-8">
                    <input type="button" id="printreport" value="imprimir" onclick="printReport('<%=rpt.ClientID %>')"/>                   
                </div>
                 <div class="col-md-4">
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Regresar" />
                 </div>

                <%--<rsweb:ReportViewer ID="rptw" runat="server" Height="500px" Width="900px" ShowPrintButton="true"></rsweb:ReportViewer>
                <rsweb:ReportViewer ID="rpt" runat="server" Height="500px" Width="900px" ShowPrintButton="true"></rsweb:ReportViewer>
            </div>
           </asp:Panel>--%> 
       <%-- <rsweb:ReportViewer ID="rpt" runat="server"  Height="500px" Width="900px" ShowPrintButton="true"></rsweb:ReportViewer>--%>

       <%-- <asp:Panel ID="pnlRecibo" runat="server" Width="800px" Height="500px" BackColor="White">
                <div class="width:100%;margin:1px;">
                    <asp:ImageButton ID="imCerrarRecibo" runat="server" ImageUrl="~/Img/eliminar.png" ImageAlign="Right" OnClick="imCerrarRecibo_Click" />
                </div>
                <iframe id="frameRecibo" runat="server" src="" width="100%" height="100%" style="border: none;" />
        </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="modalRecibo" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnlRecibo" TargetControlID="btnRecibo">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnRecibo" runat="server" />--%>

    </div>

     <asp:Panel ID="pnlRecibo" runat="server" Width="800px" Height="500px" BackColor="White">
                <div class="width:100%;margin:1px;">
                    <asp:ImageButton ID="imCerrarRecibo" runat="server" ImageUrl="~/Img/eliminar.png" ImageAlign="Right" OnClick="imCerrarRecibo_Click" />
                </div>
                <iframe id="frameRecibo" runat="server" src="" width="100%" height="100%" style="border: none;" />
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="modalRecibo" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnlRecibo" TargetControlID="btnRecibo">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnRecibo" runat="server" />


    <uc1:ModalPopupMensaje runat="server" ID="mgs" />
</asp:Content>
