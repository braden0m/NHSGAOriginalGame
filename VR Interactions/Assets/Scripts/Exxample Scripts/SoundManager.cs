using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource au_mixedfire;
    private AudioSource au_fullsong;
    [SerializeField] private Dictionary<string, AudioClip> audioClipDictionary = new Dictionary<string, AudioClip>();

    [SerializeField] private GameObject audioNodePrefab;

    private void Awake()
    {
        audioClipDictionary = new Dictionary<string, AudioClip>();

        foreach (GameObject thing in GameObject.FindGameObjectsWithTag("Breakable"))
        {
            AudioSource targetSource = thing.GetComponent<AudioSource>();

            if (targetSource != null)
            {
                if (!audioClipDictionary.ContainsKey(thing.name))
                {
                    audioClipDictionary.Add(thing.name, targetSource.clip);
                }

                Destroy(targetSource);
            }
        }

        Debug.Log(audioClipDictionary);
    }

    // Start is called before the first frame update
    void Start()
    {

        au_mixedfire = GetComponentsInChildren<AudioSource>()[0];
        au_fullsong = GetComponentsInChildren<AudioSource>()[1];

        if (au_mixedfire == null)
            Debug.LogWarning("can't find");
        else
        {
            Debug.LogWarning("good");
        }
            au_mixedfire.Play();

        au_fullsong.Play();

    }

    public void PlaySound(GameObject destroyedObject)
    {
        if (audioClipDictionary.ContainsKey(destroyedObject.name))
        {
            GameObject newNode = Instantiate(audioNodePrefab, destroyedObject.transform.position, Quaternion.identity);
            AudioSource newAudioSource = newNode.GetComponent<AudioSource>();

            newAudioSource.clip = audioClipDictionary[destroyedObject.name];
            newAudioSource.Play();

            StartCoroutine(DestroyWait(newNode, audioClipDictionary[destroyedObject.name].length));
        }
    }

    IEnumerator DestroyWait(GameObject audioNode, float waitTime)
    {
        yield return new WaitForSeconds(waitTime + 1);
        Destroy(audioNode);
    }
}
