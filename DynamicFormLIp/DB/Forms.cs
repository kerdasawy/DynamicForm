using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DynamicFormLIp.Common;
using NLog;

namespace DB
{
    public class Forms
    {

        private Logger Log = Genral_DB_Oprations.Log;
        public const String _Name = "Forms";
     
        public Forms() {
            Sections = new List<Sections>();
        }
   
        #region Field Name and access
        public static string Name { get { return "Forms"; } }
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
        public static string Moudle { get { return "Moudle"; } }
        public String _Moudle
        {
            get;
            set;
        }
        public static string Desc { get { return "Desc"; } }
        public String _Desc
        {
            get;
            set;
        }
        public static string Form_Type_ID_FK { get { return "Form_Type_ID_FK"; } }
        public Int32 _Form_Type_ID_FK
        {
            get;
            set;
        }
        public static string Parent_From_ID_FK { get { return "Parent_From_ID_FK"; } }
        public Int32? _Parent_From_ID_FK
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
        public static string IsPublic { get { return "IsPublic"; } }
        public Boolean _IsPublic
        {
            get;
            set;
        }
        public List<Sections> Sections { get; set; }
        #endregion

        #region  Sql Statment
        public const string INSERT_STAT = @" 
INSERT INTO [dbo].[Forms]
           ([Name]
           ,[Moudle]
           ,[Desc]
           ,[Form_Type_ID_FK]
           ,[Parent_From_ID_FK]
           ,[User_created_ID_FK]
           ,[IsPublic])
     VALUES
           ({0}
           ,{1}
           ,{2}
           ,{3}
           ,{4}
           ,{5}
           ,{6})
 

";


        public const string UPDATE_STAT = @"
UPDATE [dbo].[Forms]
   SET [Name] = {0}
      ,[Moudle] = {1}
      ,[Desc] = {2}
      ,[Form_Type_ID_FK] = {3}
      ,[Parent_From_ID_FK] = {4}
      ,[User_created_ID_FK] = {5}
      ,[IsPublic] = {6}
 WHERE  ID ={7}

";
        public const string DELETE_STAT = @" 
DELETE FROM [dbo].[Forms]
    WHERE  ID ={0}


";
        #endregion
       
        

        public int Save()
        {
             Log.Debug("Starting Save");
            using (SqlConnection con = new SqlConnection(DynamicFormLIp.Properties.Resources.ConStr))
            {
                Log.Debug("Starting open the connection database..");
                con.Open();
                Log.Debug("Starting create new Tranaction");
                var transaction = con.BeginTransaction();
                Log.Debug("Tranaction created.["+transaction.ToString()+"]");
                try
                {
                    Log.Debug("check the cuurent object id :"+ this._ID);
                    if (this._ID > 0)
                    {
                        Log.Debug("Starting update opertion");
                        //update
                        var state = string.Format(UPDATE_STAT, _OBJ_Name.SQLStr(), _Moudle.SQLStr(), _Desc.SQLStr(), _Form_Type_ID_FK.SQLStr(),
                            _Parent_From_ID_FK.SQLStr(), _User_created_ID_FK.SQLStr(),_IsPublic.SQLStr(),_ID.SQLStr());
                        state.ExcuteUpdate(con,transaction);
                        //update childs.
                        Log.Debug("Starting update child opertion");
                    }
                    else
                    {
                        //insert
                        Log.Debug("Starting insert opertion");
                        var state = string.Format(INSERT_STAT, _OBJ_Name.SQLStr(), _Moudle.SQLStr(), _Desc.SQLStr(), _Form_Type_ID_FK.SQLStr(),
                            _Parent_From_ID_FK.SQLStr(), _User_created_ID_FK.SQLStr(), _IsPublic.SQLStr() );
                        var id  = state.ExcuteInsert(con, transaction);
                        this._ID = id;
                        //insert childs
                        Log.Debug("Starting insert  child opertion");
                        //insert Table name
                        Tables formTable = new Tables(con, transaction);
                        formTable._OBJ_Name = Tables.prederedTablePrefex+"_" + this._ID +"_"+this._OBJ_Name;

                        formTable.Save();
                        //insert sections
                        foreach (var sec in this.Sections)
                        {
                            sec.Con = con;
                            sec.Transaction = transaction;
                            sec._Parnet_Form = this._ID;
                            sec.Save();
                        
                            foreach (var ele in sec.Elements)
                            {
                                ele.Con = con;
                                ele.Transaction = transaction;
                                ele._Section_ID_FK = sec._ID;
                                ele.Save();
                                Columns col = new Columns(con, transaction);
                                formTable.Columns.Add(col);
                                col._OBJ_Name = ele._OBJ_Name;
                                col._Table_ID_FK = formTable._ID;
                                var elemnetType = (new Elemnts_Types(con, transaction)).Load(ele._Elements_Type_ID_FK);
                                if (!elemnetType._Is_Parent)
                                {
                                    col._Column_Type_ID_FK = elemnetType.ColumnsType._ID;
                                }
                                else
                                {
                                    //Grid and drop down go here..
                                }
                                col.Save();
                                Elemnets_Columns ele_col = new Elemnets_Columns(con, transaction);
                                ele_col._Column_ID_FK = col._ID;
                                ele_col._Element_ID_FK = ele._ID;
                                ele_col.Save();
                                //create table for data
                                
                                
                            }
                      
                    }
                        //insert element
                        //insert columns 
                        //insert element columns

                        formTable.CreateDBTable();
                        transaction.Commit();
                    }
            }
                catch (Exception ex)
            {
                Log.Error("Exception happend:[" + ex.Message + "] \r\n exception stack :" + ex.StackTrace);
                transaction.Rollback();
            }

              
            //Get elements 

                //Save Tables and columns
                //Save From
                //
            }
            return -1;
        }
    }

}

