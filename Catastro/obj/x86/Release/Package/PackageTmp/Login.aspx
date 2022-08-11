<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Catastro.Login" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title><%: Page.Title %>- SidePred</title>

    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>
    <webopt:BundleReference runat="server" Path="~/Content/css" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <!-- CSS -->
    <link href="css/preload.css" rel="stylesheet">
    <link href="css/vendors.css" rel="stylesheet">
    <link href="css/syntaxhighlighter/shCore.css" rel="stylesheet">
    <link href="css/style-gray.css" rel="stylesheet" title="default">
    <link href="css/width-full.css" rel="stylesheet" title="default">
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" EnableScriptGlobalization="true">
            <Scripts>
                <%--To learn more about bundling scripts in ScriptManager see http://go.microsoft.com/fwlink/?LinkID=301884 --%>
                <%--Framework Scripts--%>
                <asp:ScriptReference Name="MsAjaxBundle" />
                <asp:ScriptReference Name="jquery" />
                <asp:ScriptReference Name="bootstrap" />
                <asp:ScriptReference Name="respond" />
                <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                <asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
                <asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
                <asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
                <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
                <asp:ScriptReference Name="WebFormsBundle" />
                <%--Site Scripts--%>
            </Scripts>
        </asp:ScriptManager>
        <div class="container-fluid">
            
            <div class="row">     
                <div class="row">
                     <div class="col-lg-12" style="height: 80px; text-align: center; margin-top: 15px;">
                        <img alt="" src="Img/login/Logo.png" height="70x" />
                    </div>
                     <div class="col-lg-12" style="text-align: center; height: 120px">
                        <h3 style="font-weight:bold;">Gestoría Digital de Predial y Catastro</h3>                   
                    </div>
                  <br />
            </div>
             <div class="col-lg-12" height: 120px"></div>

            </div>
            <div class="row">
                <div class="col-lg-12" style="display: flex; justify-content: center; align-items: center; height: 320px; background-image: url('img/login/fondoMapa.png'); background-repeat: no-repeat;">
                    <div style="display: flex; justify-content: center; align-items: center; width: 400px; background-image: url('img/login/Flogin.png'); background-repeat: no-repeat;">

                        <div class="form-group" style="width: 300px; text-align: center;">
                            <h3 style="color: #ffffff; font-weight: bold;">Iniciar sesión</h3>
                            <div class="input-group login-input">
                                <span class="input-group-addon"><i class="fa fa-user"></i></span>
                                <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" placeholder="Usuario"
                                    MaxLength="15"></asp:TextBox>
                               <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                        ControlToValidate="txtUsuario" CssClass="validador" ErrorMessage="*"
                                        ForeColor="#CC0000"></asp:RequiredFieldValidator>--%>
                            </div>
                            <br>
                            <div class="input-group login-input">
                                <span class="input-group-addon"><i class="fa fa-lock"></i></span>
                                <asp:TextBox ID="txtContrasenia" runat="server" CssClass="form-control" placeholder="Contraseña"
                                    MaxLength="20" TextMode="Password"></asp:TextBox>
                                 <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                        ControlToValidate="txtContrasenia" CssClass="validador" ErrorMessage="*"
                                        ForeColor="#CC0000"></asp:RequiredFieldValidator>--%>
                            </div>
                            <br />
                               <asp:Button ID="btnAceptar" runat="server" Text=" Entrar "
                                        CssClass="btn btn-ar btn-primary pull-right" OnClick="btnAceptar_Click"/>

                            <%--<button type="submit" class="btn btn-ar btn-primary pull-right">Login</button>--%>
                            <br />
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
            </div>
            <%--<div class="row">
                <div class="col-lg-12" style="text-align: center; height: 120px">
                    <h3 style="font-weight:bold;">Gestoria Digital de Predial y Catastro</h3>
                </div>
            </div>--%>

            <%--<div class="row">
                <div class="progress-bar" style="height:200px; width:100%;">
                </div>
            </div>--%>
        </div>
          <footer>
                <p>&copy; <%: DateTime.Now.Year %> - SidePred</p>
            </footer>

        <%-- <div class="jumbotron">
            <h1>Sistema de Predial y Catastro</h1>
            </div>--%>
        <%--<img src="img/banner.png" alt="banner" width="959px" />
            <br />           
        --%>
        <%--           <center>
            <table style="width: 500px; vertical-align: central;">
                <tr>
                    <td>&nbsp;</td>
                    <td></td>
                    <td style="background-image: url('img/Sesion.png'); background-repeat: no-repeat;" height="255" width="360">
                        <table style="padding: 10px; width: 89%; height: 300px;">
                            <tr>
                                <td class="style9" align="right" colspan="3">
                                    <em><b>Iniciar Sesión</b></em>
                                </td>
                            </tr>
                            <tr>
                                <td class="style8" align="center" colspan="3"></td>
                            </tr>
                            <tr>
                                <td rowspan="6" align="center" valign="middle" style="padding: 10px;"
                                    class="style10">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style5"></td>
                                <td class="style5"></td>
                            </tr>
                            <tr>
                                <td class="style5">&nbsp;<asp:TextBox
                                    ID="txtUsuario" runat="server" CssClass="txtUsuario" placeholder="Usuario"
                                    MaxLength="15" Height="31px" Width="196px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                        ControlToValidate="txtUsuario" CssClass="validador" ErrorMessage="*"
                                        ForeColor="#CC0000"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="style7">&nbsp;<asp:TextBox ID="txtContrasenia" runat="server" CssClass="txtContrasenia" placeholder="Contraseña"
                                    MaxLength="20" TextMode="Password" Height="31px" Width="196px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                        ControlToValidate="txtContrasenia" CssClass="validador" ErrorMessage="*"
                                        ForeColor="#CC0000"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <b>
                                        <asp:Label ID="lblMensaje" runat="server" CssClass="mensajeValidador"
                                            ForeColor="#8A0808">
                                        </asp:Label>
                                    </b>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center" class="style2">
                                    <asp:Button ID="btnAceptar" runat="server" Text=" Entrar "
                                        CssClass="btnVer_Imagen" OnClick="btnAceptar_Click" BackColor="#003A90"
                                        BorderColor="#003A90" ForeColor="White" Height="33px" Width="92px" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" class="style1" style="padding: 10px;">
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="mensajeValidador"
                                        DisplayMode="SingleParagraph"
                                        HeaderText="Existen campos requeridos que se encuentran vacíos. 
                                            favor de verificar."
                                        ForeColor="#CC0000" Height="41px"></asp:ValidationSummary>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td></td>
                    <td>&nbsp;</td>
                </tr>
            </table>
                </center>--%>

        <uc1:ModalPopupMensaje runat="server" ID="mgs" />
    </form>


    <script src="js/vendors.js"></script>
    <script src="js/syntaxhighlighter/shCore.js"></script>
    <script src="js/syntaxhighlighter/shBrushXml.js"></script>
    <script src="js/syntaxhighlighter/shBrushJScript.js"></script>
    <script src="js/DropdownHover.js"></script>
    <script src="js/app.js"></script>
    <script src="js/holder.js"></script>

</body>
</html>
