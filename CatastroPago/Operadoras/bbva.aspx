<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="bbva.aspx.cs" Inherits="CatastroPago.Operadoras.bbva" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <script type="text/javascript">

        function envia_formulario() {
            document.form1.submit();
        }       
           
	</script>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body onLoad="document.forms[0].submit()">
    
    <form id="form2"  action="https://www.adquiramexico.com.mx/clb/endpoint/tlaltizapan"  method="post" runat="server">
    <%--<form id="form2"  action="https://prepro.adquiracloud.mx/clb/endpoint/tlaltizapan"  method="post" runat="server">--%>
       
    <div style="text-align: center">
                       
        <asp:hiddenfield id="mp_account" runat="server"/>
        <asp:hiddenfield id="mp_product" runat="server" Value ="1"/>
        <asp:hiddenfield id="mp_order" runat="server"/>
        <asp:hiddenfield id="mp_reference" runat="server" /> 
        <asp:hiddenfield id="mp_node" runat="server"/>
        <asp:hiddenfield id="mp_concept" runat="server"/>
        <asp:hiddenfield id="mp_amount" runat="server" />
		<asp:hiddenfield id="mp_customername" runat="server"/>
        <asp:hiddenfield id="mp_currency" runat="server" />
        <asp:hiddenfield id="mp_signature" runat="server"  />   
        <asp:hiddenfield id="mp_urlsuccess" runat="server"  />   
        <asp:hiddenfield id="mp_urlfailure" runat="server"  />      
        <asp:hiddenfield id="hfId" runat="server"  />       

        <asp:Button ID="envio_info" runat="server" Text="Realizar Pago" 
        OnClientClick="envia_formulario();" Font-Bold="True" Width="174px" 
             Visible="False" />

    </div>
    </form>

</body>
</html>
