using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Priority_Queue;

namespace fieldtree
{
    /// <summary>
    /// Cover Fieldtree node which extends from the FieldNode (Abstract class) and implements generic IFieldNode interface.
    /// </summary>
    class CoverNode : FieldNode, IFieldNode<CoverNode>
    {
        private CoverNode parent;
         /*      Childern Positions
         *      ---------------------------------
         *      |       0       |       1       |
         *      ---------------------------------
         *      |       2       |       3       |
         *      ---------------------------------
         */
        private Dictionary<int, CoverNode> children;
        private double pVal;

        private RectangleObj original_bounds;

        public CoverNode() : this(10, new Point(5, 5), 2, null, 1.0) { }

        public CoverNode(int size, Point center_coord, int cap, CoverNode node_parent, double p_value)
        {
            children = new Dictionary<int, CoverNode>();
            bounds = new RectangleObj();
            original_bounds = new RectangleObj();
            capacity = cap;
            parent = node_parent;
            double expanded_size = (1.0 + p_value) * size;
            original_bounds.SetRectangleByCenter(center_coord, size, size);
            bounds.SetRectangleByCenter(center_coord, (int)expanded_size, (int)expanded_size);
            pVal = p_value;

            if (node_parent == null)
                layerNum = 0;
            else
                layerNum = node_parent.layerNum + 1;
        }

        public bool IsRootNode()
        {
            return (parent == null);
        }

        public Dictionary<int, CoverNode> getChildern()
        {
            return children;
        }

        public CoverNode getSingleChild(int child)
        {
            if (children.Count < 1)
                return null;
            return children[child];
        }

        public bool HasChildren()
        {
            return (children.Count > 1);
        }

        public override void CreateChildren()
        {
            // Do nothing, if children are already created
            if (children.Count > 1)
                return;

            int children_layer = layerNum + 1;
            int children_size = original_bounds.rect_width / 2;

            int x1 = bounds.rect_center.X - (children_size / 2);
            int x2 = bounds.rect_center.X + (children_size / 2);
            int y1 = bounds.rect_center.Y - (children_size / 2);
            int y2 = bounds.rect_center.Y + (children_size / 2);

            children.Add(0, new CoverNode(children_size, new Point(x1, y1), capacity, this, pVal));
            children.Add(1, new CoverNode(children_size, new Point(x2, y1), capacity, this, pVal));
            children.Add(2, new CoverNode(children_size, new Point(x1, y2), capacity, this, pVal));
            children.Add(3, new CoverNode(children_size, new Point(x2, y2), capacity, this, pVal));
        }

        public CoverNode GetParent()
        {
            return parent;
        }

        public bool AreExistingChildrenEmpty()
        {
            if (children.Count < 1)
            {
                return false;
            }
            foreach (KeyValuePair<int, CoverNode> child in children)
            {
                if (child.Value.HasChildren() || !child.Value.IsEmpty())
                {
                    return false;
                }
            }
            return true;
        }

        public void MergeEmptyChildren()
        {
            if (children.Count < 1)
            {
                return;
            }
            children = new Dictionary<int, CoverNode>();
        }

        public CoverNode LookupChild(Point p)
        {
            int childnum = HelperFuncs.LookupChildCover(p, this);
            return getSingleChild(childnum);
        }

        public Dictionary<int, CoverNode> GetParents()
        {
            Dictionary<int, CoverNode> node_parents = new Dictionary<int, CoverNode>();
            node_parents.Add(0, parent);
            return node_parents;
        }

        public bool HasAllChildren()
        {
            return (children.Count == 4);
        }

        public Dictionary<int, CoverNode> GetAllSiblings()
        {
            return new Dictionary<int, CoverNode>();
        }
    }
}
