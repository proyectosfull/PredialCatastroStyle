<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdministrarRecibos.aspx.cs" Inherits="Catastro.Recibos.AdministrarRecibos" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function printPdf() {
            var PDF = document.getElementById("MainContent_frameRecibo");
            PDF.focus();
            PDF.contentWindow.print();
        }
    </script>
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 100%;">
                <tr>
                    <td colspan="7" style="height: 60px;">
                        <asp:Label ID="lblTitulo" runat="server" Text="Administración de Recibos" CssClass="letraTitulo"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="7" style="height: 10px">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>Estado:</td>
                    <td>
                        <asp:DropDownList ID="ddlEstado" runat="server">
                            <asp:ListItem Value="0">Todos</asp:ListItem>
                            <asp:ListItem Value="P">Recibos Pagados</asp:ListItem>
                            <asp:ListItem Value="C">Recibos Cancelados</asp:ListItem>
                            <%--  <asp:ListItem Value="FP">CFDI Pagados</asp:ListItem>
                            <asp:ListItem Value="FC">CFDI Cancelados</asp:ListItem>--%>
                        </asp:DropDownList>
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="height: 52px"></td>
                    <td style="height: 52px">Tipo de busqueda:&nbsp;&nbsp;&nbsp;</td>
                    <td style="height: 52px">
                        <asp:DropDownList ID="ddlTipoBusqueda" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoBusqueda_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="0">Seleccione una opción</asp:ListItem>
                            <asp:ListItem Value="1">No. Recibo</asp:ListItem>
                            <asp:ListItem Value="2">Entre Fechas</asp:ListItem>
                            <asp:ListItem Value="3">Clave Catastral</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="height: 52px">&nbsp;<asp:TextBox ID="txtBusqueda" runat="server"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender runat="server" ID="meeFiltro" Mask="9999-99-999-999" TargetControlID="txtBusqueda" MaskType="Number" InputDirection="RightToLeft" Enabled="false" />
                        <asp:RequiredFieldValidator ID="rfvBusqueda" runat="server" ControlToValidate="txtBusqueda" CssClass="valida" ErrorMessage="*" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textMediano" placeholder="Fecha Infraccion." Visible="False"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFechaInicio" runat="server" ControlToValidate="txtFechaInicio" CssClass="valida" ErrorMessage="*" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                        <ajaxToolkit:MaskedEditExtender ID="txtFechaInicio_MaskedEditExtender" runat="server" BehaviorID="txtFechaInfra_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaInicio" />
                        <ajaxToolkit:CalendarExtender ID="txtFechaInicio_CalendarExtender" runat="server" BehaviorID="txtFechaInfra_CalendarExtender" CssClass="CalendarCSS" Format="dd/MM/yyyy" TargetControlID="txtFechaInicio" />
                        <asp:TextBox ID="txtFechaFin" runat="server" CssClass="textMediano" placeholder="Fecha Infraccion." Visible="False"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender ID="txtFechaFin_MaskedEditExtender" runat="server" BehaviorID="txtFechaFin_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaFin" />
                        <asp:RequiredFieldValidator ID="rfvFechaFin" runat="server" ControlToValidate="txtFechaFin" CssClass="valida" ErrorMessage="*" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="txtFechaInfra_CalendarExtender" CssClass="CalendarCSS" Format="dd/MM/yyyy" TargetControlID="txtFechaFin" />
                    </td>
                    <td style="height: 52px">
                        <asp:ImageButton ID="imbBuscar" runat="server" ImageUrl="~/img/consultar.fw.png" OnClick="imbBuscar_Click" ValidationGroup="buscar" />
                    </td>
                    <td style="height: 52px">&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>

                    <td colspan="4">
                        <asp:GridView ID="grdRecibo" runat="server" AllowPaging="True" AutoGenerateColumns="False" BorderStyle="None" CssClass="grd" DataKeyNames="id,EstadoRecibo,Tipo" OnRowCommand="grdRecibo_RowCommand" ShowFooter="True" OnRowDataBound="grdRecibo_RowDataBound" OnPageIndexChanging="grdRecibo_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="No. Recibo" SortExpression="Id" />
                                <asp:BoundField DataField="ClavePredial" HeaderText="Clave Catastral" SortExpression="ClavePredial" />
                                <asp:BoundField DataField="FechaPago" HeaderText="Fecha Pago" SortExpression="FechaPago" />
                                <asp:BoundField DataField="EstadoRecibo" HeaderText="Estado" SortExpression="EstadoRecibo" />
                                <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
                                <asp:BoundField DataField="Contribuyente" HeaderText="Contribuyente" SortExpression="Contribuyente" />
                                <asp:BoundField DataField="ImportePagado" HeaderText="Importe" SortExpression="ImportePagado" />
                                <asp:BoundField DataField="UsuarioCobra" HeaderText="Usuario Cobra" SortExpression="UsuarioCobra" />
                                <asp:BoundField DataField="UsuarioCancela" HeaderText="Usuario Cancela" SortExpression="UsuarioCancela" />
                                <asp:TemplateField HeaderText="Herramientas" ItemStyle-CssClass="" ItemStyle-Width="180px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgConsulta" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' CommandName="ConsultarRegistro" CssClass="imgButtonGrid" ImageUrl="~/img/consultar.fw.png" ToolTip="Consultar!" />
                                        <asp:ImageButton ID="imgDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' CommandName="EliminarRegistro" CssClass="imgButtonGrid" ImageUrl="~/img/eliminar.png" OnClientClick="return Alert_Confirmar(this.id,'Está seguro que quiere cancelar la Infraccion', 'Eliminación cancelada.');" ToolTip="Cancelar!" />
                                        <%--<asp:ImageButton ID="imgTipoPago" Height="35px" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' CommandName="TipoPago" CssClass="imgButtonGrid" ImageUrl="~/img/pagos.png" ToolTip="Tipo de Pago" />--%>
                                        <asp:ImageButton ID="imgFacturaPendiente" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' CommandName="FacturarPendiente" CssClass="imgButtonGrid" ImageUrl="~/img/cerrarCaja.png" OnClientClick="return Alert_Confirmar(this.id,'Está seguro que quiere facturar el recibo', 'Facturación pendiente.');" ToolTip="Facturar!" />
                                    </ItemTemplate>
                                    <ItemStyle Width="180px" />
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
                    <td></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>

                </tr>
            </table>
            <asp:Panel ID="pnl" runat="server" class="formPanel">
                <table>
                    <tr>
                        <td style="width: 543px">
                            <asp:Label ID="lbl_titulo" runat="server" CssClass="textModalTitulo2" Text="Cancelar Recibo"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 543px">
                            <asp:Label ID="lblMotivo" runat="server" Text="Motivo:" CssClass="letraMediana"></asp:Label>
                            <asp:TextBox ID="txtMotivo" runat="server" CssClass="textMultiExtraGrande" MaxLength="200" placeholder="Descripción." TextMode="MultiLine"></asp:TextBox>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="valida" ControlToValidate="txtMotivo" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="cancela"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 543px">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 543px">
                            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" ValidationGroup="cancela" OnClick="btnAceptar_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnCancelar" runat="server" CausesValidation="False" Text="Cancelar" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="pnlCancelar" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnl" TargetControlID="btnPnl">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnPnl" runat="server" />

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

            <asp:Panel ID="pnlFactura" runat="server" class="formPanel">
                <table>
                    <tr>
                        <td style="width: 600px">
                            <asp:Label ID="Label1" runat="server" CssClass="textModalTitulo2" Text="Introduzca su RFC:"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 600px">
                            <asp:TextBox ID="txtRFCbuscar" MaxLength="13" runat="server" CssClass="textGrande"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ForeColor="OrangeRed" ControlToValidate="txtRFCbuscar" ValidationGroup="BuscarRFC"></asp:RequiredFieldValidator>
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
                                        <asp:Label ID="Label2" runat="server" CssClass="letraSubTitulo" Text="RFC:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRFC" runat="server" MaxLength="13" CssClass="textMediano" AutoPostBack="True"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" CssClass="letraSubTitulo" Text="Nombre:"></asp:Label>
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
                                        <asp:Label ID="Label4" runat="server" CssClass="letraSubTitulo" Text="Calle:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCalle" runat="server" MaxLength="100" CssClass="textGrande"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rfv1" ControlToValidate="txtCalle" runat="server" ErrorMessage="*" ForeColor="OrangeRed" ValidationGroup="Registro">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" CssClass="letraSubTitulo" Text="Municipio:"></asp:Label>
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
                                        <asp:Label ID="Label5" runat="server" CssClass="letraSubTitulo" Text="Estado:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEstado" MaxLength="50" runat="server" CssClass="textGrande"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rfv2" runat="server" ControlToValidate="txtEstado" ErrorMessage="*" ForeColor="OrangeRed" ValidationGroup="Registro">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" CssClass="letraSubTitulo" Text="Pais:"></asp:Label>
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
                                        <asp:Label ID="Label8" runat="server" CssClass="letraSubTitulo" Text="CP:"></asp:Label>
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
                                        <asp:Label ID="Label10" runat="server" CssClass="letraSubTitulo" Text="Colonia:"></asp:Label>
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
                                        <asp:Label ID="Label11" runat="server" CssClass="letraSubTitulo" Text="Localidad:"></asp:Label>
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
                                        <asp:Label ID="Label12" runat="server" CssClass="letraSubTitulo" Text="Referencia:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtReferencia" MaxLength="100" runat="server" CssClass="textGrande"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>--%>
                                <tr>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" CssClass="letraSubTitulo" Text="Correo Electronico:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtCorreoReg" MaxLength="100" runat="server" CssClass="textGrande"></asp:TextBox>
                                        <br />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtCorreoReg"
                                            CssClass="mensajeValidador" ErrorMessage="DIRECCIÓN DE CORREO INVALIDA." ForeColor="OrangeRed"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="Registro"></asp:RegularExpressionValidator>

                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="Label16" runat="server" CssClass="letraSubTitulo" Text="Uso CFDI:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlUsuCFDI" runat="server" Width="350px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlUsuCFDI" ValidationGroup="factura" InitialValue="" CssClass="valida" ErrorMessage="*"></asp:RequiredFieldValidator>
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
                            <asp:Label ID="Label15" runat="server" CssClass="textModalTitulo2" Text="DIRECCIÓN DE CORREO."></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 543px">
                            <asp:TextBox ID="txtCorreoEnvio" MaxLength="100" runat="server" CssClass="textGrande"></asp:TextBox>
                            <br />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCorreoEnvio"
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

            <asp:Panel ID="pnlTipoPago" runat="server" class="formPanel">
                <table>
                    <tr>
                        <td style="width: 543px">
                            <asp:Label ID="Label4" runat="server" CssClass="textModalTitulo2" Text="Tipo de Pago."></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 543px">
                            <asp:DropDownList ID="ddlTipoPago" runat="server" Width="350px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvTipoPago" runat="server" ControlToValidate="ddlTipoPago" ValidationGroup="tipoPago" InitialValue="" CssClass="valida" ErrorMessage="*"></asp:RequiredFieldValidator>                                          
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 543px">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 543px">
                            <asp:Button ID="btnGuardarTipoPago" runat="server" Text="Guardar" ValidationGroup="tipoPago" OnClick="btnGuardarTipoPago_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnCancelarTipoPago" runat="server" CausesValidation="False" Text="Cancelar" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="modalTipoPago" runat="server" BackgroundCssClass="ventanaBackground"
                PopupControlID="pnlTipoPago" TargetControlID="btnPnlTipoPago">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HiddenField ID="btnPnlTipoPago" runat="server" />



            <uc1:ModalPopupMensaje runat="server" ID="vtnModal" />
        </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnDescargaPDF" />
            <asp:PostBackTrigger ControlID="btnDescargaXML" />--%>
        </Triggers>
    </asp:UpdatePanel>


</asp:Content>
