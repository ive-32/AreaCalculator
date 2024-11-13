using System.Data;
using System.Drawing;
using AreaCalculator;
using AutoFixture;
using FluentAssertions;

namespace AreaCalculatorTests;

public class RegularPolyTests
{
    private readonly Fixture _fixture = new Fixture();
    
    [Fact]
    public void TriangleArea_ForRightTriangle_Should_ReturnValidArea()
    {
        var sides = GetRightTriangle();
        
        var area = RegularPoly.GetAreaBySides(sides);
        area.Should().BeApproximately(sides[0] * sides[1] / 2f, sides.Max() * 1e-4f);
    }
    
    [Fact]
    public void CircleAreaWithRadiusOne_Should_ReturnPi()
    {
        var area = RegularPoly.GetAreaBySides([1]);
        area.Should().BeApproximately(3.1415926535f, 1e-9f);
    }
    
    [Fact]
    public void SquareArea_Should_ReturnValidArea()
    {
        var side1 = _fixture.Create<float>();
        var side2 = _fixture.Create<float>();
        
        var area = RegularPoly.GetAreaBySides([side1, side2, side1, side2]);
        area.Should().BeApproximately(side1 * side2, 1e-9f);
    }

    [Fact]
    public void TriangleAreaBySides_Should_BeEqualToAreaByGauss()
    {
        var point1 = _fixture.Create<PointF>();
        var point2 = _fixture.Create<PointF>();
        var point3 = _fixture.Create<PointF>();
        var side1 = GetDistance(point1, point2);
        var side2 = GetDistance(point2, point3);
        var side3 = GetDistance(point3, point1);
        
        var areaBySides = RegularPoly.GetAreaBySides([side1, side2, side3]);
        var areaByPoints = SimplePoly.GetAreaByPoints([point1, point2, point3]);
        areaBySides.Should().BeApproximately(areaByPoints, areaByPoints * 1e-6f);
    }

    [Fact]
    public void TriangleArea_ForWrongSides_ShouldThrow()
    {
        var side1 = _fixture.Create<float>();
        var side2 = _fixture.Create<float>();
        var side3 = Math.Abs(side1 - side2) * 0.8f;

        var exception = Assert.Throws<ConstraintException>(
            () => RegularPoly.GetAreaBySides([side1, side2, side3]));

        exception.Message.Should().Be("Area for polygon with this sides can't be calculated");
    }
    
    [Theory]
    [InlineData(7)]
    [InlineData(5)]
    public void GetAreaBySides_ForWrongSides_ShouldThrow(int numSides)
    {
        var sides = _fixture.CreateMany<float>(numSides);

        var exception = Assert.Throws<ConstraintException>(
            () => RegularPoly.GetAreaBySides(sides.ToArray()));

        exception.Message.Should().Be("Area for polygon with this sides can't be calculated");
    }

    [Fact]
    public void IsRightTriangle_ShouldReturnTrue()
    {
        var sides = GetRightTriangle();
        var isRight = RegularPoly.IsRightTriangle(sides);

        isRight.Should().BeTrue();
    }

    [Theory]
    [InlineData(new [] { 1, 1.4142135f, 1 })]
    [InlineData(new [] { 1.4142135f, 1, 1 })]
    [InlineData(new [] { 1, 1f, 1.4142135f })]
    public void IsRightTriangle_ShouldReturnTrue_ForAnyPositionOfSides(float[] sides)
    {
        var isRight = RegularPoly.IsRightTriangle(sides);

        isRight.Should().BeTrue();
    }

    [Fact]
    public void IsRightTriangle_ForNonRight_ShouldReturnFalse()
    {
        var sides = GetRightTriangle();
        sides[2] -= sides[2] * 0.1f;
        var isRight = RegularPoly.IsRightTriangle(sides);

        isRight.Should().BeFalse();
    }
    
    private float[] GetRightTriangle()
    {
        var side1 = _fixture.Create<float>();
        var side2 = _fixture.Create<float>();
        var side3 = MathF.Sqrt(side1 * side1 + side2 * side2);

        return [side1, side2, side3];
    }

    private static float GetDistance(PointF point1, PointF point2)
    {
        var deltaX = point2.X - point1.X;
        var deltaY = point2.Y - point1.Y;
        return MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }
}