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
    [XmlRoot("Proyectos")]
    /// <summary>
    /// Comportamiento de Proyecto
    /// </summary>
    public class Proyectos
    {
        #region Propiedades Publicas

        /// <summary>
        /// Id del Proyecto
        /// </summary>
        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int Id { get; set; }

        /// <summary>
        /// Nombre del Proyecto
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
        /// Id del Proyecto en TF
        /// </summary>
        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int TFId { get; set; }

        /// <summary>
        /// Id del estado del Proyecto
        /// </summary>
        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int EstadoId { get; set; }

        /// <summary>
        /// lista de Proyectos
        /// </summary>
        [DataMember]
        [XmlArray("ListaProyectos", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Proyectos", Form = XmlSchemaForm.Unqualified)]
        public List<Proyecto> ListaProyectos { get; set; }

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
        public Proyectos()
        { }
        #endregion

        #region Destructor
        ~Proyectos()
        {
            this.Nombre = null;
            this.Ordenamiento = null;
            this.AscendenteDescendiente = null;
            this.ListaProyectos = null;
        }
        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Busca un Proyecto
        /// </summary>
        public List<Proyecto> Buscar()
        {
            List<Proyecto> Proyectos = null;
            SqlDataReader reader = null;
            try
            {
                Proyectos = new List<Proyecto>();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SistemasProyectosSEL", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (this.Nombre != null)
                            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = this.Nombre;
                        if (this.TFId != 0)
                            cmd.Parameters.Add("@TFId", SqlDbType.Int).Value = this.TFId;
                        if (this.EstadoId != 0)
                            cmd.Parameters.Add("@EstadoId", SqlDbType.Int).Value = this.EstadoId;
                        cmd.Parameters.Add("@Borrado", SqlDbType.Bit).Value = this.Borrado;
                        cmd.Parameters.Add("@Inicio", SqlDbType.Int).Value = this.Inicio;
                        cmd.Parameters.Add("@UsuarioId", SqlDbType.Int).Value = this.UsuarioId;
                        cmd.Parameters.Add("@PaginaNumero", SqlDbType.Int).Value = this.PaginaNumero;
                        cmd.Parameters.Add("@PaginaTamano", SqlDbType.Int).Value = this.PaginaTamaño;
                        cmd.Parameters.Add("@Ordenamiento", SqlDbType.NVarChar).Value = this.Ordenamiento;
                        cmd.Parameters.Add("@AscendenteDescendente", SqlDbType.NVarChar).Value = this.AscendenteDescendiente;

                        con.Open();
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Proyectos.Add(new Proyecto()
                            {
                                Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                                Nombre = reader["Nombre"] != DBNull.Value ? (string)reader["Nombre"] : null,
                                FechaInicio = reader["FechaInicio"] != DBNull.Value ? (DateTime)reader["FechaInicio"] : new DateTime(),
                                FechaFin = reader["FechaFin"] != DBNull.Value ? (DateTime)reader["FechaFin"] : new DateTime(),
                                Borrado = reader["Borrado"] != DBNull.Value ? (bool)reader["Borrado"] : false,
                                TFId = reader["TFId"] != DBNull.Value ? (int)reader["TFId"] : 0,
                                Observaciones = reader["Observaciones"] != DBNull.Value ? (string)reader["Observaciones"] : null,
                                EstadoNombre = reader["EstadoNombre"] != DBNull.Value ? (string)reader["EstadoNombre"] : null,
                                TareasDuracion = reader["TareasDuracion"] != DBNull.Value ? (int)reader["TareasDuracion"] : 0
                            });
                            this.PaginaTotal = reader["PaginaTotal"] == DBNull.Value ? 0 : (int)reader["PaginaTotal"];
                        }

                        return Proyectos;
                    }
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "Proyectos.Buscar", error, this.Id, "Proyecto");
                throw error;
            }
            finally
            {
                reader = null;
                Proyectos = null;
            }
        }

        public List<Proyecto> Obtener()
        {
            List<Proyecto> proyectos = null;
            SqlDataReader reader = null;

            try
            {
                proyectos = new List<Proyecto>();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SistemasProyectosObtenerSEL", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        con.Open();
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            proyectos.Add(new Proyecto()
                            {
                                Id = (int)reader["Id"],
                                Nombre = (string)reader["Nombre"]
                            });
                        }
                    }
                }

                return proyectos;
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "Proyecto.Obtener", error, 2, "Sistemas");
                throw error;
            }
            finally
            {
                proyectos = null;
                reader = null;
            }
        }

        #endregion Métodos Públicos

    }
}