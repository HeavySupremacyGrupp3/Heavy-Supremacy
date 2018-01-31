using System.Collections.Generic;

namespace Events
{
    public class StoryNode
    {
        public StoryNode()
        {
            Init();
        }

        public StoryNode(string _title, string _text, string _metalBonus, string _angstBonus, string _fameBonus, string _cashBonus, string _energyBonus)
        {
            Title = _title;
            Text = _text;
            MetalBonus = _metalBonus;
            AngstBonus = _angstBonus;
            FameBonus = _fameBonus;
            CashBonus = _cashBonus;
            EnergyBonus = _energyBonus;

            Init();
        }

        private void Init()
        {
            Choices = new List<StoryNode>();
        }

        public string Title, Text, MetalBonus, AngstBonus, FameBonus, CashBonus, EnergyBonus;
        public List<StoryNode> Choices;
    }
}
