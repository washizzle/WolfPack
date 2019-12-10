namespace WolfPack.Controllers
{
    public static class WolfExtensions
    {
        public static Wolf Convert(this EntityFramework.Wolf wolf)
        {
            return new Wolf()
            {
                Id = wolf.Id,
                Name = wolf.Name,
                Gender = wolf.Gender,
                Birthdate = wolf.Birthdate,
                GpsLocation = wolf.GpsLocation

            };
        }
    }
}