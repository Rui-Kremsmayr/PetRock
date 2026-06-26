using Unity.Cinemachine;
using UnityEngine;

public class CinemachineManager : MonoBehaviour
{
    
    [SerializeField] CinemachineSplineDolly _beginningSpline;
    [SerializeField] CinemachineCamera _topCam;
    [SerializeField] CinemachineCamera _toyCam;


    void OnEnable()
    {
        PetRock.OnWhistled += ToyCamera;
    }

    void OnDisable()
    {
        PetRock.OnWhistled -= ToyCamera;
    }


    void Update()
    {
        if (_beginningSpline.SplineSettings.Position >= 1)
            _beginningSpline.gameObject.SetActive(false);
    }


    void ToyCamera(bool showToys) => _toyCam.gameObject.SetActive(showToys);

}
