<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="catTipoTramite.aspx.cs" Inherits="Catastro.Catalogos.catTipoTramite" %>

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
                        <asp:Label ID="Label1" runat="server" Text="Tipo Tramite" CssClass="letraTitulo"></asp:Label></td>
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
                                        Text="Nuevo Tipo Tramite" CausesValidation="False" />
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
                                <asp:BoundField DataField="Tramite" HeaderText="Tramite" SortExpression="Tramite" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
                                <asp:CheckBoxField DataField="ConDescuento" HeaderText="Aplica Descuento" SortExpression="Descuento" />
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" DataFormatString="{0:d}" />
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
                        <td colspan="5"></td>
                        <td >
                            <asp:Label ID="lbl_titulo" runat="server" CssClass="textModalTitulo2" Text="Alta o Edición Tipo Tramite"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 82px">
                            <asp:Label ID="lblTramite" runat="server" Text="Tramite:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td colspan="5" style="width: 317px">
                            <asp:TextBox ID="txtTramite" runat="server" CssClass="textExtraGrande" placeholder="Tramite." MaxLength="15" Width="100px" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" ControlToValidate="txtTramite" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 82px; height: 17px;">
                            <asp:Label ID="lblFecha" runat="server" Text="Fecha:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td colspan="5" style="width: 317px">
                            
                            <asp:TextBox ID="txtFecha" runat="server" CssClass="textExtraGrande" placeholder="Fecha." Width="100px"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="MeFechaIngreso_MaskedEditExtender" runat="server" BehaviorID="txtFechaIngreso_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFecha" />
                            <ajaxToolkit:CalendarExtender ID="txtFechaIngreso_CalendarExtender" runat="server" BehaviorID="txtFechaIngreso_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFecha" Format="dd/MM/yyyy" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="txtFecha" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            &nbsp;&nbsp;
                            <asp:Label ID="lblDescuento" runat="server" Text="Descuento:"></asp:Label>
                            &nbsp;&nbsp;
                            <asp:CheckBox ID="chkDescuento" runat="server" Text="Si" />
                        </td>
                        
                        <td>
                            
                        </td>
                        
                    </tr>
                    <tr>
                        <td style="width: 82px">
                            <asp:Label ID="lblDescripcion" runat="server" Text="Descripcion:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td colspan="5" style="width: 317px">                            
                            <asp:TextBox ID="txtDescripcion" TextMode="MultiLine" runat="server" CssClass="textMultiExtraGrande" MaxLength="80" placeholder="Descripción." Width="229px" Height="50px" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="valida" ControlToValidate="txtDescripcion" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                    </tr> 
                     <tr>
                        <td style="width: 82px">
                            
                        </td>
                        <td colspan="5" style="width: 317px">                            
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="txtDescripcion" CssClass="valida" ErrorMessage="Máximo 80 caracteres" ClientValidationFunction="validateLength" ValidationGroup="guardar" Font-Size="Small"></asp:CustomValidator></td>
                        </td>
                    </tr> 
                   
                    
                    <tr>
                       
                        <td style="width: 82px">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="guardar" OnClick="btnGuardar_Click" />
                        </td>
                        <td colspan="5" style="width: 317px">
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
            args.IsValid = (args.Value.length <= 80);
        }
    </script>
</asp:Content>

