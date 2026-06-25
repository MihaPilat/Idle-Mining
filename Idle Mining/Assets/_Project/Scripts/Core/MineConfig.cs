using UnityEngine;

namespace Project.Configs
{
    [CreateAssetMenu(fileName = "NewMineConfig", menuName = "Configs/Mine Config")]
    public class MineConfig : ScriptableObject
    {
        [SerializeField] private string _name= "Øàõòà";
        [SerializeField] private double _baseCost = 50;
        [SerializeField] private double _baseIncome = 2;
        [SerializeField] private float _costMultiplier = 1.15f;

        public string Name => _name;
        public double BaseCost => _baseCost;
        public double BaseIncome => _baseIncome;
        public float CostMultiplier => _costMultiplier;
    }
}
