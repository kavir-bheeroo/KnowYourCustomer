﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.7.3081.0.
// 
namespace KnowYourCustomer.Kyc.MrzProcessor.Abbyy.Models
{
    [SerializableAttribute]
    [XmlTypeAttribute(Namespace = "http://ocrsdk.com/schema/captureData-1.0.xsd")]
    [XmlRootAttribute("document", Namespace = "http://ocrsdk.com/schema/captureData-1.0.xsd", IsNullable = false)]
    public partial class DocumentType
    {

        private FieldType[] itemsField;

        private string typeField;

        [XmlElementAttribute("field", typeof(FieldType))]
        public FieldType[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        [XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    [SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://ocrsdk.com/schema/captureData-1.0.xsd")]
    public partial class FieldType
    {

        private string valueField;

        private SuspiciousCharType[] charactersField;

        private string typeField;

        private string documentPathField;

        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        [XmlArrayItemAttribute("char", IsNullable = false)]
        public SuspiciousCharType[] characters
        {
            get
            {
                return this.charactersField;
            }
            set
            {
                this.charactersField = value;
            }
        }

        [XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        [XmlAttributeAttribute()]
        public string documentPath
        {
            get
            {
                return this.documentPathField;
            }
            set
            {
                this.documentPathField = value;
            }
        }
    }

    [SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://ocrsdk.com/schema/captureData-1.0.xsd")]
    public partial class SuspiciousCharType
    {
        private bool suspiciousField;

        private bool suspiciousFieldSpecified;

        private string valueField;

        [XmlAttributeAttribute()]
        public bool suspicious
        {
            get
            {
                return this.suspiciousField;
            }
            set
            {
                this.suspiciousField = value;
            }
        }

        [XmlIgnoreAttribute()]
        public bool suspiciousSpecified
        {
            get
            {
                return this.suspiciousFieldSpecified;
            }
            set
            {
                this.suspiciousFieldSpecified = value;
            }
        }

        [XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
}