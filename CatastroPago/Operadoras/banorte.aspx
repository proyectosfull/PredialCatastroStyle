<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="banorte.aspx.cs" Inherits="CatastroPago.Operadoras.banorte" %>
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
    <form id="form1"  action="https://via.banorte.com/bancybhosted/index.do"  method="post" runat="server">
    <div style="text-align: center">
                       
        <asp:hiddenfield id="USER" runat="server"/>		
		<asp:hiddenfield id="PASSWORD" runat="server"/>
        <asp:hiddenfield id="MODE" runat="server" />  <%--AUTORIZACION ACEPTADA EN MODO PRUEBA--%>
        <asp:hiddenfield id="ENTRY_MODE" runat="server" Value ="MANUAL"/>
        <asp:hiddenfield id="Mr" runat="server" Value ="0"/>
        <asp:hiddenfield id="CMD_TRANS" runat="server" />
        <asp:hiddenfield id="RESPONSE_URL" runat="server"  />
        <asp:hiddenfield id="CONTROL_NUMBER" runat="server"/>
        <asp:hiddenfield id="TERMINAL_ID" runat="server" />
        <asp:hiddenfield id="AMOUNT" runat="server" />
        <asp:hiddenfield id="CUSTOMER_REF1" runat="server" />
        <asp:hiddenfield id="CUSTOMER_REF2" runat="server" />
        <asp:hiddenfield id="CUSTOMER_REF3" runat="server" />
        <asp:hiddenfield id="CUSTOMER_REF4" runat="server" />
        <asp:hiddenfield id="CUSTOMER_REF5" runat="server" />
        <asp:hiddenfield id="MerchantNumber" runat="server" />
        <asp:hiddenfield id="MerchantName" runat="server" />
        <asp:hiddenfield id="MerchantCity" runat="server" />
        <asp:hiddenfield id="NumberOfPayments" runat="server" />
        <asp:hiddenfield id="MERCHANT_ID" runat="server" />
        
        <asp:Button ID="envio_info" runat="server" Text="Realizar Pago" 
        OnClientClick="envia_formulario();" Font-Bold="True" Width="174px" 
             Visible="False" />

    </div>
    </form>

</body>
</html>
