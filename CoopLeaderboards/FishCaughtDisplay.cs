using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System;

public class FishCaughtDisplay : IClickableMenu
{
    private string message;
    private uint displayDuration;
    private uint elapsedTime;

    public FishCaughtDisplay(string message, uint displayDuration)
    {
        this.message = message;
        this.displayDuration = displayDuration;
        this.elapsedTime = 0;
    }

    public override void draw(SpriteBatch b)
    {
        base.draw(b);

        if (this.elapsedTime < this.displayDuration)
        {
            SpriteFont font = Game1.dialogueFont;
            Vector2 textSize = font.MeasureString(this.message);
            Vector2 position = new Vector2((Game1.viewport.Width - textSize.X) / 2, (Game1.viewport.Height - textSize.Y) / 2);

            b.DrawString(font, this.message, position, Color.White);
        }
    }

    public void Update(uint milliseconds)
    {
        this.elapsedTime += milliseconds;
    }

    public bool IsDone()
    {
        if (this.elapsedTime > this.displayDuration)
        {
            return true;
        }
        return false;
    }
}

