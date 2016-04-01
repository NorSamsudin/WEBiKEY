using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBiKEY.Application.Classes
{
    public class FriendlyColumnNames
    {
        public static string CategoryName = "Category Name";
        public static string CharacterCode = "Character Code";
        public static string CharacterDescription = "Character Description";
        public static string CharacterStateCode = "Character State Code";
        public static string CharacterStateDescription = "Character State Description";

        public static string SpeciesName = "Species Name";

        public static string[] GetAllCharacterColumnNames()
        {
            return new[] { CategoryName, CharacterCode, CharacterDescription, CharacterStateCode, CharacterStateDescription };
        }

        public static string[] GetAllSpeciesColumnNames()
        {
            return new[] { SpeciesName };
        }
    }
}