using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace fieldtree
{
    /// <summary>
    /// Interface for each Cover and Partition Fieldtree node need to implement
    /// </summary>
    public interface IFieldNode<T>
    {
        bool IsPointInNode(Point p);
        bool IsOverflown();
        List<RectangleObj> GetOverflowObjs();
        Point getCenter();
        int getNodeSize();
        int GetLayerlevel();
        bool IsFull();
        bool IsEmpty();
        int Count();
        List<RectangleObj> GetAllStoredObjs();
        void DeleteRectangle(RectangleObj rect, bool overflow_only = false);
        void DeleteRectangles(List<RectangleObj> rects, bool overflow_only = false);
        bool StoreRectangle(RectangleObj rect);
        bool RemoveRectangle(RectangleObj rect);
        List<RectangleObj> GetRangeQueryObj(Point c, int r);
        List<RectangleObj> GetRangeQueryObj(RectangleObj window);
        Dictionary<RectangleObj, double> GetNearestRectangle(Point p);
        void CreateChildren();
        RectangleObj GetBounds();
        T getSingleChild(int child);
        T LookupChild(Point p);
        bool HasChildren();
        Dictionary<int, T> getChildern();
        bool IsRootNode();
        bool AreExistingChildrenEmpty();
        void MergeEmptyChildren();
        Dictionary<int, T> GetParents();
        bool HasAllChildren();
    }

    public struct ObjAndNode<T> where T : IFieldNode<T>
    {
        public RectangleObj rect;
        public T node;
    }

    /// <summary>
    /// An Abstract class for FieldNode which implements all routines that are shared between different node types.
    /// </summary>
    public abstract class FieldNode : IComparable<FieldNode>, IEquatable<FieldNode>
    {
        public int layerNum;
        public int capacity;
        public RectangleObj bounds;
        public List<RectangleObj> storedObjs;
        public List<RectangleObj> overflowObjs;

        public int CompareTo(FieldNode other)
        {
            if (layerNum < other.layerNum)
                return -1;
            if (layerNum < other.layerNum)
                return 1;
            return bounds.CompareTo(other.bounds);
        }

        public bool Equals(FieldNode other)
        {
            return (layerNum == other.layerNum && bounds.Equals(other.bounds));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals(obj as FieldNode);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = bounds.GetHashCode();
                hash = hash * 23 + layerNum.GetHashCode();
                return hash;
            }
        }

        public FieldNode()
        {
            storedObjs = new List<RectangleObj>();
            overflowObjs = new List<RectangleObj>();
        }

        public bool IsPointInNode(Point p)
        {
            return bounds.ContainsPoint(p);
        }

        public bool IsOverflown()
        {
            return (overflowObjs.Count > 0);
        }

        public List<RectangleObj> GetOverflowObjs()
        {
            return overflowObjs;
        }

        public Point getCenter()
        {
            return bounds.rect_center;
        }

        public int getNodeSize()
        {
            return bounds.rect_width;
        }

        public int GetLayerlevel()
        {
            return layerNum;
        }

        public void ClearOverFlow()
        {
            overflowObjs.Clear();
        }

        public bool IsFull()
        {
            return (storedObjs.Count + overflowObjs.Count >= capacity);
        }

        public bool IsEmpty()
        {
            return (storedObjs.Count + overflowObjs.Count == 0);
        }

        public int Count()
        {
            return (storedObjs.Count + overflowObjs.Count);
        }

        public List<RectangleObj> GetAllStoredObjs()
        {
            List<RectangleObj> objs = new List<RectangleObj>(storedObjs);
            objs.AddRange(overflowObjs);
            return objs;
        }

        public void DeleteRectangle(RectangleObj rect, bool overflow_only = false)
        {
            if (!overflow_only)
            {
                for (int i = 0; i < storedObjs.Count; i++)
                {
                    if (rect.IsEqual(storedObjs[i]))
                    {
                        storedObjs.RemoveAt(i);
                        return;
                    }
                }
            }
            for (int i = 0; i < overflowObjs.Count; i++)
            {
                if (rect.IsEqual(overflowObjs[i]))
                {
                    overflowObjs.RemoveAt(i);
                    return;
                }
            }
        }

        public void DeleteRectangles(List<RectangleObj> rects, bool overflow_only = false)
        {
            foreach (RectangleObj rect in rects)
            {
                DeleteRectangle(rect, overflow_only);
            }
        }

        public bool StoreRectangle(RectangleObj rect)
        {
            // Only potential Objs then can sink further go into overflow
            bool overflow = Math.Max(rect.rect_width, rect.rect_height) <= getNodeSize() / 2 && rect.importance_layer > layerNum;
            if (overflow)
            {
                overflowObjs.Add(rect);
            }
            else
            {
                storedObjs.Add(rect);
            }
            return (IsOverflown());
        }

        public bool RemoveRectangle(RectangleObj rect)
        {
            DeleteRectangle(rect, false);
            return (IsOverflown());
        }

        public List<RectangleObj> GetRangeQueryObj(Point c, int r)
        {
            List<RectangleObj> objs = new List<RectangleObj>();
            foreach (RectangleObj rect in storedObjs)
            {
                if (rect.ContainedByCircle(c, r))
                {
                    objs.Add(rect);
                }
            }
            foreach (RectangleObj rect in overflowObjs)
            {
                if (rect.ContainedByCircle(c, r))
                {
                    objs.Add(rect);
                }
            }
            return objs;
        }

        public List<RectangleObj> GetRangeQueryObj(RectangleObj window)
        {
            List<RectangleObj> objs = new List<RectangleObj>();
            foreach (RectangleObj rect in storedObjs)
            {
                if (rect.ContainedByArea(window.rect_center, window.rect_width, window.rect_height))
                {
                    objs.Add(rect);
                }
            }
            foreach (RectangleObj rect in overflowObjs)
            {
                if (rect.ContainedByArea(window.rect_center, window.rect_width, window.rect_height))
                {
                    objs.Add(rect);
                }
            }
            return objs;
        }

        public Dictionary<RectangleObj, double> GetNearestRectangle(Point p)
        {
            Dictionary<RectangleObj, double> answer = new Dictionary<RectangleObj, double>();

            double min_dist = -1.0;
            RectangleObj nearest_rect = null;

            foreach (RectangleObj rect in storedObjs)
            {
                double dist = rect.GetDistanceSqToPoint(p);
                if (dist <= min_dist || nearest_rect == null)
                {
                    nearest_rect = rect;
                    min_dist = dist;
                    if (min_dist < 0.01)
                    {
                        answer.Add(nearest_rect, min_dist);
                    }
                }
            }
            foreach (RectangleObj rect in overflowObjs)
            {
                double dist = rect.GetDistanceSqToPoint(p);
                if (dist <= min_dist || nearest_rect == null)
                {
                    nearest_rect = rect;
                    min_dist = dist;
                    if (min_dist < 0.01)
                    {
                        answer.Add(nearest_rect, min_dist);
                    }
                }
            }

            if (nearest_rect != null && answer.Count < 1)
            {
                answer.Add(nearest_rect, min_dist);
            }
            return answer;
        }

        public RectangleObj GetBounds()
        {
            return bounds;
        }

        public abstract void CreateChildren();

    }
}
