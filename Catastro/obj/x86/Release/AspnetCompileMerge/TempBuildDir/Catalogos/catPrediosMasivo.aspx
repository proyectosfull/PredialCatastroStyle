<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="catPrediosMasivo.aspx.cs" Inherits="Catastro.Catalogos.catPrediosMasivo" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="UploadButton" />
        </Triggers>
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">           <tr>
                    <td style="height: 30px;" colspan="3"></td>
                </tr>
                <tr>
                    <td style="height: 60px;" colspan="3">
                        <asp:Label ID="Label1" runat="server" Text="Cargas Masivas" CssClass="letraTitulo"></asp:Label>
                    </td>
                </tr>               
                <tr>
                    <td>                        
                        <asp:RadioButtonList ID="rblTipo" runat="server" OnSelectedIndexChanged="rblTipo_SelectedIndexChanged" RepeatDirection="vertical" AutoPostBack="True">
                            <asp:ListItem>Predios</asp:ListItem>  
                             <asp:ListItem>Planos</asp:ListItem>  
                            <asp:ListItem>Condominios</asp:ListItem>  
                            <asp:ListItem>Colonias</asp:ListItem>  
                        </asp:RadioButtonList>
                  </td>
                </tr>
                 <tr>
                    <td style="height: 2px" colspan="3">
                        <hr />
                    </td>
                </tr>
                </table>

            <table runat="server" id="tbPredios" visible="false" cellpadding="0" cellspacing="0" width="100%">     
                <tr>
                    <td style="width: 200px">
                        <asp:Label ID="Label2" runat="server" Text="Clave Catastral:" CssClass="letraMediana"></asp:Label>
                    </td>
                    <td style="width: 300px">
                        <asp:TextBox ID="txtClavePredial" runat="server" CssClass="textMediano"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender runat="server" ID="meeFiltro" Mask="9999-99-999-999" TargetControlID="txtClavePredial" MaskType="Number" InputDirection="RightToLeft" />
                        &nbsp;&nbsp;&nbsp;                                   
                                    <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="False"
                                        ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" />
                    </td>
                    <td>
                        <asp:HyperLink ID="HyperLink2" runat="server" Target="_blank" NavigateUrl='~/Documentos/CargaMasivaPredios.xlsx'>
                            <asp:Label ID="Label4" runat="server" Text="Descargar Plantilla:" CssClass="letraMediana"></asp:Label>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Img/Excel.ico" Height="30px" />
                        </asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblPropietario" runat="server" CssClass="letraMediana" Text="Propietario:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPropietario" runat="server" CssClass="textMediano" MaxLength="100" placeholder="Propietario" Enabled="False" ReadOnly="True" Width="622px" Wrap="False"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblUbicacion" runat="server" CssClass="letraMediana" Text="Ubicación:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtUbicacion" runat="server" CssClass="textMediano" MaxLength="100" placeholder="Ubicación" Enabled="False" ReadOnly="True" Width="622px" Wrap="False"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <table style="background-color: lightgray; border: solid;">
                            <tr>
                                <td colspan="5">
                                    <h4>Ultimo periodo de pago de las claves catastrales</h4>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblBimestreIP" runat="server" Text="Ultimo Bimestre IP:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBimestreIP" runat="server">
                                        <asp:ListItem Text="1 - Enero-Febrero" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2 - Marzo-Abril" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3 - Mayo-Junio" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4 - Julio-Agosto" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5 - Septiembre-Octubre" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6 - Noviembre-Diciembre" Value="6"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblAaFinalIP" runat="server" Text="Ultimo Ejercicio IP:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="txtAaFinalIP" runat="server" CssClass="textGrande" placeholder="Ult Ejercicio IP."></asp:Label>
                                    <asp:TextBox ID="txtAaFinalIPV" runat="server" CssClass="textGrande" placeholder="Ultimo Ejercicio IP" MaxLength="4"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvAaFinalIPV" runat="server" CssClass="valida" ControlToValidate="txtAaFinalIPV" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar" Enabled="false" Visible="false"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revAaFinalIPV" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^\d+$" ControlToValidate="txtAaFinalIPV" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small" Enabled="false" Visible="false"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblBimestreSm" runat="server" Text="Ultimo Bimestre SM:" CssClass="letraMediana" Visible="False"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBimestreSm" runat="server" Visible="False">
                                        <asp:ListItem Text="1 - Enero-Febrero" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2 - Marzo-Abril" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3 - Mayo-Junio" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4 - Julio-Agosto" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5 - Septiembre-Octubre" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6 - Noviembre-Diciembre" Value="6"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblAaFinalSm" runat="server" Text="Ultimo Ejercicio SM:" CssClass="letraMediana" Visible="False"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="txtAaFinalSm" runat="server" CssClass="textGrande" placeholder="Ult Ejercicio SM" Visible="False"></asp:Label>
                                    <asp:TextBox ID="txtAaFinalSmV" runat="server" CssClass="textGrande" placeholder="Ult Ejercicio SM" MaxLength="4" Visible="False"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvAaFinalSmV" runat="server" CssClass="valida" ControlToValidate="txtAaFinalSmV" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar" Enabled="false" Visible="false"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revAaFinalSmV" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^\d+$" ControlToValidate="txtAaFinalSmV" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small" Enabled="false" Visible="false"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server" id="trcarga">
                    <td>
                        <asp:Label ID="Label3" runat="server" CssClass="letraMediana" Text="Seleccionar archivo de excel:"></asp:Label>
                    </td>
                    <td>
                        <asp:FileUpload ID="fuExcel" runat="server" />
                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.xls|.xlsx)$"
                            ControlToValidate="fuExcel" runat="server" CssClass="valida" ErrorMessage="Por favor seleccione un archivo de excel."
                            ValidationGroup="carga" Display="Dynamic" />--%>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic"
                            ControlToValidate="fuExcel" ErrorMessage="Por favor seleccione un archivo de excel." CssClass="valida" ValidationGroup="carga"/>--%>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="UploadButton" Text="Cargar" OnClick="UploadButton_Click" CausesValidation="True" ValidationGroup="carga" />
                    </td>
                </tr>
                <tr runat="server" id="trgrid">
                    <td colspan="3" style="padding: 0px 10px 10px 10px;">
                        <asp:Button runat="server" ID="btnGuardar" Text="Guardar" ValidationGroup="guardar" OnClick="btnGuardar_Click" />
                        <asp:Button ID="btnTramitePlanos" runat="server"  Text="Tramites de planos" OnClick="TramitePlanos_Click" />
                        <br />
                        <asp:Panel ID="Panel1" runat="server" Width="1100" ScrollBars="Auto" Height="400px">
                            <asp:GridView ID="grdMasivo" runat="server" CssClass="grd" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="ClaveCatastral" HeaderText="Clave Catastral" />
                                </Columns>
                                <Columns>
                                    <asp:BoundField DataField="Calle" HeaderText="Calle" />
                                </Columns>
                                <Columns>
                                    <asp:BoundField DataField="Numero" HeaderText="Numero" />
                                </Columns>
                                <Columns>
                                    <asp:BoundField DataField="SuperficieTerreno" HeaderText="Superficie Terreno" />
                                </Columns>
                                <Columns>
                                    <asp:BoundField DataField="TerrenoPrivativo" HeaderText="Terreno Privativo" />
                                </Columns>
                                <Columns>
                                    <asp:BoundField DataField="TerrenoComun" HeaderText="Terreno Comun" />
                                </Columns>
                                <Columns>
                                    <asp:BoundField DataField="ValorTerreno" HeaderText="Valor Terreno" />
                                </Columns>
                                <Columns>
                                    <asp:BoundField DataField="SuperficieConstruccion" HeaderText="Superficie Construccion" />
                                </Columns>
                                <Columns>
                                    <asp:BoundField DataField="ConstruccionPrivativa" HeaderText="ConstruccionPrivativa" />
                                </Columns>
                                <Columns>
                                    <asp:BoundField DataField="ConstruccionComun" HeaderText="Construccion Comun" />
                                </Columns>
                                <Columns>
                                    <asp:BoundField DataField="ValorConstruccion" HeaderText="Valor Construccion" />
                                </Columns>
                                <%-- <Columns>
                                    <asp:BoundField DataField="ValorConstruccionComun" HeaderText="Valor Construccion Comun" />
                                </Columns>
                                <Columns>
                                    <asp:BoundField DataField="ValorConstruccionPrivativa" HeaderText="Valor Construccion Privativa" />
                                </Columns>--%>
                                <Columns>
                                    <asp:BoundField DataField="MetrosFrente" HeaderText="Metros Frente" />
                                </Columns>
                                <Columns>
                                    <asp:BoundField DataField="ReciboAlta" HeaderText="ReciboAlta" />
                                </Columns>
                                <Columns>
                                    <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" />
                                </Columns>
                                <Columns>
                                    <asp:BoundField DataField="Referencias" HeaderText="Referencias" />
                                </Columns>
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblEmptySearch" runat="server" CssClass="letraTituloEmptyGrid">No se encontraron resultados</asp:Label>
                                </EmptyDataTemplate>
                                <FooterStyle CssClass="grdFooter" />
                                <HeaderStyle CssClass="grdHead" />
                                <RowStyle CssClass="grdRowPar" />
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>

            <table runat="server" id="tbCondominios" visible="false" cellpadding="0" cellspacing="0" width="100%">     
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlCondominio" runat="server"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="valida" ControlToValidate="ddlCondominio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardarCondominio"  InitialValue="" Visible="false"></asp:RequiredFieldValidator>
                         <br />
                        <asp:TextBox ID="txtMasivoCondominio" runat="server" CssClass="textExtraGrande" TextMode="MultiLine" Height="54px" Width="453px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" ControlToValidate="txtMasivoCondominio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardarCondominio"  Visible="false"></asp:RequiredFieldValidator>
                        <br />
                        <asp:Button ID="btnGuardarCondominio" runat="server" Text="Guardar" ValidationGroup="guardarCondominio" OnClick="btnGuardarCondominio_Click" />
                    </td>
                    </tr>
                </table>

             <table runat="server" id="tbColonias" visible="false" cellpadding="0" cellspacing="0" width="100%">     
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlColonias" runat="server"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="valida" ControlToValidate="ddlColonias" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardarColonia" InitialValue="" Visible="false"></asp:RequiredFieldValidator>
                        <br />
                        <asp:TextBox ID="txtMasivocolonias" runat="server" CssClass="textExtraGrande" TextMode="MultiLine" Height="54px" Width="453px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="txtMasivocolonias" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardarColonia"  Visible="false"></asp:RequiredFieldValidator>
                        <br />
                        <asp:Button ID="btnGuardarColonias" runat="server" Text="Guardar" ValidationGroup="guardarColonia" OnClick="btnGuardarColonias_Click" />
                    </td>
                    </tr>
                </table>

            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />
        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>
