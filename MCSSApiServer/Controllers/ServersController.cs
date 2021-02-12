using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCSSApiServer.Models;
using Microsoft.EntityFrameworkCore;

namespace MCSSApiServer.Controllers
{
    [Route("api/[controller]")]
    public class ServersController : Controller
    {
        private readonly DatabaseContext databaseContext;
        public ServersController(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        private async Task AddAndSaveServerAsync(Server server)
        {
            await Task.Run(() => AddAndSaveServer(server));
        }

        private void AddAndSaveServer(Server server)
        {
            databaseContext.Servers.Add(server);
            databaseContext.SaveChanges();
        }

        // GET: api/servers
        [HttpGet]
        public async Task<IQueryable<Server>> GetServersAsync()
        {
            var servers = await databaseContext.Servers.ToListAsync();

            return servers.AsQueryable();
        }

        // GET: api/servers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Server>> GetServerByIdAsync(int id)
        {
            var server = await databaseContext.Servers.FindAsync(id);

            if (server == null)
            {
                return NotFound();
            }

            return server;
        }

        // POST: api/servers
        [HttpPost]
        public async Task<IActionResult> AddNewServerAsync([FromBody] Server server)
        {
            await AddAndSaveServerAsync(server);

            return Ok(server);
        }

    }
}
