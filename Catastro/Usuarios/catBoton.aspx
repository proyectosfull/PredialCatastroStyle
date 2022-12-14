<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="catBoton.aspx.cs" Inherits="Catastro.Catalogos.catBoton" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 30px;"></td>
                </tr>
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="Label1" runat="server" Text="Botones" CssClass="letraTitulo"></asp:Label>
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
                                <td>
                                    <asp:DropDownList ID="ddlFiltro" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" />
                                    &nbsp;<asp:TextBox ID="txtFiltro" runat="server" CssClass="textGrande"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;
                                   
                                    <asp:CheckBox ID="ChkActivos" runat="server" Text="Activos" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                   
                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False"
                                        ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" />
                                    <asp:Button ID="btnNuevo" runat="server" OnClick="btnNuevo_Click"
                                        Text="Nuevo Botón" CausesValidation="False" />
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
                                <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" />
                                <asp:BoundField DataField="Boton" HeaderText="Botón" SortExpression="Boton" />
                                <asp:CheckBoxField DataField="AccesoTotal" SortExpression="AccesoTotal" HeaderText="Acceso Total" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="descripcion" />
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
                <table cellpadding="0" cellspacing="0">

                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lbl_titulo" runat="server" CssClass="textModalTitulo2" Text="Alta o Edición Botón"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 105px; height: 41px;">
                            <asp:Label ID="Label3" runat="server" CssClass="textExtraGrande" Text="Ventana:"></asp:Label>
                        </td>
                        <td style="width: 307px; height: 41px;">
                            <asp:DropDownList ID="ddlVentana" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlVentana" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar" InitialValue="%" CssClass="valida"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 105px; height: 41px;">
                            <asp:Label ID="lblClave" runat="server" CssClass="textExtraGrande" Text="Clave:"></asp:Label>
                        </td>
                        <td style="width: 307px; height: 41px;">
                            <asp:TextBox ID="txtClave" runat="server" CssClass="textExtraGrande" MaxLength="20" placeholder="Clave" Width="273px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" ControlToValidate="txtClave" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 105px; height: 41px;">
                            <asp:Label ID="lblBoton" runat="server" CssClass="textExtraGrande" Text="Botón:"></asp:Label>
                        </td>
                        <td style="width: 307px; height: 41px;">
                            <asp:TextBox ID="txtBoton" runat="server" CssClass="textExtraGrande" MaxLength="50" placeholder="Botón" Width="273px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="valida" ControlToValidate="txtBoton" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 105px; height: 41px;">
                            <asp:Label ID="Label2" runat="server" CssClass="textExtraGrande" Text="Acceso Total:"></asp:Label>
                        </td>
                        <td style="width: 307px; height: 41px;">

                            <asp:RadioButtonList ID="rblAccesoTotal" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="false">NO</asp:ListItem>
                                <asp:ListItem Value="true">SI</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>                                         
                    <tr>
                        <td style="width: 105px; height: 41px;">
                            <asp:Label ID="lblDescripcion" runat="server" CssClass="textExtraGrande" Text="Descripción:"></asp:Label>
                        </td>
                        <td style="width: 307px; height: 41px;">
                            <asp:TextBox ID="txtDescripcion" TextMode="MultiLine" runat="server" CssClass="textMultiExtraGrande" MaxLength="200" placeholder="Descripción." Height="75px" Width="274px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="txtDescripcion" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 105px; height: 41px;">
                            
                        </td>
                        <td style="width: 307px; height: 41px;">
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="txtDescripcion" CssClass="valida" ErrorMessage="Máximo 300 caracteres" ClientValidationFunction="validateLength" ValidationGroup="guardar" Font-Size="Small"></asp:CustomValidator></td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 105px">
                            <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" ValidationGroup="guardar" OnClick="btnGuardar_Click" />

                        </td>
                        <td style="width: 307px">
                            <asp:Button ID="btnCancelar" runat="server" CausesValidation="False" Text="Cancelar" OnClick="btnCancelar_Click" />
                        </td>
                    </tr>
                </table>

            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="pnl_Modal" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnl" TargetControlID="btnPnl">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnPnl" runat="server" />

            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="False" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function validateLength(oSrc, args) {
            args.IsValid = (args.Value.length <= 300);
        }
    </script>
</asp:Content>
