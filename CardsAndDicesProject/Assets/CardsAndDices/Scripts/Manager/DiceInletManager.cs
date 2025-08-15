using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardsAndDices
{
    [CreateAssetMenu(fileName = "DiceInletManager", menuName = "CardsAndDices/Systems/DiceInletManager")]
    public class DiceInletManager : ScriptableObject
    {
        public void Initialize()
        {
        }
/*
        [SerializeField] private List<DiceInletData> _diceInlets = new();
        private Dictionary<CompositeObjectId, DiceInletData> _diceInletsById;


        public DiceInletData GetInlet(CompositeObjectId id)
        {
            return _diceInletsById.GetValueOrDefault(id);
        }

        public IEnumerable<DiceInletData> GetAllInlets()
        {
            return _diceInlets.ToList();
        }
*/
    }
}