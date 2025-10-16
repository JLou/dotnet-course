namespace Aspire.WebApp.Clients;

internal class TodoClient : ITodoClient
{
    private readonly HttpClient _httpClient;

    public TodoClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TodoItem>> GetTodoItemsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<TodoItem>>("/api/todo");
        return response ?? new List<TodoItem>();
    }

    public async Task<TodoItem> CreateTodoItemAsync(TodoItem item)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/todo", item);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TodoItem>()
            ?? throw new Exception("Failed to create todo item");
    }
}
