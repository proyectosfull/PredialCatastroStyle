<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="catAgentesFiscales.aspx.cs" Inherits="Catastro.Catalogos.CatAgentesFiscales1" %>

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
                        <asp:Label ID="Label1" runat="server" Text="Agentes Fiscales" CssClass="letraTitulo"></asp:Label></td>
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
                                        Text="Nuevo Agente" CausesValidation="False" />
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
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                                <asp:BoundField DataField="ApellidoPaterno" HeaderText="Apellido Paterno" SortExpression="Apellido Paterno" />
                                <asp:BoundField DataField="ApellidoMaterno" HeaderText="Apellido Materno" SortExpression="Apellido Materno" />
                                <asp:BoundField DataField="Telefono" HeaderText="Teléfono" SortExpression="Teléfono" />
                                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
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
                        <td colspan="5" style="width: 287px">
                            <asp:Label ID="lbl_titulo" runat="server" CssClass="textModalTitulo2" Text="Alta o Edición del Agente"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="width: 287px">
                            <asp:Label ID="lblNombre" runat="server" Text="Nombres:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="textGrande" MaxLength="100" placeholder="Nombres." Width="240px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" ControlToValidate="txtNombre" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 287px">
                            <asp:Label ID="lblPaterno" runat="server" Text="Appellido Paterno:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtPaterno" runat="server" CssClass="textGrande" MaxLength="100" placeholder="Apellido Paterno."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="valida" ControlToValidate="txtPaterno" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Maximo 2 decimales." ValidationExpression="\d+(\.\d{2})?|\.\d{2}" ControlToValidate="txtImporte" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>--%>
                        </td>
                        <td>
                            <asp:Label ID="lblMaterno" runat="server" Text="Appellido Materno:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtMaterno" runat="server" CssClass="textGrande" MaxLength="100" placeholder="Apellido Materno."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="valida" ControlToValidate="txtMaterno" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Maximo 2 decimales." ValidationExpression="\d+(\.\d{2})?|\.\d{2}" ControlToValidate="txtImporte" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>--%>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="5" style="width: 287px">
                            <asp:Label ID="lblDireccion" runat="server" Text="Direccion:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtDireccion" runat="server" CssClass="textGrande" MaxLength="100" placeholder="Direccion."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" CssClass="valida" ControlToValidate="txtDireccion" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Maximo 2 decimales." ValidationExpression="\d+(\.\d{2})?|\.\d{2}" ControlToValidate="txtImporte" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 287px">
                            <asp:Label ID="lblCiudad" runat="server" Text="Ciudad:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtCiudad" runat="server" CssClass="textGrande" MaxLength="50" placeholder="Ciudad."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" CssClass="valida" ControlToValidate="txtCiudad" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Label ID="lblEstado" runat="server" Text="Estado:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtEstado" runat="server" CssClass="textGrande" MaxLength="50" placeholder="Estado."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" CssClass="valida" ControlToValidate="txtEstado" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                    </tr>

                    <tr>
                        <td style="width: 287px">
                            <asp:Label ID="lblSexo" runat="server" Text="Sexo:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddlSexo" runat="server">
                                <asp:ListItem Value="F">FEMENINO</asp:ListItem>
                                <asp:ListItem Value="M">MASCULINO</asp:ListItem>
                            </asp:DropDownList>
                            <%--&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" CssClass="valida" ControlToValidate="txtSexo" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                        </td>
                        <td>
                            <asp:Label ID="lblTel" runat="server" Text="Teléfono:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtTel" runat="server" CssClass="textGrande" MaxLength="15" placeholder="Teléfono."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" CssClass="valida" ControlToValidate="txtTel" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^\d+$" ControlToValidate="txtTel" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                    </tr>

                    <tr>
                        <td style="width: 287px">
                            <asp:Label ID="lblRfc" runat="server" Text="RFC:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtRfc" runat="server" CssClass="textGrande" MaxLength="15" placeholder="RFC."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" CssClass="valida" ControlToValidate="txtRfc" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Label ID="lblCurp" runat="server" Text="CURP:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtCurp" runat="server" CssClass="textGrande" MaxLength="18" placeholder="CURP."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" CssClass="valida" ControlToValidate="txtCurp" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                    </tr>

                    <tr>
                        <td style="width: 287px">
                            <asp:Label ID="lblEmail" runat="server" Text="Email:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="textGrande" MaxLength="60" placeholder="Email."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" CssClass="valida" ControlToValidate="txtEmail" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>


                        </td>
                        <td>
                            <asp:Label ID="lblFechaIngreso" runat="server" Text="Fecha de Ingreso:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtFechaIngreso" runat="server" CssClass="textExtraGrande" MaxLength="50" placeholder="Fecha Ingreso" Width="94px"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="MeFechaIngreso_MaskedEditExtender" runat="server" BehaviorID="txtFechaIngreso_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaIngreso" />
                            <ajaxToolkit:CalendarExtender ID="txtFechaIngreso_CalendarExtender" runat="server" BehaviorID="txtFechaIngreso_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFechaIngreso" Format="dd/MM/yyyy" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="txtFechaIngreso" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Favor de validar el formato del email." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                    </tr>

                    <tr>
                        <td style="width: 287px">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="guardar" OnClick="btnGuardar_Click" />

                        </td>
                        <td style="width: 286px">
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
