using UnityEngine;
using UnityEngine.Serialization;

public class CameraOrbit : MonoBehaviour
{
    private Transform _xFormCamera;
    private Transform _xFormParent;

    private Vector3 _localRotation;
    private float _cameraDistance = 5f;

    public float mouseSensitivity = 4f;
    public float scrollSensitvity = 2f;
    public float orbitDampening = 10f;
    public float scrollDampening = 6f;

    void Start()
    {
        _xFormCamera = transform;
        _xFormParent = transform.parent;
    }


    void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            //Rotation of the Camera based on Mouse Coordinates
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                _localRotation.x += Input.GetAxis("Mouse X") * mouseSensitivity;
                _localRotation.y += Input.GetAxis("Mouse Y") * mouseSensitivity;
                //Clamp the y Rotation to horizon and not flipping over at the top
                if (_localRotation.y < -90f)
                    _localRotation.y = -90f;
                else if (_localRotation.y > 90f)
                    _localRotation.y = 90f;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0f && !Input.anyKey)
        {
            var scrollAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSensitvity;

            scrollAmount *= _cameraDistance * 0.3f;
            _cameraDistance += scrollAmount * -1f;

            _cameraDistance = Mathf.Clamp(_cameraDistance, 1.6f, 5f);
        }


        //Actual Camera Rig Transformations
        Quaternion QT = Quaternion.Euler(-_localRotation.y, _localRotation.x, 0);
        _xFormParent.rotation = Quaternion.Lerp(_xFormParent.rotation, QT, Time.deltaTime * orbitDampening);

        if (_xFormCamera.localPosition.z != _cameraDistance * -1f)
        {
            _xFormCamera.localPosition = new Vector3(0f, 0f,
                Mathf.Lerp(_xFormCamera.localPosition.z, -_cameraDistance, Time.deltaTime * scrollDampening));
        }
    }
}