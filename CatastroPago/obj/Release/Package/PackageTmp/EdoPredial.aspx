<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EdoPredial.aspx.cs" Inherits="CatastroPago.EdoPredial" %>

<%@ Register Src="~/Controles/ModalPopupMensaje.ascx" TagPrefix="uc1" TagName="ModalPopupMensaje" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
                <div style="text-align: center"><asp:Image ID="ImagenLogo" runat="server" ImageUrl="~/Img/logoZapata.jpg" Height="102px" Width="318px"/></div>
                <div id="barra" style="height:12px; width:100%; background-color: <%=this.colorDiv%>; text-align: center;"> </div>
                <br />
                <h1 ><span style="color: #660066">Estado de Cuenta</span> </h1>
                <%--<asp:Label ID="Label2" runat="server" CssClass="textModalTitulo2" Text="Impuesto Predial y Servicios Municipales"></asp:Label>--%>
                <br />
                <%--<div id="divBusqueda" runat="server">
                    <div class="row">
                        <div class="col-md-1">
                            <asp:Label ID="lblBPredial" runat="server" Text="Clave Predial:" CssClass="letraMediana"></asp:Label>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtBPredial" runat="server"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender  runat="server" ID="meeFiltro" Mask="9999-99-999-999" MaskType="Number"  InputDirection="LeftToRight" TargetControlID="txtBPredial" />
                        </div>
                        <div class="col-md-1">
                            <asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" Text="Buscar" />
                        </div>

                    </div>

                </div>--%>
                <br />
                <div class="row">
                    <div class="form-group">
                        <div class="col-md-9">
                            <div class="col-md-8">
                            </div>
                            <div class="col-md-4">
                                <asp:Label ID="lblTVigencia" runat="server" Text="Vigencia" CssClass="letraSubTitulo"></asp:Label>
                            </div>
                            <div class="col-md-8">
                            </div>
                            <div class="col-md-4">
                                <asp:Label ID="lblVigencia" runat="server" CssClass="letraSubTitulo" ></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="col-md-12">
                                <asp:Label ID="lblTSaldo" runat="server" Text="Saldo" CssClass="letraSubTitulo"></asp:Label>
                            </div>

                            <div class="col-md-12">
                                <asp:Label ID="lblSaldo" runat="server" CssClass="letraSubTitulo" ></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="form-group">
                        <div class="col-md-12" style="text-align: center;  ">
                            <asp:Label ID="lblTDatos" runat="server" Text="DATOS DEL PREDIO" CssClass="letraSubTitulo"></asp:Label>
                        </div>
                    </div>
                </div>

                <div id="divPredial" runat="server">
                    <div class="form-group">

                        <div class="row">

                            <div class="col-md-2">
                                <asp:Label ID="lblTClave" runat="server" Text="Clave Catastral" CssClass="letraMediana"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:Label ID="lblClave" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblTSuperficie" runat="server" Text="Superficie del Predio" CssClass="letraMediana"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:Label ID="lblSuperficie" runat="server" Text=""></asp:Label>
                            </div>
                        </div>

                        <div class="row">

                            <div class="col-md-2">
                                <asp:Label ID="lblTPropietario" runat="server" Text="Propietario" CssClass="letraMediana"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:Label ID="lblPropietario" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblTValor" runat="server" Text="Valor del Terreno" CssClass="letraMediana"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:Label ID="lblValor" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">

                            <div class="col-md-2">
                                <asp:Label ID="lblTDireccion" runat="server" Text="Dirección del Predio" CssClass="letraMediana"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:Label ID="lblDirecion" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblTConstruida" runat="server" Text="Superficie Construída" CssClass="letraMediana"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:Label ID="lblConstruida" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">

                            <div class="col-md-2">
                                <asp:Label ID="lblTBase" runat="server" Text="Base Gravable" CssClass="letraMediana"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:Label ID="lblBase" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblTConstruccion" runat="server" Text="Valor de la Construcción" CssClass="letraMediana"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:Label ID="lblConstruccion" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">

                            <div class="col-md-2">
                                <asp:Label ID="lblTZona" runat="server" Text="Zona" CssClass="letraMediana"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:Label ID="lblZona" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblTMetros" runat="server" Text="Metros de Frente" CssClass="letraMediana"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:Label ID="lblMetros" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="row">
                                    <div class="col-md-12" style="background-color: #EFBEB1">
                                        <asp:Label ID="lblIP" runat="server" CssClass="letraSubTitulo" Text="IMPUESTO PREDIAL"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblPeriodo" runat="server" CssClass="letraMediana" Text="Periodo a pagar"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="txtPeriodo" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblImpuestoAnt" runat="server" CssClass="letraMediana" Text="Impuesto Anticipado"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="txtImpuestoAnt" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblAdicionalAnt" runat="server" CssClass="letraMediana" Text="Adicional Anticipado"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="txtAdicionalAnt" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblImpuesto" runat="server" CssClass="letraMediana" Text="Impuesto"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="txtImpuesto" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblDiferencias" runat="server" CssClass="letraMediana" Text="Diferencias"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="txtDiferencias" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblRecargoDiferencias" runat="server" CssClass="letraMediana" Text="Recargo Diferencias"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="txtRecargosDif" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblAdicional" runat="server" CssClass="letraMediana" Text="Adicional"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="txtAdicional1" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblRezagos" runat="server" CssClass="letraMediana" Text="Rezagos"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="txtRezagos" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblRecargos" runat="server" CssClass="letraMediana" Text="Recargos"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="txtRecargos" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblGastosEj" runat="server" CssClass="letraMediana" Text="Honorarios/ Ejecución"></asp:Label>
                                    </div>
                                    <div class="col-md-8">

                                        <asp:Label ID="txtEjecucion" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>

                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">

                                        <asp:Label ID="lblMultas" runat="server" CssClass="letraMediana" Text="Multas"></asp:Label>

                                    </div>
                                    <div class="col-md-8">

                                        <asp:Label ID="txtMultas" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>

                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">

                                        <asp:Label ID="lblDescuentos" runat="server" CssClass="letraMediana" Text="Descuentos"></asp:Label>

                                    </div>
                                    <div class="col-md-8">

                                        <asp:Label ID="txtDescuentos" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>

                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">

                                        <asp:Label ID="lblTotal" runat="server" CssClass="letraMediana letraGrande" Text="IMPORTE TOTAL:"></asp:Label>

                                    </div>
                                    <div class="col-md-8">

                                        <asp:Label ID="txtTotalIp" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>

                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="row">
                                    <div class="col-md-12" style="background-color:#EFBEB1">
                                        <asp:Label ID="lblSM" runat="server" visible="true" CssClass="letraSubTitulo" Text="-"></asp:Label>
                                    </div>
                                </div>
                                 <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblPeriodoSM"  visible="false" runat="server" CssClass="letraMediana" Text="Periodo a pagar"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="txtPeriodoSM"  visible="false" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblInfraestructuraAntSm" visible="false" runat="server" CssClass="letraMediana" Text="Infraestructura Anticipada"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="txtInfraestructuraAntSm"  visible="false" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblAdicionalAntSm"  visible="false" runat="server" CssClass="letraMediana" Text="Adicional Anticipado"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="txtAdicionalAntSm"  visible="false" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblInfraestructuraAnt"  visible="false" runat="server" CssClass="letraMediana" Text="Infraestructura"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="txtInfraestructuraSm"  visible="false" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblRecoleccionSm"  visible="false" runat="server" CssClass="letraMediana" Text="Recolección De Residuos"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="txtRecoleccionSm"  visible="false" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblLimpiezaSm"  visible="false" runat="server" CssClass="letraMediana" Text="Limpieza De Frente Baldio"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="txtLimpiezaSm"  visible="false" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblDapSm"  visible="false" runat="server" CssClass="letraMediana" Text="DAP"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="txtDapSm"  visible="false" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblAdicionalSm"  visible="false" runat="server" CssClass="letraMediana" Text="Adicional"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="txtAdicionalSm"  visible="false" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblRezagosSm" visible="false" runat="server" CssClass="letraMediana" Text="Rezagos"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="txtRezagosSm"  visible="false" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblRecargosSm"  visible="false" runat="server" CssClass="letraMediana" Text="Recargos"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="txtRecargosSm"  visible="false" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblGastosEjSm" visible="false" runat="server" CssClass="letraMediana" Text="Honorarios / Ejecución"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="txtEjecucionSm"  visible="false" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">

                                        <asp:Label ID="lblMultasSm"  visible="false" runat="server" CssClass="letraMediana" Text="Multas"></asp:Label>

                                    </div>
                                    <div class="col-md-8">

                                        <asp:Label ID="txtMultasSm"  visible="false" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>

                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">

                                        <asp:Label ID="lblDescuentosSm"  visible="false" runat="server" CssClass="letraMediana" Text="Descuentos"></asp:Label>

                                    </div>
                                    <div class="col-md-8">

                                        <asp:Label ID="txtDescuentosSm"  visible="false" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>

                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblTotalSm" visible="false"  runat="server" CssClass="letraMediana letraGrande" Text="Subtotal"></asp:Label>
                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="txtTotalSm"  visible="false" runat="server" CssClass="textMediano textNoBorder" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">

                                    <div class="col-md-4" style="text-align: right;">

                                        <asp:Label ID="lblImporteTotal" visible="false" runat="server" CssClass="letraSubTitulo" Text="IMPORTE TOTAL:"></asp:Label>

                                    </div>
                                    <div class="col-md-8">
                                        <asp:Label ID="lblImporte" visible="false" runat="server" CssClass="letraSubTitulo" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>



                    </div>
                    <br />
                    <br />
                    <div class="row">
                        <asp:Label ID="Label1" visible="false" server" CssClass="letraSubTitulo" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True" style="color: #CC0000">Importante: Al realizar su pago en línea deberá de esperar a generar su recibo bancario y SU RECIBO OFICIAL EMITIDO POR EL AYUNTAMIENTO!!! ( Favor de no cerrar su navegador hasta que se emita su recibo oficial, este puede tardar dependiendo de su velocidad de internet)</asp:Label>
                    </div>                    
                    <br />
                    <div class="row">
                        <div class="col-md-12">
                            <div class="col-md-3">
                                <asp:Label ID="lblLeyenda" runat="server" CssClass="letraSubTitulo" Enabled="False" MaxLength="15" placeholder="0.00" ReadOnly="True"></asp:Label>
                            </div>                            
                            <div class="col-md-8">
                                <div class="col-md-6">
                                   <asp:Button ID="btnConsulta" runat="server" OnClick="btnConsulta_Click" Text="Nueva Consulta" Width="159px" />
                                </div>
                                
                                <div class="col-md-6" style="text-align: left;">
                                     <asp:Button ID="btnPagar" runat="server" Text="Pagar" OnClick="btnPagar_Click" Width="159px" ToolTip="No cierre su Navegador hasta que sea generado sus recibo oficial del Ayuntamiento, una vez realizado su pago bancario" Enabled="False" />
                                     
                                </div>
                            </div>
                             
                        </div>
                    </div>

                    <asp:HiddenField ID="hfImporteTotal" runat="server" />

                    <br />
                    <br />

                </div>
            </div>

            <br />
            <br />
             <br />
             <br />
            <uc1:ModalPopupMensaje runat="server" ID="vtnModal" DysplayAceptar="True" DysplayCancelar="False" />
        </ContentTemplate>
    </asp:UpdatePanel>   
</asp:Content>
