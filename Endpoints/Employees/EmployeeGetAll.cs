using IWantApp.Infra.Dados;
using Microsoft.AspNetCore.Authorization;

namespace IWantApp.Endpoints.Employees;

public class EmployeeGetAll 
{ 
    public static string Template => "/employee";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "Employee006Policy")] //apenas o EmployeeCode 006 terá acesso
    public static async Task<IResult> Action(int? page, int? rows, QueryAllUserWithClaimName query)
    {
        var result = await query.Execute(page.Value, rows.Value);
        return Results.Ok(result);
    }
}