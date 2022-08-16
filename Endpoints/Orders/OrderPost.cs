using IWantApp.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using IWantApp.Infra.Dados;
using IWantApp.Domain.Products;
using IWantApp.Domain.Orders;

namespace IWantApp.Endpoints.Clients;
public class OrderPost
{
    public static string Template => "/orders";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "CpfPolicy")]
    public static async Task<IResult> Action(OrderRequest orderRequest, HttpContext http, ApplicationDbContext context)
    {
        var clientId = http.User.Claims
            .First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var clientName = http.User.Claims
            .First(c => c.Type == "Name").Value;
        //DESSA FORMA FUNCIONA, MAS FAZ UMA CONSULTA POR LAÇO
        /*var products = new List<Product>();
        foreach(var item in orderRequest.ProductIds)
        {
            var product = context.Products.First(p => p.Id == item);
            products.Add(product);
        }*/ 
        //ASSIM CARREGA TUDO DE UMA VEZ E DEPOIS MANIPULA
        var productsFound = context.Products.Where(p => orderRequest.ProductIds.Contains(p.Id)).ToList();

        var order = new Order(clientId, clientName, productsFound, orderRequest.DeliveryAddress);
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        return Results.Created($"/orders/{order.Id}", order.Id);
    }
}