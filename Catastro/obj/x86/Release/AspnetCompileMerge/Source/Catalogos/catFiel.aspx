<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="catFiel.aspx.cs" Inherits="Catastro.Catalogos.catFiel" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="Label1" runat="server" Text="FIEL" CssClass="letraTitulo"></asp:Label></td>
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
                                        Text="Nuevo FIEL" CausesValidation="False" />
                                </td>
                                <td>
                                    <asp:Button ID="btnConsultarCreditos" runat="server"
                                        Text="Consultar Creditos" CausesValidation="False" OnClick="btnConsultarCreditos_Click" /></td>
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
                                <asp:BoundField DataField="RFC" HeaderText="RFC" SortExpression="RFC" />
                                <asp:BoundField DataField="Calle" HeaderText="Calle" SortExpression="Calle" />
                                <asp:BoundField DataField="Municipio" HeaderText="Municipio" SortExpression="Municipio" />
                                <asp:BoundField DataField="Estado" HeaderText="Estado" SortExpression="Estado" />
                                <asp:BoundField DataField="Pais" HeaderText="País" SortExpression="Pais" />
                                <asp:BoundField DataField="CodigoPostal" HeaderText="Código Postal" SortExpression="CodigoPostal" />
                                <asp:BoundField DataField="NoExterior" HeaderText="No. Exterior" SortExpression="NoExterior" />
                                <asp:BoundField DataField="NoInterior" HeaderText="No. Interior" SortExpression="NoInterior" />
                                <asp:BoundField DataField="Colonia" HeaderText="Colonia" SortExpression="Colonia" />
                                <asp:BoundField DataField="Localidad" HeaderText="Localidad" SortExpression="Localidad" />
                                <asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" />
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
                        <td colspan="3">
                            <asp:Label ID="lbl_titulo" runat="server" CssClass="textModalTitulo2" Text="Alta o Edición de FIEL"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label Text="Nombre:" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label Text="RFC:" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label Text="Calle:" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRFC" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="txtRFC" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCalle" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" ControlToValidate="txtCalle" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label Text="No Exterior:" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label Text="No Interior:" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label Text="Colonia:" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtNoExterior" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoInterior" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtColonia" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label Text="Código Postal:" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label Text="Localidad:" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label Text="Municipio:" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtCodigoPostal" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" CssClass="valida" ControlToValidate="txtCodigoPostal" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLocalidad" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMunicipio" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="valida" ControlToValidate="txtMunicipio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label Text="Estado:" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label Text="Páis:" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label Text="Referencia:" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtEstado" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="valida" ControlToValidate="txtEstado" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPais" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" CssClass="valida" ControlToValidate="txtPais" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReferencia" runat="server" CssClass="textMediano" MaxLength="50" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label Text="Archivo KEY:" runat="server"></asp:Label>
                        </td>
                        <td></td>
                        <td>
                            <asp:Label Text="KeyPass" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:FileUpload ID="fileKey" runat="server" AllowMultiple="false" />
                            <br />
                            <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="fileKey"
                                runat="server" Display="Dynamic" CssClass="valida" ValidationGroup="guardar" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.key|.KEY)$"
                                ControlToValidate="fileKey" runat="server" CssClass="valida" ErrorMessage="*Solo se permiten archivos KEY."
                                Display="Dynamic" ValidationGroup="guardar" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtKeyPass" runat="server" CssClass="textMediano" MaxLength="50" Width="250px" TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label Text="Archivo CER:" runat="server"></asp:Label>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:FileUpload ID="fileCer" runat="server" AllowMultiple="false" />
                            <br />
                            <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="fileCer"
                                runat="server" Display="Dynamic" CssClass="valida" ValidationGroup="guardar" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.cer|.CER)$"
                                ControlToValidate="fileCer" runat="server" CssClass="valida" ErrorMessage="*Solo se permiten archivos CER."
                                Display="Dynamic" ValidationGroup="guardar" />
                        </td>
                        <td></td>
                    </tr>

                    <tr>
                        <td>
                            <asp:Label Text="Logo:" runat="server"></asp:Label>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:FileUpload ID="fileLogo" runat="server" AllowMultiple="false" />
                            <br />
                            <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="fileLogo"
                                runat="server" Display="Dynamic" CssClass="valida" ValidationGroup="guardar" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.jpeg)$"
                                ControlToValidate="fileLogo" runat="server" CssClass="valida" ErrorMessage="*Solo se permiten archivos de png, jpg,jpeg"
                                Display="Dynamic" ValidationGroup="guardar" />
                        </td>
                        <td></td>
                    </tr>
                    <caption>
                        <br />
                        <tr>
                            <td>
                                <asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" Text="Guardar" ValidationGroup="guardar" />
                            </td>
                            <td>
                                <asp:Button ID="btnCancelar" runat="server" CausesValidation="False" OnClick="btnCancelar_Click" Text="Cancelar" />
                            </td>
                            <td></td>
                        </tr>
                    </caption>

                </table>
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="pnl_Modal" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnl" TargetControlID="btnPnl">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnPnl" runat="server" />

            <asp:Panel ID="pnlCreditos" runat="server" class="formPanel">
                <table>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Label16" runat="server" CssClass="textModalTitulo2" Text="Consulta de Creditos:"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="grdTimbres" runat="server" AutoGenerateColumns="False"
                            CssClass="grd" AllowPaging="True" BorderStyle="None" ShowFooter="True" OnPageIndexChanging="grdTimbres_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="numPaquete" HeaderText="N. Paquete"/>
                                <asp:BoundField DataField="fechaActivacion" HeaderText="Fecha de Activación"/>
                                <asp:BoundField DataField="fechaVencimiento" HeaderText="Fecha de Vencimiento"/>
                                <asp:BoundField DataField="paquete" HeaderText="Paquete" />
                                <asp:BoundField DataField="timbres" HeaderText="Timbres"/>
                                <asp:BoundField DataField="timbresRestantes" HeaderText="Timbres Restantes" />
                                <asp:BoundField DataField="timbresUsados" HeaderText="Timbres Usados" />
                                <asp:BoundField DataField="vigente" HeaderText="Vigente" />                                
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
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnCerrar" runat="server" Text="Cerrar" CausesValidation="False" OnClick="btnCerrar_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="modalPnlCreditos" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnlCreditos" TargetControlID="btnPnlCreditos">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnPnlCreditos" runat="server" />


            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnGuardar" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
