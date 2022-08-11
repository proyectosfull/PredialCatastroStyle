<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="catPredios.aspx.cs" Inherits="Catastro.Catalogos.catPredios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 40px;"></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblTitulo" runat="server" Text="Alta Predio" CssClass="letraTitulo"></asp:Label>

                    </td>
                </tr>
                <tr>
                    <td style="height: 2px">
                        <hr />
                        <asp:HiddenField ID="hdfId" runat="server" />
                        <asp:ValidationSummary ID="ValidationSummary1" HeaderText="Existen campos que faltan completar." runat="server" ValidationGroup="guardar" DisplayMode="SingleParagraph" CssClass="valida" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
                            <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Datos Generales(1)">
                                <ContentTemplate>
                                    <table style="min-width: 50%;">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblClavePredial" runat="server" Text="Clave Predial:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtClavePredial" runat="server" CssClass="textMediano" placeholder="Clave Predial." MaxLength="12" Width="163px"></asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender runat="server" ID="meeFiltro" Mask="9999-99-999-999" TargetControlID="txtClavePredial" MaskType="Number" InputDirection="RightToLeft" BehaviorID="_content_meeFiltro" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" ControlToValidate="txtClavePredial" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblNumero0" runat="server" CssClass="letraMediana" Text="Recibo:" Visible="False"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRecibo" runat="server" Visible="False"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="revRecibo" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^\d+$" ControlToValidate="txtRecibo" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCalle" runat="server" Text="Calle:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCalle" runat="server" MaxLength="150" CssClass="textMultiExtraGrande" placeholder="Calle." TextMode="MultiLine"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" CssClass="valida" ControlToValidate="txtCalle" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblNumero" runat="server" Text="Número:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNumero" runat="server" CssClass="textGrande" placeholder="Número." MaxLength="50"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" CssClass="valida" ControlToValidate="txtNumero" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblColonia" runat="server" Text="Colonia:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtColonia" runat="server" CssClass="textGrande" placeholder="Escriba nombre de la colonia." autocomplete="off"></asp:TextBox>
                                                <ajaxToolkit:AutoCompleteExtender
                                                    runat="server"
                                                    BehaviorID="AutoCompleteEx1"
                                                    ID="autoComplete1"
                                                    TargetControlID="txtColonia"
                                                    ServicePath="../WS/AutoComplete.asmx"
                                                    ServiceMethod="GetCompletionList"
                                                    MinimumPrefixLength="5"
                                                    CompletionInterval="1000"
                                                    EnableCaching="true"
                                                    CompletionSetCount="20"
                                                    CompletionListCssClass="autocomplete_completionListElement"
                                                    CompletionListItemCssClass="autocomplete_listItem"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    DelimiterCharacters=""
                                                    ShowOnlyCurrentWordInCompletionListItem="True">
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
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="valida" ControlToValidate="txtColonia" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCP" runat="server" Text="CP:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCP" runat="server" CssClass="textGrande" placeholder="CP." MaxLength="5"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" CssClass="valida" ControlToValidate="txtCP" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                                <ajaxToolkit:MaskedEditExtender runat="server" ID="MaskedEditExtender2" Mask="99999" TargetControlID="txtCP" MaskType="Number" InputDirection="RightToLeft" />
                                                <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^\d+$" ControlToValidate="txtCP" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblLocalidad" runat="server" Text="Localidad:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLocalidad" runat="server" CssClass="textGrande" placeholder="Localidad." MaxLength="80"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" CssClass="valida" ControlToValidate="txtLocalidad" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCondominio" runat="server" CssClass="letraMediana" Text="Condominio:"></asp:Label>

                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCondominio" runat="server">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" CssClass="valida" ControlToValidate="ddlCondominio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar" InitialValue=""></asp:RequiredFieldValidator>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSuperTerrenoL" runat="server" CssClass="letraMediana" Text="Superficie del Terreno:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSuperTerreno" runat="server" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSuperficeConstruccionL" runat="server" Text="Superficie Construcción:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSuperficeConstruccion" runat="server" CssClass="letraMediana"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTerrenoComunL" runat="server" CssClass="letraMediana" Text="Terreno Común:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTerrenoComun" runat="server" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblConstruccionComunL" runat="server" Text="Construcción Común:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblConstruccionComun" runat="server" CssClass="letraMediana"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTerrenoPrivativoL" runat="server" CssClass="letraMediana" Text="Terreno Privativo:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTerrenoPrivativo" runat="server" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblConstruccionPrivativaL" runat="server" CssClass="letraMediana" Text="Construcción Privativa:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblConstruccionPrivativa" runat="server" CssClass="letraMediana"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblValorTerrenoL" runat="server" CssClass="letraMediana" Text="Valor Terreno:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblValorTerreno" runat="server" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblValorConstruccionL" runat="server" CssClass="letraMediana" Text="Valor Construcción:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblValorConstruccion" runat="server" CssClass="letraMediana"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFechaAlta" runat="server" CssClass="letraMediana" Text="Fecha Alta:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFechaAlta" runat="server" CssClass="textMediano" placeholder="Fecha Alta."></asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender ID="txtFechaAlta_MaskedEditExtender" runat="server" BehaviorID="txtFechaAlta_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaAlta" />
                                                <ajaxToolkit:CalendarExtender ID="txtFechaAlta_CalendarExtender" runat="server" BehaviorID="txtFechaAlta_CalendarExtender" CssClass="CalendarCSS" Format="dd/MM/yyyy" TargetControlID="txtFechaAlta" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtFechaAlta" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFechaEvaluoL" runat="server" CssClass="letraMediana" Text="Fecha Avaluo:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFechaEvaluo" runat="server" CssClass="letraMediana"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <asp:Label ID="lblFechaTraslado" runat="server" Text="Fecha Traslado:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFechaTraslado" runat="server" CssClass="textMediano" placeholder="Fecha Traslado."></asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender ID="txtFechaTraslado_MaskedEditExtender" runat="server" BehaviorID="txtFechaTraslado_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaTraslado" />
                                                <ajaxToolkit:CalendarExtender ID="txtFechaTraslado_CalendarExtender" runat="server" BehaviorID="txtFechaTraslado_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFechaTraslado" Format="dd/MM/yyyy" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" CssClass="valida" ControlToValidate="txtFechaTraslado" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblZona" runat="server" Text="Zona:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtZona" runat="server" CssClass="textGrande" MaxLength="4" placeholder="Zona."></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" CssClass="valida" ControlToValidate="txtZona" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator15" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^\d+$" ControlToValidate="txtZona" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMetrosFrente" runat="server" Text="Metros de Frente:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMetrosFrente" runat="server" CssClass="textMediano" placeholder="Metros de Frente." MaxLength="10"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" CssClass="valida" ControlToValidate="txtMetrosFrente" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator16" runat="server" ErrorMessage="Formato de número invalido, se permiten máximo ocho números enteros y dos decimales." ValidationExpression="\d{0,8}(\.\d{1,2})?|\.\d{1,2}" ControlToValidate="txtMetrosFrente" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Datos Generales(2)">
                                <ContentTemplate>
                                    <table style="min-width: 50%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblUsoSuelo" runat="server" Text="Uso de Suelo:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlUsoSuelo" runat="server"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" CssClass="valida" ControlToValidate="ddlUsoSuelo" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblExentoPago" runat="server" Text="Exento Pago:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlExentoPago" runat="server"></asp:DropDownList>
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" CssClass="valida" ControlToValidate="ddlExentoPago" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>                            
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblStatusPredio" runat="server" Text="Estado del Predio:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlStatusPredio" runat="server">
                                                    <asp:ListItem Text="Alta" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Baja" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Suspendido" Value="3"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" CssClass="valida" ControlToValidate="ddlStatusPredio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFechaBaja" runat="server" Text="Fecha Baja:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFechaBaja" runat="server" CssClass="textMediano" placeholder="Fecha Baja."></asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender ID="txtFechaBaja_MaskedEditExtender1" runat="server" BehaviorID="txtFechaBaja_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaBaja" />
                                                <ajaxToolkit:CalendarExtender ID="txtFechaBaja_CalendarExtender1" runat="server" BehaviorID="txtFechaBaja_CalendarExtender" CssClass="CalendarCSS" TargetControlID="txtFechaBaja" Format="dd/MM/yyyy" />
                                                <%--<asp:RequiredFieldValidator ID="txtFechaBaja_RequiredFieldValidator29" runat="server" CssClass="valida" ControlToValidate="txtFechaBaja" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTipoPredio" runat="server" Text="Tipo de Predio:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTipoPredio" runat="server"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" CssClass="valida" ControlToValidate="ddlTipoPredio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblNivel" runat="server" Text="Nivel:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNivel" runat="server" CssClass="textGrande" MaxLength="4" placeholder="Nivel."></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator32" runat="server" CssClass="valida" ControlToValidate="txtNivel" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>--%>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator18" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^\d+$" ControlToValidate="txtNivel" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblContribuyente" runat="server" Text="Contribuyente:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:HiddenField ID="hdfContribuyente" runat="server" />
                                                <asp:TextBox ID="txtConstribuyente" runat="server" CssClass="textGrande" MaxLength="250" placeholder="Escriba Nombre del contribuyente."  autocomplete="off"></asp:TextBox>
                                                <ajaxToolkit:AutoCompleteExtender
                                                    runat="server" 
                                                    BehaviorID="AutoCompleteEx"
                                                    ID="AutoCompleteExCon" 
                                                    TargetControlID="txtConstribuyente"
                                                    ServicePath="../WS/AutoComplete.asmx" 
                                                    ServiceMethod="GetCompletionListCon"
                                                    MinimumPrefixLength="5" 
                                                    CompletionInterval="1000"
                                                    EnableCaching="true"
                                                    CompletionSetCount="20"
                                                    CompletionListCssClass="autocomplete_completionListElement" 
                                                    CompletionListItemCssClass="autocomplete_listItem" 
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    DelimiterCharacters=""
                                                    ShowOnlyCurrentWordInCompletionListItem="true" >
                                                    <Animations>
                                                        <OnShow>
                                                            <Sequence>
                                                                
                                                                <OpacityAction Opacity="0" />
                                                                <HideAction Visible="true" />
                            
                                                                
                                                                <ScriptAction Script="
                                                                    // Cache the size and setup the initial size
                                                                    var behavior = $find('AutoCompleteEx');
                                                                    if (!behavior._height) {
                                                                        var target = behavior.get_completionList();
                                                                        behavior._height = target.offsetHeight - 2;
                                                                        target.style.height = '0px';
                                                                    }" />
                            
                                                                
                                                                <Parallel Duration=".4">
                                                                    <FadeIn />
                                                                    <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx')._height" />
                                                                </Parallel>
                                                            </Sequence>
                                                        </OnShow>
                                                        <OnHide>
                                                            
                                                            <Parallel Duration=".4">
                                                                <FadeOut />
                                                                <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx')._height" EndValue="0" />
                                                            </Parallel>
                                                        </OnHide></Animations>
                                                </ajaxToolkit:AutoCompleteExtender>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" CssClass="valida" ControlToValidate="txtConstribuyente" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                                <asp:ImageButton ID="imbAgregarContribuyente" runat="server" CausesValidation="False"
                                                    ImageUrl="~/img/agregar.fw.png" OnClick="imbAgregarContribuyente_Click" />
                                                <asp:ImageButton ID="imgBusContribuyente" runat="server" CausesValidation="False"
                                                    ImageUrl="~/Img/buscar.png" OnClick="imbBuscarContribuyente_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblUbicacionExpediente" runat="server" Text="Ubicación del expediente:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtUbicacionExpediente" runat="server" CssClass="textGrande" placeholder="Ubicación del expediente." MaxLength="50"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator33" runat="server" CssClass="valida" ControlToValidate="txtUbicacionExpediente" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTipoMovAvaluo" runat="server" Text="Tipo Movimiento Avaluo:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTipoMovAvaluo" runat="server">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" CssClass="valida" ControlToValidate="ddlTipoMovAvaluo" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblBimestreIP" runat="server" Text="Ult Bimestre IP:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlBimestreIP" runat="server" Enabled="false">
                                                    <asp:ListItem Text="1 - Enero-Febrero" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="2 - Marzo-Abril" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="3 - Mayo-Junio" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="4 - Julio-Agosto" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="5 - Septiembre-Octubre" Value="5"></asp:ListItem>
                                                    <asp:ListItem Text="6 - Noviembre-Diciembre" Value="6"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblAaFinalIP" runat="server" Text="Ult Ejercicio IP:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="txtAaFinalIP" runat="server" CssClass="textGrande" placeholder="Ult Ejercicio IP."></asp:Label>
                                                <asp:TextBox ID="txtAaFinalIPV" runat="server" CssClass="textGrande" placeholder="Ult Ejercicio IP" MaxLength="4" Enabled="false" Visible="false"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvAaFinalIPV" runat="server" CssClass="valida" ControlToValidate="txtAaFinalIPV" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar" Enabled="false" Visible="false"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="revAaFinalIPV" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^\d+$" ControlToValidate="txtAaFinalIPV" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small" Enabled="false" Visible="false"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblBimestreSm" runat="server" Text="Ult Bimestre SM:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlBimestreSm" runat="server" Enabled="false">
                                                    <asp:ListItem Text="1 - Enero-Febrero" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="2 - Marzo-Abril" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="3 - Mayo-Junio" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="4 - Julio-Agosto" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="5 - Septiembre-Octubre" Value="5"></asp:ListItem>
                                                    <asp:ListItem Text="6 - Noviembre-Diciembre" Value="6"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblAaFinalSm" runat="server" Text="Ult Ejercicio SM:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="txtAaFinalSm" runat="server" CssClass="textGrande" placeholder="Ult Ejercicio SM"></asp:Label>
                                                <asp:TextBox ID="txtAaFinalSmV" runat="server" CssClass="textGrande" placeholder="Ult Ejercicio SM" MaxLength="4" Enabled="false" Visible="false"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvAaFinalSmV" runat="server" CssClass="valida" ControlToValidate="txtAaFinalSmV" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar" Enabled="false" Visible="false"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="revAaFinalSmV" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^\d+$" ControlToValidate="txtAaFinalSmV" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small" Enabled="false" Visible="false"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblBaseGravableL" runat="server" Text="Base Gravable:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBaseGravable" runat="server" Text="" CssClass="letraMediana"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTipoFaseIPL" runat="server" Text="Tipo de Fase IP:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTipoFaseIP" runat="server" Text="" CssClass="letraMediana"></asp:Label>
                                                <asp:HiddenField ID="hdfTipoFaseIp" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTipoFaseSmL" runat="server" Text="Tipo de Fase Sm:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTipoFaseSm" runat="server" Text="" CssClass="letraMediana"></asp:Label>
                                                <asp:HiddenField ID="hdfTipoFaseSm" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4"></td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Contratos de agua">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblContrato" runat="server" Text="No Contrato:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtContrato" runat="server" CssClass="textMediano" placeholder="No Contrato." MaxLength="30"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="txtContrato" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="contrato"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnAgregarContrato" runat="server" Text="Agregar" OnClick="btnAgregarContrato_Click" ValidationGroup="contrato" ToolTip="Agregar Contrato" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:GridView ID="grdContratos" runat="server" AutoGenerateColumns="False" CssClass="grd"
                                                    BorderStyle="None" ShowFooter="True" Width="100%" OnRowCommand="grdContratos_RowCommand">
                                                    <Columns>
                                                        <asp:BoundField DataField="NoContrato" HeaderText="No Contrato" />
                                                        <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="80px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgDelete" runat="server" ToolTip="Eliminar!"
                                                                    ImageUrl="~/img/eliminar.png"
                                                                    CssClass="imgButtonGrid"
                                                                    CommandName="EliminarRegistro"
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
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="TabPanel4" runat="server" HeaderText="Observaciones">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblObservacion" runat="server" Text="Observación:" CssClass="letraMediana"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtObservacion" runat="server" CssClass="textMultiExtraGrande" placeholder="Observación." MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" CssClass="valida" ControlToValidate="txtObservacion" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="observacion"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnObservaciones" runat="server" Text="Agregar" OnClick="btnObservaciones_Click" ValidationGroup="observacion" ToolTip="Agregar observación" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:GridView ID="grdObservacions" runat="server" AutoGenerateColumns="False" CssClass="grd"
                                                    BorderStyle="None" ShowFooter="True" Width="100%" OnRowCommand="grdObservacions_RowCommand">
                                                    <Columns>
                                                        <asp:BoundField DataField="Observacion" HeaderText="Observación" />
                                                        <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="80px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgDelete" runat="server" ToolTip="Eliminar!"
                                                                    ImageUrl="~/img/eliminar.png"
                                                                    CssClass="imgButtonGrid"
                                                                    CommandName="EliminarRegistro"
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
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="TabPanel5" runat="server" HeaderText="Bases gravables">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="grdBG" runat="server" AutoGenerateColumns="False"
                                                    CssClass="grd" DataKeyNames="id,activo" AllowPaging="True"
                                                    AllowSorting="True" BorderStyle="None" ShowFooter="True" OnSorting="grdBG_Sorting" OnPageIndexChanging="grdBG_PageIndexChanging">
                                                    <Columns>
                                                        <asp:BoundField DataField="Ejercicio" HeaderText="Ejercicio" SortExpression="Ejercicio" />
                                                        <asp:BoundField DataField="Bimestre" HeaderText="Bimestre" SortExpression="Bimestre" />
                                                        <asp:BoundField DataField="Valor" HeaderText="Base Gravable" SortExpression="Valor" DataFormatString="{0:C}" />
                                                        <asp:BoundField DataField="FechaAvaluo" HeaderText="Fecha Avaluo" SortExpression="FechaAvaluo" DataFormatString="{0:d}" />        
                                                         <asp:BoundField DataField="ValorTerreno" HeaderText="Valor Terreno" SortExpression="ValorTerreno" DataFormatString="{0:C}" />                                               
                                                        <asp:BoundField DataField="ValorConstruccion" HeaderText="Valor Construccion" SortExpression="ValorConstruccion" DataFormatString="{0:C}" />
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
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="TabPanel6" runat="server" HeaderText="Historial">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Button ID="btnReasignarH" runat="server" Text="Reasignar Historial" OnClick="btnReasignarH_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="grdH" runat="server" AutoGenerateColumns="False" OnPageIndexChanging="grdH_PageIndexChanging"
                                                    CssClass="grd" AllowPaging="True" BorderStyle="None" ShowFooter="True">
                                                    <Columns>
                                                        <asp:BoundField DataField="ClavePredial" HeaderText="ClavePredial" />
                                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                                        <asp:BoundField DataField="TipoTramite" HeaderText="Tipo Tramite" />
                                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                                        <asp:BoundField DataField="Periodo" HeaderText="Periodo" />
                                                        <asp:BoundField DataField="Recibo" HeaderText="Recibo" />
                                                        <asp:BoundField DataField="FechaPago" HeaderText="FechaPago" />
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
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                        </ajaxToolkit:TabContainer>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="guardar" OnClick="btnGuardar_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnRegresar" runat="server" CausesValidation="False" Text="Regresar" OnClick="btnRegresar_Click" /></td>
                </tr>
            </table>
            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />

            <asp:Panel ID="pnl" runat="server" class="formPanel" ScrollBars="Vertical" Height="800px">
                <table cellpadding="0" cellspacing="0" >
                    <tr>
                        <td style="height: 60px;">
                            <asp:Label ID="Label1" runat="server" Text="Contribuyentes" CssClass="letraTitulo"></asp:Label></td>
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
                                   <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="False"
                                       ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" />
                                        <asp:Button ID="btnCancelarContribuyente" runat="server" OnClick="btnCancelarContribuyente_Click"
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
                            <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False"
                                CssClass="grd" DataKeyNames="id,activo" AllowPaging="True"
                                AllowSorting="True" BorderStyle="None" ShowFooter="True"
                                OnRowCommand="grd_RowCommand" OnSorting="grd_Sorting" OnPageIndexChanging="grd_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                                    <asp:BoundField DataField="ApellidoPaterno" HeaderText="ApellidoPaterno" SortExpression="ApellidoPaterno" />
                                    <asp:BoundField DataField="ApellidoMaterno" HeaderText="ApellidoMaterno" SortExpression="ApellidoMaterno" />
                                    <asp:BoundField DataField="Calle" HeaderText="Calle" SortExpression="Calle" />
                                    <asp:BoundField DataField="Numero" HeaderText="Numero" SortExpression="Numero" />
                                    <asp:BoundField DataField="Colonia" HeaderText="Colonia" SortExpression="Colonia" />
                                    <asp:BoundField DataField="Municipio" HeaderText="Municipio" SortExpression="Municipio" />
                                    <asp:BoundField DataField="Estado" HeaderText="Estado" SortExpression="Estado" />
                                    <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="135px">
                                        <ItemTemplate>
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
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="pnl_Modal" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnl" TargetControlID="btnPnl">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnPnl" runat="server" />

            <asp:Panel ID="pnlReasigna" runat="server" class="formPanel">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="height: 60px;">
                            <asp:Label ID="Label2" runat="server" Text="Si esta seguro de reasignar el historial del predio, </br>capture la nueva clave y guarde los cambios." CssClass="valida"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 2px">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 0px 10px 10px 10px;">
                            <asp:Label ID="Label3" runat="server" Text="Clave Predial:" CssClass="letraMediana"></asp:Label>
                            <asp:TextBox ID="txtPredioNuevo" runat="server" CssClass="textMediano"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender runat="server" ID="MaskedEditExtender1" Mask="9999-99-999-999" TargetControlID="txtPredioNuevo" MaskType="Number" InputDirection="LeftToRight" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="valida" ControlToValidate="txtPredioNuevo" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardarNuevoHistorial"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 0px 10px 10px 10px;">
                            <asp:Button ID="btnGuardarNuevoHistorial" runat="server" Text="Guardar" ValidationGroup="guardarNuevoHistorial" OnClick="btnGuardarNuevoHistorial_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="cancelar" runat="server" CausesValidation="False" Text="Cancelar" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="pnlReasigna_Modal" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnlReasigna" TargetControlID="btnPnlReasigna">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnPnlReasigna" runat="server" />
            <asp:Panel ID="pnlNuevoContribuyente" runat="server" class="formPanel">
                <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 40px;"></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Alta contribuyente" CssClass="letraTitulo"></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        <asp:RadioButtonList ID="rbltipoPersona" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rbltipoPersona_SelectedIndexChanged" RepeatDirection="Horizontal">
                            <asp:ListItem Selected="True">Física</asp:ListItem>
                            <asp:ListItem>Moral</asp:ListItem>
                        </asp:RadioButtonList>

                    </td>
                </tr>
                <tr>
                    <td style="height: 2px">
                        <hr />
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                    </td>
                </tr>

                <tr>
                    <td style="padding: 0px 10px 10px 10px;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblApellidoP" runat="server" Text="Apellido Paterno:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtApellidoP" runat="server" CssClass="textGrande" placeholder="Apellido Paterno." MaxLength="100"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvApellidoPaterno" runat="server" CssClass="valida" ControlToValidate="txtApellidoP" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardarCon"></asp:RequiredFieldValidator>
                                </td>                            
                                <td>
                                    <asp:Label ID="lblApellidoM" runat="server" Text="Apellido Materno:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtApellidoM" runat="server" CssClass="textGrande" placeholder="Apellido Paterno." MaxLength="100"></asp:TextBox>                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblNombre" runat="server" Text="Nombre o Razó Social:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNombre" runat="server" CssClass="textGrande" placeholder="Nombre." MaxLength="100"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" CssClass="valida" ControlToValidate="txtNombre" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardarCon"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Calle:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCalleN" runat="server" CssClass="textGrande" MaxLength="100" placeholder="Calle."></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" CssClass="valida" ControlToValidate="txtCalleN" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardarCon"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="Número:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNumeroN" runat="server" CssClass="textGrande" placeholder="Número." MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" CssClass="valida" ControlToValidate="txtNumeroN" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardarCon"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Text="Colonia:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtColoniaN" runat="server" CssClass="textGrande" placeholder="Colonia." MaxLength="100"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" CssClass="valida" ControlToValidate="txtColoniaN" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardarCon"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text="Localidad:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLocalidadN" runat="server" CssClass="textGrande" placeholder="Localidad." MaxLength="80"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" CssClass="valida" ControlToValidate="txtLocalidadN" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardarCon"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Label ID="lblMunicipio" runat="server" Text="Municipio:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMunicipio" runat="server" CssClass="textGrande" MaxLength="80" placeholder="Municipio."></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" CssClass="valida" ControlToValidate="txtMunicipio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardarCon"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblEstado" runat="server" Text="Estado:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEstado" runat="server" CssClass="textGrande" MaxLength="80" placeholder="Estado."></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" CssClass="valida" ControlToValidate="txtEstado" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardarCon"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text="Código Postal:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCPN" runat="server" CssClass="textGrande" placeholder="Código Postal." MaxLength="5"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" CssClass="valida" ControlToValidate="txtCPN" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardarCon"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^\d+$" ControlToValidate="txtCPN" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardarCon" Font-Size="Small"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblEmail" runat="server" Text="Email:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="textGrande" placeholder="Email." MaxLength="100"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Formato de correo incorrecto" ValidationExpression="[A-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Z0-9](?:[A-Z0-9-]*[A-Z0-9])?\.)+[A-Z0-9](?:[A-Z0-9-]*[A-Z0-9])?" ControlToValidate="txtEmail" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardarCon" Font-Size="Small"></asp:RegularExpressionValidator>
                                </td>
                                <td>
                                    <asp:Label ID="lbTelefono" runat="server" Text="Teléfono:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="textGrande" placeholder="Teléfono." MaxLength="10"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" CssClass="valida" ControlToValidate="txtTelefono" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardarCon"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^\d+$" ControlToValidate="txtTelefono" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardarCon" Font-Size="Small"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCurp" runat="server" Text="CURP:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCurp" runat="server" CssClass="textGrande" MaxLength="18" placeholder="CURP."></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rvfCurp" runat="server" CssClass="valida" ControlToValidate="txtCurp" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardarCon"></asp:RequiredFieldValidator>

                                </td>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Text="Adulto Mayor:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chbAdultoMayor" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Button ID="btnGuardarContribuyente" runat="server" Text="Guardar" ValidationGroup="guardarCon" OnClick="btnGuardarContribuyente_Click" />
                                    <asp:Button ID="btnCancelarContribuyenteN" runat="server" CausesValidation="False" Text="Regresar" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="mpeNuevoContribuyente" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnlNuevoContribuyente" TargetControlID="btnNuevoContribuyente" CancelControlID="btnCancelarContribuyenteN">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnNuevoContribuyente" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
