<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PadronPredios.aspx.cs" Inherits="Catastro.Reportes.PadronPredios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="Label1" runat="server" Text="Padrón de predios" CssClass="letraTitulo"></asp:Label></td>
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
                                <td style="width: 602px">
                                    <asp:Label ID="Label2" runat="server" Text="Situación del predio:"></asp:Label>&nbsp;&nbsp;&nbsp;
                                    <asp:DropDownList ID="ddlStatus" runat="server" class="ddlMediano">
                                        <asp:ListItem Text="Todos" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Activos" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Baja" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Suspendido" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Activos y Suspendidos" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Periodo pagado:"></asp:Label>&nbsp;&nbsp;&nbsp;<asp:DropDownList ID="ddlAnio" runat="server" class="ddlMediano"/>
                                    &nbsp;&nbsp;-<asp:DropDownList ID="ddlBimestre" runat="server"  class="ddlMediano">
                                        <asp:ListItem Text="Todos" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="1 - Enero-Febrero" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2 - Marzo-Abril" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3 - Mayo-Junio" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4 - Julio-Agosto" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5 - Septiembre-Octubre" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6 - Noviembre-Diciembre" Value="6"></asp:ListItem>
                                    </asp:DropDownList></td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="Tipo de predio:"></asp:Label>&nbsp;&nbsp;&nbsp;<asp:DropDownList ID="ddlTipo" runat="server"  class="ddlMediano" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 74px; width: 602px">
                                    <asp:Label ID="lblContribuyente" runat="server" CssClass="letraMediana" Text="Contribuyente:"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <br />
                                    <asp:TextBox ID="txtContribuyente" runat="server" CssClass="textExtraGrande" MaxLength="200" placeholder="Contribuyente" Width="429px"></asp:TextBox>
                                    &nbsp;&nbsp;
                                    <br />
                                    <br />
                                    <br />
                                </td>
                                <td style="height: 74px">
                                    <asp:Label ID="lblCondominio" runat="server" CssClass="letraMediana" Text="Condominio:"></asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtCondominio" runat="server" CssClass="textExtraGrande" MaxLength="200" placeholder="Condominio" ReadOnly="true" Width="426px"></asp:TextBox>
                                    <br />
                                    <asp:ImageButton ID="imbCondominio" runat="server" CausesValidation="False" ImageUrl="~/img/consultar.fw.png" OnClick="imbCondominio_Click" />
                                    <asp:ImageButton ID="imbBorrarCondominio" runat="server" ImageUrl="~/Img/eliminar.png" OnClick="imbBorrarCondominio_Click" /><asp:HiddenField ID="hdfIdCondominio" runat="server" />
                                </td>
                                <td style="height: 74px">
                                    <asp:Label ID="lblColonia" runat="server" CssClass="letraMediana" Text="Colonia:"></asp:Label>
                                    <asp:TextBox ID="txtColonia" runat="server" CssClass="textExtraGrande" MaxLength="200" placeholder="Colonia" Width="315px"></asp:TextBox>
                                    <br />
                                    <br />
                                    <br />
                                    <br />
                                </td>
                           </tr>
                            <tr>
                                <td style="width: 602px">
                                    <table>
                                        <tr>
                                            <td rowspan="2">
                                                <asp:Label ID="Label11" runat="server" Text="Rangos de claves:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label6" runat="server" Text="Clave de Inicio:"></asp:Label>
                                            </td>
                                            <td style="width: 221px">
                                                <asp:TextBox ID="txtInicioClave" runat="server"></asp:TextBox>
                                                <asp:Label ID="lblInicio" runat="server" ForeColor="Orange" Text="*" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label7" runat="server" Text="Clave Final:"></asp:Label>
                                            </td>
                                            <td style="width: 221px">
                                                <asp:TextBox ID="txtFinClave" runat="server"></asp:TextBox>
                                                <asp:Label ID="lblFinal" runat="server" ForeColor="Orange" Text="*" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text="Buscar claves específicas"></asp:Label>
                                    &nbsp;<br />
                                    <asp:Label ID="lblClaveExp" runat="server" CssClass="letraChica" Text="(Listado de claves separadas por coma, por ejemplo: 000005138001,000005138002,000005138003)"></asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtClave" runat="server" CssClass="textExtraGrande" Height="54px" TextMode="MultiLine" Width="453px"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="False" Height="45px" ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" Width="46px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 602px">
                                </td>
                                <td>
                                    &nbsp;<asp:Label ID="Label13" runat="server" CssClass="letraChica" Text="Primero buscara entre el rango de claves y despues en las claves seleccionadas"></asp:Label>
                                </td>
                                <td>
                                    <%--<br />
                                    <asp:ImageButton ID="imbColonia" runat="server" CausesValidation="False" ImageUrl="~/img/consultar.fw.png" OnClick="imbColonia_Click" />
                                    <asp:ImageButton ID="imbBorrarColonia" runat="server" ImageUrl="~/Img/eliminar.png" OnClick="imbBorrarColonia_Click" />
                                    <asp:HiddenField ID="hdfIdColonia" runat="server" />--%>
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
                                    <input type="button" id="printreport" value="imprimir" onclick="printReport('<%=rpvtPredios.ClientID %>')" />
                                </div>
                            </div>
                        </asp:Panel>
                        <rsweb:reportviewer id="rpvtPredios" runat="server" height="500px" width="900px" showprintbutton="true"></rsweb:reportviewer>
                    </td>
                </tr>
            </table>




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




            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


