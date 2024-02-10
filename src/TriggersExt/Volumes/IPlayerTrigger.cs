using JetBrains.Annotations;
using UnityEngine;

namespace Oxide.Ext.TriggersExt;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public interface IPlayerTrigger
{
    Vector3 Position { get; set; }
    Quaternion Rotation { get; set; }

    event Action<BasePlayer> OnPlayerWalksIn, OnPlayerWalksOut;

    void Kill();
}