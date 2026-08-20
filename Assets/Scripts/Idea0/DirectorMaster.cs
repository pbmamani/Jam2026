using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DirectorMaster : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public PlayableDirector playableDirector;
    public static DirectorMaster instance;
    [SerializeField] GameObject newCharacter;
    [SerializeField] GameObject ContainerPlayer;

    [SerializeField] List<PlayableAsset> playables = new List<PlayableAsset>();
    private void Awake()
    {
        instance = this;
        playableDirector = GetComponent<PlayableDirector>();
        playableDirector.playableAsset = playables[0];
    }

    public void PauseAction(Vector3 pos) {
        if (playableDirector != null) { 
            //playableDirector.Pause();
            playableDirector.playableAsset = playables[1];

            foreach (var track in ((TimelineAsset)playables[1]).GetOutputTracks())
            {
                if (track.name == "Animation Track")
                {
                    playableDirector.SetGenericBinding(track, newCharacter);
                }
            }
            ContainerPlayer.transform.position = pos;
            playableDirector.time = 0;
            playableDirector.Play();
        }
    }

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
