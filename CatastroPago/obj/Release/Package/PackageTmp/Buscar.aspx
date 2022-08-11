<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Buscar.aspx.cs" Inherits="CatastroPago.Buscar" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container" text-align: "center">         
                 <br />
                 <div class="row" text-align: "center">   
                     <div class="col-xs-4 col-md-4">   
                        <asp:Image ID="ImagenLogo" runat="server" ImageUrl="~/Img/logoZapata.jpg" Height="102px" Width="318px" style="margin-right: 0px" />
                     </div>
                     <div class="col-xs-8 col-md-8">  
                         <div class="row"><br /></div>
                         <div class="row"><h2>Dirección de Predial y Catastro</h2></div>
                    </div>
                 </div>
               
                 <div id="barra" style="height:15px; width:100%; background-color: <%=this.colorDiv%>; text-align: center;"> 
                 </div>
                 <br /><br />
                 
                 <div class="row">
                      <div class="col-xs-6 col-md-4">      
                          <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Img/1a.jpg" Height="74px" PostBackUrl="~/Servicios/EstadoDeCuenta" Width="168px"/>
                          <br />
                      </div>
                      <div class="col-xs-6 col-md-4">
                          <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Img/2a.jpg" Height="74px" PostBackUrl="~/Servicios/EstadoDeCuenta" Width="168px"/>
                          <br />
                      </div>
                      <div class="col-xs-6 col-md-4">
                          <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Img/3a.jpg" Height="74px" PostBackUrl="~/Servicios/ImprimePlano" Width="168px"/>
                          <br />
                      </div>
                 </div> 
                <br /><br />
                <div  id="divBusqueda"  class="row">
                      <div class="col-xs-3 col-md-3"> 
                          <br />
                      </div>
                      <div class="col-xs-3 col-md-3">
					       <asp:Label ID="lblBPredial" runat="server" Text="Clave Predial:" CssClass="letraSubTitulo"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtClavePredial" runat="server"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender  runat="server" ID="meeFiltro" Mask="9999-99-999-999" MaskType="Number"  InputDirection="RightToLeft" TargetControlID="txtClavePredial" />
                            &nbsp;&nbsp
                            <br />
                            <asp:Label ID="Iniciales" runat="server" Text="Inciales del contribuyente:" CssClass="letraSubTitulo"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtIniciales" runat="server"></asp:TextBox>
                            <br />  
                            <br />  
                      </div>
                      <div class="col-xs-3 col-md-3">
                        <div class="row"  > <br /> </div>
                        <div class="row"  ><br /> <br /></div>
                        <div class="row"  ><br /> </div>
					    <asp:Button ID="Button3" runat="server" OnClick="btnInicio_Click" Text="Buscar" Width="181px" Height="40px" />
                         
                      </div>
                      <div class="col-xs-3 col-md-3">
                         <br />
                      </div>
                 </div> 
                 <br />
                 <br />
                 <div class="row">
                         <div class="col-lg-11">
                              <div style="font-size: medium" >  
                                Pagos con tarjeta <asp:Image ID="Image3" runat="server" ImageUrl="~/Img/visa-master.jpg" Height="43px" Width="115px" />
                                <br />  
                                Al realizar el pago en línea deberá esperar que se genere la factura oficial de pago del ayuntamiento<br />
                                <br />  
                                No cierre el Navegador
                                <br />                        
                              </div>
                           </div>
                 </div>
            
            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />
     </div>  
    <script type="text/javascript">
        function validateLength(oSrc, args) {
            args.IsValid = (args.Value.length <= 150);
        }
    </script>
</asp:Content>
