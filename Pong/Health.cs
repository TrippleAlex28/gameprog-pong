using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong;

public class Health
{
    public static void DrawPaddleHearts(SpriteBatch spriteBatch, Viewport viewport, ushort[] healths)
    {
        for (int i = 0; i < healths.Length && i < (Globals.gameType == GameType.TwoPlayer ? 2 : 4); i++)
        {
            Vector2 drawStartPos = new();
            sbyte drawDirection = 0;

            switch (i)
            {
                case 0:
                    drawStartPos = new(Globals.heartEdgePadding, Globals.heartEdgePadding);
                    drawDirection = 1;
                    break;
                case 1:
                    drawStartPos = new(viewport.Width - Globals.heartDrawSize - Globals.heartEdgePadding, Globals.heartEdgePadding);
                    drawDirection = -1;
                    break;
                case 2:
                    drawStartPos = new(Globals.heartEdgePadding, viewport.Height - Globals.heartDrawSize - Globals.heartEdgePadding);
                    drawDirection = 1;
                    break;
                case 3:
                    drawStartPos = new(viewport.Width - Globals.heartDrawSize - Globals.heartEdgePadding, viewport.Height - Globals.heartDrawSize - Globals.heartEdgePadding);
                    drawDirection = -1;
                    break;
                default:
                    Console.WriteLine("DrawPaddleHearts(): unhandled i in switch case");
                    break;
            }

            for (int j = 0; j < healths[i]; j++)
            {
                Point drawPos = new(
                    (int)(drawStartPos.X + drawDirection * (j * (Globals.heartDrawGap + Globals.heartDrawSize))),
                    (int)drawStartPos.Y
                );
                spriteBatch.Draw(
                    Globals.heartTexture,
                    new Rectangle(drawPos, new(Globals.heartDrawSize)),
                    Globals.playerColors[i]
                );
            }
        }
    }

}