using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Web.Services;
using ICSRED.API;
using ICSRED.Usuario;


namespace wsSistemas
{
    /// <summary>
    /// Descripción breve de wsSistemas
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class wsSistemas : System.Web.Services.WebService
    {
        #region Proyecto

        [WebMethod]
        public Response BuscarProyecto(Request request)
        {
            List<Proyecto> proyectos = null;
            Usuario usuario = null;
            try
            {
                if (request == null)
                {
                    return new Response() { Resultado = false, Error = "El metodo no soporta un objeto request nulo." };
                }
                if (request.Usuario == null)
                {
                    return new Response() { Resultado = false, Error = "El metodo no soporta un objeto usuario nulo." };
                }
                usuario = new Usuario();
                usuario = usuario.VerificarUsuario(request.Usuario.CodigoAcceso, request.Usuario.Clave);
                if (usuario.Resultado)
                {

                    proyectos = request.Proyectos.Buscar();

                    if (proyectos.Count == 0)
                    {
                        return new Response()
                        {
                            Resultado = false,
                            Error = "La búsqueda no refleja resultados.",
                        };
                    }

                    else
                    {
                        return new Response()
                        {
                            Resultado = true,
                            Proyectos = new Proyectos()
                            {
                                ListaProyectos = proyectos
                            },
                            PaginaNumero = request.Proyectos.PaginaNumero,
                            PaginaTotal = request.Proyectos.PaginaTotal,
                            PaginaTamaño = request.Proyectos.PaginaTamaño

                        };
                    }

                }
                else
                {
                    return new Response() { Resultado = false, Error = usuario.Error };
                }

            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.BuscarProyecto", error, request.Usuario.Id, "Sistemas");
                return new Response() { Resultado = false, Error = string.Format("Ha sucedido una excepcion - Descripcion: {0}", error.Message) };
            }
            finally
            {
                proyectos = null;
                usuario = null;
            }
        }

        [WebMethod]
        public Response GuardarProyecto(Request request)
        {
            Proyecto proyecto = null;
            Usuario usuario = null;
            Proyecto proyectoAnterior = null;

            try
            {
                if (request == null)
                {
                    return new Response() { Resultado = false, Error = "El metodo no soporta un objeto request nulo." };
                }

                if (request.Usuario == null)
                {
                    return new Response() { Resultado = false, Error = "El request no contiene un usuario para guardar." };
                }
                usuario = new Usuario();
                usuario = usuario.VerificarUsuario(request.Usuario.CodigoAcceso, request.Usuario.Clave);
                if (usuario.Resultado)
                {
                    if (request.Proyecto.VerificarProyecto())
                    {
                        return new Response() { Proyecto = request.Proyecto, Resultado = false, Error = "El usuario ya existe." };
                    }

                    proyectoAnterior = request.Proyecto.Cargar();

                    proyecto = request.Proyecto.Guardar();

                    request.Proyecto = proyecto;

                    this.enviarMensajeProyecto(request, proyectoAnterior != null ? proyectoAnterior.EstadoId : 0);

                    return new Response() { Resultado = true, Proyecto = proyecto };
                }
                else
                {
                    return new Response() { Resultado = false, Error = usuario.Error };
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.GuardarProyecto", error, request.Usuario.Id, "Sistemas");
                return new Response() { Resultado = false, Error = string.Format("Ha sucedido una excepcion - Descripcion: {0}", error.Message) };
            }

            finally
            {
                proyecto = null;
                usuario = null;
                proyectoAnterior = null;
            }

        }

        [WebMethod]
        public Response EliminarProyecto(Request request)
        {
            Usuario usuario = null;
            try
            {
                if (request == null)
                {
                    return new Response() { Resultado = false, Error = "El metodo no soporta un objeto request nulo." };
                }

                if (request.Usuario == null)
                {
                    return new Response() { Resultado = false, Error = "El request no contiene un usuario para guardar." };
                }
                usuario = new Usuario();
                usuario = usuario.VerificarUsuario(request.Usuario.CodigoAcceso, request.Usuario.Clave);
                if (usuario.Resultado)
                {
                    request.Proyecto.Eliminar();

                    return new Response() { Resultado = true };
                }
                else
                {
                    return new Response() { Resultado = false, Error = usuario.Error };
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.EliminarProyecto", error, request.Usuario.Id, "Sistemas");
                return new Response() { Resultado = false, Error = string.Format("Ha sucedido una excepcion - Descripcion: {0}", error.Message) };
            }
            finally
            {
                usuario = null;
            }

        }

        [WebMethod]
        public Response CargarProyecto(Request request)
        {
            Proyecto proyecto = null;
            Usuario usuario = null;
            try
            {
                if (request == null)
                {
                    return new Response() { Resultado = false, Error = "El metodo no soporta un objeto request nulo." };
                }

                if (request.Usuario == null)
                {
                    return new Response() { Resultado = false, Error = "El request no contiene un usuario para guardar." };
                }
                usuario = new Usuario();
                usuario = usuario.VerificarUsuario(request.Usuario.CodigoAcceso, request.Usuario.Clave);
                if (usuario.Resultado)
                {

                    proyecto = request.Proyecto.Cargar();

                    return new Response() { Resultado = true, Proyecto = proyecto };
                }
                else
                {
                    return new Response() { Resultado = false, Error = usuario.Error };
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.CargarProyecto", error, request.Usuario.Id, "Sistemas");
                return new Response() { Resultado = false, Error = string.Format("Ha sucedido una excepcion - Descripcion: {0}", error.Message) };
            }
            finally
            {
                proyecto = null;
                usuario = null;
            }
        }


        #endregion

        #region Tarea

        [WebMethod]
        public Response BuscarTarea(Request request)
        {
            List<Tarea> tareas = null;
            Usuario usuario = null;
            try
            {
                if (request == null)
                {
                    return new Response() { Resultado = false, Error = "El metodo no soporta un objeto request nulo." };
                }
                if (request.Usuario == null)
                {
                    return new Response() { Resultado = false, Error = "El metodo no soporta un objeto usuario nulo." };
                }
                usuario = new Usuario();
                usuario = usuario.VerificarUsuario(request.Usuario.CodigoAcceso, request.Usuario.Clave);
                if (usuario.Resultado)
                {

                    tareas = request.Tareas.Buscar();

                    if (tareas.Count == 0)
                    {
                        return new Response()
                        {
                            Resultado = false,
                            Error = "La búsqueda no refleja resultados.",
                        };
                    }

                    else
                    {
                        return new Response()
                        {
                            Resultado = true,
                            Tareas = new Tareas()
                            {
                                ListaTareas = tareas
                            },
                            PaginaNumero = request.Tareas.PaginaNumero,
                            PaginaTotal = request.Tareas.PaginaTotal,
                            PaginaTamaño = request.Tareas.PaginaTamaño

                        };
                    }

                }
                else
                {
                    return new Response() { Resultado = false, Error = usuario.Error };
                }

            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.BuscarTarea", error, request.Usuario.Id, "Sistemas");
                return new Response() { Resultado = false, Error = string.Format("Ha sucedido una excepcion - Descripcion: {0}", error.Message) };
            }
            finally
            {
                tareas = null;
                usuario = null;
            }
        }

        [WebMethod]
        public Response GuardarTarea(Request request)
        {
            Tarea tarea = null;
            Tarea tareaAnterior = null;
            Usuario usuario = null;
            try
            {
                if (request == null)
                {
                    return new Response() { Resultado = false, Error = "El metodo no soporta un objeto request nulo." };
                }

                if (request.Usuario == null)
                {
                    return new Response() { Resultado = false, Error = "El request no contiene un usuario para guardar." };
                }
                usuario = new Usuario();
                usuario = usuario.VerificarUsuario(request.Usuario.CodigoAcceso, request.Usuario.Clave);
                if (usuario.Resultado)
                {
                    if (request.Tarea.VerificarTarea())
                    {
                        return new Response() { Tarea = request.Tarea, Resultado = false, Error = "El usuario ya existe." };
                    }

                    tareaAnterior = request.Tarea.Cargar();

                    tarea = request.Tarea.Guardar();

                    if (request.Tarea.Id != 0)
                    {
                        request.Tarea = tarea;
                        this.enviarMensajeTarea(request, tareaAnterior.EstadoId);

                    }

                    return new Response() { Resultado = true, Tarea = tarea };
                }
                else
                {
                    return new Response() { Resultado = false, Error = usuario.Error };
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.GuardarTarea", error, request.Usuario.Id, "Sistemas");
                return new Response() { Resultado = false, Error = string.Format("Ha sucedido una excepcion - Descripcion: {0}", error.Message) };
            }

            finally
            {
                tarea = null;
                usuario = null;
                tareaAnterior = null;
            }


        }

        [WebMethod]
        public Response EliminarTarea(Request request)
        {
            Usuario usuario = null;
            try
            {
                if (request == null)
                {
                    return new Response() { Resultado = false, Error = "El metodo no soporta un objeto request nulo." };
                }

                if (request.Usuario == null)
                {
                    return new Response() { Resultado = false, Error = "El request no contiene un usuario para guardar." };
                }
                usuario = new Usuario();
                usuario = usuario.VerificarUsuario(request.Usuario.CodigoAcceso, request.Usuario.Clave);
                if (usuario.Resultado)
                {

                    request.Tarea.Eliminar();

                    return new Response() { Resultado = true };
                }
                else
                {
                    return new Response() { Resultado = false, Error = usuario.Error };
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.EliminarTarea", error, request.Usuario.Id, "Sistemas");
                return new Response() { Resultado = false, Error = string.Format("Ha sucedido una excepcion - Descripcion: {0}", error.Message) };
            }
            finally
            {
                usuario = null;
            }

        }

        [WebMethod]
        public Response CargarTarea(Request request)
        {
            Tarea tarea = null;
            Usuario usuario = null;
            try
            {
                if (request == null)
                {
                    return new Response() { Resultado = false, Error = "El metodo no soporta un objeto request nulo." };
                }

                if (request.Usuario == null)
                {
                    return new Response() { Resultado = false, Error = "El request no contiene un usuario para guardar." };
                }
                usuario = new Usuario();
                usuario = usuario.VerificarUsuario(request.Usuario.CodigoAcceso, request.Usuario.Clave);
                if (usuario.Resultado)
                {

                    tarea = request.Tarea.Cargar();

                    return new Response() { Resultado = true, Tarea = tarea };
                }
                else
                {
                    return new Response() { Resultado = false, Error = usuario.Error };
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.CargarTarea", error, request.Usuario.Id, "Sistemas");
                return new Response() { Resultado = false, Error = string.Format("Ha sucedido una excepcion - Descripcion: {0}", error.Message) };
            }
            finally
            {
                tarea = null;
                usuario = null;
            }
        }

        #endregion

        #region Trabajo

        [WebMethod]
        public Response BuscarTrabajo(Request request)
        {
            List<Trabajo> trabajos = null;
            Usuario usuario = null;
            try
            {
                if (request == null)
                {
                    return new Response() { Resultado = false, Error = "El metodo no soporta un objeto request nulo." };
                }
                if (request.Usuario == null)
                {
                    return new Response() { Resultado = false, Error = "El metodo no soporta un objeto usuario nulo." };
                }
                usuario = new Usuario();
                usuario = usuario.VerificarUsuario(request.Usuario.CodigoAcceso, request.Usuario.Clave);
                if (usuario.Resultado)
                {

                    trabajos = request.Trabajos.Buscar();

                    if (trabajos.Count == 0)
                    {
                        return new Response()
                        {
                            Resultado = false,
                            Error = "La búsqueda no refleja resultados.",
                        };
                    }

                    else
                    {
                        return new Response()
                        {
                            Resultado = true,
                            Trabajos = new Trabajos()
                            {
                                ListaTrabajos = trabajos
                            },
                            PaginaNumero = request.Trabajos.PaginaNumero,
                            PaginaTotal = request.Trabajos.PaginaTotal,
                            PaginaTamaño = request.Trabajos.PaginaTamaño

                        };
                    }

                }
                else
                {
                    return new Response() { Resultado = false, Error = usuario.Error };
                }

            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.BuscarTrabajo", error, request.Usuario.Id, "Sistemas");
                return new Response() { Resultado = false, Error = string.Format("Ha sucedido una excepcion - Descripcion: {0}", error.Message) };
            }
            finally
            {
                trabajos = null;
                usuario = null;
            }
        }

        [WebMethod]
        public Response GuardarTrabajo(Request request)
        {
            Trabajo trabajo = null;
            Usuario usuario = null;
            try
            {
                if (request == null)
                {
                    return new Response() { Resultado = false, Error = "El metodo no soporta un objeto request nulo." };
                }

                if (request.Usuario == null)
                {
                    return new Response() { Resultado = false, Error = "El request no contiene un usuario para guardar." };
                }
                usuario = new Usuario();
                usuario = usuario.VerificarUsuario(request.Usuario.CodigoAcceso, request.Usuario.Clave);
                if (usuario.Resultado)
                {

                    trabajo = request.Trabajo.Guardar();

                    return new Response() { Resultado = true, Trabajo = trabajo };
                }
                else
                {
                    return new Response() { Resultado = false, Error = usuario.Error };
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.GuardarTrabajo", error, request.Usuario.Id, "Sistemas");
                return new Response() { Resultado = false, Error = string.Format("Ha sucedido una excepcion - Descripcion: {0}", error.Message) };
            }

            finally
            {
                trabajo = null;
                usuario = null;
            }


        }

        [WebMethod]
        public Response EliminarTrabajo(Request request)
        {
            Usuario usuario = null;
            try
            {
                if (request == null)
                {
                    return new Response() { Resultado = false, Error = "El metodo no soporta un objeto request nulo." };
                }

                if (request.Usuario == null)
                {
                    return new Response() { Resultado = false, Error = "El request no contiene un usuario para guardar." };
                }
                usuario = new Usuario();
                usuario = usuario.VerificarUsuario(request.Usuario.CodigoAcceso, request.Usuario.Clave);
                if (usuario.Resultado)
                {

                    request.Trabajo.Eliminar();

                    return new Response() { Resultado = true };
                }
                else
                {
                    return new Response() { Resultado = false, Error = usuario.Error };
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.EliminarTrabajo", error, request.Usuario.Id, "Sistemas");
                return new Response() { Resultado = false, Error = string.Format("Ha sucedido una excepcion - Descripcion: {0}", error.Message) };
            }
            finally
            {
                usuario = null;
            }

        }

        [WebMethod]
        public Response CargarTrabajo(Request request)
        {
            Trabajo trabajo = null;
            Usuario usuario = null;
            try
            {
                if (request == null)
                {
                    return new Response() { Resultado = false, Error = "El metodo no soporta un objeto request nulo." };
                }

                if (request.Usuario == null)
                {
                    return new Response() { Resultado = false, Error = "El request no contiene un usuario para guardar." };
                }
                usuario = new Usuario();
                usuario = usuario.VerificarUsuario(request.Usuario.CodigoAcceso, request.Usuario.Clave);
                if (usuario.Resultado)
                {

                    trabajo = request.Trabajo.Cargar();

                    return new Response() { Resultado = true, Trabajo = trabajo };
                }
                else
                {
                    return new Response() { Resultado = false, Error = usuario.Error };
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.CargarTrabajo", error, request.Usuario.Id, "Sistemas");
                return new Response() { Resultado = false, Error = string.Format("Ha sucedido una excepcion - Descripcion: {0}", error.Message) };
            }
            finally
            {
                trabajo = null;
                usuario = null;
            }
        }

        #endregion

        #region "Informes"

        /// <summary>
        /// Obtiene la carga laboral por colaborador
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string ObtenerCargaLaboral(string proyectoId)
        {
            JavaScriptSerializer jscript = null;
            Informe informe = null;

            try
            {
                informe = new Informe();
                informe.Id = Convert.ToInt32(proyectoId);
                jscript = new JavaScriptSerializer();
                return jscript.Serialize(informe.CargaLaboral());
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.ObtenerCargaLaboral", error, 2, "Sistemas");
                throw error;
            }
            finally
            {
                informe = null;
                jscript = null;
            }
        }

        /// <summary>
        /// Obtiene la carga laboral por colaborador
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string ObtenerProduccion()
        {
            JavaScriptSerializer jscript = null;
            Informes informes = null;

            try
            {
                informes = new Informes();

                jscript = new JavaScriptSerializer();
                return jscript.Serialize(informes.Produccion());
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.ObtenerProduccion", error, 2, "Sistemas");
                throw error;
            }
            finally
            {
                informes = null;
                jscript = null;
            }
        }

        /// <summary>
        /// Obtiene la carga laboral por colaborador
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string ObtenerCantidadTareasEstado(string proyectoId)
        {
            JavaScriptSerializer jscript = null;
            Informe informe = null;

            try
            {
                informe = new Informe();
                informe.Id = Convert.ToInt32(proyectoId);
                jscript = new JavaScriptSerializer();
                return jscript.Serialize(informe.CantidadTareasEstado());
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.ObtenerCantidadTareasEstado", error, 2, "Sistemas");
                throw error;
            }
            finally
            {
                informe = null;
                jscript = null;
            }
        }

        /// <summary>
        /// Obtiene la Productividad Mesual por colaborador
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string ObtenerProductividadMeses(Informes informes)
        {
            JavaScriptSerializer jscript = null;
            try
            {
                jscript = new JavaScriptSerializer();
                return jscript.Serialize(informes.ProductividadMeses());
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.ObtenerProductividadMeses", error, 2, "Sistemas");
                return jscript.Serialize(new List<Informe>());
            }
            finally
            {
                jscript = null;
            }
        }

        /// <summary>
        /// Obtiene la Productividad Anual por colaborador
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string ObtenerProductividadAnual(Informes informes)
        {
            JavaScriptSerializer jscript = null;
            try
            {
                jscript = new JavaScriptSerializer();
                return jscript.Serialize(informes.ProductividadAnual());
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.ObtenerProductividadAnual", error, 2, "Sistemas");
                return jscript.Serialize(new List<Informe>());
            }
            finally
            {
                jscript = null;
            }
        }

        /// <summary>
        /// Obtiene la estado del proyecto
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string ObtenerProyectoEstado(Informes informes)
        {
            JavaScriptSerializer jscript = null;
            try
            {
                jscript = new JavaScriptSerializer();
                return jscript.Serialize(informes.ProyectoEstado());
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.ObtenerProyectoEstado", error, 2, "Sistemas");
                return jscript.Serialize(new List<Informe>());
            }
            finally
            {
                jscript = null;
            }
        }

        /// <summary>
        /// Obtiene los estados de los proyectos
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string ObtenerProyectosEstado()
        {
            JavaScriptSerializer jscript = null;
            Informes informes = null;
            try
            {
                informes = new Informes();
                jscript = new JavaScriptSerializer();
                return jscript.Serialize(informes.ProyectosEstado());
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.ObtenerProyectosEstado", error, 2, "Sistemas");
                return jscript.Serialize(new List<Informe>());
            }
            finally
            {
                informes = null;
                jscript = null;
            }
        }

        /// <summary>
        /// Obtiene detalle del proyecto, estados de las tareas
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string ObtenerProyectoEstadoDetalle(Informes informes)
        {
            JavaScriptSerializer jscript = null;
            try
            {
                jscript = new JavaScriptSerializer();
                return jscript.Serialize(informes.ProyectoEstadoDetalle());
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.ObtenerProyectoEstadoDetalle", error, 2, "Sistemas");
                return jscript.Serialize(new List<Informe>());
            }
            finally
            {
                jscript = null;
            }
        }

        #endregion

        #region Persona

        /// <summary>
        /// Elimina un Usuario
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [WebMethod]
        public Response EliminarPersona(Request request)
        {
            try
            {
                if (request != null)
                {
                    if (request.Persona != null)
                    {

                        if (request.Persona.Eliminar())
                        {
                            return new Response() { Resultado = true, Error = "Usuario eliminado correctamente" };

                        }
                        else
                        {
                            return new Response() { Resultado = false, Error = "Error al eliminar usuario" };
                        }
                    }
                    else
                    {
                        return new Response() { Resultado = false, Error = "El objeto usuario debe tener valores" };
                    }
                }
                else
                {
                    return new Response() { Resultado = false, Error = "El objeto request debe tener valores" };
                }

            }
            catch (Exception ex)
            {
                return new Response() { Resultado = false, Error = string.Format("Error no controlado, exepcion: {0}", ex.InnerException.ToString()) };

            }
            finally
            {

            }
        }

        /// <summary>
        /// Elimina una persona de la base de datos
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [WebMethod]
        public Response GuardarPersona(Request request)
        {
            try
            {
                if (request != null)
                {
                    if (request.Persona != null)
                    {

                        if (request.Persona.Guardar())
                        {
                            return new Response() { Resultado = true, Error = "Se ha guardado correctamente la persona" };
                        }
                        else
                        {
                            return new Response() { Resultado = false, Error = "Error al Guardar" };
                        }

                    }
                    else
                    {
                        return new Response() { Resultado = false, Error = "El objeto persona debe tener valores" };
                    }

                }
                else
                {
                    return new Response() { Resultado = false, Error = "El objeto request debe tener valores" };
                }
            }
            catch (Exception ex)
            {

                return new Response() { Resultado = false, Error = string.Format("Error no controlado: {0}", ex.InnerException.ToString()) };
            }
            finally
            {

            }
        }

        /// <summary>
        /// Busca Personas que coincidan con nombre ingresado
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [WebMethod]
        public Response BuscarPersona(Request request)
        {
            List<Persona> personas = null;
            try
            {
                if (request != null)
                {
                    if (request.Personas != null)
                    {
                        personas = request.Personas.Buscar();

                        if (personas.Count == 0)
                        {
                            return new Response()
                            {
                                Resultado = false,
                                Error = "La búsqueda no refleja resultados.",
                            };
                        }
                        else
                        {
                            return new Response()
                            {
                                Resultado = true,
                                Personas = new Personas()
                                {
                                    ListaPersonas = personas
                                },
                                PaginaNumero = request.Personas.PaginaNumero,
                                PaginaTotal = request.Personas.PaginaTotal,
                                PaginaTamaño = request.Personas.PaginaTamaño

                            };
                        }
                    }
                    else
                    {
                        return new Response { Resultado = false, Error = "La busqueda Fallo" };
                    }
                }
                else
                {
                    return new Response { Resultado = false, Error = "La busqueda Fallo" };
                }
            }
            catch (Exception ex)
            {

                return new Response() { Resultado = false, Error = string.Format("Error no Controlado {0}", ex.InnerException.ToString()) };
            }
            finally
            {
                personas = null;
            }
        }

        #endregion

        #region "Métodos Compartidos"

        [WebMethod]
        public Response VerificarUsuario(Request request)
        {
            Response response = null;
            Usuario usuario = null;
            try
            {
                if (request == null)
                {
                    return new Response() { Resultado = false, Error = "El metodo no soporta un objeto request nulo." };
                }
                if (request.Usuario.CodigoAcceso == null)
                {
                    return new Response() { Resultado = false, Error = "El request no contiene un usuario." };
                }

                usuario = new Usuario();
                usuario = usuario.VerificarUsuario(request.Usuario.CodigoAcceso, request.Usuario.Clave);
                if (usuario.Resultado)
                {
                    response = new Response()
                    {
                        Usuario = usuario,
                        Resultado = true
                    };
                    return response;
                }
                else
                {
                    return new Response() { Resultado = false, Error = usuario.Error };
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.VerificarUsuario", error, 2, "Sistemas");
                return new Response() { Resultado = false, Error = string.Format("Ha sucedido una excepcion - Descripcion: {0}", error.Message) };
            }
            finally
            {
                response = null;
                usuario = null;
            }
        }

        /// <summary>
        /// Obtiene los proyectos
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string ObtenerProyectos()
        {
            JavaScriptSerializer jscript = null;
            Proyectos proyectos = null;

            try
            {
                proyectos = new Proyectos();

                jscript = new JavaScriptSerializer();
                return jscript.Serialize(proyectos.Obtener());
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.ObtenerProyectos", error, 2, "Sistemas");
                throw error;
            }
            finally
            {
                proyectos = null;
                jscript = null;
            }
        }

        /// <summary>
        /// Obtiene las tareaa en base al proyecto que se le envia
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string ObtenerTareas(string proyectoId)
        {
            JavaScriptSerializer jscript = null;
            Tareas tareas = null;

            try
            {
                tareas = new Tareas();
                tareas.ProyectoId = int.Parse(proyectoId);

                jscript = new JavaScriptSerializer();
                return jscript.Serialize(tareas.Obtener());
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.ObtenerProyectos", error, 2, "Sistemas");
                throw error;
            }
            finally
            {
                tareas = null;
                jscript = null;
            }
        }

        /// <summary>
        /// Obtiene las tareaa en base al proyecto que se le envia
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string ObtenerActores(string tipo)
        {
            JavaScriptSerializer jscript = null;
            Usuario usuario = null;
            Usuarios usuarios = null;
            try
            {
                usuario = new Usuario();
                usuario.Id = int.Parse(tipo);

                jscript = new JavaScriptSerializer();

                usuarios = usuario.ObtenerActores();
                return jscript.Serialize(usuarios.ListaUsuarios);
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.ObtenerActores", error, 2, "Sistemas");
                throw error;
            }
            finally
            {
                usuario = null;
                jscript = null;
                usuarios = null;
            }
        }

        #endregion

        #region "Métodos Privados"
        /// <summary>
        /// comprueba si el usuario existe en la bbdd
        /// </summary>
        /// <returns></returns>
        public Usuario verificarUsuario(string codigoAcceso, string clave)
        {
            Usuario usuario = null;
            SqlDataReader reader = null;
            SqlCommand cmd = null;
            SqlConnection con = null;

            try
            {
                using (con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (cmd = new SqlCommand("VerificaUsuarioSEL", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Usuario", SqlDbType.NVarChar).Value = codigoAcceso;
                        cmd.Parameters.Add("@Contrasena", SqlDbType.NVarChar).Value = clave;

                        con.Open();
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            usuario = new Usuario()
                            {
                                Id = (int)reader["Id"],
                                Nombre = reader["UsuarioNombre"] != DBNull.Value ? (string)reader["UsuarioNombre"] : null,
                                CodigoAcceso = reader["UsuarioCodigo"] != DBNull.Value ? (string)reader["UsuarioCodigo"] : null,
                                Mail = reader["UsuarioMail"] != DBNull.Value ? (string)reader["UsuarioMail"] : null,
                                SectorId = reader["UsuarioSector"] != DBNull.Value ? (int)reader["UsuarioSector"] : 0,
                                Permiso = reader["Permisos"] == DBNull.Value ? new List<Permiso>() : Serializar.Deserialize<List<Permiso>>(Convert.ToString(reader["Permisos"]))
                            };
                        }
                    }
                }
                return usuario;
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.verificarUsuario", error, usuario.Id, "SolicitudPersonal");
                return null;
            }
            finally
            {
                reader = null;
                cmd = null;
                con = null;
                usuario = null;
            }
        }

        private void enviarMensajeTarea(Request request, int estadoAnterior)
        {
            wsXRM.Request xrmRequest = null;
            wsXRM.Response xrmResponse = null;
            wsXRM.Mensaje mensaje = null;
            wsXRM.wsXRM xrmServicio = null;
            Proyecto proyecto = null;
            string destinatarioMail = null;
            bool enviarCorreo = false;
            try
            {
                mensaje = new wsXRM.Mensaje();
                xrmRequest = new wsXRM.Request();
                xrmServicio = new wsXRM.wsXRM();

                xrmRequest.Usuario = new wsXRM.Usuario()
                {
                    Nombre = request.Usuario.Nombre,
                    CodigoAcceso = request.Usuario.CodigoAcceso,
                    Clave = request.Usuario.Clave,
                    Id = request.Usuario.Id
                };
                switch (request.Tarea.EstadoId)
                {
                    //Asignada
                    case 1:
                        if (estadoAnterior == 0)
                        {
                            mensaje.ProcesoId = request.Tarea.Id;
                            mensaje.DestinatarioId = request.Tarea.DesarrolladorId;
                            mensaje.EmisorId = request.Usuario.Id;
                            mensaje.UsuarioId = request.Usuario.Id;
                            mensaje.Contenido = string.Format("{0}: Tarea {1}, avanzó a Estado: {2}.", request.Usuario.Nombre, request.Tarea.Nombre, request.Tarea.EstadoNombre);
                            mensaje.Url = string.Format("/foSistemas/Tarea.aspx?Id={0}&Usuario=", request.Tarea.Id);
                            mensaje.Estilo = "fa fa-fw fa-certificate fa";
                            xrmRequest.Mensaje = mensaje;
                            xrmResponse = xrmServicio.NuevoMensaje(xrmRequest);
                            destinatarioMail = request.Tarea.DesarrolladorMail;
                            enviarCorreo = true;
                        }
                        break;
                    //Por Instalar
                    case 3:
                        if (estadoAnterior == 1)
                        {
                            proyecto = new Proyecto() { Id = request.Tarea.ProyectoId };
                            proyecto = proyecto.Cargar();
                            mensaje.ProcesoId = request.Tarea.Id;
                            mensaje.DestinatarioId = proyecto.CoordinadorId;
                            mensaje.EmisorId = request.Usuario.Id;
                            mensaje.UsuarioId = request.Usuario.Id;
                            mensaje.Contenido = string.Format("{0}: Tarea {1}, avanzó a Estado: {2}.", request.Usuario.Nombre, request.Tarea.Nombre, request.Tarea.EstadoNombre);
                            mensaje.Url = string.Format("/foSistemas/Tarea.aspx?Id={0}&Usuario=", request.Tarea.Id);
                            mensaje.Estilo = "fa fa-fw fa-certificate fa";
                            xrmRequest.Mensaje = mensaje;
                            xrmResponse = xrmServicio.NuevoMensaje(xrmRequest);
                            destinatarioMail = request.Tarea.ProyectoCoordinadorMail;
                            enviarCorreo = true;
                        }
                        break;
                    //Testeando
                    case 4:
                        if (estadoAnterior == 3)
                        {
                            mensaje.ProcesoId = request.Tarea.Id;
                            mensaje.DestinatarioId = request.Tarea.TesterId;
                            mensaje.EmisorId = request.Usuario.Id;
                            mensaje.UsuarioId = request.Usuario.Id;
                            mensaje.Contenido = string.Format("{0}: Tarea {1}, avanzó a Estado: {2}.", request.Usuario.Nombre, request.Tarea.Nombre, request.Tarea.EstadoNombre);
                            mensaje.Url = string.Format("/foSistemas/Tarea.aspx?Id={0}&Usuario=", request.Tarea.Id);
                            mensaje.Estilo = "fa fa-fw fa-certificate fa";
                            xrmRequest.Mensaje = mensaje;
                            xrmResponse = xrmServicio.NuevoMensaje(xrmRequest);
                            destinatarioMail = request.Tarea.TesterMail;
                            enviarCorreo = true;
                        }
                        break;
                    //Con Errores
                    case 6:
                        if (estadoAnterior == 4)
                        {
                            mensaje.ProcesoId = request.Tarea.Id;
                            mensaje.DestinatarioId = request.Tarea.DesarrolladorId;
                            mensaje.EmisorId = request.Usuario.Id;
                            mensaje.UsuarioId = request.Usuario.Id;
                            mensaje.Contenido = string.Format("{0}: Tarea {1}, avanzó a Estado: {2}.", request.Usuario.Nombre, request.Tarea.Nombre, request.Tarea.EstadoNombre);
                            mensaje.Url = string.Format("/foSistemas/Tarea.aspx?Id={0}&Usuario=", request.Tarea.Id);
                            mensaje.Estilo = "fa fa-fw fa-certificate fa";
                            xrmRequest.Mensaje = mensaje;
                            xrmResponse = xrmServicio.NuevoMensaje(xrmRequest);
                            destinatarioMail = request.Tarea.DesarrolladorMail;
                            enviarCorreo = true;
                        }
                        break;
                    //Testado
                    case 5:
                        if (estadoAnterior == 4)
                        {
                            proyecto = new Proyecto() { Id = request.Tarea.ProyectoId };
                            proyecto = proyecto.Cargar();
                            mensaje.ProcesoId = request.Tarea.Id;
                            mensaje.DestinatarioId = proyecto.CoordinadorId;
                            mensaje.EmisorId = request.Usuario.Id;
                            mensaje.UsuarioId = request.Usuario.Id;
                            mensaje.Contenido = string.Format("{0}: Tarea {1}, avanzó a Estado: {2}.", request.Usuario.Nombre, request.Tarea.Nombre, request.Tarea.EstadoNombre);
                            mensaje.Url = string.Format("/foSistemas/Tarea.aspx?Id={0}&Usuario=", request.Tarea.Id);
                            mensaje.Estilo = "fa fa-fw fa-certificate fa";
                            xrmRequest.Mensaje = mensaje;
                            xrmResponse = xrmServicio.NuevoMensaje(xrmRequest);
                            destinatarioMail = request.Tarea.ProyectoCoordinadorMail;
                            enviarCorreo = true;
                        }
                        break;
                }
                if (enviarCorreo)
                { this.enviarMailTarea(request.Tarea, destinatarioMail); }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.enviarMensajeTarea", error, 2, "Sistemas");
            }
            finally
            {
                xrmRequest = null;
                xrmResponse = null;
                xrmServicio = null;
                mensaje = null;
                proyecto = null;
                destinatarioMail = null;
            }
        }

        private void enviarMensajeProyecto(Request request, int estadoAnterior)
        {
            wsXRM.Request xrmRequest = null;
            wsXRM.Response xrmResponse = null;
            wsXRM.Mensaje mensaje = null;
            wsXRM.wsXRM xrmServicio = null;
            bool enviarCorreo = false;
            try
            {
                mensaje = new wsXRM.Mensaje();
                xrmRequest = new wsXRM.Request();
                xrmServicio = new wsXRM.wsXRM();

                xrmRequest.Usuario = new wsXRM.Usuario()
                {
                    Nombre = request.Usuario.Nombre,
                    CodigoAcceso = request.Usuario.CodigoAcceso,
                    Clave = request.Usuario.Clave,
                    Id = request.Usuario.Id
                };
                switch (request.Proyecto.EstadoId)
                {
                    //Desarrollando
                    case 3:
                        if (estadoAnterior == 2)
                        {
                            mensaje.ProcesoId = request.Proyecto.Id;
                            mensaje.DestinatarioId = request.Proyecto.ResponsableId;
                            mensaje.EmisorId = request.Usuario.Id;
                            mensaje.UsuarioId = request.Usuario.Id;
                            mensaje.Contenido = string.Format("{0}: Proyecto: {1}, avanzó a Estado: {2}.", request.Usuario.Nombre, request.Proyecto.Nombre, request.Proyecto.EstadoNombre);
                            mensaje.Url = string.Format("/foSistemas/Proyecto.aspx?Id={0}&Usuario=", request.Proyecto.Id);
                            mensaje.Estilo = "fa fa-fw fa-certificate fa";
                            xrmRequest.Mensaje = mensaje;
                            xrmResponse = xrmServicio.NuevoMensaje(xrmRequest);
                            enviarCorreo = true;
                        }
                        break;

                    //Por Verificar
                    case 5:
                        if (estadoAnterior == 3)
                        {
                            mensaje.ProcesoId = request.Proyecto.Id;
                            mensaje.DestinatarioId = request.Proyecto.ResponsableId;
                            mensaje.EmisorId = request.Usuario.Id;
                            mensaje.UsuarioId = request.Usuario.Id;
                            mensaje.Contenido = string.Format("{0}: Proyecto: {1}, avanzó a Estado: {2}.", request.Usuario.Nombre, request.Proyecto.Nombre, request.Proyecto.EstadoNombre);
                            mensaje.Url = string.Format("/foSistemas/Proyecto.aspx?Id={0}&Usuario=", request.Proyecto.Id);
                            mensaje.Estilo = "fa fa-fw fa-certificate fa";
                            xrmRequest.Mensaje = mensaje;
                            xrmResponse = xrmServicio.NuevoMensaje(xrmRequest);
                            enviarCorreo = true;
                        }
                        break;
                    //Verificado
                    case 6:
                        if (estadoAnterior == 5)
                        {
                            mensaje.ProcesoId = request.Proyecto.Id;
                            mensaje.DestinatarioId = request.Proyecto.CoordinadorId;
                            mensaje.EmisorId = request.Usuario.Id;
                            mensaje.UsuarioId = request.Usuario.Id;
                            mensaje.Contenido = string.Format("{0}: Proyecto: {1}, avanzó a Estado: {2}.", request.Usuario.Nombre, request.Proyecto.Nombre, request.Proyecto.EstadoNombre);
                            mensaje.Url = string.Format("/foSistemas/Proyecto.aspx?Id={0}&Usuario=", request.Proyecto.Id);
                            mensaje.Estilo = "fa fa-fw fa-certificate fa";
                            xrmRequest.Mensaje = mensaje;
                            xrmResponse = xrmServicio.NuevoMensaje(xrmRequest);
                            enviarCorreo = true;
                        }
                        break;
                    //Implementado
                    case 4:
                        if (estadoAnterior == 6)
                        {
                            mensaje.ProcesoId = request.Proyecto.Id;
                            mensaje.DestinatarioId = request.Proyecto.ResponsableId;
                            mensaje.EmisorId = request.Usuario.Id;
                            mensaje.UsuarioId = request.Usuario.Id;
                            mensaje.Contenido = string.Format("{0}: Proyecto: {1}, avanzó a Estado: {2}.", request.Usuario.Nombre, request.Proyecto.Nombre, request.Proyecto.EstadoNombre);
                            mensaje.Url = string.Format("/foSistemas/Proyecto.aspx?Id={0}&Usuario=", request.Proyecto.Id);
                            mensaje.Estilo = "fa fa-fw fa-certificate fa";
                            xrmRequest.Mensaje = mensaje;
                            xrmResponse = xrmServicio.NuevoMensaje(xrmRequest);
                            enviarCorreo = true;
                        }
                        break;
                    //Rechazado
                    case 7:
                        if (estadoAnterior == 5)
                        {
                            mensaje.ProcesoId = request.Proyecto.Id;
                            mensaje.DestinatarioId = request.Proyecto.CoordinadorId;
                            mensaje.EmisorId = request.Usuario.Id;
                            mensaje.UsuarioId = request.Usuario.Id;
                            mensaje.Contenido = string.Format("{0}: Proyecto: {1}, avanzó a Estado: {2}.", request.Usuario.Nombre, request.Proyecto.Nombre, request.Proyecto.EstadoNombre);
                            mensaje.Url = string.Format("/foSistemas/Proyecto.aspx?Id={0}&Usuario=", request.Proyecto.Id);
                            mensaje.Estilo = "fa fa-fw fa-certificate fa";
                            xrmRequest.Mensaje = mensaje;
                            xrmResponse = xrmServicio.NuevoMensaje(xrmRequest);
                            enviarCorreo = true;
                        }
                        break;
                    //Modificado
                    case 8:
                        if (estadoAnterior == 5)
                        {
                            mensaje.ProcesoId = request.Proyecto.Id;
                            mensaje.DestinatarioId = request.Proyecto.CoordinadorId;
                            mensaje.EmisorId = request.Usuario.Id;
                            mensaje.UsuarioId = request.Usuario.Id;
                            mensaje.Contenido = string.Format("{0}: Proyecto: {1}, avanzó a Estado: {2}.", request.Usuario.Nombre, request.Proyecto.Nombre, request.Proyecto.EstadoNombre);
                            mensaje.Url = string.Format("/foSistemas/Proyecto.aspx?Id={0}&Usuario=", request.Proyecto.Id);
                            mensaje.Estilo = "fa fa-fw fa-certificate fa";
                            xrmRequest.Mensaje = mensaje;
                            xrmResponse = xrmServicio.NuevoMensaje(xrmRequest);
                            enviarCorreo = true;
                        }
                        break;
                }
                if (enviarCorreo)
                {
                    this.enviarMailProyecto(request.Proyecto);
                }

            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.enviarMensajeProyecto", error, 2, "Sistemas");
            }
            finally
            {
                xrmRequest = null;
                xrmResponse = null;
                xrmServicio = null;
                mensaje = null;
            }
        }

        private bool enviarMailTarea(Tarea tarea, string destinatarioMail)
        {
            EnviarMail enviarMail = null;
            try
            {
                if (tarea != null && destinatarioMail != null)
                {
                    enviarMail = new EnviarMail();
                    enviarMail.Asunto = string.Format("TeamFoundationId: {0} - Tarea: {1}", tarea.TFId, tarea.Nombre);
                    enviarMail.Cuerpo = string.Format("Estimados,<br /> TeamFoundation Id: {0} avanzó a Estado: {1} <br />", tarea.TFId, tarea.EstadoNombre);
                    enviarMail.Cuerpo += "Observaciones: " + tarea.Observaciones + "<br />";
                    enviarMail.Cuerpo += "Saludos!<br />Equipo PGP<br />Por favor, no respondas o reenvíes mensajes a esta cuenta.";
                    enviarMail.Cuerpo = this.repararAcentos(enviarMail.Cuerpo);
                    enviarMail.Desde = "pgpnotifica@icsred.com";
                    enviarMail.Para = destinatarioMail;
                    enviarMail.Enviar();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(1, "wsSistemas.enviarMailTarea", error, 2, "Sistemas");
                throw error;
            }
            finally
            {
                enviarMail = null;
            }
        }

        private bool enviarMailProyecto(Proyecto proyecto)
        {
            EnviarMail enviarMail = null;
            try
            {
                if (proyecto != null)
                {
                    enviarMail = new EnviarMail();
                    enviarMail.Asunto = string.Format("TeamFoundationId: {0} - Proyecto: {1}", proyecto.TFId, proyecto.Nombre);
                    enviarMail.Cuerpo = string.Format("Estimados,<br /> TeamFoundation Id: {0} avanzó a Estado: {1} <br />", proyecto.TFId, proyecto.EstadoNombre);
                    enviarMail.Cuerpo += "Observaciones: " + proyecto.Observaciones + "<br />";
                    enviarMail.Cuerpo += "Saludos!<br />Equipo PGP<br />Por favor, no respondas o reenvíes mensajes a esta cuenta.";
                    enviarMail.Cuerpo = this.repararAcentos(enviarMail.Cuerpo);
                    enviarMail.Desde = "pgpnotifica@icsred.com";
                    enviarMail.Para = proyecto.CoordinadorMail;
                    enviarMail.Enviar();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(1, "wsSistemas.enviarMailProyecto", error, 2, "Sistemas");
                throw error;
            }
            finally
            {
                enviarMail = null;
            }
        }

        private string repararAcentos(string cadenaConAcentos)
        {
            try
            {
                cadenaConAcentos = cadenaConAcentos.Replace("á", "&aacute;");
                cadenaConAcentos = cadenaConAcentos.Replace("é", "&eacute;");
                cadenaConAcentos = cadenaConAcentos.Replace("í", "&iacute;");
                cadenaConAcentos = cadenaConAcentos.Replace("ó", "&oacute;");
                cadenaConAcentos = cadenaConAcentos.Replace("ú", "&uacute;");
                cadenaConAcentos = cadenaConAcentos.Replace("Á", "&Aacute;");
                cadenaConAcentos = cadenaConAcentos.Replace("É", "&Eacute;");
                cadenaConAcentos = cadenaConAcentos.Replace("Í", "&Iacute;");
                cadenaConAcentos = cadenaConAcentos.Replace("Ó", "&Oacute;");
                cadenaConAcentos = cadenaConAcentos.Replace("Ú", "&Uacute;");
                return cadenaConAcentos;
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(1, "wsSistemas.repararAcentos", error, 2, "Sistemas");
                return cadenaConAcentos;
            }
            finally
            {
            }
        }

        #endregion
    }
}