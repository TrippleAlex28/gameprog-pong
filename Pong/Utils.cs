using System;
using System.Drawing;

namespace Pong;

class Utils
{
    /// <summary>
    /// Are objects colliding
    /// </summary>
    public static bool IsColliding(Rectangle obj1, Rectangle obj2)
    {
        return IsCollidingPriv(obj1, obj2);
    }

    /// <summary>
    /// Are objects colliding and execute function if so
    /// </summary>
    public static bool IsColliding(Rectangle obj1, Rectangle obj2, Action func)
    {
        bool collided = IsCollidingPriv(obj1, obj2);
        if (collided) func();
        return collided;
    }

    private static bool IsCollidingPriv(Rectangle obj1, Rectangle obj2)
    {
        return (
            obj1.Location.X < obj2.Location.X + obj2.Size.Width &&
            obj1.Location.X + obj1.Size.Width > obj2.Location.X &&
            obj1.Location.Y < obj2.Location.Y + obj2.Size.Height &&
            obj1.Location.Y + obj1.Size.Height > obj2.Location.Y
        );
    }
}

/*

return (
    rect1.x < rect2.x + rect2.width &&
    rect1.x + rect1.width > rect2.x &&
    rect1.y < rect2.y + rect2.height &&
    rect1.y + rect1.height > rect2.y
  );

*/