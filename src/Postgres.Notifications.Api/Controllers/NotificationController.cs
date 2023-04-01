using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Postgres.Notifications.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController: ControllerBase
{
   [HttpPost("Send")]
   [ProducesResponseType(StatusCodes.Status200OK)]   
   public async Task<IActionResult> Send(string message)
   {
      await using var connection = new NpgsqlConnection("server=localhost;port=5432;database=postgres;user id=postgres;password=Password123");
      await connection.OpenAsync();
      var cmd = connection.CreateCommand();
      cmd.CommandText = $"select pg_notify('mychannel', '{message}')";
      await cmd.ExecuteScalarAsync();
      return Ok();
   }
}