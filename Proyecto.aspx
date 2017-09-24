<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Proyecto.aspx.cs" Inherits="foSistemas.Proyecto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-1.10.2.min.js"></script>
    <script src="Scripts/jquery-ui-1.10.4.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <link href="Content/datepicker.css" rel="stylesheet" />
    <link href="Content/Sistemas.css?v1.1" rel="stylesheet" />
    <script src="Scripts/Proyecto.js?v3"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdId" runat="server" />
    <asp:HiddenField ID="hdVersion" runat="server" />
    <asp:HiddenField ID="hdEstadoId" runat="server" />
    <asp:HiddenField ID="hdCoordinadorId" runat="server" />
    <asp:HiddenField ID="hdResponsableId" runat="server" />
    <asp:HiddenField ID="hdUsuarioId" runat="server" />
    <asp:HiddenField ID="hdPanelPadre" runat="server" />
    <asp:HiddenField ID="hdExpiroSesion" runat="server" />
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
    <div id="divPrincipal" class="row" runat="server">
        <div class="col-lg-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <span class="panel-title"><b>Proyectos </b></span>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <span class="panel-title"><b>Datos </b></span>
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-lg-10">
                                            <div class="row">
                                                <div class="col-lg-2">
                                                    <label style="height: 26px; margin-top: 10px">Nombre</label>
                                                    <label style="width: 1px; height: 26px; color: red;">*</label>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:TextBox class="form-control" ID="txtNombre" Style="display: inline; width: 100%;" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-lg-2">
                                                    <label style="height: 26px; margin-top: 10px">Team Fundation</label>
                                                    <label style="width: 1px; height: 26px; color: red;">*</label>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:TextBox class="form-control" ID="txtTFId" Style="display: inline; width: 100%;" runat="server"></asp:TextBox>
                                                </div>

                                            </div>

                                            <div class="row">
                                                <div class="col-lg-2">
                                                    <label style="height: 26px; margin-top: 10px">Coordinador</label>
                                                    <label style="width: 1px; height: 26px; color: red;">*</label>
                                                </div>
                                                <div class="col-lg-3">
                                                    <select class="form-control" id="ddlCoordinador" style="display: inline; width: 100%;">
                                                    </select>
                                                </div>
                                                <div class="col-lg-2">
                                                    <label style="height: 26px; margin-top: 10px">Responsable</label>
                                                    <label style="width: 1px; height: 26px; color: red;">*</label>
                                                </div>
                                                <div class="col-lg-3">
                                                    <select class="form-control" id="ddlResponsable" style="display: inline; width: 100%;">
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-2">
                                                    <label style="height: 26px; margin-top: 10px">Estado</label>
                                                    <label style="width: 1px; height: 26px; color: red;" id="ast5">*</label>
                                                </div>
                                                <div class="col-lg-3">
                                                    <select class="form-control" id="ddlEstado" style="display: inline; width: 100%;">
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h1 class="panel-title"><b>Duración</b></h1>
                                </div>
                                <div class="panel-body">

                                    <div class="row">
                                        <label style="width: 50px; height: 26px;">Inicia</label><label style="color: red; width: 10px; height: 26px; margin-right: 15px">*</label>
                                        <asp:TextBox ID="txtFechaInicio" runat="server" class="form-control" AutoCompleteType="Disabled" Style="width: 18%; display: inline; margin-right: 10px"></asp:TextBox>
                                        <label style="width: 60px; height: 26px;">Finaliza</label><label style="color: red; width: 10px; height: 26px; margin-right: 15px">*</label>
                                        <asp:TextBox ID="txtFechaFin" runat="server" class="form-control" AutoCompleteType="Disabled" Style="width: 18%; display: inline; margin-right: 10px"></asp:TextBox><br />
                                        <label style="width: 130px; height: 26px; margin-top: 10px">Observación</label><br />
                                        <asp:TextBox ID="txtObservacion" runat="server" Width="100%" Height="80px" TextMode="MultiLine" class="form-control" Style="display: inline;"></asp:TextBox><br />
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12" id="Boton" style="margin-top: 25px; text-align: right">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" Width="100px" OnClientClick="return validarCamposObligatorios();" OnClick="btnGuardar_Click" CssClass="btn btn-primary" />
                            <asp:Button ID="btnVolver" runat="server" Text="Volver" Width="100px" OnClientClick="return cargando();" OnClick="btnVolver_Click" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="divCargando" class="overlay">
        <div class="overlayContent">
            <img src="res/ajax-loader.gif" alt="Loading" />
            <h2>Procesando...</h2>
        </div>
    </div>
</asp:Content>
