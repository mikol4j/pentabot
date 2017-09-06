using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace PentaBot.Dialogs
{
    [Serializable]
    public class GreetingDialog : IDialog
    {

        public async Task StartAsync(IDialogContext ctx)
        {

            await Respond(ctx);
            ctx.Wait(MessageReceivedAsync);

        }
        private static async Task Respond(IDialogContext ctx)
        {
            var userName = String.Empty;
            ctx.UserData.TryGetValue<string>("Name", out userName);
            if (string.IsNullOrEmpty(userName))
            {
                userName = " friend,";
                await ctx.PostAsync("What is your name?");
                ctx.UserData.SetValue<bool>("GetName", true);
            }
            else
            {
                await ctx.PostAsync($"Hi {userName}. How can I help you today?");
            }
        }

        private async Task MessageReceivedAsync(IDialogContext ctx, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            var userName = String.Empty;
            var getName = false;
            ctx.UserData.TryGetValue<string>("Name", out userName);
            ctx.UserData.TryGetValue<bool>("Getname", out getName);
            if (getName)
            {
                ctx.UserData.SetValue<string>("Name", userName);
                ctx.UserData.SetValue<bool>("Getname", false);
            }
            await Respond(ctx);
            ctx.Done(message);

            ////var activity = await result as Activity;

            ////// calculate something for us to return
            ////int length = (activity.Text ?? string.Empty).Length;

            ////// return our reply to the user
            //////await context.PostAsync($"You sent {activity.Text} which was {length} characters");
            ////await context.PostAsync("Hello friend. It has been a while.");
            ////await context.PostAsync($"You sent {activity.Text} which was {length} characters");
            ////context.Wait(MessageReceivedAsync);
        }
    }
}