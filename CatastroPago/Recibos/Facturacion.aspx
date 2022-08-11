<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Facturacion.aspx.cs" Inherits="CatastroPago.Recibos.Facturacion" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function printPdf() {
            var PDF = document.getElementById("MainContent_frameRecibo");
            PDF.focus();
            PDF.contentWindow.print();
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div style="text-align: center">
                <asp:Image ID="ImagenLogo" runat="server" ImageUrl="~/Img/logo_yaute.jpg" Height="143px" Width="239px" />
            </div>
            <div id="barra" style="height: 12px; width: 100%; background-color: <%=this.colorDiv%>; text-align: center;"></div>
            <div>
                <div>
                    <h1>Facturación Electrónica de Predial y Catastro</h1>
                </div>
            </div>


            <br />
            <div id="divBusqueda" runat="server">
                <div class="row">
                    <div class="col-md-3">
                        <asp:Label ID="lblCodigo" runat="server" Text="Ingrese el codigo de seguridad de su recibo:" CssClass="letraMediana"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="txtCodigo" runat="server" MaxLength="40" CssClass="textExtraGrande"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnCodigo" runat="server" Text="Buscar" Width="181px" Height="40px" OnClick="btnCodigo_Click" />
                    </div>
                    <br />
                    <div class="col-md-1"></div>

                </div>
            </div>

            <hr />

            <asp:Panel ID="pnlRecibo" runat="server" Width="100%" Height="500px" BackColor="White" HorizontalAlign="Center" Visible="False">
                <table runat="server" id="tbFacturar" width="100%">
                    <tr>
                        <td class="style2" align="center">
                            <asp:Label ID="lblCodigo1" runat="server" CssClass="letraTitulo"
                                Text="Si desea facturar este recibo de clic en continuar."></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnSiguiente" runat="server" Text="Continuar" OnClick="btnSiguiente_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                         
                            <asp:Button ID="btnCancelarBusqueda" runat="server" Text="Cancelar" OnClick="btnCancelarBusqueda_Click" />
                        </td>
                    </tr>
                </table>
                <div class="width:100%;margin:1px;" runat="server" id="divCerrarFactura" visible="false">


                    <asp:Button ID="btnDescargaPDF" runat="server" Text="Descargar PDF" OnClick="btnDescargaPDF_Click" />
                    <asp:Button ID="btnDescargaXML" runat="server" Text="Descargar XML" OnClick="btnDescargaXML_Click" />

                    <asp:Button ID="btnCorreo" runat="server" OnClick="btnCorreo_Click" Text="Enviar por correo" />

                    <%--<asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Img/eliminar.png" ImageAlign="Right" OnClick="ImageButton1_Click" />--%>
                </div>
                <iframe id="frameRecibo" runat="server" src="" width="100%" height="90%" style="border: none;" />


            </asp:Panel>
            <%--    <ajaxToolkit:ModalPopupExtender ID="modalRecibo" runat="server" BackgroundCssClass="ventanaBackground"
        PopupControlID="pnlRecibo" TargetControlID="btnRecibo">
    </ajaxToolkit:ModalPopupExtender>
    <asp:HiddenField ID="btnRecibo" runat="server" />--%>

            <asp:Panel ID="pnlFactura" runat="server" class="formPanel">
                <table>
                    <tr>
                        <td style="width: 600px">
                            <asp:Label ID="Label1" runat="server" CssClass="textModalTitulo2" Text="Introduzca su RFC:"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 600px">
                            <asp:TextBox ID="txtRFCbuscar" MaxLength="13" runat="server" CssClass="textGrande"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ForeColor="OrangeRed" ControlToValidate="txtRFCbuscar" ValidationGroup="BuscarRFC"></asp:RequiredFieldValidator>
                            <br />
                            <asp:Label ID="lblValidaRFC" runat="server" Text="" ForeColor="OrangeRed"></asp:Label>
                            <br />
                            <asp:Label ID="lblMensaje" runat="server" Text="RFC no encontrado" ForeColor="OrangeRed" Visible="false"></asp:Label>
                            <br />
                            <asp:Button ID="btnBuscarRFC" runat="server" Text="Buscar" OnClick="btnBuscarRFC_Click" ValidationGroup="BuscarRFC" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnEditar" runat="server" Text="Editar" Visible="false" OnClick="btnEditar_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnRFCRegistro" runat="server" Text="Registrar RFC" Visible="false" OnClick="btnRFCRegistro_Click" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Button2" runat="server" Text="Cancelar" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 700px">
                            <table runat="server" id="InformacionRFC" visible="false">
                                <tr>
                                    <td style="width: 20px;"></td>
                                    <td colspan="5">
                                        <asp:Label ID="lblCodigo3" runat="server" CssClass="letraTitulo"
                                            Text="Información del RFC:"></asp:Label>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" CssClass="letraSubTitulo" Text="RFC:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRFC" runat="server" MaxLength="13" CssClass="textMediano" AutoPostBack="True"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" CssClass="letraSubTitulo" Text="Nombre:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNombre" runat="server" MaxLength="100" CssClass="textGrande"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rfv6" runat="server" ControlToValidate="txtNombre" ErrorMessage="*" ForeColor="OrangeRed" ValidationGroup="Registro">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <%-- <tr>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" CssClass="letraSubTitulo" Text="Calle:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCalle" runat="server" MaxLength="100" CssClass="textGrande"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rfv1" ControlToValidate="txtCalle" runat="server" ErrorMessage="*" ForeColor="OrangeRed" ValidationGroup="Registro">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" CssClass="letraSubTitulo" Text="Municipio:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMunicipio" runat="server" CssClass="textGrande" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rfv11" runat="server" ControlToValidate="txtMunicipio" ErrorMessage="*" ForeColor="OrangeRed" ValidationGroup="Registro">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" CssClass="letraSubTitulo" Text="Estado:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEstado" MaxLength="50" runat="server" CssClass="textGrande"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rfv2" runat="server" ControlToValidate="txtEstado" ErrorMessage="*" ForeColor="OrangeRed" ValidationGroup="Registro">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" CssClass="letraSubTitulo" Text="Pais:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPais" MaxLength="50" runat="server" CssClass="textGrande"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rfv12" runat="server" ControlToValidate="txtPais" ErrorMessage="*" ForeColor="OrangeRed" ValidationGroup="Registro">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" CssClass="letraSubTitulo" Text="CP:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCP" runat="server" MaxLength="5" CssClass="textMediano"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv3" ControlToValidate="txtCP" runat="server" ErrorMessage="*" ForeColor="OrangeRed" ValidationGroup="Registro">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator98" runat="server"
                                            ControlToValidate="txtCP" CssClass="mensajeValidador"
                                            ErrorMessage="Sólo números" ValidationExpression="\d+"
                                            ForeColor="OrangeRed"
                                            SetFocusOnError="True"
                                            ValidationGroup="Registro"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="Label10" runat="server" CssClass="letraSubTitulo" Text="Colonia:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtColonia" MaxLength="100" runat="server" CssClass="textGrande"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rfv13" runat="server" ControlToValidate="txtColonia" ErrorMessage="*" ForeColor="OrangeRed" ValidationGroup="Registro">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <asp:Label ID="Label14" runat="server" CssClass="letraSubTitulo" Text="No. Exterior:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNoExt" runat="server" CssClass="textMediano" MaxLength="10"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv10" runat="server" ControlToValidate="txtNoExt" ErrorMessage="*" ForeColor="OrangeRed" ValidationGroup="Registro">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="Label11" runat="server" CssClass="letraSubTitulo" Text="Localidad:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLocalidad" MaxLength="100" runat="server" CssClass="textGrande"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <asp:Label ID="Label13" runat="server" CssClass="letraSubTitulo" Text="No. Interior"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNoInt" runat="server" CssClass="textMediano" MaxLength="10"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="Label12" runat="server" CssClass="letraSubTitulo" Text="Referencia:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtReferencia" MaxLength="100" runat="server" CssClass="textGrande"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>--%>
                                <tr>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" CssClass="letraSubTitulo" Text="Correo Electronico:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtCorreoReg" MaxLength="100" runat="server" CssClass="textGrande"></asp:TextBox>
                                        <br />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtCorreoReg"
                                            CssClass="mensajeValidador" ErrorMessage="DIRECCIÓN DE CORREO INVALIDA." ForeColor="OrangeRed"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="Registro"></asp:RegularExpressionValidator>

                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="Label18" runat="server" CssClass="letraSubTitulo" Text="Uso CFDI:"></asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="ddlUsuCFDI" runat="server" Width="350px">
                                        </asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlUsuCFDI" ValidationGroup="factura" InitialValue="" CssClass="valida" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 20px;"></td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" Text="Guardar" ValidationGroup="Registro" Visible="false" />
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Button ID="btnGeneraFactura" runat="server" OnClick="btnGeneraFactura_Click" Text="Facturar" ValidationGroup="factura" />
                                        &nbsp;&nbsp;<asp:Button ID="btnCancelarTodo" runat="server" Text="Cancelar" OnClick="btnCancelarTodo_Click" />
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="modalFactura" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnlFactura" TargetControlID="btnFactura">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnFactura" runat="server" />

            <asp:Panel ID="pnlCorreo" runat="server" class="formPanel">
                <table>
                    <tr>
                        <td style="width: 543px">
                            <asp:Label ID="Label15" runat="server" CssClass="textModalTitulo2" Text="DIRECCIÓN DE CORREO."></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 543px">
                            <asp:TextBox ID="txtCorreoEnvio" MaxLength="100" runat="server" CssClass="textGrande"></asp:TextBox>
                            <br />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCorreoEnvio"
                                CssClass="mensajeValidador" ErrorMessage="DIRECCIÓN DE CORREO INVALIDA." ForeColor="OrangeRed"
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="correo"></asp:RegularExpressionValidator>

                        </td>
                    </tr>
                    <tr>
                        <td style="width: 543px">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 543px">
                            <asp:Button ID="btnEnvioCorreo" runat="server" Text="Enviar" ValidationGroup="correo" OnClick="btnEnvioCorreo_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Button3" runat="server" CausesValidation="False" Text="Cancelar" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="ModalEnvioCorreo" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnlCorreo" TargetControlID="btnPnlCorreo">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnPnlCorreo" runat="server" />


            <uc1:ModalPopupMensaje runat="server" ID="vtnModal" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDescargaPDF" />
            <asp:PostBackTrigger ControlID="btnDescargaXML" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
