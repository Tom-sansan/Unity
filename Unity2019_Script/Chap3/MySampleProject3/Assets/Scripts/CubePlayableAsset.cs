using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class CubePlayableAsset : PlayableAsset
{
    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        CubePlayableBehaviour behaviour = new CubePlayableBehaviour();
        behaviour.cubeObject = go;
        ScriptPlayable<CubePlayableBehaviour> playable = ScriptPlayable<CubePlayableBehaviour>.Create(graph, behaviour);
        return playable;
    }
}
