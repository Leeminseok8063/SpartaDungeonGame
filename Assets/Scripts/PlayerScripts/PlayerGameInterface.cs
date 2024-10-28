using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PlayerScripts
{
    internal class PlayerGameInterface : MonoBehaviour
    {
        [Header("InteractObject")]
        public GameObject interactPanel;
        public Text ItemNameText;
        public Text ItemDescText;

        [Header("IconObject")]
        public GameObject heartObject;
        public GameObject energeObject;
        public GameObject shiledObject;

        [Header("LayoutGroup")]
        public GridLayoutGroup heartGridLayoutGroup;
        public GridLayoutGroup energeGridLayoutGroup;
        public GridLayoutGroup shiledGridLayoutGroup;

        public void UpdateGameInterface()
        {
            PlayerObject player = GameManager.Instance.Player;
            DestoryChild(heartGridLayoutGroup.transform);
            CreateChild(heartGridLayoutGroup.transform, player.health, heartObject);

            DestoryChild(energeGridLayoutGroup.transform);
            CreateChild(energeGridLayoutGroup.transform, player.energe, energeObject);

            DestoryChild(shiledGridLayoutGroup.transform);
            CreateChild(shiledGridLayoutGroup.transform, player.shield, shiledObject);
        }
        private void DestoryChild(Transform parent)
        {
            foreach (Transform child in parent)
            {
                Destroy(child.gameObject);
            }
        }
        private void CreateChild(Transform parent, int count, GameObject childObject)
        {
            for (int i = 0; i < count; i++)
            {
                Instantiate(childObject, parent);
            }
        }
    }
}
