using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Postgres.Notifications.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController
{
   [HttpPost("Send")]
   public async Task Send()
   {
      await using var connection = new NpgsqlConnection("server=localhost;port=5432;database=postgres;user id=postgres;password=Password123");
      await connection.OpenAsync();
      var cmd = connection.CreateCommand();
      cmd.CommandText = "select pg_notify('hi there')";
      var result = await cmd.ExecuteScalarAsync();
      Debug.WriteLine(result);
   }
}