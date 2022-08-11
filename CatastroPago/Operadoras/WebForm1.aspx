<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="CatastroPago.Operadoras.WebForm1" %>

<!DOCTYPE html>




<html><head><title>..:: Payworks Hosted Secure ::..</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1"/>

    </head><body onload=""><div id="principal">
        <div id="encabezado">
            <img src="/img_bancybhosted/images/back-completo.gif" width="630" height="193"/>

        </div><script language="javascript">
                  function KeyCodeChk(d, c)
                  {
                      var b = event.keyCode; var a = d.value; var e = a.length;
                      if ((b < 48 || b > 57) && (b < 65 || b > 90) && (b < 96 || b > 122)) { if ((b != 13) && (b != 44) && (b != 45) && (b != 46) && (b != 47) && (b != 8) && (b != 16) && (b != 32)) { if ((b == 9) && (e > c)) { return } else { if ((b == 9) && (e == 0)) { return } else { if ((b == 9) && (e < c)) { alert("Este campo no puede ser menor a " + (c + 1) + " digitos") } } } window.event.returnValue = false } }
                  };</script><!-- JAVASCRIPT -->
        <script src="https://h.online-metrix.net/fp/check.js?org_id=k8vif92e&amp;session_id=banorteixe12-511406003071-24" type="text/javascript">

        </script>
        <form id="cybersourceTxBean" name="form" action="cybersource.do" method="post">
            <input id="MerchantID" name="MerchantID" value="banorteixe" type="hidden" value=""/>
            <input id="MERCHANT_ID" name="MERCHANT_ID" value="7940700" type="hidden" value=""/>
            <input id="PurchaseTotals_grandTotalAmount" name="PurchaseTotals_grandTotalAmount" value="1" type="hidden" value=""/>
            <input id="MerchantReferenceCode" name="MerchantReferenceCode" value="12-511406003071-24" type="hidden" value=""/>
            <input id="MerchantNumber" name="MerchantNumber" value="7940700" type="hidden" value=""/>
            <input id="Review" name="Review" type="hidden" value=""/><input id="TERMINAL_ID" name="TERMINAL_ID" value="79407001" type="hidden" value=""/>
            <input id="ENTRY_MODE" name="ENTRY_MODE" value="MANUAL" type="hidden" value=""/><table height="450" width="630" border="0"><tr><td><!-- Datos Generales --><table width="100%" border="0" align="center" cellspacing="0"><tr><td height="29" colspan="2">
                <p>Para realizar su pago con tarjeta de cr&eacute;dito Visa o MasterCard, proporcione la siguiente informaci&oacute;n:</p></td>

                                                                                                                                                                                                                                                                                                                                                                                  </tr>
                <tr>
                    <td align="left"><u></u><br><br></td></tr></table><!-- Datos Pago -->
                <table width="100%" border="0" cellspacing="0" align="center" id="tabla">
                    <tr>
                        <td colspan="2"><table width="100%" border="0" align="center" cellspacing="0">
                            <tr>
                                <td height="29">
                                    <div align="right">N&uacute;mero de tarjeta:

                                                </div>

                                </td>
                                <td height="29">
                                    <input id="Card_accountNumber" name="Card_accountNumber" onkeypress="javascript:KeyCodeChk(this);" type="text" value="" size="25" maxlength="19"/>

                                </td>
                                <td height="29">
                                    <div align="right">CVV2:

                                    </div>

                                </td>
                                <td height="29">
                                    <input id="Cvv2Val" name="Cvv2Val" onkeypress="javascript:KeyCodeChk(this);" type="text" value="" size="3" maxlength="16"/>
                                    
                                </td>

                            </tr>
                            <tr>
                                <td height="29">
                                    <div align="right">V&aacute;lida hasta:</div>

                                </td>
                                <td height="29">
                                    <select name="Card_expirationMonth" size="1">
                                        <option value="01">01</option>
                                        <option value="02">02</option>
                                        <option value="03">03</option>
                                        <option value="04">04</option>
                                        <option value="05">05</option>
                                        <option value="06">06</option>
                                        <option value="07">07</option>
                                        <option value="08">08</option>
                                        <option value="09">09</option>
                                        <option value="10">10</option>
                                        <option value="11">11</option>
                                        <option value="12">12</option>

                                    </select>&nbsp;/&nbsp; 
                                    <select name="Card_expirationYear" size="1">
                                        <option value="13">2013</option>
                                        <option value="14">2014</option>
                                        <option value="15">2015</option>
                                        <option value="16">2016</option>
                                        <option value="17">2017</option>
                                        <option value="18">2018</option>
                                        <option value="19">2019</option>
                                        <option value="20">2020</option>
                                        <option value="21">2021</option>
                                        <option value="22">2022</option>
                                        <option value="23">2023</option>
                                        <option value="24">2024</option>

                                    </select>MM/YYYY 

                                </td>
                                <td height="29">
                                    <div align="right">Tipo de tarjeta:</div>

                                </td>
                                <td height="29">
                                    <select name="Card_cardType" size="1">
                                        <option value="VISA" selected>Visa</option>
                                        <option value="MC">MasterCard</option>

                                    </select>

                                </td>

                            </tr>
                            <tr><td height="29">
                                <div align="right">Referencia:</div>

                                </td><td height="29">
                                    <input id="MerchantReferenceCode" name="MerchantReferenceCode" value="12-511406003071-24" disabled="disabled" type="text" value="" size="25" maxlength="19"/>

                                     </td>
                                <td height="29">
                                    <div align="right">Total:

                                    </div>

                                </td>
                                <td height="29">
                                    <input id="PurchaseTotals_grandTotalAmount" name="PurchaseTotals_grandTotalAmount" value="1" disabled="disabled" type="text" value="" size="10" maxlength="16"/>

                                </td>

                            </tr>
                            <tr>
                                <td height="29" colspan="4">&nbsp;</td>
                                <td height="29">Tel&eacute;fono:</td>
                                <td height="29">
                                    <input id="BillTo_phoneNumber" name="BillTo_phoneNumber" onkeypress="javascript:KeyCodeChk(this);" type="text" value="" size="25" maxlength="15"/>
                                </td>
                                <td height="29">&nbsp;
                                </td>
                                <td height="29">&nbsp;</td>
                            </tr>
                            <tr>
                                                                                                                                                                                                                                                                                                                                                                                          <td height="29">Email:</td><td height="29" colspan="3"><input id="BillTo_email" name="BillTo_email" onkeypress="javascript:KeyCodeChk(this);" type="text" value="" size="76" maxlength="60"/></td></tr><tr><td height="29" class="auto-style1">&nbsp;</td><td height="29">&nbsp;</td><td height="29"><div align="right"></div></td><td height="29">&nbsp;</td></tr><tr><td height="29"><div align="right"></div></td><td height="29" colspan="3">&nbsp;</td></tr></table></td></tr></table><table width="100%" border="0" cellspacing="0" align="center"><tr><td width="100%" valign="top" align="center"><input type="submit" id="botonContinuar" value="Pagar"></td></tr></table></table></form><p style="background:url(https://h.online-metrix.net/fp/clear.png?org_id=k8vif92e&amp;session_id=banorteixe12-511406003071-24&amp;m=1)"></p><img src="https://h.online-metrix.net/fp/clear.png?org_id=k8vif92e&amp;session_id=banorteixe12-511406003071-24&amp;m=2" alt=""><!-- FLASH CODE --><object type="application/x-shockwave-flash" data="https://h.online-metrix.net/fp/fp.swf?org_id=k8vif92e&amp;session_id=banorteixe12-511406003071-24" width="1" height="1" id="thm_fp"><param name="movie" value="https://h.online-metrix.net/fp/fp.swf?org_id=k8vif92e&amp;session_id=banorteixe12-511406003071-24"/><div></div></object><div id="pie"><img src="/img_bancybhosted/images/footerptdc.png" width="630" height="34"/></div></div></body></html>