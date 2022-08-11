<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="catTarifaRecargo.aspx.cs" Inherits="Catastro.Catalogos.catTarifaRecargo" %>

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
                        <asp:Label ID="Label1" runat="server" Text="Tarifa Recargo" CssClass="letraTitulo"></asp:Label></td>
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
                                        Text="Nueva Tarifa Recargo" CausesValidation="False" />
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
                                <asp:BoundField DataField="Ejercicio" HeaderText="Ejercicio" SortExpression="Ejercicio" />
                                <asp:BoundField DataField="Bimestre" HeaderText="Mes" SortExpression="Bimestre" />
                                <asp:BoundField DataField="Porcentaje" HeaderText="Porcentaje" SortExpression="Porcentaje" />                                
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
                <table style="width: 604px">
                    <tr>
                        <td style="width: 602px; height: 37px">
                            <asp:Label ID="lbl_titulo" runat="server" CssClass="textModalTitulo2" Text="Alta o Edición de Tarifa Recargo"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 602px">
                            <asp:Label ID="lblNombre" runat="server" Text="Ejercicio:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtEjercicio" runat="server" CssClass="textGrande" MaxLength="4" placeholder="Ejercicio."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" ControlToValidate="txtEjercicio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^\d+$" ControlToValidate="txtEjercicio" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                      <tr>
                          <td style="width: 602px">
                              <asp:Label ID="lblNombre0" runat="server" CssClass="letraMediana" Text="Mes:"></asp:Label>
                              <br />
                              <%--<asp:TextBox ID="txtBimestre" runat="server" CssClass="textGrande" MaxLength="2" placeholder="Bimestre."></asp:TextBox>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtBimestre" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                              <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtBimestre" 
                                  CssClass="valida" ErrorMessage="Ingresar solo números del 1 al 12" Font-Size="Small" SetFocusOnError="True" 
                                  ValidationExpression="^[0-9]+$" ValidationGroup="guardar"></asp:RegularExpressionValidator>--%>
                              <asp:DropDownList ID="ddlMes" runat="server">
                                <asp:ListItem Text="1 - Enero" Value="1" Selected="True" ></asp:ListItem>
                                <asp:ListItem Text="2 - Febrero" Value="2"></asp:ListItem>
                                <asp:ListItem Text="3 - Marzo" Value="3"></asp:ListItem>
                                <asp:ListItem Text="4 - Abril" Value="4"></asp:ListItem>
                                <asp:ListItem Text="5 - Mayo" Value="5"></asp:ListItem>
                                <asp:ListItem Text="6 - Junio" Value="6"></asp:ListItem>
                                <asp:ListItem Text="7 - Julio" Value="7"></asp:ListItem>
                                <asp:ListItem Text="8 - Agosto" Value="8"></asp:ListItem>
                                <asp:ListItem Text="9 - Septiembre" Value="9"></asp:ListItem>
                                <asp:ListItem Text="10 - Octubre" Value="10"></asp:ListItem>
                                <asp:ListItem Text="11 - Noviembre" Value="11"></asp:ListItem>
                                <asp:ListItem Text="12 - Diciembre" Value="12"></asp:ListItem>
                            </asp:DropDownList>
                          </td>
                    </tr>
                      <tr>
                        <td style="width: 602px">
                            <asp:Label ID="Label2" runat="server" Text="Porcentaje:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtPorcentaje" runat="server" CssClass="textGrande" MaxLength="7" placeholder="Porcentaje."></asp:TextBox>
                            <asp:RangeValidator id="RangeValidator1" runat="server" ErrorMessage="Ingresar solo números mayor a 0 y menor o igual a 100" ControlToValidate="txtPorcentaje" MinimumValue="0.0001" MaximumValue="100" Display="Dynamic" Type="Double"></asp:RangeValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="valida" ControlToValidate="txtPorcentaje" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Maximo 4 decimales." ValidationExpression="\d+(\.\d{4})?|\.\d{4}" ControlToValidate="txtPorcentaje" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                    </tr>                   
                    <tr>
                        <td style="width: 602px">
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
