using Reggborn.Music;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public void IncreaseVolume()
    {
        if (MusicManager.Instance.GetMusicVolume() > .99f) return;
        MusicManager.Instance.IncreaseVolume(.05f);
    }

    public void DecreaseVolume()
    {
        if (MusicManager.Instance.GetMusicVolume() < .01f) return;
        MusicManager.Instance.DecreaseVolume(.05f);
    }

}