using UnityEngine;
using VContainer;
using VContainer.Unity;
using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace CardsAndDices
{
    public class GameInitializer : MonoBehaviour
    {
        [Header("Object Pools")]
        [SerializeField] private List<BaseSpriteView> _baseSpriteViews = new List<BaseSpriteView>();
        private void Awake()
        {
            foreach (var baseSpriteView in _baseSpriteViews)
            {
                baseSpriteView.OnAwake();
            }
        }
    }
}
