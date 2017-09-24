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
    public class Organigrama
    {
        #region Propiedades Públicas

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int Id { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Nombre { get; set; }

        #endregion

        #region Constructor
        public Organigrama()
        { }
        #endregion

        #region Destructor
        ~Organigrama()
        {
            this.Nombre = null;

        }
        #endregion

        public List<Organigrama> Obtener()
        {
            List<Organigrama> organigramas = null;
            SqlDataReader reader = null;

            try
            {
                organigramas = new List<Organigrama>();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("ObtenerOrganigramasSEL", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        con.Open();
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            organigramas.Add(new Organigrama()
                            {
                                Id = (int)reader["Id"],
                                Nombre = (string)reader["Nombre"]
                            });
                        }
                    }
                }

                return organigramas;
            }
            catch (Exception error)
            {
                ManejoExcepciones.Logeo(2, "Organigrama.Obtener", error, 2, "UsuarioX");
                throw error;
            }
            finally
            {
                organigramas = null;
                reader = null;
            }
        }
    }


}