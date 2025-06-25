using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Shimmer.Wpf.Converters;

internal class BorderClipConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 3 &&
            values[0] is double width &&
            values[1] is double height &&
            values[2] is CornerRadius cr &&
            width > 0 && height > 0)
        {
            var rect = new Rect(0, 0, width, height);
            var geo = new StreamGeometry();

            using (var ctx = geo.Open())
            {
                Point topLeft = new(rect.Left + cr.TopLeft, rect.Top);
                Point topRight = new(rect.Right - cr.TopRight, rect.Top);
                Point rightTop = new(rect.Right, rect.Top + cr.TopRight);
                Point rightBottom = new(rect.Right, rect.Bottom - cr.BottomRight);
                Point bottomRight = new(rect.Right - cr.BottomRight, rect.Bottom);
                Point bottomLeft = new(rect.Left + cr.BottomLeft, rect.Bottom);
                Point leftBottom = new(rect.Left, rect.Bottom - cr.BottomLeft);
                Point leftTop = new(rect.Left, rect.Top + cr.TopLeft);

                ctx.BeginFigure(topLeft, true, true);
                ctx.LineTo(topRight, true, false);
                ctx.ArcTo(rightTop, new Size(cr.TopRight, cr.TopRight), 0, false, SweepDirection.Clockwise, true, false);
                ctx.LineTo(rightBottom, true, false);
                ctx.ArcTo(bottomRight, new Size(cr.BottomRight, cr.BottomRight), 0, false, SweepDirection.Clockwise, true, false);
                ctx.LineTo(bottomLeft, true, false);
                ctx.ArcTo(leftBottom, new Size(cr.BottomLeft, cr.BottomLeft), 0, false, SweepDirection.Clockwise, true, false);
                ctx.LineTo(leftTop, true, false);
                ctx.ArcTo(topLeft, new Size(cr.TopLeft, cr.TopLeft), 0, false, SweepDirection.Clockwise, true, false);
            }

            geo.Freeze();
            return geo;
        }

        return Geometry.Empty;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
