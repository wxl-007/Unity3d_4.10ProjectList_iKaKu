using UnityEngine;
using System.Collections;

public class SoundSystem : MonoBehaviour {
    public delegate void PlaySound(ExploseType type);
    public static PlaySound Play;

    void Awake() {
        Play = _Play;
    }

    void _Play(ExploseType type) { }
}
