<%@ Page Title=""  Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="rptRezago.aspx.cs" Inherits="Catastro.Rezagos.rptRezago" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <br />
                                                <asp:Label ID="Label1" runat="server" Text="Rango de claves" CssClass="letraMediana"></asp:Label>
                                                <asp:TextBox ID="txtRangoDe" runat="server" CssClass="textMediano" MaxLength="12" placeholder="Rango de"></asp:TextBox>
    <asp:Label ID="Label2" runat="server" Text=" a " CssClass="letraMediana"></asp:Label>
    <asp:TextBox ID="txtRangoA" runat="server" CssClass="textMediano" MaxLength="12" placeholder="Rango a"></asp:TextBox>

    
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Calcular hasta el periodo:<asp:DropDownList ID="ddlBimestre" runat="server" CssClass="selectChico" Width="70px">
                                                    <asp:ListItem Selected="True" Value="0">Bimestre</asp:ListItem>
                                                    <asp:ListItem>1</asp:ListItem>
                                                    <asp:ListItem>2</asp:ListItem>
                                                    <asp:ListItem>3</asp:ListItem>
                                                    <asp:ListItem>4</asp:ListItem>
                                                    <asp:ListItem>5</asp:ListItem>
                                                    <asp:ListItem>6</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlAnio" runat="server" CssClass="selectChico" Width="110px"></asp:DropDownList>

    
    
                                <table style="width: 100%">
                                        <%-- <tr>
                                            <td>
                                                <asp:Label ID="lblDocumento" runat="server" Text="Con Documento:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList ID="ddlDocumento" runat="server"></asp:DropDownList>
                                            </td>
                                            <td style="width: 40px"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkBaldios" runat="server" Text=" Ignorar los predios baldíos" CssClass="letraMediana" AutoPostBack="True" OnCheckedChanged="chkBaldios_CheckedChanged" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkUBaldios" runat="server" Text=" Únicamente baldíos" CssClass="letraMediana" AutoPostBack="True" OnCheckedChanged="chkUBaldios_CheckedChanged" />
                                            </td>
                                            <td style="width: 40px"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkPExentos" runat="server" Text=" Ignorar los predíos exentos" CssClass="letraMediana" AutoPostBack="True" OnCheckedChanged="chkPExentos_CheckedChanged" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkExentos" runat="server" CssClass="letraMediana" Text=" Solo exentos" AutoPostBack="True" OnCheckedChanged="chkExentos_CheckedChanged" />
                                            </td>
                                            <td style="width: 40px"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblRango" runat="server" Text="Rango de Claves:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="TextBox1" runat="server" CssClass="textMediano" MaxLength="12" placeholder="Rango de"></asp:TextBox>
                                                -
                                                <asp:TextBox ID="TextBox2" runat="server" CssClass="textMediano" MaxLength="12" placeholder="Rango a"></asp:TextBox>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td style="width: 40px"></td>
                                        </tr>--%>
                                        <tr>
                                            <td class="auto-style4">
                                                <asp:Label ID="lblContribuyente" runat="server" Text="Contribuyente:" CssClass="letraMediana"></asp:Label>
                                                <br />
                                                <asp:TextBox ID="txtContribuyente" runat="server" CssClass="textExtraGrande" MaxLength="200" placeholder="Contribuyente"></asp:TextBox>
                                                <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="False"
                                                    ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" />
                                                <asp:ImageButton ID="imgBorrarCon" runat="server" ImageUrl="~/Img/eliminar.png" OnClick="imgBorrarCon_Click" />                      
                                                <asp:HiddenField ID="hdfIdCon" runat="server" />
                                            </td>
                                            <td class="auto-style4">
                                                <asp:Label ID="lblColonia" runat="server" CssClass="letraMediana" Text="Colonia:"></asp:Label>
                                                <br />
                                                <asp:TextBox ID="txtColonia" runat="server" CssClass="textExtraGrande" MaxLength="200" placeholder="Colonia"></asp:TextBox>
                                                <asp:ImageButton ID="imbColonia" runat="server" CausesValidation="False" ImageUrl="~/img/consultar.fw.png" OnClick="imbColonia_Click" />
                                                <asp:ImageButton ID="imbBorrarColonia" runat="server" ImageUrl="~/Img/eliminar.png" OnClick="imbBorrarColonia_Click" />
                                                <asp:HiddenField ID="hdfIdColonia" runat="server" />
                                            </td>
                                            <td class="auto-style4">
                                                <asp:Label ID="lblCondominio" runat="server" CssClass="letraMediana" Text="Condominio:"></asp:Label>
                                                <br />
                                    <asp:DropDownList ID="ddlCondominio" runat="server" />
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td colspan="3" style="text-align: left" class="auto-style3">
                                                <asp:Label ID="lblClaves" runat="server" Text="Claves:" CssClass="letraMediana"></asp:Label>
                                                <asp:Label ID="lblClaveExp" runat="server" Text="(Listado de claves separadas por coma, por ejemplo: 000005138001,000005138002,000005138003)" CssClass="letraChica"></asp:Label>
                                                <br />
                                                <asp:TextBox ID="txtClave" runat="server" CssClass="textExtraGrande" TextMode="MultiLine" Height="54px" Width="508px"></asp:TextBox>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnBuscar1" runat="server" OnClick="btnBuscar_Click" Text="Buscar" ValidationGroup="buscar" />
                                                &nbsp;&nbsp;
                                                <asp:Label ID="lblCalculando" runat="server" Text="...." CssClass="letraMediana"></asp:Label>

                                                </td>
                                        </tr>
                                       <tr>
                                            <td class="auto-style4">
                                                &nbsp;</td>
                                            <td class="auto-style2"></td>
                                        </tr>
                                                                              
                                    </table>                                       
                            
    <br />
    <rsweb:ReportViewer ID="rpCalcula" runat="server" Height="409px" Width="982px">
    </rsweb:ReportViewer>
    <br />

    <style>
        .selectChico {
            min-width: 0px !important;
        }

        .auto-style2 {
            width: 312px;
        }

        .auto-style3 {
            height: 73px;
        }
        .auto-style4 {
            width: 276px;
        }

    </style>

    <br />
    <%--Modal para busqueda de contribuyente--%>
                        <asp:Panel ID="pnl" runat="server" class="formPanel">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td style="height: 60px;">
                                        <asp:Label ID="lblbuscarContribuyente" runat="server" Text="Buscar Contribuyente" CssClass="letraTitulo"></asp:Label></td>
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
                                                    <asp:DropDownList ID="ddlFiltroContribuyente" runat="server" />
                                                    &nbsp;<asp:TextBox ID="txtFiltroContribuyente" runat="server" CssClass="textGrande"></asp:TextBox>
                                                    &nbsp;&nbsp;&nbsp;                                      
                                        <asp:ImageButton ID="imbBuscarContribuyenteFiltro" runat="server" CausesValidation="false"
                                            ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscarContribuyenteFiltro_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <tr>
                                        <td>
                                            <div style="height: 200px; overflow: scroll;">
                                                <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False"
                                                    CssClass="grd" DataKeyNames="id,activo" AllowPaging="True"
                                                    AllowSorting="True" BorderStyle="None" ShowFooter="True" OnRowCommand="grd_RowCommand" OnSorting="grd_Sorting" OnPageIndexChanging="grd_PageIndexChanging">
                                                    <Columns>
                                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                                                        <asp:BoundField DataField="ApellidoPaterno" HeaderText="Apellido Paterno" SortExpression="ApellidoPaterno" />
                                                        <asp:BoundField DataField="ApellidoMaterno" HeaderText="Apellido Materno" SortExpression="ApellidoMaterno" />
                                                        <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="135px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgSeleccionar" runat="server" ToolTip="Seleccionar!"
                                                                    ImageUrl="~/img/Activar.png"
                                                                    CssClass="imgButtonGrid"
                                                                    CommandName="SeleccionarPersona"
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
                                                <asp:GridView ID="grdCon" runat="server" AutoGenerateColumns="False"
                                                    CssClass="grd" DataKeyNames="id,activo" AllowPaging="True"
                                                    AllowSorting="True" BorderStyle="None" ShowFooter="True" OnRowCommand="grdCon_RowCommand" OnSorting="grdCon_Sorting" OnPageIndexChanging="grdCon_PageIndexChanging">
                                                    <Columns>
                                                        <asp:BoundField DataField="Descripcion" HeaderText="Nombre" SortExpression="Descripcion" />                                                        
                                                        <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="135px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgSeleccionar" runat="server" ToolTip="Seleccionar!"
                                                                    ImageUrl="~/img/Activar.png"
                                                                    CssClass="imgButtonGrid"
                                                                    CommandName="SeleccionarPersona"
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
                                                <asp:GridView ID="grdCol" runat="server" AutoGenerateColumns="False"
                                                    CssClass="grd" DataKeyNames="id,activo" AllowPaging="True"
                                                    AllowSorting="True" BorderStyle="None" ShowFooter="True" OnRowCommand="grdCol_RowCommand" OnSorting="grdCol_Sorting" OnPageIndexChanging="grdCol_PageIndexChanging">
                                                    <Columns>
                                                        <asp:BoundField DataField="NombreColonia" HeaderText="Nombre" SortExpression="NombreColonia" />                                                        
                                                        <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="135px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgSeleccionar" runat="server" ToolTip="Seleccionar!"
                                                                    ImageUrl="~/img/Activar.png"
                                                                    CssClass="imgButtonGrid"
                                                                    CommandName="SeleccionarPersona"
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
                                            </div>
                                        </td>
                                    </tr>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" OnClick="btnCancel_Click" Text="Cancelar" /></td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <ajaxToolkit:ModalPopupExtender ID="pnl_Modal" runat="server" BackgroundCssClass="ventanaBackground"
                            PopupControlID="pnl" TargetControlID="btnPnl">
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:HiddenField ID="btnPnl" runat="server" />
    <br />
    <br />
    



</asp:Content>
