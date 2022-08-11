<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReporteAntecPredio.aspx.cs" Inherits="Catastro.Recibos.ReporteAntecPredio" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 60px;">
                <asp:Label ID="Label1" runat="server" Text="Reporte Antecedente del Predio." CssClass="letraTitulo"></asp:Label></td>
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
                        <td  width="300px"> 
                            <asp:RadioButtonList ID="RBLtipo" runat="server" AutoPostBack="True"  RepeatDirection="Horizontal" OnSelectedIndexChanged="RBLtipo_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="Clave">Clave Castastral</asp:ListItem>
                                <asp:ListItem>Contribuyente</asp:ListItem>
                            </asp:RadioButtonList>                        
                        </td>  
                        <td width="50px">

                        </td>                     
                        <td  width="300px">    
                            <asp:RadioButtonList ID="rblFecha" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="Tramite">Fecha de Tramite</asp:ListItem>
                                <asp:ListItem Value="Pago">Fecha de Pago</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                         <td>    
                        </td>
                    </tr>
                    <tr>
                        <td>                       
                        
                            <asp:TextBox ID="txtClave" runat="server"></asp:TextBox>
                        
                            <asp:HiddenField ID="hdfIdContribuyente" runat="server" Value="0" />
                        
                            <asp:Label ID="lblContribuyente" runat="server" CssClass="letraSubTitulo" Text="" Visible="False"></asp:Label>
                        
                        </td>
                      <td width="50px">

                        <asp:ImageButton ID="imBuscarPropietario" runat="server" Height="39px" ImageUrl="~/Img/persona.png" OnClick="buscarPropietario" Visible="False" ToolTip="Contribuyente"/>
                        
                        </td> 
                        <td>
                            <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textMediano" placeholder="Fecha Inicio."  Width="120px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvFechaInicio" runat="server" ControlToValidate="txtFechaInicio" CssClass="valida" ErrorMessage="*" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                            <ajaxToolkit:MaskedEditExtender ID="txtFechaInicio_MaskedEditExtender" runat="server" BehaviorID="txtFechaInfra_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaInicio" />
                            <ajaxToolkit:CalendarExtender ID="txtFechaInicio_CalendarExtender" runat="server" BehaviorID="txtFechaInfra_CalendarExtender" CssClass="CalendarCSS" Format="dd/MM/yyyy" TargetControlID="txtFechaInicio" />
                            <asp:TextBox ID="txtFechaFin" runat="server" CssClass="textMediano" placeholder="Fecha Final."></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="txtFechaFin_MaskedEditExtender" runat="server" BehaviorID="txtFechaFin_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaFin" />
                            <asp:RequiredFieldValidator ID="rfvFechaFin" runat="server" ControlToValidate="txtFechaFin" CssClass="valida" ErrorMessage="*" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="txtFechaInfra_CalendarExtender" CssClass="CalendarCSS" Format="dd/MM/yyyy" TargetControlID="txtFechaFin" />
                        </td>
                        <td>
                            <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="true" ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" ValidationGroup="buscar" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="padding: 0px 10px 10px 10px;">
                <asp:Panel ID="pnlReport" runat="server" Visible="false">
                    <div class="row">
                        <div class="col-md-12">
                            <input type="button" id="printreport" value="imprimir" onclick="printReport('<%=rpt.ClientID %>    ')" />
                        </div>
                    </div>
                </asp:Panel>
                <rsweb:ReportViewer ID="rpt" runat="server" Height="500px" Width="900px" ShowPrintButton="true"></rsweb:ReportViewer>
        </tr>
    </table>

    
<asp:Panel ID="pnlPropietario" runat="server" class="formPanel">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="height: 60px;">
                            <asp:Label ID="Label2" runat="server" Text="Propietarios" CssClass="letraTitulo"></asp:Label></td>
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
                                        <asp:DropDownList ID="ddlFiltro" runat="server" />
                                        <asp:RequiredFieldValidator ID="rfvFechaInicio1" runat="server" ControlToValidate="ddlFiltro" CssClass="valida" ErrorMessage="*" ValidationGroup="buscarC" InitialValue="%"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFiltro" runat="server" CssClass="textGrande"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvFechaInicio0" runat="server" ControlToValidate="txtFiltro" CssClass="valida" ErrorMessage="*" ValidationGroup="buscarC"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="btnPropietarioM" runat="server" 
                                            ImageUrl="~/img/consultar.fw.png" OnClick="consultarPropietario" ValidationGroup="buscarC" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnCancelarPropietarios" runat="server" OnClick="btnCancelarPropietario_Click"
                                            Text="Cancelar" CausesValidation="False" />
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
                            <asp:GridView ID="grdPropietarios" runat="server" AutoGenerateColumns="False"
                                CssClass="grd" DataKeyNames="IdContribuyente" AllowPaging="True"
                                AllowSorting="True" BorderStyle="None" ShowFooter="True" 
                                OnRowCommand="grdPropietarios_RowCommand"  OnSorting="grdPropietarios_Sorting" 
                                OnPageIndexChanging="grdPropietarios_PageIndexChanging" PageSize="5">
                                <Columns>
                                    <asp:BoundField DataField="NombreCompleto" HeaderText="Nombre Completo" SortExpression="NombreCompleto" />
                                    <asp:BoundField DataField="Domicilio" HeaderText="Domicilio" SortExpression="Domicilio" />
                                    <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="135px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgActivar" runat="server" ToolTip="Seleccionar"
                                                ImageUrl="~/img/Activar.png"
                                                CssClass="imgButtonGrid"
                                                CommandName="Activar"
                                                CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>'/>
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
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="modalPropietario" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnlPropietario" TargetControlID="btnPropietario">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnPropietario" runat="server" />

</asp:Content>
