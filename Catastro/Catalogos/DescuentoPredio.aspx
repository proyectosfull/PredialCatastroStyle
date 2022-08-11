<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DescuentoPredio.aspx.cs" Inherits="Catastro.Catalogos.DescuentoPredio" %>

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
                        <td>
                            <asp:Label ID="lblCve" runat="server" CssClass="letraSubTitulo" Font-Size="Medium" Text="Clave del predio:"></asp:Label>
                        </td>

                        <td style="width: 316px">
                            <asp:TextBox ID="txtCve" runat="server" CssClass="textChico" MaxLength="50" placeholder="clave del predio" Width="217px"></asp:TextBox>
                            <%--<ajaxToolkit:MaskedEditExtender ID="txtFolio_MaskedEditExtender" runat="server" BehaviorID="txtFolio_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" InputDirection="RightToLeft" Mask="9999999999" MaskType="Number" TargetControlID="txtCve" />--%>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtCve" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                        
                            
                        </td>
                        

                        <td>
                           <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" ValidationGroup="buscar" />
                        </td>
                    </tr>
                    
                    <tr>
                        <td colspan="6" style="height: 10px;">
                            <hr />
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl2" runat="server" CssClass="letraSubTitulo" Text="Clave del Predio:"></asp:Label>
                        </td>
                        <td style="width: 316px">
                            <asp:Label ID="lblPred" runat="server" CssClass="letraSubTitulo"></asp:Label>
                        </td>
                        
                        <td>
                             <asp:DropDownList ID="ddltipo" runat="server" Enabled="False">
                                <asp:ListItem Value="IP">Impuesto Predial</asp:ListItem>
                                <asp:ListItem Value="SM">Servicios Mpal</asp:ListItem>
                            </asp:DropDownList>
                            <%--<asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" Text="Guardar" ValidationGroup="guardar" Visible="False" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl3" runat="server" CssClass="letraSubTitulo" Text="Propietario:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblPropietario" runat="server" CssClass="letraSubTitulo"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="Button1" runat="server" OnClick="btnGuardar_Click" Text="Guardar" ValidationGroup="guardar" Visible="False"  />
                        </td>
                    </tr>

                    <tr>
                        <td colspan="6" style="height: 10px">
                            <asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" Text="Guardar" ValidationGroup="guardar" Visible="False"  />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:GridView ID="grdDetalle" runat="server" AutoGenerateColumns="False"
                                CssClass="grd" DataKeyNames="Id,Descuento,IdDescuento" BorderStyle="None" ShowFooter="True" >
                                <Columns>
                                    <asp:BoundField DataField="Cri" HeaderText="Cri" SortExpression="cri" />
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                                    <asp:TemplateField ItemStyle-CssClass="" HeaderText="Descuentos" ItemStyle-Width="135px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDscto" runat="server" CssClass="textChico" MaxLength="50" placeholder="Ingresar %" ></asp:TextBox>
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
                    <tr>
                        <td colspan="6">
                            
                        </td>
                    </tr>
                </table>
            </div>
            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="False" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
