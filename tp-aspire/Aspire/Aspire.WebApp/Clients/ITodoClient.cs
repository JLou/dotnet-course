namespace Aspire.WebApp.Clients;

internal interface ITodoClient
{
    Task<List<TodoItem>> GetTodoItemsAsync();
    Task<TodoItem> CreateTodoItemAsync(TodoItem item);
}

internal class TodoItem
{
    public int Id { get; set; }
    public string Value { get; set; }
}
