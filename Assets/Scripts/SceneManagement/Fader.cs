using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader: MonoBehaviour
    {
        private CanvasGroup canvasGroup;
        
        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha  >  0)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
        
        public IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha  <  1)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

    }
}