using System.Collections.Generic;

namespace AhoCorasick
{
    public abstract class AhoCorasick<T>
    {
        #region Non-public variables

        protected AhoCorasick<T>[] NextNodes;
        private AhoCorasick<T> _fall;
        protected AhoCorasick<T> Parent;
        protected T Val;
        protected internal List<T> Completed;
        #endregion

        #region abstract methods
        protected abstract void CreateChildren();
        protected abstract AhoCorasick<T> FactoryCreate();
        protected abstract int Index(T val);
        #endregion

        #region Initialization/Setup
        protected void Install(List<List<T>> vals)
        {
            CreateChildren();
            foreach (var val in vals)
            {
                Install(val, 0);
            }
            SetFalls();
        }

        private void Install(List<T> vals, int index)
        {
            if (index == vals.Count)
            {
                Completed = vals;
                return;
            }
            var indexCur = Index(vals[index]);
            AhoCorasick<T> next = NextNodes[indexCur];
            if (next == null)
            {
                next = NextNodes[indexCur] = FactoryCreate();
                next.Parent = this;
                next.Val = vals[index];
            }
            next.Install(vals, index + 1);
        }

        void SetFalls()
        {
            _fall = this;
            var q = new Queue<AhoCorasick<T>>();
            q.Enqueue(this);
            while (q.Count != 0)
            {
                var n = q.Dequeue();
                var index = Index(n.Val);
                for (var i = 0; i < NextNodes.Length; i++)
                {
                    var no = n.NextNodes[i];
                    if (no != null)
                    {
                        q.Enqueue(no);
                    }
                }
                if (n == this)
                {
                    continue;
                }
                var fall = n.Parent._fall;
                while (fall.NextNodes[index] == null && fall != this)
                {
                    fall = fall._fall;
                }
                n._fall = fall.NextNodes[index];
                if (n._fall == null || n._fall == n)
                {
                    n._fall = this;
                }
            }
        }
        #endregion

        #region Accessing
        protected internal AhoCorasick<T> Next(T val, AhoCorasick<T> rootNode)
        {
            var n = this;
            var index = Index(val);
            while (n.NextNodes[index] == null && n != rootNode)
            {
                n = n._fall;
            }

            if (n == rootNode)
            {
                n = n.NextNodes[index] ?? rootNode;
            }
            else
            {
                n = n.NextNodes[index];
            }

            return n;
        }

        // TODO: generalize so we can employ this function in AhoCorasickString and other subclasses
        public IEnumerable<List<T>> LocatePart(List<T> vals)
        {
            var curNode = this;
            foreach (T t in vals)
            {
                curNode = curNode.Next(t, this);
                if (curNode.Completed != null)
                {
                    yield return Completed;
                }
            }
        }
        #endregion
    }
}
