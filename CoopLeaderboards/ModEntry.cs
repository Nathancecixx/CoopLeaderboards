using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Tools;
using System.Collections.Generic;


public class FishingCompetitionMod : Mod
{
    private FishCaughtDisplay fishCaughtDisplay = null;
    private List<Item> previousInventory;

    Leaderboard Leaderboard;

    public override void Entry(IModHelper helper)
    {
        Leaderboard = new Leaderboard(helper);

        helper.Events.GameLoop.DayStarted += this.OnDayStarted;
        helper.Events.Display.RenderedHud += this.OnRenderedHud;
        helper.Events.Input.ButtonPressed += this.OnButtonPressed;
        helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
        helper.Events.Multiplayer.ModMessageReceived += this.OnMessageRecieved;
    }

    private void OnDayStarted(object sender, DayStartedEventArgs e)
    {
        Monitor.Log("The fishing competition is on! Catch as many fish as you can!", LogLevel.Info);
        this.fishCaughtDisplay = new FishCaughtDisplay("The fishing competition is on! Catch as many fish as you can!", 300000);
        previousInventory = new List<Item>(Game1.player.Items);
    }

    private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
    {
        
    }


    private void OnRenderedHud(object sender, RenderedHudEventArgs e)
    {
        this.Leaderboard.draw(e.SpriteBatch);
        this.fishCaughtDisplay?.draw(e.SpriteBatch);
    }

    private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
    {
        if (this.fishCaughtDisplay != null)
        {
            this.fishCaughtDisplay.Update(e.Ticks);
            if (this.fishCaughtDisplay.IsDone() )
            {
                this.fishCaughtDisplay = null;
            }
        }

        CheckForCaughtFish();
    }

    private void CheckForCaughtFish()
    {
        foreach (Item item in Game1.player.Items)
        {
            if (item is StardewValley.Object obj && obj.Category == StardewValley.Object.FishCategory)
            {
                if (!this.previousInventory.Contains(item))
                {
                    Monitor.Log("Fish caught!", LogLevel.Alert);
                    this.fishCaughtDisplay = new FishCaughtDisplay("Fish caught!", 30000);
                    this.Leaderboard.UpdateLeaderboard(Game1.player.Name, 1);
                    break;
                }
            }

        }

        this.previousInventory = new List<Item>(Game1.player.Items);
    }

    private void OnMessageRecieved(object sender, ModMessageReceivedEventArgs e)
    {
        if(e.Type == "LeaderboardUpdate")
        {
            var message = e.ReadAs<DTP>();
            Leaderboard.RecieveUpdate(message);
        }
    }
}




