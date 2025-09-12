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
    private readonly VisitorDbContext _context;

    public RegisterVisitor(ILogger<RegisterVisitor> logger, VisitorDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [Function("RegisterVisitor")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req
    )
    {
        try
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

            // Creates and save a new Visitor to database
            var visitor = new Visitor
            {
                Name = name,
                Email = email,
                CreatedAt = DateTime.UtcNow,
            };

            _context.Visitors.Add(visitor);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Visitor {name} registered successfully with ID {visitor.Id}");

            string responseMessage = $"Welcome {name}!";

            return new OkObjectResult(responseMessage);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Invalid JSON format in request");
            return new BadRequestObjectResult("Invalid JSON format.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in RegisterVisitor function");
            return new StatusCodeResult(500);
        }
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
