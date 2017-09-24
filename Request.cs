using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml.Schema;
using ICSRED.Usuario;
using ICSRED.API;

namespace wsSistemas
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract]
    [XmlRoot("Request")]
    public class Request
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

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public Persona Persona { get; set; }

        [DataMember]
        [XmlElement(Form = XmlSchemaForm.Unqualified)]

        public Personas Personas { get; set; }

        #endregion

        #region Constructor
        public Request()
        {

        }
        #endregion

        #region Destructor
        ~Request()
        {
            this.Error = null;
            this.Usuario = null;
            this.Proyectos = null;
            this.Proyecto = null;
            this.Tareas = null;
            this.Tarea = null;
            this.Trabajos = null;
            this.Trabajo = null;
            this.Persona = null;
        }
        #endregion
    }
}