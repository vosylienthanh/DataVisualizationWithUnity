using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OnMouseClickSphere : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;

    private void OnMouseDown()
    {
        textMeshPro.text = this.transform.name;
    }
}
