<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="catTipoFase.aspx.cs" Inherits="Catastro.Catalogos.CatTipoFase" %>

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
                        <asp:Label ID="Label1" runat="server" Text="Tipos de Fase" CssClass="letraTitulo"></asp:Label></td>
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
                                    <asp:Button ID="btnNuevaCaja" runat="server" OnClick="btnNuevo_Click"
                                        Text="Nueva Fase" CausesValidation="False" />
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
                                <asp:BoundField DataField="Fase" HeaderText="Fase" SortExpression="Fase" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
                                <asp:BoundField DataField="FaseSiguiente" HeaderText="Fase Siguiente" SortExpression="Fase Siguiente" />
                                <asp:BoundField DataField="PorcentajeEjecucion" HeaderText="% Ejecución" SortExpression="PorcentajeEjecucion" />
                                <asp:BoundField DataField="SalarioMinimoMultas" HeaderText="UMA Mutas" SortExpression="SalarioMinimoMultas" />
                                <asp:BoundField DataField="DoctoImprimir" HeaderText="Documento" SortExpression="DoctoImprimir" />
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
                        <td colspan="5">
                            <asp:Label ID="lbl_titulo" runat="server" CssClass="textModalTitulo2" Text="Alta o Edición de la Fase"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblFase" runat="server" Text="Fase:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtFase"  runat="server" CssClass="textGrande" MaxLength="10" placeholder="Fase."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" ControlToValidate="txtFase" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Label ID="lblFaseSig" runat="server" Text="Fase Siguiente:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtFasesig" runat="server" CssClass="textGrande" MaxLength="10" placeholder="Fase Siguiente."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="valida" ControlToValidate="txtFasesig" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                    </tr> 
                    <tr>
                        <td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Ingresar sólo números enteros." ValidationExpression="^\d+$" ControlToValidate="txtFase" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                        <td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Ingresar sólo números enteros." ValidationExpression="^\d+$" ControlToValidate="txtFasesig" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                    </tr> 
                    <tr>
                        <td colspan="5">
                            <asp:Label ID="lblDescripcion" runat="server" Text="Descripción:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtDescripcion" TextMode="MultiLine" runat="server" CssClass="textMultiExtraGrande" MaxLength="60" placeholder="Descripción."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="txtDescripcion" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="txtDescripcion" CssClass="valida" ErrorMessage="Máximo 60 caracteres" ClientValidationFunction="validateLength" ValidationGroup="guardar"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblPorcentaje" runat="server" Text="Porcentaje Ejecución:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtPorcentaje" runat="server" CssClass="textGrande" MaxLength="10" placeholder="Porcentaje Ejecución."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="valida" ControlToValidate="txtPorcentaje" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Label ID="lblSmmultas" runat="server" Text="UMA Multa:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtSmmultas" runat="server" CssClass="textGrande" MaxLength="10" placeholder="SM Multas"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" CssClass="valida" ControlToValidate="txtSmmultas" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                    </tr> 
                    <tr>
                        <td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Maximo 2 decimales." ValidationExpression="\d+(\.\d{2})?|\.\d{2}" ControlToValidate="txtPorcentaje" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                        <td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="Ingresar sólo números enteros." ValidationExpression="^\d+$" ControlToValidate="txtSmmultas" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Label ID="lblDocto" runat="server" Text="Documento a Imprimir:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtDcto" runat="server" CssClass="textGrande" MaxLength="30" placeholder="Documento A Imprimir." Width="503px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" CssClass="valida" ControlToValidate="txtDcto" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Label ID="Label2" runat="server" Text="Cobranza:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:DropDownList id ="ddlHon" runat ="server">
                                <asp:ListItem value ="1"> Gastos de notificación </asp:listitem>
                                <asp:ListItem value ="2"> Honorarios de Notificación </asp:listitem>
                            </asp:DropDownList>
                            <%--<asp:TextBox ID="txtCobranza" runat="server" CssClass="textGrande" MaxLength="30" placeholder="Cobranza." Width="503px"></asp:TextBox>--%>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" CssClass="valida" ControlToValidate="txtCobranza" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;&nbsp;
                        </td>
                    </tr>  
                    <tr>
                        <td>
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="guardar" OnClick="btnGuardar_Click" />
                            &nbsp;&nbsp;
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
     <script type="text/javascript">
        function validateLength(oSrc, args) {
            args.IsValid = (args.Value.length <= 60);
        }
    </script>
</asp:Content>


