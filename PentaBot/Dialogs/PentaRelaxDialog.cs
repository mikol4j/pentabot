using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using PentaBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Threading.Tasks;

namespace PentaBot.Dialogs
{
    public class PentaRelaxDialog
    {
        public static readonly IDialog<string> dialog = Chain.PostToChain()
            .Select(msg => msg.Text)
            .Switch(
            new RegexCase<IDialog<string>>(new System.Text.RegularExpressions.Regex("^hi", RegexOptions.IgnoreCase), (ctx, txt) =>
             {
                 return Chain.ContinueWith(new GreetingDialog(), AfterGreetingContinuation);
             }),
            new DefaultCase<string, IDialog<string>>((ctx, txt) =>
             {
                 return Chain.ContinueWith(FormDialog.FromForm(RoomReservation.BuildForm, FormOptions.PromptInStart), AfterGreetingContinuation);
             }
             )).Unwrap().PostToUser();

        private async static Task<IDialog<string>> AfterGreetingContinuation(IBotContext context, IAwaitable<object> item)
        {
            var token = await item;
            var name = "User";
            context.UserData.TryGetValue<string>("Name", out name);
            return Chain.Return($"Thank you for using the PentaRelax bot: {name}");
        }
    }
}