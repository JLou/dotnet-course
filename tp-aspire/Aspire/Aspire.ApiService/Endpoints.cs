namespace Aspire.ApiService;

using global::Microsoft.AspNetCore.Authorization;
using global::Microsoft.AspNetCore.Mvc;
using global::Microsoft.EntityFrameworkCore;
using Persistence;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGet("/api/todo", async (MyAppContext db) => await db.Items.ToListAsync());
        app.MapPost(
            "/api/todo",
            [Authorize(Roles = "gestionnaire")]
            async ([FromBody] Item item, [FromServices] MyAppContext db, HttpContext context) =>
            {
                var isGestionnaire = context.User.IsInRole("gestionnaire");
                db.Items.Add(item);
                await db.SaveChangesAsync();
                return Results.Created($"/api/todo/{item.Id}", item);
            }
        );
    }
}
