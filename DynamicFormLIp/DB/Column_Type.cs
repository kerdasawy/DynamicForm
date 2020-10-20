using System;
using System.Data.SqlClient;
using DynamicFormLIp.Common;
using NLog;

namespace DB
{
    public class Column_Type
    {
        private SqlConnection con = null;
        private SqlTransaction transaction = null;
        public const String _Name = "Column_Type";
        private Logger Log = Genral_DB_Oprations.Log;
        public Column_Type(SqlConnection con, SqlTransaction transaction) { this.con = con; this.transaction = transaction; }
        public static string Name { get { return "Column_Type"; } }
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

        public Column_Type Load(int id)
        {

            var selectState = "select * from " + _Name + " where " + ID + " = " + id;
            var row = selectState.ExecuteToTable(con,transaction).Rows[0];
            this._ID = (int)row[ID];
            this._OBJ_Name = row[OBJ_Name].ToString();
            return this;
            // throw new NotImplementedException();
        }

        public Column_Type Load( string typeName)
        {

            var selectState = "select * from " + _Name + " where " + OBJ_Name + " = '" + typeName+"'";
            var rows = selectState.ExecuteToTable(con, transaction).Rows;
            if (rows.Count>0)
            {
                var row = selectState.ExecuteToTable(con, transaction).Rows[0];
                this._ID = (int)row[ID];
                this._OBJ_Name = row[OBJ_Name].ToString();
                return this;
            }

            else
            {
                return null;
            }
            // throw new NotImplementedException();
        }
        #region  Sql Statment
        public const string INSERT_STAT = @" 
 

INSERT INTO [dbo].[Column_Type]
           ([Name]
          
     VALUES
            
GO


 


           ({0} 
          )
 

";


        public const string UPDATE_STAT = @"
 

UPDATE [dbo].[Column_Type]
   SET [Name] = {0}
      
 WHERE ID = {1}
 

";
        public const string DELETE_STAT = @" 
DELETE FROM [dbo].[Column_Type]
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
                var state = string.Format(UPDATE_STAT, _OBJ_Name.SQLStr(),    _ID.SQLStr());
                state.ExcuteUpdate(con, transaction);
                //update childs.

            }
            else
            {
                //insert
                Log.Debug("Starting insert opertion");
                var state = string.Format(INSERT_STAT, _OBJ_Name.SQLStr() );
               var id =  state.ExcuteInsert(con, transaction);
                this._ID = id;
                //insert childs

            }
            return _ID;
        }
    }

}

