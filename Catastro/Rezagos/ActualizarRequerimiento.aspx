<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ActualizarRequerimiento.aspx.cs" Inherits="Catastro.Requerimientos.ActualizarRequerimiento" %>

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
                    <td>
                        <br />
                        <br />
                        <asp:Label ID="lblTitulo" runat="server" CssClass="textModalTitulo2" Text="Actualizar Requerimientos"></asp:Label>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="height: 2px">
                        <asp:HiddenField ID="hdfId" runat="server" />
                        <asp:HiddenField ID="hdfIdPredio" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 70%">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 250px">
                                    <asp:RadioButton ID="rbnUnificado" runat="server" GroupName="tipoServicio" AutoPostBack="True" OnCheckedChanged="rbnUnificado_CheckedChanged" Text="Unificado" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rbtIPSM" runat="server" GroupName="tipoServicio" AutoPostBack="True" Text="Predial y Servicios Municipales" OnCheckedChanged="rbtIPSM_CheckedChanged" Checked="True" />
                                </td>
                                <td>&nbsp;</td>
                                <td style="width: 250px">&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 250px">
                                    <asp:Label ID="lblClave" runat="server" CssClass="letraMediana" Text="Clave Predial"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtClvCastatral" runat="server" OnTextChanged="txtClvCastatral_TextChanged"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender runat="server" ID="meeFiltro" Mask="9999-99-999-999" MaskType="Number" InputDirection="LeftToRight" TargetControlID="txtClvCastatral" />
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="txtClvCastatral" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="buscar"></asp:RequiredFieldValidator>                                --%>&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" Text="Buscar" ValidationGroup="buscar" />
                                </td>
                                <td>&nbsp;</td>
                                <td style="width: 250px"></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width: 250px">
                                    <asp:Label ID="lblContribuyente" runat="server" Text="Contribuyente" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtContribuyente" runat="server" CssClass="textGrande" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width: 250px">
                                    <asp:Label ID="lblEmision" runat="server" Text="Emisión" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaEmision" runat="server" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 250px">
                                    <asp:Label ID="lblUbicacion" runat="server" Text="Ubicación" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtUbicacion" runat="server" Enabled="false" CssClass="textExtraGrande"></asp:TextBox>
                                </td>
                                <td style="width: 250px">
                                    <asp:Label ID="lblLimite" runat="server" Text="Limite" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaLimite" runat="server" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 250px">
                                    <asp:Label ID="lblDocumento" runat="server" Text="Documento" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtDocumento" runat="server" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width: 250px">&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False"
                        CssClass="grd" DataKeyNames="Folio" AllowPaging="True"
                        AllowSorting="True" BorderStyle="None" ShowFooter="True"
                        OnRowCommand="grd_RowCommand" OnSorting="grd_Sorting" OnPageIndexChanging="grd_PageIndexChanging"
                        OnRowDataBound="grd_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio" />
                            <asp:BoundField DataField="FechaEmision" HeaderText="Fecha Emisión" SortExpression="FechaEmision" />
                            <asp:BoundField DataField="Inicial" HeaderText="Año-Bimestre Inicial" SortExpression="Inicial" />
                            <asp:BoundField DataField="Final" HeaderText="Año-Bimestre Final" SortExpression="Final" />
                            <asp:BoundField DataField="Importe" HeaderText="Importe" SortExpression="Importe" />
                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                            <asp:BoundField DataField="Activo" HeaderText="Activo" SortExpression="Activo" Visible="false" />
                            <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="135px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgConsulta" runat="server" ToolTip="Consultar!"
                                        ImageUrl="~/img/consultar.fw.png"
                                        CssClass="imgButtonGrid"
                                        CommandName="ConsultarRegistro"
                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                        <%--CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Folio")%>' />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmptySearch" runat="server" CssClass="letraTituloEmptyGrid"> Sin Requerimiento </asp:Label>
                        </EmptyDataTemplate>
                        <FooterStyle CssClass="grdFooter" />
                        <HeaderStyle CssClass="grdHead" />
                        <RowStyle CssClass="grdRowPar" />
                    </asp:GridView>
                </tr>
                <caption>
                    <hr />
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td colspan="5" style="align-content: center;  background-color: lightblue; height: 22px;">
                                        <asp:Label ID="lblPeridos" runat="server" class="letraMediana" Height="40px" Text="PERIODOS DE PAGO"></asp:Label>
                                    </td>
                                    <td style="width: 20px; height: 22px;"></td>
                                    <td colspan="2" style="align-content: center; background-color: lightblue; height: 22px;">
                                        <asp:Label ID="lblFase" runat="server" class="letraMediana" Height="16px" Text="FASE DEL DOCUMENTO" Width="249px"></asp:Label>
                                    </td>
                                </tr>
                                <%--<tr>
                                <td style="height: 50px;">
                                    <asp:Label ID="lblPredial" runat="server" Text="Predial" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPIIP" runat="server" CssClass="textChico" Enabled="false"></asp:TextBox>
                                    &nbsp;<asp:TextBox ID="txtPFIP" runat="server" CssClass="textChico" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width: 20px"></td>
                                 <td>
                                    <asp:Label ID="lblMunicipales" runat="server" Text="SM" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPISM" runat="server" CssClass="textChico" Enabled="false"></asp:TextBox>
                                    &nbsp;<asp:TextBox ID="txtPFSM" runat="server" CssClass="textChico" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width: 20px"></td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtFase" runat="server" CssClass="textGrande" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>--%>
                                <tr>
                                    <td class="letraMediana" colspan="2" style="vertical-align: text-top;">
                                        <div id="divIzq" runat="server" style="width: 100%">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="height: 50px;">
                                                        <asp:Label ID="lblPredial" runat="server" CssClass="letraMediana" Text="Predial"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPIIP" runat="server" CssClass="textChico" Enabled="false"></asp:TextBox>
                                                        &nbsp;<asp:TextBox ID="txtPFIP" runat="server" CssClass="textChico" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr style="align-content: center; background-color: lightgray !important;">
                                                    <td class="letraMediana">Estado de Cuenta</td>
                                                    <td>
                                                        <br />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblImpuestoIP" runat="server" CssClass="letraMediana" Text="Impuesto"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtImpuestoIP" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAdicionalesIP" runat="server" CssClass="letraMediana" Text="Adicionales"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAdicionalesIP" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblRecargosIP" runat="server" CssClass="letraMediana" Text="Recargos"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRecargoIP" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblRezagosIP" runat="server" CssClass="letraMediana" Text="Rezagos"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRezagoIP" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDiferenciasIP" runat="server" CssClass="letraMediana" Text="Diferencias"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDiferenciasIP" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblRecIP" runat="server" CssClass="letraMediana" Text="Rec Diferencias"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRecIP" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>
                                                        <br />
                                                        <br />
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td>
                                                        <asp:Label ID="lblHonorarioIP" runat="server" CssClass="letraMediana" Text="Honorarios de Not."></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtHonorarioIP" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblEjecucionIP" runat="server" CssClass="letraMediana" Text="Ejecución"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEjecucionIP" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblMultaIP" runat="server" CssClass="letraMediana" Text="Multa"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtMultaIP" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <div id="divDAP" runat="server" style="width: 100%">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblDAP0" runat="server" CssClass="letraMediana" Text="DAP"></asp:Label>
                                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtDAP0" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblRecoleccion" runat="server" CssClass="letraMediana" Text="Recolección" Visible="False"></asp:Label>
                                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRecoleccion" runat="server" CssClass="textMediano" Enabled="false" Visible="False"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblImporteIP" runat="server" CssClass="letraMediana" Text="Importe"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtImporteIP" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblNumPagoIP" runat="server" CssClass="letraMediana" Text="Num. Pago"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNumPagoIP" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblFechaPagoIP" runat="server" CssClass="letraMediana" Text="Fecha pago"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFechaPagoIP" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblReciboIP" runat="server" CssClass="letraMediana" Text="Recibo"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtReciboIP" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                    <td style="width: 45px">&nbsp;</td>
                                    <td class="letraMediana" colspan="2" style="vertical-align: top; width: 452px;">
                                        <div id="divDer" runat="server"  style="width: 100% ">
                                            <table class="nav-justified">
                                                <tr>
                                                    <td style="height: 50px;">
                                                        <asp:Label ID="lblMunicipales" runat="server" CssClass="letraMediana" Text="SM"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPISM" runat="server" CssClass="textChico" Enabled="false"></asp:TextBox>
                                                        &nbsp;<asp:TextBox ID="txtPFSM" runat="server" CssClass="textChico" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr style="align-content: center; background-color: lightgray !important;">
                                                    <td class="letraMediana">Estado de Cuenta</td>
                                                    <td>
                                                        <br />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblImpuestoSM" runat="server" CssClass="letraMediana" Text="Impuesto"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtImpuestoSM" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAdicionalesSM" runat="server" CssClass="letraMediana" Text="Adicionales"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAdicionalesSM" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblrecargosSM" runat="server" CssClass="letraMediana" Text="Recargos"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRecargosSM" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblRezagosSM" runat="server" CssClass="letraMediana" Text="Rezagos"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRezagoSM" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDAP" runat="server" CssClass="letraMediana" Text="DAP"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDAP" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><asp:Label ID="lblLimpiezaSM" runat="server" CssClass="letraMediana" Text="Limpieza"></asp:Label></td>
                                                    <td>
                                                     <asp:TextBox ID="txtLimpiezaSM" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><asp:Label ID="lblRecoleccionSM" runat="server" CssClass="letraMediana" Text="Recolección"></asp:Label></td>
                                                    <td>
                                                     <asp:TextBox ID="txtRecoleccionSM" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblHonorarioSM" runat="server" CssClass="letraMediana" Text="Honorarios de Not."></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtHonorarioSM" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblEjecucionSM" runat="server" CssClass="letraMediana" Text="Ejecución"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEjecucionSM" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblMultaSM" runat="server" CssClass="letraMediana" Text="Multa"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtMultaSM" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblImporteSM" runat="server" CssClass="letraMediana" Text="Importe"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtImporteSM" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblNumPagoSM" runat="server" CssClass="letraMediana" Text="Num. Pago"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNumPagoSM" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblFechaPagoSM" runat="server" CssClass="letraMediana" Text="Fecha pago"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFechaPagoSM" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblReciboSM" runat="server" CssClass="letraMediana" Text="Recibo"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtReciboSM" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                    <td style="width: 20px">&nbsp;</td>
                                    <td colspan="1" style="vertical-align:top;">
                                        <table style="vertical-align: top; width: 100%">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtFase" runat="server" CssClass="textGrande" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="letraMediana">
                                                    <asp:Label ID="lblFolioOficio" runat="server" CssClass="letraMediana" Text="Folio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFolioOficio" runat="server" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="letraMediana">
                                                    <asp:Label ID="lblStatusPredial" runat="server" CssClass="letraMediana" Text="Estatus Predial"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlStatusPredial" runat="server">
                                                        <asp:ListItem Value="-">-</asp:ListItem>
                                                        <asp:ListItem Value="A">Activo</asp:ListItem>
                                                        <asp:ListItem Value="C">Cancelado</asp:ListItem>
                                                        <asp:ListItem Value="P">Pagado</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblStatusSM" runat="server" CssClass="letraMediana" Text="Estatus Servicios"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlStatusServicios" runat="server">
                                                        <asp:ListItem Value="-">-</asp:ListItem>
                                                        <asp:ListItem Value="A">Activo</asp:ListItem>
                                                        <asp:ListItem Value="C">Cancelado</asp:ListItem>
                                                        <asp:ListItem Value="P">Pagado</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTipoDoc" runat="server" CssClass="letraMediana" Text="Tipo Documento:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlTipoDocumento" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </tr>
                                            <tr>
                                                <td class="letraMediana" colspan="2" style="align-content: center; background-color: lightgray !important;">
                                                    <asp:Label ID="lblResultado" runat="server" class="letraMediana" Text="Resultado de la Diligencia"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblAgente" runat="server" CssClass="letraMediana" Text="Agente:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlAgente" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblOficio" runat="server" CssClass="letraMediana" Text="Oficio:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOficio" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblFecha" runat="server" CssClass="letraMediana" Text="Fecha recepción:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFechaRecepcion" runat="server"></asp:TextBox>
                                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" BehaviorID="txtFecha_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaRecepcion" />
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" BehaviorID="txtFecha_CalendarExtender" CssClass="CalendarCSS" Format="dd/MM/yyyy" TargetControlID="txtFechaRecepcion" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblFirmante" runat="server" CssClass="letraMediana" Text="Caracter del firmante:"></asp:Label>
                                                </td>
                                                <td><%--<asp:TextBox ID="txtFirmante" runat="server"></asp:TextBox>--%>
                                                    <asp:DropDownList ID="ddlFirmante" runat="server">
                                                        <asp:ListItem>Personal</asp:ListItem>
                                                        <asp:ListItem>Por Estrado</asp:ListItem>
                                                        <asp:ListItem>Por Correo</asp:ListItem>
                                                        <asp:ListItem>Por Edicto</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblObservaciones" runat="server" CssClass="letraMediana" Text="Observaciones:"></asp:Label>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtObservaciones" runat="server" Height="78px" TextMode="MultiLine" Width="369px" MaxLength="2000"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblFechaEstrado" runat="server" CssClass="letraMediana" Text="Fecha Estrado:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFechaEstrado" runat="server"></asp:TextBox>
                                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" runat="server" BehaviorID="txtFechaEstrado_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaEstrado" />
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" BehaviorID="txtFechaEstrado_CalendarExtender" CssClass="CalendarCSS" Format="dd/MM/yyyy" TargetControlID="txtFechaEstrado" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td style="width: 20px">&nbsp;</td>
                        <td>
                            <td></td>
                            <td style="width: 20px">&nbsp;</td>
                            <td></td>
                            <td></td>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Button ID="btnReimprimir" runat="server" CausesValidation="False" OnClick="btnReimprimir_Click" Text="Reimprimir" />
                        </td>
                        <td style="width: 20px">&nbsp;</td>
                        <td>
                            <asp:Button ID="btnGuardar" runat="server" CausesValidation="False" OnClick="btnGuardar_Click" Text="Guardar" />
                        </td>
                        <td style="width: 20px">&nbsp;</td>
                        <td>
                            <asp:Button ID="btnCancelar" runat="server" CausesValidation="False" OnClick="btnCancelar_Click" Text="Cancelar" />
                        </td>
                        <td></td>
                    </tr>
                </caption>

            </table>

            <hr />            
            <asp:Panel ID="pnlRecibo" runat="server" Width="800px" Height="500px" BackColor="White" HorizontalAlign="Center">
                <div class="width:100%;margin:1px;" runat="server" id="divCerrarFactura" visible="true">
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Img/eliminar.png" OnClick="ImageButton1_Click" ImageAlign="Right" />
                </div>
                <iframe id="frameRecibo" runat="server" src="" width="100%" height="90%" style="border: none;" />
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="modalRecibo" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnlRecibo" TargetControlID="btnRecibo">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnRecibo" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc1:ModalPopupMensaje runat="server" ID="vtnModal" DysplayAceptar="True" DysplayCancelar="False" />
</asp:Content>
