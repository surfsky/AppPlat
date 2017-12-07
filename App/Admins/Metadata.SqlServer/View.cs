using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Serialization;

namespace Kingsoc.Data.Metadata.SqlServer
{
    /// <summary>
    /// SqlServer view metadata
    /// </summary>
    [Serializable]
    public class View
    {
        [XmlAttribute] public string Catalog;
        [XmlAttribute] public string Schema;
        [XmlAttribute] public string Name;
        [XmlAttribute] public string CheckOption;
        [XmlAttribute] public bool IsUpdatable;

        [XmlIgnore]
        public string FullName
        {
            get { return string.Format("{0}.{1}.{2}", Catalog, Schema, Name); }
        }

        public View() { }
        public View(DataRow row)
        {
            Catalog     = DbCommon.ToString(row["TABLE_CATALOG"]);
            Schema      = DbCommon.ToString(row["TABLE_SCHEMA"]);
            Name        = DbCommon.ToString(row["TABLE_NAME"]);
            CheckOption = DbCommon.ToString(row["CHECK_OPTION"]);
            IsUpdatable = DbCommon.ToString(row["IS_UPDATABLE"]) == "Yes";
        }

    }
}
