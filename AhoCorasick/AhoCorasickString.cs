using System.Collections.Generic;
using System.Linq;

namespace AhoCorasick
{
    public class AhoCorasickString : AhoCorasick<char, string>
    {
        #region Creation
        public static AhoCorasickString Create(List<string> words)
        {
            // We MUST give the root node the proper number of children.  All descendants
            // will then have this same count.
            var root = new AhoCorasickString {NextNodes = new AhoCorasick<char, string>[52]};
            root.Install(words.Select(w => w.ToCharArray().ToList()).ToList(), words);
            return root;
        }
        #endregion

        #region Accessing
        public IEnumerable<string> LocateParts(string searched)
        {
            foreach (var s in base.LocateParts(searched.ToCharArray().ToList()))
            {
                yield return s;
            }
        }
        #endregion

        #region Overrides
        protected override AhoCorasick<char, string> FactoryCreate()
        {
            var ret = new AhoCorasickString();
            return ret;
        }

        protected override int Index(char ch)
        {
            if (ch >= 'a')
            {
                return ch - 'a';
            }
            return ch - 'A' + 26;
        }
        #endregion
    }
}