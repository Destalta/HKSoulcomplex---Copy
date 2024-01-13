
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class FadeAudioSource
{

    public static IEnumerator PlayCoroutine(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}