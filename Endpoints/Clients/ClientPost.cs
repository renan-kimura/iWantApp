﻿using IWantApp.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;


namespace IWantApp.Endpoints.Clients;
public class ClientPost
{
    public static string Template => "/clients";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(ClientRequest clientRequest, UserCreator userCreator)
    {
        var userClaims = new List<Claim>
        {
            new Claim("ClientCode", clientRequest.Cpf),
            new Claim("Name", clientRequest.Name),
        };
        (IdentityResult identity, string userId) result = await userCreator.Create(clientRequest.Email, clientRequest.Password, userClaims);

        if (!result.identity.Succeeded)
            return Results.ValidationProblem(result.identity.Errors.ConvertToProblemDetails());

        return Results.Created($"/clients/{result.userId}", result.userId);
    }
}