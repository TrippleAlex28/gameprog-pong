using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong;

public class Ball
{

    private Random _rng = new();
    private Vector2 _angle;

    public Vector2 Position { get; private set; }
    public float Velocity { get; set; }
    
    public Ball(Viewport viewport)
    {
        SetAngle(_rng.NextSingle() * 2f * float.Pi);
        Position = ToCenter(viewport, new Vector2(Globals.ballSize));
        Velocity = Globals.ballSpeedBase;
    }

    public void Update(GameTime gt)
    {
        Position += _angle * Velocity * (float) gt.ElapsedGameTime.TotalSeconds;
    }
    
    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(Globals.ballTexture, Position, Color.White);
    }

    /// <summary>
    /// Set angle of where ball moves
    /// </summary>
    public void SetAngle(float angle)
    {
        _angle = new Vector2(float.Cos(angle), -float.Sin(angle));
    }

    /// <summary>
    /// Mirror angle that ball moves. Used when ball collides with wall
    /// </summary>
    public void MirrorAngle(bool horizontal)
    {
        _angle = new Vector2(horizontal ? -_angle.X : _angle.X, horizontal ? _angle.Y : -_angle.Y);
    }

    private static Vector2 ToCenter(Viewport viewport, Vector2 thingSize)
    {
        return new Vector2(viewport.Height / 2f - thingSize.X / 2f,
            viewport.Width / 2f - thingSize.Y / 2f);
    }
}