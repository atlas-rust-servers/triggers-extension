using JetBrains.Annotations;
using Oxide.Ext.GizmosExt;
using Oxide.Ext.IlovepatatosExt;
using Rust;
using UnityEngine;

namespace Oxide.Ext.TriggersExt;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class BoxPlayerTrigger : BaseTriggerVolume<BoxCollider>, IPlayerTrigger
{
    public Vector3 Size
    {
        get => Collider.size;
        set => Collider.size = value;
    }

    public event Action<BasePlayer> OnPlayerWalksIn, OnPlayerWalksOut;

    public static BoxPlayerTrigger Create(Vector3 pos, Quaternion rot, Vector3 size, Action<BasePlayer> onPlayerWalksIn = null,
        Action<BasePlayer> onPlayerWalksOut = null)
    {
        var trigger = new GameObject(nameof(BoxPlayerTrigger)).AddComponent<BoxPlayerTrigger>();
        trigger.Position = pos;
        trigger.Rotation = rot;
        trigger.Collider.size = size;
        trigger.OnPlayerWalksIn += onPlayerWalksIn;
        trigger.OnPlayerWalksOut += onPlayerWalksOut;
        return trigger;
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
        return Collider != null && Collider.Contains(pos);
    }

    public override Vector3 GetRandomPos(float offset = 1, float yOffset = 10, float range = 25)
    {
        Vector3 pos = Collider.GetRandomPointWithinBounds(new Vector3(offset, offset, offset));
        float height = MapUtility.GetTerrainHeightAt(pos, yOffset, range);
        return pos.WithY(height);
    }

    public override void DrawGizmos(IEnumerable<BasePlayer> players, float duration)
    {
        OxideGizmos.Box(players, Position, Rotation, Size, Color.green, duration);
    }

    public IEnumerable<BasePlayer> GetPlayers()
    {
        return Entities.Where(entity => entity is BasePlayer).Cast<BasePlayer>();
    }
}