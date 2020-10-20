using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DynamicFormLIp.Common
{
    public static class Genral_DB_Oprations
    {
         
        public static Logger Log = LogManager.GetCurrentClassLogger();
        public static DataTable ExecuteToTable(this string Stat, SqlConnection con, SqlTransaction transaction)
        {
            if (con == null || con.State != ConnectionState.Open)
            {
                throw new ArgumentException("Invalid connection be sure the connection is opened and intialized");
            }
            else
            {
                var command = con.CreateCommand();
                command.CommandText = Stat;
                command.Transaction = transaction;
                var reader = command.ExecuteReader();
                DataTable data = new DataTable();
                data.Load(reader);
                reader.Dispose();
                return data;
            }
        }


       
        public static SqlDataReader ExecuteToReader(this string Stat, SqlConnection con)
        {
            if (con == null || con.State != ConnectionState.Open)
            {
                throw new ArgumentException("Invalid connection be sure the connection is opened and intialized");
            }
            else
            {
                var command = con.CreateCommand();
                command.CommandText = Stat;
                var reader = command.ExecuteReader();
                return reader;
            }
        }
        
        public static int ExcuteUpdate(this string insertStat, SqlConnection con)
        {
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            var command = con.CreateCommand();
            command.CommandText = insertStat;
            return int.Parse(command.ExecuteNonQuery().ToString());

        }
        public static int ExcuteUpdate(this string insertStat, SqlConnection con ,SqlTransaction transaction)
        {
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            var command = con.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = insertStat;
            return int.Parse(command.ExecuteNonQuery().ToString());

        }
        public static object ExecuteScalar(this string Stat, SqlConnection con, SqlTransaction transaction)
        {
            if (con == null || con.State != ConnectionState.Open)
            {
                throw new ArgumentException("Invalid connection be sure the connection is opened and intialized");
            }
            else
            {
                var command = con.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = Stat;
                return command.ExecuteScalar();
            }
        }
        public static object ExecuteScalar(this string Stat, SqlConnection con )
        {
            if (con == null || con.State != ConnectionState.Open)
            {
                throw new ArgumentException("Invalid connection be sure the connection is opened and intialized");
            }
            else
            {
                var command = con.CreateCommand();
                
                command.CommandText = Stat;
                return command.ExecuteScalar();
            }
        }
        /// <summary>
        /// excute sql statment and add select identity to insert 
        /// and return the id integar of inserted row.
        /// </summary>
        /// <param name="insertStat"> the sql statment to excute </param>
        /// <param name="con"> open connection to DB </param>
        /// <returns> the id integar of inserted row</returns>
        public static int ExcuteInsert(this string insertStat, SqlConnection con)
        {
            if (con == null || con.State != ConnectionState.Open)
            {
                throw new ArgumentException("Invalid connection be sure the connection is opened and intialized");
            }
            else
            {
                var command = con.CreateCommand();
                command.CommandText = insertStat + " SELECT SCOPE_IDENTITY();";
                return int.Parse(command.ExecuteScalar().ToString());
            }
        }
        public static int ExcuteInsert(this string insertStat, SqlConnection con, SqlTransaction transaction)
        {
            if (con == null || con.State != ConnectionState.Open)
            {
                throw new ArgumentException("Invalid connection be sure the connection is opened and intialized");
            }
            else
            {
                var command = con.CreateCommand();
                command.CommandText = insertStat + "; SELECT SCOPE_IDENTITY();";
                command.Transaction = transaction;
                return int.Parse(command.ExecuteScalar().ToString());
            }
        }

        public static string SQLStr(this object o, bool addCots = true)
        {
            if (o == null || Convert.IsDBNull(o))
            {
                return "NULL";
            }
            else
            {
                if (o is string)
                {
                    if (addCots)
                        return "'" + o.ToString().Replace("'", "''") + "'";
                    else
                    {
                        return o.ToString();
                    }
                }
                else if (o is bool)
                {
                    return (((bool)o) ? "1" : "0");
                }
                else if (o is DateTime)
                {
                    if (addCots)
                        return GetDateTime((DateTime)o);
                    else
                    {
                        DateTime dateTime = (DateTime)o;
                        return dateTime.Year + "-" + String.Format("{0:D2}", dateTime.Month)
                + "-" + String.Format("{0:D2}", dateTime.Day) + "  " +
                String.Format("{0:D2}", dateTime.Hour) + ":" +
                String.Format("{0:D2}", dateTime.Minute + ":" +
                String.Format("{0:D2}", dateTime.Second) + "." +
                String.Format("{0:D3}", dateTime.Millisecond));
                    }

                }
                else if (o is DateTime?)
                {
                    if (((DateTime?)o).HasValue)
                    {


                        if (addCots)
                            return GetDateTime(((DateTime?)o).Value);
                        else
                        {
                            DateTime dateTime = ((DateTime?)o).Value;
                            return dateTime.Year + "-" + String.Format("{0:D2}", dateTime.Month)
                    + "-" + String.Format("{0:D2}", dateTime.Day) + "  " +
                    String.Format("{0:D2}", dateTime.Hour) + ":" +
                    String.Format("{0:D2}", dateTime.Minute + ":" +
                    String.Format("{0:D2}", dateTime.Second) + "." +
                    String.Format("{0:D3}", dateTime.Millisecond));
                        }
                    }
                    else
                    {
                        return "Null";
                    }

                }
                else
                {
                    if (addCots)
                    {
                        return "'" + o.ToString().Replace("'", "''") + "'";
                    }
                    else
                    {
                        return o.ToString().Replace("'", "''");
                    }

                }
            }
        }
        public static string SQLStrNull(this object o, bool addCots = true)
        {
            if (o == null || Convert.IsDBNull(o))
            {
                return "NULL";
            }
            else
            {
                if (o is string)
                {
                    if (o.ToString().Trim() == "")
                    { return "NULL"; }

                    else
                    {
                        if (addCots)
                            return "'" + o.ToString().Replace("'", "''") + "'";
                        else
                        {
                            return o.ToString();
                        }
                    }
                }
                else if (o is bool)
                {
                    return (((bool)o) ? "1" : "0");
                }

                else if (o is DateTime)
                {
                    if (addCots)
                        return GetDateTime((DateTime)o);
                    else
                    {
                        DateTime dateTime = (DateTime)o;
                        return dateTime.Year + "-" + String.Format("{0:D2}", dateTime.Month)
                + "-" + String.Format("{0:D2}", dateTime.Day) + "  " +
                String.Format("{0:D2}", dateTime.Hour) + ":" +
                String.Format("{0:D2}", dateTime.Minute + ":" +
                String.Format("{0:D2}", dateTime.Second) + "." +
                String.Format("{0:D3}", dateTime.Millisecond));
                    }

                }
                else
                {
                    if (addCots)
                    {
                        return "'" + o.ToString().Replace("'", "''") + "'";
                    }
                    else
                    {
                        return o.ToString().Replace("'", "''");
                    }

                }
            }
        }

        public static string GetDateTime(DateTime dateTime)
        {
            return "'" + dateTime.Year + "-" + String.Format("{0:D2}", dateTime.Month)
                + "-" + String.Format("{0:D2}", dateTime.Day) + "  " +
                String.Format("{0:D2}", dateTime.Hour) + ":" +
                String.Format("{0:D2}", dateTime.Minute + ":" +
                String.Format("{0:D2}", dateTime.Second) + "." +
                String.Format("{0:D3}", dateTime.Millisecond)) + "'";
        }
    }
}
