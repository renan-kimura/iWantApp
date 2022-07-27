namespace IWantApp.Endpoints.Employees;

public record EmployeeRequest(string Email, string password, string Name, string EmployeeCode);