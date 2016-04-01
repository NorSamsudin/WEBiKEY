using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WEBiKEY.Application;

namespace WEBiKEY.Application.Classes
{
    /// <summary>
    /// Summary description for Extensions
    /// </summary>
    public static class Extensions
    {
        public static bool? GetNullableBool(this string value)
        {
            bool temp;

            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()))
            {
                return null;
            }
            if (bool.TryParse(value, out temp))
            {
                return temp;
            }

            string firstChar = value.Trim().Substring(0, 1);
            if (firstChar.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            else if (firstChar.Equals("N", StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }

            return null;
        }

        public static int? GetNullableInteger(this string value)
        {
            int temp;

            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()))
            {
                return null;
            }
            if (int.TryParse(value, out temp))
            {
                return temp;
            }

            return null;
        }

        public static decimal? GetNullableDecimal(this string value)
        {
            decimal temp;
            float tempf;

            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()))
            {
                return null;
            }
            if (decimal.TryParse(value, out temp))
            {
                return temp;
            }
            else if (float.TryParse(value, out tempf))
            {
                return (decimal)tempf;
            }

            return null;
        }

        public static int AssignNewdCharacterCategoryId(this CharacterCategory category, InteractiveKeyEntities context)
        {
            int id = 0;
            try
            {
                id = context.CharacterCategories.Max(c => c.CategoryID);
            }
            catch (Exception)
            {

            }

            category.CategoryID = id + 1;
            return id + 1;
        }

        public static int AssignNewdCharacterId(this Character character, InteractiveKeyEntities context)
        {
            int id = 0;
            try
            {
                id = context.Characters.Max(s => s.CharacterID);
            }
            catch (Exception)
            {

            }

            character.CharacterID = id + 1;
            return id + 1;
        }

        public static int AssignNewCharacterStateId(this Application.CharacterState characterState, InteractiveKeyEntities context)
        {
            int id = 0;
            try
            {
                id = context.CharacterStates.Max(s => s.CharacterStateID);
            }
            catch (Exception)
            {

            }

            characterState.CharacterStateID = id + 1;
            return id + 1;
        }

        public static int AssignNewSpeciesId(this Species species, InteractiveKeyEntities context)
        {
            int id = 0;
            try
            {
                id = context.Species.Max(s => s.SpeciesID);
            }
            catch (Exception)
            {

            }

            species.SpeciesID = id + 1;
            return id + 1;
        }
    }

}

namespace WEBiKEY.Application
{
    public partial class CharacterState
    {
        public string CharacterStateDescriptionWithCode { get { return this.CharacterStateCode + ": " + this.CharacterStateDescription; } }
    }
}