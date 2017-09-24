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
    [XmlRoot("Proyecto")]
    /// <summary>
    /// Comportamiento de Proyecto
    /// </summary>
    public class Proyecto
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
        public int UsuarioId { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public bool Borrado { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int EstadoId { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public DateTime FechaInicio { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public DateTime FechaFin { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int TFId { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int Version { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Observaciones { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int CoordinadorId { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int ResponsableId { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string EstadoNombre { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string CoordinadorMail { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ResponsableMail { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int TareasDuracion { get; set; }

        #endregion

        #region Constructor
        public Proyecto()
        { }
        #endregion

        #region Destructor
        ~Proyecto()
        {
            this.Nombre = null;
            this.Observaciones = null;
            this.CoordinadorMail = null;
            this.ResponsableMail = null;
            this.EstadoNombre = null;
        }
        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Guarda un Proyecto
        /// </summary>
        public Proyecto Guardar()
        {
            SqlDataReader reader = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SistemasProyectoINUP", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (!this.Id.Equals(0))
                            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = this.Id;
                        cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = this.Nombre;
                        cmd.Parameters.Add("@Borrado", SqlDbType.Bit).Value = this.Borrado;
                        cmd.Parameters.Add("@FechaInicio", SqlDbType.DateTime).Value = this.FechaInicio;
                        cmd.Parameters.Add("@FechaFin", SqlDbType.DateTime).Value = this.FechaFin;
                        cmd.Parameters.Add("@UsuarioId", SqlDbType.Int).Value = this.UsuarioId;
                        cmd.Parameters.Add("@TFId", SqlDbType.Int).Value = this.TFId;
                        cmd.Parameters.Add("@EstadoId", SqlDbType.Int).Value = this.EstadoId;
                        cmd.Parameters.Add("@CoordinadorId", SqlDbType.Int).Value = this.CoordinadorId;
                        cmd.Parameters.Add("@ResponsableId", SqlDbType.Int).Value = this.ResponsableId;
                        cmd.Parameters.Add("@Observaciones", SqlDbType.NVarChar).Value = this.Observaciones;
                        con.Open();
                        reader = cmd.ExecuteReader();
                        return this.cargar(reader);

                    }
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "Proyecto.Guardar", error, this.Id, "Proyecto");
                throw error;
            }

            finally
            {
                reader = null;
            }
        }

        /// <summary>
        /// Elimina un Proyecto
        /// </summary>
        public void Eliminar()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SistemasProyectoDEL", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = this.Id;
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "Proyecto.Eliminar", error, this.Id, "Proyecto");
                throw error;
            }
            finally
            {
            }
        }


        /// <summary>
        /// Carga un Proyecto
        /// </summary>
        public Proyecto Cargar()
        {
            SqlDataReader reader = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SistemasProyectoCargarSEL", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = this.Id;

                        con.Open();
                        reader = cmd.ExecuteReader();
                        return this.cargar(reader);
                    }
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "Proyecto.Cargar", error, this.Id, "Proyecto");
                throw error;
            }
            finally
            {
                reader = null;
            }
        }

        /// <summary>
        /// Comprueba que el Proyecto ya exista
        /// </summary>
        public bool VerificarProyecto()
        {
            SqlDataReader reader = null;

            try
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SistemasProyectoVerificarSEL", con))
                    {

                        {
                            if (this.Id == 0)
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = this.Nombre;
                                cmd.Parameters.Add("@TFId", SqlDbType.Int).Value = this.TFId;

                                con.Open();
                                reader = cmd.ExecuteReader();
                                return reader.HasRows;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "Proyecto.VerificarProyecto", error, this.Id, "Proyecto");
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
        /// Carga un Proyecto
        /// </summary>
        private Proyecto cargar(SqlDataReader reader)
        {
            Proyecto Proyecto = null;
            try
            {
                while (reader.Read())
                {
                    Proyecto = new Proyecto()
                    {
                        Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                        Nombre = reader["Nombre"] != DBNull.Value ? (string)reader["Nombre"] : null,
                        FechaInicio = reader["FechaInicio"] != DBNull.Value ? (DateTime)reader["FechaInicio"] : new DateTime(),
                        FechaFin = reader["FechaFin"] != DBNull.Value ? (DateTime)reader["FechaFin"] : new DateTime(),
                        Borrado = reader["Borrado"] != DBNull.Value ? (bool)reader["Borrado"] : false,
                        Observaciones = reader["Observaciones"] != DBNull.Value ? (string)reader["Observaciones"] : null,
                        TFId = reader["TFId"] != DBNull.Value ? (int)reader["TFId"] : 0,
                        Version = reader["Version"] != DBNull.Value ? (int)reader["Version"] : 0,
                        EstadoId = reader["EstadoId"] != DBNull.Value ? (int)reader["EstadoId"] : 0,
                        CoordinadorId = reader["CoordinadorId"] != DBNull.Value ? (int)reader["CoordinadorId"] : 0,
                        ResponsableId = reader["ResponsableId"] != DBNull.Value ? (int)reader["ResponsableId"] : 0,
                        EstadoNombre = reader["EstadoNombre"] != DBNull.Value ? (string)reader["EstadoNombre"] : null,
                        CoordinadorMail = reader["CoordinadorMail"] != DBNull.Value ? (string)reader["CoordinadorMail"] : null,
                        ResponsableMail = reader["ResponsableMail"] != DBNull.Value ? (string)reader["ResponsableMail"] : null
                    };

                }

                return Proyecto;
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.cargar", error, this.Id, "Proyecto");
                throw error;
            }
            finally
            {
                Proyecto = null;
            }

        }
        #endregion

    }
}