using System;

namespace Source.Data
{
    [Serializable]
    public class PositionOnLevel
    {
        public string Level;
        public Vector3Data Position;
        private string intialLevel;

        public PositionOnLevel(string intialLevel)
        {
            Level = intialLevel;
        }

        public PositionOnLevel(string level, Vector3Data position)
        {
            Level = level;
            Position = position;
        }
    }
}