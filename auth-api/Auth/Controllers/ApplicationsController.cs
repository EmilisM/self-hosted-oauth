using Auth.Models;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;

namespace Auth.Controllers;

[Route("api/applications")]
[ApiController]
public class ApplicationsController(IOpenIddictApplicationManager applicationManager)
    : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Application>> Get()
    {
        var applications = applicationManager.ListAsync();

        var results = new List<Application>();

        await foreach (var application in applications)
        {
            var clientId = await applicationManager.GetClientIdAsync(application);
            var displayName = await applicationManager.GetDisplayNameAsync(application);

            if (clientId is null)
            {
                continue;
            }

            results.Add(new Application(clientId, displayName));
        }

        return results;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Application>> Get(string id)
    {
        var application = await applicationManager.FindByClientIdAsync(id);

        if (application is null)
        {
            return NotFound();
        }

        var clientId = await applicationManager.GetClientIdAsync(application);
        var displayName = await applicationManager.GetDisplayNameAsync(application);

        return new Application(clientId, displayName);
    }

    [HttpPost]
    public async Task<ActionResult<object>> Create([FromBody] CreateApplication request)
    {
        var applicationObject = await applicationManager.FindByClientIdAsync(request.ClientId);

        if (applicationObject is not null)
        {
            return BadRequest();
        }

        var newClient = await applicationManager.CreateAsync(
            new OpenIddictApplicationDescriptor
            {
                ClientId = request.ClientId,
                ClientSecret = request.ClientSecret,
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.Endpoints.Introspection,
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                },
                DisplayName = request.DisplayName
            }
        );

        return Ok(newClient);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<object>> Delete(string id)
    {
        var application = await applicationManager.FindByClientIdAsync(id);

        if (application is null)
        {
            return NotFound();
        }

        await applicationManager.DeleteAsync(application);

        return Ok();
    }
}
