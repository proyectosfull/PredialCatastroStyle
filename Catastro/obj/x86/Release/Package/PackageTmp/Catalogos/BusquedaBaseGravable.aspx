<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BusquedaBaseGravable.aspx.cs" Inherits="Catastro.Catalogos.BusquedaBaseGravable" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function calculaValor() {
            var VT = parseFloat(document.getElementById("MainContent_txtValorTerreno").value.replace(',','')|| 0);
            var VC = parseFloat(document.getElementById("MainContent_txtValorConstruccion").value.replace(',', '') || 0);
            var V = document.getElementById("MainContent_txtValor");
            V.value = '$' + parseFloat(VT + VC).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
        }
        
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 30px;"></td>
                </tr>
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="Label1" runat="server" Text="Base Gravable" CssClass="letraTitulo"></asp:Label></td>
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
                                    <%--<asp:DropDownList ID="ddlFiltro" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" >
                                       <asp:ListItem Text="Clave Catastral" Value="ClavePredial" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>--%>
                                    <asp:Label ID="Label2" runat="server" Text="Clave Catastral" CssClass="letraTitulo"></asp:Label>
                                    &nbsp;<asp:TextBox ID="txtFiltro" runat="server" CssClass="textGrande"></asp:TextBox>
                                    <%--<ajaxToolkit:MaskedEditExtender  runat="server" ID="meeFiltro" Mask="9999-99-999-999" TargetControlID="txtFiltro" />--%>
                                    <ajaxToolkit:MaskedEditExtender runat="server" ID="meeFiltro" Mask="9999-99-999-999" TargetControlID="txtFiltro" MaskType="Number" InputDirection="RightToLeft" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="valida" ControlToValidate="txtFiltro" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                                    &nbsp;&nbsp;&nbsp;
                                    <%--<asp:CheckBox ID="chkInactivo" runat="server" Text="Activos" />--%>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="True" ValidationGroup="buscar"
                                        ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnNuevo" runat="server" OnClick="btnNuevo_Click"
                                        Text="Nueva Base Gravable" CausesValidation="False" Visible="False" />
                                    <%--<asp:HiddenField ID="hdfidBG" runat="server" />--%>
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
                            CssClass="grd" DataKeyNames="id,activo,ejercicio" AllowPaging="True"
                            AllowSorting="True" BorderStyle="None" ShowFooter="True"
                            OnRowCommand="grd_RowCommand" OnSorting="grd_Sorting" OnPageIndexChanging="grd_PageIndexChanging" OnRowDataBound="grd_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="Ejercicio" HeaderText="Ejercicio" SortExpression="Ejercicio" />
                                <asp:BoundField DataField="Bimestre" HeaderText="Bimestre" SortExpression="Bimestre" />
                                <asp:BoundField DataField="Valor" HeaderText="Valor" SortExpression="Valor" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                                <asp:BoundField DataField="FechaModificacion" HeaderText="Fecha Modificación" SortExpression="FechaModificacion" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="IdUsuario" HeaderText="Usuario Modificación" SortExpression="IdUsuario" />
                                <asp:TemplateField HeaderText="Activo" SortExpression="Activo">
                                    <ItemTemplate><%# (Boolean.Parse(Eval("Activo").ToString())) ? "SI" : "NO" %></ItemTemplate>
                                </asp:TemplateField>
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
                                    </ItemTemplate>
                                    <ItemStyle Width="135px" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmptySearch" runat="server" CssClass="letraTituloEmptyGrid">  </asp:Label>
                            </EmptyDataTemplate>
                            <FooterStyle CssClass="grdFooter" />
                            <HeaderStyle CssClass="grdHead" />
                            <RowStyle CssClass="grdRowPar" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>

            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />


            <asp:Panel ID="pnlBG" runat="server" BackColor="White" ScrollBars="Vertical" Style="padding: 30px; margin-right: 21px; width: 1000px; height: 650px; max-height: 90%; max-width: 90%" BorderStyle="Solid">

                <table>
                    
                    <tr>
                        <td style="width: 135px; font-size: 12px; color: #666666;">
                            <span style="font-weight: bold">Clave Catastral:</span></td>
                        <td >
                            <asp:TextBox ID="txtClavePredial" runat="server" Enabled="false" CssClass="textGrande" placeholder="Clave Predial." MaxLength="15"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" ControlToValidate="txtClavePredial" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            
                            
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblCuentaPredial" runat="server" CssClass="letraMediana"></asp:Label> &nbsp;&nbsp;
                            <asp:Label ID="lblAlerta" runat="server" CssClass="letraMediana" Font-Size="Medium" Text="" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 135px">
                            <asp:Label ID="lblFechaEvaluo" runat="server" Text="Fecha Evaluo:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFechaEvaluo" runat="server" CssClass="textGrande" placeholder="Fecha Evaluo."></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="txtFechaEvaluo_MaskedEditExtender" runat="server" BehaviorID="txtFechaEvaluo_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaEvaluo" />
                            <ajaxToolkit:CalendarExtender ID="txtFechaEvaluo_CalendarExtender" runat="server" BehaviorID="txtFechaEvaluo_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFechaEvaluo" Format="dd/MM/yyyy" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" CssClass="valida" ControlToValidate="txtFechaEvaluo" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblPeriodoPagado" runat="server" CssClass="letraMediana"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblNombrePredio" runat="server" CssClass="letraMediana" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 135px">
                            <asp:Label ID="lblNombre" runat="server" Text="Ejercicio:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <%--<asp:TextBox ID="txtEjercicio" runat="server" CssClass="textGrande" Enabled="false" MaxLength="4" placeholder="Ejercicio."></asp:TextBox>--%>
                            <asp:DropDownList ID="ddlEjercicio" runat="server" AutoPostBack="true" OnTextChanged="ddlEjercicio_TextChanged"></asp:DropDownList>
                            <asp:TextBox ID="txtEjercicio" runat="server" CssClass="textGrande" MaxLength="4" placeholder="Ejercicio." Width="130px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" CssClass="valida" ControlToValidate="txtEjercicio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^\d+$" ControlToValidate="txtEjercicio" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                            <%--<asp:CheckBox ID="cbejercicios" runat="server" Text="Actualizar Posteriores" Checked="false" />--%>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" CssClass="valida" ControlToValidate="txtEjercicio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^\d+$" ControlToValidate="txtEjercicio" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>--%>
                            <%--<asp:HiddenField runat="server" ID="hdfaniofin" />--%>
                            <%--<asp:HiddenField runat="server" ID="hdfajersup" />--%>
                            <%-- <asp:Button ID="btnAgregarSiguiente" runat="server" Text="Agregar Siguiente Año" Visible="false" OnClick="btnAgregarSiguiente_Click" />
                                    <asp:Button ID="btnAgregarAnterior" runat="server" Text="Agregar Año Anterior" Visible="false" OnClick="btnAgregarAnterior_Click" />
                                    <asp:Button ID="btncancelarnuevo" runat="server" Text="Cancelar" Visible="false" OnClick="btncancelarnuevo_Click" />--%>

                        </td>
                        <td>
                            <asp:Label ID="lblBimestre" runat="server" Text="Bimestre:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBimestre" runat="server">
                                <asp:ListItem Text="1 - Enero-Febrero" Value="1"></asp:ListItem>
                                <asp:ListItem Text="2 - Marzo-Abril" Value="2"></asp:ListItem>
                                <asp:ListItem Text="3 - Mayo-Junio" Value="3"></asp:ListItem>
                                <asp:ListItem Text="4 - Julio-Agosto" Value="4"></asp:ListItem>
                                <asp:ListItem Text="5 - Septiembre-Octubre" Value="5"></asp:ListItem>
                                <asp:ListItem Text="6 - Noviembre-Diciembre" Value="6" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4"><hr /></td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            
                            <asp:Label ID="lblSuperTerreno" runat="server" Text="Superficie del Terreno:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSuperTerreno" runat="server" CssClass="textGrande" placeholder="Superficie del Terreno." MaxLength="12"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" CssClass="valida" ControlToValidate="txtSuperTerreno" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="\d+(\.\d{1,3})?|\.\d{1,2}" ControlToValidate="txtSuperTerreno" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                        <td>
                            <asp:Label ID="lblSuperficeConstruccion" runat="server" Text="Superficie Construcción:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSuperficeConstruccion" runat="server" CssClass="textGrande" placeholder="Superficie Construcción." MaxLength="12"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" CssClass="valida" ControlToValidate="txtSuperficeConstruccion" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="\d+(\.\d{1,3})?|\.\d{1,2}" ControlToValidate="txtSuperficeConstruccion" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>

                    </tr>
                    <tr>
                        <td style="width: 100px">
                            <asp:Label ID="lblTerrenoPrivativo" runat="server" Text="Terreno Privativo:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTerrenoPrivativo" runat="server" CssClass="textGrande" placeholder="Terreno Privativo." MaxLength="12"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" CssClass="valida" ControlToValidate="txtTerrenoPrivativo" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="\d+(\.\d{1,3})?|\.\d{1,2}" ControlToValidate="txtTerrenoPrivativo" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                        <td>
                            <asp:Label ID="lblConstruccionPrivativa" runat="server" Text="Construcción Privativa:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtConstruccionPrivativa" runat="server" CssClass="textGrande" placeholder="Construcción Privativa." MaxLength="12"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" CssClass="valida" ControlToValidate="txtConstruccionPrivativa" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="\d+(\.\d{1,3})?|\.\d{1,2}" ControlToValidate="txtConstruccionPrivativa" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>

                    </tr>
                    <tr>

                        <td style="width: 100px">
                            <asp:Label ID="lblTerrenoComun" runat="server" Text="Terreno Común:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTerrenoComun" runat="server" CssClass="textGrande" placeholder="Terreno Común." MaxLength="12"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" CssClass="valida" ControlToValidate="txtTerrenoComun" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="\d+(\.\d{1,3})?|\.\d{1,2}" ControlToValidate="txtTerrenoComun" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                        <td>
                            <asp:Label ID="lblConstruccionComun" runat="server" Text="Construcción Común:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtConstruccionComun" runat="server" CssClass="textGrande" placeholder="Construcción Común." MaxLength="12"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" CssClass="valida" ControlToValidate="txtConstruccionComun" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="\d+(\.\d{1,3})?|\.\d{1,2}" ControlToValidate="txtConstruccionComun" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            <asp:Label ID="lblValorTerreno" runat="server" Text="Valor Terreno:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtValorTerreno" runat="server" CssClass="textGrande" placeholder="Valor Terreno." MaxLength="12" onchange="calculaValor();"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" CssClass="valida" ControlToValidate="txtValorTerreno" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="^\d+(,\d{3})*(\.\d+)?$" ControlToValidate="txtValorTerreno" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                        <td>
                            <asp:Label ID="lblValorConstruccion" runat="server" Text="Valor Construcción:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtValorConstruccion" runat="server" CssClass="textGrande" placeholder="Valor Construcción." MaxLength="12" onchange="calculaValor();"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="txtValorConstruccion" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="^\d+(,\d{3})*(\.\d+)?$" ControlToValidate="txtValorConstruccion" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>

                    </tr>
                    <tr>
                        <td style="width: 135px">
                            <asp:Label ID="lblValor" runat="server" Text="Base Gravable:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtValor" runat="server" CssClass="valida" placeholder="Valor." MaxLength="16" onkeydown="return false;" BorderStyle="None"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:CheckBox ID="cHKejercicios" runat="server" Checked="false" />
                            &nbsp;<asp:Label ID="Label3" runat="server" CssClass="letraMediana" Font-Size="Small" Text="Actualizar años posteriores"></asp:Label>
                        </td>
                    </tr>
                    <%--<tr>
                        <td>
                            <asp:Label ID="lblValorConstruccionComun" runat="server" Text="Valor Construcción Común:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtValorConstruccionComun" runat="server" CssClass="textGrande" placeholder="Valor Construcción Común." MaxLength="12"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender runat="server" ID="MaskedEditExtenderVTC" Mask="999,999,999.99" TargetControlID="txtValorConstruccionComun" MaskType="Number" InputDirection="RightToLeft" />                            
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="\d+(\.\d{1,3})?|\.\d{1,2}" ControlToValidate="txtValorConstruccionComun" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                        <td>
                            <asp:Label ID="lblValorConstruccionPrivativa" runat="server" Text="Valor Construcción Privativa:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtValorConstruccionPrivativa" runat="server" CssClass="textGrande" placeholder="Valor Construcción Privativa." MaxLength="12"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender runat="server" ID="MaskedEditExtenderVCP" Mask="999,999,999.99" TargetControlID="txtValorConstruccionPrivativa" MaskType="Number" InputDirection="RightToLeft" />
                            
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="\d+(\.\d{1,3})?|\.\d{1,2}" ControlToValidate="txtValorConstruccionPrivativa" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblPrototipo" runat="server" Text="Prototipo:" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPrototipo" runat="server" CssClass="textGrande" placeholder="Prototipo." MaxLength="12"></asp:TextBox>
                            
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="\d+(\.\d{1,3})?|\.\d{1,2}" ControlToValidate="txtPrototipo" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        </td>
                    </tr>--%>
                    <tr>
                        <td style="width: 135px">
                            &nbsp;</td>                        
                        <td style="text-align: center;">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="guardar" OnClick="btnGuardar_Click" />
                        </td>
                        <td style="text-align: center;">
                            <asp:Button ID="btnhistorial" runat="server" CausesValidation="False" Text="Regresar" OnClick="btnHistorial_Click" />
                        </td>
                        <td style="text-align: center;">
                            <asp:Button ID="btnCierraPanel" runat="server" CausesValidation="False" Text="Nueva búsqueda" OnClick="btnCierraPanel_Click" />
                        </td>
                    </tr>
                </table>

            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="pnlBG_Modal" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnlBG" TargetControlID="btnPnlBG">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnPnlBG" runat="server" />



        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

