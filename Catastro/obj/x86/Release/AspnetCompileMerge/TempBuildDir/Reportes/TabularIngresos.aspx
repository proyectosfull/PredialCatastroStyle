<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TabularIngresos.aspx.cs" Inherits="Catastro.Reportes.TabularIngresos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 60px; font-family: 'Trebuchet MS'; font-size: 25px; color: #575655;">
                        Tabular de Ingresos</td>
                </tr>
                <tr>
                    <td style="height: 2px">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="padding: 0px 10px 10px 10px;">
                        <table cellpadding="0" cellspacing="0" width="100%" style="height: 316px">
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
                                    <asp:Label ID="Label14" runat="server" Text="Mesa" CssClass="letraMediana"></asp:Label>
                                    <asp:DropDownList ID="ddlMesa" runat="server"  />
                                </td>
                                <td style="height: 53px">
                                    <asp:Label ID="Label15" runat="server" Text="Estatus del recibo:" CssClass="letraMediana"></asp:Label>
                                    <asp:DropDownList ID="ddlStatus" runat="server" class="ddlMediano">
                                        <asp:ListItem Text="TODOS" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="PAGADO" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="CANCELADO" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 371px; height: 52px;">
                                    <asp:Label ID="Label16" runat="server" CssClass="letraMediana" Text="Tipo tramite:"></asp:Label>
                                    <asp:DropDownList ID="ddlTipoTramite" runat="server" />
                                </td>
                                <td style="width: 275px; height: 52px;">
                                    <asp:Label ID="Label4" runat="server" CssClass="letraMediana" style="font-size: small" Text="Tipo de predio:"></asp:Label>
                                    <asp:DropDownList ID="ddlTipo" runat="server" />
                                </td>
                                <td style="height: 52px">
                                    <asp:Label ID="lblCondominio" runat="server" CssClass="letraMediana" Text="Condominio:"></asp:Label>
                                    <asp:DropDownList ID="ddlCondominio" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 371px; height: 53px;">
                                    <asp:Label ID="lblColonia" runat="server" CssClass="letraMediana" Text="Colonia:"></asp:Label>
                                    <asp:TextBox ID="txtColonia" runat="server" CssClass="textExtraGrande" MaxLength="200" placeholder="Colonia" Width="315px"></asp:TextBox>
                                    </td>
                                <td style="height: 53px" colspan="2">
                                    &nbsp;<asp:ImageButton ID="imbBuscar" runat="server" Height="45px" ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" ValidationGroup="buscar" Width="46px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
 <asp:Panel ID="pnlReport" runat="server" Visible="false">
                            <div class="row">
                                <div class="col-md-12">
                                    <input type="button" id="printreport" value="imprimir" onclick="printReport('<%=rpvtPredios.ClientID %>')" />
                                </div>
                            </div>
                        </asp:Panel>
                        <rsweb:reportviewer id="rpvtPredios" runat="server" height="640px" width="1151px" showprintbutton="true" style="margin-right: 191px"></rsweb:reportviewer>
                    



            <asp:Panel ID="pnl" runat="server" class="formPanel">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td style="height: 60px;">
                                        <asp:Label ID="lblbuscarContribuyente" runat="server" Text="Buscar Contribuyente" CssClass="letraTitulo"></asp:Label></td>
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
                                                    <asp:DropDownList ID="ddlFiltroContribuyente" runat="server" />
                                                    &nbsp;<asp:TextBox ID="txtFiltroContribuyente" runat="server" CssClass="textGrande"></asp:TextBox>
                                                    &nbsp;&nbsp;&nbsp;                                      
                                        <asp:ImageButton ID="imbBuscarContribuyenteFiltro" runat="server" CausesValidation="false"
                                            ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscarContribuyenteFiltro_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <tr>
                                        <td>
                                            <div style="height: 200px; overflow: scroll;">
                                                <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False"
                                                    CssClass="grd" DataKeyNames="id,activo" AllowPaging="True"
                                                    AllowSorting="True" BorderStyle="None" ShowFooter="True" OnRowCommand="grd_RowCommand" OnSorting="grd_Sorting" OnPageIndexChanging="grd_PageIndexChanging">
                                                    <Columns>
                                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                                                        <asp:BoundField DataField="ApellidoPaterno" HeaderText="Apellido Paterno" SortExpression="ApellidoPaterno" />
                                                        <asp:BoundField DataField="ApellidoMaterno" HeaderText="Apellido Materno" SortExpression="ApellidoMaterno" />
                                                        <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="135px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgSeleccionar" runat="server" ToolTip="Seleccionar!"
                                                                    ImageUrl="~/img/Activar.png"
                                                                    CssClass="imgButtonGrid"
                                                                    CommandName="SeleccionarPersona"
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
                                                <asp:GridView ID="grdCon" runat="server" AutoGenerateColumns="False"
                                                    CssClass="grd" DataKeyNames="id,activo" AllowPaging="True"
                                                    AllowSorting="True" BorderStyle="None" ShowFooter="True" OnRowCommand="grdCon_RowCommand" OnSorting="grdCon_Sorting" OnPageIndexChanging="grdCon_PageIndexChanging">
                                                    <Columns>
                                                        <asp:BoundField DataField="Descripcion" HeaderText="Nombre" SortExpression="Descripcion" />                                                        
                                                        <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="135px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgSeleccionar" runat="server" ToolTip="Seleccionar!"
                                                                    ImageUrl="~/img/Activar.png"
                                                                    CssClass="imgButtonGrid"
                                                                    CommandName="SeleccionarPersona"
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
                                                <asp:GridView ID="grdCol" runat="server" AutoGenerateColumns="False"
                                                    CssClass="grd" DataKeyNames="id,activo" AllowPaging="True"
                                                    AllowSorting="True" BorderStyle="None" ShowFooter="True" OnRowCommand="grdCol_RowCommand" OnSorting="grdCol_Sorting" OnPageIndexChanging="grdCol_PageIndexChanging">
                                                    <Columns>
                                                        <asp:BoundField DataField="NombreColonia" HeaderText="Nombre" SortExpression="NombreColonia" />                                                        
                                                        <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="135px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgSeleccionar" runat="server" ToolTip="Seleccionar!"
                                                                    ImageUrl="~/img/Activar.png"
                                                                    CssClass="imgButtonGrid"
                                                                    CommandName="SeleccionarPersona"
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
                                            </div>
                                        </td>
                                    </tr>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" OnClick="btnCancel_Click" Text="Cancelar" /></td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <ajaxToolkit:ModalPopupExtender ID="pnl_Modal" runat="server" BackgroundCssClass="ventanaBackground"
                            PopupControlID="pnl" TargetControlID="btnPnl">
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:HiddenField ID="btnPnl" runat="server" />




            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


