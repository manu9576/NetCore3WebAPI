using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using NetCore3WebAPI.Models;
using System.Threading.Tasks;

namespace NetCore3WebAPI.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ConnectionStrings _connectionString;

        public UserController(ConnectionStrings connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Return the users list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] User vm)
        {
            return await Task.Run(() =>
            {
                using (var connection = new MySqlConnection(_connectionString.MySql))
                {
                    var sql = @"SELECT * FROM user 
                                WHERE (@id = 0 or id = @id)
                                AND (@name IS NULL OR UPPER(name) = UPPER(@name))";

                    var query = connection.Query<User>(sql, vm, commandTimeout: 30);

                return Ok(query);
                }
                
            });
        }

        /// <summary>
        /// Return the users list
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] User vm)
        {
            return await Task.Run(() =>
            {
                using (var connection = new MySqlConnection(_connectionString.MySql))
                {
                    var sql = @"INSERT INTO user (name) VALUES (@name)";
                    connection.Execute(sql, vm, commandTimeout: 30);
                    return Ok();
                }
            });
        }
    }
}
