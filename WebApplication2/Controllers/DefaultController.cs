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

                    // Insert some data
                    //using (var cmd = new NpgsqlCommand())
                    //{
                    //    cmd.Connection = conn;
                    //    cmd.CommandText = "INSERT INTO data (some_field) VALUES (@p)";
                    //    cmd.Parameters.AddWithValue("p", "Hello world");
                    //    cmd.ExecuteNonQuery();
                    //}

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
                result = string.Join(',',result.ToArray())
            });
        }

    }
}