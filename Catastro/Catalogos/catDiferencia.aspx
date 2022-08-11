<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="catDiferencia.aspx.cs" Inherits="Catastro.Catalogos.catDiferencia" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                        <td colspan="7">
                            <asp:Label ID="lbl_titulo" runat="server" CssClass="textModalTitulo2" Text="Captura de Diferencia"></asp:Label>
                        </td>
                    </tr>
                <tr>
                    <td style="height: 2px"><asp:HiddenField ID="hdfId" runat="server" />
                        <hr />
                    </td>
                </tr>
                <tr >
                   <%-- <td colspan="1" style="background-color: #b4b4b4;vertical-align:middle; height: 73px;">
                        <asp:Label ID="Label2" runat="server" CssClass="letraMediana" Text="Clave Catastral:"></asp:Label>
                    </td>--%>
                    <td colspan="4" style="background-color: #b4b4b4;vertical-align:middle; width: 224px; height: 73px;"> &nbsp;&nbsp;
                        <asp:Label ID="Label2" runat="server" CssClass="letraMediana" Text="Clave Catastral:"></asp:Label>  
                        <asp:TextBox ID="txtClvCastatral" runat="server" CssClass="textGrande" MaxLength="12" placeholder="Clave Catastral" style="margin-right: 54"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender runat="server" ID="meeFiltro" Mask="9999-99-999-999" MaskType="Number" InputDirection="RightToLeft" TargetControlID="txtClvCastatral" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtClvCastatral" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar" ></asp:RequiredFieldValidator>
                        &nbsp;<asp:ImageButton ID="imbBuscar" runat="server" ImageAlign="Middle" CausesValidation="False" ImageUrl="~/img/consultar.fw.png" OnClick="buscarClaveCatastral" />
                    </td >
                    <td colspan="1" style="height: 73px">
                        <asp:Label ID="lblCuentaPredial" runat="server"  Text="" Visible="false"></asp:Label>
                    </td>  
                    <td colspan="2" style="height: 73px">
                         <asp:Label ID="Label1" runat="server" Text="Último pago: " Visible="False" CssClass="letraSubTitulo"></asp:Label>
                        <asp:TextBox ID="txtUltPerPag" Visible="false"  class="letraMediana" runat="server" ValidateRequestMode="Enabled"></asp:TextBox>
                    </td>       
                </tr>
                <tr>
                    <td colspan="1" style="height: 40px"></td>
                    <td colspan="1" style="height: 40px">Base Gravable Ant</td>
                    <td colspan="1" style="height: 40px">Base Gravable Nueva</td>
                    <td colspan="1" style="height: 40px">Diferencia</td>
                    <td colspan="1" style="height: 40px">Impuesto</td>
                    <td colspan="1" style="height: 40px">Bimestre</td>
                    <td colspan="1" style="height: 40px">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="1"></td>
                    <td colspan="1">
                        <asp:TextBox ID="txtBaseAnt" runat="server" CssClass="textMedianoRight" MaxLength="11" placeholder="0.00"  ></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtBaseAnt" CssClass="valida"  Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d+(\.\d{2})?|\.\d{2}" ></asp:RegularExpressionValidator>
                        <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="txtBaseAnt" Display="Dynamic" MaximumValue="9999999.99" MinimumValue="0.0001" Type="Double"></asp:RangeValidator>
                    </td>
                    <td colspan="1">
                        <asp:TextBox ID="txtBaseNew" runat="server" CssClass="textMedianoRight" MaxLength="11" placeholder="0.00" AutoPostBack="true"  OnTextChanged="ValidadorImpuesto"></asp:TextBox>
                        &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtBaseNew" CssClass="valida"  Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d+(\.\d{2})?|\.\d{2}"></asp:RegularExpressionValidator>
                        <asp:RangeValidator ID="RangeValidator5" runat="server" ControlToValidate="txtBaseNew" Display="Dynamic" MaximumValue="999999.99" MinimumValue="0.0001" Type="Double"></asp:RangeValidator>
                    </td>
                    <td colspan="1">
                        <asp:TextBox ID="txtBaseDif" runat="server" CssClass="textMedianoRight" MaxLength="11" placeholder="0.00"  AutoPostBack="true"  OnTextChanged="ValidadorImpuesto"></asp:TextBox>
                        <br />
                    </td>
                    <td colspan="1">
                        <asp:TextBox ID="txtImpuesto" runat="server" CssClass="textMedianoRight" MaxLength="11" placeholder="0.00"></asp:TextBox>
                    </td>
                    <td colspan="1">
                        <asp:TextBox ID="txtBimestre" runat="server" CssClass="textMedianoRight" MaxLength="11" placeholder="0.00"></asp:TextBox>
                    </td>
                    <td colspan="1">
                        <asp:Button ID="btnCalcular" runat="server" OnClick="ValidadorImpuesto" Text="Guardar" />
                    </td>
                </tr>
                 <tr>
                     <td colspan="1">
                         <asp:Label ID="Label3" runat="server" CssClass="letraSubTitulo" Text="Concepto"></asp:Label>
                     </td>
                     <td colspan="1" style="width: 224px">
                         <asp:Label ID="Label4" runat="server" CssClass="letraSubTitulo" Text="Diferencia"></asp:Label>
                     </td>
                     <td colspan="5">
                         <asp:Label ID="Label5" runat="server" CssClass="letraSubTitulo" Text="Periodo de Vigencia"></asp:Label>
                     </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label6" runat="server" CssClass="letraMediana" Text="Avalúo"></asp:Label>
                    </td>
                    <td style="width: 224px">
                        <asp:TextBox ID="txtAvaluoDiferencia" runat="server" placeholder="0.00" MaxLength="11" CssClass="textMedianoRight"></asp:TextBox><br />
                        <asp:RangeValidator id="RangeValidator1" runat="server" ErrorMessage="Ingresar solo números mayor a 0 y menor a 999999.99" ControlToValidate="txtAvaluoDiferencia" MinimumValue="0.0001" MaximumValue="999999.99" Display="Dynamic" Type="Double"></asp:RangeValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Maximo 2 decimales." ValidationExpression="\d+(\.\d{2})?|\.\d{2}" ControlToValidate="txtAvaluoDiferencia" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                    </td>
                      <td>                          
                         <asp:DropDownList ID="ddlAvaluoBI" runat="server" CssClass="ddlChico"></asp:DropDownList>
                       </td> 
                    <td>   
                        <asp:DropDownList ID="ddlAvaluoEI" runat="server" CssClass="ddlMediano"></asp:DropDownList>
                         </td>     
                    <td>
                        <asp:Label ID="Label18" runat="server" CssClass="letraMediana" Text=" hasta "></asp:Label>
                    </td>
                     <td>                          
                        <asp:DropDownList ID="ddlAvaluoBF" runat="server" CssClass="ddlChico" ></asp:DropDownList>
                   </td> 
                    <td>
                         <asp:DropDownList ID="ddlAvaluoEF" runat="server" CssClass="ddlMediano"></asp:DropDownList>
                    </td>
                </tr>       
                <tr>
                    <td>
                        <asp:Label ID="Label7" runat="server" CssClass="letraMediana" Text="Tdo. de Dominio"></asp:Label>
                    </td>
                    <td colspan="1" style="width: 224px">
                        <asp:TextBox ID="txtTDominioDiferencia" runat="server" MaxLength="11" CssClass="textMedianoRight" placeholder="0.00"></asp:TextBox><br />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Maximo 2 decimales." ValidationExpression="\d+(\.\d{2})?|\.\d{2}" ControlToValidate="txtTDominioDiferencia" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                        <asp:RangeValidator id="RangeValidator2" runat="server" ErrorMessage="Ingresar solo números mayor a 0 y menor a 999999.99" ControlToValidate="txtTDominioDiferencia" MinimumValue="0.0001" MaximumValue="999999.99" Display="Dynamic" Type="Double"></asp:RangeValidator>
                    </td>
                     <td>                         
                          <asp:DropDownList ID="ddlTDominioBI" runat="server" CssClass="ddlChico"></asp:DropDownList>
                   </td> 
                    <td>
                        <asp:DropDownList ID="ddlTDominioEI" runat="server" CssClass="ddlMediano"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label8" runat="server" CssClass="letraMediana" Text=" hasta "></asp:Label>
                    </td>
                     <td>                           
                         <asp:DropDownList ID="ddlTDominioBF" runat="server" CssClass="ddlChico"></asp:DropDownList>
                   </td> 
                    <td>
                        <asp:DropDownList ID="ddlTDominioEF" runat="server" CssClass="ddlMediano"></asp:DropDownList>
                    </td>
                </tr>      
                <tr>
                    <td>
                        <asp:Label ID="Label9" runat="server" CssClass="letraMediana" Text="Construcción"></asp:Label>
                    </td>
                    <td colspan="1" style="width: 224px">
                        <asp:TextBox ID="txtConstruccionDiferencia" runat="server" CssClass="textMedianoRight" MaxLength="11" placeholder="0.00"></asp:TextBox><br />
                        <asp:RangeValidator id="RangeValidator3" runat="server" ErrorMessage="Ingresar solo números mayor a 0 y menor a 999999.99" ControlToValidate="txtConstruccionDiferencia" MinimumValue="0.0001" MaximumValue="999999.99" Display="Dynamic" Type="Double"></asp:RangeValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Maximo 2 decimales." ValidationExpression="\d+(\.\d{2})?|\.\d{2}" ControlToValidate="txtConstruccionDiferencia" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                    </td>
                     <td>                        
                          <asp:DropDownList ID="ddlConstruccionBI" runat="server" CssClass="ddlChico"></asp:DropDownList>
                   </td> 
                    <td>
                         <asp:DropDownList ID="ddlConstruccionEI" runat="server" CssClass="ddlMediano"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label10" runat="server" CssClass="letraMediana" Text=" hasta "></asp:Label>
                    </td>
                     <td>                        
                        <asp:DropDownList ID="ddlConstruccionBF" runat="server" CssClass="ddlChico" ></asp:DropDownList>
                   </td> 
                    <td>
                        <asp:DropDownList ID="ddlConstruccionEF" runat="server" CssClass="ddlMediano"></asp:DropDownList>
                    </td>
                </tr>   
                    <tr>
                         <td colspan="1">
                                    <asp:Label ID="Label11" runat="server" Text="  Notificación: " CssClass="letraMediana"></asp:Label>
                         </td>
                         <td colspan="3">
                        <asp:TextBox ID="txtFecha" runat="server" CssClass="textMedianoRight" MaxLength="50" placeholder="Fecha"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="txtFecha_MaskedEditExtender1" runat="server" BehaviorID="txtFecha_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFecha" />
                                    <ajaxToolkit:CalendarExtender ID="txtFecha_CalendarExtender1" runat="server" BehaviorID="txtFecha_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFecha" Format="dd/MM/yyyy" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" CssClass="valida" ControlToValidate="txtFecha" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                    </td>              
                        
                         <td colspan="1">
                                    <asp:Label ID="Label12" runat="server" Text="  Status: " CssClass="letraMediana"></asp:Label>
                         </td>
                       <td colspan="3"> 
                          <asp:DropDownList ID="ddlStatus" runat="server"></asp:DropDownList>
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="ddlStatus" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar" InitialValue="%" CssClass="valida"></asp:RequiredFieldValidator>                                                                                                                                                       
                   </td>           
                    </tr>     
                 <tr>
                        <td colspan="5">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="guardar" OnClick="btnGuardar_Click" />
                        </td>
                     <td colspan="2">
                         <asp:Button ID="btnCancelar" runat="server" CausesValidation="False" Text="Regresar" OnClick="btnCancelar_Click" />
                     </td>
                    </tr>
            </table>
            <hr />
            <asp:GridView ID="grdcreditos" runat="server" AutoGenerateColumns="False" CssClass="grd" AllowPaging="True"
                            AllowSorting="True" BorderStyle="None" ShowFooter="True">
                            <Columns>
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha Modificacion" SortExpression="Fecha" />
                                <asp:BoundField DataField="UsuarioMod" HeaderText="UsuarioMod" SortExpression="UsuarioMod" />
                                <asp:BoundField DataField="Ejercicio" HeaderText="Ejercicio" SortExpression="Ejercicio" />
                                <asp:BoundField DataField="ValorTerreno" HeaderText="Valor Terreno" SortExpression="ValorTerreno" />
                                <asp:BoundField DataField="ValorConstruccion" HeaderText="Valor Construcción" SortExpression="ValorConstruccion" />
                                <asp:BoundField DataField="ValorPredio" HeaderText="Base Gravable" SortExpression="ValorPredio" />
                                <asp:BoundField DataField="CreditoFiscal" HeaderText="Credito Fiscal" SortExpression="CreditoFiscal" DataFormatString="{0:C}" />
                                <asp:BoundField DataField="ImpuestoBimestral" HeaderText="Impuesto Bimestral" SortExpression="ImpuestoBimestral" DataFormatString="{0:C}" />
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmptySearch" runat="server" CssClass="letraTituloEmptyGrid">No se encontraron resultados</asp:Label>
                            </EmptyDataTemplate>
                            <FooterStyle CssClass="grdFooter" />
                            <HeaderStyle CssClass="grdHead" />
                            <RowStyle CssClass="grdRowPar" />
                        </asp:GridView>
            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
