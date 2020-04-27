using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace BDConnection
{
    public class BDConnection
    {
        private SqlConnection _connection;

        private string _query;


        private void connection()
        {
            string constring;
            try
            {
              
                JObject appSettingsJson = JObject.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + "\\appsettings.json"));
                var appSettings = appSettingsJson.ToObject<AppSettings.AppSettings>();

                // read JSON directly from a file
              
                 _connection = new SqlConnection(@"Data Source="+appSettings.SQLServer.ServerName+ ";Initial Catalog=DependencyChecker;User ID=" + appSettings.SQLServer.User + ";Password=" + appSettings.SQLServer.Password + "");

                var g = 0;
            }
            catch (Exception ex)
            {
                System.ArgumentException argEx = new System.ArgumentException("", "", ex);
                throw argEx;
            }


            _query = "";
        }



        public bool Add<T>(object modelo)
        {

            connection();


            var cmd = new SqlCommand("", _connection);


            _query = @"INSERT INTO [dbo].[" + modelo.GetType().Name.Replace("Model", "") + "] (";


            var columns = "";
            var data = "";

            foreach (var property in modelo.GetType().GetProperties())
            {
                var value = modelo.GetType().GetProperty(property.Name).GetValue(modelo, null);


                if (value != null)
                {
                    columns += "[" + property.Name + "],";

                    data += "@" + property.Name + ",";


                    cmd.Parameters.AddWithValue("@" + property.Name, value);
                }



            }

            _query += columns.Remove(columns.Length - 1) + ") VALUES (" + data.Remove(data.Length - 1) + ")";

            cmd.CommandText = _query;


            _connection.Open();

            int i;


            try
            {

                i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                System.ArgumentException argEx = new System.ArgumentException("", "", ex);
                throw argEx;
            }


            _connection.Close();



            return Convert.ToBoolean(i);

        }




        public bool Edit<T>(object modelo, string id)
        {
            int i;
            connection();
            try
            {
                var cmd = new SqlCommand("", _connection);

                _query = @"UPDATE [dbo].[" + modelo.GetType().Name.Replace("Model", "") + "] SET ";

                var where = "";

                var data = "";

                foreach (var property in modelo.GetType().GetProperties())
                {

                    if (property.Name == modelo.GetType().Name.Replace("Model", "").ToString() + "Id")
                    {

                        where = property.Name + " = '" + id + "' ";
                    }
                    else
                    {

                        var value = modelo.GetType().GetProperty(property.Name).GetValue(modelo, null);



                        if (value != null)
                        {
                            data += "[" + property.Name + "] = @" + property.Name + ",";


                            cmd.Parameters.AddWithValue("@" + property.Name, value);
                        }


                    }


                }


                _query += data.Remove(data.Length - 1) + " WHERE " + where + "";


                cmd.CommandText = _query;

                _connection.Open();





                i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                System.ArgumentException argEx = new System.ArgumentException("", "", ex);
                throw argEx;
            }

            _connection.Close();


            return Convert.ToBoolean(i);

        }



        public bool Delete<T>(object modelo, long? id)
        {

            connection();

            var cmd = new SqlCommand("", _connection);

            _query = @"UPDATE [dbo].[" + modelo.GetType().Name.Replace("Model", "") + "] SET ";

            var where = "";

            var data = "";

            foreach (var property in modelo.GetType().GetProperties())
            {

                if (property.Name == modelo.GetType().Name.Replace("Model", "").ToString() + "Id")
                {
                    where = property.Name + " = " + id;
                }

                if (property.Name == modelo.GetType().Name.Replace("Model", "").ToString() + "_estado")
                {
                    data += "[" + property.Name + "] = @" + property.Name + " ";

                    cmd.Parameters.AddWithValue("@" + property.Name, modelo.GetType().GetProperty(property.Name).GetValue(modelo, null));
                }

                if (property.Name == modelo.GetType().Name.Replace("Model", "").ToString() + "_activado")
                {
                    data += "[" + property.Name + "] = @" + property.Name + " ";

                    cmd.Parameters.AddWithValue("@" + property.Name, modelo.GetType().GetProperty(property.Name).GetValue(modelo, null));
                }




            }

            _query += data.Remove(data.Length - 1) + " WHERE " + where + "";

            cmd.CommandText = _query;

            _connection.Open();

            int i;

            try
            {
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                System.ArgumentException argEx = new System.ArgumentException("", "", ex);
                throw argEx;
            }

            _connection.Close();


            return Convert.ToBoolean(i);

        }
        public bool DeletePost<T>(object modelo, long? id)
        {

            connection();

            var cmd = new SqlCommand("", _connection);

            _query = @"DELETE FROM [" + modelo.GetType().Name.Replace("Model", "") + "]  ";

            var where = "";

            var data = "";

            foreach (var property in modelo.GetType().GetProperties())
            {
                var g = modelo.GetType().Name.Replace("Model", "").ToString() + "Id";

                if (property.Name == modelo.GetType().Name.Replace("Model", "").ToString() + "Id")
                {
                    where = property.Name + " = " + id;
                }



            }

            _query += " WHERE " + where + "";

            cmd.CommandText = _query;

            _connection.Open();

            int i;

            try
            {
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                System.ArgumentException argEx = new System.ArgumentException("", "", ex);
                throw argEx;
            }

            _connection.Close();


            return Convert.ToBoolean(i);

        }


        public bool Execute(string query)
        {

            connection();

            var cmd = new SqlCommand("", _connection);

            _query = query;

            var where = "";

            cmd.CommandText = _query;

            _connection.Open();

            int i;

            try
            {
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                System.ArgumentException argEx = new System.ArgumentException("", "", ex);
                throw argEx;
            }

            _connection.Close();


            return Convert.ToBoolean(i);

        }

        public object ExecuteQuery(string query)
        {

            connection();

            var cmd = new SqlCommand("", _connection);

            _query = query;

            var where = "";
            cmd.CommandTimeout = 100000;
            cmd.CommandText = _query;

            _connection.Open();

            int i;

            try
            {
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {

                System.ArgumentException argEx = new System.ArgumentException("", "", ex);

            }
            finally
            {
                _connection.Close();
            }




            return null;

        }
        public string ExecuteQueryAnswer(string query)
        {

            connection();

            var cmd = new SqlCommand("", _connection);

            _query = query;

            var where = "";

            cmd.CommandText = _query;

            _connection.Open();

            int i;
            var res = "";
            try
            {

                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            res = dr["max"].ToString();
                        }


                    }

                }
            }
            catch (Exception ex)
            {

                System.ArgumentException argEx = new System.ArgumentException("", "", ex);
                throw argEx;
            }

            _connection.Close();


            return res;

        }

        public List<object> Read<T>(object modelo) where T : new()
        {

            connection();

            var List = new List<object>();

            try
            {
                _connection.Open();
            }
            catch (Exception ex )
            {

                throw;
            }
         


            var cmd = new SqlCommand("select * from [" + modelo.GetType().Name.Replace("Model", "") + "]   ", _connection);
            cmd.CommandTimeout = 1500;


            try
            {
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                        while (dr.Read())
                            List.Add(ListGenericObject(dr, Activator.CreateInstance(modelo.GetType())));
                }

                _connection.Close();

            }
            catch (Exception ex)
            {
                _connection.Close();
                return null;
            }

            return List;
        }


        public object ReadWhere<T>(object modelo) where T : new()
        {
            connection();

            var where = " where 1=1 and ";

            var cmd = new SqlCommand("", _connection);
            cmd.CommandTimeout = 1500;
            foreach (var property in modelo.GetType().GetProperties())
            {
                if ((property.GetValue(modelo, null) != null))
                {

                    where += property.Name + " = @" + property.Name + " and ";

                    cmd.Parameters.AddWithValue("@" + property.Name, modelo.GetType().GetProperty(property.Name).GetValue(modelo, null));



                }
            }


            _query = "select * from [" + modelo.GetType().Name.Replace("Model", "") + "]   " + where.Remove(where.Length - 4);

            cmd.CommandText = _query;

            _connection.Open();

            var objeto = new object();
            using (var dr = cmd.ExecuteReader())
            {

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        objeto = ListGenericObject(dr, Activator.CreateInstance(modelo.GetType()));
                    }
                }
                else
                {
                    objeto = null;
                }

            }

            _connection.Close();
            return objeto;


        }





        public object Read<T>(object modelo, string id) where T : new()
        {
            connection();

            var where = " where ";

            foreach (var property in modelo.GetType().GetProperties())
            {
                var h = modelo.GetType().Name.Replace("Model", "").ToString() + "Id";
                if (property.Name == h)
                {
                    where += property.Name + " = '" + id + "'";
                }
            }

            var cmd = new SqlCommand("select * from [" + modelo.GetType().Name.Replace("Model", "") + "]   " + where, _connection);
            cmd.CommandTimeout = 1500;
            _connection.Open();

            var objeto = new object();
            using (var dr = cmd.ExecuteReader())
            {

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {


                        objeto = ListGenericObject(dr, Activator.CreateInstance(modelo.GetType()));

                    }
                }
                else
                {
                    objeto = null;
                }


            }
            _connection.Close();


            return objeto;
        }

        public object ExecuteStoreProcedure(string query)
        {
            connection();



            var cmd = new SqlCommand(query, _connection);
            cmd.CommandTimeout = 1500;
            _connection.Open();

            var objeto = new object();
            try
            {
                using (var dr = cmd.ExecuteReader())
                {

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {


                            objeto = dr[0];

                        }
                    }
                    else
                    {
                        objeto = null;
                    }


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }

            _connection.Close();


            return objeto;
        }


        public List<string> ReadTables()
        {
            connection();

            var List = new List<string>();
            var cmd = new SqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' and TABLE_NAME!='sysdiagrams'", _connection);
            cmd.CommandTimeout = 1500;
            _connection.Open();

            var objeto = new object();
            using (var dr = cmd.ExecuteReader())
            {

                if (dr.HasRows)
                    while (dr.Read())
                        List.Add(dr["TABLE_NAME"].ToString());
            }

            _connection.Close();
            return List;
        }


        public List<object> ReadWhereList<T>(object modelo) where T : new()
        {
            connection();
            var List = new List<object>();

            var where = " where 1=1 and ";

            var cmd = new SqlCommand("", _connection);
            cmd.CommandTimeout = 1500;
            foreach (var property in modelo.GetType().GetProperties())
            {
                if ((property.GetValue(modelo, null) != null))
                {

                    where += property.Name + " = @" + property.Name + " and ";

                    cmd.Parameters.AddWithValue("@" + property.Name, modelo.GetType().GetProperty(property.Name).GetValue(modelo, null));



                }
            }


            _query = "select * from [" + modelo.GetType().Name.Replace("Model", "") + "]   " + where.Remove(where.Length - 4);

            cmd.CommandText = _query;

            _connection.Open();

            var objeto = new object();
            using (var dr = cmd.ExecuteReader())
            {

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        List.Add(ListGenericObject(dr, Activator.CreateInstance(modelo.GetType())));
                    }
                }
                else
                {
                    List = null;
                }

            }

            _connection.Close();
            return List;


        }

        public List<object> ReadLike<T>(object modelo) where T : new()
        {
            connection();
            var List = new List<object>();

            var where = " where 1=1 and ";

            var cmd = new SqlCommand("", _connection);
            cmd.CommandTimeout = 1500;
            foreach (var property in modelo.GetType().GetProperties())
            {
                if ((property.GetValue(modelo, null) != null) && !property.Name.Contains("State") && !property.Name.Contains("UserReadOnly"))
                {

                    where += property.Name + " like '%" + modelo.GetType().GetProperty(property.Name).GetValue(modelo, null) + "%' and ";




                }
            }


            _query = "select * from [" + modelo.GetType().Name.Replace("Model", "") + "]   " + where.Remove(where.Length - 4);

            cmd.CommandText = _query;

            _connection.Open();

            var objeto = new object();
            using (var dr = cmd.ExecuteReader())
            {

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        List.Add(ListGenericObject(dr, Activator.CreateInstance(modelo.GetType())));
                    }
                }
                else
                {
                    List = null;
                }

            }

            _connection.Close();
            return List;


        }

        public object ListGenericObject(SqlDataReader dr, object objeto)
        {

            foreach (var property in objeto.GetType().GetProperties())
            {
                try
                {

                    if (property.Name != "mensaje")
                    {
                        objeto.GetType().GetProperty(property.Name).SetValue(objeto,
                         (dr[property.Name] == DBNull.Value) ? null : dr[property.Name]);
                    }





                }
                catch (Exception ex)
                {

                    System.ArgumentException argEx = new System.ArgumentException("Problema de Conexión", "Error", ex);
                    throw argEx;
                }
            }




            return objeto;
        }







    }
}