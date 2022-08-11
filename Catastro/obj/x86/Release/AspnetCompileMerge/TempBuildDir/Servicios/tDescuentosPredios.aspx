    <%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="tDescuentosPredios.aspx.cs" Inherits="Catastro.Catalogos.DescuentoPredio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="formCaptura">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label1" runat="server" Text="Descuento por Concepto" CssClass="letraTitulo"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 150px">
                            &nbsp;&nbsp;
                            <asp:DropDownList ID="ddlTramite" runat="server"></asp:DropDownList>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlTramite" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar" InitialValue="%" CssClass="valida"></asp:RequiredFieldValidator>--%>
                            <%--<asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" Text="Guardar" ValidationGroup="guardar" Visible="False" />--%>
                        </td>
                        <%--<tr>--%>
                                <td style="width: 186px">&nbsp;&nbsp;
                                    <asp:Label ID="lblCve" runat="server" CssClass="letraSubTitulo" Font-Size="Medium" Text="Clave del predio:"></asp:Label>
                                </td>
                                <td style="width: 316px">
                                    <asp:TextBox ID="txtCve" runat="server" CssClass="textChico" MaxLength="50" placeholder="clave del predio" Width="217px"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender runat="server" ID="meeFiltro" Mask="9999-99-999-999" TargetControlID="txtCve" MaskType="Number" InputDirection="RightToLeft" />
                                    <%--<ajaxToolkit:MaskedEditExtender ID="txtFolio_MaskedEditExtender" runat="server" BehaviorID="txtFolio_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" InputDirection="RightToLeft" Mask="9999999999" MaskType="Number" TargetControlID="txtCve" />--%>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtCve" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" ValidationGroup="buscar" />
                                </td>
                           <%-- </tr>--%>
                        
                    </tr>
                    
                    <tr>
                        <td colspan="6" style="height: 10px;">
                            <hr />
                        </td>

                    </tr>
                    <tr>
                        <td style="width: 150px">
                            <asp:Label ID="lbl2" runat="server" CssClass="letraSubTitulo" Text="Clave del Predio:"></asp:Label>
                        </td>
                        <td style="width: 186px">
                            <asp:Label ID="lblPred" runat="server" CssClass="letraSubTitulo"></asp:Label>
                        </td>
                        
                        
                    </tr>
                    <tr>
                        <td style="width: 150px">
                            <asp:Label ID="lbl3" runat="server" CssClass="letraSubTitulo" Text="Propietario:"></asp:Label>
                        </td>
                        <td colspan="2" style="width: 186px">
                            <asp:Label ID="lblPropietario" runat="server" CssClass="letraSubTitulo"></asp:Label>
                        </td>

                       
                    </tr>

                    
                    <tr>
                        <td colspan="6">
                             <asp:GridView ID="grdClaveGenerica" runat="server" DataKeyNames="Id" CssClass="grd" AutoGenerateColumns="False"
                               OnRowCommand="grdClaveGenerica_RowCommand"  BorderStyle="None" Visible="False">
                                <Columns>
                                    <asp:BoundField DataField="NombreAdquiriente" HeaderText="Nombre Contribuyente"/>
                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripcion"/>
                                    <asp:TemplateField HeaderText="Herramientas" ItemStyle-CssClass="" ItemStyle-Width="135px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgConsulta" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' CommandName="ConsultarRegistro" CssClass="imgButtonGrid" ImageUrl="~/img/consultar.fw.png" ToolTip="Consultar" />
                                        </ItemTemplate>
                                        <ItemStyle Width="135px" />
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle CssClass="grdFooter" />
                                <HeaderStyle CssClass="grdHead" />
                                <RowStyle CssClass="grdRowPar" />
                            </asp:GridView>
                            <br />
                            <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False"
                                CssClass="grd" DataKeyNames="Id,estado,Porcentaje,FechaFin" AllowPaging="True"
                            AllowSorting="True" BorderStyle="None" ShowFooter="True"
                            OnRowCommand="grd_RowCommand" OnSorting="grd_Sorting" OnPageIndexChanging="grd_PageIndexChanging" OnRowDataBound="grd_RowDataBound" >
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="IdConcepto" SortExpression="IdConcepto" Visible="false" />
                                    <asp:BoundField DataField="Cri" HeaderText="Cri" SortExpression="cri" />
                                    <asp:BoundField DataField="Nombre" HeaderText="Concepto" SortExpression="Nombre" />
                                    <asp:BoundField DataField="Porcentaje" HeaderText="Porcentaje" SortExpression="Porcentaje" />
                                    <asp:BoundField DataField="FechaFin" HeaderText="Vencimiento" SortExpression="Vencimiento" DataFormatString="{0:dd-MM-yyyy}" />
                                    <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="135px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgNuevo" runat="server" ToolTip="Nuevo!"
                                                ImageUrl="~/img/modificar.png"
                                                CssClass="imgButtonGrid"
                                                CommandName="NuevoRegistro"
                                                CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' />
                                        <asp:ImageButton ID="imgEditar" runat="server" ToolTip="Actualizar!"
                                                ImageUrl="~/img/modificar.png"
                                                CssClass="imgButtonGrid"
                                                CommandName="ActualizarRegistro"
                                                CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' />
                                         <%--<asp:ImageButton ID="imgConsulta" runat="server" ToolTip="Consultar!"
                                            ImageUrl="~/img/consultar.fw.png"
                                            CssClass="imgButtonGrid"
                                            CommandName="ConsultarRegistro"
                                            CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' />--%>
                                        <asp:ImageButton ID="imgActivar" runat="server" ToolTip="Activar!"
                                            ImageUrl="~/img/Activar.png"
                                            CssClass="imgButtonGrid"
                                            CommandName="ActivarRegistro"
                                            CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' />
                                            <%--CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />--%>
                                        <asp:ImageButton ID="imgDelete" runat="server" ToolTip="Eliminar!"
                                            ImageUrl="~/img/eliminar.png"
                                            CssClass="imgButtonGrid"
                                            CommandName="EliminarRegistro"
                                            CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' />
                                            <%--CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />--%>
                                        
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
                            <asp:GridView ID="grdISABI" runat="server" AutoGenerateColumns="False"
                                CssClass="grd" DataKeyNames="Id" AllowPaging="True"
                            AllowSorting="True" BorderStyle="None" ShowFooter="True"
                            OnRowCommand="grd_RowCommandISABI" OnSorting="grd_SortingISABI" OnPageIndexChanging="grd_PageIndexChangingISABI" OnRowDataBound="grd_RowDataBoundISABI" >
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="IdConcepto" SortExpression="Id" Visible="false" />
                                    <asp:BoundField DataField="Adquiriente" HeaderText="Adquiriente" SortExpression="Adquiriente" />
                                    <asp:BoundField DataField="Cri" HeaderText="Cri" SortExpression="Cri" />
                                    <asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
                                    <asp:BoundField DataField="Porcentaje" HeaderText="Porcentaje" SortExpression="Porcentaje" />
                                    <asp:BoundField DataField="FechaFin" HeaderText="Vencimiento" SortExpression="FechaFin" DataFormatString="{0:dd-MM-yyyy}" />
                                    <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="135px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgNuevo" runat="server" ToolTip="Nuevo!"
                                                ImageUrl="~/img/modificar.png"
                                                CssClass="imgButtonGrid"
                                                CommandName="NuevoRegistro"
                                                CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' />
                                        <asp:ImageButton ID="imgEditar" runat="server" ToolTip="Actualizar!"
                                                ImageUrl="~/img/modificar.png"
                                                CssClass="imgButtonGrid"
                                                CommandName="ActualizarRegistro"
                                                CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' />
                                         <%--<asp:ImageButton ID="imgConsulta" runat="server" ToolTip="Consultar!"
                                            ImageUrl="~/img/consultar.fw.png"
                                            CssClass="imgButtonGrid"
                                            CommandName="ConsultarRegistro"
                                            CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' />--%>
                                        <asp:ImageButton ID="imgActivar" runat="server" ToolTip="Activar!"
                                            ImageUrl="~/img/Activar.png"
                                            CssClass="imgButtonGrid"
                                            CommandName="ActivarRegistro"
                                            CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' />
                                            <%--CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />--%>
                                        <asp:ImageButton ID="imgDelete" runat="server" ToolTip="Eliminar!"
                                            ImageUrl="~/img/eliminar.png"
                                            CssClass="imgButtonGrid"
                                            CommandName="EliminarRegistro"
                                            CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' />
                                            <%--CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />--%>
                                        
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
                            <asp:Label ID="lbl_titulo" runat="server" CssClass="textModalTitulo2" Text="Alta o Edición del Descuento"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="width: 105px; height: 41px;">
                            <asp:Label ID="lblConcepto" runat="server" CssClass="textGrande" Text="Concepto:" ></asp:Label>
                        </td>
                       
                        
                    </tr>
                    <tr>
                        <td style="width: 105px; height: 41px;">
                            <asp:Label ID="lblPorcentaje" runat="server" CssClass="textGrande" Text="Porcentaje:"></asp:Label>
                        </td>
                        <td style="width: 307px; height: 41px;">
                            
                            <asp:TextBox ID="txtPorcentaje" runat="server" CssClass="textExtraGrande" MaxLength="100" placeholder="Porcentaje %" Width="71px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" ControlToValidate="txtPorcentaje" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Ingresar solo numeros" ValidationExpression="\d*\.?\d*" ControlToValidate="txtPorcentaje" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 105px; height: 41px;">
                            <asp:Label ID="lblFechaFin" runat="server" CssClass="textGrande" Text="Vigencia:"></asp:Label>
                        </td>
                        <td style="width: 307px; height: 41px;">
                            <asp:TextBox ID="txtFechaFin" DataFiel="FechaFin" runat="server" CssClass="textExtraGrande" MaxLength="50" placeholder="Fecha Vigencia" Width="103px"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="txtFechaFin_MaskedEditExtender" runat="server" BehaviorID="txtFechaFin_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaFin" />
                            <ajaxToolkit:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" BehaviorID="txtFechaFin_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFechaFin" Format="dd/MM/yyyy" />
                        </td>
                    </tr>
                    
                   <tr>
                        <td style="width: 105px">
                            <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" ValidationGroup="guardar" OnClick="btnGuardar_Click" />
                        </td>
                        <td style="width: 307px">
                            <asp:Button ID="btnCancelar" runat="server" CausesValidation="False" Text="Cancelar" OnClick="btnCancelar_Click" />
                            <asp:HiddenField runat="server" id="claveid" Value="" />
                        </td>

                    </tr>

                </table>

            </asp:Panel>
                </div>
            <ajaxToolkit:ModalPopupExtender ID="pnl_Modal" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnl" TargetControlID="btnPnl">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnPnl" runat="server" />

















            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="False" />
        </ContentTemplate>
    </asp:UpdatePanel>
     <script type="text/javascript">
        function validateLength(oSrc, args) {
            args.IsValid = (args.Value.length <= 500);
        }
    </script>
</asp:Content>
