<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Catastro._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<div class="col-lg-12"><br /><br /><br /><br /><br /></div>
<div class="row">
  <div class="col-xs-6 col-md-3">      
      <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Img/login/pago.jpg" Height="170px" PostBackUrl="~/Servicios/EstadoDeCuenta"/>
      <asp:LinkButton ID="LinkButton1" runat="server" CssClass="letraTitulo" PostBackUrl="~/Servicios/EstadoDeCuenta">Cobro</asp:LinkButton> <br />
  </div>
  <div class="col-xs-6 col-md-3">
      <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Img/login/otrosCobros.png" Height="170px" PostBackUrl="~/Servicios/EstadoDeCuenta"/>
      <asp:LinkButton ID="LinkButton2" runat="server" CssClass="letraTitulo" PostBackUrl="~/Servicios/Serivios/Cobros">Otros Cobros</asp:LinkButton> <br />
  </div>
  <div class="col-xs-6 col-md-3">
      <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Img/login/mapa.png" Height="170px" PostBackUrl="~/Servicios/ImprimePlano"/>
      <asp:LinkButton ID="LinkButton3" runat="server" CssClass="letraTitulo" PostBackUrl="~/Servicios/ImprimePlano">Plano Certificado</asp:LinkButton> <br />
  </div>
   <div class="col-xs-6 col-md-3">      
      <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/Img/login/constancia.png" Height="170px" PostBackUrl="~/Servicios/Certificado"/>
       <asp:LinkButton ID="LinkButton4" runat="server" CssClass="letraTitulo" PostBackUrl="~/Servicios/Certificado">Constancia</asp:LinkButton> <br />
  </div>
</div>

<div class="col-lg-12" style="display: flex;z-index:0 !important; justify-content: center; align-items: center; height: 300px; background-image: url('img/login/fondoMapa2.png'); background-repeat: no-repeat;">
    </div>
<br />

</asp:Content>
