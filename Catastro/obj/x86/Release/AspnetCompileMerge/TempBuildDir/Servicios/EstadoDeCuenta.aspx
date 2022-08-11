<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="EstadoDeCuenta.aspx.cs" Inherits="Catastro.Servicios.EstadoDeCuenta" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function printPdf() {
            var PDF = document.getElementById("MainContent_frameRecibo");
            PDF.focus();
            PDF.contentWindow.print();
        }
        function printPdfEdo() {
            var PDF = document.getElementById("MainContent_frameEstado");
            PDF.focus();
            PDF.contentWindow.print();
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div runat="server" id="divEncabezado">
                <table>
                    <%--<cellpadding="0" cellspacing="0" width="100%">--%>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblClavePredial" runat="server" CssClass="letraTituloEmptyGrid" Text="Clave Catastral:"></asp:Label>
                        </td>
                        <td>
                           <%-- asp:TextBox ID="txtClavePredial" runat="server" CssClass="textMediano" MaxLength="12" placeholder="Clave Predial" ValidationGroup="BuscaPredio" ></asp:TextBox>--%>
                            <asp:TextBox ID="txtKeyDev" runat="server" CssClass="textMediano" MaxLength="12" placeholder="Clave Predial" ValidationGroup="BuscaPredio" Visible="False" >-----BEGIN PRIVATE KEY-----
MIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQCN0peKpgfOL75i
YRv1fqq+oVYsLPVUR/GibYmGKc9InHFy5lYF6OTYjnIIvmkOdRobbGlCUxORX/tL
sl8Ya9gm6Yo7hHnODRBIDup3GISFzB/96R9K/MzYQOcscMIoBDARaycnLvy7FlMv
O7/rlVnsSARxZRO8Kz8Zkksj2zpeYpjZIya/369+oGqQk1cTRkHo59JvJ4Tfbk/3
iIyf4H/Ini9nBe9cYWo0MnKob7DDt/vsdi5tA8mMtA953LapNyCZIDCRQQlUGNgD
qY9/8F5mUvVgkcczsIgGdvf9vMQPSf3jjCiKj7j6ucxl1+FwJWmbvgNmiaUR/0q4
m2rm78lFAgMBAAECggEAbYDO9YTgvfjrPTbRyam12F7mFFHaUusBzXJaHzclD2GL
zzW98e4y1GqX7dxnbXxJXidE1qsijrrXY0kkV8zdJp5n1zCgg9JeYeTycGaD3HMR
uJFJUjMDT249kHi30QH6w1hC8OQ8y4+fRvcRZqr4tZGdrJhotn+Fxw7H6bWZycmc
izbv4Q+e5+tQpWeIKC5u6tChTdURdIULMGTbeuFK7bS9Q6KQu65TiBy9Z+d9Sg7B
2FvaIGGOAxuyNNOaDf4ZC4+1uUJJMqOTXUVhfYwkcQMV/BXNK6uZuoBkL2uOvs9t
+ULwYyY99rUsJvsgbmz0Agzi/0V5rRLJk/7+kz2bOQKBgQDKMs9TuJ6qXL36aB5v
Z+ZaHuliA7j0q4Uiqoec/4tpddgr9O8InxfMnc/EGDBUw5P3iddIM2PM0vOnSGXw
5ZYEfS03R+KrU4TLJIzcyWOLqjwqkZxTWTiMWRcwrWtVBxbxyQubMytAXdKmFCLh
SISktfVltrhhDh80crw0ccdoQwKBgQCzjyY57iIRlxUOdqD4ynRmOC7iZgl554a1
VRkUIk57IsOxBWae3mYkiAbCvGDijeqWO61oBRfN/xphnR9RECwtzCdCDTdMcUp4
i33hHD/vafZRZP4BlNTcZC27B/6ixkdR9LddMtpAT4DUkELhcsn32P/3mzG3rFnL
ljJ+F9jT1wKBgBurKEPEl7GoTzbc2I1WImdio30OFVklv2om+7e4IFOmFJavRaZg
XtlZHv0uci6nNLBC5Hq0zYtRspXJimmUgRrMJkvSQmo/W4SQ09XCmSSbfvA0TLf7
FYnfBxVaJb3U4objg/sQ3XJJZHHlf4BkdAI2BAaPIlvlms+Kg8aJa0gRAoGAT/QH
83ej1+1MRPpxxxZvKi0OQ2VoBs4fX5Ma7aoxBAeA18wt28Pv+4hOalvzUC4dLPQ5
zL2n0eQr3RdXoILxCRuEx5aW7wTrQi3qyVgI6BRox+mOaSnadqBs9IEk01oy2716
AJfqMwSzuvLZtQWmBSStJZYHV1/5Q/wHU7pOpFUCgYBVCgJQ9WboLqHpXl2A++wk
zEnwSM4KDCRc25wAdZykFXf8uXEuIIZG4QsH56ljGrAoulJAGGV1qwqaYoHQzowV
PDFfDEKYKLzT4MF0/kDsYgrnZka2HreLba0Ujwx4MjMDkeoAjbg/uW2jOgRgAHsY
h0lBer2hP8NFqBPBBTNDwQ==
-----END PRIVATE KEY-----
</asp:TextBox>
                           
                            <asp:TextBox ID="txtCerDev" runat="server" CssClass="textMediano" MaxLength="12" placeholder="Clave Predial" ValidationGroup="BuscaPredio" Visible="False" >-----BEGIN CERTIFICATE-----
MIIFuzCCA6OgAwIBAgIUMzAwMDEwMDAwMDA0MDAwMDI0MzQwDQYJKoZIhvcNAQEL
BQAwggErMQ8wDQYDVQQDDAZBQyBVQVQxLjAsBgNVBAoMJVNFUlZJQ0lPIERFIEFE
TUlOSVNUUkFDSU9OIFRSSUJVVEFSSUExGjAYBgNVBAsMEVNBVC1JRVMgQXV0aG9y
aXR5MSgwJgYJKoZIhvcNAQkBFhlvc2Nhci5tYXJ0aW5lekBzYXQuZ29iLm14MR0w
GwYDVQQJDBQzcmEgY2VycmFkYSBkZSBjYWRpejEOMAwGA1UEEQwFMDYzNzAxCzAJ
BgNVBAYTAk1YMRkwFwYDVQQIDBBDSVVEQUQgREUgTUVYSUNPMREwDwYDVQQHDAhD
T1lPQUNBTjERMA8GA1UELRMIMi41LjQuNDUxJTAjBgkqhkiG9w0BCQITFnJlc3Bv
bnNhYmxlOiBBQ0RNQS1TQVQwHhcNMTkwNjE3MTk0NDE0WhcNMjMwNjE3MTk0NDE0
WjCB4jEnMCUGA1UEAxMeRVNDVUVMQSBLRU1QRVIgVVJHQVRFIFNBIERFIENWMScw
JQYDVQQpEx5FU0NVRUxBIEtFTVBFUiBVUkdBVEUgU0EgREUgQ1YxJzAlBgNVBAoT
HkVTQ1VFTEEgS0VNUEVSIFVSR0FURSBTQSBERSBDVjElMCMGA1UELRMcRUtVOTAw
MzE3M0M5IC8gWElRQjg5MTExNlFFNDEeMBwGA1UEBRMVIC8gWElRQjg5MTExNk1H
Uk1aUjA1MR4wHAYDVQQLExVFc2N1ZWxhIEtlbXBlciBVcmdhdGUwggEiMA0GCSqG
SIb3DQEBAQUAA4IBDwAwggEKAoIBAQCN0peKpgfOL75iYRv1fqq+oVYsLPVUR/Gi
bYmGKc9InHFy5lYF6OTYjnIIvmkOdRobbGlCUxORX/tLsl8Ya9gm6Yo7hHnODRBI
Dup3GISFzB/96R9K/MzYQOcscMIoBDARaycnLvy7FlMvO7/rlVnsSARxZRO8Kz8Z
kksj2zpeYpjZIya/369+oGqQk1cTRkHo59JvJ4Tfbk/3iIyf4H/Ini9nBe9cYWo0
MnKob7DDt/vsdi5tA8mMtA953LapNyCZIDCRQQlUGNgDqY9/8F5mUvVgkcczsIgG
dvf9vMQPSf3jjCiKj7j6ucxl1+FwJWmbvgNmiaUR/0q4m2rm78lFAgMBAAGjHTAb
MAwGA1UdEwEB/wQCMAAwCwYDVR0PBAQDAgbAMA0GCSqGSIb3DQEBCwUAA4ICAQBc
pj1TjT4jiinIujIdAlFzE6kRwYJCnDG08zSp4kSnShjxADGEXH2chehKMV0FY7c4
njA5eDGdA/G2OCTPvF5rpeCZP5Dw504RZkYDl2suRz+wa1sNBVpbnBJEK0fQcN3I
ftBwsgNFdFhUtCyw3lus1SSJbPxjLHS6FcZZ51YSeIfcNXOAuTqdimusaXq15GrS
rCOkM6n2jfj2sMJYM2HXaXJ6rGTEgYmhYdwxWtil6RfZB+fGQ/H9I9WLnl4KTZUS
6C9+NLHh4FPDhSk19fpS2S/56aqgFoGAkXAYt9Fy5ECaPcULIfJ1DEbsXKyRdCv3
JY89+0MNkOdaDnsemS2o5Gl08zI4iYtt3L40gAZ60NPh31kVLnYNsmvfNxYyKp+A
eJtDHyW9w7ftM0Hoi+BuRmcAQSKFV3pk8j51la+jrRBrAUv8blbRcQ5BiZUwJzHF
EKIwTsRGoRyEx96sNnB03n6GTwjIGz92SmLdNl95r9rkvp+2m4S6q1lPuXaFg7DG
BrXWC8iyqeWE2iobdwIIuXPTMVqQb12m1dAkJVRO5NdHnP/MpqOvOgLqoZBNHGyB
g4Gqm4sCJHCxA1c8Elfa2RQTCk0tAzllL4vOnI1GHkGJn65xokGsaU4B4D36xh7e
Wrfj4/pgWHmtoDAYa8wzSwo2GVCZOs+mtEgOQB91/g==
-----END CERTIFICATE-----
</asp:TextBox>

                             <asp:TextBox ID="txtKey" runat="server" CssClass="textMediano" MaxLength="12" placeholder="Clave Predial" ValidationGroup="BuscaPredio" Visible="False" >-----BEGIN PRIVATE KEY-----
MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQDG/n/1ykS/pnDy
0fg67D2P+NgM5pFVO87kcAn33BIl3j5TXI+uB4L+Iu1GDZWQY9UTma8exSO+2sIA
qb1GpeeUTAyNRj03PIcOaDW+tZtvmhuFezKAPgWj3L7ES1eYcJyRWdGlmfql3XMt
riEVj8hTewX+lJ54hSHESyphbk5xKj4kTmBQK7dkDNv0uyuAVeQZvzF3If6oPCU3
sT3XiyoV9Jr2Pte3Rtahv6nkAVONNROb7l0gHuN7Vf6SCYYI73+Fzg0XHYhD5mlA
ZNjTr/4YXaEF60GJjdFvNn3ov9ITp6mVy4vreH0Q4ggG6BHXG9IwGsStAC4zK6tk
oWhj8fVBAgMBAAECggEBAJ9auxoXyoo3PYgWgVSeToZ23n0mPwgkhwAEgNcOUzIk
EYEsRJs2xL3DNoO86SLh74ZssgJQGNoD0Qw64aorvZHSfNK9htQvEnCFH1UDld//
Zz6zc7Oi911LrzD+rL0UoSz84phdAI3HEy9nnHLp26COijRey7Dz3CCXmO7BKOwR
YHJeoCYfKm2jaGifWLiT5dv00hWMCNNhQzGmWOwvZ/IystMKpWOJmR5LH9oyPpID
2qzb+lAZ9bWtY25vNLAGNE1KXzb8Oflf1rQZ/jsPlTmz2gaJOi+fi8AACNRwhhuS
lAbeKBp5dhxrz/rHxWRpYmKPLlnZrTYOtCnMsA0c82ECgYEA5p4pIDXjvagDUehi
9gpdDPwcM27A5JYtdd8UThbTUvou/ky4g9b9boeesv0MIvAT8WdlU/yIsgCvcDbU
MA6fbN4CYVeFpgenceTf/DQC3Jfw7FYdfDU2vmPnNHLOS3bYFunsQ3kjQugnbjr1
eEH3/NrCciUJq5LdOuICJVOp3f0CgYEA3OVRMAqE7TbI28qSmZ0M6lrSP7TcJiQn
X7coc0vhx4zO8XXSa5Jm556jvT/IfsQBATA4VyK6E2TrVBsPs9nj4Vo7+fGVmKFd
btiOAtTvB4aQw6jt/Y6rwdR5TQJt1yLF6q4z8r1Oy5kYgebp6CACSg0YofAL8Duq
sEVuGAW2FZUCgYA63hqPX1IwAmg+izxfo8uW2e/07QuODguyr+wF9uugnb5LKZhc
BxAQG8xV0iQ78t6UW5lQ9ACMt4IQ+d32GnV1m8ItyOKTvBZxC0Rlo30rhBl6qozC
PO8pcGT/TWL9fmuwhavKmWx30rzl9WTderFruQezjWLHiiiwtCpqDs4onQKBgD3M
jiUxnDS9Uf5jsxX04Ssjk5StJbYqATX/CPsQrK7mTvMwsljUEaQVNtv4X0BP17Qc
aHbASWypnEjgdUks2Vsvon3vv5l+86PSRBC4v9LMK+4BceuxIY5Nwk3wMwiwOszI
RdJQch31y+xzzNbbNai/9zc+8CgamUtNcVqZJnLtAoGBALWyEQQM1P26gH+pLtnv
1eUktNAi55sGUaNxolke2GVbtdewGXORhnWV1dSz/6bScAoCu73yKLVmyyfJHxV0
/Cp4v3emgf2qTUdBtqK43TMI1AsnKTLbfvqWyF0u0WPljJ769bqugOW37GcwA0Z4
H6SgjWvXqbwVZDyZX4Unt3K5
-----END PRIVATE KEY-----
</asp:TextBox>
                           
                            <asp:TextBox ID="txtCer" runat="server" CssClass="textMediano" MaxLength="12" placeholder="Clave Predial" ValidationGroup="BuscaPredio" Visible="False" >-----BEGIN CERTIFICATE-----
MIIGFTCCA/2gAwIBAgIUMDAwMDEwMDAwMDA1MDM2Mjc1NTcwDQYJKoZIhvcNAQEL
BQAwggGEMSAwHgYDVQQDDBdBVVRPUklEQUQgQ0VSVElGSUNBRE9SQTEuMCwGA1UE
CgwlU0VSVklDSU8gREUgQURNSU5JU1RSQUNJT04gVFJJQlVUQVJJQTEaMBgGA1UE
CwwRU0FULUlFUyBBdXRob3JpdHkxKjAoBgkqhkiG9w0BCQEWG2NvbnRhY3RvLnRl
Y25pY29Ac2F0LmdvYi5teDEmMCQGA1UECQwdQVYuIEhJREFMR08gNzcsIENPTC4g
R1VFUlJFUk8xDjAMBgNVBBEMBTA2MzAwMQswCQYDVQQGEwJNWDEZMBcGA1UECAwQ
Q0lVREFEIERFIE1FWElDTzETMBEGA1UEBwwKQ1VBVUhURU1PQzEVMBMGA1UELRMM
U0FUOTcwNzAxTk4zMVwwWgYJKoZIhvcNAQkCE01yZXNwb25zYWJsZTogQURNSU5J
U1RSQUNJT04gQ0VOVFJBTCBERSBTRVJWSUNJT1MgVFJJQlVUQVJJT1MgQUwgQ09O
VFJJQlVZRU5URTAeFw0yMDAzMjMyMDI1MTRaFw0yNDAzMjMyMDI1MTRaMIHjMSUw
IwYDVQQDExxNVU5JQ0lQSU8gREUgRU1JTElBTk8gWkFQQVRBMSUwIwYDVQQpExxN
VU5JQ0lQSU8gREUgRU1JTElBTk8gWkFQQVRBMSUwIwYDVQQKExxNVU5JQ0lQSU8g
REUgRU1JTElBTk8gWkFQQVRBMSUwIwYDVQQtExxNRVo5NDAxMDFLMjYgLyBBVVBG
NjUwMzA5UzU1MR4wHAYDVQQFExUgLyBBVVBGNjUwMzA5SE1TR0xSMDcxJTAjBgNV
BAsTHE1VTklDSVBJTyBERSBFTUlMSUFOTyBaQVBBVEEwggEiMA0GCSqGSIb3DQEB
AQUAA4IBDwAwggEKAoIBAQDG/n/1ykS/pnDy0fg67D2P+NgM5pFVO87kcAn33BIl
3j5TXI+uB4L+Iu1GDZWQY9UTma8exSO+2sIAqb1GpeeUTAyNRj03PIcOaDW+tZtv
mhuFezKAPgWj3L7ES1eYcJyRWdGlmfql3XMtriEVj8hTewX+lJ54hSHESyphbk5x
Kj4kTmBQK7dkDNv0uyuAVeQZvzF3If6oPCU3sT3XiyoV9Jr2Pte3Rtahv6nkAVON
NROb7l0gHuN7Vf6SCYYI73+Fzg0XHYhD5mlAZNjTr/4YXaEF60GJjdFvNn3ov9IT
p6mVy4vreH0Q4ggG6BHXG9IwGsStAC4zK6tkoWhj8fVBAgMBAAGjHTAbMAwGA1Ud
EwEB/wQCMAAwCwYDVR0PBAQDAgbAMA0GCSqGSIb3DQEBCwUAA4ICAQCqRXPYVOCQ
zYW13Lx6ZK3IQGExulJLWwwE2pyERH7kEWZD2oxeJ+EmPSpVoypwJ4khxFj1aDg+
pgSNGEUitnqvQ8dLKEQdsJgP9MG38RwqOOd4aS1QH1iJi6IQg5aoBL46NeT0h5Lx
RMH9wf0q440ZYnaHI3hRLwt2Lz8On6z5aG24BncJbWQtEdJnU3MLEyJ8mgBBCXiG
HPXbZm76W+nzcn/jpB0siSWZQd4Lbuz5tIw7PbpSKvYCxouuElz8MEYIaubybjNu
4VVXdA9YfP75ELcDsrWfJCDz16gmiTsaXer8oEj4CB6WuuZ5GEVyquPDehtvOBBv
GGf953P7P2pGycOV+qEXE6Ox5zNr8p/qNY5BlLqqX///muLstSIFVGZ0ex55VadL
8az+RvErA2hI2kXzYlBIcFR49mjTMS+SPxQ8co34p99N3zDwuqZ1nB7to80u+/5c
TJoy28nBoTt+MMiJ9RTbkCrWBlsIoaMWJmdH6O3VEq897+s7GSZrc0Z8UeBVjZgC
Uj+q7oI6b9tbP9RYGCscYzq9OP3rYEJwVH6c/lHO6MbXt+7W/sojHEtZsqn5CW5Z
KGkFTXNm4DMTH6z0uDmk9LZ8RZNLOwKB283uyKIIieteaDrSxfmTs50yZTmASYUk
vXbaG9Q9P/fzasjhebxnZlOjT027tRkkDg==
-----END CERTIFICATE-----
</asp:TextBox>
                            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtClavePredial" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                            &nbsp;&nbsp;
                            <asp:TextBox ID="txtClavePredial" runat="server" CssClass="letraSubTitulo" MaxLength="12" placeholder="Clave Predial" ValidationGroup="BuscaPredio"></asp:TextBox>
                        </td>

                        <td>
                            <asp:ImageButton ID="imbBuscar" runat="server" ImageUrl="~/Img/consultar.png" OnClick="buscaPredio" ValidationGroup="BuscaPredio" />
                            <asp:ImageButton ID="imBuscarPropietario" runat="server" Height="39px" ImageUrl="~/Img/persona.png" OnClick="buscarPropietario" />
                        </td>
                        <td>
                            <asp:Button ID="bntNuevaConsulta" runat="server" OnClick="bntNuevaConsulta_Click" Text="Recargar" Width="97px" />
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td >
                            <asp:Label ID="lblCuentaPredial" runat="server" CssClass="letraSubTitulo" Text="Cuenta predial:" Visible="False"></asp:Label>
                        </td>
                        <td>
                            <ajaxToolkit:MaskedEditExtender  runat="server" ID="meeFiltro" Mask="9999-99-999-999" MaskType="Number"  InputDirection="LeftToRight" TargetControlID="txtClavePredial" />
                            <asp:Label ID="lblCuentaPredialTxt" runat="server" CssClass="letraSubTitulo" Text="Cuenta predial:" Visible="False"></asp:Label>
                        </td>
                        <td>&nbsp;&nbsp;  </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblPropietario" runat="server" CssClass="letraMediana" Text="Propietario:"></asp:Label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtPropietario" runat="server" CssClass="textMediano" MaxLength="100" placeholder="Propietario" Enabled="False" ReadOnly="True" Width="622px" Wrap="False"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblUbicacion" runat="server" CssClass="letraMediana" Text="Ubicación:"></asp:Label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtUbicacion" runat="server" CssClass="textMediano" MaxLength="100" placeholder="Ubicación" Enabled="False" ReadOnly="True" Width="622px" Wrap="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblTipoPredio" runat="server" CssClass="letraMediana" Text="Tipo de Predio:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSuperficieTerreno" runat="server" CssClass="letraMediana" Text="Superficie terreno:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblValorTerreno" runat="server" CssClass="letraMediana" Text="Valor Terreno:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSuperficieConstruccion" runat="server" CssClass="letraMediana" Text="Superficie Construcción:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblValorConstruccion" runat="server" CssClass="letraMediana" Text="Valor Construcción:"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtTipoPredio" runat="server" CssClass="textMediano" MaxLength="15" placeholder=" - " Enabled="False" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSuperficieTerreno" runat="server" CssClass="textMediano" MaxLength="15" placeholder="0" Enabled="False" ReadOnly="True" Width="83px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtValorTerreno" runat="server" CssClass="textMediano" MaxLength="15" placeholder="0" Enabled="False" ReadOnly="True" Width="83px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSuperficieConstruccion" runat="server" CssClass="textMediano" MaxLength="15" placeholder="0" Enabled="False" ReadOnly="True" Width="83px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtValorConstruccion" runat="server" CssClass="textMediano" MaxLength="15" placeholder="0" Enabled="False" ReadOnly="True" Width="83px"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblZona" visible="false" runat="server" CssClass="letraMediana" Text="Zona:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblMetrosFrente" visible="false" runat="server" CssClass="letraMediana" Text="Metros De  Frente:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblUltimoPagoIP" runat="server" CssClass="letraMediana" Text="Ultimo Pago Predial"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblUltimoPagoSm" visible="false" runat="server" CssClass="letraMediana" Text="Ultimo Pago SM"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblBaseGravable" runat="server" CssClass="letraMediana" Text="Base Gravable:"></asp:Label>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtZona" visible="false" runat="server" CssClass="textMediano" MaxLength="15" placeholder=" - " Enabled="False" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMetrosFrente" visible="false" runat="server" CssClass="textMediano" MaxLength="15" placeholder="0" Enabled="False" ReadOnly="True" Width="83px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtUltimoPago" runat="server" CssClass="textMediano" MaxLength="10" placeholder=" - " Enabled="False" ReadOnly="True" Width="83px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtUltimoPagoSm" visible="false" runat="server" CssClass="textMediano" MaxLength="10" placeholder=" - " Enabled="False" ReadOnly="True" Width="83px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBaseGravable" runat="server" CssClass="textMediano" MaxLength="15" placeholder="0.00" Enabled="False" ReadOnly="True" Width="83px"></asp:TextBox>
                        </td>
                    </tr>

                </table>
                <br id="imbAsignarDescto">
                <table>
                    <tr>
                        <td style="height: 20px; width: 256px; ">
                            &nbsp;</td>
                        <td style="height: 20px; width: 7px; background-color: #CCCCCC;">&nbsp;</td>
                        <td style="height: 20px; width: 233px; background-color: #CCCCCC;">
                            <asp:Label ID="lblIP0" runat="server" CssClass="letraTituloEmptyGrid"  Text="Periodo a pagar:"></asp:Label>                            
                        </td>
                        <td style="height: 20px; width: 7px; background-color: #CCCCCC;">&nbsp;</td>
                        <td style="height: 20px; background-color: #CCCCCC;">&nbsp;</td>
                        <td style="height: 20px; background-color: #CCCCCC;">&nbsp;</td>
                        <td style="height: 20px; width: 151px; background-color: #CCCCCC;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="height: 37px; width: 256px;">
                            &nbsp;&nbsp;<asp:Label ID="lblInicialSM" runat="server" CssClass="letraMediana" Text="Inicial:" Visible="False"></asp:Label>
                            <asp:DropDownList ID="ddlBimIniSM" runat="server" AutoPostBack="True" CssClass="ddlChico" Enabled="False" Visible="False">
                                <asp:ListItem Value="0">-</asp:ListItem>
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>6</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlEjIniSm" runat="server" AutoPostBack="True" CssClass="ddlMediano" Enabled="False" Visible="False">
                            </asp:DropDownList>
                        </td>
                        <td style="height: 37px; width: 7px;">&nbsp;</td>
                        <td style="height: 37px; width: 233px;">
                            &nbsp;<asp:Label ID="lblInicialIP" runat="server" CssClass="letraMediana" Text="Inicial:"></asp:Label>
                            <asp:DropDownList ID="ddlBimIniIP" runat="server" AutoPostBack="True" CssClass="ddlChico" Enabled="False" Width="29px">
                                <asp:ListItem Value="0">-</asp:ListItem>
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>6</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlEjIniIP" runat="server" AutoPostBack="True" CssClass="ddlMediano" Enabled="False">
                            </asp:DropDownList>
                        </td>
                        <td style="height: 37px; width: 7px;">&nbsp;</td>
                        <td style="height: 37px" colspan="3">
                            <asp:Label ID="lblDif" runat="server" CssClass="letraMediana" Text="Diferencias:"></asp:Label>
                            <asp:TextBox ID="txtImporteDif" runat="server" CssClass="textMedianoRight" Enabled="False" MaxLength="10" placeholder=" - " ReadOnly="True" Width="83px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 256px">
                            &nbsp;<asp:Label ID="lblFinalSM" runat="server" CssClass="letraMediana" Text="Final         :" Visible="False"></asp:Label>
                            &nbsp;<asp:DropDownList ID="ddlBimFinSm" runat="server" AutoPostBack="True" CssClass="ddlChico" Visible="False">
                                <asp:ListItem Value="0">-</asp:ListItem>
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>6</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlEjFinSM" runat="server" AutoPostBack="True" CssClass="ddlMediano" Visible="False">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 7px">&nbsp;</td>
                        <td style="width: 233px">
                            &nbsp;&nbsp;<asp:Label ID="lblFinalIP" runat="server" CssClass="letraMediana" Text="Final     :"></asp:Label>
                            <asp:DropDownList ID="ddlBimFinIP" runat="server" AutoPostBack="True" CssClass="ddlChico">
                                <asp:ListItem Value="0">-</asp:ListItem>
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>6</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlEjFinIP" runat="server" AutoPostBack="True" CssClass="ddlMediano" Enabled="False">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 7px">&nbsp;</td>
                        <td colspan="3">
                            <asp:Label ID="lblDescuento" runat="server" CssClass="letraMediana" Text="Clave descuento:"></asp:Label>
                            <asp:TextBox ID="txtClaveDescuento" runat="server" CssClass="textMediano" MaxLength="4" Width="69px"></asp:TextBox>
                            <asp:ImageButton ID="imgGuardarDescto" runat="server" ImageUrl="~/Img/activar.png" OnClick="imgGuardarDescto_Click" />
                            <asp:ImageButton ID="imgEliminarDescto" runat="server" ImageUrl="~/Img/eliminar.png" OnClick="imgEliminarDescto_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 256px">
                            &nbsp;<asp:Label ID="lblFaseSM" runat="server" CssClass="letraMediana" Text="Fase:" Visible="False"></asp:Label>
                            <asp:TextBox ID="txtFaseSM" runat="server" CssClass="textMediano" Enabled="False" MaxLength="15" placeholder=" - " ReadOnly="True" Visible="False"></asp:TextBox>
                        </td>
                        <td style="width: 7px">&nbsp;</td>
                        <td style="width: 233px">
                            &nbsp;<asp:Label ID="lblFaseIP" runat="server" CssClass="letraMediana" Text="Fase:"></asp:Label>
                            <asp:TextBox ID="txtFaseIP" runat="server" CssClass="textMediano" Enabled="False" MaxLength="15" placeholder=" - " ReadOnly="True"></asp:TextBox>
                        </td>
                        <td style="width: 7px">&nbsp;<td style="text-align: left" colspan="3">
                            <asp:CheckBox ID="checkBoxJYP" runat="server" Text="Descuento Jub y Pens, Adulto Mayor, Discapacitado" />
                        </td>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 12px; width: 256px;">&nbsp;</td>
                        <td style="width: 7px; height: 12px"></td>
                        <td style="height: 12px; width: 233px;"></td>
                        <td style="width: 7px; height: 12px">
                            <td style="height: 12px">&nbsp;<td style="height: 12px"></td>
                                <td style="height: 12px; width: 151px;"></td>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 256px">
                            &nbsp;</td>
                        <td style="width: 7px">&nbsp;</td>
                        <td style="width: 233px">
                            <asp:RadioButton ID="rdbDetalladoIP" runat="server" GroupName="diferencias" Text="Impuesto Desglosado"  />
                        </td>
                        <td style="width: 7px">&nbsp;<td>
                            <asp:RadioButton ID="rdbCalculoCompleto" runat="server" Checked="True" GroupName="diferencias" Text="Calculo completo" />
                            <td>&nbsp;</td>
                            <td style="width: 151px">
                                <asp:Button ID="btnCalcular" runat="server" OnClick="btnCalcular_Click" Text="Calcular" Width="131px" />
                            </td>
                        </td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 256px">
                            &nbsp;</td>
                        
                        <td style="width: 7px;">
                            &nbsp;</td>
                        <td style="width: 233px">
                            <asp:RadioButton ID="rdbDetalladoDif" runat="server" GroupName="diferencias" Text="Diferencias Desglosado" />
                        </td>
                        <td style="width: 7px">&nbsp;<td>
                            <asp:RadioButton ID="rdbDiferencias" runat="server" GroupName="diferencias" Text="Solo Diferencias" />
                            <td>&nbsp;</td>
                            <td style="width: 151px">
                                <asp:Button ID="btnUltimoRec" runat="server" OnClick="btnUltimoRec_Click" Text="Ultimo Recibo" Width="130px" />
                            </td>
                            </td>
                        </td>
                    </tr>
                    <tr> <td></td></tr>
                    <tr> <td></td></tr>

                    <tr>
                          <td colspan="4">
                                <asp:Label ID="Label7" runat="server" CssClass="letraTituloEmptyGrid" Text=" Datos fiscales "></asp:Label>
                        </td>
                     </tr>                   
                    <tr>
                                <td colspan="7" style="height: 22px">
                                    <asp:Label ID="Label8" runat="server" CssClass="letraGrande" Text=" Nombre: "></asp:Label>;&nbsp;&nbsp;
                                    <asp:Label ID="lblNombreFiscaltxt" runat="server" CssClass="letraMediana" Text=""></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="blrfc" runat="server" CssClass="letraGrande" Text=" RFC: "></asp:Label>;&nbsp;&nbsp;
                                    <asp:Label ID="lblRFCtxt" runat="server" CssClass="letraMediana" Text=""></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Label6" runat="server" CssClass="letraGrande" Text=" Regimen: "></asp:Label>;&nbsp;&nbsp;
                                    <asp:Label ID="lblRegimen" runat="server" CssClass="letraMediana" Text=""></asp:Label>
                                    <br />
                                    <asp:Label ID="Label1" runat="server" CssClass="letraGrande" Text=" Correo: "></asp:Label>;&nbsp;&nbsp;
                                    <asp:Label ID="lblCorreoFiscaltxt" runat="server" CssClass="letraMediana" Text=""></asp:Label>
                                </td>
                      </tr>
                      
                      <tr>
                                <td colspan="7">
                                    <asp:Label ID="lbldirfiscallbl" runat="server" CssClass="letraGrande" Text=" Domicilio: "></asp:Label>
                                    <asp:Label ID="lblDirFiscaltxt" runat="server" CssClass="letraMediana" Text=""></asp:Label>
                                    <br />
                                    <asp:Label ID="lblEstadolnlbl" runat="server" CssClass="letraGrande" Text=" Estado : "></asp:Label>
                                    <asp:Label ID="lblEstadotxt" runat="server" CssClass="letraMediana" Text=""></asp:Label>
                                </td>
                       </tr>
          
                                                     
                </table>
            </div>

            <br />
            <div runat="server" id="divEstadoCta">
                <table  style="width: 100%;">

                    <tr>
                        <td colspan="3" style="background-color: #FFFFFF; height: 18px;">
                            &nbsp;
                            <asp:Label ID="lblIP" runat="server" CssClass="letraTituloEmptyGrid" Text="ESTADO DE CUENTA"></asp:Label>
                        </td>
                        <td colspan="2" style="background-color: #FFFFFF; height: 18px;">
                            <asp:Label ID="lblVigencia" runat="server" CssClass="letraMediana" Text="Vigencia"></asp:Label>
                            <asp:TextBox ID="txtVigencia" runat="server" CssClass="textMediano" Enabled="False" MaxLength="15" placeholder="00/00/0000" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="background-color: #FFFFFF; height: 18px;">
                            <asp:Label ID="lblDescripcion" runat="server" CssClass="h4" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="background-color: #FFFFFF; height: 24px;">
                            <asp:Label ID="lblDetalle" runat="server" CssClass="lead" Text="Label"></asp:Label>
                        </td>
                    </tr>
                      <tr>
                        <td colspan="4" style="background-color: #FFFFFF; height: 24px;">
                            <asp:Label ID="lbl_titulo" runat="server" CssClass="leftCol" Text="Label"></asp:Label>
                        </td>
                    </tr>
                      <tr>
                        <td colspan="1" style="background-color: #FFFFFF; height: 24px;">
                            <asp:Label ID="lblPeriodoIP" runat="server" CssClass="letraMediana" Text="Periodo de pago"></asp:Label>
                          </td>
                         <td colspan="2" >
                             <asp:TextBox ID="txtPeriodoIP" runat="server" CssClass="textGrandeCenter" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>
                          </td>                  
                                           
                         <td>
                             <asp:Label ID="lblPeriodoSM" runat="server" CssClass="letraMediana" Text="Periodo de pago" visible="false"></asp:Label>
                          </td>                     
                         <td>
                             <asp:TextBox ID="txtPeriodoSM" runat="server" CssClass="textChico" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True" visible="false"></asp:TextBox>
                          </td>
                    </tr>
                    <tr>
                        <td colspan="1" style="background-color: #C0C0C0" class="letraTituloEmptyGrid">
                            Concepto</td>
                         <td colspan="1" style="background-color: #C0C0C0">
                             <asp:Label ID="lblSM" runat="server" CssClass="letraTituloEmptyGrid" Text="Importe"></asp:Label>
                        </td>
                        <td colspan="1" style="background-color: #C0C0C0">
                            <asp:Label ID="lblSM0" runat="server" CssClass="letraTituloEmptyGrid" Text="Descuento"></asp:Label>
                        </td>
                        <td colspan="1" style="background-color: #C0C0C0">
                            &nbsp;</td>
                         <td colspan="1" style="background-color: #C0C0C0">
                            &nbsp;</td>
                    </tr>
                    
                    <tr>
                        <td  style="height: 37px">
                            <asp:Label ID="lblImpuestoAntTexto" runat="server" CssClass="letraMediana" Text="Impuesto Predial Anticipado "></asp:Label>
                            <asp:Label ID="lblImpuestoAnt" runat="server" CssClass="letraMediana" Text=""></asp:Label>
                        </td>                        
                        <td style="height: 37px">
                            <asp:TextBox ID="txtImpuestoAnt" runat="server" CssClass="textMedianoRight" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td style="height: 37px"> 
                            <asp:TextBox ID="txtDescImpuestoAnt" runat="server" CssClass="textMedianoRight" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>                            
                        </td>
                        
                        <td style="width: 139px" >
                            <asp:Label ID="lblInfraestructuraAntSm" visible="False" runat="server" CssClass="letraMediana" Text="IA"></asp:Label>
                            <asp:TextBox ID="txtInfraestructuraAntSm" runat="server" CssClass="textChico" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True" visible="false"></asp:TextBox>
                        </td>
                         <td style="width: 242px" >
                            <asp:Label ID="lblAdicionalAntSm" runat="server" CssClass="letraMediana" Text="AA" visible="False"></asp:Label>
                            <asp:TextBox ID="txtAdicionalAntSm" runat="server" CssClass="textChico" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True" visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 17px">
                            <asp:Label ID="lblImpuesto" runat="server" CssClass="letraMediana" Text="Impuesto Predial"></asp:Label>
                            <asp:Label ID="lblImpuestoActual" runat="server" CssClass="letraMediana" Text=""></asp:Label>
                        </td>                          
                        <td style="height: 17px">
                            <asp:TextBox ID="txtImpuesto" runat="server" CssClass="textMedianoRight" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td style="height: 37px"> 
                            <asp:TextBox ID="txtDescImpuesto" runat="server" CssClass="textMedianoRight" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>                            
                        </td>

                         <td style="width: 139px" >
                            <asp:Label ID="lblInfraestructuraAnt" runat="server" CssClass="letraMediana" Text="I" visible="False"></asp:Label>
                            <asp:TextBox ID="txtInfraestructuraSm" runat="server" CssClass="textChico" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True" visible="false"></asp:TextBox>
                        </td>
                         <td style="width: 242px" >
                            <asp:Label ID="lblRecoleccionSm" runat="server" CssClass="letraMediana" Text="RR" visible="False"></asp:Label>
                            <asp:TextBox ID="txtRecoleccionSm" runat="server" CssClass="textChico" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True" visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDiferencias" runat="server" CssClass="letraMediana" Text="Diferencias"></asp:Label>
                        </td>                        
                        <td>
                            <asp:TextBox ID="txtDiferencias" runat="server" CssClass="textMedianoRight" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>
                        </td>
                         <td style="height: 37px"> 
                            <asp:TextBox ID="txtDescDiferencias" runat="server" CssClass="textMedianoRight" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>                            
                        </td>

                        <td style="width: 139px">
                            <asp:Label ID="lblLimpiezaSm" runat="server" CssClass="letraMediana" Text="LDFB" visible="False"></asp:Label>
                            <asp:TextBox ID="txtLimpiezaSm" runat="server" CssClass="textChico" Enabled="False"  placeholder="0.00" ReadOnly="True" visible="false" ></asp:TextBox>
                        </td>
                        <td style="width: 242px">
                            <asp:Label ID="lblDapSm" runat="server" CssClass="letraMediana" Text="DAP" visible="false"></asp:Label>
                            <asp:TextBox ID="txtDapSm" runat="server" CssClass="textChico" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True" visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblRecargoDiferencias" runat="server" CssClass="letraMediana" Text="Recargo Diferencias"></asp:Label>
                        </td>                        
                        <td>
                            <asp:TextBox ID="txtRecargosDif" runat="server" CssClass="textMedianoRight" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>
                        </td>
                         <td style="height: 37px"> 
                            <asp:TextBox ID="txtDescRecargoDif" runat="server" CssClass="textMedianoRight" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>                            
                        </td>

                        <td style="width: 139px">
                            <asp:Label ID="lblAdicionalSm" runat="server" CssClass="letraMediana" Text="A" visible="False"></asp:Label>
                            <asp:TextBox ID="txtAdicionalSm" runat="server" CssClass="textChico" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True" visible="false"></asp:TextBox>
                        </td>
                        <td style="width: 242px">
                            <asp:Label ID="lblRezagosSm" runat="server" CssClass="letraMediana" Text="Rez" visible="False"></asp:Label>
                            <asp:TextBox ID="txtRezagosSm" runat="server" CssClass="textChico" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True" visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 22px">
                            <asp:Label ID="lblRezagos" runat="server" CssClass="letraMediana" Text="Rezagos de Impuesto Predial"></asp:Label>
                            &nbsp;<asp:Label ID="Label17" runat="server" CssClass="letraMediana" Text="Años Anteriores"></asp:Label>
                        </td>                        
                        <td style="height: 22px">
                            <asp:TextBox ID="txtRezagos" runat="server" CssClass="textMedianoRight" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>
                        </td>
                         <td style="height: 37px"> 
                            <asp:TextBox ID="txtDescRezagos" runat="server" CssClass="textMedianoRight" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>                            
                        </td>

                         <td style="width: 139px" >
                            <asp:Label ID="lblRecargosSm" runat="server" CssClass="letraMediana" Text="R" visible="False"></asp:Label>
                            <asp:TextBox ID="txtRecargosSm" runat="server" CssClass="textChico" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True" visible="false"></asp:TextBox>
                        </td>
                         <td style="width: 242px" >
                            <asp:Label ID="lblHonorariosSM" runat="server" CssClass="letraMediana" Text="HN" visible="False"></asp:Label>
                            <asp:TextBox ID="txtHonorariosSm" runat="server" CssClass="textChico" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True" visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblRecargos" runat="server" CssClass="letraMediana" Text="Recargos"></asp:Label>
                        </td>                        
                        <td>
                            <asp:TextBox ID="txtRecargos" runat="server" CssClass="textMedianoRight" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>
                        </td>
                         <td style="height: 37px"> 
                            <asp:TextBox ID="txtDescRecargos" runat="server" CssClass="textMedianoRight" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>                            
                        </td>

                        <td style="width: 139px">
                            <asp:Label ID="lblGastosEjSm" runat="server" CssClass="letraMediana" Text="GDE" visible="False"></asp:Label>
                            <asp:TextBox ID="txtEjecucionSm" runat="server" CssClass="textChico" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True" visible="false"></asp:TextBox>
                        </td>
                        <td style="width: 242px">
                            <asp:Label ID="lblMultasSm" runat="server" CssClass="letraMediana" Text="Multas" visible="false"></asp:Label>
                            <asp:TextBox ID="txtMultasSm" runat="server" CssClass="textChico" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True" visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblHonorarios" runat="server" CssClass="letraMediana" Text="Honorarios De Notificación"></asp:Label>
                        </td>                        
                        <td>
                            <asp:TextBox ID="txtHonorarios" runat="server" CssClass="textMedianoRight" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td style="height: 37px"> 
                            <asp:TextBox ID="txtDescHonorarios" runat="server" CssClass="textMedianoRight" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>                            
                        </td>

                        <td style="width: 139px">
                            <asp:Label ID="lblDescuentosSm" runat="server" CssClass="letraMediana" Text="D" visible="False"></asp:Label>
                            <asp:TextBox ID="txtDescuentosSm" runat="server" CssClass="textChico" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True" visible="false"></asp:TextBox>
                        </td>
                        <td style="width: 242px">
                            <asp:Label ID="lblTotalSm" runat="server" CssClass="text-danger" Text="TTL SM" visible="False"></asp:Label>
                            <asp:TextBox ID="txtTotalSm" runat="server" CssClass="textChico" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True" visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <asp:Label ID="lblGastosEj" runat="server" CssClass="letraMediana" Text="Gastos De Ejecución"></asp:Label>
                        </td>                        
                        <td>
                            <asp:TextBox ID="txtEjecucion" runat="server" CssClass="textMedianoRight" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td style="height: 37px"> 
                            <asp:TextBox ID="txtdDescEjecucion" runat="server" CssClass="textMedianoRight" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>                            
                        </td>

                        <td style="width: 139px">
                            <asp:Label ID="lblAdicionalAnt" visible="False" runat="server" CssClass="letraMediana" Text="AA"></asp:Label>
                            <asp:TextBox ID="txtAdicionalAnt" visible="false" runat="server" CssClass="textChico" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td style="width: 242px">
                            <asp:Label ID="lblAdicional" visible="false" runat="server" CssClass="letraMediana" Text="Adicional"></asp:Label>
                            <asp:TextBox ID="txtAdicional" visible="false" runat="server" CssClass="textChico" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblMultas" runat="server" CssClass="letraMediana" Text="Multas"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMultas" runat="server" CssClass="textMedianoRight" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="textDescMultas" runat="server" CssClass="textMedianoRight" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td style="width: 139px">
                            &nbsp;</td>
                        <td style="width: 242px">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblTotal" runat="server" CssClass="letraMediana" Text="SUBTOTAL"></asp:Label>
                        </td>

                        <td>
                            <asp:TextBox ID="txtTotalIp" runat="server" CssClass="textMedianoRight" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>
                        <td>
                            <asp:TextBox ID="txtDescuentos" runat="server" CssClass="textMedianoRight" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:TextBox>
                          <td style="height: 37px; width: 139px;"> 
                                           
                        </td>
                        <td style="width: 242px">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="height: 22px">
                            </td>
                        <td style="height: 22px">
                            </td>
                        <td style="height: 22px">
                            </td>
                        <td style="height: 22px; width: 139px;">
                            </td>
                    </tr>
                    <tr text-align: center;>
                        <td colspan="1" style="height: 23px">
                            <asp:Label ID="lblVigencia0" runat="server" CssClass="letraTituloEmptyGrid" Text="IMPORTE A PAGAR"></asp:Label>
                        </td>
                        <td colspan="2" style="height: 23px" text-align:"center">
                            <asp:TextBox ID="txtImporte" runat="server" CssClass="textGrandeCenter" Enabled="False"  placeholder="Importe Total" ReadOnly="True" >0.00</asp:TextBox>
                        </td >
                       
                    </tr>
                    <tr>
                        <td style="height: 21px">
                            <asp:Button ID="btnConvenio" runat="server" OnClick="btnConvenio_Click" Text="Generar Convenio" Width="163px" />
                        </td>
                        <td style="height: 21px">
                            <asp:Button ID="btnEstado" runat="server" CausesValidation="False" OnClick="btnEstado_Click" Text="Estado de Cuenta" Width="163px" />
                        </td>
                        <td style="height: 21px">
                            <asp:Button ID="btnCancelar" runat="server" CausesValidation="False" OnClick="btnCancelar_Click" Text="Regresar" Width="163px" />
                        </td>
                        <td style="height: 21px; width: 139px;">
                            <asp:Button ID="btnCobrar" runat="server" CausesValidation="False" OnClick="btnCobrar_Click" Text="Cobrar" Width="163px" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 22px">
                            </td>
                        <td style="height: 22px">
                            </td>
                        <td style="height: 22px">
                            </td>
                        <td style="height: 22px; width: 139px;">
                            </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center">
                            &nbsp;</td>
                        <td colspan="2" style="text-align: center">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align: center">
                            <asp:Label ID="lblWarning" runat="server" CssClass="label-warning" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td style="width: 139px">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td style="width: 139px">&nbsp;</td>
                    </tr>
                </table>
            </div>
            <br />
            <br />
            <br />

            <div runat="server" id="divDetallado">
                <asp:GridView ID="grd" runat="server">

                    <FooterStyle CssClass="grdFooter" />
                    <HeaderStyle CssClass="grdHead" />
                    <RowStyle CssClass="grdRowPar" />
                </asp:GridView>
                <br />
                <asp:GridView ID="grds" runat="server">
                    <FooterStyle CssClass="grdFooter" />
                    <HeaderStyle CssClass="grdHead" />
                    <RowStyle CssClass="grdRowPar" />
                </asp:GridView>
                <br />
                <br />
                <asp:Button ID="btnCancelarGrid" runat="server" CausesValidation="False" OnClick="btnCancelar_Click" Text="Cancelar" Width="163px" />
                &nbsp;&nbsp;&nbsp;
                <br />
            </div>


            
             <asp:Panel ID="pnl" runat="server" class="formPanel">
                <table>
                    <tr style="background-color: #b4b4b4" >
                        <td colspan="6" style="height: 36px">
                            &nbsp;&nbsp;&nbsp;<asp:Label ID="lblTituloConceptoId" runat="server" CssClass="textModalTitulo2" Text="Cobros"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblImportePago" runat="server" CssClass="textModalTitulo2" Text="Importe:"></asp:Label>
                            <asp:RadioButton ID="rdbIP" runat="server" AutoPostBack="True" Checked="True" CssClass="radio" GroupName="tipoCobro" visible="false" OnCheckedChanged="rdbIP_CheckedChanged" Text="IP" />
                            <asp:RadioButton ID="rdbSM" runat="server" AutoPostBack="True" CssClass="radio" GroupName="tipoCobro" visible="false"  OnCheckedChanged="rdbSM_CheckedChanged" Text="SM" />
                        
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="vertical-align:top; " >
                            &nbsp;</td>
                        <td colspan="4">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="1" style="height: 51px" >
                            <asp:Label ID="lblMetodoPago" runat="server" Text="Metodo de Pago:" Visible="False" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td colspan="3" style="vertical-align:top;">
                            <asp:DropDownList ID="ddlMetodoPago" runat="server" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlMetodoPago_SelectedIndexChanged"></asp:DropDownList>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                           <br />
                        </td>
                        <td colspan="2" style="vertical-align:top;">
                             <asp:Button ID="btnAgregarConcepto" runat="server" OnClick="btnAgregarConcepto_Click" Text="Agregar Concepto" ValidationGroup="agregarConcepto" />
                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="vertical-align:top;">
                            <asp:Label ID="lblMonto" runat="server" Text="Monto:" CssClass="letraMediana"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtMonto" runat="server" CssClass="textMedianoRight" placeholder=" monto "></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtMonto" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="validaCheque"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtMonto" CssClass="valida"  Font-Size="XX-Small" SetFocusOnError="True" ErrorMessage="Formato de número invalido, se permiten máximo dos decimales." ValidationExpression="\d+(\.\d{1,2})?|\.\d{1,2}" ValidationGroup="validaCheque"></asp:RegularExpressionValidator>
                        </td>
                        <td colspan="2" style="vertical-align:top;">
                            <asp:Label ID="lblNumeroAprobacion" runat="server" CssClass="letraMediana" Text="No. de Operación:"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtNumeroAprobacion" runat="server" CssClass="textMedianoRight" MaxLength="10" placeholder="número"></asp:TextBox>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtNumeroAprobacion" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="validaTarjeta"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtNumeroAprobacion" CssClass="valida" ErrorMessage="Ingresar solo numeros enteros" Font-Size="XX-Small" SetFocusOnError="True" ValidationExpression="^\d+$" ValidationGroup="validaTarjeta"></asp:RegularExpressionValidator>
                            <br />
                        </td>
                        <td colspan="2" style="vertical-align:top;">
                            <asp:Label ID="lblInstitucion" runat="server" CssClass="letraMediana" Text="Inst. Financiera"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtInstitucion" runat="server" CssClass="textMedianoRight" MaxLength="10" placeholder="Banco"></asp:TextBox>
                        </td>
                    </tr>
                   
                    <tr>
                        <td  style="vertical-align:top;"  colspan="6">

                          <asp:GridView ID="grdAlta" runat="server" DataKeyNames="Id,Clave" AutoGenerateColumns="false" CssClass="grdRowPar" style="margin-right: 0px"
                                AllowSorting="True" BorderStyle="None" ShowFooter="false" OnRowCommand="grdAlta_RowCommand" OnRowDataBound="grdAlta_RowDataBound" OnSorting="grdAlta_Sorting" ShowHeader="false" Width="415px">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>  
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Clave" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClave" runat="server" Text='<%# Eval("Clave") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Método" ItemStyle-Width="300px" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTipoCobro" runat="server" Text='<%# Eval("TipoCobro") %>' Visible="true"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                   
                                   <asp:TemplateField HeaderStyle-HorizontalAlign="Left" Visible="true" HeaderText="Importe" InsertVisible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblImporte" runat="server" Visible="true" Text='<%# Eval("Importe") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotal" runat="server" Text="0" CssClass="valida"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="135px" HeaderText="No. Transaccción" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAprobacion" runat="server" Text='<%# Eval("Transaccion") %>'></asp:Label>
                                        </ItemTemplate>                                      
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left"  ItemStyle-Width="300px" HeaderText="Institución" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInstitucion" runat="server" Text='<%# Eval("Institucion") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField ItemStyle-CssClass="" HeaderText=" " ItemStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgDelete" runat="server" ToolTip="Eliminar Tramite!"
                                                ImageUrl="~/img/eliminar.png"
                                                CssClass="imgButtonGrid"
                                                CommandName="EliminarRegistro"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                                           <%-- <asp:ImageButton ID="imgActivar" runat="server" ToolTip="Activar!"
                                                ImageUrl="~/img/Activar.png"
                                                CssClass="imgButtonGrid"
                                                CommandName="ActivarRegistro"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                                            <asp:ImageButton ID="imgUpdate" runat="server" ToolTip="Editar!"
                                                ImageUrl="~/img/modificar.png"
                                                CssClass="imgButtonGrid"
                                                CommandName="EditarImporte"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle CssClass="grdFooter" />
                                <HeaderStyle CssClass="grdHead" />
                                <RowStyle CssClass="grdRowPar" />
                            </asp:GridView>
                      
                       
                            <asp:Label ID="lblCambiosFooter" runat="server" CssClass="grdFooter"></asp:Label>
                            <br />
                      
                       
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="vertical-align:top;" >
                        
                            <br />
                        
                            <br />
                        </td>
                        <td  colspan="2" style="vertical-align:top;" >
                             <br />
                        </td>
                         <td  style="vertical-align:top;"  colspan="2">
                             <br />
                             <br />
                           
                             &nbsp;</td>
                    </tr>
                     <tr>
                        <td style="vertical-align:top; width: 45px;" >
                            <asp:Label ID="lblObservacion" runat="server" CssClass="letraMediana" Text="Observaciones:"></asp:Label>
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtObservacion" TextMode="MultiLine" runat="server" CssClass="textGrande" MaxLength="150" placeholder="Observaciones" Height="40px" Width="406px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 45px; height: 37px;">
                            <%--<asp:Label ID="lblNumeroTarjeta" runat="server" Text="Número de Tarjeta:" CssClass="letraMediana"></asp:Label>
                        --%>
                            <asp:Label ID="lblCambio" runat="server" CssClass="textModalTitulo2" SetFocusOnError="True" Text="Cambio:"></asp:Label>
                         </td>
                        <td colspan="5" style="height: 37px">
                            <%--<asp:TextBox ID="txtNumeroTarjeta" runat="server" CssClass="textGrande" MaxLength="4" placeholder="Número de Tarjeta."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="valida" ControlToValidate="txtNumeroTarjeta" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="validaTarjeta"></asp:RequiredFieldValidator><br />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Ingresar solo numeros enteros" ValidationExpression="^\d+$" ControlToValidate="txtNumeroTarjeta" CssClass="valida" SetFocusOnError="True" ValidationGroup="validaTarjeta" Font-Size="Small"></asp:RegularExpressionValidator>
                        --%>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 45px;">
                           </td>
                        <td colspan="5">
                            <asp:Button ID="btnAceptarCobro" runat="server" OnClick="btnAceptarCobro_Click" Text="Aceptar" ValidationGroup="agregarCobro" />
                            <asp:Button ID="btnAceptarPago" runat="server" OnClick="btnAceptarPago_Click" Text="Aceptar" ValidationGroup="validaEfectivo" />
                            <asp:Button ID="btnCancelarCobro" runat="server" OnClick="btnCancelarCobro_Click" Text="Cancelar" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 45px;">
                            &nbsp;</td>
                       <td style="text-align: right; width: 113px;">
                            &nbsp;</td>
                        <td style="text-align: right; width: 54px;">
                            &nbsp;</td>
                        <td style="text-align: right; width: 54px;">
                            &nbsp;</td>
                        <td style="text-align: right;">
                            &nbsp;</td>
                        <td style="text-align: right; width: 54px;">
                            &nbsp;</td>
                    </tr>
                </table>
            </asp:Panel>





            <ajaxToolkit:ModalPopupExtender ID="pnl_Modal" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnl" TargetControlID="btnPnl">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnPnl" runat="server" />



            <asp:Panel ID="pnlConvenio" runat="server" class="formPanel">
                <asp:Label ID="lblConvenio" runat="server" CssClass="letraTituloEmptyGrid">Selecciona el tipo de convenio:</asp:Label>
                <br />
                <br />
                <asp:RadioButton ID="rbtnIP" runat="server" Checked="True" GroupName="convenio" Text="Impuesto Predial" />
                <asp:RadioButton ID="rbtnSM" runat="server" GroupName="convenio" Text="Servicios Municipales" />
                &nbsp;<asp:RadioButton ID="rbtnAM" runat="server" GroupName="convenio" Text="Ambos" ValidationGroup="convenio" />
                <br />
                <br />
                <asp:Button ID="btnAceptarConv" runat="server" Text="Aceptar" Visible="true" OnClick="btnAceptarConv_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnCancelarConv" runat="server" Text="Cancelar" Visible="true" />
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="modalConvenio" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnlConvenio" TargetControlID="btnConv">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnConv" runat="server" />




            <%--            <asp:Panel ID="pnlDescuento" runat="server" class="formPanel">
                <asp:Label ID="Label17" runat="server" CssClass="letraTituloEmptyGrid">Selecciona el Impuesto para aplicar Descuento:</asp:Label>
                <br />
                <br />
                <asp:RadioButton ID="RadioButton1" runat="server" Checked="True" GroupName="Descuento" Text="Impuesto Predial" />               
                <asp:RadioButton ID="RadioButton2" runat="server" GroupName="Descuento" Text="Servicios Municipales" />          
                &nbsp;<asp:RadioButton ID="RadioButton3" runat="server" GroupName="Descuento" Text="Ambos" ValidationGroup="Descuento" />
                <br />
                <br />
                <asp:Button ID="AceptarDescuento" runat="server" Text="Aceptar" Visible="true" OnClick="AceptarDescuento_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="Button4" runat="server" Text="Cancelar" Visible="true"  />
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="modalDescuento" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnlDescuento"  TargetControlID="btnDescto">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnDescto" runat="server" />--%>



            <asp:Panel ID="pnlRecibo" runat="server" Width="1013px" Height="565px" BackColor="White">
            <div class="width:100%;margin:1px;" id="divCerrarFactura" visible="false">
	            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Img/eliminar.png" ImageAlign="Right" OnClick="ImageButton1_Click" />
            </div>
            <iframe id="frameRecibo" runat="server" src="" width="100%" height="90%" style="border: none;" />
            </asp:Panel>

            <ajaxToolkit:ModalPopupExtender ID="modalRecibo" runat="server" BackgroundCssClass="ventanaBackground"
            PopupControlID="pnlRecibo" TargetControlID="btnRecibo">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnRecibo" runat="server" />

            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnl" TargetControlID="btnPnl">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="HiddenField1" runat="server" />

            <asp:Panel ID="pnlFactura" runat="server" class="formPanel">
                <table>
                    <tr>
                        <td style="width: 600px">
                            <asp:Label ID="Label3" runat="server" CssClass="textModalTitulo2" Text="Introduzca su RFC:"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 600px">
                            <asp:TextBox ID="txtRFCbuscar" MaxLength="13" runat="server" CssClass="textGrande"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ForeColor="OrangeRed" ControlToValidate="txtRFCbuscar" ValidationGroup="BuscarRFC"></asp:RequiredFieldValidator>
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
                            <asp:Button ID="Button2" runat="server" Text="Cancelar" OnClick="Button2_Click1" />
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
                                        <asp:Label ID="Label4" runat="server" CssClass="letraSubTitulo" Text="RFC:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRFC" runat="server" MaxLength="13" CssClass="textMediano" AutoPostBack="True"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" CssClass="letraSubTitulo" Text="Nombre:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNombre" runat="server" MaxLength="100" CssClass="textGrande"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rfv6" runat="server" ControlToValidate="txtNombre" ErrorMessage="*" ForeColor="OrangeRed" ValidationGroup="Registro">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <%--<tr>
                            <td style="width: 20px;"></td>
                            <td>
                                <asp:Label ID="Label6" runat="server" CssClass="letraSubTitulo" Text="Calle:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCalle" runat="server" MaxLength="100" CssClass="textGrande"></asp:TextBox>

                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfv1" ControlToValidate="txtCalle" runat="server" ErrorMessage="*" ForeColor="OrangeRed" ValidationGroup="Registro">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" CssClass="letraSubTitulo" Text="Municipio:"></asp:Label>
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
                                <asp:Label ID="Label8" runat="server" CssClass="letraSubTitulo" Text="Estado:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEstado" MaxLength="50" runat="server" CssClass="textGrande"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfv2" runat="server" ControlToValidate="txtEstado" ErrorMessage="*" ForeColor="OrangeRed" ValidationGroup="Registro">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label ID="Label9" runat="server" CssClass="letraSubTitulo" Text="Pais:"></asp:Label>
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
                                <asp:Label ID="Label10" runat="server" CssClass="letraSubTitulo" Text="CP:"></asp:Label>
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
                                <asp:Label ID="Label11" runat="server" CssClass="letraSubTitulo" Text="Colonia:"></asp:Label>
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
                                <asp:Label ID="Label12" runat="server" CssClass="letraSubTitulo" Text="Localidad:"></asp:Label>
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
                                <asp:Label ID="Label15" runat="server" CssClass="letraSubTitulo" Text="Referencia:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferencia" MaxLength="100" runat="server" CssClass="textGrande"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                        </tr>--%>
                                <tr>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <asp:Label ID="Label16" runat="server" CssClass="letraSubTitulo" Text="Correo Electronico:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtCorreoReg" MaxLength="100" runat="server" CssClass="textGrande"></asp:TextBox>
                                        <br />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtCorreoReg"
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



            <asp:Panel ID="pnlEstado" runat="server" Width="1013px" Height="565px" BackColor="White">

                <div class="width:100%;margin:1px;" style="text-align: right">
                    <asp:ImageButton ID="imCerrarEstado" runat="server" ImageUrl="~/Img/eliminar.png" ImageAlign="Right" OnClick="imCerrarEstado_Click" />
                </div>
                <iframe id="frameEstado" runat="server" src="" style="border-style: none; border-color: inherit; border-width: medium; height: 75%; width: 96%;" />
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="modalEstado" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnlEstado" TargetControlID="btnEdoCta">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnEdoCta" runat="server" />


            <asp:Panel ID="pnlPropietario" runat="server" class="formPanel">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="height: 60px;">
                            <asp:Label ID="Label2" runat="server" Text="Propietarios" CssClass="letraTitulo"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="height: 2px">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 0px 10px 10px 10px;">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlFiltro" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFiltro" runat="server" Enabled="false" CssClass="textGrande"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="btnPropietarioM" runat="server" CausesValidation="False"
                                            ImageUrl="~/img/consultar.fw.png" OnClick="consultarPropietario" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnCancelarPropietarios" runat="server" OnClick="btnCancelarPropietario_Click"
                                            Text="Cancelar" CausesValidation="False" />
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="padding: 0px 10px 10px 10px;">
                            <asp:GridView ID="grdPropietarios" runat="server" AutoGenerateColumns="False"
                                CssClass="grd" DataKeyNames="id,activo" AllowPaging="True"
                                AllowSorting="True" BorderStyle="None" ShowFooter="True"
                                OnRowCommand="grdPropietarios_RowCommand" OnSorting="grdPropietarios_Sorting"
                                OnPageIndexChanging="grdPropietarios_PageIndexChanging" OnRowDataBound="grdPropietarios_RowDataBound" PageSize="5">
                                <Columns>
                                    <asp:BoundField DataField="ClavePredial" HeaderText="Clave Catastral" SortExpression="ClavePredial" />
                                    <asp:BoundField DataField="IdContribuyente" HeaderText="Propietario" SortExpression="IdContribuyente" />
                                    <asp:BoundField DataField="IdColonia" HeaderText="Dirección" SortExpression="IdColonia" />
                                    <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="135px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgActivar" runat="server" ToolTip="Seleccionar"
                                                ImageUrl="~/img/Activar.png"
                                                CssClass="imgButtonGrid"
                                                CommandName="ActivarRegistro"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblEmptySearch" runat="server" CssClass="letraTituloEmptyGrid">No se encontraron resultados</asp:Label>
                                </EmptyDataTemplate>
                                <FooterStyle CssClass="grdFooter" />
                                <HeaderStyle CssClass="grdHead" />
                                <RowStyle CssClass="grdRowPar" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="modalPropietario" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnlPropietario" TargetControlID="btnPropietario">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnPropietario" runat="server" />
            <uc1:ModalPopupMensaje ID="ModalPopupMensaje1" runat="server" DysplayAceptar="True" DysplayCancelar="True" />
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>

            <asp:Panel ID="pnlCorreo" runat="server" class="formPanel">
                <table>
                    <tr>
                        <td style="width: 543px">
                            <asp:Label ID="Label19" runat="server" CssClass="textModalTitulo2" Text="DIRECCIÓN DE CORREO."></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 543px">
                            <asp:TextBox ID="txtCorreoEnvio" MaxLength="100" runat="server" CssClass="textGrande"></asp:TextBox>
                            <br />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtCorreoEnvio"
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

            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />
        </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnDescargaPDF" />--%>
            <%--<asp:PostBackTrigger ControlID="btnDescargaXML" />--%>
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
