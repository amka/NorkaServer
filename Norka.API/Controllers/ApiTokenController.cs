using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Norka.API.Data;
using Norka.API.Entities;
using Norka.API.Models.Request;
using Norka.API.Services;

namespace Norka.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ApiTokenController(
    ApiTokenService tokenService,
    UserManager<ApplicationUser> userManager) : ControllerBase
{
    // GET: api/ApiToken
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApiToken>>> GetApiTokensAsync()
    {
        var userId = userManager.GetUserId(HttpContext.User);
        return Ok(await tokenService.GetApiTokensAsync(userId!));
    }

    // GET: api/ApiToken/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiToken>> GetApiToken(string id)
    {
        var userId = userManager.GetUserId(HttpContext.User);
        var apiToken = await tokenService.GetApiTokenAsync(id, userId!);

        if (apiToken == null) return NotFound();

        return apiToken;
    }

    // POST: api/ApiToken
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<ApiToken>> PostApiToken(CreateApiTokenRequest requst)
    {
        if (!ModelState.IsValid) return BadRequest();

        var userId = userManager.GetUserId(HttpContext.User);
        var apiToken = await tokenService.CreateApiTokenAsync(userId!, requst.Title, requst.ExpiresIn);

        return CreatedAtAction("GetApiToken", new { id = apiToken.Id }, apiToken);
    }

    // DELETE: api/ApiToken/5
    /// <summary>
    /// Delete api token by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteApiToken(string id)
    {
        var userId = userManager.GetUserId(HttpContext.User);
        var apiToken = await tokenService.GetApiTokenAsync(id, userId!);
        if (apiToken == null) return NotFound();

        await tokenService.DeleteAsync(apiToken);

        return NoContent();
    }
}