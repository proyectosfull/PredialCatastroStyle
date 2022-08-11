<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="configuracionMesa.aspx.cs" Inherits="Catastro.Catalogos.configuracionMesa" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="Label1" runat="server" Text="Configuración Mesa" CssClass="letraTitulo"></asp:Label></td>
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
                                    <asp:Button ID="btnNuevo" runat="server" OnClick="btnNuevo_Click"
                                        Text="Nueva Configuración Mesa" CausesValidation="False" />
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
                                <asp:BoundField DataField="IdMesa" HeaderText="Mesa" SortExpression="IdMesa" />
                                <asp:BoundField DataField="IdCajero" HeaderText="Empleado" SortExpression="IdUsuario" />
                                <asp:BoundField DataField="IdCaja" HeaderText="Caja" SortExpression="IdCaja" />
                                <asp:BoundField DataField="Turno" HeaderText="Turno" SortExpression="Turno" />
                                <asp:BoundField DataField="Lugar" HeaderText="Lugar" SortExpression="Lugar" />
                                <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="135px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgConsulta" runat="server" ToolTip="Consultar!"
                                            ImageUrl="~/img/consultar.fw.png"
                                            CssClass="imgButtonGrid"
                                            CommandName="ConsultarRegistro"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                                        <%--<asp:ImageButton ID="imgUpdate" runat="server" ToolTip="Modificar!"
                                            ImageUrl="~/img/modificar.png"
                                            CssClass="imgButtonGrid"
                                            CommandName="ModificarRegistro"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />--%>
                                        <asp:ImageButton ID="imgDelete" runat="server" ToolTip="Cerrar Caja!"
                                            ImageUrl="~/img/cerrarCaja.png"
                                            CssClass="imgButtonGrid"
                                            CommandName="EliminarRegistro"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                                        <%--<asp:ImageButton ID="imgActivar" runat="server" ToolTip="Activar!"
                                            ImageUrl="~/img/Activar.png"
                                            CssClass="imgButtonGrid"
                                            CommandName="ActivarRegistro"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />--%>
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
            </table>

            <asp:Panel ID="pnl" runat="server" class="formPanel">
                <table>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblTitulo" runat="server" CssClass="textModalTitulo2" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblAgente" runat="server" Text="Cajero:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCajero" runat="server" CssClass="texMediano" ></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlCajero" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar" InitialValue="%" CssClass="valida" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlCajero" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="buscar" InitialValue="%" CssClass="valida" />
                        </td>
                        <td>
                            &nbsp;</td>                        
                    </tr>
                    <%--<tr>
                        <td>
                            <asp:Label ID="lblNombreAgente" runat="server"></asp:Label>
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                            <asp:Label ID="lblMesa" runat="server" Text="Mesa:" CssClass="letraMediana" ></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMesa" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlMesa" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar" InitialValue="%" CssClass="valida" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlMesa" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="buscar" InitialValue="%" CssClass="valida" />
                        </td>
                        <td>
                            <asp:ImageButton ID="idBuscarAgente0" runat="server" ImageUrl="~/img/consultar.fw.png" OnClick="idBuscarAgente_Click" ValidationGroup="buscar" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblNoCaja" runat="server" Text="No. Caja:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlNumeroCaja" runat="server" placeholder="Seleccione el número de caja" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlNumeroCaja" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar" InitialValue="%" CssClass="valida" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblTurno" runat="server" Text="Turno:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTurno" runat="server" CssClass="textGrande" placeholder="Turno." MaxLength="50"></asp:TextBox>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblLugar" runat="server" Text="Lugar:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLugar" runat="server" CssClass="textGrande" placeholder="Lugar." MaxLength="100"></asp:TextBox>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="guardar" OnClick="btnGuardar_Click" />                            
                        </td>
                        <td>
                            <asp:Button ID="btnCancelar" runat="server" CausesValidation="False" Text="Cancelar" OnClick="btnCancelar_Click" />
                        </td>
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
