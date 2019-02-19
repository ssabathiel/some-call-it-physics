using System.Collections.Generic;

namespace TexDrawLib
{
    // Represents graphical box that is part of math expression, and can itself contain child boxes.
    public abstract class Box : IFlushable
    {
        //public List<Box> children;

        protected Box() { }

        public float totalHeight { get { return height + depth; } }

        public float width, height, depth, shift;

        public void Set (float w, float h, float d, float s)
        {
            width = w;
            height = h;
            depth = d;
            shift = s;
        }

        public virtual void Draw(DrawingContext drawingContext, float scale, float x, float y)
        {
            //EASTER-EGG: Un-strip this line for a fun part ;)
#if UNITY_EDITOR
            if (TEXConfiguration.main.Debug_HighlightBoxes)
                drawingContext.DrawWireDebug(new UnityEngine.Rect(x * scale, (y - depth) * scale, width * scale, totalHeight * scale), new UnityEngine.Color(1, 1, 0, 0.07f));
#endif
            //PS : this line is intended for debugging only
        }

        //public virtual void Add(Box box)
        //{
        //    children.Add(box);
        //}

        //public virtual void Add(int position, Box box)
        //{
        //    children.Insert(position, box);
        //}

        public abstract void Flush();
        //{
        //    width = height = depth = shift = 0;
        //    if (children != null && children.Count > 0)
        //    {
        //        for (int i = 0; i < children.Count; i++)
        //        {
        //            children[i].Flush();
        //        }
        //        children.Clear();
        //    }
        //}

        public bool IsFlushed { get; set; }

        /*public override string ToString ()
        {
        	var subs = new StringBuilder();
        	ToString(subs, 0);
        	return subs.ToString();
        }

        public void ToString (StringBuilder builder, int indent)
        {
        	builder.AppendLine(new string(' ', indent * 2) + GetType().ToString());
        	if (children != null) {
			for (int i = 0; i < children.Count; i++) {
				children[i].ToString(builder, indent + 1);
			}
			}
        }*/

        public override string ToString()
        {
            return base.ToString().Replace("TexDrawLib.", string.Empty) +
                string.Format(" H:{0:F2} D:{1:F2} W:{2:F2} S:{3:F2}", height, depth, width, shift);
        }
    }
}