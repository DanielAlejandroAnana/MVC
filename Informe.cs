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
    [XmlRoot("Informes")]
    /// <summary>
    /// Comportamiento de Tarea
    /// </summary>
    public class Informes
    {
        #region Propiedades Publicas

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int UsuarioId { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int ProyectoId { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int Anio { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int Mes { get; set; }

        /// <summary>
        /// lista de Trabajos
        /// </summary>
        [DataMember]
        [XmlArray("ListaInformes", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Informe", Form = XmlSchemaForm.Unqualified)]
        public List<Informe> ListaInformes { get; set; }

        #endregion

        #region Constructor
        public Informes()
        { }
        #endregion

        #region Destructor
        ~Informes()
        {
            this.ListaInformes = null;
        }
        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Retiorna la productividad por mes de un colaborador
        /// </summary>
        /// <returns></returns>
        public List<Informe> ProductividadMeses()
        {

            SqlDataReader reader = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("InformeSistemasProductividadMensual", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.Add("@Año", SqlDbType.Int).Value = this.Anio;
                        cmd.Parameters.Add("@Mes", SqlDbType.Int).Value = this.Mes;
                        cmd.Parameters.Add("@UsuarioId", SqlDbType.Int).Value = this.UsuarioId;

                        con.Open();
                        reader = cmd.ExecuteReader();

                        return this.cargar(reader, 3);
                    }
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.Informees.ProductividadMeses", error, this.UsuarioId, "Sistemas");
                throw error;
            }

            finally
            {
                reader = null;
            }
        }

        /// <summary>
        /// Retiorna la productividad por mes de un colaborador
        /// </summary>
        /// <returns></returns>
        public List<Informe> ProductividadAnual()
        {

            SqlDataReader reader = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("InformeSistemasProductividadAnual", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.Add("@Año", SqlDbType.Int).Value = this.Anio;
                        if (this.UsuarioId != 0)
                            cmd.Parameters.Add("@UsuarioId", SqlDbType.Int).Value = this.UsuarioId;

                        con.Open();
                        reader = cmd.ExecuteReader();

                        return this.cargar(reader, 3);
                    }
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.Informees.ProductividadAnual", error, this.UsuarioId, "Sistemas");
                throw error;
            }

            finally
            {
                reader = null;
            }
        }

        /// <summary>
        /// Retiorna la productividad por mes de un colaborador
        /// </summary>
        /// <returns></returns>
        public List<Informe> ProyectoEstado()
        {

            SqlDataReader reader = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("InformeSistemasProyectoEstado", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@ProyectoId", SqlDbType.Int).Value = this.ProyectoId;

                        con.Open();
                        reader = cmd.ExecuteReader();

                        return this.cargar(reader, 2);
                    }
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.Informees.ProyectoEstado", error, this.UsuarioId, "Sistemas");
                throw error;
            }

            finally
            {
                reader = null;
            }
        }

        /// <summary>
        /// Retiorna el estado de los proyectos
        /// </summary>
        /// <returns></returns>
        public List<Informe> ProyectosEstado()
        {

            SqlDataReader reader = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("InformeSistemasProyectosEstado", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        con.Open();
                        reader = cmd.ExecuteReader();

                        return this.cargar(reader, 4);
                    }
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.Informees.ProyectoEstado", error, this.UsuarioId, "Sistemas");
                throw error;
            }

            finally
            {
                reader = null;
            }
        }

        /// <summary>
        /// Retiorna las tareas y estados de un proyecto
        /// </summary>
        /// <returns></returns>
        public List<Informe> ProyectoEstadoDetalle()
        {

            SqlDataReader reader = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("InformeSistemasProyectoEstadoDetalle", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@ProyectoId", SqlDbType.Int).Value = this.ProyectoId;

                        con.Open();
                        reader = cmd.ExecuteReader();

                        return this.cargar(reader, 4);
                    }
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.Informees.ProyectoEstado", error, this.UsuarioId, "Sistemas");
                throw error;
            }

            finally
            {
                reader = null;
            }
        }

        /// <summary>
        /// Retiorna la productividad por mes de un colaborador
        /// </summary>
        /// <returns></returns>
        public List<Informe> Produccion()
        {

            SqlDataReader reader = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("InformeSistemasProduccion", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        con.Open();
                        reader = cmd.ExecuteReader();

                        return this.cargar(reader);
                    }
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.Informees.Produccion", error, this.UsuarioId, "Sistemas");
                throw error;
            }

            finally
            {
                reader = null;
            }
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Carga un Tarea
        /// </summary>
        private List<Informe> cargar(SqlDataReader reader)
        {
            return cargar(reader, 1);
        }

        /// <summary>
        /// Carga un Tarea
        /// </summary>
        private List<Informe> cargar(SqlDataReader reader, int cantidad)
        {
            Informe informe = null;

            try
            {
                this.ListaInformes = new List<Informe>();
                while (reader.Read())
                {
                    switch (cantidad)
                    {
                        case 1:
                            informe = new Informe()
                            {
                                Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                                Nombre = reader["Nombre"] != DBNull.Value ? (string)reader["Nombre"] : null,
                                Cantidad = reader["Cantidad"] != DBNull.Value ? (int)reader["Cantidad"] : 0,
                                Fecha = reader["Fecha"] != DBNull.Value ? (string)reader["Fecha"] : null
                            };
                            break;
                        case 2:
                            informe = new Informe()
                            {
                                Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                                Nombre = reader["Nombre"] != DBNull.Value ? (string)reader["Nombre"] : null,
                                Cantidad = reader["Cantidad"] != DBNull.Value ? (int)reader["Cantidad"] : 0,
                                Fecha = reader["Fecha"] != DBNull.Value ? (string)reader["Fecha"] : null,
                                Cantidad2 = reader["Cantidad2"] != DBNull.Value ? (int)reader["Cantidad2"] : 0
                            };
                            break;
                        case 3:
                            informe = new Informe()
                            {
                                Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                                Nombre = reader["Nombre"] != DBNull.Value ? (string)reader["Nombre"] : null,
                                Cantidad = reader["Cantidad"] != DBNull.Value ? (int)reader["Cantidad"] : 0,
                                Fecha = reader["Fecha"] != DBNull.Value ? (string)reader["Fecha"] : null,
                                Cantidad2 = reader["Cantidad2"] != DBNull.Value ? (int)reader["Cantidad2"] : 0,
                                Cantidad3 = reader["Cantidad3"] != DBNull.Value ? (int)reader["Cantidad3"] : 0
                            };
                            break;
                        case 4:
                            informe = new Informe()
                            {
                                Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                                Nombre = reader["Nombre"] != DBNull.Value ? (string)reader["Nombre"] : null,
                                Cantidad = reader["Cantidad"] != DBNull.Value ? (int)reader["Cantidad"] : 0,
                                Fecha = reader["Fecha"] != DBNull.Value ? (string)reader["Fecha"] : null,
                                Cantidad2 = reader["Cantidad2"] != DBNull.Value ? (int)reader["Cantidad2"] : 0,
                                Cantidad3 = reader["Cantidad3"] != DBNull.Value ? (int)reader["Cantidad3"] : 0,
                                Estado = reader["Estado"] != DBNull.Value ? (string)reader["Estado"] : null
                            };
                            break;

                    }
                    this.ListaInformes.Add(informe);
                }

                return this.ListaInformes;
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.Informes.cargar", error, this.UsuarioId, "Sistemas");
                throw error;
            }
            finally
            {
                informe = null;
            }

        }

        #endregion
    }


    [Serializable]
    [DataContract]
    [XmlRoot("Informe")]
    /// <summary>
    /// Comportamiento de Tarea
    /// </summary>
    public class Informe
    {
        #region Propiedades Publicas

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int Id { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Nombre { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int Cantidad { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Fecha { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int Cantidad2 { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int Cantidad3 { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Estado { get; set; }

        #endregion

        #region Constructor
        public Informe()
        { }
        #endregion

        #region Destructor
        ~Informe()
        {
            this.Nombre = null;
            this.Fecha = null;
            this.Estado = null;
        }
        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Carga laboral por colaborador
        /// </summary>
        /// <returns></returns>
        public List<Informe> CargaLaboral()
        {
            SqlDataReader reader = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("InformeSistemasCargaLaboral", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (this.Id != 0)
                            cmd.Parameters.Add("@ProyectoId", SqlDbType.Int).Value = this.Id;
                        con.Open();
                        reader = cmd.ExecuteReader();

                        return this.cargar(reader);
                    }
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.Informe.CargaLaboral", error, this.Id, "Sistemas");
                throw error;
            }

            finally
            {
                reader = null;
            }
        }

        /// <summary>
        /// Carga laboral por colaborador
        /// </summary>
        /// <returns></returns>
        public List<Informe> CantidadTareasEstado()
        {
            SqlDataReader reader = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("InformeSistemasCantidadTareasEstado", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (this.Id != 0)
                            cmd.Parameters.Add("@ProyectoId", SqlDbType.Int).Value = this.Id;
                        con.Open();
                        reader = cmd.ExecuteReader();

                        return this.cargar(reader);
                    }
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.Informe.CantidadTareasEstado", error, this.Id, "Sistemas");
                throw error;
            }

            finally
            {
                reader = null;
            }
        }

        #endregion Métodos Públicos

        #region Métodos Privados

        /// <summary>
        /// Carga un Tarea
        /// </summary>
        private List<Informe> cargar(SqlDataReader reader)
        {
            Informe informe = null;
            List<Informe> informes = null;
            try
            {
                informes = new List<Informe>();
                while (reader.Read())
                {
                    informe = new Informe()
                    {
                        Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                        Nombre = reader["Nombre"] != DBNull.Value ? (string)reader["Nombre"] : null,
                        Cantidad = reader["Cantidad"] != DBNull.Value ? (int)reader["Cantidad"] : 0
                    };
                    informes.Add(informe);
                }

                return informes;
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.Informe.cargar", error, this.Id, "Sistemas");
                throw error;
            }
            finally
            {
                informe = null;
                informes = null;
            }

        }

        #endregion

    }
}