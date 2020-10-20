using System;
using System.Data.SqlClient;
using DynamicFormLIp.Common;
using NLog;

namespace DB
{
    public class Elements
    {
        private SqlConnection con = null;
        private SqlTransaction transaction = null;
        public const String _Name = "Elements";
        private Logger Log = Genral_DB_Oprations.Log;
        public Elements(SqlConnection con, SqlTransaction transaction) { this.con = con; this.transaction = transaction; }
        public Elements()
        {

        }
        public static string Name { get { return "Elements"; } }
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
        public static string Section_ID_FK { get { return "Section_ID_FK"; } }
        public Int32 _Section_ID_FK
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
        public static string Elements_Type_ID_FK { get { return "Elements_Type_ID_FK"; } }
        public Int32 _Elements_Type_ID_FK
        {
            get;
            set;
        }
        public static string isPublic { get { return "isPublic"; } }
        public Boolean _isPublic
        {
            get;
            set;
        }
        public static string IsRequired { get { return "isRequired"; } }
        public bool _isRequired { get; set; }

        public Elements Load(int id)
        {

            var selectState = "select * from " + _Name + " where " + ID + " = " + id;
            var row = selectState.ExecuteToTable(con, transaction).Rows[0];
            this._ID = (int)row[ID];
            this._OBJ_Name = row[OBJ_Name].ToString();
            this._Section_ID_FK = (int)row[Section_ID_FK];
            this._isPublic = (bool)row[isPublic];
            this._Elements_Type_ID_FK = (int)row[Elements_Type_ID_FK];
            this._isRequired = (bool)row[IsRequired];
            return this;
            // throw new NotImplementedException();
        }
        #region  Sql Statment
        public const string INSERT_STAT = @" 
  
INSERT INTO [dbo].[Elements]
           ([Name]
           ,[Section_ID_FK]
           ,[User_created_ID_FK]
           ,[Elements_Type_ID_FK]
           ,[isPublic],[isRequired])
     

     VALUES
           ({0},{1},{2},{3},{4},{5}
          )
 

";


        public const string UPDATE_STAT = @"
 
 
UPDATE [dbo].[Elements]
   SET [Name] ={0}
      ,[Section_ID_FK] ={1}
      ,[User_created_ID_FK] ={2}
      ,[Elements_Type_ID_FK] = {3}

      ,[isPublic] = {4}
 ,[isRequired]={5}


 WHERE ID = {6}
 

";
        public const string DELETE_STAT = @" 
DELETE FROM [dbo].[Elements]
    WHERE  ID ={0}


";
        #endregion
        public int Save()
        {
            if (con==null || transaction==null)  throw new ArgumentNullException("Connection not intialized!");
             
            Log.Debug("check table   cuurent object id :" + this._ID);
            if (this._ID > 0)
            {
                Log.Debug("Starting update opertion");
                //update
                var state = string.Format(UPDATE_STAT, _OBJ_Name.SQLStr(), _Section_ID_FK.SQLStr(), _User_created_ID_FK.SQLStr()
                    ,_Elements_Type_ID_FK.SQLStr(), _isPublic.SQLStr(),_isRequired.SQLStr(),
                    _ID.SQLStr());
                 state.ExcuteUpdate(con, transaction);
               
                //update childs.

            }
            else
            {
                //insert
                Log.Debug("Starting insert opertion");
                var state = string.Format(INSERT_STAT, _OBJ_Name.SQLStr(), _Section_ID_FK.SQLStr(), _User_created_ID_FK.SQLStr()
                    , _Elements_Type_ID_FK.SQLStr(), _isPublic.SQLStr(),_isRequired.SQLStr());
                var id =state.ExcuteInsert(con, transaction);
                this._ID = id;
                //insert childs

            }
            return -1;
        }
        public Elemnts_Types Type { get { 
            return (new Elemnts_Types( con , transaction)).Load(_Elements_Type_ID_FK);
            } }

        public SqlConnection Con { get => con; set => con = value; }
        public SqlTransaction Transaction { get => transaction; set => transaction = value; }
    }

}

