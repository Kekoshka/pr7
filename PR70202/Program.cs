using HtmlAgilityPack;
using System.Diagnostics;
using System.Net;
using System.Text;

Cookie token = SingIn("user", "user");
string content = GetContent(token);
ParsingHtml(content);

Console.Read();

void ParsingHtml(string htmlCode)
{
    var html = new HtmlDocument();
    html.LoadHtml(htmlCode);
    var document = html.DocumentNode;
    var divsNews = document.Descendants(0).Where(n => n.HasClass("news"));
    foreach(HtmlNode divNews in divsNews)
    {
        var src = divNews.ChildNodes[1].GetAttributeValue("src", "none");
        var name = divNews.ChildNodes[3].InnerText;
        var description = divNews.ChildNodes[5].InnerText;
        Console.WriteLine($"{name}\nИзображение: {src}\nОписание: {description}\n");
    }
}
string GetContent(Cookie token)
{
    string url = "";
    Debug.WriteLine($"Выполняем запрос {url}");
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
    request.CookieContainer = new CookieContainer();
    request.CookieContainer.Add(token);

    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    Debug.WriteLine($"Статус выполнения: {response.StatusCode}");
    string content = new StreamReader(response.GetResponseStream()).ReadToEnd();
    return content;
}
Cookie SingIn(string login, string password)
{
    Cookie token = null;
    string url = "";

    Debug.WriteLine($"Выполняем запрос: {url}");
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
    request.Method = "POST";
    request.ContentType = "application/x-www-form-urlencoded";
    request.CookieContainer = new();
    byte[] data = Encoding.ASCII.GetBytes($"login={login}&password={password}");
    request.ContentLength = data.Length;
    using Stream stream = request.GetRequestStream();
    stream.Write(data, 0, data.Length);

    using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    Debug.WriteLine($"Статус выполнения: {response.StatusCode}");
    string responseFromServer = new StreamReader(response.GetResponseStream()).ReadToEnd();
    Console.WriteLine(responseFromServer);
    token = response.Cookies["token"];
    return token;

}
