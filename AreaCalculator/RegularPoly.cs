using System.Data;

namespace AreaCalculator;

public static class RegularPoly
{
    private const float Threshold = 1e-6f;
    
    /// <summary>
    /// Calculate area for few right polygons or circle
    /// <see cref="sides"/> lenght define calculation method> <br/>
    /// 1 - calculate circle area with radius from this single value
    /// 2 - return zero - for polygon with two sides
    /// 3 - triangle area
    /// 4 - rectangle or square area
    /// </summary>
    /// <param name="sides"></param>
    /// <returns></returns>
    /// <exception cref="ConstraintException"></exception>
    public static float GetAreaBySides(float[] sides)
    {
        switch (sides.Length)
        {
            case 0: return 0;
            case 1 when sides[0] >= 0: 
                return sides[0] * sides[0] * float.Pi;
            case 2 when Math.Abs(sides[0] - sides[2]) < sides[0] * Threshold: 
                return 0;
            case 3 when sides.All(s => s > 0) 
                        && sides[0] + sides[1] >= sides[2] 
                        && sides[1] + sides[2] >= sides[0] 
                        && sides[0] + sides[2] >= sides[1]: 
                var p = (sides[0] + sides[1] + sides[2]) / 2; 
                return MathF.Sqrt(p * (p - sides[0]) * (p - sides[1]) * (p - sides[2]));
            case 4 when Math.Abs(sides[0] - sides[2] + sides[1] - sides[3]) < sides[0] * Threshold:
                return sides[0] * sides[1];
            default: throw new ConstraintException("Area for polygon with this sides can't be calculated");
        }
    }

    /// <summary>
    /// Return true if triangle is right
    /// </summary>
    /// <param name="sides"></param>
    /// <returns></returns>
    public static bool IsRightTriangle(float[] sides)
    {
        if (sides.Length != 3)
        {
            return false;
        }   
        
        var hypotenuse = sides[0] > sides[1] ? sides[0] : sides[1];
        hypotenuse = sides[2] > hypotenuse ? sides[2] : hypotenuse;
        
        return Math.Abs(sides[0] * sides[0] 
            + sides[1] * sides[1] 
            + sides[2] * sides[2]
            - hypotenuse * hypotenuse * 2
            ) < hypotenuse * hypotenuse * Threshold;
    }}