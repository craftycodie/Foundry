using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.HaloOnline
{
    public static class Maps
    {
        public static Dictionary<int, string> mapIDs = new Dictionary<int, string>()
        {
            //Halo 3
            {30, "Last Resort"},
            {300, "Construct"},
            {310, "deadlock"},
            {320, "Guardian"},
            {330, "Isolation"},
            {340, "Valhalla"},
            {350, "Salvation"},
            {360, "Snowbound"},
            {380, "Narrows"},
            {390, "The Pit"},
            {400, "Sandtrap"},
            {410, "Standoff"},
            {440, "Longshore"},
            {470, "sidewinder"},
            {480, "Foundry"},
            {490, "descent"},
            {500, "Orbital"},
            {520, "Lockout"},
            {580, "Rat's Nest"},
            {590, "Ghost Town"},
            {600, "Cold Storage"},
            {720, "midship"},
            {730, "Sandbox"},
            {740, "fortress"},
            //Halo Online
            {31, "Turf"},
            {700, "Reactor"},
            {703, "Edge"},
            {705, "Diamondback"},
            //Halo 3 Campaign
            {3005, "Arrival"},
            {3010, "Sierra 117"},
            {3020, "Crow's Nest"},
            {3030, "Tsavo Highway"},
            {3040, "The Storm"},
            {3050, "Floodgate"},
            {3070, "The Ark"},
            {3100, "The Covenant"},
            {3110, "Cortana"},
            {3120, "Halo"},
            {3130, "Epilogue"},
            //Custom
            {-1, "Flatgrass" }
        };
    }
}
