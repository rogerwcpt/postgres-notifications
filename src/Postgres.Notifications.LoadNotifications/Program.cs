// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

var items = Enumerable.Range(1, 1000);

foreach (var i in items)
{
    var httpClient = new HttpClient();
    Debug.WriteLine($"Sending {i}");
    var result = await httpClient.PostAsync($"http://localhost:5028/Notification/Send?message={i}",
        new StringContent(string.Empty));
    Debug.WriteLine(result.StatusCode);
};
