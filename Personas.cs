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
    [XmlRoot("Personas")]
    public class Personas
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
        /// lista de Proyectos
        /// </summary>
        [DataMember]
        [XmlArray("ListaPersonas", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Peronas", Form = XmlSchemaForm.Unqualified)]
        public List<Persona> ListaPersonas { get; set; }

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
        public Personas()
        {

        }
        #endregion

        #region Destructor
        ~Personas()
        {
            this.AscendenteDescendiente = null;
            this.ListaPersonas = null;
            this.Nombre = null;
            this.Ordenamiento = null;
        }
        #endregion Destructor

        #region Metodos Publicos

        public List<Persona> Buscar()
        {
            List<Persona> personas = null;
            SqlDataReader reader = null;
            try
            {
                personas = new List<Persona>();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connDaniel"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SistemasPersonasSEL", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (this.Nombre != null)
                            cmd.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = this.Nombre;

                        //Genericos 
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
                            personas.Add(new Persona()
                            {
                                Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                                Nombre = reader["Nombre"] != DBNull.Value ? (string)reader["Nombre"] : null,
                                Fecha = reader["Fecha"] != DBNull.Value ? (DateTime)reader["Fecha"] : new DateTime(),
                                CodigoAcceso = reader["CodigoAcceso"] != DBNull.Value ? (string)reader["CodigoAcceso"] : null,
                                Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : null
                            });
                            this.PaginaTotal = reader["PaginaTotal"] == DBNull.Value ? 0 : (int)reader["PaginaTotal"];
                        }

                        return personas;
                    }
                }
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "Personas.Buscar", error, this.Id, "Personas");
                throw error;
            }
            finally
            {
                reader = null;
                personas = null;
            }
        }

        #endregion Metodos Publicos

    }

}