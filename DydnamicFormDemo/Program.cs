using DB;
using DynamicFormLIp.Common;
using System;
using System.Collections.Generic;

namespace DydnamicFormDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Genral_DB_Oprations.Log.Debug("Starting The app");
            Forms test = new Forms() {  _Desc="Test" , _Form_Type_ID_FK=2, _IsPublic =true , _Moudle="test" , _OBJ_Name="TestForm" , _Parent_From_ID_FK =null , _User_created_ID_FK=1
            ,Sections = new List<Sections>() { new Sections() {  _IsPublic=true , _User_created_ID_FK = 1 , _OBJ_Name="S1" 
            ,Elements = new List<Elements>(){ new Elements() {  _OBJ_Name="SSN" , _Elements_Type_ID_FK = 3 ,_User_created_ID_FK = 1 , _isRequired =true }
            ,new Elements() {  _OBJ_Name="SSN2" , _Elements_Type_ID_FK = 3 ,_User_created_ID_FK = 1 , _isRequired =false }
             ,new Elements() {  _OBJ_Name="ID" , _Elements_Type_ID_FK = 1 ,_User_created_ID_FK = 1 , _isRequired =true }
            }
            }

            , new Sections() { _IsPublic = true, _User_created_ID_FK = 1, _OBJ_Name = "S2", Elements = new List<Elements>(){ new Elements() {  _OBJ_Name="SSN" , _Elements_Type_ID_FK = 3 ,_User_created_ID_FK = 1 , _isRequired =true }
            ,new Elements() {  _OBJ_Name="SSN2" , _Elements_Type_ID_FK = 3 ,_User_created_ID_FK = 1 , _isRequired =false }
            } } }
            
            };
            test.Save();
        }
    }
}
