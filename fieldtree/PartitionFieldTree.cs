using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Priority_Queue;

namespace fieldtree
{
    /// <summary>
    /// Partition Fieldtree node which extends from the FieldNode (Abstract class) and implements generic IFieldNode interface.
    /// </summary>
    public class PartitionNode : FieldNode, IFieldNode<PartitionNode>
    {
        /*      Parent Positions
         *      |                      |        Parent1       |         Parent2      |       Parent3       |       Parent4      |
         *      -----------------------------------------------------------------------------------------------------------------
         *      | Number of Parent     |          1           |           2          |         3           |        4           |
         *      -----------------------------------------------------------------------------------------------------------------
         *      |       1              |        Center        |             -        |          -          |        -           |
         *      -----------------------------------------------------------------------------------------------------------------
         *      |       2              |          Top/Left    |        Bottom/Right  |          -          |        -           |
         *      -----------------------------------------------------------------------------------------------------------------
         *      |       4              |      Top-left        |       Top-right      |      Bottom-left    |    Bottom-right    |
         *      -----------------------------------------------------------------------------------------------------------------
         */
        private Dictionary<int, PartitionNode> Parents;
        /*
         *      Childern Positions
         *      -------------------------------------------------
         *      |       0       |       1       |       2       |
         *      -------------------------------------------------
         *      |       3       |       4       |       5       |
         *      -------------------------------------------------
         *      |       6       |       7       |       8       |
         *      -------------------------------------------------
         */
        private Dictionary<int, PartitionNode> Children;

        public PartitionNode() : this(0, 10, new Point(5, 5), 2) {  }

        public PartitionNode(int node_layer, int size, Point center_coord, int cap)
        {
            Parents = new Dictionary<int, PartitionNode>();
            Children = new Dictionary<int, PartitionNode>();
            layerNum = node_layer;
            bounds = new RectangleObj();
            bounds.SetRectangleByCenter(center_coord, size, size);
            capacity = cap;
        }

        public bool IsRootNode()
        {
            return (Parents.Count < 1);
        }
        
        public int DetermineMyChildNum(int parent_num) {
            // Determines the child position number of current node (0-8) based on the Childern Positions table for the given parent.
            if (Parents.Count < 1) {
        			return -1;
        	}
        	if (Parents.ContainsKey(parent_num)) {
        		foreach (KeyValuePair<int, PartitionNode> par_child in Parents[parent_num].getChildern()) {
        			if (par_child.Value.Equals(this)) {
        				return par_child.Key;
        			}
        		}
        	}
        	return -1;
        }
        
        
        public Dictionary<int, PartitionNode> GetMySiblings() {
            // To get my siblings, need to go 1 level up and look at all parents, and identify which position I am located with respect to 
            // other children, then idenify a neighboring sibling and translate its number back relative to me.
        	Dictionary<int, PartitionNode> siblings = new Dictionary<int, PartitionNode>();
        	if (IsRootNode()) {
        			return siblings;
        	}
        	foreach (KeyValuePair<int, PartitionNode> parent in Parents) {
        		Dictionary<int, PartitionNode> par_children = parent.Value.getChildern();
        		foreach (KeyValuePair<int, PartitionNode> par_child in par_children) {
        			if (par_child.Value.Equals(this)) {
        				int sib_offset = NodeHelperFuncs.ComputeSiblingShiftForChildNum(par_child.Key);
        				foreach (int sibling_num in NodeHelperFuncs.getChildSiblings(par_child.Key)) {
        					if (par_children.ContainsKey(sibling_num)) {
        						int my_sib_num = sib_offset + sibling_num;
        						if (siblings.ContainsKey(my_sib_num)) {
        							siblings[my_sib_num] = par_children[sibling_num];
        						}
        						else 
        						{
        							siblings.Add(my_sib_num, par_children[sibling_num]);
        						}
        					}
        				}
        				break;
        			}
        		}
        	}
            return siblings;
        } 
        
        public PartitionNode GetMySingleSibling(int sibling) {
        	Dictionary<int, PartitionNode> siblings = GetMySiblings();
        	if (siblings.ContainsKey(sibling)) {
        		return siblings[sibling];
        	}
        	return null;
        } 

        private void ConnectSharedChildren(Dictionary<int, PartitionNode> added_children)
        {
            // For created children finds and connects other parents sharing the same children.
            Dictionary<int, PartitionNode> all_siblings = GetMySiblings();
            foreach (KeyValuePair<int, PartitionNode> child in added_children)
            {
                foreach (int sibling_num in NodeHelperFuncs.getRelevantSiblings(child.Key))
                {
                    if (all_siblings.ContainsKey(sibling_num) && all_siblings[sibling_num] != null)
                    {
                        int sChildNum = NodeHelperFuncs.getSharedSiblingChildNumber(child.Key, sibling_num);
                        all_siblings[sibling_num].SetSingleChild(sChildNum, child.Value);
                        child.Value.AddParent(all_siblings[sibling_num], NodeHelperFuncs.GetParentPosition(sChildNum));
                    }
                }
            }
        }
        public Dictionary<int, PartitionNode> getChildern()
        {
            return Children;
        }

        private void SetSingleChild (int child, PartitionNode child_node)
        {
            if (!Children.ContainsKey(child))
            {
                Children.Add(child, child_node);
            }
        }

        public PartitionNode getSingleChild(int child)
        {
            if (Children.Count < 1)
                return null;
            if (!Children.ContainsKey(child))
                return null;
            return Children[child];
        }

        public bool HasChildren()
        {
            return (Children.Count > 1);
        }

        public void AddParent(PartitionNode parent, int place, bool force_update = false)
        {
            if (!Parents.ContainsKey(place))
            {
                Parents.Add(place, parent);
            }
            else
            {
                if (force_update)
                {
                    Parents[place] = parent;
                }
            }
        }

        public override void CreateChildren()
        {
            // Do nothing, if children are already created
            if (Children.Count >= 9)
                return;

            int children_layer = layerNum + 1;
            int children_size = getNodeSize() / 2;
            Dictionary<int, PartitionNode> added_children = new Dictionary<int, PartitionNode>();

            for (int i = 0; i < 9; i++)
            {
                if (Children.ContainsKey(i) && Children[i] != null)
                {
                    continue;
                }
                else
                {
                    PartitionNode new_child = new PartitionNode(children_layer, children_size, NodeHelperFuncs.GetChildCenter(this, i), capacity);
                    new_child.AddParent(this, NodeHelperFuncs.GetParentPosition(i));
                    Children.Add(i, new_child);
                    added_children.Add(i, new_child);
                }
            }
            ConnectSharedChildren(added_children);
        }

        public Dictionary<int, PartitionNode> GetParents()
        {
            return Parents;
        }

        public bool AreExistingChildrenEmpty()
        {
            if (Children.Count < 1)
            {
                return false;
            }
            foreach (KeyValuePair<int, PartitionNode> child in Children)
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
            if (Children.Count < 1)
            {
                return;
            }
            for (int i = 0; i < 9; i++) {
            	RemoveChild(i);
            }
        }
        
        public void RemoveChild(int child, bool check_siblings = true) {
        	if (!Children.ContainsKey(child)) {
        		return;
        	}
        	if (IsRootNode() || !check_siblings) {
        		Children.Remove(child);
        		return;
        	}
        	Dictionary<int, PartitionNode> siblings = GetMySiblings();
        	foreach (int sibling_num in NodeHelperFuncs.getRelevantSiblings(child))
        	{
        		if (siblings.ContainsKey(sibling_num) && siblings[sibling_num] != null) {
        			int sib_child_num = NodeHelperFuncs.getSharedSiblingChildNumber(child, sibling_num);
        			siblings[sibling_num].RemoveChild(sib_child_num, false);
        		}
        	}
        	Children.Remove(child);
        }

        public PartitionNode LookupChild(Point p)
        {
            int childnum = HelperFuncs.LookupChildPartition(p, this);
            return getSingleChild(childnum);
        }

        public bool HasAllChildren()
        {
            return (Children.Count == 9);
        }
    }
}
