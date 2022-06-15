using System.Text;

internal class Program
{
    private static async Task Main(string[] args)
    {
        using (var httpClient = new HttpClient())
        {
            var host = "https://example.com";
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"{host}/alfresco/api/-default-/public/alfresco/versions/1/nodes/-root-/children"))
            {
                var user = "admin";
                var pass = "admin";
                var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{user}:{pass}"));
                request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                var filePath = "FileTest/TestDocument.pdf";
                var targetPath = "/Api/Test";
                var targetName = "Upload from api.pdf";
                var multipartContent = new MultipartFormDataContent();
                multipartContent.Add(new ByteArrayContent(File.ReadAllBytes(filePath)), "filedata");
                multipartContent.Add(new StringContent(targetPath), "relativePath");
                multipartContent.Add(new StringContent(targetName), "name");
                // Add properties (ชื่อ Parameter ให้ดูจากไฟล์ Spec API)
                multipartContent.Add(new StringContent("Application Form"), "TNI:DocuemntType");
                request.Content = multipartContent;

                var response = await httpClient.SendAsync(request);
                var status = response.StatusCode;
                if (status == System.Net.HttpStatusCode.Created){
                    Console.WriteLine("Upload Success -- {0}", await response.Content.ReadAsStringAsync());
                } else {
                    Console.WriteLine("Upload Failed -- {0} -- {1}", (int)status , await response.Content.ReadAsStringAsync());
                }                
            }
        }
    }
}