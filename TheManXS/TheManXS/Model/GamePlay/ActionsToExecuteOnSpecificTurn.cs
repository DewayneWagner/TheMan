using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using static TheManXS.Model.Financial.NextAction;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.GamePlay
{
    public class ActionsToExecuteOnSpecificTurn
    {
        Game _game;
        public ActionsToExecuteOnSpecificTurn() { }
        public ActionsToExecuteOnSpecificTurn(Game game, NextActionType nextAction, int turnIncrementForNextAction, int sqKey)
        {
            _game = game;
            NextActionType = nextAction;
            TurnToExecute = turnIncrementForNextAction;
            SQKey = sqKey;
            PlayerNumber = _game.ActivePlayer.Number;
            CurrentSavedGame = QC.CurrentSavedGameSlot;
        }
        public int TurnToExecute { get; set; }
        public NextActionType NextActionType { get; set; }
        public int PlayerNumber { get; set; }
        public int SQKey { get; set; }
        public int CurrentSavedGame { get; set; }
    }
    public class ActionsToExecuteOnSpecificTurnDBConfig : IEntityTypeConfiguration<ActionsToExecuteOnSpecificTurn>
    {
        public void Configure(EntityTypeBuilder<ActionsToExecuteOnSpecificTurn> builder)
        {
            builder.HasNoKey();

            builder.Property(a => a.NextActionType)
                .HasConversion(new EnumToStringConverter<NextActionType>());
        }
    }
}
