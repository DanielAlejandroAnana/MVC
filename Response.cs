using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml.Schema;
using ICSRED.Usuario;
using ICSRED.API;

namespace wsSistemas
{
    [Serializable]
    [DataContract]
    [XmlRoot("Response")]
    /// <summary>
    /// 
    /// </summary>
    public class Response
    {
        #region Propiedades Publicas
        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public bool Resultado { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Error { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public Usuario Usuario { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public Proyectos Proyectos { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public Proyecto Proyecto { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public Tareas Tareas { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public Tarea Tarea { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public Trabajos Trabajos { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public Trabajo Trabajo { get; set; }

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
        public Personas Personas { get; set; }

        #endregion

        #region Constructor

        public Response()
        {

        }

        #endregion

        #region Destructor

        ~Response()
        {
            this.Error = null;
            this.Usuario = null;
            this.Proyectos = null;
            this.Proyecto = null;
            this.Tarea = null;
            this.Tareas = null;
            this.Trabajos = null;
            this.Trabajo = null;
            this.Personas = null;
        }

        #endregion

    }
}