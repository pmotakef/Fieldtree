using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace fieldtree
{
    /// <summary>
    /// Holds the extents (bounds) for a given layer of nodes
    /// </summary>
    public struct LayerBounds
    {
        public Rectangle bounds;
        public int layerNum;
    }

    /// <summary>
    /// A Rectangle Object used as the type of data being saved in the data structure.
    /// Actual shapes can use this RectangleObj as bounding box for storage.
    /// </summary>
    public class RectangleObj : IComparable<RectangleObj>, IEquatable<RectangleObj>
    {
        public Point rect_center;
        public int rect_height;
        public int rect_width;

        public int max_extent_X;
        public int max_extent_Y;
        public int min_extent_X;
        public int min_extent_Y;

        public int importance_layer; // -1 means any layer

        public Color rect_color;

        public RectangleObj()
        {
            rect_color = Color.Black;
        }

        public RectangleObj(RectangleObj rect)
        {
            rect_color = rect.rect_color;
            SetRectangleByCenter(rect.rect_center, rect.rect_width, rect.rect_height);
        }

        public void SetRectangleByCenter(Point center, int width, int height)
        {
            rect_center = center;
            rect_width = width;
            rect_height = height;

            max_extent_X = center.X + (width / 2);
            min_extent_X = center.X - (width / 2);
            max_extent_Y = center.Y + (height / 2);
            min_extent_Y = center.Y - (height / 2);
            importance_layer = -1;
        }

        public void SetRectangleByCoords(Point top_left, Point bottom_right)
        {
            rect_center = new Point((top_left.X + bottom_right.X) / 2, (top_left.Y + bottom_right.Y) / 2);
            rect_width = Math.Abs(top_left.X - bottom_right.X);
            rect_height = Math.Abs(top_left.Y - bottom_right.Y);

            max_extent_X = Math.Max(top_left.X, bottom_right.X);
            max_extent_Y = Math.Max(top_left.Y, bottom_right.Y);
            min_extent_X = Math.Min(top_left.X, bottom_right.X);
            min_extent_Y = Math.Min(top_left.Y, bottom_right.Y);
        }

        public void UpdateWH(int width, int height)
        {
            if (width != rect_width)
            {
                min_extent_X = rect_center.X - (width / 2);
                max_extent_X = rect_center.X + (width / 2);
                rect_width = width;
            }
            if (height != rect_height)
            {
                min_extent_Y = rect_center.Y - (height / 2);
                max_extent_Y = rect_center.Y + (height / 2);
                rect_height = height;
            }
        }

        public void MoveCenter(Point dp)
        {
            rect_center = dp;
            min_extent_X = rect_center.X - (rect_width / 2);
            max_extent_X = rect_center.X + (rect_width / 2);
            min_extent_Y = rect_center.Y - (rect_height / 2);
            max_extent_Y = rect_center.Y + (rect_height / 2);
        }

        public bool ContainsRect(RectangleObj other)
        {
            Rectangle this_rect = new Rectangle(min_extent_X, min_extent_Y, rect_width, rect_height);
            Rectangle other_rect = new Rectangle(other.min_extent_X, other.min_extent_Y, other.rect_width, other.rect_height);
            return this_rect.Contains(other_rect);
        }

        public bool ContainsRect(int minX, int maxX, int minY, int maxY)
        {
            RectangleObj other = new RectangleObj();
            other.SetRectangleByCoords(new Point(minX, minY), new Point(maxX, maxY));
            return ContainsRect(other);
        }

        public bool ContainedByArea(RectangleObj other)
        {
            Rectangle this_rect = new Rectangle(min_extent_X, min_extent_Y, rect_width, rect_height);
            Rectangle other_rect = new Rectangle(other.min_extent_X, other.min_extent_Y, other.rect_width, other.rect_height);
            return (other_rect.Contains(this_rect));
        }


        public bool ContainedByArea(int minX, int maxX, int minY, int maxY)
        {
            RectangleObj other = new RectangleObj();
            other.SetRectangleByCoords(new Point(minX, minY), new Point(maxX, maxY));
            return ContainedByArea(other);
        }

        public bool ContainsPoint(Point p)
        {
            return (p.X <= max_extent_X && p.X >= min_extent_X && p.Y <= max_extent_Y && p.Y >= min_extent_Y);
        }

        public bool ContainedByArea(Point center, int width, int height)
        {
            RectangleObj other = new RectangleObj();
            other.SetRectangleByCenter(center, width, height);
            return ContainedByArea(other);
        }

        public bool IsEqual(RectangleObj rect)
        {
            return (rect.rect_center.X == rect_center.X && rect.rect_center.Y == rect_center.Y && rect.rect_width == rect_width && rect.rect_height == rect_height);
        }

        public double GetDistanceSqToPoint(Point p)
        {
            if (p.X <= max_extent_X && p.X >= min_extent_X && p.Y >= min_extent_Y && p.Y <= max_extent_Y)
            {
                return 0.0;
            }
            List<double> distances = new List<double>();
            distances.Add(CalcDistSq(p, new Point(min_extent_X, min_extent_Y)));
            distances.Add(CalcDistSq(p, new Point(max_extent_X, min_extent_Y)));
            distances.Add(CalcDistSq(p, new Point(min_extent_X, max_extent_Y)));
            distances.Add(CalcDistSq(p, new Point(max_extent_X, max_extent_Y)));
            if (p.Y <= max_extent_Y && p.Y >= min_extent_Y)
            {
                distances.Add(CalcDistSq(p, new Point(min_extent_X, p.Y)));
                distances.Add(CalcDistSq(p, new Point(max_extent_X, p.Y)));
            }
            if (p.X <= max_extent_X && p.X >= min_extent_X)
            {
                distances.Add(CalcDistSq(p, new Point(p.X, min_extent_Y)));
                distances.Add(CalcDistSq(p, new Point(p.X, max_extent_Y)));
            }
            return distances.Min();
        }

        public static double CalcDistSq(Point p1, Point p2)
        {
            return ((p1.X - p2.X) * (p1.X - p2.X)) + ((p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        public int CompareTo(RectangleObj other)
        {
            if (rect_center.X < other.rect_center.X)
                return -1;
            if (rect_center.X > other.rect_center.X)
                return 1;
            if (rect_center.Y < other.rect_center.Y)
                return -1;
            if (rect_center.Y > other.rect_center.Y)
                return 1;
            if (rect_width < other.rect_width)
                return -1;
            if (rect_width > other.rect_width)
                return 1;
            if (rect_height < other.rect_height)
                return -1;
            if (rect_height > other.rect_height)
                return 1;
            return 0;
        }

        public bool Equals(RectangleObj other)
        {
            return (rect_center.X == other.rect_center.X && rect_center.Y == other.rect_center.Y && rect_width == other.rect_width && rect_height == other.rect_height);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals(obj as RectangleObj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + rect_center.X.GetHashCode();
                hash = hash * 23 + rect_center.Y.GetHashCode();
                hash = hash * 23 + rect_width.GetHashCode();
                hash = hash * 23 + rect_height.GetHashCode();
                return hash;
            }
        }

        public bool ContainedByCircle(Point center, int radius)
        {
            List<double> dists = new List<double>();
            dists.Add(CalcDistSq(center, new Point(min_extent_X, min_extent_Y)));
            dists.Add(CalcDistSq(center, new Point(max_extent_X, min_extent_Y)));
            dists.Add(CalcDistSq(center, new Point(min_extent_X, max_extent_Y)));
            dists.Add(CalcDistSq(center, new Point(max_extent_X, max_extent_Y)));
            return ((int)dists.Max() <= radius * radius);
        }

        public bool IntersectsCircle(Point center, int radius)
        {
            double dist = GetDistanceSqToPoint(center);
            return ((int)dist <= radius * radius);
        }

        public bool IntersectsWith(RectangleObj other)
        {
            Rectangle this_rect = new Rectangle(min_extent_X, min_extent_Y, rect_width, rect_height);
            Rectangle other_rect = new Rectangle(other.min_extent_X, other.min_extent_Y, other.rect_width, other.rect_height);
            return (this_rect.IntersectsWith(other_rect));
        }
    }



}
