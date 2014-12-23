using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Dapper;
using Npgsql;

namespace NancyOwinHeroku_sample.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => "Hello World";

            Get["/con"] =_ => {
                return GetConnectionString();
            };

            Get["/json"] = _ =>
            {
                var ringo = new[]
                {
                    new { Id = 1, ItemName = "トキ"},
                    new { Id = 2, ItemName = "秋映"}
                };

                return Response.AsJson(ringo);
            };

            Get["/ringo/{id}"] = _ =>
            {
                IEnumerable<Ringo> result = SelectByID(_.id);
                var response = (Response)(result.FirstOrDefault().Name);

                // DBから持ってきた日本語を表示する場合、charsetを指定しないと文字化けする
                response.ContentType = "text/html; charset=utf8";
                return response;
            };

            Get["/json/{id}"] = _ =>
            {
                IEnumerable<Ringo> ringo = SelectByID(_.id);
                // AsJsonの場合は、特にcharsetを指定しなくても日本語は文字化けしない
                return Response.AsJson(ringo);
            };

        }

        private IEnumerable<Ringo> SelectByID(string id)
        {
            var parsedID = int.Parse(id);

            using (var cn = new NpgsqlConnection(GetConnectionString()))
            {
                cn.Open();
                var sql = "SELECT * FROM \"Ringo\" WHERE \"ID\" = @ID";
                var result = cn.Query<Ringo>(sql, new { ID = parsedID });
                cn.Close();
                return result;
            }
        }


        private bool IsProduction()
        {
            return Environment.GetEnvironmentVariable("DATABASE_URL") != null;
        }

        private string GetConnectionString()
        {
            if (IsProduction())
            {
                var uri = new Uri(Environment.GetEnvironmentVariable("DATABASE_URL").ToString());
                var userInfo = uri.UserInfo.Split(':');
                var herokuConnection = 
                    string.Format("Server={0}; Port={1}; Database={2}; User Id={3}; Password={4}; SSL=true;SslMode=Require;",
                    uri.Host, uri.Port, uri.Segments[1], userInfo[0], userInfo[1]);
                return herokuConnection;
            }
            else
            {
                return @"Server=127.0.0.1; Port=5432; Database=fluentmigrator; User Id=postgres; Password=postgres;";
            }
        }


        private class Ringo
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
    }
}
