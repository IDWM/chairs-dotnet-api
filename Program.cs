using chairs_dotnet8_api;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("chairlist"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

var chairs = app.MapGroup("api/chair");

chairs.MapPost("/", AddChair);
chairs.MapGet("/", GetChairs);
chairs.MapGet("/{nombre}", GetChairByName);
chairs.MapPut("/{id}", UpdateChair);
chairs.MapPut("/{id}/stock", UpdateStock);
chairs.MapPost("/purchase", PurchaseChair);
chairs.MapDelete("/{id}", DeleteChair);

app.Run();


static IResult AddChair([FromBody] Chair chair, DataContext db){
    var existingChair = db.Chairs.Where(c => c.Nombre == chair.Nombre).FirstOrDefault();
    if(existingChair != null)
    {
        return TypedResults.BadRequest("La silla ya existe");
    }
    if(chair.Nombre == string.Empty || chair.Tipo == string.Empty || chair.Material == string.Empty || chair.Color == string.Empty
        || chair.Stock < 0 || chair.Precio <= 0 || chair.Altura <= 0 || chair.Anchura <= 0 || chair.Profundidad <= 0)
    {
        return TypedResults.BadRequest("Debes agregar todas las caracteristicas de la silla");
    }

    db.Chairs.Add(chair);
    db.SaveChanges();
    return TypedResults.Created($"/chair/{chair.Id}", chair);
}

static IResult GetChairs(DataContext db){
    var chairs = db.Chairs.ToList();
    return TypedResults.Ok(chairs);
}

static IResult GetChairByName(string nombre, DataContext db){
    var existingChair = db.Chairs.Where(c => c.Nombre == nombre).FirstOrDefault();
    if(existingChair == null)
    {
        return TypedResults.NotFound("La silla no existe");
    }
    return TypedResults.Ok(existingChair);
}

static IResult UpdateChair(int id, [FromBody] Chair chair, DataContext db){
    var existingChair = db.Chairs.Where(c => c.Id == id).FirstOrDefault();
    if(existingChair == null)
    {   
        return TypedResults.NotFound("La silla no existe");
    }

    if(chair.Nombre != string.Empty){
        existingChair.Nombre = chair.Nombre;
    }
    if(chair.Tipo != string.Empty){
        existingChair.Tipo = chair.Tipo;
    }
    if(chair.Material != string.Empty){
        existingChair.Material = chair.Material;
    }
    if(chair.Color != string.Empty){
        existingChair.Color = chair.Color;
    }    
    if(chair.Altura > 0){
        existingChair.Altura = chair.Altura;
    }
    if(chair.Anchura > 0){
        existingChair.Anchura = chair.Anchura;
    }
    if(chair.Profundidad > 0){
        existingChair.Profundidad = chair.Profundidad;
    }
    if(chair.Precio > 0){
        existingChair.Precio = chair.Precio;
    }

    db.Entry(existingChair).State = EntityState.Modified;
    db.SaveChanges();

    return TypedResults.NoContent();      
}

static IResult UpdateStock(int id, [FromBody] UpdateStockDto updateStockDto, DataContext db){
    var existingChair = db.Chairs.Where(c => c.Id == id).FirstOrDefault();
    if(existingChair == null)
    {   
        return TypedResults.NotFound("La silla no existe");
    }

    if(updateStockDto.Stock <= 0){
        return TypedResults.BadRequest("El stock proporcionado debe ser mayor a 0");
    }

    existingChair.Stock += updateStockDto.Stock;
    db.Entry(existingChair).State = EntityState.Modified;
    db.SaveChanges();

    return TypedResults.Ok(existingChair);
}

static IResult PurchaseChair([FromBody] PurchaseDto purchaseDto, DataContext db){
    var existingChair = db.Chairs.Where(c => c.Id == purchaseDto.Id).FirstOrDefault();
    if(existingChair == null)
    {   
        return TypedResults.NotFound("La silla no existe");
    }
    if(purchaseDto.Cantidad <= 0){
        return TypedResults.BadRequest("Debes proporcionar los datos de la compra");
    }
    if(existingChair.Stock < purchaseDto.Cantidad){
        return TypedResults.BadRequest("No hay stock suficiente");
    }
    int total = existingChair.Precio * purchaseDto.Cantidad;
    if(purchaseDto.Pago != total){
        return TypedResults.BadRequest("El pago no es valido");
    }
    existingChair.Stock -= purchaseDto.Cantidad;
    db.Entry(existingChair).State = EntityState.Modified;
    db.SaveChanges();

    return TypedResults.Ok("Compra realizada con éxito");

}

static IResult DeleteChair(int id, DataContext db){
    var existingChair = db.Chairs.Where(c => c.Id == id).FirstOrDefault();
    if(existingChair == null)
    {   
        return TypedResults.NotFound("La silla no existe");
    }

    db.Chairs.Remove(existingChair);
    db.SaveChanges();

    return TypedResults.NoContent();
}

