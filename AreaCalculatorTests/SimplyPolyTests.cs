using System.Drawing;
using AreaCalculator;
using AutoFixture;
using FluentAssertions;

namespace AreaCalculatorTests;

public class SimplyPolyTests
{
    private readonly Fixture _fixture = new Fixture();
    
    [Fact]
    public void SquareArea_ForRightTriangle_Should_ReturnValidArea()
    {
        var side1 = _fixture.Create<float>();
        var side2 = _fixture.Create<float>();
        
        var area = SimplePoly.GetAreaByPoints([new PointF(0, 0), new PointF(side1, 0), new PointF(0, side2)]);
        area.Should().BeApproximately(side1 * side2 / 2, 1e-9f);
    }
    
    [Fact]
    public void RectArea_Should_ReturnValidArea()
    {
        var side1 = _fixture.Create<float>();
        var side2 = _fixture.Create<float>();
        
        var area = SimplePoly.GetAreaByPoints([new PointF(0, 0), new PointF(side1, 0), new PointF(side1, side2), new PointF(0, side2)]);
        area.Should().BeApproximately(side1 * side2, 1e-9f);
    }

    [Fact]
    public void GetAreaByPoints_ForPolyLikeCircle_ShouldReturnPi()
    {
        const int numSides = 1000;
        const double angleStep = 2 * Math.PI / numSides;

        var points = new PointF[numSides];
        for (var i = 0; i < numSides; i++)
        {
            var sinCos = Math.SinCos(angleStep * i);
            points[i] = new PointF((float)sinCos.Cos, (float)sinCos.Sin);
        }

        var area = SimplePoly.GetAreaByPoints(points);
        area.Should().BeApproximately(float.Pi, 1e-5f);
    }
}