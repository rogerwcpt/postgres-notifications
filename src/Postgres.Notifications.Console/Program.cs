// See https://aka.ms/new-console-template for more information

using Npgsql;

Console.WriteLine("Connecting...");
var connection = new NpgsqlConnection("server=localhost;port=5432;database=postgres;user id=postgres;password=Password123");
connection.Open();
Console.WriteLine("Connected");
connection.Notification += (sender, eventArgs) =>
{
    Console.WriteLine("Notification received " + eventArgs.Payload);
};

var cmd = new NpgsqlCommand("LISTEN mychannel", connection);
cmd.ExecuteNonQuery();
Console.WriteLine("Listening");
while (true)
{
    connection.Wait();
}
Console.WriteLine("Done");