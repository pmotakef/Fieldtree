using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace fieldtree
{
    class HelperFuncs
    {
        /// <summary>
        /// Some very long helper functions which holds the relations between children, parent, and sibling positions
        /// </summary>
        public static int LookupChildCover(Point p, CoverNode parent_node)
        {
            int childNum = -1;
            if (p.X < parent_node.GetBounds().rect_center.X)
            {
                if (p.Y < parent_node.GetBounds().rect_center.Y)
                    childNum = 0;
                else
                    childNum = 2;
            }
            else
            {
                if (p.Y < parent_node.GetBounds().rect_center.Y)
                    childNum = 1;
                else
                    childNum = 3;
            }
            return (childNum);
        }


        public static int LookupChildPartition(Point p, PartitionNode parent_node)
        {
            int offset = parent_node.getNodeSize() / 4;
            int x1 = parent_node.getCenter().X - offset;
            int x2 = parent_node.getCenter().X + offset;
            int y1 = parent_node.getCenter().Y - offset;
            int y2 = parent_node.getCenter().Y + offset;

            int childNum = -1;
            if (p.X < x1)
            {
                if (p.Y < y1)
                    childNum = 0;
                else if (p.Y < y2)
                    childNum = 3;
                else
                    childNum = 6;
            }
            else if (p.X < x2)
            {
                if (p.Y < y1)
                    childNum = 1;
                else if (p.Y < y2)
                    childNum = 4;
                else
                    childNum = 7;
            }
            else
            {
                if (p.Y < y1)
                    childNum = 2;
                else if (p.Y < y2)
                    childNum = 5;
                else
                    childNum = 8;
            }
            return (childNum);
        }

    }

    public static class NodeHelperFuncs
    {


        public static Point GetChildCenter(PartitionNode parent, int child_num)
        {
            int children_size = parent.getNodeSize() / 2;
            int x1 = parent.getCenter().X - children_size;
            int x2 = x1 + children_size;
            int x3 = x2 + children_size;
            int y1 = parent.getCenter().Y - children_size;
            int y2 = y1 + children_size;
            int y3 = y2 + children_size;

            switch (child_num)
            {
                case 0:
                    return new Point(x1, y1);
                case 1:
                    return new Point(x2, y1);
                case 2:
                    return new Point(x3, y1);
                case 3:
                    return new Point(x1, y2);
                case 4:
                    return new Point(x2, y2);
                case 5:
                    return new Point(x3, y2);
                case 6:
                    return new Point(x1, y3);
                case 7:
                    return new Point(x2, y3);
                case 8:
                    return new Point(x3, y3);
                default:
                    return new Point(0, 0);
            }
        }

        public static bool canUpdateChild(Dictionary<int, PartitionNode> children, int childKey, Dictionary<int, PartitionNode> siblings, int sibKey)
        {
            return (!children.ContainsKey(childKey) && siblings.ContainsKey(sibKey) && siblings[sibKey] != null);
        }

        public static int GetParentPosition(int childnum)
        {
            switch (childnum)
            {
                case 0:
                    return 4;
                case 1:
                    return 2;
                case 2:
                    return 3;
                case 3:
                    return 2;
                case 4:
                    return 1;
                case 5:
                    return 1;
                case 6:
                    return 2;
                case 7:
                    return 1;
                case 8:
                    return 1;
                default:
                    return 0;
            }
        }

        public static int getSharedSiblingChildNumber (int child, int sibling)
        {
            int answer = -1;

            switch (child)
            {
                case 0:
                    if (sibling == 0)
                        return 8;
                    else if (sibling == 1)
                        return 6;
                    else if (sibling == 3)
                        return 2;
                    break;
                case 1:
                    if (sibling == 1)
                        return 7;
                    break;
                case 2:
                    if (sibling == 1)
                        return 8;
                    else if (sibling == 2)
                        return 6;
                    else if (sibling == 5)
                        return 0;
                    break;
                case 3:
                    if (sibling == 3)
                        return 5;
                    break;
                case 5:
                    if (sibling == 5)
                        return 3;
                    break;
                case 6:
                    if (sibling == 3)
                        return 8;
                    else if (sibling == 6)
                        return 2;
                    else if (sibling == 7)
                        return 0;
                    break;
                case 7:
                    if (sibling == 7)
                        return 1;
                    break;
                case 8:
                    if (sibling == 5)
                        return 6;
                    else if (sibling == 7)
                        return 2;
                    else if (sibling == 8)
                        return 0;
                    break;
                default:
                    break;
            }
            return answer;
        }

        public static List<int> getRelevantSiblings (int child)
        {
            switch (child)
            {
                case 0:
                    return new List<int> { 0, 1, 3 };
                case 1:
                    return new List<int> { 1 };
                case 2:
                    return new List<int> { 1, 2, 5 };
                case 3:
                    return new List<int> { 3 };
                case 5:
                    return new List<int> { 5 };
                case 6:
                    return new List<int> { 3, 6, 7 };
                case 7:
                    return new List<int> { 7 };
                case 8:
                    return new List<int> { 5, 7, 8 };
            }
            return new List<int>();
        }
        
        public static List<int> getChildSiblings (int child)
        {
            switch (child)
            {
                case 0:
                    return new List<int> { 1, 3, 4 };
                case 1:
                    return new List<int> { 0, 2, 3, 4, 5 };
                case 2:
                    return new List<int> { 1, 4, 5 };
                case 3:
                    return new List<int> { 0, 1, 4, 6, 7 };
                case 4:
                		return new List<int> { 0, 1, 2, 3, 5, 6, 7, 8 };
                case 5:
                    return new List<int> { 1, 2, 4, 7, 8 };
                case 6:
                    return new List<int> { 3, 4, 7 };
                case 7:
                    return new List<int> { 3, 4, 5, 6, 8 };
                case 8:
                    return new List<int> { 4, 5, 7 };
            }
            return new List<int>();
        }
        
        // This magic formula is used for computing the sibling position number of the current node, for a given parent this node is a child of.
        public static int ComputeSiblingShiftForChildNum (int child)
        {
        	// Magic formula
        	return (4 - child);
        }

        
    }

}
