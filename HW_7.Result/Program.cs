using System.Net.Http.Json;
using HW_7.Result;
Console.WriteLine("Solution");

using var httpClient = new HttpClient();
var url = "https://dummyjson.com/products/";
var request = new HttpRequestMessage(HttpMethod.Get, url + "?limit=10");
var response = await httpClient.SendAsync(request);
var dummyProducts = await response.Content.ReadFromJsonAsync<DummyProductResponse>();
foreach (var item in dummyProducts.Products)
{
    Console.WriteLine($"Id: {item.Id}, title: {item.Title}");
}

bool continueUser = true;
string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Images");
Directory.CreateDirectory(directoryPath);

var pageUrl = "https://sithai.choiceqr.com";
var stringContent = await httpClient.GetStringAsync(pageUrl);
await File.WriteAllTextAsync(Path.Combine(directoryPath, "page.html"), stringContent);

var imagesLinks = new List<string>();
string start = "<script id=\"__NEXT_DATA__\"";
int startJsonIndex = stringContent.IndexOf(start, StringComparison.OrdinalIgnoreCase);

if (startJsonIndex != -1)
{
    int endJsonIndex = stringContent.IndexOf("</script>", startJsonIndex, StringComparison.OrdinalIgnoreCase);
    if (endJsonIndex != -1)
    {
        string jsonText = stringContent.Substring(startJsonIndex, endJsonIndex - startJsonIndex);
        int index = 0;
        while (true)
        {
            index = jsonText.IndexOf("https://cdn-media.choiceqr.com", index, StringComparison.OrdinalIgnoreCase);
            if (index == -1) break;
            int endIndex = jsonText.IndexOf('"', index);
            if (endIndex == -1) break;
            string imageUrl = jsonText.Substring(index, endIndex - index);
            if (!string.IsNullOrEmpty(imageUrl) && (imageUrl.EndsWith(".jpg") || imageUrl.EndsWith(".png") || imageUrl.EndsWith(".webp") || imageUrl.EndsWith(".jpeg")))
            {
                if (!imagesLinks.Contains(imageUrl))
                {
                    imagesLinks.Add(imageUrl);
                }
            }
            index = endIndex + 1;
        }
    }
}

Console.WriteLine($"Found Photos: {imagesLinks.Count}");
Console.WriteLine($"Downloading... please wait some minutes");
int imgIndex = 0;
foreach (var item in imagesLinks)
{
    try
    {
        var uri = new Uri(item);
        string fileName = Path.GetFileName(uri.LocalPath);
        if (string.IsNullOrEmpty(fileName) || !fileName.Contains('.'))
        {
            fileName = $"image_{imgIndex}.jpg";
        }
        string filePath = Path.Combine(directoryPath, fileName);
        var imageBytes = await httpClient.GetByteArrayAsync(item);
        await File.WriteAllBytesAsync(filePath, imageBytes);
        imgIndex++;
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}


do
{
    Console.Write("Enter id: ");
    string? id = Console.ReadLine();
    var requestId = new HttpRequestMessage(HttpMethod.Get, url + $"{id}");
    var responseId = await httpClient.SendAsync(requestId);
    var dummyProductId = await responseId.Content.ReadFromJsonAsync<DummyProduct>();
    Console.WriteLine($"Title: {dummyProductId.Title}");
    Console.WriteLine($"Price: {dummyProductId.Price}");

    Console.WriteLine("Do you want to download photos of this product?(yes/no)");
    string optionPhoto = Console.ReadLine();
    if (optionPhoto.ToLower() == "no") { }
    else if (optionPhoto.ToLower() == "yes")
    {
        string filePath = "";
        string imageUrl = "";
        for (int i = 0; i < dummyProductId.Images.Count; i++)
        {
            imageUrl = dummyProductId.Images[i];
            filePath = Path.Combine(directoryPath, $"{id}-{i}.webp");
            using var imageStream = await httpClient.GetStreamAsync(imageUrl);
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            await imageStream.CopyToAsync(fileStream);
        }
    }
    else
    {
        Console.WriteLine("Wrong option");

    }

    Console.WriteLine("Do you want to search more?(yes/no)");
    string optionContinue = Console.ReadLine();
    if (optionContinue.ToLower() == "no")
    {
        Console.WriteLine("Exiting...");
        continueUser = false;
    }
    else if (optionContinue.ToLower() == "yes")
    {
        continue;
    }
    else
    {
        Console.WriteLine("Wrong option");
        continue;
    }
} while (continueUser);


