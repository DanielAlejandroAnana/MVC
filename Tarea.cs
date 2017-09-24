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
    [XmlRoot("Tarea")]
    /// <summary>
    /// Comportamiento de Tarea
    /// </summary>
    public class Tarea
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
        public int ProyectoId { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ProyectoNombre { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int TipoId { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int ComplejidadId { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int HorasUsadasDesarrollo { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int HorasUsadasTesteo { get; set; }

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
        public int DesarrolladorId { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int TesterId { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int EstadoId { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ProyectoCoordinadorMail { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string TesterMail { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string DesarrolladorMail { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int ProyectoCoordnadorId { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string EstadoNombre { get; set; }

        #endregion

        #region Constructor
        public Tarea()
        { }
        #endregion

        #region Destructor
        ~Tarea()
        {
            this.Nombre = null;
            this.Observaciones = null;
            this.ProyectoCoordinadorMail = null;
            this.TesterMail = null;
            this.DesarrolladorMail = null;
            this.EstadoNombre = null;
            this.ProyectoNombre = null;
        }
        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Guarda un Tarea
        /// </summary>
        public Tarea Guardar()
        {
            SqlDataReader reader = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SistemasTareaINUP", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (!this.Id.Equals(0))
                            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = this.Id;
                        cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = this.Nombre;
                        cmd.Parameters.Add("@ProyectoId", SqlDbType.Int).Value = this.ProyectoId;
                        cmd.Parameters.Add("@TipoId", SqlDbType.Int).Value = this.TipoId;
                        cmd.Parameters.Add("@ComplejidadId", SqlDbType.Int).Value = this.ComplejidadId;
                        cmd.Parameters.Add("@HorasUsadasDesarrollo", SqlDbType.Int).Value = this.HorasUsadasDesarrollo;
                        cmd.Parameters.Add("@HorasUsadasTesteo", SqlDbType.Int).Value = this.HorasUsadasTesteo;
                        cmd.Parameters.Add("@Borrado", SqlDbType.Bit).Value = this.Borrado;
                        cmd.Parameters.Add("@UsuarioId", SqlDbType.Int).Value = this.UsuarioId;
                        cmd.Parameters.Add("@TFId", SqlDbType.Int).Value = this.TFId;
                        cmd.Parameters.Add("@EstadoId", SqlDbType.Int).Value = this.EstadoId;
                        cmd.Parameters.Add("@DesarrolladorId", SqlDbType.Int).Value = this.DesarrolladorId;
                        cmd.Parameters.Add("@TesterId", SqlDbType.Int).Value = this.TesterId;
                        cmd.Parameters.Add("@Observaciones", SqlDbType.NVarChar).Value = this.Observaciones;
                        con.Open();
                        reader = cmd.ExecuteReader();
                        return this.cargar(reader);

                    }
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "Tarea.Guardar", error, this.Id, "Tarea");
                throw error;
            }

            finally
            {
                reader = null;
            }
        }

        /// <summary>
        /// Elimina un Tarea
        /// </summary>
        public void Eliminar()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SistemasTareaDEL", con))
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
                ManejoExcepciones.Logeo(2, "Tarea.Eliminar", error, this.Id, "Tarea");
                throw error;
            }
            finally
            {
            }
        }

        /// <summary>
        /// Carga un Tarea
        /// </summary>
        public Tarea Cargar()
        {
            SqlDataReader reader = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SistemasTareaCargarSEL", con))
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
                ManejoExcepciones.Logeo(2, "Tarea.Cargar", error, this.Id, "Tarea");
                throw error;
            }
            finally
            {
                reader = null;
            }
        }

        /// <summary>
        /// Comprueba que el Tarea ya exista
        /// </summary>
        public bool VerificarTarea()
        {
            SqlDataReader reader = null;

            try
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SistemasTareaVerificarSEL", con))
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
                ManejoExcepciones.Logeo(2, "Tarea.VerificarTarea", error, this.Id, "Tarea");
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
        private Tarea cargar(SqlDataReader reader)
        {
            Tarea tarea = null;
            try
            {
                while (reader.Read())
                {
                    tarea = new Tarea()
                    {
                        Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                        Nombre = reader["Nombre"] != DBNull.Value ? (string)reader["Nombre"] : null,
                        ProyectoNombre = reader["ProyectoNombre"] != DBNull.Value ? (string)reader["ProyectoNombre"] : null,
                        ProyectoId = reader["ProyectoId"] != DBNull.Value ? (int)reader["ProyectoId"] : 0,
                        TipoId = reader["TipoId"] != DBNull.Value ? (int)reader["TipoId"] : 0,
                        ComplejidadId = reader["ComplejidadId"] != DBNull.Value ? (int)reader["ComplejidadId"] : 0,
                        HorasUsadasDesarrollo = reader["HorasUsadasDesarrollo"] != DBNull.Value ? (int)reader["HorasUsadasDesarrollo"] : 0,
                        HorasUsadasTesteo = reader["HorasUsadasTesteo"] != DBNull.Value ? (int)reader["HorasUsadasTesteo"] : 0,
                        Borrado = reader["Borrado"] != DBNull.Value ? (bool)reader["Borrado"] : false,
                        Observaciones = reader["Observaciones"] != DBNull.Value ? (string)reader["Observaciones"] : null,
                        TFId = reader["TFId"] != DBNull.Value ? (int)reader["TFId"] : 0,
                        EstadoId = reader["EstadoId"] != DBNull.Value ? (int)reader["EstadoId"] : 0,
                        DesarrolladorId = reader["DesarrolladorId"] != DBNull.Value ? (int)reader["DesarrolladorId"] : 0,
                        TesterId = reader["TesterId"] != DBNull.Value ? (int)reader["TesterId"] : 0,
                        ProyectoCoordnadorId = reader["CoordinadorId"] != DBNull.Value ? (int)reader["CoordinadorId"] : 0,
                        TesterMail = reader["TesterMail"] != DBNull.Value ? (string)reader["TesterMail"] : null,
                        DesarrolladorMail = reader["DesarrolladorMail"] != DBNull.Value ? (string)reader["DesarrolladorMail"] : null,
                        ProyectoCoordinadorMail = reader["CoordinadorMail"] != DBNull.Value ? (string)reader["CoordinadorMail"] : null,
                        EstadoNombre = reader["EstadoNombre"] != DBNull.Value ? (string)reader["EstadoNombre"] : null,
                        Version = reader["Version"] != DBNull.Value ? (int)reader["Version"] : 0
                    };

                }

                return tarea;
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "wsSistemas.Tarea.cargar", error, this.Id, "Sistemas");
                throw error;
            }
            finally
            {
                tarea = null;
            }

        }
        #endregion

    }
}