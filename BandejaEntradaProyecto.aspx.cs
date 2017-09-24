using foSistemas.wsSistemas;
using System;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using ICSRED.API;

namespace foSistemas
{
    public partial class BandejaEntradaProyecto : System.Web.UI.Page
    {
        #region "Paginacion"

        void controlPaginacion_ButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (Session["ProyectoInicio"].ToString() == "1")
                    this.filtrosSessiones();
                this.cargarBandejaEntrada(int.Parse(Session["ProyectoInicio"].ToString()), ((ICSRED.controls.paginacion)(sender)).pagina, this.ddlOrdenar.SelectedValue, this.ddlAscendente.SelectedValue, ((ICSRED.controls.paginacion)(sender)).ver);
                this.hdPaginaActual.Value = ((ICSRED.controls.paginacion)(sender)).pagina.ToString();
                this.hdVerPagina.Value = ((ICSRED.controls.paginacion)(sender)).ver.ToString();
                Session["ProyectoPaginaActual"] = ((ICSRED.controls.paginacion)(sender)).pagina.ToString();
                Session["ProyectoVerPagina"] = ((ICSRED.controls.paginacion)(sender)).ver.ToString();
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(1, "BandejaEntradaProyecto.controlPaginacion_ButtonClicked", error, Session["UsuarioId"] == null ? 2 : int.Parse(Session["UsuarioId"].ToString()), "Sistemas");
                throw error;
            }
        }

        void controlPaginacion_SelectedIndexChange(object sender, EventArgs e)
        {
            try
            {
                if (Session["ProyectoInicio"].ToString() == "1")
                    this.filtrosSessiones();
                this.cargarBandejaEntrada(int.Parse(Session["ProyectoInicio"].ToString()), 1, this.ddlOrdenar.SelectedValue, this.ddlAscendente.SelectedValue, ((ICSRED.controls.paginacion)(sender)).ver);
                this.hdPaginaActual.Value = "1";
                this.hdVerPagina.Value = ((ICSRED.controls.paginacion)(sender)).ver.ToString();
                Session["ProyectoPaginaActual"] = "1";
                Session["ProyectoVerPagina"] = ((ICSRED.controls.paginacion)(sender)).ver.ToString();
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(1, "BandejaEntradaProyecto.controlPaginacion_SelectedIndexChange", error, Session["UsuarioId"] == null ? 2 : int.Parse(Session["UsuarioId"].ToString()), "Sistemas");
                throw error;
            }
        }

        #endregion

        #region "Métodos Protegidos"

        protected void Page_Load(object sender, EventArgs e)
        {
            Request request = null;
            Response response = null;
            wsSistemas.wsSistemas servicio = null;
            Permiso[] listaPermisos = null;
            bool sinPermiso = true;

            try
            {
                if (Request.QueryString["usuario"] != null && Session["UsuarioCodigo"] != null)
                {
                    if (!Request.QueryString["usuario"].ToString().Equals(Session["UsuarioCodigo"].ToString()))
                    {
                        Session.Clear();
                    }
                }
                if (!IsPostBack)
                {
                    if (Request.QueryString["usuario"] == null || Request.QueryString["clave"] == null)
                    {
                        this.sinAccesoMensaje("No se pudo acceder. Usuario o contraseña incorrectos.");
                        this.Principal.Style.Add("display", "none");
                        limpiarSesion(true);
                        return;
                    }

                    Session["Externa"] = null;
                    request = new Request();
                    request.Usuario = new Usuario();
                    servicio = new wsSistemas.wsSistemas();
                    request.Usuario.CodigoAcceso = Request.QueryString["usuario"] == null ? "" : Request.QueryString["usuario"].ToString();
                    request.Usuario.Clave = Request.QueryString["clave"] == null ? "" : Request.QueryString["clave"].ToString();
                    request.Usuario.Id = 2;
                    response = servicio.VerificarUsuario(request);
                    if (response.Resultado)
                    {
                        Session["UsuarioCodigo"] = response.Usuario.CodigoAcceso;
                        Session["UsuarioId"] = response.Usuario.Id;
                        Session["UsuarioNombre"] = response.Usuario.Nombre;
                        Session["UsuarioPermisos"] = response.Usuario.ArrayOfPermisos;
                        Session["UsuarioClave"] = Request.QueryString["clave"].ToString();
                    }
                    else
                    {
                        this.mensajeOperacionFallo(response.Error);
                        this.Principal.Style.Add("display", "none");
                        limpiarSesion(true);
                        return;
                    }

                    if (Session["UsuarioPermisos"] != null)
                    {
                        listaPermisos = (Permiso[])Session["UsuarioPermisos"];
                        foreach (var permiso in listaPermisos)
                        {
                            switch (permiso.Id)
                            {
                                case 1601:
                                    sinPermiso = false;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        this.sinAccesoMensaje("No tiene permisos para acceder a la aplicación");
                        this.limpiarSesion(true);
                        return;
                    }
                    if (sinPermiso)
                    {
                        this.sinAccesoMensaje("No tiene permisos para acceder a la aplicación");
                        this.limpiarSesion(true);
                        return;
                    }


                    this.hdPaginaActual.Value = Session["ProyectoPaginaActual"] == null ? "1" : Session["ProyectoPaginaActual"].ToString();
                    this.hdVerPagina.Value = Session["ProyectoVerPagina"] == null ? "25" : Session["ProyectoVerPagina"].ToString();
                    if (Session["ProyectoInicio"] == null)
                    {
                        this.hdVerPagina.Value = "25";
                        this.cargarBandejaEntrada(1, int.Parse(this.hdPaginaActual.Value), "Nombre", "ASC", int.Parse(this.hdVerPagina.Value));
                        this.ddlOrdenar.SelectedValue = "Nombre";
                        this.ddlAscendente.SelectedValue = "ASC";
                        this.hdInicio.Value = "1";
                        this.chbMisSolicitudes.Checked = true;
                        Session["ProyectoInicio"] = "1";
                    }
                    else
                    {
                        this.filtrosTextos();
                        this.cargarBandejaEntrada(int.Parse(Session["ProyectoInicio"].ToString()), int.Parse(this.hdPaginaActual.Value), "Nombre", "ASC", int.Parse(this.hdVerPagina.Value));
                        this.ddlOrdenar.SelectedValue = "Nombre";
                        this.ddlAscendente.SelectedValue = "ASC";
                    }
                }
                else
                {
                    this.controlPaginacion.ButtonClicked += new EventHandler(controlPaginacion_ButtonClicked);
                    this.controlPaginacion.SelectedIndexChanged += new EventHandler(controlPaginacion_SelectedIndexChange);
                }

            }
            catch (Exception error)
            {
                this.mensajePorError("al cargar la página");
                ManejoExcepciones.Logeo(1, "BandejaEntradaProyecto.Page_Load", error, Session["UsuarioId"] == null ? 2 : int.Parse(Session["UsuarioId"].ToString()), "Sistemas");
            }
            finally
            {
                request = null;
                response = null;
                servicio = null;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.verificarSesion())
                {
                    this.limpiarSesion(true);
                    return;
                }
                this.divResultado.Style.Add("display", "none");
                this.filtrosSessiones();
                this.cargarBandejaEntrada(this.hdInicio.Value != "" ? int.Parse(this.hdInicio.Value) : 0, 1, "Nombre", "ASC", int.Parse(this.hdVerPagina.Value));
                this.ddlOrdenar.SelectedValue = "Nombre";
                this.ddlAscendente.SelectedValue = "ASC";
                this.hdInicio.Value = this.chbMisSolicitudes.Checked == true ? "1" : "0";
                Session["TareaInicio"] = this.chbMisSolicitudes.Checked == true ? 1 : 0;
                Session["ProyectoInicio"] = "0";
                this.hdPaginaActual.Value = "1";
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(1, "BandejaEntradaProyecto.btnBuscar_Click", error, Session["UsuarioId"] == null ? 2 : int.Parse(Session["UsuarioId"].ToString()), "Sistemas");
                this.mensajePorError("al buscar los usuarios");
            }
            finally
            {
                //
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            Request request = null;
            wsSistemas.Proyecto proyecto = null;
            Response response = null;
            wsSistemas.wsSistemas servicio = null;

            try
            {
                if (!this.verificarSesion())
                {
                    this.limpiarSesion(true);
                    return;
                }
                proyecto = new wsSistemas.Proyecto();
                request = new Request();
                response = new Response();
                request.Usuario = new Usuario();

                request.Usuario.CodigoAcceso = Session["UsuarioCodigo"].ToString();
                request.Usuario.Clave = Session["UsuarioClave"].ToString();
                request.Usuario.Id = int.Parse(Session["UsuarioId"].ToString());

                proyecto.Id = int.Parse(this.hdId.Value);

                request.Proyecto = proyecto;
                servicio = new wsSistemas.wsSistemas();
                response = servicio.EliminarProyecto(request);

                if (response.Resultado)
                {
                    this.cargarBandejaEntrada(0, 1, this.ddlOrdenar.SelectedValue, this.ddlAscendente.SelectedValue, int.Parse(this.hdVerPagina.Value));
                    this.mensajeOperacionRealizada("Proyecto eliminado.");
                }
                else
                {
                    this.mensajePorError(response.Error);
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(1, "BandejaEntradaProyecto.btnEliminar_Click", error, Session["UsuarioId"] == null ? 2 : int.Parse(Session["UsuarioId"].ToString()), "Sistemas");
                this.mensajePorError("al eliminar un usuario");
            }
            finally
            {
                response = null;
                request = null;
                proyecto = null;
                servicio = null;
            }
        }

        protected void ddlOrdenar_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.verificarSesion())
                {
                    this.limpiarSesion(true);
                    return;
                }
                this.divResultado.Style.Add("display", "none");
                this.filtrosSessiones();
                this.cargarBandejaEntrada(0, 1, this.ddlOrdenar.SelectedValue, this.ddlAscendente.SelectedValue, int.Parse(this.hdVerPagina.Value));

            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(1, "BandejaEntradaProyecto.ddlOrdenar_SelectedIndexChanged", error, Session["UsuarioId"] == null ? 2 : int.Parse(Session["UsuarioId"].ToString()), "LegajoPersonal");
            }
        }

        protected void ddlAscendente_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.verificarSesion())
                {
                    this.limpiarSesion(true);
                    return;
                }
                this.divResultado.Style.Add("display", "none");
                this.filtrosSessiones();
                this.cargarBandejaEntrada(0, 1, this.ddlOrdenar.SelectedValue, this.ddlAscendente.SelectedValue, int.Parse(this.hdVerPagina.Value));

            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(1, "BandejaEntradaProyecto.ddlAscendente_SelectedIndexChanged", error, Session["UsuarioId"] == null ? 2 : int.Parse(Session["UsuarioId"].ToString()), "LegajoPersonal");
            }
        }

        #endregion

        #region Metodos Privados

        #region Mensajes

        private void mensajePorError(string mensaje)
        {
            this.divResultado.Style.Add("display", "inline");

            if (mensaje.Equals("La búsqueda no refleja resultados."))
            {
                this.lblResultado.Text = mensaje;
            }
            else
            {
                this.lblResultado.Text = string.Format("Houston, tenemos un error {0}. Si persiste ve a <b>Ayuda</b> y escríbenos todos los detalles.", mensaje);
            }

            this.divResultadoEstilo.Attributes["class"] = "alert alert-danger";
        }

        private void mensajeOperacionRealizada(string mensaje)
        {
            this.divResultado.Style.Add("display", "inline");
            this.lblResultado.Text = string.Format("Operación realizada. {0}", mensaje);
            this.divResultadoEstilo.Attributes["class"] = "alert alert-success";
        }

        private void sinAccesoMensaje(string mensaje)
        {
            this.divResultado.Style.Add("display", "inline");
            this.lblResultado.Text = string.Format("Sin acceso. {0}", mensaje);
            this.Principal.Style.Add("display", "none");
            this.divResultadoEstilo.Attributes["class"] = "alert alert-danger";
        }

        private void mensajeOperacionFallo(string mensaje)
        {
            this.divResultado.Style.Add("display", "inline");
            this.lblResultado.Text = string.Format("La operación falló. {0}", mensaje);
            this.divResultadoEstilo.Attributes["class"] = "alert alert-danger";
        }

        #endregion

        private void limpiarSesion(bool salir)
        {
            try
            {
                Session.Clear();
                this.hdExpiroSesion.Value = (salir) ? "1" : "0";
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(1, "BandejaEntradaProyecto.limpiarSesion", error, Session["UsuarioId"] == null ? 2 : int.Parse(Session["UsuarioId"].ToString()), "Sistemas");
                throw error;
            }
        }

        private bool verificarSesion()
        {
            return Session["UsuarioCodigo"] != null;
        }

               
        private void cargarBandejaEntrada(int inicio, int paginaNumero, string ordenamiento, string ascendenteDescendente, int ver)
        {
            Request request = null;
            Response response = null;
            wsSistemas.Proyectos proyectos = null;
            wsSistemas.wsSistemas servicio = null;
            StringBuilder sentenciaBandejaEntrada = null;
            int tfId = 0;
            try
            {
                proyectos = new wsSistemas.Proyectos();
                request = new Request();
                response = new Response();

                proyectos.UsuarioId = int.Parse(Session["UsuarioId"].ToString());
                proyectos.Inicio = inicio;
                proyectos.PaginaNumero = paginaNumero;
                proyectos.PaginaTamaño = ver;
                proyectos.Ordenamiento = ordenamiento;
                proyectos.AscendenteDescendiente = ascendenteDescendente;

                if (this.txtNombre.Text != string.Empty)
                    proyectos.Nombre = this.txtNombre.Text;
                if (int.TryParse(this.txtTFId.Text, out tfId))
                    proyectos.TFId = tfId;

                proyectos.EstadoId = Convert.ToInt32(this.ddlEstado.SelectedValue);
                proyectos.Borrado = this.chbInactivos.Checked ? true : false;

                request.Proyectos = proyectos;
                request.Usuario = new Usuario()
                {
                    CodigoAcceso = Session["UsuarioCodigo"].ToString(),
                    Clave = Session["UsuarioClave"].ToString()
                };
                servicio = new wsSistemas.wsSistemas();
                response = servicio.BuscarProyecto(request);

                if (response.Resultado)
                {
                    sentenciaBandejaEntrada = new StringBuilder();

                    foreach (var proyectoMostrar in response.Proyectos.ListaProyectos)
                    {
                        sentenciaBandejaEntrada.Append(string.Format("<div  class=\"row\" style=\"padding: 5px; border-bottom: 1px solid #dddddd; background-color:{0}\">", this.chbInactivos.Checked ? "#e6c1c7" : "#FFFFFF"));
                        sentenciaBandejaEntrada.Append(string.Format("<div class=\"col-lg-9\"><a target=\"_blank\" href=\"https://icsred.visualstudio.com/Panel%20de%20tareas/_workitems?id={1}\"><h3 style=\"width: 100%; color:#428bca; margin-top:0\">{0} - {6} - TF:{1}</h3></a><p style=\"width: 100%\"><b>Esfuerzo:</b> {5} días - <b>Fechas:</b> {2} - {3} - <b>Observaciones</b> {4}</p></div>",
                            proyectoMostrar.Nombre, proyectoMostrar.TFId, proyectoMostrar.FechaInicio.ToShortDateString(), proyectoMostrar.FechaFin.ToShortDateString(), proyectoMostrar.Observaciones, proyectoMostrar.TareasDuracion, proyectoMostrar.EstadoNombre));
                        sentenciaBandejaEntrada.Append(string.Format("<div class=\"col-lg-2\" style=\"text-align: right\">"));
                        sentenciaBandejaEntrada.Append(string.Format("<a href=\"Proyecto.aspx?Id={0}\"><i class=\"fa fa-pencil fa-2x\"></i></a>", proyectoMostrar.Id));
                        if (this.chbInactivos.Checked == false)
                            sentenciaBandejaEntrada.Append(string.Format("<a onclick=\"return eliminarProyecto('{0}')\"><i class=\"fa fa-trash fa-2x\" style=\"margin-left: 10px; margin-right:10px\"></i></a>", proyectoMostrar.Id));
                        sentenciaBandejaEntrada.Append(string.Format("</div></div>"));

                    }

                    if (Page.IsPostBack)
                        this.mensajeOperacionRealizada("Búsqueda satisfactoria.");

                    this.Principal.Style.Add("display", "inline");
                    this.controlPaginacion.PaginacionCrear(response.PaginaTotal, response.PaginaNumero, response.PaginaTamaño);
                    this.divBandejaEntrada.InnerHtml = sentenciaBandejaEntrada.ToString();
                }
                else
                {
                    this.divBandejaEntrada.InnerHtml = "";
                    this.mensajePorError(response.Error);
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(1, "BandejaEntradaProyecto.cargarBandejaEntrada", error, Session["UsuarioId"] == null ? 2 : int.Parse(Session["UsuarioId"].ToString()), "Sistemas");
                throw error;
            }
            finally
            {
                request = null;
                response = null;
                proyectos = null;
                servicio = null;
            }
        }

        private void filtrosSessiones()
        {
            try
            {
                Session["ProyectoNombre"] = this.txtNombre.Text;
                Session["ProyectoTFId"] = this.txtTFId.Text;
                Session["ProyectoEstado"] = this.ddlEstado.SelectedValue;
                Session["ProyectoInactivos"] = this.chbInactivos.Checked;
                Session["ProyectoOrdenamiento"] = this.ddlOrdenar.SelectedValue;
                Session["ProyectoAscendenteDescendente"] = this.ddlAscendente.SelectedValue;
                Session["ProyectoVerPagina"] = this.hdVerPagina.Value;
                Session["ProyectoPaginaActual"] = this.hdPaginaActual.Value == "" ? "1" : this.hdPaginaActual.Value;
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(1, "BandejaEntrada.filtrosSessiones", error, Session["UsuarioId"] == null ? 2 : int.Parse(Session["UsuarioId"].ToString()), "Ingenieria");
                throw error;
            }
        }

        private bool filtrosTextos()
        {
            try
            {
                if (Session["ProyectoNombre"] != null)
                {
                    this.chbMisSolicitudes.Checked = Session["ProyectoInicio"].ToString() == "1" ? true : false;
                    this.txtNombre.Text = Session["ProyectoNombre"].ToString();
                    this.txtTFId.Text = Session["ProyectoTFId"].ToString();
                    this.ddlEstado.SelectedValue = Session["ProyectoEstado"].ToString();
                    this.chbInactivos.Checked = (Session["ProyectoInactivos"].ToString().Equals("true")) ? true : false;
                    this.ddlOrdenar.SelectedValue = Session["ProyectoOrdenamiento"].ToString();
                    this.ddlAscendente.SelectedValue = Session["ProyectoAscendenteDescendente"].ToString();
                    this.hdVerPagina.Value = Session["ProyectoVerPagina"].ToString();
                    this.hdPaginaActual.Value = Session["ProyectoPaginaActual"].ToString();
                    this.hdInicio.Value = Session["ProyectoInicio"].ToString();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(1, "BandejaEntrada.filtrosTextos", error, Session["UsuarioId"] == null ? 2 : int.Parse(Session["UsuarioId"].ToString()), "Ingenieria");
                throw error;
            }
        }

        #endregion

    }


}