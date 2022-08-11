<%@ Page Title="Descuentos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="catDescuento.aspx.cs" Inherits="Transito.Catalogos.catDescuento" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="Label1" runat="server" Text="Catalogo de Descuentos" CssClass="letraTitulo"></asp:Label></td>
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
                                    <asp:Button ID="btnNuevoDescuento" runat="server" OnClick="btnNuevo_Click"
                                        Text="Nuevo Descuento" CausesValidation="False" />
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
                                <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" />
                                <asp:BoundField DataField="Ejercicio" HeaderText="Ejercicio" SortExpression="Ejercicio" />
                                <asp:BoundField DataField="FechaInicio" HeaderText="Fecha Inicio" SortExpression="FechaInicio" />
                                <asp:BoundField DataField="FechaFin" HeaderText="Fecha Fin" SortExpression="FechaFin" />
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
                        <td colspan="2">
                            <asp:Label ID="lbl_titulo" runat="server" CssClass="textModalTitulo2" Text="Alta o Edición Descuentos"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:ValidationSummary ID="ValidationSummary1" HeaderText="Existen campos que faltan completar dentro de algún pestañado. <br /> Favor de revisar la información." runat="server" ValidationGroup="guardar" DisplayMode="SingleParagraph"  CssClass="valida"/>
                            <ajaxToolkit:Accordion ID="Accordion1" runat="server" FadeTransitions="True" FramesPerSecond="50" 
                                TransitionDuration="200" HeaderCssClass="accordionHeader"
                                 ContentCssClass="accordionContenido"> 
                                <Panes> 
                                    <ajaxToolkit:AccordionPane runat="server" ID="pnl1" > 
                                        <Header>
                                            INFORMACIÓN GENERAL
                                        </Header> 
                                        <Content>
                                            <Table style="width:100%;">
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:Label ID="lblUltAct" runat="server" Text="" CssClass="letraMediana"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:25%">
                                                        <asp:Label ID="lblClave" runat="server" Text="Clave:" CssClass="letraMediana"></asp:Label>
                                                        <br />
                                                        <asp:TextBox ID="txtClave" runat="server" CssClass="textChico" MaxLength="4" placeholder="Clave"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="txtClave" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td style="width:25%">
                                                        <br />
                                                        <asp:Label ID="lblEjercicio" runat="server" Text="Ejercicio:" CssClass="letraMediana"></asp:Label>
                                                        <br />
                                                        <asp:TextBox ID="txtEjercicio" runat="server" CssClass="textChico" MaxLength="4" placeholder="Ejercio"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" ControlToValidate="txtEjercicio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressValidator1" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^[0-9]*" ControlToValidate="txtEjercicio" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td style="width:25%">
                                                        <asp:Label ID="lblFechaInicio" runat="server" Text="Fecha Inicio:" CssClass="letraMediana"></asp:Label>
                                                        <br />
                                                        <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textMediano"  placeholder="Inicio" AutoCompleteType="Disabled"></asp:TextBox>
                                                        <ajaxToolkit:CalendarExtender ID="txtFechaInicio_CalendarExtender" runat="server" TargetControlID="txtFechaInicio" Format="yyyy-MM-dd" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="valida" ControlToValidate="txtFechaInicio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td style="width:25%">
                                                        <asp:Label ID="lblFechaFin" runat="server" Text="Fecha Fin:" CssClass="letraMediana"></asp:Label>
                                                        <br />
                                                        <asp:TextBox ID="txtFechaFin" runat="server" CssClass="textMediano"  placeholder="Final" AutoCompleteType="Disabled"></asp:TextBox>
                                                        <ajaxToolkit:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" TargetControlID="txtFechaFin" Format="yyyy-MM-dd"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" CssClass="valida" ControlToValidate="txtFechaFin" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="width:50%">
                                                        <asp:Label ID="lblDescripcion" runat="server" Text="Descripción:" CssClass="letraMediana"></asp:Label>
                                                        <asp:TextBox ID="txtDescripcion" TextMode="MultiLine" runat="server" CssClass="textMultiExtraGrande" Width="330px" MaxLength="100" placeholder="Descripción."></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" CssClass="valida" ControlToValidate="txtDescripcion" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td colspan="2" style="width:50%">
                                                        <asp:Label ID="lblAutorizacion" runat="server" Text="Autorización:" CssClass="letraMediana"></asp:Label>
                                                        <asp:TextBox ID="txtAutorizacion" TextMode="MultiLine" runat="server" CssClass="textMultiExtraGrande" Width="330px" MaxLength="100" placeholder="Autorización."></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" CssClass="valida" ControlToValidate="txtAutorizacion" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                            </Table>
                                        </Content> 
                                    </ajaxToolkit:AccordionPane> 
                                    <ajaxToolkit:AccordionPane runat="server"> 
                                        <Header>
                                            TABLA DE DESCUENTOS ANTICIPADOS
                                        </Header> 
                                        <Content>
                                            <Table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label2" runat="server" CssClass="letraMediana" Text="Anticipado Impuesto:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label3" runat="server" CssClass="letraMediana" Text="Anticipado Adicional:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label4" runat="server" CssClass="letraMediana" Text="Anticipado Limpieza:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label5" runat="server" CssClass="letraMediana" Text="Anticipado Recoleccion:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label6" runat="server" CssClass="letraMediana" Text="Anticipado Dap:"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="align-content:center;align-items:center;text-align:center;">
                                                    <td>
                                                        <asp:TextBox ID="txtAnticipadoImpuesto" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe." AutoCompleteType="Disabled"></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtAnticipadoImpuesto" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d+(\.\d{2})?|\.\d{2}" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAnticipadoAdicional" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe." AutoCompleteType="Disabled"></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtAnticipadoAdicional" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{1,3}(.\d{1,2})?" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAnticipadoLimpieza" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe." AutoCompleteType="Disabled"></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtAnticipadoLimpieza" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{1,3}(.\d{1,2})?" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAnticipadoRecoleccion" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe." AutoCompleteType="Disabled"></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtAnticipadoRecoleccion" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{1,3}(.\d{1,2})?" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAnticipadoDap" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe." AutoCompleteType="Disabled"></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtAnticipadoDap" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{1,3}(.\d{1,2})?" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSAmtInfr" runat="server" CssClass="letraMediana" Text="Anticipado Infraestructura:"></asp:Label>
                                                    </td>
                                                    <td>    
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtAnticipadoInfraestructura" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe." AutoCompleteType="Disabled"></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator21" runat="server" ControlToValidate="txtAnticipadoInfraestructura" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{1,3}(.\d{1,2})?" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td>    
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </Table>
                                        </Content> 
                                    </ajaxToolkit:AccordionPane> 
                                    <ajaxToolkit:AccordionPane runat="server"> 
                                        <Header>
                                            TABLA DE DESCUENTOS ACTUAL
                                        </Header> 
                                        <Content>
                                            <Table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblActualImpuesto" runat="server" CssClass="letraMediana" Text="Actual Impuesto:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblActualAdicional" runat="server" CssClass="letraMediana" Text="Actual Adicional:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblActualRecargo" runat="server" CssClass="letraMediana" Text="Actual Recargo:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="ActualLimpieza" runat="server" CssClass="letraMediana" Text="Actual Limpieza:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblActualRecoleccion" runat="server" CssClass="letraMediana" Text="Actual Recoleccion:"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="align-content:center;align-items:center;text-align:center;">
                                                    <td>
                                                        <asp:TextBox ID="txtActualImpuesto" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe." AutoCompleteType="Disabled"></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtActualImpuesto" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{1,3}(.\d{1,2})?" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtActualAdicional" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe." AutoCompleteType="Disabled"></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtActualAdicional" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{1,3}(.\d{1,2})?" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtActualRecargo" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe." AutoCompleteType="Disabled"></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtActualRecargo" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{1,3}(.\d{1,2})?" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtActualLimpieza" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe." AutoCompleteType="Disabled"></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="txtActualLimpieza" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{1,3}(.\d{1,2})?" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtActualRecoleccion" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe." AutoCompleteType="Disabled"></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ControlToValidate="txtActualRecoleccion" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{1,3}(.\d{1,2})?" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblActualDap" runat="server" CssClass="letraMediana" Text="Actual DAP:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label7" runat="server" CssClass="letraMediana" Text="Infraestructura:"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtActualDap" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe."></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server" ControlToValidate="txtActualDap" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{1,3}(.\d{1,2})?" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtInfraestructura" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe." AutoCompleteType="Disabled"></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator22" runat="server" ControlToValidate="txtInfraestructura" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{1,3}(.\d{1,2})?" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                            </Table>
                                        </Content> 
                                    </ajaxToolkit:AccordionPane> 
                                    <ajaxToolkit:AccordionPane runat="server"> 
                                        <Header>
                                            TABLA DE DESCUENTOS GENERAL
                                        </Header> 
                                        <Content>
                                            <Table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDiferencia" runat="server" CssClass="letraMediana" Text="Diferencia:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDiferenciaRecargo" runat="server" CssClass="letraMediana" Text="Diferencia Recargo:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblRezago" runat="server" CssClass="letraMediana" Text="Rezago:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblRezagoRecargo" runat="server" CssClass="letraMediana" Text="Rezago Recargo:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="RezagoAdicional" runat="server" CssClass="letraMediana" Text="Rezago Adicional:"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="align-content:center;align-items:center;text-align:center;">
                                                    <td>
                                                        <asp:TextBox ID="txtDiferencia" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe." AutoCompleteType="Disabled"></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" ControlToValidate="txtDiferencia" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{1,3}(.\d{1,2})?" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDiferenciaRecargo" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe." AutoCompleteType="Disabled"></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server" ControlToValidate="txtDiferenciaRecargo" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{1,3}(.\d{1,2})?" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRezago" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe." AutoCompleteType="Disabled"></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator14" runat="server" ControlToValidate="txtRezago" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{1,3}(.\d{1,2})?" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRezagoRecargo" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe." AutoCompleteType="Disabled"></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator15" runat="server" ControlToValidate="txtRezagoRecargo" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{1,3}(.\d{1,2})?" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRezagoAdicional" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe." AutoCompleteType="Disabled"></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator16" runat="server" ControlToValidate="txtRezagoAdicional" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{1,3}(.\d{1,2})?" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblBasegravable" runat="server" CssClass="letraMediana" Text="Basegravable:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblImporte" runat="server" CssClass="letraMediana" Text="Importe:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblMultas" runat="server" CssClass="letraMediana" Text="Multas:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblEjecucion" runat="server" CssClass="letraMediana" Text="Gastos de Ejecución:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblNotificacion" runat="server" CssClass="letraMediana" Text="Honorarios de Notificacion:"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtBasegravable" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe." AutoCompleteType="Disabled"></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator17" runat="server" ControlToValidate="txtBasegravable" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{1,3}(.\d{1,2})?" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtImporte" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe." AutoCompleteType="Disabled"></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator18" runat="server" ControlToValidate="txtImporte" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{1,3}(.\d{1,2})?" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtMultas" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe." AutoCompleteType="Disabled"></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator19" runat="server" ControlToValidate="txtMultas" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{1,3}(.\d{1,2})?" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEjecucion" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe." AutoCompleteType="Disabled"></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator20" runat="server" ControlToValidate="txtEjecucion" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{1,3}(.\d{1,2})?" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtHonorarios" runat="server" CssClass="textChico" MaxLength="6" placeholder="Importe." AutoCompleteType="Disabled"></asp:TextBox>%
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator23" runat="server" ControlToValidate="txtHonorarios" CssClass="valida" ErrorMessage="Maximo 2 decimales." Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{1,3}(.\d{1,2})?" ValidationGroup="guardar"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                            </Table>
                                        </Content> 
                                    </ajaxToolkit:AccordionPane> 
                                </Panes> 
                            </ajaxToolkit:Accordion> 
                            <br />
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
