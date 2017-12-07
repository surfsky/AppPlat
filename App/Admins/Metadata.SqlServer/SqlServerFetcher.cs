using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System;

namespace Kingsoc.Data.Metadata.SqlServer
{
    /// <summary>
    /// SqlServer 元数据获取器
    /// </summary>
    public class SqlServerFetcher : IDisposable
    {
        private DbConnection _connection;


        public void Dispose()
        {
            if (_connection != null)
                _connection.Close();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SqlServerFetcher(string dataSource, string initDb, string user, string password)
        {
            // connection
            string connectionString = string.Format("Data Source={0}; Initial Catalog={1}; User={2}; Password={3};",
                dataSource,
                initDb,
                user,
                password
                );
            this._connection = new SqlConnection(connectionString);
        }
        public SqlServerFetcher(string connectionString)
        {
            this._connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// 获取表清单
        /// </summary>
        public List<Table> GetTables()
        {
            _connection.Open();
            DataTable dt = _connection.GetSchema("Tables");
            List<Table> tables = new List<Table>();
            foreach (DataRow row in dt.Rows)
                tables.Add(new Table(row));
            tables.Sort();
            return tables;
        }

        /// <summary>
        /// 获取视图清单
        /// </summary>
        public List<View> GetViews()
        {
            DataTable dt = _connection.GetSchema("Views");
            List<View> views = new List<View>();
            foreach (DataRow row in dt.Rows)
                views.Add(new View(row));
            return views;
        }

        /// <summary>
        /// 获取表的字段信息
        /// </summary>
        public List<Column> GetColumns(string database, string owner, string tableName)
        {
            string[] restrictionValues = new string[] { database, owner, tableName };
            DataTable dtColumns = _connection.GetSchema("Columns", restrictionValues);
            List<Column> columns = new List<Column>();
            foreach (DataRow row in dtColumns.Rows)
                columns.Add(new Column(row));
            return columns;
        }

        /// <summary>
        /// 获取Sqlserver2008字段信息
        /// </summary>
        public List<Column> GetColumns2008(string database, string owner, string tableName)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = string.Format(@"
                -- 必须在指定库内运行该语句
                SELECT
                     sch.name as schema_name
 	                ,tbl.name as table_name
	                ,col.name as column_name
	                ,tp.name  as column_type
	                ,col.max_length
	                ,col.precision
	                ,col.scale
	                ,col.is_nullable
	                ,col.is_identity
	                ,p.value as description
                FROM sys.tables tbl
                inner join sys.schemas sch on sch.schema_id=tbl.schema_id
                inner JOIN sys.columns col ON col.object_id = tbl.object_id
                left join systypes tp on  tp.xusertype=col.user_type_id
                LEFT JOIN sys.extended_properties p ON p.major_id = col.object_id AND p.minor_id = col.column_id
                WHERE sch.name = '{0}'
                  and tbl.name = '{1}'
                ", owner, tableName
                );
            cmd.CommandType = CommandType.Text;
            using (var rdr = cmd.ExecuteReader())
            {
                var columns = new List<Column>();
                while (rdr.Read())
                {
                    var col = new Column();
                    col.TableSchema = DbCommon.ToString(rdr["schema_name"]);
                    col.TableName = DbCommon.ToString(rdr["table_name"]);
                    col.Name = DbCommon.ToString(rdr["column_name"]);
                    col.DataType = DbCommon.ToString(rdr["column_type"]);
                    col.CharacterMaximumLength = DbCommon.ToInt32(rdr["max_length"]);
                    col.NumericPrecision = DbCommon.ToInt32(rdr["precision"]);
                    col.NumericScale = DbCommon.ToInt32(rdr["scale"]);
                    col.IsNullbable = DbCommon.ToBoolean(rdr["is_nullable"]);
                    col.Identity = DbCommon.ToBoolean(rdr["is_identity"]);
                    col.Description = DbCommon.ToString(rdr["description"]);
                    columns.Add(col);
                }
                return columns;
            }
        }

    }
}
