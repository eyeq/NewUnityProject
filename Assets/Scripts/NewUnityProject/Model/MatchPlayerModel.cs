using System.Collections.Generic;

namespace NewUnityProject.Model
{
    public struct MatchPlayerModel
    {
        public int Number { get; set; }
        public string PlayerId { get; set; }
        public List<int> Deck { get; set; }
        public List<int> Field { get; set; }
        public List<int> Hand { get; set; }
    }
}