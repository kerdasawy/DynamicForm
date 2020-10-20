using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DynamicFormLIp.Common;
using NLog;

namespace DB
{
    public class Tables
    {
        private SqlConnection con = null;
        private SqlTransaction transaction = null;
        private Logger Log = Genral_DB_Oprations.Log;
        public const String _Name = "Tables";
        public static string prederedTablePrefex = "FormTable_";
        private Tables() { Columns = new List<Columns>(); }
       
        public Tables(SqlConnection con , SqlTransaction transaction) { this.con = con; this.transaction = transaction;
            Columns = new List<Columns>();
        }
       
        public static string Name { get { return "Tables"; } }
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
       public List<Columns> Columns { get; set; }
        public SqlConnection Con { get => con; set => con = value; }
        public SqlTransaction Transaction { get => transaction; set => transaction = value; }

        public Tables Load(int id)
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
 
INSERT INTO [dbo].[Tables]
           ([Name])
     VALUES
 


           ({0}
          )
 

";


        public const string UPDATE_STAT = @"
UPDATE [dbo].[Tables]
   SET [Name] = {0}
       
 WHERE  ID ={1}

";
        public const string DELETE_STAT = @" 
DELETE FROM [dbo].[Tables]
    WHERE  ID ={0}


";
        #endregion
        public int Save() {

            if (con == null || transaction == null) throw new ArgumentNullException("Connection not intialized!");
            Log.Debug("check table   cuurent object id :" + this._ID);
            if (this._ID > 0)
            {
                Log.Debug("Starting update opertion");
                //update
                var state = string.Format(UPDATE_STAT, _OBJ_Name.SQLStr(),  _ID.SQLStr());
                state.ExcuteUpdate(con, transaction);
                //update childs.
             
            }
            else
            {
                //insert
                Log.Debug("Starting insert opertion");
                var state = string.Format(INSERT_STAT, _OBJ_Name.SQLStr() );
                var id = state.ExcuteInsert(con, transaction);

                this._ID = id;
                //insert childs
             
            }
            return _ID;
        }

        public void CreateDBTable()
        {
            string createStatTemplete = @"CREATE TABLE [dbo].[{0}](
	                {1}
                 CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED 
                (
	                [ID] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

                select 0;";

            string colStates = "";
            foreach (var col in this.Columns)
            {
                colStates += col.ToString();
            }
            var createState = string.Format(createStatTemplete, _OBJ_Name, colStates);
            var res = createState.ExecuteScalar(Con, transaction);
        }
    }

}

