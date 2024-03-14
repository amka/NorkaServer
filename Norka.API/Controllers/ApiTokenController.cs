using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Norka.API.Data;
using Norka.API.Entities;

namespace Norka.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ApiTokenController(
    NorkaDbContext context,
    UserManager<ApplicationUser> userManager) : ControllerBase
{
    // GET: api/ApiToken
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApiToken>>> GetApiTokensAsync()
    {
        var userId = userManager.GetUserId(HttpContext.User);
        return await context.ApiTokens.Where(x => x.UserId == userId).ToListAsync();
    }

    // GET: api/ApiToken/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiToken>> GetApiToken(string id)
    {
        var userId = userManager.GetUserId(HttpContext.User);
        var apiToken = await context.ApiTokens
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);

        if (apiToken == null) return NotFound();

        return apiToken;
    }

    // PUT: api/ApiToken/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutApiToken(string id, ApiToken apiToken)
    {
        if (id != apiToken.Id)
        {
            return BadRequest();
        }

        context.Entry(apiToken).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ApiTokenExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/ApiToken
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<ApiToken>> PostApiToken(ApiToken apiToken)
    {
        context.ApiTokens.Add(apiToken);
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (ApiTokenExists(apiToken.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetApiToken", new { id = apiToken.Id }, apiToken);
    }

    // DELETE: api/ApiToken/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteApiToken(string id)
    {
        var apiToken = await context.ApiTokens.FindAsync(id);
        if (apiToken == null)
        {
            return NotFound();
        }

        context.ApiTokens.Remove(apiToken);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool ApiTokenExists(string id)
    {
        return context.ApiTokens.Any(e => e.Id == id);
    }
}