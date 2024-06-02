using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;
using System.Linq;

public struct Message
{
    public Message(string text, uint displayDuration, SpriteFont font, Color color)
    {
        this.Text = text;
        this.DisplayDuration = displayDuration;
        this.ElapsedTime = 0;
        this.Font = font;
        this.Color = color;
    }
    public string Text { get; set; }
    public uint DisplayDuration { get; set; }
    public uint ElapsedTime { get; set; }
    public SpriteFont Font { get; set; }
    public Color Color { get; set; }
}

public class TextWriter : IClickableMenu
{
    private readonly List<Message> messages;

    public TextWriter()
    {
        messages = new List<Message>();
    }

    public void WriteToScreen(string text, uint displayDuration, SpriteFont font, Color color)
    {
        Message message = new Message(text, displayDuration, font, color);
        messages.Add(message);
    }

    public override void draw(SpriteBatch b)
    {
        base.draw(b);

        foreach (var message in messages)
        {
            if (message.ElapsedTime < message.DisplayDuration)
            {
                Vector2 textSize = message.Font.MeasureString(message.Text);
                Vector2 position = new Vector2((Game1.viewport.Width - textSize.X) / 2, (Game1.viewport.Height - textSize.Y) / 2);

                b.DrawString(message.Font, message.Text, position, message.Color);
            }
        }
    }

    public void Update(uint milliseconds)
    {
        for (int i = 0; i < messages.Count; i++)
        {
            Message message;
        }
        messages.RemoveAll(m => m.ElapsedTime > m.DisplayDuration);
    }
}

