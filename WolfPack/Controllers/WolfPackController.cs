using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WolfPack.EntityFramework;

namespace WolfPack.Controllers
{
    [ApiController]
    public class WolfPackController: WolfPackControllerBase
    {
        private readonly WolfPackContext _wolfPackContext;

        public WolfPackController(WolfPackContext wolfPackContext)
        {
            _wolfPackContext = wolfPackContext;
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
            var wolves = await _wolfPackContext.Wolves.ToListAsync();
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
            return Ok(result);// TODO: 400 response seems to be done automatically, should I still try to catch it?
                              // TODO: and what about the other calls, like 404?
        }

        public override async Task<ActionResult<Wolf>> AddWolf(Wolf body) //TODO: Shouldn't this be AddWolves (plural)?
        {
            var result1 = _wolfPackContext.Wolves.Add(new EntityFramework.Wolf() //TODO: Should I use async here?
            {
                Id = Guid.NewGuid(),
                Name = body.Name,
                Gender = body.Gender,
                Birthdate = body.Birthdate,
                GpsLocation = body.GpsLocation
            });
            var result2 = await _wolfPackContext.SaveChangesAsync();
            Console.Write("log AddWolf: " + result1 + ", result2: " + result2);
            //System.Diagnostics.Trace("log addwolf: " + result1 + ", " + result2);
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

        public override async Task<ActionResult<Pack>> AddPack(Pack body)
        {
            List<EntityFramework.Wolf> wolves = new List<EntityFramework.Wolf>();//TODO: Is this the way to create an empty list?
            foreach (var wolf in body.Wolves)
            {
                var aWolf = new EntityFramework.Wolf() //Seems like this could be replaced by the AddWolf method?
                {
                    Id = Guid.NewGuid(),
                    Name = wolf.Name,
                    Gender = wolf.Gender,
                    Birthdate = wolf.Birthdate,
                    GpsLocation = wolf.GpsLocation
                };
                wolves.Add(aWolf);
                var result1 = _wolfPackContext.Wolves.Add(aWolf);//TODO: how to use console.log or w/e?
            }

//            IList<Wolf> newWolves = body.Wolves;
//            var wolves = _wolfPackContext.Wolves.AddRange(newWolves); //TODO: Should I just call AddWol(f/ves) here? 
            var result2 = _wolfPackContext.Packs.Add(new EntityFramework.Pack() //TODO: Should I use await & async here?
            {
                Id = Guid.NewGuid(),
                Name = body.Name,
                Wolves = wolves
            });
            var result3 = await _wolfPackContext.SaveChangesAsync();
            throw new NotImplementedException();
                
        }

        public override Task<ActionResult<ICollection<Pack>>> GetPacks(string searchString, int? skip, int? limit)
        {
            throw new NotImplementedException();
        }
    }
}