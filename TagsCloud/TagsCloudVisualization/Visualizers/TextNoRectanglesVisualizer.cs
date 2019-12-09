using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Drawing.Drawing2D;
using TagsCloudVisualization.Styling;
using TagsCloudVisualization.Styling.Themes;

namespace TagsCloudVisualization.Visualizers
{
    public class TextNoRectanglesVisualizer : ICloudVisualizer
    {
        public Bitmap Visualize(Theme theme, IEnumerable<RectangleF> rectangles,
            int width = 1000, int height = 1000)
        {
            var result = new Bitmap(width, height);
            var random = new Random();
            using (var graphics = Graphics.FromImage(result))
            {
                graphics.FillRectangle(theme.BackgroundBrush, new RectangleF(0, 0, width, height));
                foreach (var rectangle in rectangles)
                    graphics.FillRectangle(GetRandomBrush(random, theme.RectangleBrushes), rectangle);
                return result;
            }
        }
        
        private Brush GetRandomBrush(Random random, ImmutableArray<Brush> brushes)
        {
            return brushes[random.Next(0, brushes.Length)];
        }

        public Bitmap Visualize(Style style, IEnumerable<Tag> tags,
            int width = 1000, int height = 1000)
        {
            var result = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(result))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.CompositingMode = CompositingMode.SourceOver;
                graphics.FillRectangle(style.Theme.BackgroundBrush, new RectangleF(0, 0, width, height));
                foreach (var tag in tags)
                    DrawTag(style, tag, graphics);
                return result;
            }
        }

        private static void DrawTag(Style style, Tag tag, Graphics graphics)
        {
            using (var path = GenerateTagPath(style, tag))
            {
                var transformMatrix = new[]
                {
                    new PointF(tag.Area.Left, tag.Area.Top),
                    new PointF(tag.Area.Right, tag.Area.Top),
                    new PointF(tag.Area.Left, tag.Area.Bottom)
                };
                graphics.Transform = new Matrix(path.GetBounds(), transformMatrix);
                graphics.FillPath(style.Theme.GetTagBrush(tag), path);
            }
        }

        private static GraphicsPath GenerateTagPath(Style style, Tag tag)
        {
            using (var font = new Font(style.FontProperties.Name,
                style.TagSizeCalculator.GetScaleFactor(tag.Count,style.FontProperties.MinSize)))
            {
                var formatCentered = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center,
                };
                var path = new GraphicsPath();
                path.AddString(
                    tag.Word,
                    font.FontFamily,
                    (int) font.Style,
                    tag.Area.Height,
                    new PointF(0, 0),
                    formatCentered);
                return path;
            }
        }
        
    }
}