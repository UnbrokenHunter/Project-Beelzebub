using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectBeelzebub
{
    public class ToastManager : MonoBehaviour
    {

        [SerializeField] private GameObject toastPrefab;
        [SerializeField] private List<GameObject> toasts;


        public void StartToast(InventoryItem item)
        {
            if (HasItem(item) == null) {
                GameObject toast = Instantiate(toastPrefab, Vector3.zero, Quaternion.identity, transform);
                toast.GetComponent<PopupScript>().sprite = item.sprite;
                toast.name = item.name;
                toast.GetComponent<PopupScript>().manager = this;
                toasts.Add(toast);
            }
            else
            {
                PopupScript popup = HasItem(item).GetComponent<PopupScript>();
                popup.AddCount();
                popup.ResetTimer();
            }

        }

        public void RemoveItem(GameObject obj)
        {
            if(toasts.Remove(obj))
            {
                toasts.Remove(obj);
            }
        }

        private GameObject HasItem(InventoryItem item)
        {
            foreach(GameObject toast in toasts)
            {
                if(toast.name == item.name)
                {
                    return toast;
                }
            }
            return null;
        }

    }
}
