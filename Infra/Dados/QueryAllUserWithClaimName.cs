using Dapper;
using IWantApp.Endpoints.Employees;
using Microsoft.Data.SqlClient;

namespace IWantApp.Infra.Dados;

public class QueryAllUserWithClaimName
{
    private readonly IConfiguration configuration;

    public QueryAllUserWithClaimName(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task<IEnumerable<EmployeeResponse>> Execute(int page, int rows)
    {
        var db = new SqlConnection(configuration["ConnectionString:IWantDb"]);
        var query =
            @"SELECT Email, ClaimValue as Name
                FROM AspNetUsers u inner join AspNetUserClaims c 
                ON u.id = c.UserId and ClaimType = 'Name'
                ORDER BY Name
                OFFSET (@page -1) * @rows ROWS FETCH NEXT @rows ROWS ONLY
            ";
        return await db.QueryAsync<EmployeeResponse>(
            query,
            new { page, rows }
        );
    }
}
