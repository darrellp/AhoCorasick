using System.Collections.Generic;
using System.Linq;

namespace AhoCorasick
{
    public class AhoCorasickString : AhoCorasick<char>
    {
        #region Creation
        public static AhoCorasickString Create(List<string> words)
        {
            var root = new AhoCorasickString();
            root.Install(words.Select(w => w.ToCharArray().ToList()).ToList());
            return root;
        }
        #endregion

        #region Accessing
        public IEnumerable<string> LocatePart(string search)
        {
            var curNode = (AhoCorasick<char>)this;
            foreach (var val in search.ToCharArray())
            {
                curNode = curNode.Next(val, this);
                if (curNode.Completed != null)
                {
                    yield return new string(curNode.Completed.ToArray());
                }
            }
        }
        #endregion

        #region Overrides
        protected override void CreateChildren()
        {
            // ReSharper disable once CoVariantArrayConversion
            NextNodes = new AhoCorasickString[52];
        }

        protected override AhoCorasick<char> FactoryCreate()
        {
            var ret = new AhoCorasickString();
            ret.CreateChildren();
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

        public override string ToString()
        {
            if (Parent == null)
            {
                return "";
            }
            return Parent.ToString() + Val;
        }
        #endregion
    }
}