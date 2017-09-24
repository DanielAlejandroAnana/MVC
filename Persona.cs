using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Schema;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Data;
using System.Data.Sql;
using System.Configuration;
using System.Data.SqlClient;
using ICSRED.API;


namespace wsSistemas
{
    [Serializable]
    [DataContract]
    [XmlRoot("Personas")]
    public class Persona
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
        public string Password { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public bool Borrado { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Email { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string CodigoAcceso { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public DateTime Fecha { get; set; }

        #endregion Propiedades Publicas

        #region Constructor
        public Persona()
        {

        }
        #endregion

        #region Destructor
        ~Persona()
        {
            this.Nombre = null;
            this.Password = null;
            this.CodigoAcceso = null;
            this.Email = null;
        }
        #endregion Destructor

        #region Metodos Publicos

        /// <summary>
        /// Elimina un Usuario
        /// </summary>
        /// <returns></returns>
        public bool Eliminar()
        {

            try
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connDaniel"].ConnectionString))
                {
                    if (this.Password != null && this.Nombre != null && this.CodigoAcceso != null && this.Email != null)
                    {

                        using (SqlCommand cmd = new SqlCommand("UsuarioDEL", con))
                        {

                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = this.Nombre;
                            cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = this.Email;
                            cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = this.Password;
                            cmd.Parameters.Add("@CodigoAcceso", SqlDbType.VarChar).Value = this.CodigoAcceso;

                            cmd.ExecuteNonQuery();
                            con.Close();
                            return true;

                        }
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Guarda un usuario en la base de datos
        /// </summary>
        /// <returns></returns>
        public bool Guardar()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connDaniel"].ConnectionString))
                {
                    if (this.Password != null && this.Nombre != null && this.CodigoAcceso != null && this.Email != null)
                    {
                        using (SqlCommand cmd = new SqlCommand("UsuarioINSERT", con))
                        {
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = this.Nombre;
                            cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = this.Email;
                            cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = this.Password;
                            cmd.Parameters.Add("@CodigoAcceso", SqlDbType.VarChar).Value = this.CodigoAcceso;
                            cmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = DateTime.Now;

                            cmd.ExecuteNonQuery();
                            con.Close();
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {

            }
        }


        /*public Persona Cargar()
        {
            try
            {

            }
            catch (Exception)
            {
                
                throw;
            }
        }*/

        #endregion Metodos Publicos

        #region Metodos Privados

        /*private Persona cargar(SqlDataReader reader)
        {
            try
            {

            }
            catch (Exception)
            {
                
                throw;
            }
        }*/
        #endregion
    }

}
