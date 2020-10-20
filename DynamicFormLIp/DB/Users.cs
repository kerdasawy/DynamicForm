using System;
using System.Data.SqlClient;
using DynamicFormLIp.Common;
using NLog;

namespace DB
{
    public class Users
    {
        private SqlConnection con = null;
        private SqlTransaction transaction = null;
        private Logger Log = Genral_DB_Oprations.Log;
        public const String _Name = "Users";
        
        public Users(SqlConnection con, SqlTransaction transaction) { this.con = con; this.transaction = transaction; }
       
        public static string Name { get { return "Users"; } }
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

        public Users Load(int id)
        {

            var selectState = "select * from " + _Name + " where " + ID + " = " + id;
            var row = selectState.ExecuteToTable(con, transaction).Rows[0];
            this._ID = (int)row[ID];
            this._OBJ_Name = row[OBJ_Name].ToString();
            return this;
            // throw new NotImplementedException();
        }
        #region  Sql Statment
        public const string INSERT_STAT = @" 
 

INSERT INTO [dbo].[Users]
           ([Name])
     VALUES        ({0}  )
 

";


        public const string UPDATE_STAT = @"
 

UPDATE [dbo].[Users]
   SET [Name] = {0}
      
 WHERE ID = {1}
 

";
        public const string DELETE_STAT = @" 
DELETE FROM [dbo].[Users]
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
                var state = string.Format(UPDATE_STAT, _OBJ_Name.SQLStr(), _ID.SQLStr());
                state.ExcuteUpdate(con, transaction);
                //update childs.

            }
            else
            {
                //insert
                Log.Debug("Starting insert opertion");
                var state = string.Format(INSERT_STAT, _OBJ_Name.SQLStr());
                var id = state.ExcuteInsert(con, transaction);
                this._ID = id;
                //insert childs

            }
            return _ID;
        }
    }

}

