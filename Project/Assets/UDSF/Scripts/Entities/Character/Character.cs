using System.Collections.Generic;
using System.Linq;
using System;

namespace Entities
{
    [Serializable]
    public class Character : Person
    {
        public Location[] CommonLocations { get { return _commonLocations.ToArray(); } }
        public bool AddLocation(Location location) => _commonLocations.Add(location);

        private HashSet<Location> _commonLocations;
    }
}