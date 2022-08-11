<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="catRFC.aspx.cs" Inherits="Catastro.Catalogos.catRFC" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <table style="width:100%;">
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="Label1" runat="server" Text="Catalogo de RFC" CssClass="letraTitulo"></asp:Label></td>
                </tr>
                <tr>
                    <td style="height: 2px">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="padding: 0px 10px 10px 10px;">
                        <table style="width:100%;">
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlFiltro" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" />
                                    &nbsp;&nbsp;
                                    <asp:TextBox ID="txtFiltro" runat="server" CssClass="textGrande"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="chkInactivo" runat="server" Text="Activos" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="False"
                                        ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" />
                                    <asp:Button ID="btnNuevoDescuento" runat="server" OnClick="btnNuevo_Click"
                                        Text="Nuevo RFC" CausesValidation="False" />
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
                                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                                <asp:BoundField DataField="RFC" HeaderText="RFC" SortExpression="RFC" />
                                <asp:BoundField DataField="IdPredio" HeaderText="Clave Catastral" SortExpression="RFC" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
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
            <table cellpadding="0" cellspacing="0" width="90%">
                <tr>
                    <td>
                        <asp:Label ID="lbl_titulo" runat="server" Text="Alta RFC" CssClass="letraTitulo"></asp:Label>
                        
                    </td>
                </tr>
                <tr>
                    <td style="height: 2px">
                        &nbsp; &nbsp;&nbsp;&nbsp;
                        <asp:HiddenField ID="hdfId" runat="server" />
                    </td>
                </tr>

                <tr>
                    <td >
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRFC" runat="server" Text="RFC:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRFC" runat="server" CssClass="textGrande" placeholder="NNNN-XXXX-XX-XX-NNN." MaxLength="13"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="valida" ControlToValidate="txtRFC" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                    <asp:Label ID="lblValidaRFC" runat="server" ForeColor="OrangeRed" Text=""></asp:Label>
                                </td>
                                 <td>
                                     <asp:Label ID="lblclave" runat="server" CssClass="letraMediana" Text="Clave Catastral:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtClave" runat="server" CssClass="textGrande"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Label ID="lblNombre" runat="server" CssClass="letraMediana" Text="Nombre:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNombre" runat="server" CssClass="textGrande" MaxLength="100" placeholder="Nombre."></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNombre" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                    <br />
                                </td>
                                <td>
                                    <asp:Label ID="lblEmail" runat="server" CssClass="letraMediana" Text="Email:"></asp:Label>
                                 </td>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="textGrande" MaxLength="100" placeholder="Email."></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEmail" CssClass="mensajeValidador" ErrorMessage="DIRECCIÓN DE CORREO INVALIDA." ForeColor="OrangeRed" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="Registro"></asp:RegularExpressionValidator>
                                 </td>
                            </tr>
                         <tr>                               
                                <td>
                                    <asp:Label ID="lblCalle" runat="server" CssClass="letraMediana" Text="Calle:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCalle" runat="server" CssClass="textGrande" MaxLength="100" placeholder="Calle."></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtCalle" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Label ID="lblEstado" runat="server" CssClass="letraMediana" Text="Estado:"></asp:Label>
                                </td>
                             <td>
                                 <asp:TextBox ID="txtEstado" runat="server" CssClass="textGrande" MaxLength="80" placeholder="Estado."></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtEstado" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                </td>
                         </tr>
                              <tr>
                                <td>
                                    <asp:Label ID="lblNumero" runat="server" CssClass="letraMediana" Text="Número Exterior:"></asp:Label>
                                  </td>
                                <td>
                                    <asp:TextBox ID="txtNumero" runat="server" CssClass="textGrande" MaxLength="50" placeholder="Número Ext."></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNumero" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtNumero" CssClass="valida" ErrorMessage="Ingresar solo numeros enteros" Font-Size="Small" SetFocusOnError="True" ValidationExpression="^\d+$" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                  </td>
                                <td>
                                    <asp:Label ID="lblNumeroInt" runat="server" CssClass="letraMediana" Text="Número Interior:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNumeroInt" runat="server" CssClass="textGrande" MaxLength="50" placeholder="Número Int."></asp:TextBox>
                                </td>
                            </tr>
                          <tr>
                                <td>
                                    <asp:Label ID="lblColonia" runat="server" CssClass="letraMediana" Text="Colonia:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtColonia" runat="server" CssClass="textGrande" MaxLength="100" placeholder="Colonia." style="margin-top: 0; margin-bottom: 0"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtColonia" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Label ID="lblLocalidad" runat="server" CssClass="letraMediana" Text="Localidad:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLocalidad" runat="server" CssClass="textGrande" MaxLength="80" placeholder="Localidad."></asp:TextBox>
                                </td>
                            </tr>
                              <tr>
                                <td>
                                    <asp:Label ID="lblMunicipio" runat="server" CssClass="letraMediana" Text="Municipio:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMunicipio" runat="server" CssClass="textGrande" MaxLength="80" placeholder="Municipio."></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtMunicipio" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Label ID="lblCP" runat="server" Text="Código Postal:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCP" runat="server" CssClass="textGrande" placeholder="Código Postal." MaxLength="5"></asp:TextBox>
                                   <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" CssClass="valida" ControlToValidate="txtCP" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^\d+$" ControlToValidate="txtCP" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                --%></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPais" runat="server" CssClass="letraMediana" Text="Pais:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPais" runat="server" CssClass="textGrande" MaxLength="50" placeholder="Pais." Text="Mexico"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtEstado" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    &nbsp;</td>
                                <td >
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtLocalidad" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                </td>
                            </tr>                            
                             <tr>
                                <td>
                                    <asp:Label ID="lblReferencia" runat="server" CssClass="letraMediana" Text="Referencia:"></asp:Label>
                                 </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtReferecnia" runat="server" CssClass="textGrande" Height="25px" MaxLength="200" placeholder="Referecnia." Rows="1" TextMode="MultiLine" Width="557px"></asp:TextBox>
                                 </td>
                                 
                            </tr>
                             <tr>
                                 <td>
                                     <asp:Label ID="lblRegimen" runat="server" CssClass="letraMediana" Text="Regimen Fiscal:"></asp:Label>
                                 </td>
                                 <td>
                                     <asp:TextBox ID="txtRegimen" runat="server" CssClass="textGrande" MaxLength="100" placeholder="Nombre."></asp:TextBox>
                                 </td>
                                 <td>
                                     <asp:Label ID="lblUso" runat="server" CssClass="letraMediana" Text="Uso CFDI:"></asp:Label>
                                 </td>
                                 <td>
                                     <asp:TextBox ID="txtUsoCFDI" runat="server" CssClass="textGrande" MaxLength="100" placeholder="Nombre."></asp:TextBox>
                                 </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align:center;">
                                    <%--<asp:Button ID="btnModificar" runat="server" Text="Modificar" OnClick="btnModificar_Click" Visible="false" />--%>
                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="guardar" OnClick="btnGuardar_Click" Visible="false" />
                                </td>
                                <td colspan="2" style="text-align:center;">
                                    <asp:Button ID="btnCancelar" runat="server" CausesValidation="False" Text="Cancelar" OnClick="btnCancelar_Click" Visible="true" />
                                </td>
                            </tr>
                        </table>
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
