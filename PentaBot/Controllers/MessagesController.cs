using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using PentaBot.Dialogs;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Collections.Specialized;
using PentaBot.Infrastructure.Services;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Autofac;

namespace PentaBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Microsoft.Bot.Connector.Activity activity)
        {

            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new LUISDialog());
            }
            else
            {
                await HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }


        private static async Task HandleSystemMessage(Microsoft.Bot.Connector.Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, message))
                {
                    var client = scope.Resolve<IConnectorClient>();

                        var rpl = message.CreateReply();
                        foreach (var newMember in message.MembersAdded)
                        {
                            if (CheckIfMemberIsBot(message, newMember)) continue;

                        rpl.Text = $"Hi there! Welcome to the PentaBOT. How can I help you?";
                            await client.Conversations.ReplyToActivityAsync(rpl);
                        }
                    
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

        }
        private static bool CheckIfMemberIsBot(IActivity activity, ChannelAccount newMember)
        {
            return newMember.Id == activity.Recipient.Id;
        }
    }
}