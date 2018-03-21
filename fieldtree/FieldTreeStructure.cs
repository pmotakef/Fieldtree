using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Priority_Queue;

namespace fieldtree
{
    public enum CreationMode
    {
        ALL_AT_ONCE = 1,
        LAZY = 2,
    }

    public class FieldTreeStructure<NodeType> where NodeType : IFieldNode<NodeType>, new()
    {
        #region Node or Rectangle Object

        /// <summary>
        /// A generic class for NodeType (can be either Cover or Partition Fieldtree node).
        /// </summary>
        private class NodeOrObj : IEquatable<NodeOrObj>
    	{
            /// <summary>
            /// A Private wrapper class for Nodes and Rectangle objects since both have RectangleObj part.
            /// </summary>
            private NodeType node;
    		private RectangleObj obj;
    				
    		public NodeOrObj() 
    		{
    					
    		}
    				
    		public void SetNode(NodeType n) 
    		{
    				node = n;
    		}
    				
    		public void SetObj(RectangleObj o)
    		{
    				obj = o;
    		}
    				
    		public NodeType GetNode() 
    		{
    				return node;	
    		}
    				
    		public RectangleObj GetObj() 
    		{
    				return obj;
    		}
    				
    		public bool IsNode() 
    		{
    			return (node != null);
    		}
    				
    		public bool IsObj()
    		{
    			return (obj != null);
    		}

            public bool Equals(NodeOrObj other)
            {
                if (other.IsNode() && IsNode() && other.GetNode().Equals(node))
                {
                    return true;
                }
                if (other.IsObj() && IsObj() && other.GetObj().Equals(obj))
                {
                    return true;
                }
                return false;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                    return false;
                if (ReferenceEquals(this, obj))
                    return true;
                if (obj.GetType() != GetType())
                    return false;
                return Equals(obj as NodeOrObj);
            }

            public override int GetHashCode()
            {
                if (IsNode())
                {
                    return node.GetHashCode();
                }
                if (IsObj())
                {
                    return obj.GetHashCode();
                }
                return 0;
            }
        }

        #endregion

        public readonly int region_width;
        public readonly int region_height;
        private int field_capacity;
        private int numlayers;
        private CreationMode descendent_creation_mode;

        private NodeType root;
        private List<RectangleObj> allObjs;

        private NodeType moving_node;
        private RectangleObj moving_obj;
        
        private SimplePriorityQueue<NodeOrObj> incrNN_queue;
        private Point incrNN_origin;

        #region Initialization and Setup

        public FieldTreeStructure(int width, int height, NodeType root_node, int capacity = 2, bool allow_overflow = true, CreationMode creation_mode = CreationMode.LAZY)
        {
            region_height = height;
            region_width = width;
            field_capacity = capacity;

            root = root_node;
            allObjs = new List<RectangleObj>();
            numlayers = 0;
            descendent_creation_mode = creation_mode;
            if (allow_overflow)
            {
                field_capacity *= 2;
            }
        }

        public void SetFieldCapacity(int size)
        {
            field_capacity = size;
        }

        public int GetLayers()
        {
            return numlayers;
        }

        public List<RectangleObj> GetAllObjs()
        {
            return allObjs;
        }

        public List<LayerBounds> GetNodeBounds()
        {
            // This is used for drawing node bounds (only for display purpose)
            List<LayerBounds> layerBounds = new List<LayerBounds>();
            allObjs = new List<RectangleObj>();
            int nlayers = 0;

            if (root == null)
                return layerBounds;

            List<NodeType> visitedNodes = new List<NodeType>();

            Queue<NodeType> nodeQueue = new Queue<NodeType>();
            nodeQueue.Enqueue(root);

            while (nodeQueue.Count > 0)
            {
                NodeType node = nodeQueue.Dequeue();

                if (visitedNodes.Contains(node))
                    continue;

                allObjs.AddRange(node.GetAllStoredObjs());
                if (node.GetLayerlevel() > nlayers)
                {
                    nlayers = node.GetLayerlevel();
                }

                LayerBounds bound = new LayerBounds
                {
                    bounds = new Rectangle(node.GetBounds().min_extent_X, node.GetBounds().min_extent_Y, node.GetBounds().rect_width, node.GetBounds().rect_height),
                    layerNum = node.GetLayerlevel()
                };
                layerBounds.Add(bound);

                if (node.HasChildren())
                {
                    foreach (KeyValuePair<int, NodeType> child in node.getChildern())
                    {
                        nodeQueue.Enqueue(child.Value);
                    }
                }
            }
            numlayers = nlayers;
            return layerBounds;
        }

        #endregion

        #region Add and Remove

        public void AddManyRectangles(List<RectangleObj> rects)
        {
            foreach (RectangleObj rect in rects)
            {
                AddRectangle(rect, false);
            }
            ReorganizeOverflownNodes();
        }

        public NodeType AddRectangle(RectangleObj rect, bool reorganize = true)
        {
            NodeType deepest_field = FindContainingField(rect, root);

            if (deepest_field.IsFull() && !deepest_field.HasAllChildren())
            {
                PartitionField(deepest_field);
                NodeType node = AddRectangle(rect);
                if (reorganize)
                    ReorganizeOverflownNodes();
                if (node != null)
                    return node;
            }
            else
            {
                bool overflown = deepest_field.StoreRectangle(rect);
            }
            return (deepest_field);
        }

        private void PartitionField(NodeType node)
        {
            // Creates child nodes and deeper layer field
            if (descendent_creation_mode == CreationMode.ALL_AT_ONCE)
            {
                int deepest_level = node.GetLayerlevel();
                Queue<NodeType> queued_nodes = new Queue<NodeType>();
                queued_nodes.Enqueue(root);
                while (queued_nodes.Count > 0)
                {
                    NodeType current_node = queued_nodes.Dequeue();
                    if (current_node.GetLayerlevel() == deepest_level)
                    {
                        if (!current_node.HasChildren())
                            current_node.CreateChildren();
                    }
                    else
                    {
                        if (current_node.HasChildren())
                        {
                            foreach (KeyValuePair<int, NodeType> entry in current_node.getChildern())
                            {
                                queued_nodes.Enqueue(entry.Value);
                            }
                        }
                    }
                }
            }
            else // Lazy
            {
                node.CreateChildren();
            }
        }

        private void ReorganizeOverflownNodes()
        {
            ReorganizeNode(root);
        }

        private void ReorganizeNode(NodeType node)
        {
            // After adding objects and creating new partitions, checks to see if any of the upper level objects can go deeper into the tree.
            Queue<NodeType> all_nodes = new Queue<NodeType>();
            all_nodes.Enqueue(node);

            while (all_nodes.Count > 0)
            {
                List<RectangleObj> rects_removed = new List<RectangleObj>();

                NodeType current_node = all_nodes.Dequeue();

                foreach (RectangleObj rect in current_node.GetOverflowObjs())
                {
                    NodeType deepest_field = FindContainingField(rect, current_node);

                    if (!deepest_field.Equals(current_node))
                    {
                        bool overflown = deepest_field.StoreRectangle(rect);
                        rects_removed.Add(rect);
                    }
                }
                if (rects_removed.Count > 0)
                {
                    current_node.DeleteRectangles(rects_removed, true);
                }

                if (current_node.HasChildren())
                {
                    foreach (KeyValuePair<int, NodeType> child in current_node.getChildern())
                    {
                        all_nodes.Enqueue(child.Value);
                    }
                }
            }
        }

        public NodeType FindContainingField(RectangleObj rect, NodeType starting_node)
        {
            Point node_center = starting_node.getCenter();
            int node_size = starting_node.getNodeSize();
            if (starting_node.IsPointInNode(rect.rect_center))
            {
                bool contained_in_node = rect.ContainedByArea(node_center, node_size, node_size);
                if (contained_in_node)
                {
                    if (node_size <= 1 || !starting_node.HasChildren() || rect.importance_layer == starting_node.GetLayerlevel() || Math.Max(rect.rect_width, rect.rect_height) > node_size / 2)
                    {
                        return starting_node;
                    }
                }
                NodeType child_node = starting_node.LookupChild(rect.rect_center);

                if (child_node != null)
                {
                    NodeType answer_node = FindContainingField(rect, child_node);
                    if (answer_node != null)
                    {
                        return answer_node;
                    }
                }
                if (contained_in_node)
                {
                    return starting_node;
                }
            }
            return default(NodeType);
        }

        private RectangleObj RemoveRectangle(RectangleObj rect, NodeType node)
        {
            node.RemoveRectangle(rect);
            MergeEmptyChildren(node);
            return rect;
        }

        private void MergeEmptyChildren(NodeType node)
        {
            if (node.IsRootNode() && !node.IsEmpty())
            {
                return;
            }
            if (node.HasChildren() && node.AreExistingChildrenEmpty())
            {
                node.MergeEmptyChildren();
            }
            foreach (KeyValuePair<int, NodeType> par in node.GetParents())
            {
                MergeEmptyChildren(par.Value);
            }
        }

        public RectangleObj FindNearestObjAndRemove(Point p)
        {
            RectangleObj obj = new RectangleObj();
            NodeType node = default(NodeType);
            foreach (ObjAndNode<NodeType> entry in FindNearestRectToPoint(p, root))
            {
                obj = entry.rect;
                node = entry.node;
            }

            if (node != null)
            {
                return RemoveRectangle(obj, node);
            }
            return obj;
        }

        #endregion

        #region Query

        public List<RectangleObj> FindNearestObj(Point p)
        {
            List<RectangleObj> results = new List<RectangleObj>();
            foreach (ObjAndNode<NodeType> entry in FindNearestRectToPoint(p, root))
            {
                RectangleObj rect1 = new RectangleObj(entry.rect);
                rect1.rect_color = Color.Red;
                RectangleObj rect2 = new RectangleObj(entry.node.GetBounds());
                rect2.rect_color = Color.Red;
                results.Add(rect1);
                results.Add(rect2);
            }
            return results;
        }

        private List<ObjAndNode<NodeType>> FindNearestRectToPoint(Point p, NodeType node)
        {
            List<ObjAndNode<NodeType>> answer_list = new List<ObjAndNode<NodeType>>();
            SimplePriorityQueue<NodeType> searching_nodes = new SimplePriorityQueue<NodeType>();
            searching_nodes.Enqueue(node, 0);

            RectangleObj answer = null;
            NodeType answer_node = default(NodeType);

            double min_distance_sq = region_width * region_width;

            while (searching_nodes.Count > 0)
            {
                NodeType current_node = searching_nodes.Dequeue();
                Dictionary<RectangleObj, double> nearest_rect = current_node.GetNearestRectangle(p);
                if (nearest_rect.Count > 0)
                {
                    foreach (KeyValuePair<RectangleObj, double> entry in nearest_rect)
                    {
                        if (entry.Value <= min_distance_sq || entry.Value < 0.01)
                        {
                            min_distance_sq = entry.Value;
                            answer = entry.Key;
                            answer_node = current_node;
                            if (min_distance_sq < 0.01)
                            {
                                ObjAndNode<NodeType> ans = new ObjAndNode<NodeType>();
                                ans.node = answer_node;
                                ans.rect = answer;
                                answer_list.Add(ans);
                                answer = null;
                            }
                        }
                    }
                }
                if (current_node.HasChildren())
                {
                    foreach (KeyValuePair<int, NodeType> child in current_node.getChildern())
                    {
                        NodeType child_node = child.Value;
                        RectangleObj field_rect = new RectangleObj();
                        field_rect.SetRectangleByCenter(child_node.getCenter(), child_node.getNodeSize(), child_node.getNodeSize());
                        double field_dist = field_rect.GetDistanceSqToPoint(p);
                        if (field_dist <= min_distance_sq)
                            searching_nodes.Enqueue(child_node, (float)field_dist);
                    }
                }
            }
            if (answer != null)
            {
                ObjAndNode<NodeType> ans = new ObjAndNode<NodeType>();
                ans.node = answer_node;
                ans.rect = answer;
                answer_list.Add(ans);
            }
            return answer_list;

        }

        public List<RectangleObj> RangeQuery(Point center, int radius)
        {
            // Finds objects within a Circular range
            List<RectangleObj> answer = new List<RectangleObj>();
            Queue<NodeType> searching_nodes = new Queue<NodeType>();

            searching_nodes.Enqueue(root);
            while (searching_nodes.Count > 0)
            {
                NodeType current_node = searching_nodes.Dequeue();
                if (current_node.GetBounds().IntersectsCircle(center, radius))
                {
                    answer.AddRange(current_node.GetRangeQueryObj(center, radius));
                    if (current_node.HasChildren())
                    {
                        foreach (KeyValuePair<int, NodeType> child in current_node.getChildern())
                        {
                            if (!searching_nodes.Contains(child.Value))
                                searching_nodes.Enqueue(child.Value);
                        }
                    }
                }
            }
            return answer;
        }

        public List<RectangleObj> WindowQuery(RectangleObj window)
        {
            // Finds objects within a Rectangular range (window)
            List<RectangleObj> answer = new List<RectangleObj>();
            Queue<NodeType> searching_nodes = new Queue<NodeType>();

            searching_nodes.Enqueue(root);
            while (searching_nodes.Count > 0)
            {
                NodeType current_node = searching_nodes.Dequeue();
                if (current_node.GetBounds().IntersectsWith(window))
                {
                    answer.AddRange(current_node.GetRangeQueryObj(window));
                    if (current_node.HasChildren())
                    {
                        foreach (KeyValuePair<int, NodeType> child in current_node.getChildern())
                        {
                            if (!searching_nodes.Contains(child.Value))
                                searching_nodes.Enqueue(child.Value);
                        }
                    }
                }
            }
            return answer;
        }

        #endregion

        #region Moving objects

        public void InitMoving(Point p)
        {
            RectangleObj obj = new RectangleObj();
            NodeType node = default(NodeType);
            foreach (ObjAndNode<NodeType> entry in FindNearestRectToPoint(p, root))
            {
                obj = entry.rect;
                node = entry.node;
            }

            if (node != null)
            {
                moving_node = node;
                moving_obj = obj;
            }
        }

        public List<RectangleObj> MoveObject(Point delta_p)
        {
            RemoveRectangle(moving_obj, moving_node);
            moving_obj.MoveCenter(delta_p);
            NodeType node = AddRectangle(moving_obj);
            if (moving_node != null)
                moving_node = node;
            List<RectangleObj> answer = new List<RectangleObj>();
            answer.Add(moving_obj);
            answer.Add(moving_node.GetBounds());
            return answer;
        }

        public void StopMoving()
        {
            moving_node = default(NodeType);
            moving_obj = null;
        }

        #endregion

        #region Incremental Nearest Neighbor Search

        public void InitIncrementalNN(Point p) 
        {
        	incrNN_queue = new SimplePriorityQueue<NodeOrObj>();
        	NodeOrObj root_nodeOrObj = new NodeOrObj();
        	root_nodeOrObj.SetNode(root);
        	incrNN_queue.Enqueue(root_nodeOrObj, 0);
        	incrNN_origin = p;
        }
        
        public List<RectangleObj> IncrementalNNFindNext() 
        {
        	// 1st entry is the object found, the rest are the bounding boxes of circular search steps.
        	List<RectangleObj> answer = new List<RectangleObj>();
            Random rand = new Random();

            while (incrNN_queue.Count > 0) 
	        {
                NodeOrObj current_element = incrNN_queue.Dequeue();
                while (incrNN_queue.Count > 0 && incrNN_queue.First().Equals(current_element))
                {
                    incrNN_queue.Dequeue();
                }

                if (current_element.IsObj()) 
	        	{
                    double dist = current_element.GetObj().GetDistanceSqToPoint(incrNN_origin);
                    RectangleObj circleObj = new RectangleObj();
                    int radius = (int)(Math.Sqrt(dist) * 2.0);
                    circleObj.SetRectangleByCenter(incrNN_origin, radius, radius);
                    circleObj.rect_color = Color.Orange;
                    answer.Add(circleObj);

                    answer.Insert(0, current_element.GetObj());
	        		return answer;
	        	}
	        	else 
	        	{
	        		NodeType current_node = current_element.GetNode();
                    double current_dist = current_node.GetBounds().GetDistanceSqToPoint(incrNN_origin);
                    RectangleObj circleObj = new RectangleObj();
                    int radius = (int)(Math.Sqrt(current_dist) * 2.0);
                    circleObj.SetRectangleByCenter(incrNN_origin, radius, radius);
                    Color col = Color.FromArgb(0, rand.Next(50, 256), rand.Next(50, 256));
                    circleObj.rect_color = col;

                    answer.Add(circleObj);
	        		if (!current_node.IsEmpty()) 
	        		{
	        			foreach (RectangleObj obj in current_node.GetAllStoredObjs()) 
	        			{
	        				double distance = obj.GetDistanceSqToPoint(incrNN_origin);
	        				if (distance >= current_dist) 
	        				{
			        			NodeOrObj obj_nodeOrObj = new NodeOrObj();
	  		      				obj_nodeOrObj.SetObj(obj);
	  		      				incrNN_queue.Enqueue(obj_nodeOrObj, (float)distance);
	        				}
	        			}
	        		}
	        		if (current_node.HasChildren())
	        		{
	        			foreach (KeyValuePair<int, NodeType> child_node in current_node.getChildern()) 
	        			{
	        				double distance = child_node.Value.GetBounds().GetDistanceSqToPoint(incrNN_origin);
	        				if (distance >= current_dist) 
	        				{
			        			NodeOrObj node_nodeOrObj = new NodeOrObj();
	  		      				node_nodeOrObj.SetNode(child_node.Value);
	  		      				incrNN_queue.Enqueue(node_nodeOrObj, (float)distance);
	        				}
	        			}
	        		}
	        	}
	        }
	        return answer;
        }

        #endregion

    }
}
