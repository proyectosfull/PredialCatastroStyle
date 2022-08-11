<<<<<<< HEAD:Catastro/Catastro/Rezagos/Requerimiento.aspx
﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Requerimiento.aspx.cs" Inherits="Catastro.Requerimiento" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .selectChico {
            min-width: 0px !important;
        }
    </style>
    <br />
    <ajaxToolkit:TabContainer ID="TabContainerRequerimiento" runat="server" ActiveTabIndex="0">
        <ajaxToolkit:TabPanel ID="TabFiltro" runat="server" HeaderText="Selección de Candidatos">
            <ContentTemplate>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>

                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td style="height: 60px;">
                                    <asp:Label ID="lblTitulo" runat="server" CssClass="textModalTitulo2" Text="Requerimientos"></asp:Label>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 2px">
                                    <asp:HiddenField ID="hdfId" runat="server" />
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 70%">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 250px">
                                                <asp:Label ID="txtAdeudo" runat="server" Text="Tengan un adeudo hasta el periodo:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList ID="ddlBimestre" runat="server" CssClass="selectChico" Width="110px">
                                                    <asp:ListItem Selected="True" Value="0">Bimestre</asp:ListItem>
                                                    <asp:ListItem>1</asp:ListItem>
                                                    <asp:ListItem>2</asp:ListItem>
                                                    <asp:ListItem>3</asp:ListItem>
                                                    <asp:ListItem>4</asp:ListItem>
                                                    <asp:ListItem>5</asp:ListItem>
                                                    <asp:ListItem>6</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" InitialValue="0" ControlToValidate="ddlBimestre" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                                                /
                                                <asp:DropDownList ID="ddlAnio" runat="server" CssClass="selectChico" Width="110px"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="valida" InitialValue="0" ControlToValidate="ddlAnio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 40px"></td>
                                            <td rowspan="4" style="vertical-align: top; width: 35%; text-align: center">
                                                <asp:GridView ID="grdResultados" runat="server" AutoGenerateColumns="False" CssClass="grd" DataKeyNames="IdPredio,IdTipoPredio" OnRowDataBound="grdResultados_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="ClavePredial" HeaderText="Clave Catastral" />
                                                        <asp:BoundField DataField="IdTipoPredio" HeaderText="Tipo" />
                                                        <asp:BoundField DataField="Periodo" HeaderText="Periodo" />
                                                    </Columns>
                                                    <FooterStyle CssClass="grdFooter" />
                                                    <HeaderStyle CssClass="grdHead" />
                                                    <RowStyle CssClass="grdRowPar" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkBaldios" runat="server" Text=" Ignorar los predios baldíos" CssClass="letraMediana" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkExentos" runat="server" Text=" Solo exentos" CssClass="letraMediana" />
                                            </td>
                                            <td style="width: 40px"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkPExentos" runat="server" Text=" Ignorar los predíos exentos" CssClass="letraMediana" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkUBaldios" runat="server" Text=" Únicamente baldíos" CssClass="letraMediana" />
                                            </td>
                                            <td style="width: 40px"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblRango" runat="server" Text="Rango de Claves:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtRangoDe" runat="server" CssClass="textMediano" MaxLength="12" placeholder="Rago de"></asp:TextBox>
                                                -
                                                <asp:TextBox ID="txtRangoA" runat="server" CssClass="textMediano" MaxLength="12" placeholder="Rago a"></asp:TextBox>
                                            </td>
                                            <td style="width: 40px"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblContribuyente" runat="server" Text="Contribuyente:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtContribuyente" runat="server" CssClass="textExtraGrande" MaxLength="200" placeholder="Contribuyente"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="False"
                                                    ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" />
                                                <asp:HiddenField ID="hdfIdCon" runat="server" />
                                            </td>
                                            <td style="width: 40px"></td>
                                            <td>
                                                <asp:Label ID="lblFecha" runat="server" Text="Fecha en que se va a requerir:" CssClass="letraMediana"></asp:Label>
                                                <asp:TextBox ID="txtFecha" runat="server" CssClass="textMediano" MaxLength="200" placeholder="Fecha"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="txtFecha" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="generar"></asp:RequiredFieldValidator>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" BehaviorID="txtFecha_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFecha" Format="dd/MM/yyyy" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lblClaves" runat="server" Text="Claves:" CssClass="letraMediana"></asp:Label>
                                                <asp:Label ID="lblClaveExp" runat="server" Text="(Listado de claves separadas por coma, por ejemplo: 110001001001,110001001002,110001001003)" CssClass="letraChica"></asp:Label>
                                                <br />
                                                <asp:TextBox ID="txtClave" runat="server" CssClass="textExtraGrande" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                            <td style="width: 40px"></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td colspan="2"></td>
                                            <td style="width: 40px"></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" ValidationGroup="buscar" />
                                            </td>
                                            <td style="width: 40px"></td>
                                            <td style="text-align: right">
                                                <asp:Button ID="btnGenerar" runat="server" Text="Generar" OnClick="btnGenerar_Click" ValidationGroup="generar" /></td>
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
                                <td>
                                    <%--<asp:Button ID="btnCancelar" runat="server" CausesValidation="False" OnClick="btnCancelar_Click" Text="Regresar" />
                        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" OnClick="btnCancel_Click" Text="Cancelar" Visible="False" />--%>
                                </td>
                            </tr>
                        </table>
                        <%--Modal para busqueda de contribuyente--%>
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
                                        <tr>
                                            <td>
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

                                            </td>
                                        </tr>
                                    </td>
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
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="TabEdoCta" runat="server" HeaderText="Estado de Cuenta">
            <ContentTemplate>
                <ajaxToolkit:TabContainer ID="TabContainerEdoCta" runat="server" ActiveTabIndex="0">
                    <ajaxToolkit:TabPanel ID="TabRequerir" runat="server" HeaderText="Requerir">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblRRango" runat="server" Text="Rango:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRRangoA" runat="server" CssClass="textMediano"></asp:TextBox>
                                        a
                                        <asp:TextBox ID="txtRRanggoB" runat="server" CssClass="textMediano"></asp:TextBox>
                                        <%--<ajaxToolkit:NumericUpDownExtender ID="nudRangoA" runat="server"
                                        TargetControlID="txtRRangoA" Width="100" Step="1">
                                        </ajaxToolkit:NumericUpDownExtender> --%>                             
                                    </td>
                                    <td style="width: 40px"></td>
                                    <td>
                                        <asp:Label ID="lblIniciar" runat="server" Text="Inicio Folio en:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFolio" runat="server" CssClass="textMediano"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="valida" ControlToValidate="txtFolio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="requerir"></asp:RequiredFieldValidator>
                                        <%--<ajaxToolkit:NumericUpDownExtender ID="nudRangoA" runat="server"
                                        TargetControlID="txtRRangoA" Width="100" Step="1">
                                        </ajaxToolkit:NumericUpDownExtender> --%>                             
                                    </td>
                                    <td style="width: 40px"></td>
                                    <td>
                                        <asp:Button ID="btnRequerir" runat="server" Text="Requerir" OnClick="btnRequerir_Click" ValidationGroup="requerir" />
                                    </td>
                                    <td style="width: 40px"></td>
                                    <td>
                                        <asp:Label ID="lblTarjetas" runat="server" Text="No. Tarjetas Impresas:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTarjetas" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="TabAsignar" runat="server" HeaderText="Asignar">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSelec" runat="server" Text="Selec."></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSelec" runat="server"></asp:DropDownList>
                                    </td>
                                    <td style="width: 40px"></td>
                                    <td>
                                        <asp:Label ID="lblAsignar" runat="server" Text="Asignar a"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAsignar" runat="server"></asp:DropDownList>
                                    </td>
                                    <td style="width: 10px"></td>
                                    <td>
                                        <asp:Button ID="btnAsignar" runat="server" Text="Asignar" OnClick="btnAsignar_Click" /></td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                </ajaxToolkit:TabContainer>
                <div style="overflow: scroll">
                    <asp:GridView ID="grdEdoCta" runat="server" AutoGenerateColumns="False" CssClass="grd" DataKeyNames="IdPredio, Infraestructura, Limpieza, DAP, Recoleccion">
                        <Columns>
                            <asp:BoundField DataField="ClavePredial" HeaderText="Clave Catastral" />
                            <asp:BoundField DataField="Ubicacion" HeaderText="Ubicación" />
                            <asp:BoundField DataField="Contribuyente" HeaderText="Contribuyente" />
                            <asp:BoundField DataField="Periodo" HeaderText="Periodo Requerido" />
                            <asp:BoundField DataField="Fase" HeaderText="Fase" />
                            <asp:BoundField DataField="Impuesto" HeaderText="Impuesto" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="Adicionales" HeaderText="Adicionales" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="Rezagos" HeaderText="Rezagos" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="Recargos" HeaderText="Recargos" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="Diferencias" HeaderText="Diferencias" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="Rec" HeaderText="Rec. Difere." DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="Ejecucion" HeaderText="Ejecución" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="Base" HeaderText="Base Gravable" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="Multas" HeaderText="Multas" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="SM" HeaderText="SM" />
                            <asp:BoundField DataField="ImpuestoSM" HeaderText="Impuesto" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="AdicionalesSM" HeaderText="Adicionales" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="RezagosSM" HeaderText="Rezagos" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="RecargosSM" HeaderText="Recargos" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="EjecuciónSM" HeaderText="Ejecución" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="MultasSM" HeaderText="Multas" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="Agente" HeaderText="Agente" />
                            <asp:BoundField DataField="Folio" HeaderText="Folio Req." />
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                            <asp:TemplateField ControlStyle-Width="0px">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdfIdIP" runat="server" />
                                    <asp:HiddenField ID="hdfIdSM" runat="server" />                                                                     
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="grdFooter" />
                        <HeaderStyle CssClass="grdHead" />
                        <RowStyle CssClass="grdRowPar" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
    <uc1:ModalPopupMensaje runat="server" ID="vtnModal" DysplayAceptar="True" DysplayCancelar="True" />
</asp:Content>
=======
﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Requerimiento.aspx.cs" Inherits="Catastro.Requerimiento" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .selectChico {
            min-width: 0px !important;
        }
    </style>
    <br />
    <br />
    <ajaxToolkit:TabContainer ID="TabContainerRequerimiento" runat="server" ActiveTabIndex="0">
        <ajaxToolkit:TabPanel ID="TabFiltro" runat="server" HeaderText="Selección de Candidatos">
            <ContentTemplate>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>

                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td style="height: 30px;">
                                    <asp:Label ID="lblTitulo" runat="server" CssClass="textModalTitulo2" Text="Requerimientos"></asp:Label>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 2px">
                                    <asp:HiddenField ID="hdfId" runat="server" />
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 70%">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 250px">
                                                <asp:Label ID="txtAdeudo" runat="server" Text="Tengan un adeudo hasta el periodo:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList ID="ddlBimestre" runat="server" CssClass="selectChico" Width="110px">
                                                    <asp:ListItem Selected="True" Value="0">Bimestre</asp:ListItem>
                                                    <asp:ListItem>1</asp:ListItem>
                                                    <asp:ListItem>2</asp:ListItem>
                                                    <asp:ListItem>3</asp:ListItem>
                                                    <asp:ListItem>4</asp:ListItem>
                                                    <asp:ListItem>5</asp:ListItem>
                                                    <asp:ListItem>6</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" InitialValue="0" ControlToValidate="ddlBimestre" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                                                /
                                                <asp:DropDownList ID="ddlAnio" runat="server" CssClass="selectChico" Width="110px"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="valida" InitialValue="0" ControlToValidate="ddlAnio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 40px"></td>
                                            <td rowspan="5" style="vertical-align: top; width: 35%; text-align: center">
                                                <asp:GridView ID="grdResultados" runat="server" AutoGenerateColumns="False" CssClass="grd" DataKeyNames="IdPredio,IdTipoPredio" OnRowDataBound="grdResultados_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="ClavePredial" HeaderText="Clave Catastral" />
                                                        <asp:BoundField DataField="IdTipoPredio" HeaderText="Tipo" />
                                                        <asp:BoundField DataField="Periodo" HeaderText="Periodo" />
                                                    </Columns>
                                                    <FooterStyle CssClass="grdFooter" />
                                                    <HeaderStyle CssClass="grdHead" />
                                                    <RowStyle CssClass="grdRowPar" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td >
                                                <asp:Label ID="lblDocumento" runat="server" Text="Con Documento:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList ID="ddlDocumento" runat="server"></asp:DropDownList>
                                            </td>
                                            <td style="width: 40px"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkBaldios" runat="server" Text=" Ignorar los predios baldíos" CssClass="letraMediana" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkExentos" runat="server" Text=" Solo exentos" CssClass="letraMediana" />
                                            </td>
                                            <td style="width: 40px"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkPExentos" runat="server" Text=" Ignorar los predíos exentos" CssClass="letraMediana" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkUBaldios" runat="server" Text=" Únicamente baldíos" CssClass="letraMediana" />
                                            </td>
                                            <td style="width: 40px"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblRango" runat="server" Text="Rango de Claves:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtRangoDe" runat="server" CssClass="textMediano" MaxLength="12" placeholder="Rago de"></asp:TextBox>
                                                -
                                                <asp:TextBox ID="txtRangoA" runat="server" CssClass="textMediano" MaxLength="12" placeholder="Rago a"></asp:TextBox>
                                            </td>
                                            <td style="width: 40px"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblContribuyente" runat="server" Text="Contribuyente:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtContribuyente" runat="server" CssClass="textExtraGrande" MaxLength="200" placeholder="Contribuyente"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="False"
                                                    ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" />
                                                <asp:HiddenField ID="hdfIdCon" runat="server" />
                                            </td>
                                            <td style="width: 40px"></td>
                                            <td>
                                                <asp:Label ID="lblFecha" runat="server" Text="Fecha en que se va a requerir:" CssClass="letraMediana"></asp:Label>
                                                <asp:TextBox ID="txtFecha" runat="server" CssClass="textMediano" MaxLength="200" placeholder="Fecha"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="txtFecha" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="generar"></asp:RequiredFieldValidator>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" BehaviorID="txtFecha_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFecha" Format="dd/MM/yyyy" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lblClaves" runat="server" Text="Claves:" CssClass="letraMediana"></asp:Label>
                                                <asp:Label ID="lblClaveExp" runat="server" Text="(Listado de claves separadas por coma, por ejemplo: 110001001001,110001001002,110001001003)" CssClass="letraChica"></asp:Label>
                                                <br />
                                                <asp:TextBox ID="txtClave" runat="server" CssClass="textExtraGrande" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                            <td style="width: 40px"></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td colspan="2"></td>
                                            <td style="width: 40px"></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" ValidationGroup="buscar" />
                                            </td>
                                            <td style="width: 40px"></td>
                                            <td style="text-align: right">
                                                <asp:Button ID="btnGenerar" runat="server" Text="Generar" OnClick="btnGenerar_Click" ValidationGroup="generar" /></td>
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
                                <td>
                                    <%--<asp:Button ID="btnCancelar" runat="server" CausesValidation="False" OnClick="btnCancelar_Click" Text="Regresar" />
                        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" OnClick="btnCancel_Click" Text="Cancelar" Visible="False" />--%>
                                </td>
                            </tr>
                        </table>
                        <%--Modal para busqueda de contribuyente--%>
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
                                        <tr>
                                            <td>
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

                                            </td>
                                        </tr>
                                    </td>
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
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="TabEdoCta" runat="server" HeaderText="Estado de Cuenta">
            <ContentTemplate>
                <ajaxToolkit:TabContainer ID="TabContainerEdoCta" runat="server" ActiveTabIndex="0">
                    <ajaxToolkit:TabPanel ID="TabRequerir" runat="server" HeaderText="Requerir">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblRRango" runat="server" Text="Rango:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRRangoA" runat="server" CssClass="textMediano"></asp:TextBox>
                                        a
                                        <asp:TextBox ID="txtRRanggoB" runat="server" CssClass="textMediano"></asp:TextBox>
                                        <%--<ajaxToolkit:NumericUpDownExtender ID="nudRangoA" runat="server"
                                        TargetControlID="txtRRangoA" Width="100" Step="1">
                                        </ajaxToolkit:NumericUpDownExtender> --%>                             
                                    </td>
                                    <td style="width: 40px"></td>
                                    <td>
                                        <asp:Label ID="lblIniciar" runat="server" Text="Inicio Folio en:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFolio" runat="server" CssClass="textMediano"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="valida" ControlToValidate="txtFolio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="requerir"></asp:RequiredFieldValidator>
                                        <%--<ajaxToolkit:NumericUpDownExtender ID="nudRangoA" runat="server"
                                        TargetControlID="txtRRangoA" Width="100" Step="1">
                                        </ajaxToolkit:NumericUpDownExtender> --%>                             
                                    </td>
                                    <td style="width: 40px"></td>
                                    <td>
                                        <asp:Button ID="btnRequerir" runat="server" Text="Requerir" OnClick="btnRequerir_Click" ValidationGroup="requerir" />
                                    </td>
                                    <td style="width: 40px"></td>
                                    <td>
                                        <asp:Label ID="lblTarjetas" runat="server" Text="No. Tarjetas Impresas:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTarjetas" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="TabAsignar" runat="server" HeaderText="Asignar">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSelec" runat="server" Text="Selec."></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSelec" runat="server"></asp:DropDownList>
                                    </td>
                                    <td style="width: 40px"></td>
                                    <td>
                                        <asp:Label ID="lblAsignar" runat="server" Text="Asignar a"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAsignar" runat="server"></asp:DropDownList>
                                    </td>
                                    <td style="width: 10px"></td>
                                    <td>
                                        <asp:Button ID="btnAsignar" runat="server" Text="Asignar" OnClick="btnAsignar_Click" /></td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                </ajaxToolkit:TabContainer>
                <div style="overflow: scroll">
                    <asp:GridView ID="grdEdoCta" runat="server" AutoGenerateColumns="False" CssClass="grd" DataKeyNames="IdPredio, Infraestructura, Limpieza, DAP, Recoleccion">
                        <Columns>
                            <asp:BoundField DataField="ClavePredial" HeaderText="Clave Catastral" />
                            <asp:BoundField DataField="Ubicacion" HeaderText="Ubicación" />
                            <asp:BoundField DataField="Contribuyente" HeaderText="Contribuyente" />
                            <asp:BoundField DataField="Periodo" HeaderText="Periodo Requerido" />
                            <asp:BoundField DataField="Fase" HeaderText="Fase" />
                            <asp:BoundField DataField="Impuesto" HeaderText="Impuesto" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="Adicionales" HeaderText="Adicionales" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="Rezagos" HeaderText="Rezagos" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="Recargos" HeaderText="Recargos" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="Diferencias" HeaderText="Diferencias" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="Rec" HeaderText="Rec. Difere." DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="Ejecucion" HeaderText="Ejecución" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="Base" HeaderText="Base Gravable" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="Multas" HeaderText="Multas" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="SM" HeaderText="SM" />
                            <asp:BoundField DataField="ImpuestoSM" HeaderText="Impuesto" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="AdicionalesSM" HeaderText="Adicionales" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="RezagosSM" HeaderText="Rezagos" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="RecargosSM" HeaderText="Recargos" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="EjecuciónSM" HeaderText="Ejecución" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="MultasSM" HeaderText="Multas" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="Agente" HeaderText="Agente" />
                            <asp:BoundField DataField="Folio" HeaderText="Folio Req." />
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                            <asp:TemplateField ControlStyle-Width="0px">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdfIdIP" runat="server" />
                                    <asp:HiddenField ID="hdfIdSM" runat="server" />                                                                     
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="grdFooter" />
                        <HeaderStyle CssClass="grdHead" />
                        <RowStyle CssClass="grdRowPar" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
    <uc1:ModalPopupMensaje runat="server" ID="vtnModal" DysplayAceptar="True" DysplayCancelar="True" />
</asp:Content>
>>>>>>> 776e7d9a0a962f8626f41905875e95cd0c8d8209:Catastro/Catastro/Requerimientos/Requerimiento.aspx
