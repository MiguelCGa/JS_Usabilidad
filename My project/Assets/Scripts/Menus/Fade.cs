using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageFade : MonoBehaviour
{

    public Image img;

    void Start()
    {
        StartCoroutine(FadeImage(true));
    }

    IEnumerator FadeImage(bool fadeAway)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 4; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
        img.gameObject.SetActive(false);
    }
}