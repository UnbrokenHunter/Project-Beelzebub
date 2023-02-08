using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectBeelzebub
{
    public class AllCrafts : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> gameObjects = new();
        private Vector2 size = new Vector2(414, 38);

        [SerializeField, TableList]
        private List<CraftableItem> items = new List<CraftableItem>();

        [SerializeField] private GameObject craftPrefab;

        public List<CraftableItem> GetItems() => items;
        public List<GameObject> GetItemsObjs() => gameObjects;

        private void Awake() => SetCraft();

        [Button]
        public void SetCraft()
        {

            foreach(GameObject g in gameObjects)
            {
                DestroyImmediate(g);
            }

            gameObjects.Clear();


            foreach (CraftableItem item in items) {
               
                GameObject g = Instantiate(craftPrefab, Vector3.zero, Quaternion.identity, this.transform);
                gameObjects.Add(g);

                g.GetComponent<SetCraft>().SetCraftInfo(item);
                g.GetComponent<RectTransform>().sizeDelta = size;

            }   
        }

    }
}
