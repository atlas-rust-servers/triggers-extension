using JetBrains.Annotations;
using Oxide.Core;
using Oxide.Core.Extensions;

namespace Oxide.Ext.TriggersExt;

[UsedImplicitly]
public class TriggersExt : Extension
{
    private static readonly VersionNumber s_ExtensionVersion = new(1, 0, 0);

    public override string Name => "TriggersExt";
    public override string Author => "Ilovepatatos";
    public override VersionNumber Version => s_ExtensionVersion;

    public override bool SupportsReloading => true;

    public TriggersExt(ExtensionManager manager) : base(manager) { }
}