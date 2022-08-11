<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CancelacionRecibo.aspx.cs" Inherits="Catastro.Servicios.CancelacionRecibo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>
    <link href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/blitzer/jquery-ui.css"  rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            var fileName = "110038800001.pdf";
            $("#btnShow").click(function () {
                $("#dialog").dialog({
                    modal: true,
                    title: fileName,
                    width: 540,
                    height: 450,
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    open: function () {
                        var object = "<object data=\"{FileName}\" type=\"application/pdf\" width=\"500px\" height=\"300px\">";
                        object += "If you are unable to view file, you can download from <a href=\"{FileName}\">here</a>";
                        object += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
                        object += "</object>";
                        object = object.replace(/{FileName}/g, "/Temporales/" + fileName);
                        $("#dialog").html(object);
                    }
                });
            });
        });
        //Declarar este boton en la parte de abajo
        //<input id="btnShow" type="button" value="Show PDF" />
      </script>

    &nbsp;<div id="dialog" style="display: none">
    </div>

    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            
            <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />


            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="lblTitulo" runat="server" CssClass="textModalTitulo2" Text="Cancelación de recibo"></asp:Label>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 70%">
                        <table style="width: 100%">                           
                            <tr style="background-color: #b4b4b4">
                                <td  >
                                    <asp:Label ID="Label2" runat="server" Text="Folio Recibo:" CssClass="letraMediana"></asp:Label>                                    
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtFolio" runat="server" CssClass="textGrande" MaxLength="12" placeholder="Folio recibo" AutoPostBack="true"  ></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="txtFolio" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                     <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Ingresar solo números enteros" ValidationExpression="^\d+$" ControlToValidate="txtFolio" CssClass="valida" SetFocusOnError="True" ValidationGroup="guardar" Font-Size="Small"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1">
                                    <asp:Label ID="Label18" runat="server" CssClass="letraMediana" Text="Motivo Cancelación:"></asp:Label>
                                    </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtObservacion" runat="server" CssClass="textMultiExtraGrande" Height="84px" MaxLength="200" placeholder="Motivo de cancelación." TextMode="MultiLine" Width="599px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="3" style="text-align:center">
                                    <asp:Button ID="btnLimpiar" runat="server" CausesValidation="False" Text="Recargar" OnClick="btnLimpiar_Click" />
                                </td>
                                <td colspan="3" style="text-align:center">
                                    <asp:Button ID="btnGuardar" runat="server" CausesValidation="False" Text="Cancelar" OnClick="btnGuardar_Click" />
                                </td>
                            </tr>
                        </table>
                           
                            <tr>
                                <td colspan="6">
                                    <br />
                                </td>
                            </tr>
                            <tr style="background-color: #b4b4b4">
                                <td colspan="6">
                                    &nbsp;</td>
                            </tr>    
                    </td>
                </tr>
            </table>

            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />

            <asp:Literal ID="Literal1" runat="server"></asp:Literal>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
