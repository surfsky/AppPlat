using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Serialization;

namespace Kingsoc.Data.Metadata.SqlServer
{
    /// <summary>
    /// SqlServer column metadata
    /// </summary>
    [Serializable]
    public class Column
    {
        [XmlAttribute]public string TableCatalog;
        [XmlAttribute]public string TableSchema;
        [XmlAttribute]public string TableName;
        [XmlAttribute]public string Name;
        [XmlAttribute]public int OrdinalPosition;
        [XmlAttribute]public string Defalut;
        [XmlAttribute]public bool IsNullbable;
        [XmlAttribute]public string DataType;
        [XmlAttribute]public int CharacterMaximumLength;
        [XmlAttribute]public int CharacterOctetLength;
        [XmlAttribute]public int NumericPrecision;
        [XmlAttribute]public int NumericPrecisionRadix;
        [XmlAttribute]public int NumericScale;
        [XmlAttribute]public int DateTimePrecision;
        [XmlAttribute]public string CharacterSetCatalog;
        [XmlAttribute]public string CharacterSetSchema;
        [XmlAttribute]public string CharacterSetName;
        [XmlAttribute]public string CollationCatalog;

        // 2017-08 add
        [XmlAttribute] public bool Identity;
        [XmlAttribute] public string Description;

        [XmlIgnore]
        public string TableFullName
        {
            get { return string.Format("{0}.{1}.{2}", TableCatalog, TableSchema, TableName); }
        }

        public Column() { }
        public Column(DataRow row)
        {
            TableCatalog            = DbCommon.ToString(row["TABLE_CATALOG"]);
            TableSchema             = DbCommon.ToString(row["TABLE_SCHEMA"]);
            TableName               = DbCommon.ToString(row["TABLE_NAME"]);
            Name                    = DbCommon.ToString(row["COLUMN_NAME"]);
            OrdinalPosition         = DbCommon.ToInt32(row["ORDINAL_POSITION"]);
            Defalut                 = DbCommon.ToString(row["COLUMN_DEFAULT"]);
            IsNullbable             = DbCommon.ToString(row["IS_NULLABLE"]) == "Yes";
            DataType                = DbCommon.ToString(row["DATA_TYPE"]);
            CharacterMaximumLength  = DbCommon.ToInt32(row["CHARACTER_MAXIMUM_LENGTH"]);
            CharacterOctetLength    = DbCommon.ToInt32(row["CHARACTER_OCTET_LENGTH"]);
            NumericPrecision        = DbCommon.ToInt32(row["NUMERIC_PRECISION"]);
            NumericPrecisionRadix   = DbCommon.ToInt32(row["NUMERIC_PRECISION_RADIX"]);
            NumericScale            = DbCommon.ToInt32(row["NUMERIC_SCALE"]);
            DateTimePrecision       = DbCommon.ToInt32(row["DATETIME_PRECISION"]);
            CharacterSetCatalog     = DbCommon.ToString(row["CHARACTER_SET_CATALOG"]);
            CharacterSetSchema      = DbCommon.ToString(row["CHARACTER_SET_SCHEMA"]);
            CharacterSetName        = DbCommon.ToString(row["CHARACTER_SET_NAME"]);
            CollationCatalog        = DbCommon.ToString(row["COLLATION_CATALOG"]);
        }
    }
}
