<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BandejaEntradaProyecto.aspx.cs" Inherits="foSistemas.BandejaEntradaProyecto" %>

<%@ Register Src="controles/paginacion.ascx" TagName="paginacion" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-1.10.2.min.js"></script>
    <script src="Scripts/jquery-ui-1.10.4.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <link href="Content/Sistemas.css?v1.1" rel="stylesheet" />
    <script src="Scripts/BandejaEntradaProyecto.js?v0.12"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdId" runat="server" />
    <asp:HiddenField ID="hdOrdenamiento" runat="server" />
    <asp:HiddenField ID="hdAscendenteDescendente" runat="server" />
    <asp:HiddenField ID="hdPaginaActual" runat="server" />
    <asp:HiddenField ID="hdVerPagina" runat="server" />
    <asp:HiddenField ID="hdInicio" runat="server" />
    <asp:HiddenField ID="hdExpiroSesion" runat="server" />
    <asp:HiddenField ID="hdOrganigramaId" runat="server" Value="0" />

    <div class="col-lg-12">
        <div id="divError" runat="server" style="display: none" class="alert alert-danger" role="alert">
            <asp:Label ID="lblTituloError" runat="server" Text="" CssClass="alert-link"></asp:Label>
            <asp:Label ID="lblResultadoError" runat="server" Text="" CssClass="alert-link"></asp:Label>
        </div>
        <div id="divResultado" runat="server" style="display: none" role="alert">
            <div id="divResultadoEstilo" runat="server">
                <asp:Label ID="lblResultado" runat="server" Text="" CssClass="alert-link"></asp:Label>
            </div>
        </div>
    </div>
    <div id="Principal" runat="server" class="row">
        <div class="col-lg-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <span class="panel-title"><b>Bandeja de Entrada Proyectos</b></span>
                </div>
                <div id="divMisSolicitudes" class="MisSolicitudesChecked">
                    <asp:CheckBox ID="chbMisSolicitudes" runat="server" Text="Mis Solicitudes&nbsp" TextAlign="Left" Width="148px" />
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h3 class="panel-title"><i class="fa fa-fw fa-filter fa"></i><b>Filtros de Búsqueda</b></h3>
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-lg-3">
                                            <label>Nombre</label>

                                        </div>
                                        <div class="col-lg-3">
                                            <label>Team Id</label>

                                        </div>
                                        <div class="col-lg-1">
                                            <label>Estado</label>
                                        </div>
                                        <div class="col-lg-2" style="padding-top: 6px">
                                            <asp:CheckBox ID="chbInactivos" runat="server" Text="Inactivos" TextAlign="Right" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-3">
                                            <asp:TextBox class="form-control" ID="txtNombre" runat="server" Style="display: inline; width: 75%; margin-left: 10px"></asp:TextBox>
                                        </div>
                                        <div class="col-lg-3">
                                            <asp:TextBox class="form-control" ID="txtTFId" runat="server" Style="display: inline; width: 75%; margin-left: 10px"></asp:TextBox>
                                        </div>
                                        <div class="col-lg-3">
                                            <asp:DropDownList ID="ddlEstado" runat="server" class="form-control">
                                                <asp:ListItem Value="0" Text="Seleccionar.."></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Iniciado"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Diseñando"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Desarrollando"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="Implementado"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="Por Verificar"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="Verificado"></asp:ListItem>
                                                <asp:ListItem Value="7" Text="Rechazado"></asp:ListItem>
                                                <asp:ListItem Value="8" Text="Modificado"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div id="Botones" style="margin-top: 15px;">
                                        <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" Width="100px" PostBackUrl="~/Proyecto.aspx" CssClass="btn btn-primary" />
                                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btnBuscar_Click" OnClientClick="return obtenerDatosCombos();" CssClass="btn btn-primary" />
                                        <asp:Button ID="btnLimpiarFiltros" runat="server" Text="Limpiar Filtros" Width="124px" OnClientClick="return limpiarFiltros();" CssClass="btn btn-primary" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-12">
                        <div class="col-lg-12" style="text-align: right; padding: 10px; background-color: #eee">
                            <label id="lbl" visible="false" runat="server" style="width: 60px; height: 26px; margin-right: 10px">Ordenar</label>
                            <asp:DropDownList ID="ddlOrdenar" runat="server" class="form-control" Style="width: 15%; display: inline"
                                OnSelectedIndexChanged="ddlOrdenar_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="Nombre" Text="Nombre"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlAscendente" runat="server" class="form-control" Style="width: 15%; display: inline"
                                OnSelectedIndexChanged="ddlAscendente_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="ASC" Text="Ascendente"></asp:ListItem>
                                <asp:ListItem Value="DESC" Text="Descendente"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="list-group" id="divBandejaEntrada" runat="server">
                        </div>
                        <uc1:paginacion ID="controlPaginacion" runat="server" />
                    </div>
                </div>
                <div class="modal fade" id="divAdvertencia" role="dialog">
                    <div class="modal-dialog">
                        <div class="modal-content" style="width: 393px; margin-left: 155px; margin-top: 240px; height: auto">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title">Advertencia</h4>
                            </div>
                            <div class="modal-body" style="height: 45px">
                                <p>Estás eliminando esta Proyecto, ¿confirmas tu decisión?</p>
                            </div>
                            <div class="modal-footer">
                                <asp:Button runat="server" Text="Si" Width="100px" OnClick="btnEliminar_Click" CssClass="btn btn-primary" />
                                <asp:Button runat="server" Text="No" Width="100px" OnClientClick="return ocultarModal();" CssClass="btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
