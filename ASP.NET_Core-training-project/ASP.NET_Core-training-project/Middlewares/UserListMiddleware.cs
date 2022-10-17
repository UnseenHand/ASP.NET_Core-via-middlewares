using System.Text.RegularExpressions;
using Users;
public class UserListMiddleware
{
    private RequestDelegate next;
    public UserListMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var response = context.Response;
        var request = context.Request;
        var path = request.Path;
        //string expressionForNumber = "^/api/users/([0 - 9]+)$";   // если id представляет число

        // 2e752824-1657-4c7f-844b-6ec2e168e99c
        string expressionForGuid = @"^/index/\w{8}-\w{4}-\w{4}-\w{4}-\w{12}$";
        if (path == "/index" && request.Method == "GET")
        {
            await UserList.GetAllPeople(response);
        }
        else if (Regex.IsMatch(path, expressionForGuid) && request.Method == "GET")
        {
            // получаем id из адреса url
            string? id = path.Value?.Split("/")[2];
            await UserList.GetPerson(id, response);
        }
        else if (path == "/index" && request.Method == "POST")
        {
            await UserList.CreatePerson(response, request);
        }
        else if (path == "/index" && request.Method == "PUT")
        {
            await UserList.UpdatePerson(response, request);
        }
        else if (Regex.IsMatch(path, expressionForGuid) && request.Method == "DELETE")
        {
            string? id = path.Value?.Split("/")[2];
            await UserList.DeletePerson(id, response);
        }
        else
        {
            response.ContentType = "text/html; charset=utf-8";
            await response.SendFileAsync("wwwroot/html/index.html");
        }
    }
}