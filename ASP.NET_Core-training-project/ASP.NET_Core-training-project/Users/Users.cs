namespace Users;
internal class UserList
{
    private static List<Person> users = new List<Person>
    {
        new() { Id = Guid.NewGuid().ToString(), Name = "Tom", Age = 37 },
        new() { Id = Guid.NewGuid().ToString(), Name = "Bob", Age = 41 },
        new() { Id = Guid.NewGuid().ToString(), Name = "Sam", Age = 24 }
    };
    // получение всех пользователей
    internal static async Task GetAllPeople(HttpResponse response)
    {
        await response.WriteAsJsonAsync(users);
    }
    // получение одного пользователя по id
    internal static async Task GetPerson(string? id, HttpResponse response)
    {
        // получаем пользователя по id
        Person? user = users.FirstOrDefault((u) => u.Id == id);
        // если пользователь найден, отправляем его
        if (user != null)
            await response.WriteAsJsonAsync(user);
        // если не найден, отправляем статусный код и сообщение об ошибке
        else
        {
            response.StatusCode = 404;
            await response.WriteAsJsonAsync(new { message = "Пользователь не найден" });
        }
    }

    internal static async Task DeletePerson(string? id, HttpResponse response)
    {
        // получаем пользователя по id
        Person? user = users.FirstOrDefault((u) => u.Id == id);
        // если пользователь найден, удаляем его
        if (user != null)
        {
            users.Remove(user);
            await response.WriteAsJsonAsync(user);
        }
        // если не найден, отправляем статусный код и сообщение об ошибке
        else
        {
            response.StatusCode = 404;
            await response.WriteAsJsonAsync(new { message = "Пользователь не найден" });
        }
    }

    internal static async Task CreatePerson(HttpResponse response, HttpRequest request)
    {
        try
        {
            // получаем данные пользователя
            var user = await request.ReadFromJsonAsync<Person>();
            if (user != null)
            {
                // устанавливаем id для нового пользователя
                user.Id = Guid.NewGuid().ToString();
                // добавляем пользователя в список
                users.Add(user);
                await response.WriteAsJsonAsync(user);
            }
            else
            {
                throw new Exception("Некорректные данные");
            }
        }
        catch (Exception)
        {
            response.StatusCode = 400;
            await response.WriteAsJsonAsync(new { message = "Некорректные данные" });
        }
    }

    internal static async Task UpdatePerson(HttpResponse response, HttpRequest request)
    {
        try
        {
            // получаем данные пользователя
            Person? userData = await request.ReadFromJsonAsync<Person>();
            if (userData != null)
            {
                // получаем пользователя по id
                var user = users.FirstOrDefault(u => u.Id == userData.Id);
                // если пользователь найден, изменяем его данные и отправляем обратно клиенту
                if (user != null)
                {
                    user.Age = userData.Age;
                    user.Name = userData.Name;
                    await response.WriteAsJsonAsync(user);
                }
                else
                {
                    response.StatusCode = 404;
                    await response.WriteAsJsonAsync(new { message = "Пользователь не найден" });
                }
            }
            else
            {
                throw new Exception("Некорректные данные");
            }
        }
        catch (Exception)
        {
            response.StatusCode = 400;
            await response.WriteAsJsonAsync(new { message = "Некорректные данные" });
        }
    }
}
internal class Person
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public int Age { get; set; }
}