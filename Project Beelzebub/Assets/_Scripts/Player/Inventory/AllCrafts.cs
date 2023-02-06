using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectBeelzebub
{
    public class AllCrafts : MonoBehaviour
    {
        private List<GameObject> gameObjects = new();
        private Vector2 size = new Vector2(414, 38);

        [SerializeField]
        private Craft crafts;

        [SerializeField] private GameObject craftPrefab;

        [Button]
        public void SetCraft()
        {
            GameObject g = Instantiate(craftPrefab, Vector3.zero, Quaternion.identity, this.transform);
            gameObjects.Add(g);

            g.GetComponent<SetCraft>().SetCraftInfo(crafts);
            g.GetComponent<RectTransform>().sizeDelta= size;

            
        }


        [System.Serializable]
        public class Craft
        {
            [BoxGroup("$name")]
            [SerializeField] 
            private string name;

            [BoxGroup("$name/Craft Outcome")]
            public InventoryItem outcome;


            [BoxGroup("$name/Material One")]
            public InventoryItem mat1;
            [BoxGroup("$name/Material One")]
            public int mat1Amount;

            [BoxGroup("$name/Material Two")]
            public InventoryItem mat2;
            [BoxGroup("$name/Material Two")]
            public int mat2Amount;

            [BoxGroup("$name/Material Three")]
            public InventoryItem mat3;
            [BoxGroup("$name/Material Three")]
            public int mat3Amount;

        }
    }
}
