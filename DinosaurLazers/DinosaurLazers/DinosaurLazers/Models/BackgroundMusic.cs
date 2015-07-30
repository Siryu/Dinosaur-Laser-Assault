using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DinosaurLazers.Models
{
    public class BackgroundMusic
    {
        Song Song;

        public enum playAt
        {
            CharacterSelectScreen,
            Level1,
            Level1Boss,
            Level2,
            Level2Boss,
            Level3,
            Level3Boss,
            Level4,
            Level4Boss,
            Level5,
            Level5Boss,
            Opening,
            GameOver,
            Pause,
            BeatBoss
        }

        public BackgroundMusic(ContentManager Content, playAt at)
        {
            switch (at)
            {
                case playAt.Level1:
                    Song = Content.Load<Song>("Sounds/Music/Level1BGMusic");
                    break;
                case playAt.Level1Boss:
                    Song = Content.Load<Song>("Sounds/Music/Level1BossBGMusic");
                    break;
                case playAt.Level2:
                    Song = (Song)Content.Load<Song>("Sounds/Music/Level2BGMusic");
                    break;
                case playAt.Level2Boss:
                    Song = Content.Load<Song>("Sounds/Music/Level2BossBGMusic");
                    break;
                case playAt.Level3:
                    Song = Content.Load<Song>("Sounds/Music/Level3BGMusic");
                    break;
                case playAt.Level3Boss:
                    Song = Content.Load<Song>("Sounds/Music/Level3BossBGMusic");
                    break;
                case playAt.Level4:
                    Song = Content.Load<Song>("Sounds/Music/Level4BGMusic");
                    break;
                case playAt.Level4Boss:
                    Song = Content.Load<Song>("Sounds/Music/Level4BossBGMusic");
                    break;
                case playAt.Level5:
                    Song = Content.Load<Song>("Sounds/Music/Level5BGMusic");
                    break;
                case playAt.Level5Boss:
                    Song = Content.Load<Song>("Sounds/Music/Level5BossBGMusic");
                    break;
                case playAt.CharacterSelectScreen:
                    Song = Content.Load<Song>("Sounds/Music/CharacterSelectBGMusic");
                    break;
                case playAt.Opening:
                    Song = Content.Load<Song>("Sounds/Music/TitleScreenBGMusic");
                    break;
                case playAt.BeatBoss:
                    Song = Content.Load<Song>("Sounds/Music/BeatBossBGMusic");
                    break;
            }
        }

        public void Play()
        {
            MediaPlayer.Play(Song);
            MediaPlayer.IsRepeating = true;
        }

        public void Stop()
        {
            MediaPlayer.Stop();
        }

        public void QuietVolume()
        {
            MediaPlayer.Volume = 0.5f;
        }

        public void NormalVolume()
        {
            MediaPlayer.Volume = 1f;
        }
    }
}
