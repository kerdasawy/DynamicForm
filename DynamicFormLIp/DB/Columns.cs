using System;
using System.Data.SqlClient;
using DynamicFormLIp.Common;
using NLog;

namespace DB
{
    public class Columns
    {
        private SqlConnection con = null;
        private SqlTransaction transaction = null;
        public const String _Name = "Columns";
        private Logger Log = Genral_DB_Oprations.Log;
        public Columns(SqlConnection con, SqlTransaction transaction) { this.con = con; this.transaction = transaction; }
       
       
 
        public static string Name { get { return "Columns"; } }
        public static string ID { get { return "ID"; } }
        public Int32 _ID
        {
            get;
            set;
        }
        public static string OBJ_Name { get { return "Name"; } }
        public String _OBJ_Name
        {
            get;
            set;
        }
        public static string Column_Type_ID_FK { get { return "Column_Type_ID_FK"; } }
        public Int32 _Column_Type_ID_FK
        {
            get;
            set;
        }
        public static string Table_ID_FK { get { return "Table_ID_FK"; } }
        public Int32 _Table_ID_FK
        {
            get;
            set;
        }
        public static string IsRequired { get { return "isRequired"; } }
        public bool _isRequired { get; set; }
        public int Save (SqlTransaction transaction ){
            var id = this._ID;
            using (SqlConnection con = new SqlConnection(DynamicFormLIp.Properties.Resources.ConStr))
            {

            }
            return id;
        }


        public Columns Load(int id)
        {

            var selectState = "select * from " + _Name + " where " + ID + " = " + id;
            var row = selectState.ExecuteToTable(con, transaction).Rows[0];
            this._ID = (int)row[ID];
            this._OBJ_Name = row[OBJ_Name].ToString();
            this._Column_Type_ID_FK = (int)row[Column_Type_ID_FK];
            this._Table_ID_FK = (int)row[Table_ID_FK];
            this._isRequired = (bool)row[IsRequired];
            return this;
            // throw new NotImplementedException();
        }
        #region  Sql Statment
        public const string INSERT_STAT = @" 
 

INSERT INTO [dbo].[Columns]
           ([Name]
           ,[Column_Type_ID_FK]
           ,[Table_ID_FK],[isRequired] )

     VALUES 
           ({0},{1},{2} ,{3})
 

";


        public const string UPDATE_STAT = @"
UPDATE [dbo].[Columns]
   SET [Name] = {0}
      ,[Column_Type_ID_FK] = {1}
      ,[Table_ID_FK] = {2},
,[isRequired]={3}
 WHERE ID = {4}
";
        public const string DELETE_STAT = @" 
DELETE FROM [dbo].[Columns]
    WHERE  ID ={0}


";
        #endregion
        public int Save()
        {
            Log.Debug("check table   cuurent object id :" + this._ID);
            if (this._ID > 0)
            {
                Log.Debug("Starting update opertion");
                //update
                var state = string.Format(UPDATE_STAT, _OBJ_Name.SQLStr(),_Column_Type_ID_FK.SQLStr() ,_Table_ID_FK.SQLStr(),_isRequired.SQLStr() , _ID.SQLStr());
                state.ExcuteUpdate(con, transaction);
                //update childs.

            }
            else
            {
                //insert
                Log.Debug("Starting insert opertion");
                var state = string.Format(INSERT_STAT, _OBJ_Name.SQLStr(), _Column_Type_ID_FK.SQLStr(), _Table_ID_FK.SQLStr(),_isRequired.SQLStr());
                var id = state.ExcuteInsert(con, transaction);
                _ID = id;
                //insert childs

            }
            return -1;
        }
        public override string ToString()
        {
            if (this._OBJ_Name!="ID")
            {
                return string.Format("[{0}] {1}  {2} ,", _OBJ_Name, (new Column_Type(this.con, this.transaction)).Load(_Column_Type_ID_FK)._OBJ_Name,
               _isRequired ? "NOT NULL" : "NULL");
            }
            else
            {
                return @"[ID][int] IDENTITY(1, 1) NOT NULL,";
            }
           
        }
    }

}

