using System;
using Microsoft.Xna.Framework;

namespace Pong;

class Utils
{
    public static float NormalizeAngleDeg(float angle)
    {
        return (angle % 360f + 360f) % 360f;
    }
    
    /// <summary>
    /// Are rectangle A & B colliding
    /// </summary>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    /// <returns>returns whether the objects collided and the offset from the objects to the collision point</returns>
    public static (bool collided, Point obj1Offset, Point obj2Offset) IsColliding(Rectangle obj1, Rectangle obj2)
    {
        return IsCollidingPriv(obj1, obj2);
    }

    private static (bool collided, Point obj1Offset, Point obj2Offset) IsCollidingPriv(Rectangle obj1, Rectangle obj2)
    {
        bool collided =
            obj1.Location.X < obj2.Location.X + obj2.Size.X &&
            obj1.Location.X + obj1.Size.X > obj2.Location.X &&
            obj1.Location.Y < obj2.Location.Y + obj2.Size.Y &&
            obj1.Location.Y + obj1.Size.Y > obj2.Location.Y;

        int collisionLeft = MathHelper.Max(obj1.Location.X, obj2.Location.X);
        int collisionTop = MathHelper.Max(obj1.Location.Y, obj2.Location.Y);

        Point obj1Offset = collided
            ? new(collisionLeft - obj1.Location.X, collisionTop - obj1.Location.Y)
            : Point.Zero;
        Point obj2Offset = collided
            ? new(collisionLeft - obj2.Location.X, collisionTop - obj2.Location.Y)
            : Point.Zero;

        return (collided, obj1Offset, obj2Offset);
    }

    /// <summary>
    /// Return the close (near 0, 0) and far (near screen width, height) side border area. Useful when there are more than 2 paddles active
    /// </summary>
    public static (Rectangle closeBorder, Rectangle farBorder) GetSideBorders(Point windowSize)
    {
        Point sideBorderSize = GetSideBorderSize(windowSize).borderSize;
        return (
            new(new(0, 0), sideBorderSize),
            new(windowSize - sideBorderSize, sideBorderSize)
        );
    }

    /// <summary>
    /// Return the size of a single border area and whether this border area should be on the sides or top or bottom
    /// </summary>
    public static (Point borderSize, bool IsWidthBorder) GetSideBorderSize(Point windowSize)
    {
        if (windowSize.X > windowSize.Y)
            return (
                new((windowSize.X - windowSize.Y) / 2, windowSize.Y),
                true
            );
        else
            return (
                new(windowSize.X, (windowSize.Y - windowSize.X) / 2),
                false
            );
    }

    public static Point GetScaledPaddleSize(Point windowSize)
    {
        return new(
            Globals.basePaddleSize.X * windowSize.X / Globals.ViewportSizeMultiplier.X,
            Globals.basePaddleSize.Y * windowSize.Y / Globals.ViewportSizeMultiplier.Y
        );
    }

    public static Point GetScaledBallSize(Point windowSize)
    {
        return new(
            Globals.baseBallSize * windowSize.X / Globals.ViewportSizeMultiplier.X,
            Globals.baseBallSize * windowSize.Y / Globals.ViewportSizeMultiplier.Y
        );
    }
}