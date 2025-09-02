using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CheckInFunction;

public class VisitorRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class RegisterVisitor
{
    private readonly ILogger<RegisterVisitor> _logger;

    public RegisterVisitor(ILogger<RegisterVisitor> logger)
    {
        _logger = logger;
    }

    [Function("RegisterVisitor")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req
    )
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<VisitorRequest>(requestBody);

        if (data == null)
        {
            return new BadRequestObjectResult("Invalid json string.");
        }

        string name = data.Name;
        string email = data.Email;

        //Validation
        if (string.IsNullOrWhiteSpace(name))
        {
            return new BadRequestObjectResult("Name is required.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return new BadRequestObjectResult("Email is required.");
        }

        if (!IsValidEmail(email))
        {
            return new BadRequestObjectResult("You need to have a valid email address.");
        }

        string responseMessage = $"Welcome {name}!";

        return new OkObjectResult(responseMessage);
    }

    //.NET email validation
    private static bool IsValidEmail(string email)
    {
        try
        {
            var emailadress = new System.Net.Mail.MailAddress(email);
            return emailadress.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
