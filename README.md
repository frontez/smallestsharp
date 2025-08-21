C# net9.0
2.78 MB

Smallest docker image with c# application which returns the text "hello world" to a GET request to /* (or /hello)
(with a 200 status code and the Content-Type: text/plain header).



https://hub.docker.com/repository/docker/frontez/csharp-smallest

docker build -t frontez/csharp-smallest .

docker run -p 8080:8080 --rm --name csharp-smallest frontez/csharp-smallest

docker images --format "{{.Repository}} {{.Size}}" | grep frontez/csharp-smallest
