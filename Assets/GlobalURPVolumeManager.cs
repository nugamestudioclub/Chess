using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlobalURPVolumeManager : MonoBehaviour
{
    public static GlobalURPVolumeManager Instance;

void Awake(){
    Instance = this;
}

[SerializeField] private Volume volumeComponent;
    [SerializeField] private VolumeProfile defaultVolume;
        [SerializeField] private VolumeProfile trippyVolume;


    public void SetProfileToDefault(){
        volumeComponent.profile = defaultVolume;
    }
    public void SetProfileToTrippy(){
        volumeComponent.profile = trippyVolume;
    }
    

}
