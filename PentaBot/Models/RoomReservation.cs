using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PentaBot.Models
{
    public enum Activity
    {
        FootballTable,
        Darts,
        Rest
    }

    [Serializable]
    public class RoomReservation
    {
        public Activity? activity;
        public int? numberOfGames;
        public DateTime dateFrom = DateTime.Now;
        public DateTime dateTo;
        public static IForm<RoomReservation> BuildForm()
        {
            return new FormBuilder<RoomReservation>()
                .Message("Welcome to the PentaRelax reservation bot!")
                .Build();
        }
    }
}