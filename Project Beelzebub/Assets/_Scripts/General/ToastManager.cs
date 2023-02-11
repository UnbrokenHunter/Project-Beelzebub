using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectBeelzebub
{
    public class ToastManager : MonoBehaviour
    {

        [SerializeField] private GameObject toastPrefab;

        public void StartToast(InventoryItem item)
        {
            GameObject toast = Instantiate(toastPrefab, Vector3.zero, Quaternion.identity, transform);
            toast.GetComponent<PopupScript>().sprite = item.sprite;
        }


    }
}
