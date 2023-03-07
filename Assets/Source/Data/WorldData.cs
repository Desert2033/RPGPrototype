using System;

namespace Source.Data
{
    [Serializable]
    public class WorldData
    {
        private string intialLevel;
        
        public PositionOnLevel PositionOnLevel;
        public LootData LootData;

        public WorldData(string intialLevel)
        {
            PositionOnLevel = new PositionOnLevel(intialLevel);
            LootData = new LootData();
        }
    }
}