using UnityEngine;

namespace Assets.Scripts.Object
{
    [CreateAssetMenu(fileName = "PushableItemData", menuName = "Game/Pushable Item")]
    public class PushableItemData : ScriptableObject
    {
        public int value = 1;
    }
}