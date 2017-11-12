using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using PentaBot.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PentaBot.Dialogs
{
    [LuisModel("17c14ef1-e684-4494-aa94-4fb4d7e52bc7", "1bee2d14828c4a4c80078d5e29f7b310")]
    [Serializable]
    public class LUISDialog : LuisDialog<object>
    {
        private const string LastPentaKopEntryKeyName = "LastPentaKopEntry";
        public LUISDialog()
        {

        }

        [LuisIntent("")]
        public async Task None(IDialogContext ctx, LuisResult result)
        {
            await ctx.PostAsync("I'm sorry I don't know what you mean.");
            ctx.Wait(MessageReceived);
        }
        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext ctx, LuisResult result)
        {
            await ctx.PostAsync("What can I do for you friend?", InputHints.ExpectingInput);
            ctx.Wait(MessageReceived);
        }

        private async Task Callback(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceived);
        }

        [LuisIntent("PentaKOPNewEntry")]
        public async Task PentaKOPNewEntry(IDialogContext ctx, IAwaitable<IMessageActivity> messageAwaitable, LuisResult result)
        {
            if (ctx.UserData.TryGetValue(LastPentaKopEntryKeyName, out string projectName))
            {
                PromptDialog.Confirm(ctx, ConfirmLastPentaKopEntryAsync, $"Would you like to log your usual {projectName}?");
            }
            else
            {
                 ctx.Call(PentaKOPNewEntryDialog.CreateDialog(1, "", ""), PentaKOPNewEntryDoneAsync);
                //ctx.Wait(MessageReceived); can't handle
            }
        }
        private async Task PentaKOPNewEntryDoneAsync(IDialogContext context, IAwaitable<PentaKOPNewEntry> entry)
        {
            var pentaKOPNewEntry = await entry;
            await context.SayAsync($"Your have worked for {pentaKOPNewEntry.Project} for {pentaKOPNewEntry.NumberOfHours} hours. You have chosed activity {pentaKOPNewEntry.ActivityType}, and your comment is {pentaKOPNewEntry.Comment}");
            context.UserData.SetValue(LastPentaKopEntryKeyName, pentaKOPNewEntry.Project);
            var kop = new PentaKOPService();
            await kop.AddActivity(pentaKOPNewEntry.Comment);
            await context.SayAsync("So what would you like to do next?", InputHints.ExpectingInput);
            context.Wait(MessageReceived);
        }

        private async Task ConfirmLastPentaKopEntryAsync(IDialogContext ctx, IAwaitable<bool> isConfirmed)
        {
            if (await isConfirmed)
            {
                var projectName = ctx.UserData.GetValue<string>(LastPentaKopEntryKeyName);
                ctx.Call(PentaKOPNewEntryDialog.CreateDialog(project:projectName), PentaKOPNewEntryDoneAsync);
            }
            else
            {
                await ctx.SayAsync("No problem. So how can I help you?", InputHints.ExpectingInput);
                ctx.Wait(MessageReceived);
            }
        }

    }


}