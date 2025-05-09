using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnCommands : MonoBehaviour
{
    public VoiceOverView voiceOverView;

    public AudioSource npcAudio;

    [YarnCommand("change_audio_to_NPC")]
    public void NPCAudio()
    {
        voiceOverView.audioSource = npcAudio;
    }
}
