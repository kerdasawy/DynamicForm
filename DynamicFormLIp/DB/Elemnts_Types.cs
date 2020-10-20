using System;
using System.Data.SqlClient;
using DynamicFormLIp.Common;
using NLog;

namespace DB
{
    public class Elemnts_Types
    {
        private SqlConnection con = null;
        private SqlTransaction transaction = null;
        public const String _Name = "Elemnts_Types";
        private Logger Log = Genral_DB_Oprations.Log;

        public Elemnts_Types(SqlConnection con, SqlTransaction transaction) { this.con = con; this.transaction = transaction; }
       
        public static string Name { get { return "Elemnts_Types"; } }
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
        public static string Is_Parent { get { return "Is_Parent"; } }
        public bool _Is_Parent
        {
            get;
            set;
        }

        public Column_Type ColumnsType { get {

                //Element Simple DataType Mapped to simple Columns type
                #region Database types info
                /*
                 * Elements types
                       int
                       float
                        string
                       */

                /*
                 * ColumnType
                 int
                 nvarchar(max)
                float
                 */ 
                #endregion

                if (_OBJ_Name == "int")
                {
                    return (new Column_Type(con, transaction)).Load("int");
                }
                else if (_OBJ_Name == "string"){
                    return (new Column_Type(con, transaction)).Load("nvarchar(max)");
                }
                else if (_OBJ_Name == "float")
                {
                    return (new Column_Type(con, transaction)).Load("float");
                }
                else
                {
                    //parent type
                    return null;
                }
                
            } }

        

        public Elemnts_Types Load(int elements_Type_ID_FK)
        {

            var selectState = "select * from " + _Name + " where " + ID + " = " + elements_Type_ID_FK;
            var row = selectState.ExecuteToTable(con, transaction).Rows[0];
            this._ID = (int)row[ID];
            this._OBJ_Name = row[OBJ_Name].ToString();
            this._Is_Parent =(bool) row[Is_Parent];
            
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

