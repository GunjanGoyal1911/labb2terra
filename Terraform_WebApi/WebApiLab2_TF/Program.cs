using Microsoft.EntityFrameworkCore;
using WebApiLab2_TF.Data;
using WebApiLab2_TF.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Get all books
app.MapGet("/books", async (DataContext context) =>
{
    var books = await context.Books.ToListAsync();
    return Results.Ok(books);
});

//Get book by id
app.MapGet("/book/{id}", async(DataContext context, int id) =>
{
    var book = await context.Books.FindAsync(id);
    if(book is null)
    {
        return Results.NotFound("Sorry, book doesn´t exist");
    }   
    return Results.Ok(book);
});

//Add a book
app.MapPost("/book", async(DataContext context, Book book) =>
{
    context.Books.Add(book);
    await context.SaveChangesAsync();
    return Results.Ok("Book has been added successfully");
});

//Update a book
app.MapPut("/book/{id}", async(DataContext context, Book book, int id) => 
{
    var bookToUpdate = await context.Books.FindAsync(id);
    if(bookToUpdate is null)
    {
        return Results.NotFound("Sorry, book doesn´t exist");
    }
    bookToUpdate.Title = book.Title;
    bookToUpdate.Author = book.Author;
    await context.SaveChangesAsync();
    return Results.Ok("Book has been updated successfully");
});

//Delete a book
app.MapDelete("/book/{id}", async(DataContext context, int id) =>
{
    var bookToDelete = await context.Books.FindAsync(id);
    if(bookToDelete is null)
    {
        return Results.NotFound("Book not found");
    }
    context.Books.Remove(bookToDelete);
    await context.SaveChangesAsync();
    return Results.Ok("Book has been deleted successfully");
});


app.Run();
