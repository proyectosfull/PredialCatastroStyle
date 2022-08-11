<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contrasenia.aspx.cs" Inherits="Catastro.Configuracion.Contrasenia" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container body-content">
        <div class="jumbotron">
            <h1>Cambio de Contraseña</h1>
        </div>
        <center>
           <table   style="width:350px;" class="formCaptura">
               <tr>
                   <td  style="padding:10px;" >
             <table style="width:100%" >  
                <tr>         
                    <td >
                        <asp:Label ID="Label1" runat="server" Text="Usuario "></asp:Label>
                        <br />
                          <asp:TextBox ID="txtUsuario" runat="server" 
                           CssClass="txtUsuario" placeholder="Usuario"
                            MaxLength="20" Height="31px" Width="196px" ReadOnly="True" 
                            ></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                            ControlToValidate="txtUsuario" CssClass="valida" ErrorMessage="*" 
                            ForeColor="#CC0000" ValidationGroup="val"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>                    
                    <td >
                        <asp:Label ID="Label2" runat="server" Text="Contraseña Antigua"></asp:Label>
                        <br/>
                        <asp:TextBox ID="txtContraseniaViejaU" runat="server" CssClass="txtContrasenia" TextMode="Password" placeholder="Contraseña antigua"
                            MaxLength="20" Height="31px" Width="196px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="txtContraseniaViejaU" CssClass="valida" ErrorMessage="*" 
                            ForeColor="#CC0000" ValidationGroup="val" ></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td >
                        <asp:Label ID="Label3" runat="server" Text="Contraseña Nueva"></asp:Label> 
                        <br />
                        <asp:TextBox ID="txtContraseniaNuevaU" runat="server" CssClass="txtContrasenia" placeholder="Contraseña Nueva"
                            MaxLength="20" TextMode="Password" Height="31px" Width="196px" ></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                            ControlToValidate="txtContraseniaNuevaU" CssClass="valida" ErrorMessage="*" 
                            ForeColor="#CC0000" ValidationGroup="val"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td >
                        <asp:Label ID="Label4" runat="server" Text="Repetir Contraseña Nueva"></asp:Label>
                         <br />
                        <asp:TextBox ID="txtContraseniaNuevaRep" runat="server" 
                            CssClass="txtContrasenia" placeholder="Repetir Contraseña Nueva"
                            MaxLength="20" TextMode="Password" Height="31px" Width="196px" 
                            AutoPostBack="false"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                            ControlToValidate="txtContraseniaNuevaRep" CssClass="valida" ErrorMessage="*" 
                            ForeColor="#CC0000" ValidationGroup="val"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">                        
                        <asp:CompareValidator ID="ValidadorClaves" runat="server" 
                            ErrorMessage="Las claves ingresadas no concuerdan, verifique la clave nueva y confírmela."
                            ControlToValidate="txtContraseniaNuevaU"
                            ControlToCompare="txtContraseniaNuevaRep"
                            Operator="Equal" CssClass="valida"
                            ></asp:CompareValidator>                       
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnCambiarContraseña" runat="server" Text=" Cambiar " 
                            CssClass="btnVer_Imagen" BackColor="#003A90" 
                            BorderColor="#003A90" ForeColor="White" Height="33px" Width="92px" 
                            onclick="btnCambiarContraseña_Click" ValidationGroup="val"/>                  
                    </td>
                </tr>              
            </table>
                   </td>
               </tr>
           </table>

       
                </center>
    </div>

    <uc1:ModalPopupMensaje runat="server" ID="mgs" />
</asp:Content>
