using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ICSRED.API;

namespace wsSistemas
{
    [Serializable]
    [DataContract]
    [XmlRoot("Tareas")]
    /// <summary>
    /// Comportamiento de Tarea
    /// </summary>
    public class Tareas
    {
        #region Propiedades Publicas

        /// <summary>
        /// Id del Tarea
        /// </summary>
        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int Id { get; set; }

        /// <summary>
        /// Nombre del Tarea
        /// </summary>
        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Nombre { get; set; }

        /// <summary>
        /// Indicador de si el legajo esta o no activo
        /// </summary>
        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public bool Borrado { get; set; }

        /// <summary>
        /// Id del Tarea en TF
        /// </summary>
        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int TFId { get; set; }

        /// <summary>
        /// Id del Tarea en TF
        /// </summary>
        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int ProyectoId { get; set; }

        /// <summary>
        /// Id del Tarea en TF
        /// </summary>
        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int EstadoId { get; set; }

        /// <summary>
        /// lista de Tareas
        /// </summary>
        [DataMember]
        [XmlArray("ListaTareas", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Tareas", Form = XmlSchemaForm.Unqualified)]
        public List<Tarea> ListaTareas { get; set; }

        /// <summary>
        /// numero de paginas totales en paginacion
        /// </summary>
        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int PaginaTotal { get; set; }

        /// <summary>
        /// numero de la pagina actual
        /// </summary>
        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int PaginaNumero { get; set; }

        /// <summary>
        /// numer de registros por pagina
        /// </summary>
        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int PaginaTamaño { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Ordenamiento { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string AscendenteDescendiente { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int Inicio { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int UsuarioId { get; set; }

        #endregion

        #region Constructor
        public Tareas()
        { }
        #endregion

        #region Destructor
        ~Tareas()
        {
            this.Nombre = null;
            this.Ordenamiento = null;
            this.AscendenteDescendiente = null;
            this.ListaTareas = null;
        }
        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Busca un Tarea
        /// </summary>
        public List<Tarea> Buscar()
        {
            List<Tarea> Tareas = null;
            SqlDataReader reader = null;
            try
            {
                Tareas = new List<Tarea>();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SistemasTareasSEL", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (this.Nombre != null)
                            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = this.Nombre;
                        if (this.TFId != 0)
                            cmd.Parameters.Add("@TFId", SqlDbType.Int).Value = this.TFId;
                        if (this.ProyectoId != 0)
                            cmd.Parameters.Add("@ProyectoId", SqlDbType.Int).Value = this.ProyectoId;
                        if (this.EstadoId > -1)
                            cmd.Parameters.Add("@EstadoId", SqlDbType.Int).Value = this.EstadoId;
                        cmd.Parameters.Add("@Borrado", SqlDbType.Bit).Value = this.Borrado;
                        cmd.Parameters.Add("@UsuarioId", SqlDbType.Int).Value = this.UsuarioId;
                        cmd.Parameters.Add("@Inicio", SqlDbType.Int).Value = this.Inicio;
                        cmd.Parameters.Add("@PaginaNumero", SqlDbType.Int).Value = this.PaginaNumero;
                        cmd.Parameters.Add("@PaginaTamano", SqlDbType.Int).Value = this.PaginaTamaño;
                        cmd.Parameters.Add("@Ordenamiento", SqlDbType.NVarChar).Value = this.Ordenamiento;
                        cmd.Parameters.Add("@AscendenteDescendente", SqlDbType.NVarChar).Value = this.AscendenteDescendiente;

                        con.Open();
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Tareas.Add(new Tarea()
                            {
                                Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                                Nombre = reader["Nombre"] != DBNull.Value ? (string)reader["Nombre"] : null,
                                ProyectoNombre = reader["ProyectoNombre"] != DBNull.Value ? (string)reader["ProyectoNombre"] : null,
                                ProyectoId = reader["ProyectoId"] != DBNull.Value ? (int)reader["ProyectoId"] : 0,
                                TipoId = reader["TipoId"] != DBNull.Value ? (int)reader["TipoId"] : 0,
                                ComplejidadId = reader["ComplejidadId"] != DBNull.Value ? (int)reader["ComplejidadId"] : 0,
                                Borrado = reader["Borrado"] != DBNull.Value ? (bool)reader["Borrado"] : false,
                                Observaciones = reader["Observaciones"] != DBNull.Value ? (string)reader["Observaciones"] : null,
                                TFId = reader["TFId"] != DBNull.Value ? (int)reader["TFId"] : 0,
                                EstadoNombre = reader["EstadoNombre"] != DBNull.Value ? (string)reader["EstadoNombre"] : null
                            });
                            this.PaginaTotal = reader["PaginaTotal"] == DBNull.Value ? 0 : (int)reader["PaginaTotal"];
                        }

                        return Tareas;
                    }
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "Tareas.Buscar", error, this.Id, "Tarea");
                throw error;
            }
            finally
            {
                reader = null;
                Tareas = null;
            }
        }

        public List<Tarea> Obtener()
        {
            List<Tarea> tareas = null;
            SqlDataReader reader = null;

            try
            {
                tareas = new List<Tarea>();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SistemasTareasObtenerSEL", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ProyectoId", SqlDbType.Int).Value = this.ProyectoId;
                        con.Open();
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            tareas.Add(new Tarea()
                            {
                                Id = (int)reader["Id"],
                                Nombre = (string)reader["Nombre"]
                            });
                        }
                    }
                }

                return tareas;
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "Tarea.Obtener", error, 2, "Sistemas");
                throw error;
            }
            finally
            {
                tareas = null;
                reader = null;
            }
        }

        #endregion Métodos Públicos

    }
}