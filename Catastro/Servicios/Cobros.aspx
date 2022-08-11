<%@ Page Title="Cobros" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cobros.aspx.cs" Inherits="Catastro.Servicios.Cobros" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function printPdf() {
            var PDF = document.getElementById("MainContent_frameRecibo");
            PDF.focus();
            PDF.contentWindow.print();
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td style="height: 60px;">
                        <asp:Label ID="lblTitulo" runat="server" CssClass="textModalTitulo2" Text="Cobros"></asp:Label>
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
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 70%">
                        <table style="width: 100%">
                            <tr style="background-color: #b4b4b4">
                                <td style="text-align: right;">
                                    <asp:Label ID="Label2" runat="server" Text="Clave Catastral:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td colspan="1">
                                    <asp:TextBox ID="txtClvCastatral" runat="server" CssClass="textGrande" MaxLength="12" placeholder="Clave Catastral"></asp:TextBox>
                                    <%--<ajaxToolkit:MaskedEditExtender  runat="server" ID="meeFiltro" Mask="9999-99-999-999" MaskType="Number"  InputDirection="LeftToRight" TargetControlID="txtClvCastatral" />--%>
                                    <ajaxToolkit:MaskedEditExtender runat="server" ID="meeFiltro" Mask="9999-99-999-999" TargetControlID="txtClvCastatral" MaskType="Number" InputDirection="RightToLeft" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="valida" ControlToValidate="txtClvCastatral" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="guardar"></asp:RequiredFieldValidator>
                                </td>
                                <td colspan="1">
                                    <asp:ImageButton ID="imbBuscar" runat="server" CausesValidation="False" ToolTip="Buscar por Calve Catastral" ImageUrl="~/img/consultar.fw.png" OnClick="buscarClaveCatastral" />
                                </td>
                                <td style="text-align: right;">
                                    <asp:Label ID="Label3" runat="server" Text="Propietario:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td colspan="1">
                                    <asp:TextBox ID="txtPropietario" runat="server" CssClass="textGrande" ReadOnly="true" MaxLength="12" placeholder="Propietario"></asp:TextBox>
                                </td>
                                <td colspan="1">
                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ToolTip="Buscar por Propietario" ImageUrl="~/img/consultar.fw.png" OnClick="buscarPropietario" />
                                </td>
                            </tr>
                            <tr style="background-color: #b4b4b4">
                                <td colspan="1" style="text-align: right;">
                                    <asp:Label ID="Label15" runat="server" Text="Tipo Tramite:" CssClass="letraMediana"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlTipoTramite" runat="server" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="selectTipoTramite"></asp:DropDownList>
                                    <br />
                                </td>
                                <td colspan="1" style="text-align: right;">
                                    <asp:Label ID="Label6" runat="server" CssClass="letraMediana" Text="Cajero:"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtCajero" runat="server" CssClass="textMediano" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1">
                                    <asp:Label ID="Label5" runat="server" Text="Propietario" CssClass="letraMediana" Enabled="true"></asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:Label ID="txtContruyente" runat="server" Text="" CssClass="letraMediana" Enabled="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label8" runat="server" CssClass="letraMediana" Text="Ubicación:"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label9" runat="server" CssClass="letraMediana" Text="Calle:"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="txtCalle" runat="server" Text="" CssClass="letraMediana" Enabled="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label10" runat="server" CssClass="letraMediana" Text="Numero:"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="txtNumero" runat="server" Text="" CssClass="letraMediana" Enabled="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label11" runat="server" CssClass="letraMediana" Text="Colonia:"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="txtColonia" runat="server" Text="" CssClass="letraMediana" Enabled="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label12" runat="server" CssClass="letraMediana" Text="Codigo Postal:"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="txtCP" runat="server" Text="" CssClass="letraMediana" Enabled="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" CssClass="letraMediana" Text="Localidad:"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="txtLocalidad" runat="server" Text="" CssClass="letraMediana" Enabled="true"></asp:Label>
                                </td>
                            </tr>
                            <tr style="background-color: #b4b4b4">
                                <td colspan="6">
                                    <asp:Label ID="Label14" runat="server" Text="Detalles de Cobro" CssClass="letraMediana" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <br />
                                    <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False"
                                        CssClass="grd" DataKeyNames="id,IdTipoTramite" AllowPaging="True"
                                        AllowSorting="True" BorderStyle="None" ShowFooter="True"
                                        OnRowCommand="grd_RowCommand" OnSorting="grd_Sorting"
                                        OnPageIndexChanging="grd_PageIndexChanging" OnRowDataBound="grd_RowDataBound" PageSize="10">
                                        <Columns>
                                            <asp:BoundField DataField="IdTipoTramite" HeaderText="Clave Predial" SortExpression="IdTipoTramite" />
                                            <asp:BoundField DataField="id" HeaderText="Número Tramite" SortExpression="id" />
                                            <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="135px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgConsultaPago" runat="server" ToolTip="Consulta Pago"
                                                        ImageUrl="~/img/pagos.png"
                                                        CssClass="imgButtonGrid"
                                                        CommandName="ConsultaPago"
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmptySearchTramite" runat="server" CssClass="letraTituloEmptyGrid">No se encontraron resultados</asp:Label>
                                        </EmptyDataTemplate>
                                        <FooterStyle CssClass="grdFooter" />
                                        <HeaderStyle CssClass="grdHead" />
                                        <RowStyle CssClass="grdRowPar" />
                                    </asp:GridView>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:GridView ID="grdCobros" runat="server" AutoGenerateColumns="False"
                                        CssClass="grd" DataKeyNames="id" Visible="false"
                                        AllowSorting="True" BorderStyle="None" ShowFooter="True">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="TipoCobro" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTipoCobro" runat="server" Text='<%# Eval("TipoCobro") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Tramite" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIdTramite" runat="server" Text='<%# Eval("IdTramite") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="ConceptoP" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIdConceptoP" runat="server" Text='<%# Eval("IdConceptoP") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Mesa" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIdMesa" runat="server" Text='<%# Eval("IdMesa") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Conceptos">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblConcepto" runat="server" Text='<%# Eval("Concepto") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Costo">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCosto" runat="server" Text='<%# Eval("Costo") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPorcentaje" runat="server" Text='<%# Eval("Porcentaje") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Descuento">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDescuento" runat="server" Text='<%# Eval("Descuento") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Importe">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtImporte" runat="server" Text='<%# Eval("Importe") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmptySearch" runat="server" CssClass="letraTituloEmptyGrid">No se encontraron Conceptos de Cobro</asp:Label>
                                        </EmptyDataTemplate>
                                        <FooterStyle CssClass="grdFooter" />
                                        <HeaderStyle CssClass="grdHead" />
                                        <RowStyle CssClass="grdRowPar" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <%--<td></td>--%>
                                <td colspan="2">
                                    <asp:Label ID="lblMensajeConvenio" CssClass="grdFooter" Visible="false" ForeColor="Red" runat="server"></asp:Label>
                                </td>
                                <td colspan="1">
                                    <asp:Button ID="btnEstado" runat="server" CausesValidation="False" OnClick="btnEstado_Click" Text="Estado de Cuenta" Width="163px" Visible="false" />
                                </td>
                                <td colspan="1">
                                    <asp:Button ID="btnCobrar" runat="server" Text="Cobrar" OnClick="btnCobrar_Click" Visible="false" />
                                </td>
                                <td colspan="1">
                                    <asp:Button ID="btnRecalculo" runat="server" Text="Calculo" OnClick="btnRecalculo_Click" Visible="false" />
                                </td>
                                <td colspan="1" style="text-align: center">
                                    <asp:Label ID="Label16" runat="server" CssClass="textModalTitulo2" Text="TOTAL" Visible="false"></asp:Label>
                                    &nbsp;&nbsp;
                                    <asp:Label ID="lblTotal" runat="server" CssClass="textModalTitulo2" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            
            
             <asp:Panel ID="pnl" runat="server" class="formPanel" CssClass="formCobro">
                <table>
                    <tr style="background-color: #b4b4b4" >
                        <td colspan="6" style="height: 36px">
                            &nbsp;&nbsp;&nbsp;<asp:Label ID="lblTituloConceptoId" runat="server" CssClass="textModalTitulo2" Text="Cobros"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblImportePago" runat="server" CssClass="textModalTitulo2" Text="Importe:"></asp:Label>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="vertical-align:top; " >
                            <asp:Label ID="lblTipoCobro" runat="server" CssClass="letraMediana"></asp:Label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtTipoCobroSalario" runat="server" CssClass="textGrande" placeholder="Número de UMA's." style="margin-left: 64px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" CssClass="valida" ControlToValidate="txtTipoCobroSalario" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="agregarCobro"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="Ingresar solo números mayor a 0 y menor a 100" ControlToValidate="txtTipoCobroSalario" MinimumValue="0" MaximumValue="10" Display="Dynamic" ValidationGroup="agregarCobro" Type="Integer" Font-Size="XX-Small"></asp:RangeValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4"  style="vertical-align:top;">
                            <asp:Label ID="lblMetodoPago" runat="server" Text="Metodo de Pago:" Visible="False" CssClass="letraMediana"></asp:Label>

                            <asp:DropDownList ID="ddlMetodoPago" runat="server" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlMetodoPago_SelectedIndexChanged"></asp:DropDownList>
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
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtMonto" CssClass="valida"  Font-Size="XX-Small" SetFocusOnError="True" ErrorMessage="Número erroneo, máximo dos decimales." ValidationExpression="\d+(\.\d{1,2})?|\.\d{1,2}" ValidationGroup="validaCheque"></asp:RegularExpressionValidator>
                        </td>
                        <td colspan="2" style="vertical-align:top;">
                            <asp:Label ID="lblNumeroAprobacion" runat="server" CssClass="letraMediana" Text="No. de Operación:"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtNumeroAprobacion" runat="server" CssClass="textMedianoRight" MaxLength="10" placeholder="Número autorización"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtNumeroAprobacion" CssClass="valida" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="validaTarjeta"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtNumeroAprobacion" CssClass="valida" ErrorMessage="Solo números enteros" Font-Size="XX-Small" SetFocusOnError="True" ValidationExpression="^\d+$" ValidationGroup="validaTarjeta"></asp:RegularExpressionValidator>
                        </td>
                        <td colspan="2" style="vertical-align:top;">
                            <asp:Label ID="lblInstitucion" runat="server" CssClass="letraMediana" Text="Inst. Financiera"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtInstitucion" runat="server" CssClass="textMedianoRight" MaxLength="30" placeholder="Banco"></asp:TextBox>
                        </td>
                    </tr>
                   
                    <tr>
                        <td  style="vertical-align:top;"  colspan="6">
                          <asp:GridView ID="grdAlta" runat="server" DataKeyNames="Id,Clave" AutoGenerateColumns="false" CssClass="grdRowPar" style="margin-right: 0px"
                                AllowSorting="True" BorderStyle="None"  OnRowCommand="grdAlta_RowCommand" OnRowDataBound="grdAlta_RowDataBound" 
                               OnSorting="grdAlta_Sorting"  ShowFooter="false" Width="415px">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="135px" HeaderText="Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="135px" />
                                    </asp:TemplateField>  
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Clave" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClave" runat="server" Text='<%# Eval("Clave") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                         <ItemStyle Width="150px" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>                                      
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="3000px" HeaderText="Método" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTipoCobro" runat="server" Text='<%# Eval("TipoCobro") %>' Visible="true"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="200px" />
                                    </asp:TemplateField>                                   
                                   <asp:TemplateField HeaderStyle-HorizontalAlign="Left"  ItemStyle-Width="135px" Visible="true" HeaderText="Importe" InsertVisible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblImporte" runat="server" Visible="true" Text='<%# Eval("Importe") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate> 
                                            <asp:Label ID="lblTotal" runat="server" Text="0" CssClass="valida" Visible ="false"></asp:Label>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="200px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="250px" HeaderText="No. Transaccción" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAprobacion" runat="server" Text='<%# Eval("Transaccion") %>'></asp:Label>
                                        </ItemTemplate>                                      
                                        <HeaderStyle HorizontalAlign="Left" />                                       
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Institución" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInstitucion" runat="server" Text='<%# Eval("Institucion") %>'></asp:Label>
                                        </ItemTemplate>
                                         <ItemStyle Width="200px" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="135px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgDelete" runat="server" ToolTip="Eliminar método!"
                                                ImageUrl="~/img/eliminar1.png"
                                                CssClass="imgButtonGrid"
                                                CommandName="EliminarRegistro"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Clave")%>' />
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
                                         <ItemStyle Width="150px" />
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
                        <td style="vertical-align:top; width: 45px;" >
                            <asp:Label ID="lblObservacion" runat="server" CssClass="letraMediana" Text="Observaciones:"></asp:Label>
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtObservacion" TextMode="MultiLine" runat="server" CssClass="textGrande" MaxLength="150" placeholder="Observaciones" Height="40px" Width="406px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td  colspan="6" style="text-align: right; width: 45px; height: 37px;">
                          <asp:Label ID="lblCambio" runat="server" CssClass="textMediano" SetFocusOnError="True" Text="Datos fiscales" Font-Size="Small" Width="600px"></asp:Label>
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
                       <td style="text-align: right; width: 125px;">
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
            <br />


            <asp:Panel ID="pnlRecibo" runat="server" Width="1013px" Height="565px" BackColor="White">
            <div class="width:100%;margin:1px;" id="divCerrarFactura" visible="false">
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Img/eliminar.png" ImageAlign="Right" OnClick="ImageButton1_Click" />
            </div>
            <iframe id="frameRecibo" runat="server" src="" width="100%" height="90%" style="border: none;" />
            </asp:Panel>

            <ajaxToolkit:ModalPopupExtender ID="modalRecibo" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnlRecibo" TargetControlID="btnRecibo">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnRecibo" runat="server" />
            <%--<br />--%>

            <asp:Panel ID="pnlPropietario" runat="server" class="formPanel">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="height: 60px;">
                            <asp:Label ID="Label4" runat="server" Text="Propietarios" CssClass="letraTitulo"></asp:Label></td>
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
                                CssClass="grd" DataKeyNames="IdPredio" AllowPaging="True"
                                AllowSorting="True" BorderStyle="None" ShowFooter="True"
                                OnRowCommand="grdPropietarios_RowCommand" OnSorting="grdPropietarios_Sorting"
                                OnPageIndexChanging="grdPropietarios_PageIndexChanging" OnRowDataBound="grdPropietarios_RowDataBound" PageSize="5">
                                <Columns>
                                    <asp:BoundField DataField="ClavePredial" HeaderText="Clave Catastral" SortExpression="ClavePredial" />
                                    <asp:BoundField DataField="NombreCompleto" HeaderText="Propietario" SortExpression="NombreCompleto" />
                                    <asp:BoundField DataField="Domicilio" HeaderText="Dirección" SortExpression="Domicilio" />
                                    <asp:TemplateField ItemStyle-CssClass="" HeaderText="Herramientas" ItemStyle-Width="135px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgActivar" runat="server" ToolTip="Seleccionar"
                                                ImageUrl="~/img/Activar.png"
                                                CssClass="imgButtonGrid"
                                                CommandName="ActivarRegistro"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IdPredio")%>' />
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
            <uc1:ModalPopupMensaje ID="vtnModal" runat="server" DysplayAceptar="True" DysplayCancelar="True" />
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>


            <asp:Panel ID="pnlFactura" runat="server" class="formPanel">
                <table>
                    <tr>
                        <td style="width: 600px">
                            <asp:Label ID="Label7" runat="server" CssClass="textModalTitulo2" Text="Introduzca su RFC:"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 600px">
                            <asp:TextBox ID="txtRFCbuscar" MaxLength="13" runat="server" CssClass="textGrande"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*" ForeColor="OrangeRed" ControlToValidate="txtRFCbuscar" ValidationGroup="BuscarRFC"></asp:RequiredFieldValidator>
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
                            <asp:Button ID="Button2" runat="server" Text="Cancelar" OnClick="Button2_Click" />
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
                                        <asp:Label ID="Label13" runat="server" CssClass="letraSubTitulo" Text="RFC:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRFC" runat="server" MaxLength="13" CssClass="textMediano" AutoPostBack="True"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="Label17" runat="server" CssClass="letraSubTitulo" Text="Nombre:"></asp:Label>
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
                                <asp:Label ID="Label18" runat="server" CssClass="letraSubTitulo" Text="Calle:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCalleRFC" runat="server" MaxLength="100" CssClass="textGrande"></asp:TextBox>

                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfv1" ControlToValidate="txtCalleRFC" runat="server" ErrorMessage="*" ForeColor="OrangeRed" ValidationGroup="Registro">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label ID="Label19" runat="server" CssClass="letraSubTitulo" Text="Municipio:"></asp:Label>
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
                                <asp:Label ID="Label20" runat="server" CssClass="letraSubTitulo" Text="Estado:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEstado" MaxLength="50" runat="server" CssClass="textGrande"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfv2" runat="server" ControlToValidate="txtEstado" ErrorMessage="*" ForeColor="OrangeRed" ValidationGroup="Registro">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label ID="Label21" runat="server" CssClass="letraSubTitulo" Text="Pais:"></asp:Label>
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
                                <asp:Label ID="Label22" runat="server" CssClass="letraSubTitulo" Text="CP:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCPRFC" runat="server" MaxLength="5" CssClass="textMediano"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfv3" ControlToValidate="txtCPRFC" runat="server" ErrorMessage="*" ForeColor="OrangeRed" ValidationGroup="Registro">
                                </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator98" runat="server"
                                    ControlToValidate="txtCPRFC" CssClass="mensajeValidador"
                                    ErrorMessage="Sólo números" ValidationExpression="\d+"
                                    ForeColor="OrangeRed"
                                    SetFocusOnError="True"
                                    ValidationGroup="Registro"></asp:RegularExpressionValidator>
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Label ID="Label23" runat="server" CssClass="letraSubTitulo" Text="Colonia:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtColoniaRFC" MaxLength="100" runat="server" CssClass="textGrande"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfv13" runat="server" ControlToValidate="txtColoniaRFC" ErrorMessage="*" ForeColor="OrangeRed" ValidationGroup="Registro">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20px;"></td>
                            <td>
                                <asp:Label ID="Label24" runat="server" CssClass="letraSubTitulo" Text="No. Exterior:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNoExt" runat="server" CssClass="textMediano" MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfv10" runat="server" ControlToValidate="txtNoExt" ErrorMessage="*" ForeColor="OrangeRed" ValidationGroup="Registro">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Label ID="Label25" runat="server" CssClass="letraSubTitulo" Text="Localidad:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLocalidadRFC" MaxLength="100" runat="server" CssClass="textGrande"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 20px;"></td>
                            <td>
                                <asp:Label ID="Label26" runat="server" CssClass="letraSubTitulo" Text="No. Interior"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNoInt" runat="server" CssClass="textMediano" MaxLength="10"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Label ID="Label27" runat="server" CssClass="letraSubTitulo" Text="Referencia:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferencia" MaxLength="100" runat="server" CssClass="textGrande"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                        </tr>--%>
                                <tr>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <asp:Label ID="Label28" runat="server" CssClass="letraSubTitulo" Text="Correo Electronico:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtCorreoReg" MaxLength="100" runat="server" CssClass="textGrande"></asp:TextBox>
                                        <br />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtCorreoReg"
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
                            <asp:Label ID="Label19" runat="server" CssClass="textModalTitulo2" Text="DIRECCIÓN DE CORREO."></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 543px">
                            <asp:TextBox ID="txtCorreoEnvio" MaxLength="100" runat="server" CssClass="textGrande"></asp:TextBox>
                            <br />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtCorreoEnvio"
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

            <%--<input type="button" id="printreport" runat="server" value="imprimir" visible="false" onclick="printReport('<%=rpt.ClientID %>')" />--%>
            <asp:Panel ID="pnlReport" runat="server" Visible="false">
                <div class="row">
                    <div class="col-md-12">
                        <input type="button" id="printreport" value="imprimir" onclick="printReport('<%=rpt.ClientID %>    ')" />
                    </div>
                </div>
            </asp:Panel>
            <rsweb:ReportViewer ID="rpt" runat="server" Height="500px" Width="800px" ShowPrintButton="true"></rsweb:ReportViewer>

        </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnDescargaPDF" />
            <asp:PostBackTrigger ControlID="btnDescargaXML" />--%>
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
