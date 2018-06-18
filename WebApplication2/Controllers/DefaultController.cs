using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace WebApplication2.Controllers
{
    [Produces("application/json")]
    [Route("api/Default")]
    public class DefaultController : Controller
    {
        [HttpGet("Index")]
        public ActionResult Index()
        {
            var connString = "Host=localhost;Port=5433;Username=postgres;Password=guest;Database=postgres";
            var result = new List<string>();
            using (var conn = new NpgsqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    result = new List<string>();
                    // Insert some data
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        //default only use by not include the column in the insert statement. different from specify that column and insert NULL value into it
                        cmd.CommandText = @"INSERT INTO dbo.eventstore (id, aggid, isdeleted, jsondata, objtype, type) VALUES (@p1 , @p2 , @p3 , @p4 , @p5,@p6)";
                        cmd.Parameters.AddWithValue("p1",Guid.NewGuid());
                        cmd.Parameters.AddWithValue("p2", Guid.NewGuid());
                        cmd.Parameters.AddWithValue("p3", true);//bit not boolean
                        cmd.Parameters.AddWithValue("p4", "");
                        cmd.Parameters.AddWithValue("p5", "objtype");
                        cmd.Parameters.AddWithValue("p6", 0);
                        cmd.ExecuteNonQuery();
                    }

                    // Retrieve all rows
                    using (var cmd = new NpgsqlCommand("select * from dbo.eventstore", conn))
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                            result.Add(reader.GetString(0));
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
                
            }
            return Ok(new {
                isReceived = true,
                result = string.Join(',',result?.ToArray())
            });
        }

    }
}