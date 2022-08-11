<%@ Page Title="Indice Precio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="catIndicePrecio.aspx.cs" Inherits="Transito.Catalogos.catIndicePrecio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 30px;">
                     </td>
                </tr>
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="Label1" runat="server" Text="Indice Precio" CssClass="letraTitulo"></asp:Label></td>
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
                                    <asp:Button ID="btnEjercicio" runat="server" OnClick="btnNuevo_Click"
                                        Text="Nuevo Ejercicio" CausesValidation="False" />
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
                            CssClass="grd" DataKeyNames="activo" AllowPaging="True"
                            AllowSorting="True" BorderStyle="None" ShowFooter="True"
                            OnRowCommand="grd_RowCommand" OnSorting="grd_Sorting" OnPageIndexChanging="grd_PageIndexChanging" OnRowDataBound="grd_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="Ejercicio" HeaderText="Ejercicio" SortExpression="Ejercicio" />
                                <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="135px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgConsulta" runat="server" ToolTip="Consultar!"
                                            ImageUrl="~/img/consultar.fw.png"
                                            CssClass="imgButtonGrid"
                                            CommandName="ConsultarRegistro"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Ejercicio")%>' />
                                        <asp:ImageButton ID="imgUpdate" runat="server" ToolTip="Modificar!"
                                            ImageUrl="~/img/modificar.png"
                                            CssClass="imgButtonGrid"
                                            CommandName="ModificarRegistro"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Ejercicio")%>' />
                                        <asp:ImageButton ID="imgDelete" runat="server" ToolTip="Eliminar!"
                                            ImageUrl="~/img/eliminar.png"
                                            CssClass="imgButtonGrid"
                                            CommandName="EliminarRegistro"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Ejercicio")%>' />
                                        <asp:ImageButton ID="imgActivar" runat="server" ToolTip="Activar!"
                                            ImageUrl="~/img/Activar.png"
                                            CssClass="imgButtonGrid"
                                            CommandName="ActivarRegistro"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Ejercicio")%>' />
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

            <asp:Panel ID="pnl" runat="server" class="formPanel" ScrollBars="Auto">
                <table>
                    <tr>
                        <td colspan="4" style="text-align:center;">
                            <asp:Label ID="lbl_titulo" runat="server" CssClass="textModalTitulo2" Text="Alta o Edición de Ejercicio"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <%--<td colspan="4" style="text-align:center;">
                            <asp:Label ID="lblEjercicio" runat="server" Text="Ejercicio:" CssClass="letraMediana"></asp:Label>
                            &nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtEjercicio" runat="server" CssClass="textGrande" TextMode="Number" MaxLength="4" placeholder="Ejercicio"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" ControlToValidate="txtEjercicio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>--%>
                        <td colspan="4" style="text-align:center;">
                            <asp:Label ID="lblEjercicio" runat="server" Text="Ejercicio:" CssClass="letraMediana"></asp:Label>
                            &nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtEjercicio" runat="server" CssClass="textGrande" MaxLength="4" placeholder="Ejercicio."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" ControlToValidate="txtEjercicio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator><br />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^\d+$" ControlToValidate="txtEjercicio" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator><br />
                            <asp:RangeValidator ID="validadorEjercicio" runat="server" ErrorMessage="Ingresar Ejercicio menor a 2017" ControlToValidate="txtEjercicio" MinimumValue="1000" MaximumValue="2017" Display="Dynamic" ValidationGroup="guardar" Type="Integer"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr > 
                        <td style="vertical-align:text-top">
                            <asp:Label ID="lblEnero" runat="server" Text="Enero:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEnero" runat="server" CssClass="textGrande" MaxLength="10" placeholder="Importe." ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ReqEnero" runat="server" CssClass="valida" ControlToValidate="txtEnero" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegEnero" runat="server" ErrorMessage="Maximo 4 decimales." ValidationExpression="\d{1,5}(.\d{1,4})?" ControlToValidate="txtEnero" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                        <td  style="vertical-align:text-top">
                            <asp:Label ID="lblFebrero" runat="server" Text="Febrero:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFebrero" runat="server" CssClass="textGrande" MaxLength="10" placeholder="Importe." ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ReqFebrero" runat="server" CssClass="valida" ControlToValidate="txtFebrero" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegFebrero" runat="server" ErrorMessage="Maximo 4 decimales." ValidationExpression="\d{1,5}(.\d{1,4})?" ControlToValidate="txtFebrero" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td  style="vertical-align:text-top">
                            <asp:Label ID="lblMarzo" runat="server" Text="Marzo:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMarzo" runat="server" CssClass="textGrande" MaxLength="10" placeholder="Importe." ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ReqMarzo" runat="server" CssClass="valida" ControlToValidate="txtMarzo" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegMarzo" runat="server" ErrorMessage="Maximo 4 decimales." ValidationExpression="\d{1,5}(.\d{1,4})?" ControlToValidate="txtMarzo" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                        <td  style="vertical-align:text-top">
                            <asp:Label ID="lblAbril" runat="server" Text="Abril:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAbril" runat="server" CssClass="textGrande" MaxLength="10" placeholder="Importe." ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ReqAbril" runat="server" CssClass="valida" ControlToValidate="txtAbril" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegAbril" runat="server" ErrorMessage="Maximo 4 decimales." ValidationExpression="\d{1,5}(.\d{1,4})?" ControlToValidate="txtAbril" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td  style="vertical-align:text-top">
                            <asp:Label ID="lblMayo" runat="server" Text="Mayo:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMayo" runat="server" CssClass="textGrande" MaxLength="10" placeholder="Importe." ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ReqMayo" runat="server" CssClass="valida" ControlToValidate="txtMayo" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegMayo" runat="server" ErrorMessage="Maximo 4 decimales." ValidationExpression="\d{1,5}(.\d{1,4})?" ControlToValidate="txtMayo" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                        <td  style="vertical-align:text-top">
                            <asp:Label ID="lblJunio" runat="server" Text="Junio:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtJunio" runat="server" CssClass="textGrande" MaxLength="10" placeholder="Importe." ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ReqJunio" runat="server" CssClass="valida" ControlToValidate="txtJunio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegJunio" runat="server" ErrorMessage="Maximo 4 decimales." ValidationExpression="\d{1,5}(.\d{1,4})?" ControlToValidate="txtJunio" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td  style="vertical-align:text-top">
                            <asp:Label ID="lblJulio" runat="server" Text="Julio:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtJulio" runat="server" CssClass="textGrande" MaxLength="10" placeholder="Importe." ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ReqJulio" runat="server" CssClass="valida" ControlToValidate="txtJulio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegJulio" runat="server" ErrorMessage="Maximo 4 decimales." ValidationExpression="\d{1,5}(.\d{1,4})?" ControlToValidate="txtJulio" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                        <td  style="vertical-align:text-top">
                            <asp:Label ID="lblAgosto" runat="server" Text="Agosto:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAgosto" runat="server" CssClass="textGrande" MaxLength="10" placeholder="Importe." ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ReqAgosto" runat="server" CssClass="valida" ControlToValidate="txtAgosto" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegAgosto" runat="server" ErrorMessage="Maximo 4 decimales." ValidationExpression="\d{1,5}(.\d{1,4})?" ControlToValidate="txtAgosto" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td  style="vertical-align:text-top">
                            <asp:Label ID="lblSeptiembre" runat="server" Text="Septiembre:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeptiembre" runat="server" CssClass="textGrande" MaxLength="10" placeholder="Importe." ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ReqSeptiembre" runat="server" CssClass="valida" ControlToValidate="txtSeptiembre" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegSeptiembre" runat="server" ErrorMessage="Maximo 4 decimales." ValidationExpression="\d{1,5}(.\d{1,4})?" ControlToValidate="txtSeptiembre" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                        <td  style="vertical-align:text-top">
                            <asp:Label ID="lblOctubre" runat="server" Text="Octubre:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOctubre" runat="server" CssClass="textGrande" MaxLength="10" placeholder="Importe." ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ReqOctubre" runat="server" CssClass="valida" ControlToValidate="txtOctubre" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegOctubre" runat="server" ErrorMessage="Maximo 4 decimales." ValidationExpression="\d{1,5}(.\d{1,4})?" ControlToValidate="txtOctubre" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td  style="vertical-align:text-top">
                            <asp:Label ID="lblNoviembre" runat="server" Text="Noviembre:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoviembre" runat="server" CssClass="textGrande" MaxLength="10" placeholder="Importe." ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ReqNoviembre" runat="server" CssClass="valida" ControlToValidate="txtNoviembre" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegNoviembre" runat="server" ErrorMessage="Maximo 4 decimales." ValidationExpression="\d{1,5}(.\d{1,4})?" ControlToValidate="txtNoviembre" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                        <td  style="vertical-align:text-top">
                            <asp:Label ID="lblDiciembre" runat="server" Text="Diciembre:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDiciembre" runat="server" CssClass="textGrande" MaxLength="10" placeholder="Importe." ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ReqDiciembre" runat="server" CssClass="valida" ControlToValidate="txtDiciembre" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegDiciembre" runat="server" ErrorMessage="Maximo 4 decimales." ValidationExpression="\d{1,5}(.\d{1,4})?" ControlToValidate="txtDiciembre" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr style="text-align:center;">
                        <td>
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="guardar" OnClick="btnGuardar_Click" />
                        </td>
                        <td>
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
