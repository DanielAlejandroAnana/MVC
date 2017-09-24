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
    [XmlRoot("Trabajos")]
    /// <summary>
    /// Comportamiento de Trabajo
    /// </summary>
    public class Trabajos
    {
        #region Propiedades Publicas

        /// <summary>
        /// Id del Trabajo
        /// </summary>
        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int Id { get; set; }

        /// <summary>
        /// Nombre del Trabajo
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
        /// Id del Trabajo en TF
        /// </summary>
        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int ProyectoId { get; set; }

        /// <summary>
        /// lista de Trabajos
        /// </summary>
        [DataMember]
        [XmlArray("ListaTrabajos", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Trabajos", Form = XmlSchemaForm.Unqualified)]
        public List<Trabajo> ListaTrabajos { get; set; }

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

        #endregion

        #region Constructor
        public Trabajos()
        { }
        #endregion

        #region Destructor
        ~Trabajos()
        {
            this.Nombre = null;
            this.Ordenamiento = null;
            this.AscendenteDescendiente = null;
            this.ListaTrabajos = null;
        }
        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Busca un Trabajo
        /// </summary>
        public List<Trabajo> Buscar()
        {
            List<Trabajo> Trabajos = null;
            SqlDataReader reader = null;
            try
            {
                Trabajos = new List<Trabajo>();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SistemasTrabajosSEL", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (this.Nombre != null)
                            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = this.Nombre;
                        if (this.ProyectoId != 0)
                            cmd.Parameters.Add("@ProyectoId", SqlDbType.Int).Value = this.ProyectoId;
                        cmd.Parameters.Add("@Borrado", SqlDbType.Bit).Value = this.Borrado;
                        cmd.Parameters.Add("@Inicio", SqlDbType.Int).Value = this.Inicio;
                        cmd.Parameters.Add("@PaginaNumero", SqlDbType.Int).Value = this.PaginaNumero;
                        cmd.Parameters.Add("@PaginaTamano", SqlDbType.Int).Value = this.PaginaTamaño;
                        cmd.Parameters.Add("@Ordenamiento", SqlDbType.NVarChar).Value = this.Ordenamiento;
                        cmd.Parameters.Add("@AscendenteDescendente", SqlDbType.NVarChar).Value = this.AscendenteDescendiente;

                        con.Open();
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Trabajos.Add(new Trabajo()
                            {
                                Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                                ProyectoId = reader["ProyectoId"] != DBNull.Value ? (int)reader["ProyectoId"] : 0,
                                TipoId = reader["TipoId"] != DBNull.Value ? (int)reader["TipoId"] : 0,
                                Horas = reader["Horas"] != DBNull.Value ? (int)reader["Horas"] : 0,
                                Borrado = reader["Borrado"] != DBNull.Value ? (bool)reader["Borrado"] : false,
                                Observaciones = reader["Observaciones"] != DBNull.Value ? (string)reader["Observaciones"] : null,
                                TareaId = reader["TareaId"] != DBNull.Value ? (int)reader["TareaId"] : 0,
                                ProyectoNombre = reader["ProyectoNombre"] != DBNull.Value ? (string)reader["ProyectoNombre"] : "",
                                TareaNombre = reader["TareaNombre"] != DBNull.Value ? (string)reader["TareaNombre"] : ""
                            });
                            this.PaginaTotal = reader["PaginaTotal"] == DBNull.Value ? 0 : (int)reader["PaginaTotal"];
                        }

                        return Trabajos;
                    }
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "Trabajos.Buscar", error, this.Id, "Trabajo");
                throw error;
            }
            finally
            {
                reader = null;
                Trabajos = null;
            }
        }

        #endregion Métodos Públicos

    }
}