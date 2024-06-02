using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

//Data transfer packet used to send the other players an update
public struct DTP
{
    public string PlayerName {  get; set; }
    public int Score { get; set; }

    public DTP(string playerName, int score)
    {
        PlayerName = playerName;
        Score = score;
    }
}

public class Leaderboard : IClickableMenu
{
    private Dictionary<string, int> LeaderboardInfo;
    IModHelper ModHelper;

    public Leaderboard(IModHelper ModHelper) {
        this.ModHelper = ModHelper;
        LeaderboardInfo = new Dictionary<string, int>();
    }

    public override void draw(SpriteBatch b)
    {

        Vector2 BoxPosition = new Vector2(10, 10);

        float MaxWidth = 100;

        int LeaderboardSlots = 0;

        foreach (var entry in LeaderboardInfo)
        {
            string CurrentLine = $"{entry.Key}: {entry.Value}";

            SpriteFont font = Game1.dialogueFont;
            Vector2 CurrentLineWidth = font.MeasureString(CurrentLine);

            if(CurrentLineWidth.X > MaxWidth)
            {
                MaxWidth = CurrentLineWidth.X;
            }
            LeaderboardSlots += 50;
        }

        IClickableMenu.drawTextureBox(b, (int) BoxPosition.X, (int) BoxPosition.Y, (int) MaxWidth + 100, 100 + LeaderboardSlots, Color.White);

        //Display the leaderboard title
        string TitleText = "Leaderboard";
        SpriteFont TitleFont = Game1.smallFont;
        Vector2 TitleLength = TitleFont.MeasureString(TitleText);
        Vector2 TitlePosition = new Vector2(30, 30);

        b.DrawString(TitleFont, TitleText, TitlePosition, Color.Black);

        int HorizontalOffset = 30;
        int VerticalOffset = 100;

        foreach (var entry in LeaderboardInfo)
        {
            string CurrentLine = $"{entry.Key}: {entry.Value}";

            SpriteFont font = Game1.dialogueFont;
            Vector2 TextWidth = font.MeasureString(CurrentLine);

            Vector2 TextPosition = new Vector2(0 + HorizontalOffset, 0 + VerticalOffset);

            b.DrawString(font, CurrentLine, TextPosition, Color.Black);

            VerticalOffset += 50;
        }
    }

    public void RecieveUpdate(DTP message)
    {
        if (LeaderboardInfo.ContainsKey(message.PlayerName))
        {
            LeaderboardInfo[message.PlayerName] += message.Score;
        }
        else
        {
            LeaderboardInfo[message.PlayerName] = message.Score;
        }
    }

        public void UpdateLeaderboard(string PlayerName, int Increment)
    {
        //Update local leaderboard
        if (LeaderboardInfo.ContainsKey(PlayerName))
        {
            LeaderboardInfo[PlayerName] += Increment;
        }
        else
        {
            LeaderboardInfo[PlayerName] = Increment;
        }


        //Update Multiplayer leaderboard
        this.ModHelper.Multiplayer.SendMessage(new DTP(PlayerName, Increment),
            "LeaderboardUpdate",
            new [] {ModHelper.ModRegistry.ModID });
    }

}


