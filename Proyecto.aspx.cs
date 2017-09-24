using System;
using foSistemas.wsSistemas;
using ICSRED.API;


namespace foSistemas
{
    public partial class Proyecto : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            wsSistemas.Proyecto proyecto = null;
            int proyectoId = 0;
            try
            {
                if (Request.QueryString["Externa"] == null)
                {
                    if (!IsPostBack)
                    {
                        if (this.verificarSesion())
                        {
                            if (int.TryParse(Request.QueryString["Id"], out proyectoId))
                            {
                                proyectoId = Convert.ToInt32(Request.QueryString["Id"].ToString());
                                this.hdId.Value = proyectoId.ToString();
                                proyecto = this.cargarProyecto(proyectoId);
                                if (proyecto == null)
                                {
                                    this.divPrincipal.Style.Add("display", "none");
                                    this.btnVolver.Style.Add("display", "inline");

                                    this.mensajeOperacionFallo("No se pudo cargar el usuario");

                                    return;
                                }
                                this.cargarPagina(proyecto, false);
                            }

                            if (proyectoId.Equals(0))
                            {
                                this.cargarPagina(proyecto, true);
                                this.hdId.Value = "0";
                                this.hdCoordinadorId.Value = "0";
                                this.hdResponsableId.Value = "0";
                                this.hdVersion.Value = "0";
                                this.hdEstadoId.Value = "0";
                            }
                        }
                        else
                        {
                            this.sinAccesoMensaje("No tiene permiso para acceder a la aplicación.");
                            this.limpiarSesion(true);
                        }
                    }
                }
                else
                {
                    Session["Externa"] = Request.QueryString["Externa"].ToString();
                    if (!IsPostBack)
                    {
                        if (this.buscarPermisosUsuario(Request.QueryString["Usuario"] != null ? Request.QueryString["Usuario"].ToString() : "", Request.QueryString["clave"] != null ? Request.QueryString["clave"].ToString() : ""))
                        {
                            this.cargarPagina(this.cargarProyecto(int.Parse(Request.QueryString["Id"].ToString())), false);
                        }
                        else
                        {
                            this.mensajeOperacionFallo("al cargar la página");
                        }
                    }
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(1, "Proyecto.Load", error, Session["UsuarioId"] == null ? 2 : int.Parse(Session["UsuarioId"].ToString()), "Sistemas");
                this.mensajePorError("al cargar la página");
            }
            finally
            {
                proyecto = null;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Response response = null;
            Request request = null;
            wsSistemas.Proyecto proyecto = null;
            wsSistemas.wsSistemas servicio = null;
            try
            {
                if (!this.verificarSesion())
                {
                    this.limpiarSesion(true);
                    return;
                }
                response = new Response();
                proyecto = new wsSistemas.Proyecto();
                request = new Request();
                request.Usuario = new Usuario();

                request.Usuario.CodigoAcceso = Session["UsuarioCodigo"].ToString();
                request.Usuario.Clave = Session["UsuarioClave"].ToString();
                request.Usuario.Id = int.Parse(Session["UsuarioId"].ToString());
                request.Usuario.Nombre = Session["UsuarioNombre"].ToString();


                proyecto.Id = int.Parse(this.hdId.Value);
                proyecto.UsuarioId = request.Usuario.Id;
                proyecto.CoordinadorId = int.Parse(this.hdCoordinadorId.Value);
                proyecto.ResponsableId = int.Parse(this.hdResponsableId.Value);
                proyecto.EstadoId = int.Parse(this.hdEstadoId.Value);
                proyecto.Nombre = this.txtNombre.Text;
                proyecto.TFId = Int32.Parse(this.txtTFId.Text);
                proyecto.FechaInicio = this.txtFechaInicio.Text != string.Empty ? Convert.ToDateTime(this.txtFechaInicio.Text) : new DateTime(1900, 01, 01);
                proyecto.FechaFin = this.txtFechaFin.Text != string.Empty ? Convert.ToDateTime(this.txtFechaFin.Text) : new DateTime(1900, 01, 01);
                proyecto.Observaciones = this.txtObservacion.Text;
                proyecto.Version = Int32.Parse(this.hdVersion.Value);
                proyecto.Borrado = false;

                request.Proyecto = proyecto;
                servicio = new wsSistemas.wsSistemas();
                response = servicio.GuardarProyecto(request);

                if (response.Resultado)
                {
                    this.cargarPagina(response.Proyecto, false);
                    this.hdId.Value = Convert.ToString(response.Proyecto.Id);
                    this.mensajeOperacionRealizada("El proyecto ha sido guardado");
                }
                else
                {
                    this.mensajeOperacionFallo(response.Error);
                    this.cargarPagina(response.Proyecto, false);
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(1, "Proyecto.btnGuardarUsuario_Click", error, Session["UsuarioId"] == null ? 2 : int.Parse(Session["UsuarioId"].ToString()), "Sistemas");
                this.mensajePorError("al guardar un proyecto");
            }
            finally
            {
                request = null;
                proyecto = null;
                response = null;
                servicio = null;
            }

        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.verificarSesion())
                {
                    this.limpiarSesion(true);
                    return;
                }
                Response.Redirect("~/BandejaEntradaProyecto.aspx?usuario=" + Session["UsuarioCodigo"] + "&clave=" + Session["UsuarioClave"], false);
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(1, "Proyecto.btnVolver_Click", error, Session["UsuarioId"] == null ? 2 : int.Parse(Session["UsuarioId"].ToString()), "Sistemas");
                this.mensajePorError("al volver a la bandeja de entrada");
            }
            finally
            {

            }
        }

        #region Métodos Privados

        #region Mensajes

        private void mensajePorError(string mensaje)
        {
            this.divResultado.Style.Add("display", "inline");
            this.lblResultado.Text = string.Format("Houston, tenemos un error {0}. Si persiste ve a <b>Ayuda</b> y escríbenos todos los detalles.", mensaje);
            this.divResultadoEstilo.Attributes["class"] = "alert alert-danger";
        }

        private void mensajeOperacionFallo(string mensaje)
        {
            this.divResultado.Style.Add("display", "inline");
            this.lblResultado.Text = string.Format("La operación falló. {0}", mensaje);
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
            this.divPrincipal.Style.Add("display", "none");
            this.divResultadoEstilo.Attributes["class"] = "alert alert-danger";
        }

        #endregion

        private bool buscarPermisosUsuario(string usuarioCodigo, string usuarioClave)
        {
            Request request = null;
            Response response = null;
            wsSistemas.wsSistemas servicio = null;
            try
            {
                request = new Request();
                servicio = new wsSistemas.wsSistemas();
                request.Usuario = new Usuario();
                request.Usuario.CodigoAcceso = usuarioCodigo;
                request.Usuario.Clave = usuarioClave;
                request.Usuario.Id = 2;
                response = servicio.VerificarUsuario(request);
                if (response.Resultado)
                {
                    Session["UsuarioCodigo"] = response.Usuario.CodigoAcceso;
                    Session["UsuarioId"] = response.Usuario.Id;
                    this.hdUsuarioId.Value = response.Usuario.Id.ToString();
                    Session["UsuarioNombre"] = response.Usuario.Nombre;
                    Session["UsuarioMail"] = response.Usuario.Mail;
                    Session["UsuarioPermisos"] = response.Usuario.ArrayOfPermisos;
                    Session["UsuarioClave"] = usuarioClave;
                    return true;
                }
                else
                {
                    this.mensajeOperacionFallo(response.Error);
                    return false;
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(1, "Proyecto.buscarPermisosUsuario", error, Session["UsuarioId"] == null ? 2 : int.Parse(Session["UsuarioId"].ToString()), "Sistemas");
                throw error;
            }
            finally
            {
                request = null;
                response = null;
                servicio = null;
            }
        }

        private void cargarPagina(wsSistemas.Proyecto proyecto, bool nuevo)
        {
            Permiso[] listaPermisos = null;
            bool sinPermiso = true;
            try
            {
                if (proyecto != null && !nuevo)
                {
                    this.cargarProyecto(proyecto);
                }

                listaPermisos = (Permiso[])Session["UsuarioPermisos"];
                foreach (var permiso in listaPermisos)
                {
                    switch (permiso.Id)
                    {
                        case 1:
                            sinPermiso = false;
                            if (permiso.Nombre == "Acceso PEM")
                                this.hdPanelPadre.Value = "PEM";
                            break;
                    }
                }


                if (sinPermiso)
                {
                    this.sinAccesoMensaje("No tiene permisos para acceder a la página");
                    this.limpiarSesion(true);
                    return;
                }

            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(1, "Proyecto.cargarPagina", error, this.hdUsuarioId.Value == "" ? 2 : int.Parse(this.hdUsuarioId.Value), "PEM");
                throw error;
            }
            finally
            {
                listaPermisos = null;
            }
        }

        private wsSistemas.Proyecto cargarProyecto(int proyectoId)
        {
            Request request = null;
            wsSistemas.Proyecto proyecto = null;
            Response response = null;
            wsSistemas.wsSistemas servicio = null;
            try
            {
                request = new Request();
                response = new Response();
                proyecto = new wsSistemas.Proyecto();
                request.Usuario = new Usuario();

                request.Usuario.CodigoAcceso = Session["UsuarioCodigo"].ToString();
                request.Usuario.Clave = Session["UsuarioClave"].ToString();
                request.Usuario.Id = int.Parse(Session["UsuarioId"].ToString());

                proyecto.Id = proyectoId;
                request.Proyecto = proyecto;
                servicio = new wsSistemas.wsSistemas();
                response = servicio.CargarProyecto(request);

                if (response.Resultado)
                {
                    return response.Proyecto;
                }
                else
                {
                    this.mensajeOperacionFallo("No se pudo cargar el proyecto");
                    return null;
                }
            }

            catch (Exception error)
            {
                ManejoExcepciones.Logeo(1, "Proyecto.cargarProyecto", error, Session["UsuarioId"] == null ? 2 : int.Parse(Session["UsuarioId"].ToString()), "Sistemas");

                this.mensajePorError("al cargar el proyecto");
                throw error;
            }
            finally
            {
                proyecto = null;
                request = null;
                response = null;
                servicio = null;
            }
        }

        private void cargarProyecto(wsSistemas.Proyecto proyecto)
        {
            try
            {

                this.txtNombre.Text = proyecto.Nombre;
                this.txtFechaFin.Text = proyecto.FechaFin != new DateTime() ? proyecto.FechaFin.ToShortDateString() : string.Empty;
                this.txtFechaInicio.Text = proyecto.FechaInicio != new DateTime() ? proyecto.FechaInicio.ToShortDateString() : string.Empty;
                this.txtObservacion.Text = proyecto.Observaciones;
                this.txtTFId.Text = proyecto.TFId.ToString();
                this.hdVersion.Value = proyecto.Version.ToString();
                this.hdId.Value = proyecto.Id.ToString();
                this.hdEstadoId.Value = proyecto.EstadoId.ToString();
                this.hdCoordinadorId.Value = proyecto.CoordinadorId.ToString();
                this.hdResponsableId.Value = proyecto.ResponsableId.ToString();

            }

            catch (Exception error)
            {
                ManejoExcepciones.Logeo(1, "Proyecto.cargarProyecto", error, Session["UsuarioId"] == null ? 2 : int.Parse(Session["UsuarioId"].ToString()), "Sistemas");
                this.mensajePorError("al cargar el usuario");
                throw error;
            }

            finally
            {
                //
            }

        }

        private void limpiarSesion(bool salir)
        {
            try
            {
                Session.Clear();
                this.hdExpiroSesion.Value = (salir) ? "1" : "0";
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(1, "Proyecto.limpiarSesion", error, Session["UsuarioId"] == null ? 2 : int.Parse(Session["UsuarioId"].ToString()), "Sistemas");
                throw error;
            }
        }

        private bool verificarSesion()
        {
            return Session["UsuarioCodigo"] != null;
        }


        #endregion

    }
}