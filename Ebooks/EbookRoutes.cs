using api_dotnet_ef_csharp.Database;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api_dotnet_ef_csharp.Ebooks;

public static class EbookRoutes
{
    public static void ebookRoutes(this WebApplication app)
    {
        // var ebookRoutes = app.MapGroup("ebooks");
        app.MapPost("ebooks", async (EbookRequestDTO request, AppDbContext context, CancellationToken ct) =>
        {
            var exists = await context.Ebooks.AnyAsync(ebook => ebook.Title == request.Title, ct);

            if (exists)
                return Results.Conflict("This ebook title already exists!");

            var newEbook = new Ebook(request.Title);

            await context.Ebooks.AddAsync(newEbook, ct);
            await context.SaveChangesAsync(ct);

            var ebookToReturn = new EBookDTO(newEbook.Id, newEbook.Title);

            return Results.Ok(ebookToReturn);
        });

        app.MapGet("ebooks", async (AppDbContext context, CancellationToken ct) =>
        {
            var ebooks = await context.Ebooks
            .Where(ebook => ebook.Available)
            .Select(ebook => new EBookDTO(ebook.Id, ebook.Title))
            .ToListAsync(ct);

            return ebooks;
        });

        app.MapPut("ebooks/{id}", async (Guid id, EbookRequestDTO request, AppDbContext context, CancellationToken ct) =>
        {
            var ebook = await context.Ebooks.SingleOrDefaultAsync(ebook => ebook.Id == id, ct);

            if (ebook == null)
                return Results.NotFound();

            ebook.updateTitle(request.Title);

            await context.SaveChangesAsync(ct);

            return Results.Ok(new EBookDTO(ebook.Id, ebook.Title));
        });

        app.MapDelete("ebooks/{id}", async (Guid id, AppDbContext context, CancellationToken ct) =>
        {
            var ebook = await context.Ebooks
                .SingleOrDefaultAsync(ebook => ebook.Id == id, ct);

            if (ebook == null)
                return Results.NotFound();

            ebook.turnNotAvailable();

            await context.SaveChangesAsync(ct);
            return Results.Ok();
        });
    }

}