using System;
using System.Data.SqlClient;
using DynamicFormLIp.Common;
using NLog;

namespace DB
{
    public class Elemnets_Columns
    {
        private SqlConnection con = null;
        private SqlTransaction transaction = null;
        public const String _Name = "Elemnets_Columns";
        private Logger Log = Genral_DB_Oprations.Log;
        public Elemnets_Columns(SqlConnection con, SqlTransaction transaction) { this.con = con; this.transaction = transaction; }
       
        public static string Name { get { return "Elemnets_Columns"; } }
        public static string ID { get { return "ID"; } }
        public Int32 _ID
        {
            get;
            set;
        }
        public static string Element_ID_FK { get { return "Element_ID_FK"; } }
        public Int32 _Element_ID_FK
        {
            get ;
            set;
        }
        public static string Column_ID_FK { get { return "Column_ID_FK"; } }
        public Int32 _Column_ID_FK
        {
            get;
            set;
        }

        public Elemnets_Columns Load(int id)
        {

            var selectState = "select * from " + _Name + " where " + ID + " = " + id;
            var row = selectState.ExecuteToTable(con, transaction).Rows[0];
            this._ID = (int)row[ID];
            this._Element_ID_FK = (int)row[Element_ID_FK];
            this._Column_ID_FK = (int)row[Column_ID_FK];
            return this;
            // throw new NotImplementedException();
        }
        #region  Sql Statment
        public const string INSERT_STAT = @" 
 

INSERT INTO [dbo].[Elemnets_Columns]
           ([Element_ID_FK]
           ,[Column_ID_FK])
 

     VALUES 
           ({0},{1}  )
 

";


        public const string UPDATE_STAT = @"
 
UPDATE [dbo].[Elemnets_Columns]
   SET [Element_ID_FK] = {0}
      ,[Column_ID_FK] = {1}
 
 WHERE ID = {3}
 

";
        public const string DELETE_STAT = @" 
DELETE FROM [dbo].[Elemnets_Columns]
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
                var state = string.Format(UPDATE_STAT, _Element_ID_FK.SQLStr(), _Column_ID_FK.SQLStr(), _ID.SQLStr());
                state.ExcuteUpdate(con, transaction);
                
                //update childs.

            }
            else
            {
                //insert
                Log.Debug("Starting insert opertion");
                var state = string.Format(INSERT_STAT,  _Element_ID_FK.SQLStr(), _Column_ID_FK.SQLStr());
               var id = state.ExcuteInsert(con, transaction);
                this._ID = id;

                //insert childs

            }
             return _ID; ;
        }
    }

}

