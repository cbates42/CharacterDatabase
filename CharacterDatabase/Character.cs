using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterDatabase
{
    internal class Character
    {

        public string character;
        public string type;
        public string? location;
        public bool originalchar;
        public bool? swordfighter;
        public bool? magicuser;

        public Character(string character, string type, string? location, bool originalchar, bool? swordfighter, bool? magicuser)
        {
            this.character = character;
            this.type = type;
            this.location = location;
            this.originalchar = originalchar;
            this.swordfighter = swordfighter;  
            this.magicuser = magicuser; 
        }

    }
}
