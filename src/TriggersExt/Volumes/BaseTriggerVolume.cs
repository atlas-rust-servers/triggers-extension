using JetBrains.Annotations;
using Oxide.Ext.GizmosExt;
using Rust;
using UnityEngine;
using VLB;

namespace Oxide.Ext.TriggersExt;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public abstract class BaseTriggerVolume<T> : MonoBehaviour, IGizmosDrawer where T : Collider
{
    public T Collider;
    public Layer LayerTarget;

    public readonly HashSet<GameObject> Contents = new();
    public readonly HashSet<BaseEntity> Entities = new();

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }

    public Quaternion Rotation
    {
        get => transform.rotation;
        set => transform.rotation = value;
    }


    public event Action<BaseEntity> OnEntityWalksIn, OnEntityWalksOut;

#region Unity

    protected virtual void Awake()
    {
        GameObject obj = gameObject;
        Collider = obj.GetOrAddComponent<T>();
        Collider.isTrigger = true;

        var rb = obj.GetOrAddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.detectCollisions = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
    }

    private void OnEnable()
    {
        OxideGizmos.AllGizmos.Add(this);
    }

    protected virtual void OnDisable()
    {
        OxideGizmos.AllGizmos.Remove(this);

        if (Rust.Application.isQuitting)
            return;

        foreach (GameObject obj in Contents.ToList())
            OnTriggerExit(obj);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if (!IsObjectOfInterest(obj)) return;
        if (!Contents.Add(obj)) return;

        OnObjectAdded(obj, other);
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject obj = other.gameObject;

        if (!IsObjectOfInterest(obj)) return;
        if (!Contents.Remove(obj)) return;

        OnObjectRemoved(obj, other);
    }

    protected void OnTriggerExit(GameObject obj)
    {
        Collider col = obj != null ? obj.GetComponent<Collider>() : null;
        if (col != null)
            OnTriggerExit(col);
    }

#endregion

#region Events

    protected virtual void OnObjectAdded(GameObject obj, Collider col)
    {
        var entity = obj.ToBaseEntity();
        if (entity == null) return;

        Entities.Add(entity);
        OnObjectAdded(entity);
    }

    protected virtual void OnObjectAdded(BaseEntity entity)
    {
        OnEntityWalksIn?.Invoke(entity);
    }

    protected virtual void OnObjectRemoved(GameObject obj, Collider col)
    {
        var entity = obj.ToBaseEntity();
        if (entity == null) return;

        Entities.Remove(entity);
        OnObjectRemoved(entity);
    }

    protected virtual void OnObjectRemoved(BaseEntity entity)
    {
        OnEntityWalksOut?.Invoke(entity);
    }

#endregion

#region Gizmos

    public void DrawGizmos(BasePlayer player, float duration)
    {
        DrawGizmos(new[] { player }, duration);
    }

    public abstract void DrawGizmos(IEnumerable<BasePlayer> players, float duration);

#endregion

    public virtual bool IsObjectOfInterest(GameObject obj)
    {
        return obj != null && obj.IsOnLayer(LayerTarget);
    }

    public virtual void Kill()
    {
        Destroy(gameObject);
    }

    public void SetCollisionsEnable(bool enable)
    {
        if (gameObject != null)
            gameObject.SetActive(enable);
    }

    public void Clear()
    {
        foreach (GameObject obj in Contents.ToList())
            Remove(obj);
    }

    public void Remove(GameObject obj)
    {
        OnTriggerExit(obj);
    }

    public void Remove(BaseEntity entity)
    {
        if (entity != null)
            Remove(entity.gameObject);
    }

    public abstract bool Contains(Vector3 pos);

    public abstract Vector3 GetRandomPos(float offset = 1, float yOffset = 10, float range = 25);
}