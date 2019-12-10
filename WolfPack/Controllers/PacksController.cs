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
    public class PacksController : PacksControllerBase
    {
        private readonly WolfPackContext _wolfPackContext;

        public PacksController(WolfPackContext wolfPackContext)
        {
            _wolfPackContext = wolfPackContext;
        }

        public override async Task<IActionResult> DeleteWolfFromPack(Guid packId, Guid id)
        {
            var thePack = await _wolfPackContext.Packs.Include(a => a.Wolves).FirstOrDefaultAsync(a => a.Id == packId);
            var theWolf = await _wolfPackContext.Wolves.FirstOrDefaultAsync(a => a.Id == id);
            if (thePack == null)
            {
                return NotFound("Pack not found");
            }

            if (theWolf == null)
            {
                return NotFound("Wolf not found");
            }

            thePack.Wolves.Remove(theWolf);
            await _wolfPackContext.SaveChangesAsync();
            return Ok();
        }

        public override async Task<IActionResult> AddWolfToPack(Guid packId, Guid wolfId)
        {
            var thePack = await _wolfPackContext.Packs.Include(a => a.Wolves).FirstOrDefaultAsync(a => a.Id == packId);
            var theWolf = await _wolfPackContext.Wolves.FirstOrDefaultAsync(a => a.Id == wolfId);
            if (thePack == null)
            {
                return NotFound("Pack not found");
            }

            if (theWolf == null)
            {
                return NotFound("Wolf not found");
            }

            if (thePack.Wolves.Any(a => a.Id == theWolf.Id))
            {
                return Conflict("The wolf that you are trying to add to the pack already exists in the pack");
            }

            thePack.Wolves.Add(theWolf);
            await _wolfPackContext.SaveChangesAsync();
            return Ok(thePack);
        }

        public override async Task<ActionResult<Pack>> AddPack(Pack body)
        {
            var newPack = new EntityFramework.Pack()
            {
                Id = Guid.NewGuid(),
                Name = body.Name
            };
            await _wolfPackContext.Packs.AddAsync(newPack);
            await _wolfPackContext.SaveChangesAsync();
            return Created($"/packs/{newPack.Id}", newPack);
        }

        public override async Task<ActionResult<ICollection<Pack>>> GetPacks(string searchString, int? skip, int? limit)
        {
            var result = new List<Pack>();
            var packs = await _wolfPackContext.Packs.Include(a => a.Wolves).ToListAsync();
            foreach (var pack in packs)
            {
                var newPack = new Pack()
                {
                    Id = pack.Id,
                    Name = pack.Name,
                    Wolves = new List<Wolf>()
                };
                foreach (var wolf in pack.Wolves)
                {
                    newPack.Wolves.Add(wolf.Convert());
                }

                result.Add(newPack);
            }

            return Ok(result);
        }
    }
}