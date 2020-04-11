using System.Collections.Generic;
using System.Linq;

namespace Entities
{
    public class Location
    {
        public string Name 
        { 
            get { return _name; }
            set { _name = value; }
        }
        private string _name = "Sunset Beach";

        public Character[] CharactersOnLocation { get { return _charactersOnLocation.ToArray(); } }
        public bool AddCharacterOnLocation(Character location) => _charactersOnLocation.Add(location);

        private HashSet<Character> _charactersOnLocation;
    }
}