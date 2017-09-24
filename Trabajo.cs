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
    [XmlRoot("Trabajo")]
    /// <summary>
    /// Comportamiento de Trabajo
    /// </summary>
    public class Trabajo
    {
        #region Propiedades Publicas

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int Id { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int UsuarioId { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public bool Borrado { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int ProyectoId { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int TareaId { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ProyectoNombre { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string TareaNombre { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int TipoId { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int Horas { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int Version { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Observaciones { get; set; }

        #endregion

        #region Constructor
        public Trabajo()
        { }
        #endregion

        #region Destructor
        ~Trabajo()
        {
            this.Observaciones = null;
            this.ProyectoNombre = null;
            this.TareaNombre = null;
        }
        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Guarda un Trabajo
        /// </summary>
        public Trabajo Guardar()
        {
            SqlDataReader reader = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SistemasTrabajoINUP", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (!this.Id.Equals(0))
                            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = this.Id;

                        cmd.Parameters.Add("@ProyectoId", SqlDbType.Int).Value = this.ProyectoId;
                        cmd.Parameters.Add("@TareaId", SqlDbType.Int).Value = this.TareaId;
                        cmd.Parameters.Add("@TipoId", SqlDbType.Int).Value = this.TipoId;
                        cmd.Parameters.Add("@Horas", SqlDbType.Int).Value = this.Horas;
                        cmd.Parameters.Add("@Borrado", SqlDbType.Bit).Value = this.Borrado;
                        cmd.Parameters.Add("@UsuarioId", SqlDbType.Int).Value = this.UsuarioId;
                        cmd.Parameters.Add("@Observaciones", SqlDbType.NVarChar).Value = this.Observaciones;
                        con.Open();
                        reader = cmd.ExecuteReader();
                        return this.cargar(reader);

                    }
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "Trabajo.Guardar", error, this.Id, "Trabajo");
                throw error;
            }

            finally
            {
                reader = null;
            }
        }

        /// <summary>
        /// Elimina un Trabajo
        /// </summary>
        public void Eliminar()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SistemasTrabajoDEL", con))
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
                ManejoExcepciones.Logeo(2, "Trabajo.Eliminar", error, this.Id, "Trabajo");
                throw error;
            }
            finally
            {
            }
        }

        /// <summary>
        /// Carga un Trabajo
        /// </summary>
        public Trabajo Cargar()
        {
            SqlDataReader reader = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SistemasTrabajoCargarSEL", con))
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
                ManejoExcepciones.Logeo(2, "Trabajo.Cargar", error, this.Id, "Trabajo");
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
        /// Carga un Trabajo
        /// </summary>
        private Trabajo cargar(SqlDataReader reader)
        {
            Trabajo Trabajo = null;
            try
            {
                while (reader.Read())
                {
                    Trabajo = new Trabajo()
                    {
                        Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                        ProyectoId = reader["ProyectoId"] != DBNull.Value ? (int)reader["ProyectoId"] : 0,
                        TipoId = reader["TipoId"] != DBNull.Value ? (int)reader["TipoId"] : 0,
                        ProyectoNombre = reader["ProyectoNombre"] != DBNull.Value ? (string)reader["ProyectoNombre"] : "",
                        TareaNombre = reader["TareaNombre"] != DBNull.Value ? (string)reader["TareaNombre"] : "",
                        Horas = reader["Horas"] != DBNull.Value ? (int)reader["Horas"] : 0,
                        Borrado = reader["Borrado"] != DBNull.Value ? (bool)reader["Borrado"] : false,
                        Observaciones = reader["Observaciones"] != DBNull.Value ? (string)reader["Observaciones"] : null,
                        TareaId = reader["TareaId"] != DBNull.Value ? (int)reader["TareaId"] : 0,
                        Version = reader["Version"] != DBNull.Value ? (int)reader["Version"] : 0
                    };

                }

                return Trabajo;
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.cargar", error, this.Id, "Trabajo");
                throw error;
            }
            finally
            {
                Trabajo = null;
            }

        }
        #endregion

    }
}