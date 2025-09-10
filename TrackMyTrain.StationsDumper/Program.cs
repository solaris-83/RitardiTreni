// See https://aka.ms/new-console-template for more information

using System.Text;

StringBuilder str = new StringBuilder();
using HttpClient client = new HttpClient();

for (char letter = 'A'; letter <= 'Z'; letter++)
{
    string url = $"http://www.viaggiatreno.it/infomobilita/resteasy/viaggiatreno/autocompletaStazioneNTS/{letter}";
    try
    {
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync();
        str.Append(content);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error for {letter}: {ex.Message}");
    }
}

File.WriteAllText("stations.raw", str.ToString());
