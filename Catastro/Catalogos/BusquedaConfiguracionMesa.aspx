<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BusquedaConfiguracionMesa.aspx.cs" Inherits="Catastro.Configuracion.BusquedaConfiguracionMesa" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <br />
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="width: 716px">
                <asp:Label ID="lblTitulo" runat="server" Text="Configuración de Mesa" CssClass="letraTitulo"></asp:Label>
            </td>
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
                        <td style="width: 3px">
                            <asp:Label ID="lblMesa" runat="server" Text="Mesa:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMesa" runat="server"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button  ID="imbNuevo" runat="server"  OnClick="imbNuevo_Click" Text="Nueva Configuración" />
                        </td>
                        <td>
                            <asp:ImageButton ID="imbBuscar" runat="server" ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" />
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
                <asp:GridView ID="grdConfiguracion" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" BorderStyle="None" CssClass="grd" DataKeyNames="id" OnRowCommand="grdConfiguracion_RowCommand" OnSorting="grdConfiguracion_Sorting" OnPageIndexChanging="grdConfiguracion_PageIndexChanging" ShowFooter="True">
                    <Columns>
                        <asp:BoundField DataField="Mesa" HeaderText="Mesa" SortExpression="Mesa" />
                        <asp:BoundField DataField="Usuario" HeaderText="Usuario" SortExpression="Usuario" />
                        <asp:BoundField DataField="Caja" HeaderText="No. Caja" SortExpression="Caja" />
                        <asp:BoundField DataField="Lugar" HeaderText="Lugar" SortExpression="Lugar" />
                        <asp:BoundField DataField="Turno" HeaderText="Turno" SortExpression="Turno" />
                        <asp:BoundField DataField="Maquina" HeaderText="Maquina" SortExpression="Maquina" />
                        <asp:TemplateField HeaderText="Herramientas" ItemStyle-CssClass="" ItemStyle-Width="180px">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgUpdate" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' CommandName="ModificarRegistro" CssClass="imgButtonGrid" ImageUrl="~/img/modificar.png" ToolTip="Modificar!" />
                                <asp:ImageButton ID="imgDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' CommandName="EliminarRegistro" CssClass="imgButtonGrid" ImageUrl="~/img/eliminar.png" OnClientClick="return Alert_Confirmar(this.id,
                                            'Está seguro que quiere eliminar la configuración', 'Eliminación cancelada.');"
                                    ToolTip="Eliminar!" />
                            </ItemTemplate>
                            <ItemStyle Width="180px" />
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
    <uc1:ModalPopupMensaje runat="server" ID="vtnModal" />
</asp:Content>
