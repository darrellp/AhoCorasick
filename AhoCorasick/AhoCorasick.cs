using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AhoCorasick
{
    public abstract class AhoCorasick<T, R>
    {
        #region Non-public variables
        protected AhoCorasick<T, R>[] NextNodes;
        private AhoCorasick<T, R> _fail;
        protected AhoCorasick<T, R> Parent;
        protected T Val;
        private readonly List<R> _completed = new List<R>();
        private readonly List<int> _depth = new List<int>();
        #endregion

        #region Properties
        protected int ChildrenCount => NextNodes.Length;
        private bool IsCompleted => _completed.Count != 0;
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
                n = n._fail;
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
                if (curNode.IsCompleted)
                {
                    for(var iCompleted = 0; iCompleted < curNode._completed.Count; iCompleted++)
                    {
                        var completed = curNode._completed[iCompleted];
                        var depth = curNode._depth[iCompleted];
                        yield return (iPos - depth, completed);
                    }
                }
            }
        }

        private void Install(List<T> vals, int index, R completed)
        {
            if (index == vals.Count)
            {
                if (!_completed.Contains(completed))
                {
                    _completed.Add(completed);
                    _depth.Add(vals.Count - 1);
                }
                return;
            }
            var indexCur = Index(vals[index]);
            var next = NextNodes[indexCur];
            if (next == null)
            {
                next = NextNodes[indexCur] = FactoryCreate();
                next.Parent = this;
                next.Val = vals[index];
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
            for (var i = 0; i < ChildrenCount; i++)
            {
                if (NextNodes[i] == null)
                {
                    NextNodes[i] = this;
                }
            }
            SetFails();
        }

        private void SetFails()
        {
            var q = new Queue<AhoCorasick<T, R>>();
            for (var iAlpha = 0; iAlpha < ChildrenCount; iAlpha++)
            {
                var nextNode = NextNodes[iAlpha];
                if (nextNode != this)
                {
                    nextNode._fail = this;
                    q.Enqueue(nextNode);
                }
            }
            while (q.Count != 0)
            {
                var r = q.Dequeue();
                for (var a = 0; a < NextNodes.Length; a++)
                {
                    var u = r.NextNodes[a];
                    if (u != null)
                    {
                        q.Enqueue(u);
                        var v = r._fail;
                        while (v.NextNodes[a] == null)
                        {
                            v = v._fail;
                        }
                        u._fail = v.NextNodes[a];
                        u._completed.AddRange(u._fail._completed);
                        u._depth.AddRange(u._fail._depth);
                    }
                }
            }
        }

        protected virtual void CreateChildren(int childrenCount)
        {
            NextNodes = new AhoCorasick<T, R>[childrenCount];
        }
    }
}
