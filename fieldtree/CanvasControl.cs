using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fieldtree
{
    /// <summary>
    /// Canvas control which deals with displaying the graphical representation of the fieldtree state.
    /// </summary>
    public partial class CanvasControl : UserControl
    {
        public event EventHandler<RectangleObj> rectAdded;
        public event EventHandler<Point> pointQueryRequest;

        private List<LayerBounds> all_layer_bounds;
        private List<RectangleObj> rects_to_draw;
        private List<RectangleObj> hollow_rects_to_draw;
        private List<RectangleObj> hollow_circles;
        private int db_width;
        private int db_height;
        private double width_segment;
        private double height_segment;

        private int num_layers;

        private int num_layers_to_show;
        private int starting_layer_to_show;

        private int canvas_offset = 0;

        private Random rnd = new Random();

        bool is_draggin = false;
        PointF mouse_down_pos;
        PointF current_mouse_pos;

        private bool incrementalSet;
        private PointF incrementalPos;

        public ActionMode action_status;

        public CanvasControl()
        {
            InitializeComponent();
            all_layer_bounds = new List<LayerBounds>();
            rects_to_draw = null;
            hollow_rects_to_draw = new List<RectangleObj>();
            hollow_circles = new List<RectangleObj>();
            incrementalSet = false;
        }

        protected virtual void OnRectangleAdded(RectangleObj e)
        {
            rectAdded?.Invoke(this, e);
        }

        protected virtual void OnPointQueryRequested(Point e)
        {
            pointQueryRequest?.Invoke(this, e);
        }

        public void SetObjList(List<RectangleObj> list)
        {
            rects_to_draw = list;
        }

        public void SetLayerBounds(List<LayerBounds> layers, int num)
        {
            all_layer_bounds = layers;
            num_layers = num + 1;
        }

        public void ResetCanvas()
        {
            num_layers_to_show = 0;
            starting_layer_to_show = 0;

            DoubleBuffered = true;
            hollow_rects_to_draw.Clear();
        }

        public void UpdateCanvasSize()
        {
            SetupCanvas(db_width, db_height, false);
            RedrawCanvas();
        }

        public void SetupCanvas(int width, int height, bool reset = true)
        {
            db_height = height;
            db_width = width;

            width_segment = (this.Width - 2.0 * canvas_offset) / db_width;
            height_segment = (this.Height - 2.0 * canvas_offset) / db_height;
            if (reset)
            {
                ResetCanvas();
            }
        }

        public void LimitLayersToShow(int number_of_layers, int starting_layer)
        {
            num_layers_to_show = number_of_layers;
            starting_layer_to_show = starting_layer;
        }

        private PointF ConvertCoordToScreen(float x, float y)
        {
            return new PointF((float)(x * width_segment) + canvas_offset, (float)(y * height_segment) + canvas_offset);
        }

        private PointF ConvertCoordToScreen(PointF p)
        {
            return new PointF((float)(p.X * width_segment) + canvas_offset, (float)(p.Y * height_segment) + canvas_offset);
        }

        private PointF ConvertCoordFromScreen(float x, float y)
        {
            return new PointF((float)((x - canvas_offset) / width_segment), (float)((y - canvas_offset) / height_segment));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (all_layer_bounds.Count < 1)
                return;

            foreach (LayerBounds bound in all_layer_bounds)
            {
                int colval = ((bound.layerNum) * 220) / num_layers;
                int pen_width = Math.Max(1, 5 - bound.layerNum);
                Brush br = new SolidBrush(Color.FromArgb(colval, colval, colval));
                Pen pen = new Pen(br, pen_width);

                PointF p1 = ConvertCoordToScreen(bound.bounds.X, bound.bounds.Y);
                PointF p2 = ConvertCoordToScreen(bound.bounds.Width, bound.bounds.Height);

                e.Graphics.DrawRectangle(pen, p1.X, p1.Y, p2.X, p2.Y);

            }

            if (is_draggin && action_status != ActionMode.Move)
            {
                Brush br = new SolidBrush(Color.Black);

                PointF prevPos = ConvertCoordFromScreen(mouse_down_pos.X, mouse_down_pos.Y);
                PointF curPos = ConvertCoordFromScreen(current_mouse_pos.X, current_mouse_pos.Y);

                Point pPos = new Point((int)prevPos.X, (int)prevPos.Y);
                Point cPos = new Point((int)curPos.X, (int)curPos.Y);

                RectangleObj newRectangle = new RectangleObj();
                newRectangle.SetRectangleByCoords(pPos, cPos);
                if (action_status == ActionMode.RangeQuery)
                {
                    int rad = Math.Max(newRectangle.rect_width, newRectangle.rect_height);
                    newRectangle.UpdateWH(rad, rad);
                }

                prevPos = ConvertCoordToScreen(newRectangle.min_extent_X, newRectangle.min_extent_Y);
                curPos = ConvertCoordToScreen(newRectangle.rect_width, newRectangle.rect_height);
                if (action_status == ActionMode.Add || action_status == ActionMode.WindowQuery)
                {
                    e.Graphics.DrawRectangle(new Pen(br), prevPos.X, prevPos.Y, curPos.X, curPos.Y);
                }
                else if (action_status == ActionMode.RangeQuery)
                {
                    e.Graphics.DrawEllipse(new Pen(br), prevPos.X, prevPos.Y, curPos.X, curPos.Y);
                }
            }
            if (rects_to_draw != null && rects_to_draw.Count > 0)
            {
                foreach (RectangleObj rect in rects_to_draw)
                {
                    Brush br = new SolidBrush(rect.rect_color);
                    PointF corner_coord = ConvertCoordToScreen(rect.min_extent_X, rect.min_extent_Y);
                    PointF wh_extent = ConvertCoordToScreen(rect.rect_width, rect.rect_height);
                    e.Graphics.FillRectangle(br, corner_coord.X, corner_coord.Y, wh_extent.X, wh_extent.Y);
                }
            }
            if (hollow_rects_to_draw.Count > 0)
            {
                foreach (RectangleObj rect in hollow_rects_to_draw)
                {
                    Brush br = new SolidBrush(Color.Red);
                    PointF corner_coord = ConvertCoordToScreen(rect.min_extent_X, rect.min_extent_Y);
                    PointF wh_extent = ConvertCoordToScreen(rect.rect_width, rect.rect_height);
                    Pen pen = new Pen(br, 3);
                    e.Graphics.DrawRectangle(pen, corner_coord.X, corner_coord.Y, wh_extent.X, wh_extent.Y);
                    pen.Dispose();
                }
            }
            if (incrementalSet)
            {
                Brush br = new SolidBrush(Color.Orange);
                e.Graphics.FillRectangle(br, incrementalPos.X, incrementalPos.Y, 5, 5);
            }
            if (hollow_circles.Count > 0)
            {
                foreach (RectangleObj rect in hollow_circles)
                {
                    PointF corner_coord = ConvertCoordToScreen(rect.min_extent_X, rect.min_extent_Y);
                    PointF wh_extent = ConvertCoordToScreen(rect.rect_width, rect.rect_height);
                    Brush br = new SolidBrush(rect.rect_color);
                    Pen pen = new Pen(br, 2);
                    e.Graphics.DrawEllipse(pen, corner_coord.X, corner_coord.Y, wh_extent.X, wh_extent.Y);
                    pen.Dispose();
                }
            }

        }

        public void RedrawCanvas()
        {
            Invalidate();
        }

        public void Clear()
        {
            RedrawCanvas();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (all_layer_bounds.Count < 1)
                return;

            mouse_down_pos = e.Location;
            current_mouse_pos = e.Location;
            if (action_status == ActionMode.Add || action_status == ActionMode.RangeQuery || action_status == ActionMode.WindowQuery)
                is_draggin = true;
            else if (action_status == ActionMode.Move)
            {
                is_draggin = true;
                PointF curPos = ConvertCoordFromScreen(e.Location.X, e.Location.Y);
                OnPointQueryRequested(new Point((int)curPos.X, (int)curPos.Y));
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if ((action_status == ActionMode.Add  || action_status == ActionMode.RangeQuery || action_status == ActionMode.WindowQuery) && is_draggin)
            {
                PointF prevPos = ConvertCoordFromScreen(mouse_down_pos.X, mouse_down_pos.Y);
                PointF curPos = ConvertCoordFromScreen(current_mouse_pos.X, current_mouse_pos.Y);

                Point pPos = new Point((int)prevPos.X, (int)prevPos.Y);
                Point cPos = new Point((int)curPos.X, (int)curPos.Y);

                RectangleObj newRectangle = new RectangleObj();
                newRectangle.SetRectangleByCoords(pPos, cPos);

                if (action_status == ActionMode.RangeQuery)
                {
                    int rad = Math.Max(newRectangle.rect_height, newRectangle.rect_width);
                    newRectangle.UpdateWH(rad, rad);
                    
                }

                newRectangle.rect_color = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                newRectangle.importance_layer = -1;

                OnRectangleAdded(newRectangle);

                is_draggin = false;
                RedrawCanvas();
            }
            else if (action_status == ActionMode.PointQuery || action_status == ActionMode.Remove || action_status == ActionMode.IncrementalNN)
            {
                PointF curPos = ConvertCoordFromScreen(e.Location.X, e.Location.Y);
                OnPointQueryRequested(new Point((int)curPos.X, (int)curPos.Y));
                if (action_status == ActionMode.IncrementalNN)
                {
                    incrementalSet = true;
                    incrementalPos = e.Location;
                }
                else
                {
                    incrementalSet = false;
                }
            }
            else if (action_status == ActionMode.Move)
            {
                is_draggin = false;
                OnPointQueryRequested(new Point(-1, -1));
                RedrawCanvas();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!is_draggin)
                return;
            if (action_status == ActionMode.Move)
            {
                PointF curPos = ConvertCoordFromScreen(e.Location.X, e.Location.Y);
                OnPointQueryRequested(new Point((int)curPos.X, (int)curPos.Y));
            }
            if (action_status != ActionMode.IncrementalNN)
            {
                incrementalSet = false;
            }
            current_mouse_pos = e.Location;
            RedrawCanvas();
        }

        public void ClearHollowRectList()
        {
            hollow_rects_to_draw.Clear();
            hollow_circles.Clear();
        }

        public void AddHollowRect (RectangleObj rect)
        {
            hollow_rects_to_draw.Add(rect);
        }

        public void AddHollowCircles(List<RectangleObj> circles)
        {
            hollow_circles.AddRange(circles);
        }

        public void AddHollowRects(List<RectangleObj> rects)
        {
            hollow_rects_to_draw.AddRange(rects);
        }
    }
}
