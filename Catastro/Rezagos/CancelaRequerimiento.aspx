<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CancelaRequerimiento.aspx.cs" Inherits="Catastro.CancelaRequerimiento" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <script type="text/javascript">
       
        function hideGenerar() {
            document.getElementById("divGenerando").className = "hidden";
        }
        function showGenerar() {
             document.getElementById("divGenerando").className = "";
        }
    </script>
    <style>
        .selectChico {
            min-width: 0px !important;
        }

        .auto-style1 {
            height: 28px;
        }

        .auto-style2 {
            width: 40px;
            height: 28px;
        }
        .hidden {
            top:-1000px;
        }
        
        .auto-style3 {
            height: 28px;
            background-color: #CCCCCC;
            text-align: left;
        }
        .auto-style4 {
            width: 40px;
            height: 28px;
            background-color: #CCCCCC;
        }
        
    </style>
    <br />
    <br />

    <ajaxToolkit:TabContainer ID="TabContainerRequerimiento" runat="server" ActiveTabIndex="0">
        <ajaxToolkit:TabPanel ID="TabFiltro" runat="server" HeaderText="Selección de Candidatos">
            <ContentTemplate>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>

                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td style="height: 30px;">
                                    <asp:Label ID="lblTitulo" runat="server" CssClass="textModalTitulo2" Text="Requerimientos"></asp:Label>

                                </td>
                            </tr>
                            <tr>
                                <td style="height: 2px">
                                    <asp:HiddenField ID="hdfId" runat="server" />
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 70%">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 250px">
                                                <asp:Label ID="txtAdeudo" runat="server" Text="Tengan un adeudo hasta el periodo:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList ID="ddlBimestre" runat="server" CssClass="selectChico" Width="110px">
                                                    <asp:ListItem Selected="True" Value="0">Bimestre</asp:ListItem>
                                                    <asp:ListItem>6</asp:ListItem>
                                                    <asp:ListItem>5</asp:ListItem>
                                                    <asp:ListItem>4</asp:ListItem>
                                                    <asp:ListItem>3</asp:ListItem>
                                                    <asp:ListItem>2</asp:ListItem>
                                                    <asp:ListItem>1</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" InitialValue="0" ControlToValidate="ddlBimestre" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                                                /
                                                <asp:DropDownList ID="ddlEjercicio" runat="server">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="valida" InitialValue="0" ControlToValidate="ddlEjercicio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 40px"></td>
                                            <td rowspan="8" style="vertical-align: top; width: 35%; text-align: right">
                                                <asp:Label ID="lblTotal" runat="server" CssClass="letraMediana" Text="Total de Registros:"></asp:Label>
                                                <asp:Label ID="lblTotal2" runat="server" CssClass="letraMediana" Text="0"></asp:Label>
                                                <asp:GridView ID="grdResultados" runat="server" AutoGenerateColumns="False" CssClass="grd" DataKeyNames="IdPredio,IdTipoPredio" OnPageIndexChanging="grdResultados_PageIndexChanging" OnRowDataBound="grdResultados_RowDataBound" AllowPaging="True" EnableViewState="true">
                                                    <Columns>
                                                        <asp:BoundField DataField="ClavePredial" HeaderText="Clave Catastral" />
                                                        <asp:BoundField DataField="IdTipoPredio" HeaderText="Tipo" />
                                                        <asp:BoundField DataField="Periodo" HeaderText="Periodo" />
                                                    </Columns>
                                                    <FooterStyle CssClass="grdFooter" />
                                                    <HeaderStyle CssClass="grdHead" />
                                                    <PagerStyle HorizontalAlign="Right" />
                                                    <RowStyle CssClass="grdRowPar" />
                                                </asp:GridView>

                                            </td>
                                        </tr>
                                        <tr>
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
                                                <asp:TextBox ID="txtRangoDe" runat="server" CssClass="textMediano" MaxLength="12" placeholder="Rango de"></asp:TextBox>
                                                -
                                                <asp:TextBox ID="txtRangoA" runat="server" CssClass="textMediano" MaxLength="12" placeholder="Rango a"></asp:TextBox>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td style="width: 40px"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblContribuyente" runat="server" Text="Contribuyente:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtContribuyente" runat="server" CssClass="textExtraGrande" MaxLength="200" placeholder="Contribuyente"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="False"
                                                    ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" />
                                                <asp:ImageButton ID="imgBorrarCon" runat="server" ImageUrl="~/Img/eliminar.png" OnClick="imgBorrarCon_Click" />                      
                                                <asp:HiddenField ID="hdfIdCon" runat="server" />
                                            </td>
                                            <td style="width: 40px"></td>

                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCondominio" runat="server" CssClass="letraMediana" Text="Condominio:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCondominio" runat="server" CssClass="textExtraGrande" MaxLength="200" placeholder="Condominio"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="imbCondominio" runat="server" CausesValidation="False" ImageUrl="~/img/consultar.fw.png" OnClick="imbCondominio_Click" />
                                                <asp:ImageButton ID="imbBorrarCondominio" runat="server" ImageUrl="~/Img/eliminar.png" OnClick="imbBorrarCondominio_Click" />
                                                <asp:HiddenField ID="hdfIdCondominio" runat="server" />
                                            </td>
                                            <td style="width: 40px">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblColonia" runat="server" CssClass="letraMediana" Text="Colonia:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtColonia" runat="server" CssClass="textExtraGrande" MaxLength="200" placeholder="Colonia"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="imbColonia" runat="server" CausesValidation="False" ImageUrl="~/img/consultar.fw.png" OnClick="imbColonia_Click" />
                                                <asp:ImageButton ID="imbBorrarColonia" runat="server" ImageUrl="~/Img/eliminar.png" OnClick="imbBorrarColonia_Click" />
                                                <asp:HiddenField ID="hdfIdColonia" runat="server" />
                                            </td>
                                            <td style="width: 40px">&nbsp;</td>
                                             <td style="width: 40px">&nbsp;</td>
                                               
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="text-align: left">
                                                <asp:Label ID="lblClaves" runat="server" Text="Claves:" CssClass="letraMediana"></asp:Label>
                                                <asp:Label ID="lblClaveExp" runat="server" Text="(Listado de claves separadas por coma, por ejemplo: 000005138001,000005138002,000005138003)" CssClass="letraChica"></asp:Label>
                                                <br />
                                                <asp:TextBox ID="txtClave" runat="server" CssClass="textExtraGrande" TextMode="MultiLine" Height="54px" Width="453px"></asp:TextBox>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btnBuscar1" runat="server" OnClick="btnBuscar_Click" Text="Buscar" ValidationGroup="buscar" />
                                            </td>
                                            <td style="width: 40px"></td>
                                             <td> 
                                                 <asp:Label ID="Label2" runat="server" Text="Fecha en que se va a requerir:" CssClass="letraMediana"></asp:Label>
                                                <asp:TextBox ID="txtFecha" runat="server" CssClass="textMediano" MaxLength="200" placeholder="Fecha"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" CssClass="valida" ControlToValidate="txtFecha" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="generar"></asp:RequiredFieldValidator>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="txtFecha_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFecha" Format="dd/MM/yyyy" />

                                             </td>
                                        </tr>
                                       <tr>
                                            <td class="auto-style1">&nbsp;</td>
                                            <td colspan="2" class="auto-style1">&nbsp;</td>
                                            <td class="auto-style2">&nbsp;</td>
                                           <td class="auto-style1">
                                                <%--<asp:Label ID="lblPerioo" runat="server" CssClass="letraMediana" Text="Periodo a requerir:"></asp:Label>
                                                <asp:DropDownList ID="ddlBRequerir" runat="server" CssClass="selectChico" Width="110px">
                                                    <asp:ListItem Selected="True" Value="0">Bimestre</asp:ListItem>
                                                    <asp:ListItem>1</asp:ListItem>
                                                    <asp:ListItem>2</asp:ListItem>
                                                    <asp:ListItem>3</asp:ListItem>
                                                    <asp:ListItem>4</asp:ListItem>
                                                    <asp:ListItem>5</asp:ListItem>
                                                    <asp:ListItem>6</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" CssClass="valida" InitialValue="0" ControlToValidate="ddlBRequerir" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="generar"></asp:RequiredFieldValidator>
                                                <asp:DropDownList ID="ddlERequerir" runat="server" CssClass="selectChico" Width="90px"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" CssClass="valida" InitialValue="0" ControlToValidate="ddlERequerir" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="generar"></asp:RequiredFieldValidator>--%>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">&nbsp;</td>
                                            <td class="auto-style3" colspan="2">
                                                <asp:RadioButton ID="rbReset" runat="server" Checked="True" Text="Reiniciar predios" style="text-align: left" GroupName="cancela" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:RadioButton ID="rbAnterior" runat="server" Text="Regresar fase anterior" GroupName="cancela" />
                                            </td>
                                            <td class="auto-style4">&nbsp;</td>
                                            <td class="auto-style3">
                                                <%--<asp:Label ID="lblPerioo" runat="server" CssClass="letraMediana" Text="Periodo a requerir:"></asp:Label>
                                                <asp:DropDownList ID="ddlBRequerir" runat="server" CssClass="selectChico" Width="110px">
                                                    <asp:ListItem Selected="True" Value="0">Bimestre</asp:ListItem>
                                                    <asp:ListItem>1</asp:ListItem>
                                                    <asp:ListItem>2</asp:ListItem>
                                                    <asp:ListItem>3</asp:ListItem>
                                                    <asp:ListItem>4</asp:ListItem>
                                                    <asp:ListItem>5</asp:ListItem>
                                                    <asp:ListItem>6</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" CssClass="valida" InitialValue="0" ControlToValidate="ddlBRequerir" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="generar"></asp:RequiredFieldValidator>
                                                <asp:DropDownList ID="ddlERequerir" runat="server" CssClass="selectChico" Width="90px"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" CssClass="valida" InitialValue="0" ControlToValidate="ddlERequerir" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="generar"></asp:RequiredFieldValidator>--%></td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td colspan="2">&nbsp;</td>
                                            <td style="width: 40px">&nbsp;</td>
                                           <td style="text-align: right">
                                                <asp:Button ID="btnGenerar" runat="server" Enabled="false" Text="Procesar" OnClick="btnGenerar_Click" ValidationGroup="generar" /></td>
                                        </tr>
                                      
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbltituloErrores" runat="server" Text="Errores de Generación" CssClass="letraTitulo"></asp:Label><br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%--<asp:Button ID="btnCancelar" runat="server" CausesValidation="False" OnClick="btnCancelar_Click" Text="Regresar" />
                        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" OnClick="btnCancel_Click" Text="Cancelar" Visible="False" />--%>
                                    <asp:GridView ID="grdError" runat="server" AllowPaging="True" AutoGenerateColumns="False" CssClass="grd" EnableViewState="true" OnPageIndexChanging="grdError_PageIndexChanging" Width="70%">
                                        <Columns>
                                            <asp:BoundField DataField="ClavePredial" HeaderText="Clave Catastral" />
                                            <asp:BoundField DataField="Error" HeaderText="Error" />
                                        </Columns>
                                        <FooterStyle CssClass="grdFooter" />
                                        <HeaderStyle CssClass="grdHead" />
                                        <PagerStyle HorizontalAlign="Right" />
                                        <RowStyle CssClass="grdRowPar" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
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
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
      
    </ajaxToolkit:TabContainer>

    


    <uc1:ModalPopupMensaje runat="server" ID="vtnModal" DysplayAceptar="True" DysplayCancelar="True" />
    <div id="divGenerando" style="width: 70%; height:100px; align-content:center; top: 40%; position: absolute; background-color:#FFFFFF; margin-left:10%" class="hidden">      
        <asp:Label ID="Label1" runat="server" Text="Generando archivo(s), por favor espere..." CssClass="letraTitulo" ForeColor="#000000"></asp:Label>
    </div>
</asp:Content>
