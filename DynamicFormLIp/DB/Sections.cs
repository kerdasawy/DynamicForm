using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DynamicFormLIp.Common;
using NLog;

namespace DB
{
    public class Sections
    {

        private SqlConnection con = null;
        private SqlTransaction transaction = null;
        private Logger Log = Genral_DB_Oprations.Log;
       
        public List<Elements>  Elements { get; set; }
        public const String _Name = "Sections";
        public Sections() { Elements = new List<Elements>(); }
        public Sections(SqlConnection con, SqlTransaction transaction) { this.con = con; this.transaction = transaction; Elements = new List<Elements>(); }
       public static string Name { get { return "Sections"; } }
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
        public static string Parnet_Form { get { return "Parnet_Form"; } }
        public Int32 _Parnet_Form
        {
            get;
            set;
        }
        public static string IsPublic { get { return "IsPublic"; } }
        public Boolean _IsPublic
        {
            get;
            set;
        }
        public static string User_created_ID_FK { get { return "User_created_ID_FK"; } }
        public Int32 _User_created_ID_FK
        {
            get;
            set;
        }
        public SqlConnection Con { get => con; set => con = value; }
        public SqlTransaction Transaction { get => transaction; set => transaction = value; }

        public Sections Load(int id)
        {

            var selectState = "select * from " + _Name + " where " + ID + " = " + id;
            var row = selectState.ExecuteToTable(con, transaction).Rows[0];
            this._ID = (int)row[ID];
            this._OBJ_Name = row[OBJ_Name].ToString();
            this._Parnet_Form = (int)row[Parnet_Form];
            this._User_created_ID_FK = (int)row[User_created_ID_FK];
            this._IsPublic = (bool)row[IsPublic];
            return this;
            // throw new NotImplementedException();
        }

        #region  Sql Statment
        public const string INSERT_STAT = @" 
 

INSERT INTO [dbo].[Sections]
           ([Name]
           ,[Parnet_Form]
           ,[IsPublic]
           ,[User_created_ID_FK])
 


     VALUES        ({0}  ,{1},{2},{3})
 

";


        public const string UPDATE_STAT = @"
 
UPDATE [dbo].[Sections]
   SET [Name] = {0}
      ,[Parnet_Form] = {1}
      ,[IsPublic] = {2}
      ,[User_created_ID_FK] = {3}
 WHERE ID = {4}
 

";
        public const string DELETE_STAT = @" 
DELETE FROM [dbo].[Sections]
    WHERE  ID ={0}


";
        #endregion
        public int Save()
        {

            if (con == null || transaction == null) throw new ArgumentNullException("Connection not intialized!");
            Log.Debug("check table   cuurent object id :" + this._ID);
            if (this._ID > 0)
            {
                Log.Debug("Starting update opertion");
                //update
                var state = string.Format(UPDATE_STAT, _OBJ_Name.SQLStr(), _Parnet_Form.SQLStr(), _IsPublic.SQLStr(), _User_created_ID_FK.SQLStr(), _ID.SQLStr());
                state.ExcuteUpdate(con, transaction);
                //update childs.

            }
            else
            {
                //insert
                Log.Debug("Starting insert opertion");
                var state = string.Format(INSERT_STAT, _OBJ_Name.SQLStr() ,_Parnet_Form.SQLStr() ,_IsPublic.SQLStr() ,_User_created_ID_FK.SQLStr() );
               
                var id = state.ExcuteInsert(con, transaction);
                this._ID = id;
                //insert childs

            }
            return -1;
        }
    }

}

