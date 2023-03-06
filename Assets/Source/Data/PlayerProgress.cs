using System;

namespace Source.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public State HeroState;
        public WorldData WorldData;
        public Stats HeroStats;
        public KillData KillData;

        public PlayerProgress(string intialLevel)
        {
            WorldData = new WorldData(intialLevel);
            HeroState = new State();
            HeroStats = new Stats();
            KillData = new KillData();
        }
    }
}