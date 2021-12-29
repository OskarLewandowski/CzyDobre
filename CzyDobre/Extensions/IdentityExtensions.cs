using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace CzyDobre.IdentityExtensions
{
   public static class IdentityExtensions
    {
        public static string GetAvatarURL(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("AvatarURL");
            if (claim != null)
            {
                return "https://res.cloudinary.com/czydobre-pl/image/upload/v1640790462/CzyDobre-awatary/" + claim.Value;
            }
            else
            {
                return "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAAAXNSR0IArs4c6QAAAgZJREFUeF7tmEuLwkAQhFtBDYKIkXjwgXjx4v//F15z8SKBJIhv0YPgYZeeZZZJdlYnExVJam6SSXq6Ul0fpjKfz7+oxKsCAeAAjAAyoMQZSAhBUAAUAAVAAVCgxAoAg8AgMAgMAoMlhgD+DAGDwCAwCAwCg8CghQKz2YwcxxF3ns9nWiwWiad0u10ajUZ0OBwoCAKLCj+3vLqOFQan06lofrlcUr1ep+FwSJvNhuI4/m1UHny73VoL8I46mQVotVo0Ho9pv98nGlZfMV9vt9tUrVa1DmBxbrebcA0/bzKZ0Ol0Sgj1jDomtsssAFt7MBiIBprN5p8R4OvsiOPxSJ1ORyuA2lytVqNGo6Edobx1XiYAzzYL4Ps+9ft96vV6tF6vhSPYtrx2u93dDOD7PM8Te8MwFPvVJTMkb51HIlg5QJ15aeHr9UqXy4Vc1xVW5mx4FILqKKQPKp0ksyVPnXsiZBYgPbPqwbgQ/04vXRByTsi9ujx5Vp2nO4AfyDbn2eUR4Eb4ba1Wq0Qo3sOgzJEoisT8S9cwTtWVt86j5vl6ZgfIh6p81r3h/wTQpb7aaPrQtnVMms8lgGmBT99n7YBPb8z0fBAAH0TwQQQfRPBBxDQxi7gPFAAFQAFQABQoYrqb9gQKgAKgACgACpgmZhH3gQKgACgACoACRUx3055AAVCg5BT4BmBdKR+GxUqvAAAAAElFTkSuQmCC";
            }
        }
    }
}