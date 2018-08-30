using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.RuntimeSets;
using ShadyPixel.Singleton;

public class CameraFollow : Singleton<CameraFollow>
{
    public float normalMovetime = 1.0f;
    public float transitionMovetime = 0.25f;
    public float maxSpd = 15f;
    public Vector2 deadZone;
    [Tooltip("Camera will find average of all Transforms in this Runtime Set.")]
    public TransformRuntimeSet transformRuntimeSet;

    private Camera _camera;
    private bool _hasBounds;
    public CameraZone _cameraZone;
    private Vector2 _size;
    private Vector2 _radius;
    private float _orthoSize;

    private bool _following;
    private Vector3 _velocity;
    private Vector3 _targetPos;
    private float _currentMoveTime;

    private bool _initialized;

    /// <summary>
    /// Returns attached Camera component.
    /// </summary>
    private Camera Cam
    {
        get
        {

            if (_camera == null)
            {
                _camera = GetComponent<Camera>();

            }

            return _camera;
        }
    }

    /// <summary>
    /// Returns average position of current follow transforms.
    /// </summary>
    private Vector2 AverageTargetPosition
    {
        get
        {
            Vector3 pos = new Vector3();
            foreach( Transform t in transformRuntimeSet.Items ) { pos += t.position; }
            pos /= transformRuntimeSet.Items.Count;
            pos.z = transform.position.z;

            if(_hasBounds)
            {
                pos = ClampPositionInsideBounds(pos, _cameraZone.BoxCollider2D.bounds);
            }

            if (!_initialized && Application.isPlaying)
            {
                Init(pos);
            }

            return pos;
        }
    }

    /// <summary>
    /// Returns true if the TargetPosition is currently inside of the deadzone.
    /// </summary>
    private bool InDeadZone
    {
        get
        {
            Vector2 deadzoneRadius = deadZone/2f;
            Vector2 avg = AverageTargetPosition;
            if (avg.x > transform.position.x + deadzoneRadius.x)
            {
                return false;
            }

            if (avg.x < transform.position.x - deadzoneRadius.x)
            {
                return false;
            }

            if (avg.y > transform.position.y + deadzoneRadius.y)
            {
                return false;
            }

            if (avg.y < transform.position.y - deadzoneRadius.y)
            {
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// Returns true if the Camera is not in bounds.
    /// </summary>
    private bool InBounds
    {
        get
        {
            SetCameraSizeValues();

            if (transform.position.x > _cameraZone.BoxCollider2D.bounds.center.x + _cameraZone.BoxCollider2D.bounds.extents.x)
            {
                return false;
            }

            if (transform.position.x < _cameraZone.BoxCollider2D.bounds.center.x - _cameraZone.BoxCollider2D.bounds.extents.x)
            {
                return false;
            }

            if (transform.position.y > _cameraZone.BoxCollider2D.bounds.center.y + _cameraZone.BoxCollider2D.bounds.extents.y)
            {
                return false;
            }

            if (transform.position.y < _cameraZone.BoxCollider2D.bounds.center.y - _cameraZone.BoxCollider2D.bounds.extents.y)
            {
                return false;
            }

            return true;

        }
    }

    public void SetPosition(Vector2 pos)
    {
        Vector3 position = pos;
        position.z = -10f;
        transform.position = position;
        _targetPos = position;
        _velocity = Vector2.zero;
        _following = false;
    }

    public void SetPosition(Vector2 pos, CameraZone cameraZone)
    {
        Vector3 position = pos;
        position = ClampPositionInsideBounds(position, cameraZone.BoxCollider2D.bounds);
        position.z = -10f;
        transform.position = position;
        _targetPos = position;
        _velocity = Vector2.zero;
        _following = false;
        SetBounds(cameraZone);
    }

    public Vector2 ClampPositionInsideBounds(Vector2 position, Bounds bounds)
    {
        SetCameraSizeValues();
        Vector2 newPos = position;
        newPos.x = Mathf.Clamp(newPos.x, bounds.center.x - bounds.extents.x + _radius.x, bounds.center.x + bounds.extents.x - _radius.x);
        newPos.y = Mathf.Clamp(newPos.y, bounds.center.y - bounds.extents.y + _radius.y, bounds.center.y + bounds.extents.y - _radius.y);
        return newPos;
    }

    public void SetBounds(CameraZone cameraZone)
    {
        _cameraZone = cameraZone;
        _hasBounds = true;
        _currentMoveTime = transitionMovetime;
        _targetPos = ClampPositionInsideBounds(_targetPos, _cameraZone.BoxCollider2D.bounds);
    }

    public void RemoveBounds(CameraZone cameraZone)
    {
        if(_cameraZone == cameraZone)
        {
            _hasBounds = false;
        }
    }

    private void LateUpdate()
    {
        if (transformRuntimeSet.Items.Count > 0)
        {
            if (InDeadZone == false)
            {
                _following = true;
            }
            if(_following)
            {
                _targetPos = AverageTargetPosition;
            }

            UpdatePosition();
        }
    }

    /// <summary>
    /// Moves transform toward target transform.
    /// </summary>
    private void UpdatePosition()
    {
        _targetPos.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, _targetPos, ref _velocity, _currentMoveTime, maxSpd, Time.unscaledDeltaTime);

        if (Vector3.Distance(transform.position, _targetPos) < 0.5f)
        {
            _following = false;
            _currentMoveTime = normalMovetime;

        }
    }

    private void OnDrawGizmosSelected()
    {
        //
        // draw deadzone
        if (Application.isPlaying)
        {
            if (InDeadZone)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;
        }
        else
            Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, deadZone);

        //
        // draw target position
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(AverageTargetPosition, Vector3.one);

        //
        // draw bounds
        if(_hasBounds)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(_cameraZone.BoxCollider2D.bounds.center, _cameraZone.BoxCollider2D.bounds.size);
        }

    }


    /// <summary>
    /// Initialize object. Sets starting position and size of camera.
    /// </summary>
    /// <param name="pos"></param>
    private void Init(Vector3 pos)
    {
        _initialized = true;
        _targetPos = pos;
        //transform.position = pos;

         SetCameraSizeValues();

        Initialize(this);
    }

    private void OnEnable()
    {
        Init(transform.position);
    }

    private void SetCameraSizeValues()
    {
        _orthoSize = Camera.main.orthographicSize;
        _size.y = 2f * _orthoSize;
        _size.x = _size.y * Camera.main.aspect;
        _radius = _size / 2f;
    }
}
