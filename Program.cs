using System.Net;
using System.Text;

class Program
{
    static void Main()
    {
        using var listener = new HttpListener();
        listener.Prefixes.Add("http://*:8080/");
        listener.Start();

        while (true)
        {
            var context = listener.GetContext();
            HandleRequest(context);
        }
    }

    static void HandleRequest(HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;

        if (request.HttpMethod == "GET" && 
            (request.Url.AbsolutePath == "/" || request.Url.AbsolutePath == "/hello"))
        {
            const string responseString = "hello world";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            
            response.StatusCode = 200;
            response.ContentType = "text/plain";
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
        }
        else
        {
            response.StatusCode = 404;
        }
        
        response.Close();
    }
}