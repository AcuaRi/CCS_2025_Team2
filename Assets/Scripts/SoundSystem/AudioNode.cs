using System.Collections;
using UnityEngine;

public class AudioNode : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public void Play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);

        StartCoroutine("WaitSound");
    }

    private IEnumerator WaitSound()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);

        SoundManager.instance.SetNode(this);
    }

    public void SetVolumn(float percent)
    {
        audioSource.volume = percent;
    }
}
