using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Stealth.Components;
/// <summary>
/// Add this component to an entity that you want to be cloaked.
/// It overlays a shader on the entity to give them an invisibility cloaked effect
/// It also turns the entity invisible
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedStealthSystem))]
public sealed class StealthComponent : Component
{
    /// <summary>
    /// Whether or not the stealth effect should currently be applied.
    /// </summary>
    [DataField("enabled")]
    public bool Enabled = true;

    /// <summary>
    /// Whether or not the entity previously had an interaction outline prior to cloaking.
    /// </summary>
    [DataField("hadOutline")]
    public bool HadOutline;

    /// <summary>
    /// Minimum visibility before the entity becomes unexaminable (and thus no longer appears on context menus).
    /// </summary>
    [DataField("examineThreshold")]
    public readonly float ExamineThreshold = 0.5f;

    /// <summary>
    /// Last set level of visibility. The visual effect ranges from 1 (fully visible) and -1 (fully hidden). Values
    /// outside of this range simply act as a buffer for the visual effect (i.e., a delay before turning invisible). To
    /// get the actual current visibility, use <see cref="SharedStealthSystem.GetVisibility(EntityUid,
    /// StealthComponent?)"/>
    /// </summary>
    [DataField("lastVisibility")]
    [Access(typeof(SharedStealthSystem),  Other = AccessPermissions.None)]
    public float LastVisibility = 1;


    /// <summary>
    /// Time at which <see cref="LastVisibility"/> was set. Null implies the entity is currently paused and not
    /// accumulating any visibility change.
    /// </summary>
    [DataField("lastUpdate", customTypeSerializer:typeof(TimeOffsetSerializer))]
    public TimeSpan? LastUpdated;

    /// <summary>
    /// Rate that effects how fast an entity's visibility passively changes.
    /// </summary>
    [DataField("passiveVisibilityRate")]
    public readonly float PassiveVisibilityRate = -0.15f;

    /// <summary>
    /// Rate for movement induced visibility changes. Scales with distance moved.
    /// </summary>
    [DataField("movementVisibilityRate")]
    public readonly float MovementVisibilityRate = 0.2f;

    /// <summary>
    /// Minimum visibility. Note that the visual effect caps out at -1, but this value is allowed to be larger or smaller.
    /// </summary>
    [DataField("minVisibility")]
    public readonly float MinVisibility = -1f;

    /// <summary>
    /// Maximum visibility. Note that the visual effect caps out at +1, but this value is allowed to be larger or smaller.
    /// </summary>
    [DataField("maxVisibility")]
    public readonly float MaxVisibility = 1.5f;
}

[Serializable, NetSerializable]
public sealed class StealthComponentState : ComponentState
{
    public readonly float Visibility;
    public readonly TimeSpan? LastUpdated;
    public readonly bool Enabled;

    public StealthComponentState(float stealthLevel, TimeSpan? lastUpdated, bool enabled)
    {
        Visibility = stealthLevel;
        LastUpdated = lastUpdated;
        Enabled = enabled;
    }
}
