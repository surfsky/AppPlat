using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Data;

namespace Kingsoc.Data.Metadata.SqlServer
{
    /// <summary>
    /// SqlServer table metadata
    /// </summary>
    [Serializable]
    public class Table : IComparable<Table>
    {
        [XmlAttribute]  public string Catalog;
        [XmlAttribute]  public string Schema;
        [XmlAttribute]  public string Name;
        [XmlAttribute]  public string Type;  // BASE TABLE / VIEW

        [XmlIgnore]
        public string FullName
        {
            get { return string.Format("{0}.{1}.{2}", Catalog, Schema, Name); }
        }

        public Table() { }
        public Table(DataRow row)
        {
            Catalog     = DbCommon.ToString(row["TABLE_CATALOG"]);
            Schema      = DbCommon.ToString(row["TABLE_SCHEMA"]);
            Name        = DbCommon.ToString(row["TABLE_NAME"]);
            Type        = DbCommon.ToString(row["TABLE_TYPE"]);
        }

        // 排序用
        public int CompareTo(Table other)
        {
            return this.Schema == other.Schema
                ? this.Name.CompareTo(other.Name)
                : this.Schema.CompareTo(other.Schema)
                ;
        }
    }
}
