using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WolfPack.EntityFramework;

namespace WolfPack.Controllers
{
    [ApiController]
    public class WolfPackController: WolfPackControllerBase
    {
        private readonly WolfContext _wolfContext;

        public WolfPackController(WolfContext wolfContext)
        {
            _wolfContext = wolfContext;
        }

        public override async Task<IActionResult> DeleteWolfById(Guid id)
        {
            return BadRequest();
            throw new NotImplementedException();
        }

        public override Task<IActionResult> UpdateWolfById(Guid id, Wolf body)
        {
            throw new NotImplementedException();
        }

        public override async Task<ActionResult<ICollection<Wolf>>> GetWolves(string searchString, int? skip, int? limit)
        {
            var result = new List<Wolf>();
            var wolves = await _wolfContext.Wolves.ToListAsync();
            var numberOfWolvesInList = 0;
            foreach (var wolf in wolves)
            {
                if (skip <= numberOfWolvesInList && numberOfWolvesInList < limit) {
                    result.Add(new Wolf()
                    {
                        Id = wolf.Id,
                        Name = wolf.Name,
                        Gender = wolf.Gender,
                        Birthdate = wolf.Birthdate,
                        GpsLocation = wolf.GpsLocation
                    });
                }
                numberOfWolvesInList++;
            }
            return Ok(result);
        }

        public override async Task<ActionResult<Wolf>> AddWolf(Wolf body)
        {
            var result1 = await _wolfContext.Wolves.AddAsync(new EntityFramework.Wolf()
            {
                Id = Guid.NewGuid(),
                Name = body.Name,
                Gender = body.Gender,
                Birthdate = body.Birthdate,
                GpsLocation = body.GpsLocation
            });
            var result2 = await _wolfContext.SaveChangesAsync();
            return Created(result1.ToString(), result2); //TODO: test this

            return BadRequest();
            throw new NotImplementedException();
        }

        public override Task<IActionResult> DeleteWolfFromPack(Guid packId, Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<IActionResult> AddWolfToPack(Guid packId, Guid? body)
        {
            throw new NotImplementedException();
        }

        public override Task<ActionResult<Pack>> AddPack(Pack body)
        {
            throw new NotImplementedException();
        }

        public override Task<ActionResult<ICollection<Pack>>> GetPacks(string searchString, int? skip, int? limit)
        {
            throw new NotImplementedException();
        }
    }
}