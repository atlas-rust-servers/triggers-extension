using JetBrains.Annotations;
using Oxide.Ext.GizmosExt;
using Oxide.Ext.IlovepatatosExt;
using Rust;
using UnityEngine;

namespace Oxide.Ext.TriggersExt;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class SpherePlayerTrigger : BaseTriggerVolume<SphereCollider>, IPlayerTrigger
{
    [Serializable]
    public class Settings
    {
        public float Radius;
        public Vector3 Offset;
    }
    
    public event Action<BasePlayer> OnPlayerWalksIn, OnPlayerWalksOut;

    public static SpherePlayerTrigger Create(Vector3 pos, float radius, Action<BasePlayer> onPlayerWalksIn = null,
        Action<BasePlayer> onPlayerWalksOut = null)
    {
        var trigger = new GameObject(nameof(SpherePlayerTrigger)).AddComponent<SpherePlayerTrigger>();
        trigger.Position = pos;
        trigger.Size = radius;
        trigger.OnPlayerWalksIn += onPlayerWalksIn;
        trigger.OnPlayerWalksOut += onPlayerWalksOut;
        return trigger;
    }

    public static SpherePlayerTrigger Create(GameObject obj, float radius, Action<BasePlayer> onPlayerWalksIn = null,
        Action<BasePlayer> onPlayerWalksOut = null)
    {
        var trigger = obj.AddComponent<SpherePlayerTrigger>();
        trigger.Size = radius;
        trigger.OnPlayerWalksIn += onPlayerWalksIn;
        trigger.OnPlayerWalksOut += onPlayerWalksOut;
        return trigger;
    }

    public float Size
    {
        get => Collider.radius;
        set => Collider.radius = value;
    }

    protected override void Awake()
    {
        base.Awake();
        gameObject.layer = (int)Layer.Reserved1; //Required to collide with player... Don't know why
        LayerTarget = Layer.Player_Server;
    }

#region Events

    protected override void OnObjectAdded(BaseEntity entity)
    {
        base.OnObjectAdded(entity);

        if (entity is BasePlayer player)
            OnPlayerWalksIn?.Invoke(player);
    }

    protected override void OnObjectRemoved(BaseEntity entity)
    {
        base.OnObjectRemoved(entity);

        if (entity is BasePlayer player)
            OnPlayerWalksOut?.Invoke(player);
    }

#endregion

    public override bool IsObjectOfInterest(GameObject obj)
    {
        if (!base.IsObjectOfInterest(obj))
            return false;

        return obj.ToBaseEntity() is BasePlayer;
    }

    public override bool Contains(Vector3 pos)
    {
        return Collider != null && Vector3.Distance(Collider.transform.position + Collider.center, pos) <= Size;
    }

    public override Vector3 GetRandomPos(float offset = 1, float yOffset = 10, float range = 25)
    {
        Vector3 pos = Collider.GetRandomPointWithinBounds(offset);
        float height = MapUtility.GetTerrainHeightAt(pos, yOffset, range);
        return pos.WithY(height);
    }

    public override void DrawGizmos(IEnumerable<BasePlayer> players, float duration)
    {
        OxideGizmos.Sphere(players, Position, Size, Color.green, duration);
    }

    public IEnumerable<BasePlayer> GetPlayers()
    {
        return Entities.Where(entity => entity is BasePlayer).Cast<BasePlayer>();
    }
}