using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace fieldtree
{
    public enum ActionMode
    {
        Add = 1,
        Remove = 2,
        PointQuery = 3,
        RangeQuery = 4,
        WindowQuery = 5,
        Move = 6,
        IncrementalNN = 7,
    }

    public partial class Form1 : Form
    {
        /// <summary>
        /// Fieldtree Demo Form happens here
        /// </summary>
        PartitionNode root_node;
        CoverNode cover_root;
        FieldTreeStructure<PartitionNode> rect_database_partition;
        FieldTreeStructure<CoverNode> rect_database_cover;

        bool type_partition;
        int canvas_size = -1;

        bool is_moving;

        public Form1()
        {
            InitializeComponent();
            dbCanvas.rectAdded += new EventHandler<RectangleObj>(DbCanvas_rectAdded);
            dbCanvas.pointQueryRequest += new EventHandler<Point>(DbCanvas_pointQueryRequest);
            dbCanvas.action_status = GetCurrentActionState();
            type_partition = true;
            cmbCreationMode.SelectedIndex = 0;
            is_moving = false;
        }

        private void DbCanvas_pointQueryRequest(object sender, Point e)
        {
            if (e.X == 0 && e.Y == 0)
            {
                return;
            }
            if (GetCurrentActionState() == ActionMode.PointQuery)
            {
                List<RectangleObj> rect;

                Stopwatch watch = Stopwatch.StartNew();
                if (type_partition)
                    rect = new List<RectangleObj>(rect_database_partition.FindNearestObj(e));
                else
                    rect = new List<RectangleObj>(rect_database_cover.FindNearestObj(e));
                watch.Stop();
                tbStopwatch.Text = watch.ElapsedMilliseconds.ToString();

                dbCanvas.ClearHollowRectList();
                if (rect != null && rect.Count > 0)
                    dbCanvas.AddHollowRects(rect);
                dbCanvas.RedrawCanvas();
            }
            else if (GetCurrentActionState() == ActionMode.Move)
            {
                if (!is_moving)
                {
                    is_moving = true;
                    if (type_partition)
                        rect_database_partition.InitMoving(e);
                    else
                        rect_database_cover.InitMoving(e);
                }
                else
                {
                    if (e.X < 0 || e.Y < 0)
                    {
                        is_moving = false;
                        if (type_partition)
                            rect_database_partition.StopMoving();
                        else
                            rect_database_cover.StopMoving();
                    }
                    else
                    {
                        List<RectangleObj> hollow_rects;
                        if (type_partition)
                            hollow_rects = rect_database_partition.MoveObject(e);
                        else
                            hollow_rects = rect_database_cover.MoveObject(e);

                        dbCanvas.ClearHollowRectList();
                        if (hollow_rects != null && hollow_rects.Count > 0)
                            dbCanvas.AddHollowRects(hollow_rects);
                        UpdateCanvasBounds();
                    }
                }
            }
            else if (GetCurrentActionState() == ActionMode.IncrementalNN)
            {
                if (type_partition)
                    rect_database_partition.InitIncrementalNN(e);
                else
                    rect_database_cover.InitIncrementalNN(e);
                btnINNNext.Enabled = true;
                dbCanvas.RedrawCanvas();
            }
            else // It is delete
            {
                RectangleObj rect;
                if (type_partition)
                    rect = rect_database_partition.FindNearestObjAndRemove(e);
                else
                    rect = rect_database_cover.FindNearestObjAndRemove(e);
                UpdateCanvasBounds();
            }
        }

        private void UpdateCanvasBounds()
        {
            if (type_partition)
            {
                List<LayerBounds> layerBounds = rect_database_partition.GetNodeBounds();
                dbCanvas.SetLayerBounds(layerBounds, rect_database_partition.GetLayers());
                dbCanvas.SetObjList(rect_database_partition.GetAllObjs());
            }
            else
            {
                List<LayerBounds> layerBounds = rect_database_cover.GetNodeBounds();
                dbCanvas.SetLayerBounds(layerBounds, rect_database_cover.GetLayers());
                dbCanvas.SetObjList(rect_database_cover.GetAllObjs());
            }
            dbCanvas.RedrawCanvas();
        }

        private void DbCanvas_rectAdded(object sender, RectangleObj e)
        {
            if (GetCurrentActionState() == ActionMode.Add)
            {
                e.importance_layer = Convert.ToInt32(tbImportance.Text);
                if (type_partition)
                    rect_database_partition.AddRectangle(e);
                else
                    rect_database_cover.AddRectangle(e);
            }
            else if (GetCurrentActionState() == ActionMode.WindowQuery)
            {
                List<RectangleObj> objs;
                Stopwatch watch = Stopwatch.StartNew();
                if (type_partition)
                {
                    objs = rect_database_partition.WindowQuery(e);
                }
                else
                {
                    objs = rect_database_cover.WindowQuery(e);
                }
                watch.Stop();
                tbStopwatch.Text = watch.ElapsedMilliseconds.ToString();
                if (objs.Count > 0)
                {
                    dbCanvas.ClearHollowRectList();
                    dbCanvas.AddHollowRects(objs);
                }
            }
            else // Range query
            {
                Point p = e.rect_center;
                int radius = Math.Max(e.rect_height, e.rect_width) / 2;
                List<RectangleObj> objs;
                Stopwatch watch = Stopwatch.StartNew();
                if (type_partition)
                {
                    objs = rect_database_partition.RangeQuery(p, radius);
                }
                else
                {
                    objs = rect_database_cover.RangeQuery(p, radius);
                }
                watch.Stop();
                tbStopwatch.Text = watch.ElapsedMilliseconds.ToString();
                if (objs.Count > 0)
                {
                    dbCanvas.ClearHollowRectList();
                    dbCanvas.AddHollowRects(objs);
                }
            }
            UpdateCanvasBounds();
        }

        private void btnSetExtent_Click(object sender, EventArgs e)
        {
            if (rdbPartition.Checked)
            {
                type_partition = true;
            }
            else
            {
                type_partition = false;
            }

            int size = Convert.ToInt32(tbWidth.Text);
            int capacity = Convert.ToInt32(tbCapacity.Text);
            double pValue = Convert.ToDouble(tb_pValue.Text);
            bool overflow = chbOverFlow.Checked;
            canvas_size = size;

            CreationMode mode;
            if (cmbCreationMode.SelectedIndex == 0)
                mode = CreationMode.LAZY;
            else
                mode = CreationMode.ALL_AT_ONCE;

            root_node = new PartitionNode(0, size, new Point(size / 2, size / 2), capacity);
            cover_root = new CoverNode(size, new Point(size / 2, size / 2), capacity, null, pValue);
            if (type_partition)
            {
                rect_database_partition = new FieldTreeStructure<PartitionNode>(size, size, root_node, root_node.capacity, overflow, mode);
            }
            else
            {
                rect_database_cover = new FieldTreeStructure<CoverNode>(size, size, cover_root, cover_root.capacity, overflow, mode);
            }
            dbCanvas.SetupCanvas(size, size);
            UpdateCanvasBounds();
        }

        private ActionMode GetCurrentActionState()
        {
            if (rdbActionsAdd.Checked)
                return ActionMode.Add;
            if (rdbActionDelete.Checked)
                return ActionMode.Remove;
            if (rdbActionFind.Checked)
                return ActionMode.PointQuery;
            if (rdbActionWindowQuery.Checked)
                return ActionMode.WindowQuery;
            if (rdbActionMove.Checked)
                return ActionMode.Move;
            if (rdbRangeQuery.Checked)
                return ActionMode.RangeQuery;
            //if (rdbActionIncrNN.Checked)
            return ActionMode.IncrementalNN;
        }

        private void rdbActionsAdd_CheckedChanged(object sender, EventArgs e)
        {
            dbCanvas.action_status = GetCurrentActionState();
        }

        private void rdbActionDelete_CheckedChanged(object sender, EventArgs e)
        {
            dbCanvas.action_status = GetCurrentActionState();
        }

        private void rdbActionFind_CheckedChanged(object sender, EventArgs e)
        {
            dbCanvas.action_status = GetCurrentActionState();
        }

        private void rdbActionWindowQuery_CheckedChanged(object sender, EventArgs e)
        {
            dbCanvas.action_status = GetCurrentActionState();
        }

        private void rdbActionMove_CheckedChanged(object sender, EventArgs e)
        {
            dbCanvas.action_status = GetCurrentActionState();
        }

        private void rdbRangeQuery_CheckedChanged(object sender, EventArgs e)
        {
            dbCanvas.action_status = GetCurrentActionState();
        }

        private void rdbActionIncrNN_CheckedChanged(object sender, EventArgs e)
        {
            dbCanvas.action_status = GetCurrentActionState();
            if (!rdbActionIncrNN.Checked)
            {
                btnINNNext.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (canvas_size < 0)
                return;

            int numRects = Convert.ToInt32(tbNumRects.Text);
            int randSeed = Convert.ToInt32(tbRandSeed.Text);

            Random rand = new Random(randSeed);

            List<RectangleObj> rects = new List<RectangleObj>();
            for (int i = 0; i < numRects; i++)
            {
                int x1 = rand.Next(1, canvas_size);
                int y1 = rand.Next(1, canvas_size);
                int x2 = rand.Next(1, canvas_size);
                int y2 = rand.Next(1, canvas_size);
                RectangleObj rect = new RectangleObj();
                rect.SetRectangleByCoords(new Point(x1, y1), new Point(x2, y2));
                rect.importance_layer = Convert.ToInt32(tbImportance.Text);
                rect.rect_color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                rects.Add(rect);
            }

            Stopwatch watch = Stopwatch.StartNew();
            if (type_partition)
                rect_database_partition.AddManyRectangles(rects);
            else
                rect_database_cover.AddManyRectangles(rects);
            watch.Stop();
            tbStopwatch.Text = watch.ElapsedMilliseconds.ToString();

            UpdateCanvasBounds();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            dbCanvas.Width = Math.Max(1, this.Width - 400);
            dbCanvas.Height = Math.Max(1, this.Height - 60);
            dbCanvas.UpdateCanvasSize();
        }

        private void btnINNNext_Click(object sender, EventArgs e)
        {
            if (GetCurrentActionState() != ActionMode.IncrementalNN)
            {
                return;
            }
            List<RectangleObj> iin;
            if (type_partition)
                iin = rect_database_partition.IncrementalNNFindNext();
            else
                iin = rect_database_cover.IncrementalNNFindNext();

            if (iin.Count > 0)
            {
                RectangleObj result = iin[0];
                iin.RemoveAt(0);

                dbCanvas.ClearHollowRectList();
                dbCanvas.AddHollowRect(result);
                dbCanvas.AddHollowCircles(iin);
            }
            dbCanvas.RedrawCanvas();

        }
    }
}
