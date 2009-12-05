using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Characters
{
    public enum CharacterRace
    {
        Invalid = 0,
        Human = 1,
        Canid = 2,
        Gurian = 3,
    }

    public enum CharacterGender
    {
        Invalid = 0,
        Female = 1,
        Male = 2,
    }

    public enum CharacterClass
    {
        Invalid = 0,
        Fighter = 1,
        Ranger = 2,
        Tech = 3,
        Mage = 4,
        Healer = 5,
        Scout = 6,
    }

    public class Character
    {
        public int Level = -1;
        public int Experience = -1;
        public CharacterRace Race = CharacterRace.Invalid;
        public CharacterGender Gender = CharacterGender.Invalid;
        public CharacterClass Class = CharacterClass.Invalid;
        public UInt64 CharacterID = 0;
        public UInt64 PlayerID = 0;
        public string Name = string.Empty;
    }
}
