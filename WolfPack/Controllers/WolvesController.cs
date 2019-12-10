using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WolfPack.EntityFramework;

namespace WolfPack.Controllers
{
    [ApiController]
    public class WolvesController : WolvesControllerBase
    {
        private readonly WolfPackContext _wolfPackContext;

        public WolvesController(WolfPackContext wolfPackContext)
        {
            _wolfPackContext = wolfPackContext;
        }

        public override async Task<IActionResult> DeleteWolfById(Guid id)
        {
            var theWolf = await _wolfPackContext.Wolves.FirstOrDefaultAsync(a => a.Id == id);
            if (theWolf == null)
            {
                return NotFound("wolf not found");
            }

            _wolfPackContext.Wolves.Remove(theWolf);
            await _wolfPackContext.SaveChangesAsync();
            return Ok();
        }

        public override async Task<IActionResult> UpdateWolfById(Guid id, Wolf body)
        {
            var theWolf = await _wolfPackContext.Wolves.FirstOrDefaultAsync(a => a.Id == id);
            if (theWolf == null)
            {
                return NotFound("wolf not found");
            }

            theWolf.Birthdate = body.Birthdate;
            theWolf.Gender = body.Gender;
            theWolf.Name = body.Name;
            theWolf.GpsLocation = body.GpsLocation;
            await _wolfPackContext.SaveChangesAsync();
            return Ok(body);
        }

        public override async Task<ActionResult<ICollection<Wolf>>> GetWolves(string searchString, int? skip,
            int? limit)
        {
            var result = new List<Wolf>();
            var wolves = await _wolfPackContext.Wolves.Skip(skip ?? 0).Take(limit ?? 50).ToListAsync();
            foreach (var wolf in wolves)
            {
                result.Add(wolf.Convert());
            }

            return Ok(result);
        }

        public override async Task<ActionResult<Wolf>>
            AddWolf([Microsoft.AspNetCore.Mvc.FromBody] Wolf body)
        {
            var wolf = new EntityFramework.Wolf()
            {
                Id = Guid.NewGuid(),
                Name = body.Name,
                Gender = body.Gender,
                Birthdate = body.Birthdate,
                GpsLocation = body.GpsLocation
            };
            await _wolfPackContext.Wolves.AddAsync(wolf);
            await _wolfPackContext.SaveChangesAsync();
            return Created($"/wolves/{wolf.Id}", wolf);
        }

        
    }
}