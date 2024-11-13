using System.Drawing;

namespace AreaCalculator;

public static class SimplePoly
{
    /// <summary>
    /// Get area for simply polygon by points
    /// In this version polygon don't validates for simply
    /// </summary>
    /// <param name="points"></param>
    /// <returns></returns>
    public static float GetAreaByPoints(IEnumerable<PointF> points)
    {
        PointF? lastPoint = null;
        PointF? firstPoint = null;
        
        var area = 0f;
        foreach (var point in points)
        {
            if (lastPoint is null)
            {
                firstPoint = lastPoint = point;
                continue;
            }

            area += lastPoint.Value.X * point.Y - lastPoint.Value.Y * point.X;
            lastPoint = point;
        }

        if (firstPoint is null)
            return area;
        
        area += lastPoint!.Value.X * firstPoint.Value.Y - lastPoint.Value.Y * firstPoint.Value.X;
        
        return Math.Abs(area / 2f);
    }
}