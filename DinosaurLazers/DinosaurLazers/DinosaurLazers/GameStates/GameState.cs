using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DinosaurLazers.GameStates
{
    public enum GameState
    {
        TitleScreen,
        CharacterSelection,
        MultiplayerLobby,
        StartLevel,
        LoadGame,
        LoadingScreen,
        PlayingLevel,
        Pause,
        EndOfLevel,
        Death,
        Credits
    }
}
