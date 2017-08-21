using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AhoCorasick
{
    public abstract class AhoCorasick<T, R>
    {
        #region Non-public variables
        protected AhoCorasick<T, R>[] NextNodes;
        private AhoCorasick<T, R> _fall;
        protected AhoCorasick<T, R> Parent;
        protected T Val;
        protected internal List<T> Completed;
        private R _completed;
        private bool _isCompleted;
        private int _depth = 0;
        #endregion

        #region Properties
        protected int ChildrenCount => NextNodes.Length;
        #endregion

        #region abstract methods
        protected abstract AhoCorasick<T, R> FactoryCreate();
        protected abstract int Index(T val);
        #endregion

        protected internal AhoCorasick<T, R> Next(T val, AhoCorasick<T, R> rootNode)
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

        public IEnumerable<(int Position, R Result)> LocateParts(List<T> vals)
        {
            var curNode = this;
            for (var iPos = 0; iPos < vals.Count; iPos++)
            {
                var val = vals[iPos];
                curNode = curNode.Next(val, this);
                Debug.Assert(curNode != null, "curNode != null");
                if (curNode._isCompleted)
                {
                    yield return (iPos - curNode._depth, curNode._completed);
                }
            }
        }

        private void Install(List<T> vals, int index, R completed)
        {
            if (index == vals.Count)
            {
                _completed = completed;
                _isCompleted = true;
                _depth = vals.Count - 1;
                return;
            }
            var indexCur = Index(vals[index]);
            AhoCorasick<T, R> next = NextNodes[indexCur];
            if (next == null)
            {
                next = NextNodes[indexCur] = FactoryCreate();
                next.Parent = this;
                next.Val = vals[index];
                // TODO: I think we're setting these on leaf nodes where they shouldn't be necessary
                next.CreateChildren(ChildrenCount);
            }
            next.Install(vals, index + 1, completed);
        }

        protected void Install(List<List<T>> vals, List<R> completed)
        {
            if (vals.Count != completed.Count)
            {
                throw new ArgumentException("The two arguments in Install must be the same length");
            }
            for (var i = 0; i < vals.Count; i++)
            {
                Install(vals[i], 0, completed[i]);
            }
            SetFalls();
        }

        void SetFalls()
        {
            _fall = this;
            var q = new Queue<AhoCorasick<T, R>>();
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

        protected virtual void CreateChildren(int childrenCount)
        {
            NextNodes = new AhoCorasick<T, R>[childrenCount];
        }
    }
}
