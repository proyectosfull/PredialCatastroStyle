<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Certificado.aspx.cs" Inherits="Catastro.Catalogos.Certificado" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
                        <asp:Label ID="Label1" runat="server" Text="Constancias de no Adeudo" CssClass="letraTitulo"></asp:Label></td>
                </tr>
                <tr>
                    <td style="height: 2px">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="padding: 0px 10px 10px 10px; background-color: #b4b4b4">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td colspan="1">
                                    <asp:RadioButton ID="rdbPredial" runat="server" Text="Predial" GroupName="tipoCertificado" Checked="true" AutoPostBack="true" OnCheckedChanged="rdbPredial_CheckedChanged" CssClass="letraMediana" />
                                </td>
                                <td colspan="1">
                                    <asp:RadioButton ID="rdbServicioMun" runat="server" Text="Servicios Municipales" GroupName="tipoCertificado" AutoPostBack="true" OnCheckedChanged="rdbPredial_CheckedChanged" CssClass="letraMediana" Visible="False" />
                                </td>
                                <td colspan="1">
                                    <asp:CheckBox ID="chkImprimirCopia" visible="false" runat="server" Text="Imprimir Copia del Certificado" AutoPostBack="true" OnCheckedChanged="chkImprimirCopia_CheckedChanged" CssClass="letraMediana" />
                                </td>
                                <td colspan="1">
                                    <asp:CheckBox ID="chkBimestreAnterior" runat="server" Text="Bimestre Anterior" AutoPostBack="true" OnCheckedChanged="chkBimestreAnterior_CheckedChanged" CssClass="letraMediana" />
                                </td>
                                 <td colspan="1">
                                    <asp:CheckBox ID="chkGratuito" runat="server" Text="Gratuito" CssClass="letraMediana" AutoPostBack="true" OnCheckedChanged="chkGratuito_CheckedChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1">
                                    <asp:Label ID="Label2" runat="server" CssClass="letraMediana" Text="Clave Catastral:"></asp:Label>
                                </td>
                                <td colspan="1">
                                    <asp:TextBox ID="txtClvCastatral" runat="server" CssClass="textGrande" MaxLength="12"  placeholder="Clave Catastral"></asp:TextBox>                                    
                                    <%--<ajaxToolkit:MaskedEditExtender  runat="server" ID="meeFiltro" Mask="9999-99-999-999" MaskType="Number"  InputDirection="LeftToRight" TargetControlID="txtClvCastatral" />--%>
                                    <ajaxToolkit:MaskedEditExtender runat="server" ID="meeFiltro" Mask="9999-99-999-999" TargetControlID="txtClvCastatral" MaskType="Number" InputDirection="RightToLeft" />
                                </td>
                                 <td colspan="1">
                                    <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="False" ImageUrl="~/img/consultar.fw.png" OnClick="buscarClaveCatastral" />                                   
                                </td>
                                <td colspan="1">

                                    <asp:Label ID="Label3" runat="server" CssClass="letraMediana" Text="Calcular adeudo hasta la fecha:"></asp:Label>
                                </td>
                                <td colspan="1">
                                    <asp:TextBox ID="txtFechaAdeudo" runat="server" CssClass="textMediano" MaxLength="50" ReadOnly="true" placeholder="Fecha Final"></asp:TextBox>
                                    <%--<ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" BehaviorID="txtFecha_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaAdeudo" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="txtFecha_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFechaAdeudo" Format="dd/MM/yyyy" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="valida" ControlToValidate="txtFechaAdeudo" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="agregarConcepto"></asp:RequiredFieldValidator>--%>
                                </td>                               
                            </tr>
                            <tr>
                                <td colspan="5" style="padding: 0px 10px 10px 10px;">
                                    <br />
                                    <asp:GridView ID="grd" Visible="false" runat="server" AutoGenerateColumns="False"
                                        CssClass="grd" DataKeyNames="id,activo" AllowPaging="True"
                                        AllowSorting="True" BorderStyle="None" ShowFooter="True"
                                        OnRowCommand="grd_RowCommand" OnSorting="grd_Sorting" OnPageIndexChanging="grd_PageIndexChanging" 
                                        OnRowDataBound="grd_RowDataBound" PageSize="3">
                                        <Columns>
                                            <asp:BoundField DataField="ClavePredial" HeaderText="Clave Predial" SortExpression="ClavePredial" />
                                            <asp:BoundField DataField="Periodo" HeaderText="Perido" SortExpression="Periodo" />
                                            <asp:BoundField DataField="Adeudo" HeaderText="Adeudo" SortExpression="Adeudo" DataFormatString="{0:C}" />
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
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: left">
                                    <br />
                                    <asp:Label ID="lblMensajeTPendiente" runat="server"
                                        CssClass="letraBannerLinks" ForeColor="Blue" Visible="False"></asp:Label></td>
                                <td>
                                    <asp:Button ID="btnPrepago" runat="server" CausesValidation="False" Text="Prepago" OnClick="btnPrepago_Click" Visible="false" /></td>
                                <td>
                                    <asp:Button ID="btnImprimir" runat="server" CausesValidation="False" Text="Imprimir" OnClick="btnImprimir_Click" Visible="false" /></td>
                                <td>
                                    <asp:Button ID="btnRecargar" runat="server" CausesValidation="False" OnClick="btnRecargar_Click" Text="Recargar" /></td>
                            </tr>
                        </table>
                    </td>
                    <tr>
                        <td>
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td colspan="5" style="text-align: center">
                                        <asp:Label ID="lblMensajeAdeudo" runat="server" Text=""
                                            CssClass="letraTitulo" ForeColor="Black" Visible="false"></asp:Label></td>
                                    <caption>
                                        <br />
                                    </caption>
                                </tr>
                                <tr>
                                    <td colspan="5" style="text-align: center">
                                        <br />
                                        <asp:Label ID="lblDirectorPredial" runat="server" Text="" Visible="false"></asp:Label></td>
                                    <caption>
                                        <br />
                                    </caption>
                                </tr>
                                <tr>
                                    <td colspan="5" style="text-align: center">
                                        <br />
                                        <asp:Label ID="lblCertifica" runat="server" Text="CERTIFICA:"
                                            CssClass="letraTitulo" ForeColor="Black" Visible="false"></asp:Label></td>
                                    <caption>
                                        <br />
                                    </caption>
                                </tr>
                                <tr>
                                    <td colspan="5" style="text-align: center">
                                        <br />
                                        <asp:Label ID="lblFechaEncabezado" runat="server" Text="" Visible="false"></asp:Label></td>
                                    <caption>
                                        <br />
                                        <br />
                                    </caption>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <br />
                                        <asp:Label ID="lblNombre" runat="server" Text="NOMBRE DEL PROPIETARIO:" CssClass="letraSubTitulo" Visible="false"></asp:Label>
                                        <asp:Label ID="lblNombreText" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:Label ID="lblCuentaCatastral" runat="server" Text="CUENTA CATASTRAL:" CssClass="letraSubTitulo" Visible="false"></asp:Label>
                                        <asp:Label ID="lblCuentaCatastralText" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:Label ID="lblUbicacion" runat="server" Text="UBICADO EN:" CssClass="letraSubTitulo" Visible="false"></asp:Label>
                                        <asp:Label ID="lblUbicacionText" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:Label ID="lblSuperficiePredio" runat="server" Text="SUPERFICIE DEL PREDIO :" CssClass="letraSubTitulo" Visible="false"></asp:Label>
                                        <asp:Label ID="lblSuperficiePredioText" runat="server" Text="" Visible="false"></asp:Label>
                                        <asp:Label ID="lblSuperficiePredioLetra" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:Label ID="lblSuperficieCons" runat="server" Text="SUPERFICIE DE CONSTRUCCIÓN :" CssClass="letraSubTitulo" Visible="false"></asp:Label>
                                        <asp:Label ID="lblSuperficieConsText" runat="server" Text="" Visible="false"></asp:Label>
                                        <asp:Label ID="lblSuperficieConsLetra" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:Label ID="lblValorCast" runat="server" Text="VALOR CATASTRAL :" CssClass="letraSubTitulo" Visible="false"></asp:Label>
                                        <asp:Label ID="lblValorCastText" runat="server" Text="" Visible="false"></asp:Label>
                                        <asp:Label ID="lblValorCastLetra" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:Label ID="lblMetrosLineales" runat="server" Text="METROS LINEALES DE  FRENTE :" CssClass="letraSubTitulo" Visible="false"></asp:Label>
                                        <asp:Label ID="lblMetrosLinealesText" runat="server" Text="" Visible="false"></asp:Label>
                                        <asp:Label ID="lblMetrosLinealesLetra" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:Label ID="lblZona" runat="server" Text="ZONA :" CssClass="letraSubTitulo" Visible="false"></asp:Label>
                                        <asp:Label ID="lblZonaText" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:Label ID="lblPeriodoPago" runat="server" Text="ULTIMO PERIODO DE PAGO :" CssClass="letraSubTitulo" Visible="false"></asp:Label>
                                        <asp:Label ID="lblPeriodoPagoText" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:Label ID="lblRecibo" runat="server" Text="RECIBO OFICIAL :" CssClass="letraSubTitulo" Visible="false"></asp:Label>
                                        <asp:Label ID="lblREciboText" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:Label ID="lblFechaPago" runat="server" Text="FECHA DE PAGO :" CssClass="letraSubTitulo" Visible="false"></asp:Label>
                                        <asp:Label ID="lblFechaPagoText" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5" style="text-align: center">
                                        <br />
                                        <asp:Label ID="lblMensajeAdeudo1" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5" style="text-align: center">
                                        <br />
                                        <asp:Label ID="lblMensajePie" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                        </td>
            </table>
            </tr>
                    </tr>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
            </table>
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
            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
