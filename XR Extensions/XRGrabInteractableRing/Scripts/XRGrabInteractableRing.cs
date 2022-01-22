using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace XR.Extensions.Interactable
{
    [RequireComponent(typeof(XRGrabInteractable))]
    [RequireComponent(typeof(Collider))]
    public class XRGrabInteractableRing : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        [Tooltip("The prefab of a ring")]
        private GameObject _modelPrefab;
        
        [SerializeField] 
        [Tooltip("Ring color")]
        private Color _color = Color.white;
       
        [SerializeField] 
        [Tooltip("Transform type\n\n" +
                 "Self: The ring will be placed in the center of the object\n" +
                 "Custom: Custom attach transform\n")]
        private RingTransformType _transformType;
       
        [SerializeField] 
        [Tooltip("The transform that is used as the attach point for Interactables. Only for Custom transform type.")]
        private Transform _attachTransform;
        
        [SerializeField] 
        [Tooltip("Show on selected object")]
        private bool _showOnSelected;

        
        [Header("Ray casting")]
        [SerializeField]
        private LayerMask _layerMask;

        [SerializeField] 
        [Tooltip("Maximum distance to display the ring")]
        private float _thresholdDistance = 1f;

        [Header("Size")] 
        [SerializeField] 
        [Tooltip("Minimum scale ring")]
        private Vector3 _minScale;
        
        [SerializeField] 
        [Tooltip("Maximum scale ring")]
        private Vector3 _maxScale = new Vector3(1, 1, 1);

        [Header("Animation")]
        [SerializeField] 
        [Tooltip("Show animation")]
        private bool _enabled = true;
        
        [SerializeField] 
        [Tooltip("Speed")]
        private float _speed = 1f;
        
        [SerializeField] 
        [Tooltip("Duration")]
        private float _duration = 0.1f;

        [Header("Events")] 
        public UnityEvent OnShow = new UnityEvent();
        public UnityEvent OnHide = new UnityEvent();

        private GameObject _highlightRing;
        private XRGrabInteractable _grabInteractable;
        private Collider _collider;
        private bool _isSelected;
        private Camera _camera;

        private void Awake()
        {
            _grabInteractable = GetComponent<XRGrabInteractable>();
            _camera = FindObjectOfType<XROrigin>()?.Camera;

            if (_camera == null)
            {
                Debug.LogError("Camera can't be null!");
                return;
            }

            if (_modelPrefab == null)
            {
                Debug.LogError("Model Prefab property is required!");
                return;
            }

            if (_camera == null)
            {
                Debug.LogError("Camera property is required!");
                return;
            }

            switch (_transformType)
            {
                case RingTransformType.Self:
                    CreateSelfInstance();
                    break;

                case RingTransformType.Custom:
                    CreateTransformInstance();
                    break;

                default:
                    Debug.LogError("Unknown transform type!");
                    break;
            }
        }

        private void OnEnable()
        {
            _grabInteractable.selectEntered.AddListener(GrabSelectEntered);
            _grabInteractable.selectExited.AddListener(GrabSelectExited);
        }

        private void OnDisable()
        {
            _grabInteractable.selectEntered.RemoveListener(GrabSelectEntered);
            _grabInteractable.selectExited.RemoveListener(GrabSelectExited);
            _highlightRing.transform.localScale = _minScale;
        }

        private void Update()
        {
            ShowRing();
        }

        private void GrabSelectExited(SelectExitEventArgs arg0)
        {
            _isSelected = false;
        }

        private void GrabSelectEntered(SelectEnterEventArgs arg0)
        {
            _isSelected = true;
            
            if (!_showOnSelected)
            {
                _highlightRing.SetActive(false);
                _highlightRing.transform.localScale = _minScale;
            }
            else
            {
                _highlightRing.SetActive(true);
            }
        }

        private void ShowRing()
        {
            if (_isSelected && !_showOnSelected)
            {
                return;
            }
            
            if (_transformType == RingTransformType.Self)
            {
                _highlightRing.transform.position = _collider.bounds.center;
            }

            if (Physics.Raycast(new Ray(_camera.transform.position, _camera.transform.forward), out var hit, _layerMask))
            {
                _highlightRing.transform.LookAt(_camera.transform.position);

                if (hit.distance >= _thresholdDistance)
                {
                    if (_highlightRing.activeSelf)
                    {
                        _highlightRing.SetActive(false);
                        _highlightRing.transform.localScale = _minScale;
                        OnHide?.Invoke();
                    }
                }
                else
                {
                    if (!_highlightRing.activeSelf)
                    {
                        if (_enabled)
                        {
                            StartCoroutine(Animation(_minScale, _maxScale));
                        }

                        _highlightRing.SetActive(true);
                        OnShow?.Invoke();
                    }
                }
            }
            else
            {
                _highlightRing.SetActive(false);
                _highlightRing.transform.localScale = _minScale;
            }
        }

        private IEnumerator Animation(Vector3 from, Vector3 to)
        {
            var i = 0f;
            var rate = (1.0f / _duration) * _speed;

            while (i < 1.0f)
            {
                i += Time.deltaTime * rate;
                _highlightRing.transform.localScale = Vector3.Lerp(from, to, i);
                yield return null;
            }

            StopCoroutine(nameof(Animation));
        }

        private void CreateSelfInstance()
        {
            if (!TryGetComponent(out _collider))
            {
                Debug.LogError("Collider component is required for Self transform type!");
                return;
            }

            _highlightRing = Instantiate(_modelPrefab, _collider.bounds.center, Quaternion.identity, transform);
            _highlightRing.transform.parent = transform;
            _highlightRing.transform.localScale = _minScale;

            SetColor();

            _highlightRing.SetActive(false);
        }

        private void CreateTransformInstance()
        {
            _highlightRing = Instantiate(_modelPrefab, _attachTransform.position, Quaternion.identity, transform);
            _highlightRing.transform.parent = transform;
            _highlightRing.transform.localScale = _minScale;

            SetColor();

            _highlightRing.SetActive(false);
        }

        private void SetColor()
        {
            var meshRenderer = _highlightRing.GetComponent<MeshRenderer>();

            if (meshRenderer == null)
            {
                Debug.LogError("Component MeshRenderer not found!");
                return;
            }

            meshRenderer.material.SetColor("_Color", _color);
        }
    }

    public enum RingTransformType
    {
        Self,
        Custom
    }
}
