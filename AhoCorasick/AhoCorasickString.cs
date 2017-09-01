using System.Collections.Generic;
using System.Linq;

namespace AhoCorasick
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An aho-corasick string searcher. </summary>
    ///
    /// <remarks>   This only works for alphabetic characters.  All other characters get lumped into
    ///             one "non-alphabetic character" class so "darrell@" and "darrell1" will appear
    ///             identical in this searcher.  Darrell Plank, 8/20/2017. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class AhoCorasickString : AhoCorasick<char, string>
    {
        #region Creation
        public static AhoCorasickString Create(List<string> words)
        {
            // We MUST give the root node the proper number of children.  All descendants
            // will then have this same count.
            var root = new AhoCorasickString {NextNodes = new AhoCorasick<char, string>[53]};
            root.Install(words.Select(w => w.ToCharArray().ToList()).ToList(), words);
            return root;
        }
        #endregion

        #region Accessing
        //public IEnumerable<AcResult<string>> LocateParts(string searched, bool fSorted = false)
        //{
        //    return base.LocateParts(searched, fSorted);
        //}
        #endregion

        #region Overrides
        protected override AhoCorasick<char, string> FactoryCreate()
        {
            var ret = new AhoCorasickString();
            return ret;
        }

        protected override int Index(char ch)
        {
            if (!char.IsLetter(ch))
            {
                return 52;
            }
            if (ch >= 'a')
            {
                return ch - 'a';
            }
            return ch - 'A' + 26;
        }

        public override string ToString()
        {
            if (Parent == null)
            {
                return string.Empty;
            }
            return Parent.ToString() + Val;
        }
        #endregion
    }
}