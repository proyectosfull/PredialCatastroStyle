<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="catConcepto.aspx.cs" Inherits="Catastro.Catalogos.catConcepto1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  
<script language="javascript" type="text/javascript">
    function SelectedProdServ(source, eventArgs)
    {        
        document.getElementById('<%=hdfProdServ.ClientID %>').value = eventArgs.get_value();      
    }
    function SelectedUnidadMedida(source, eventArgs) {
        document.getElementById('<%=hdfUnidadMedida.ClientID %>').value = eventArgs.get_value();              
    }
</script> 
     <table style="width: 100%; vertical-align: middle;">
        <tr>
            <td style="height: 60px">&nbsp; </td>
            <td colspan="5">
                <asp:Label ID="lbl_titulo" runat="server" CssClass="letraTitulo" Text="Alta o Edición del Concepto"></asp:Label>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="7" style="height: 10px">
                <hr />
            </td>
        </tr>
        <tr>
            <td style="width: 82px">
                <asp:Label ID="lblCri" runat="server" CssClass="textExtraGrande" Text="CRI:"></asp:Label>
            </td>
            <td colspan="5">
                <asp:TextBox ID="txtCri" runat="server" CssClass="textExtraGrande" MaxLength="50" placeholder="CRI" Width="206px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCri" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 82px">
                <asp:Label ID="lblNombre" runat="server" CssClass="textExtraGrande" Text="Nombre:"></asp:Label>
            </td>
            <td colspan="5">
                <asp:TextBox ID="txtNombre" runat="server" CssClass="textExtraGrande" MaxLength="500" placeholder="Nombre." Width="582px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNombre" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 82px">
                <asp:Label ID="lblDescripcion" runat="server" CssClass="textExtraGrande" Text="Descripcion:"></asp:Label>
            </td>
            <td colspan="5">
                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="textMultiExtraGrande" Height="103px" MaxLength="200" placeholder="Descripción." TextMode="MultiLine" Width="585px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDescripcion" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 82px">
                <asp:Label ID="lblGrupo0" runat="server" CssClass="textExtraGrande" Text=" Ejercicio:"></asp:Label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtEjercicio" runat="server" CssClass="textChico" MaxLength="50" placeholder="Ejercicio"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtEjercicio" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar" InitialValue="%"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revSalario0" runat="server" ControlToValidate="txtEjercicio" CssClass="valida" ErrorMessage="Ingresar  números para el año" Font-Size="Small" SetFocusOnError="True" ValidationExpression="[0-9]{4}" ValidationGroup="guardar"></asp:RegularExpressionValidator>
            </td>
            <td></td>
               <td>
                <asp:Label ID="Label1" runat="server" Text="Descuento:"></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="chkDescuento" runat="server" Text="SI" />
            </td>
            <td>&nbsp;</td>
        </tr>
<%--        <tr>
            <td style="width: 82px">
                <asp:Label ID="lblAgrava" runat="server" Text="Agrava"></asp:Label>
                :</td>
            <td colspan="2">
                <asp:CheckBox ID="chkAgrava" runat="server" OnCheckedChanged="chkAgrava_CheckedChanged" Text="SI"  onClick="ClickAgrava" AutoPostBack="True"/>
            </td>
            <td colspan="2">
                <asp:Label ID="lblAdicional" runat="server" Text="Adicional"></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="chkAdicional" runat="server" AutoPostBack="True" OnCheckedChanged="chkAdicional_CheckedChanged" Text="SI" />
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 82px">
                <asp:Label ID="lblEsAdicional" runat="server" Text="Es adicional:"></asp:Label>
            </td>
            <td>
                <asp:RadioButtonList ID="rblEsAdicional" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblEsAdicional_SelectedIndexChanged" RepeatDirection="Horizontal" Width="120px">
                    <asp:ListItem>SI</asp:ListItem>
                    <asp:ListItem Selected="True">NO</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td>
                <asp:TextBox ID="txtPorcentaje" runat="server" CssClass="textChico" Visible="False"></asp:TextBox>
                <asp:Label ID="lblPorcentaje" runat="server" Text="%" Visible="False"></asp:Label>
                <asp:RequiredFieldValidator ID="rfvPorcentaje" runat="server" ControlToValidate="txtPorcentaje" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revPorcentaje" runat="server" ControlToValidate="txtPorcentaje" CssClass="valida" ErrorMessage="Ingresar  números" Font-Size="Small" SetFocusOnError="True" ValidationExpression="^\d+$" ValidationGroup="guardar"></asp:RegularExpressionValidator>
            </td>
            <td colspan="2">
                <asp:Label ID="lblDescuento" runat="server" Text="Descuento:"></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="chkDescuento" runat="server" Text="SI" />
            </td>
            <td>&nbsp;&nbsp;</td>
        </tr>--%>
        <tr>
            <td>
                Tipo Tramite:
            </td>
            <td colspan="2">
                <asp:DropDownList ID="ddlTipoTramite" runat="server" AutoPostBack="True" CssClass="textGrande" OnSelectedIndexChanged="ddlTipoCobro_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlTipoCobro" CssClass="valida" ErrorMessage="*" InitialValue="0" ValidationGroup="guardar"></asp:RequiredFieldValidator>
            </td>
            <td >
                <asp:Label ID="Label2" runat="server" Text="Adicional:"></asp:Label>
            </td>
            <td colspan="2">
                <asp:CheckBox ID="cbAdicional" runat="server" Text="SI" />
            </td>
              <td>
            </td>
        </tr>
        <tr>
            <td colspan="3">
            </td>
            <td >
                <asp:Label ID="Label3" runat="server" Text="es adicional:"></asp:Label>
            </td>
            <td colspan="2">
                <asp:CheckBox ID="cbesAdicional" runat="server" Text="SI" />
            </td>
             <td>
            </td>
        </tr>
        <tr>
            <td style="width: 82px">Tipo Cobro:</td>
            <td colspan="2">
                <asp:DropDownList ID="ddlTipoCobro" runat="server" AutoPostBack="True" CssClass="textGrande" OnSelectedIndexChanged="ddlTipoCobro_SelectedIndexChanged">
                    <asp:ListItem Value="0">Seleccione una opcion</asp:ListItem>
                    <asp:ListItem Value="S">UMA</asp:ListItem>
                    <asp:ListItem Value="I">Importe</asp:ListItem>
                    <asp:ListItem Value="P">Porcentaje</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvTipoCobro" runat="server" ControlToValidate="ddlTipoCobro" CssClass="valida" ErrorMessage="*" InitialValue="0" ValidationGroup="guardar"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:Label ID="lblMesa" runat="server" CssClass="textExtraGrande" Text="Mesa:"></asp:Label>
            </td>
            <td colspan="2">
                <%--<asp:TextBox ID="txtEjercicio" runat="server" CssClass="textGrande" MaxLength="50" placeholder="Ejercicio"></asp:TextBox>--%>
                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtEjercicio" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtEjercicio" CssClass="valida" ErrorMessage="Ingresar solo numeros" Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d{4}" ValidationGroup="guardar"></asp:RegularExpressionValidator>--%>
                <asp:DropDownList ID="ddlIdMesa" runat="server" CssClass="textExtraGrande" Height="32px" Width="253px">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlIdMesa" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar" InitialValue="%"></asp:RequiredFieldValidator>
            </td>
            <td></td>
        </tr>
        <tr>
            <td style="width: 82px">
                <asp:Label ID="lblSalarioMin" runat="server" CssClass="textExtraGrande" Text="UMA mínimo:"></asp:Label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtSalarioMin" runat="server" CssClass="textChico" MaxLength="50" placeholder="Numero de UMA's"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvSalarioMin" runat="server" ControlToValidate="txtSalarioMin" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revSalarioMin" runat="server" ControlToValidate="txtSalarioMin" CssClass="valida" ErrorMessage="Ingresar número de UMA's" Font-Size="Small" SetFocusOnError="True" ValidationExpression="^\d+$" ValidationGroup="guardar"></asp:RegularExpressionValidator>
            </td>
            <td >
                <asp:Label ID="lblImporte" runat="server" CssClass="textExtraGrande" Text="Importe:"></asp:Label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtImporte" runat="server" CssClass="textChico" Enabled="False" MaxLength="50" placeholder="Importe"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvImporte" runat="server" ControlToValidate="txtImporte" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revImporte" runat="server" ControlToValidate="txtImporte" CssClass="valida" ErrorMessage="Ingresar números para el Importe" Font-Size="Small" SetFocusOnError="True" ValidationExpression="\d*\.?\d*" ValidationGroup="guardar"></asp:RegularExpressionValidator>
            </td>
            <td></td>
        </tr>
        <tr>
            <td style="width: 82px">
                <asp:Label ID="lblSalarioMax" runat="server" CssClass="textExtraGrande" Text="UMA Máximo:"></asp:Label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtSalarioMax" runat="server" CssClass="textChico" MaxLength="50" placeholder="Numero de UMA's"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvSalarioMax" runat="server" ControlToValidate="txtSalarioMax" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revSalarioMax" runat="server" ControlToValidate="txtSalarioMin" CssClass="valida" ErrorMessage="Ingresar número de UMA's" Font-Size="Small" SetFocusOnError="True" ValidationExpression="^\d+$" ValidationGroup="guardar"></asp:RegularExpressionValidator>
            </td>
            <td> </td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label5" runat="server" CssClass="textExtraGrande" Text="Producto / Servicio:"></asp:Label>
            </td>
            <td colspan="2">
                <asp:HiddenField ID="hdfProdServ" runat="server" />
                <asp:TextBox ID="txtxProdServ" runat="server" CssClass="textExtraGrande" MaxLength="50" placeholder="Producto/Servicio" Width="300px"></asp:TextBox>
                 <ajaxToolkit:AutoCompleteExtender
                                                    runat="server"
                                                    BehaviorID="AutoCompleteEx1"
                                                    ID="autoComplete1"
                                                    TargetControlID="txtxProdServ"
                                                    ServicePath="../WS/AutoComplete.asmx"
                                                    ServiceMethod="GetCompletionListProdServ"
                                                    MinimumPrefixLength="3"
                                                    CompletionInterval="1000"
                                                    EnableCaching="true"
                                                    CompletionSetCount="20"
                                                    CompletionListCssClass="autocomplete_completionListElement"
                                                    CompletionListItemCssClass="autocomplete_listItem"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    DelimiterCharacters=""
                                                    ShowOnlyCurrentWordInCompletionListItem="True" OnClientItemSelected="SelectedProdServ">
                                                    <Animations>
                                                        <OnShow>
                                                            <Sequence>                                                                
                                                                <OpacityAction Opacity="0" />
                                                                <HideAction Visible="true" />  
                                                                <ScriptAction Script="
                                                                    // Cache the size and setup the initial size
                                                                    var behavior = $find('AutoCompleteEx1');
                                                                    if (!behavior._height) {
                                                                        var target = behavior.get_completionList();
                                                                        behavior._height = target.offsetHeight - 2;
                                                                        target.style.height = '0px';
                                                                    }" />   
                                                                <Parallel Duration=".4">
                                                                    <FadeIn />
                                                                    <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx1')._height" />
                                                                </Parallel>
                                                            </Sequence>
                                                        </OnShow>
                                                        <OnHide>                                                            
                                                            <Parallel Duration=".4">
                                                                <FadeOut />
                                                                <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx1')._height" EndValue="0" />
                                                            </Parallel>
                                                        </OnHide></Animations>
                                                </ajaxToolkit:AutoCompleteExtender>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtxProdServ" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
            </td>
            <td> <asp:Label ID="Label6" runat="server" CssClass="textExtraGrande" Text="Unidad de Medida:"></asp:Label></td>           
            <td colspan="2">
                <asp:HiddenField ID="hdfUnidadMedida" runat="server" />
                <asp:TextBox ID="txtUnidadMedidia" runat="server" CssClass="textExtraGrande" MaxLength="50" placeholder="Unidad de Medida" Width="300px"></asp:TextBox>
                <ajaxToolkit:AutoCompleteExtender
                                                    runat="server"
                                                    BehaviorID="AutoCompleteEx2"
                                                    ID="autoComplete2"
                                                    TargetControlID="txtUnidadMedidia"
                                                    ServicePath="../WS/AutoComplete.asmx"
                                                    ServiceMethod="GetCompletionListUnidadMedida"
                                                    MinimumPrefixLength="3"
                                                    CompletionInterval="1000"
                                                    EnableCaching="true"
                                                    CompletionSetCount="20"
                                                    CompletionListCssClass="autocomplete_completionListElement"
                                                    CompletionListItemCssClass="autocomplete_listItem"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    DelimiterCharacters=""
                                                    ShowOnlyCurrentWordInCompletionListItem="True" OnClientItemSelected="SelectedUnidadMedida">
                                                    <Animations>
                                                        <OnShow>
                                                            <Sequence>                                                                
                                                                <OpacityAction Opacity="0" />
                                                                <HideAction Visible="true" />  
                                                                <ScriptAction Script="
                                                                    // Cache the size and setup the initial size
                                                                    var behavior = $find('AutoCompleteEx2');
                                                                    if (!behavior._height) {
                                                                        var target = behavior.get_completionList();
                                                                        behavior._height = target.offsetHeight - 2;
                                                                        target.style.height = '0px';
                                                                    }" />   
                                                                <Parallel Duration=".4">
                                                                    <FadeIn />
                                                                    <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx2')._height" />
                                                                </Parallel>
                                                            </Sequence>
                                                        </OnShow>
                                                        <OnHide>                                                            
                                                            <Parallel Duration=".4">
                                                                <FadeOut />
                                                                <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx2')._height" EndValue="0" />
                                                            </Parallel>
                                                        </OnHide></Animations>
                                                </ajaxToolkit:AutoCompleteExtender>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtUnidadMedidia" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
            </td>
            <td></td>
        </tr>
        <tr>
            <td>
                 <asp:Label ID="Label4" runat="server" CssClass="textExtraGrande" Text="Activo:"></asp:Label>
            </td>
            <td >
                <asp:CheckBox ID="cbxActivo" runat="server" Text="SI" />
            </td>
            <td  colspan="2">
               
            </td>
            <td colspan="2"></td>
            <td></td>
        </tr>
        <tr>
            <td colspan="2"></td>
            <td colspan="2"></td>
            <td colspan="2"></td>
            <td></td>
        </tr>
        <tr>
            <td style="width: 82px"></td>
            <td colspan="2">
                <asp:Button ID="btnCancelar" runat="server" CausesValidation="False" OnClick="btnCancelar_Click" Text="Cancelar" />
            </td>
            <td colspan="2">
                <asp:Button ID="btn_Guardar" runat="server" OnClick="btnGuardar_Click" Text="Guardar" ValidationGroup="guardar" />
            </td>
            <td></td>
            <td></td>
        </tr>
    </table>


    <uc1:ModalPopupMensaje runat="server" ID="vtnModal" DysplayAceptar="True" DysplayCancelar="False" />
    <asp:HiddenField ID="btnPnl" runat="server" />
</asp:Content>
