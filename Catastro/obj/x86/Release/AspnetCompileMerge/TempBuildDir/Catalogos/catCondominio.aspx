﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="catCondominio.aspx.cs" Inherits="Transito.Catalogos.catCondominio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 30px;"></td>
                </tr>
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="Label1" runat="server" Text="Fraccionamientos y Condominios" CssClass="letraTitulo"></asp:Label></td>
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
                                    <asp:DropDownList ID="ddlFiltro" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" />
                                    &nbsp;<asp:TextBox ID="txtFiltro" runat="server" CssClass="textGrande"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="chkInactivo" runat="server" Text="Activos" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="False"
                                        ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" />
                                    <asp:Button ID="btnnuevaCondominio" runat="server" OnClick="btnNuevo_Click"
                                        Text="Nueva Condominio" CausesValidation="False" />
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
                        <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False"
                            CssClass="grd" DataKeyNames="id,activo" AllowPaging="True"
                            AllowSorting="True" BorderStyle="None" ShowFooter="True"
                            OnRowCommand="grd_RowCommand" OnSorting="grd_Sorting" OnPageIndexChanging="grd_PageIndexChanging" OnRowDataBound="grd_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="Descripcion" HeaderText="Condominio" SortExpression="Descripcion" />
                                <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="135px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgConsulta" runat="server" ToolTip="Consultar!"
                                            ImageUrl="~/img/consultar.fw.png"
                                            CssClass="imgButtonGrid"
                                            CommandName="ConsultarRegistro"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                                        <asp:ImageButton ID="imgUpdate" runat="server" ToolTip="Modificar!"
                                            ImageUrl="~/img/modificar.png"
                                            CssClass="imgButtonGrid"
                                            CommandName="ModificarRegistro"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                                        <asp:ImageButton ID="imgDelete" runat="server" ToolTip="Eliminar!"
                                            ImageUrl="~/img/eliminar.png"
                                            CssClass="imgButtonGrid"
                                            CommandName="EliminarRegistro"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                                        <asp:ImageButton ID="imgActivar" runat="server" ToolTip="Activar!"
                                            ImageUrl="~/img/Activar.png"
                                            CssClass="imgButtonGrid"
                                            CommandName="ActivarRegistro"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="135px" />
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
            </table>

            <asp:Panel ID="pnl" runat="server" class="formPanel">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_titulo" runat="server" CssClass="textModalTitulo2" Text="Alta o Edición de Condominios"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblNombre" runat="server" Text="Nombre:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtNombreCondominio" runat="server" CssClass="textGrande" MaxLength="50" placeholder="Nombre de la Condominio."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" ControlToValidate="txtNombreCondominio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                    </tr>  
                    <tr>
                        <td>
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="guardar" OnClick="btnGuardar_Click" />
                            <asp:Button ID="btnCancelar" runat="server" CausesValidation="False" Text="Cancelar" OnClick="btnCancelar_Click" />
                        </td>
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
