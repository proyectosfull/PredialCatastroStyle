<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BuscarConvenio.aspx.cs" Inherits="Catastro.Convenios.BuscarConvenio" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 60px;">
                        </td>
                </tr>
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="Label1" runat="server" Text="Convenios" CssClass="letraTitulo"></asp:Label></td>
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
                                    <asp:DropDownList ID="ddlFiltro" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" >
                                        <asp:ListItem Value="">Todos</asp:ListItem>
                                        <asp:ListItem>Folio</asp:ListItem>
                                        <asp:ListItem Value="clave">Clave catastral</asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;<asp:TextBox ID="txtFiltro" runat="server" CssClass="textGrande"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="chkInactivo" runat="server" Text="Activos" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="False"
                                        ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" />
                                    <asp:Button ID="btnNuevo" runat="server" OnClick="btnNuevo_Click"
                                        Text="Nuevo Convenio" CausesValidation="False" />
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
                                <asp:BoundField DataField="IdConvenioEdoCta" HeaderText="Clave Catastral" SortExpression="IdConvenioEdoCta" />
                                <asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio" />
                                <asp:BoundField DataField="FechaIni" HeaderText="Fecha Inicio" SortExpression="FechaIni" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="FechaFin" HeaderText="Fecha Final" SortExpression="FechaFin" DataFormatString="{0:dd/MM/yyyy}"/>
                                <asp:BoundField DataField="NoParcialidades" HeaderText="No Parcialidades" SortExpression="NoParcialidades" />
                                <asp:BoundField DataField="ImporteTotal" HeaderText="Total" SortExpression="ImporteTotal" DataFormatString ="{0:C}"/>
        
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
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' Visible="false" />
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
                        <td colspan="2" style="font-family: 'Trebuchet MS'; font-size: 25px; color: #575655">Seleccionar predios</td>
                    </tr>
                    <tr>
                        <td colspan="2" style="font-family: 'Trebuchet MS'; font-size: 25px; color: #575655"> 
                  <div style="height: 200px; overflow: scroll;">
                         <asp:GridView ID="grdPredio" runat="server" AutoGenerateColumns="False"
                                                    CssClass="grd" DataKeyNames="id" AllowPaging="false"
                                                    BorderStyle="None" ShowFooter="True" OnRowCommand="grdPredio_RowCommand"  >
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-CssClass="" HeaderText="Clave" ItemStyle-Width="45px">
                                                            <ItemTemplate>                                                               
                                                                <asp:LinkButton ID="lnkClave" runat="server" CommandName="Selec"   CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ClavePredial")%>'><%# DataBinder.Eval(Container.DataItem, "ClavePredial")%></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre contribuyente" SortExpression="Nombre" />
                                                     
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblEmptySearch" runat="server" CssClass="letraTituloEmptyGrid">No se encontraron resultados</asp:Label>
                                                    </EmptyDataTemplate>
                                                    <FooterStyle CssClass="grdFooter" />
                                                    <HeaderStyle CssClass="grdHead" />
                                                    <RowStyle CssClass="grdRowPar" />
                                                </asp:GridView>
                             </div>
                      </td>
                    </tr>
                   
                   <tr>
                       <td style="width: 105px">
                         <%--  <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click"  />--%>

                       </td>
                       <td style="width: 286px">
                           <asp:Button ID="btnCancelar" runat="server" CausesValidation="False" Text="Cancelar" OnClick="btnCancelar_Click"  />
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

