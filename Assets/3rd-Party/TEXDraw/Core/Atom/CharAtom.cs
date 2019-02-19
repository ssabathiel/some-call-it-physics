

// Atom representing single character in specific text style.
namespace TexDrawLib
{
	public class CharAtom : CharSymbol
	{
 
		public static CharAtom Get (char character, int fontIndex)
		{
            var atom = ObjPool<CharAtom>.Get();
			atom.Character = character;          
            atom.FontIndex = fontIndex == -1 ? TEXPreference.main.GetTypefaceFor(character) : fontIndex;
            return atom;
		}

		//public static CharAtom Get (char character)
		//{
  //          return Get(character, TEXPreference.main.GetTypefaceFor(character));
		//}

        public char Character;

        public int FontIndex;
        
		public override Box CreateBox ()
		{
	//		var pref = TEXPreference.main;

	
            return CharBox.Get(FontIndex, Character);

			//if (font >= 0 && !pref.IsCharAvailable(font, Character))
			//{
			//	// It's unicode, do return Unicode
			//	CharacterInfo info;

			//	var c =	pref.fonts[font].CreateCharacterDataOnTheFly(Character, TexContext.Scale, out info);
			//	return UnicodeBox.Get(c, font, info);
			//} else {
			//	if (font == -1) {
			//		//var chSymbol = pref.charMapData[Character, -1];
			//		//if (chSymbol == -1)
			//			return CharBox.Get (style, pref.GetCharMetric (Character, style), FStyle);
			//	}
			//	else
			//		return CharBox.Get (style, pref.GetCharMetric (font, Character, style), FStyle);
			//}
		}

        //public TexCharMetric GetChar ()
        //{
        //	if (FontIndex == -1)
        //		return TEXPreference.main.GetCharMetric (Character, style);
        //	if (FontIndex == -2) {
        //		if (TexUtility.RenderFont == -1)
        //			return TEXPreference.main.GetCharMetric (Character, style);
        //		else
        //			return TEXPreference.main.GetCharMetric (TexUtility.RenderFont, Character, style);
        //	}
        //	else
        //		return TEXPreference.main.GetCharMetric (FontIndex, Character, style);
        //}

        public override TexChar GetChar()
        {
            return TEXPreference.main.GetChar(FontIndex, Character);
        }

        public override void Flush()
        {
            Character = default(char);
            FontIndex = -1;
            ObjPool<CharAtom>.Release(this);
        }


	}
}