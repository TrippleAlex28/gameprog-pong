using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong;

public class Ball
{
    private Random _rng = new();
    private Vector2 _angle;
    private short _lastBouncePlayerId = -1;
    
    public Point DrawSize { get; private set; }
    public Vector2 Position { get; private set; }
    public float Velocity { get; set; }

    public Ball(Viewport viewport)
    {
        DrawSize = Utils.GetScaledBallSize(new(viewport.Width, viewport.Height));

        float angle = _rng.NextSingle() * 2f * float.Pi;
        while ((angle > 1 / 3 * float.Pi && angle < 2 / 3 * float.Pi) || (angle > 4 / 3 * float.Pi && angle < 5 / 3 * float.Pi))
            angle = _rng.NextSingle() * 2f * float.Pi;
            
        SetAngleDeg(angle);

        Position = ToCenter(viewport, new Vector2(Globals.baseBallSize));
        Velocity = Globals.ballSpeedBase;
    }

    public void Update(GameTime gt)
    {
        Position += _angle * Velocity * (float) gt.ElapsedGameTime.TotalSeconds;
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(
            Globals.ballTexture,
            new Rectangle(new((int)Position.X, (int)Position.Y), DrawSize),
            (_lastBouncePlayerId == -1 ? Color.White : Globals.playerColors[_lastBouncePlayerId])
        );
    }

    public void Bounce(short playerId, float newAngle)
    {
        if (playerId == _lastBouncePlayerId) return;

        SetAngleDeg(newAngle);
        Velocity += Globals.ballSpeedIncrement;
        _lastBouncePlayerId = playerId;
    }

    /// <summary>
    /// Set angle of where ball moves
    /// </summary>
    public void SetAngleDeg(float angle)
    {
        _angle = new Vector2(float.Cos(angle), -float.Sin(angle));
    }

    /// <summary>
    /// Get angle in degrees (from private angle Vector2)
    /// </summary>
    public float GetAngleDeg()
    {
        if (_angle.LengthSquared() == 0f) return 0f;

        float radians = float.Atan2(_angle.Y, _angle.X);
        float degrees = radians * 180f / float.Pi;

        return Utils.NormalizeAngleDeg(degrees);
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
        return new Vector2(viewport.Width / 2f - thingSize.X / 2f,
            viewport.Height / 2f - thingSize.Y / 2f);
    }
}