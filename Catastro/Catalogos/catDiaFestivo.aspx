
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="catDiaFestivo.aspx.cs" Inherits="Catastro.Catalogos.catDiaFestivo" %>

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
                        <asp:Label ID="Label1" runat="server" Text="Días Festivos" CssClass="letraTitulo"></asp:Label>

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
                                    &nbsp;<asp:TextBox ID="txtFiltro" runat="server" CssClass="textGrande" ></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="ChkActivos" runat="server" Text="Activos" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False"
                                        ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" />
                                    <asp:Button ID="btnNuevo" runat="server" OnClick="btnNuevo_Click"
                                        Text="Nuevo Día" CausesValidation="False" />
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
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="nombre" />
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="fecha" DataFormatString="{0:d}"  />
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
                            <asp:Label ID="lbl_titulo" runat="server" CssClass="textModalTitulo2" Text="Alta o Edición de Dias Festivos"></asp:Label>
                        </td>
                    </tr>
                      <tr>
                          <td style="width: 105px; height: 41px;">
                                <asp:Label ID="lblNombre" runat="server" CssClass="textExtraGrande" Text="Nombre:"></asp:Label>
                          </td>
                          <td style="width: 286px; height: 41px;">
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="textExtraGrande" MaxLength="200" placeholder="Nombre" Width="257px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" ControlToValidate="txtNombre" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                      
                      </tr>
                      <tr>
                          <td style="width: 105px; height: 41px;">
                                <asp:Label ID="lblFecha" runat="server" CssClass="textExtraGrande" Text="Fecha:"></asp:Label>
                          </td>
                      <td style="width: 286px; height: 41px;">
                          <asp:TextBox ID="txtFecha" runat="server" CssClass="textExtraGrande" MaxLength="50" placeholder="Fecha" Width="257px"></asp:TextBox>
                          <ajaxToolkit:MaskedEditExtender ID="txtFecha_MaskedEditExtender1" runat="server" BehaviorID="txtFecha_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFecha" />
                          <ajaxToolkit:CalendarExtender ID="txtFecha_CalendarExtender1" runat="server" BehaviorID="txtFecha_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFecha" Format="dd/MM/yyyy" />
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="txtFecha" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                      </td>
                   </tr>
                  
                   <tr>
                        <td style="width: 105px">
                            <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" ValidationGroup="guardar" OnClick="btnGuardar_Click" />
                            
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

            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="False" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
