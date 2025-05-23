using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;
using WinRT;

namespace Email_Inboxes.Media;

public abstract partial class Backdrop<T, K> : SystemBackdrop
    where T : class, ISystemBackdropControllerWithTargets, new()
    where K : Enum
{
    #region Abstracts & Properties

    /// <summary>
    /// Enables the use of the <see cref="Tint"/>, <see cref="TintOpacity"/> and <see cref="LuminosityOpacity"/> properties. The default is <see langword="false"/>.
    /// </summary>
    /// <remarks>Setting this to <see langword="true"/> disables automatic theme handling. Resetting this to <see langword="false"/> resets the properties mentioned above.</remarks>
    public bool UseCustomProperties
    {
        get => customProperties;
        set
        {
            customProperties = value;

            if (!customProperties)
            {
                if (controller is not null)
                {
                    ResetPropertiesRequested();

                    Tint = ControllerTint;
                    TintOpacity = ControllerTintOpacity;
                    LuminosityOpacity = ControllerLuminosityOpacity;
                }
            }
        }
    }
    private bool customProperties;

    public K Kind
    {
        get => (K)GetValue(KindProperty);
        set => SetValue(KindProperty, value);
    }
    public static DependencyProperty KindProperty { get; } = DependencyProperty.Register("Kind", typeof(K), typeof(Backdrop<T, K>), new PropertyMetadata(default(K)));

    public Color Tint
    {
        get => (Color)GetValue(TintProperty);
        set => SetValue(TintProperty, value);
    }
    public static DependencyProperty TintProperty { get; } = DependencyProperty.Register("Tint", typeof(Color), typeof(Backdrop<T, K>), new PropertyMetadata(default(Color)));

    public float TintOpacity
    {
        get => (float)GetValue(TintOpacityProperty);
        set => SetValue(TintOpacityProperty, value);
    }
    public static DependencyProperty TintOpacityProperty { get; } = DependencyProperty.Register("TintOpacity", typeof(float), typeof(Backdrop<T, K>), new PropertyMetadata(0));

    public float LuminosityOpacity
    {
        get => (float)GetValue(LuminosityOpacityProperty);
        set => SetValue(LuminosityOpacityProperty, value);
    }
    public static DependencyProperty LuminosityOpacityProperty { get; } = DependencyProperty.Register("LuminosityOpacity", typeof(float), typeof(Backdrop<T, K>), new PropertyMetadata(0));

    protected abstract K ControllerKind { get; set; }
    protected abstract Color ControllerTint { get; set; }
    protected abstract float ControllerTintOpacity { get; set; }
    protected abstract float ControllerLuminosityOpacity { get; set; }

    protected abstract void ResetPropertiesRequested();

    #endregion

    public T Controller => controller;
    private T controller;

    private ICompositionSupportsSystemBackdrop target;

    public Backdrop()
    {
        RegisterPropertyChangedCallback(TintProperty, (s, e) =>
        {
            if (UseCustomProperties && controller is not null)
                ControllerTint = Tint;
        });

        RegisterPropertyChangedCallback(TintOpacityProperty, (s, e) =>
        {
            if (UseCustomProperties && controller is not null)
                ControllerTintOpacity = TintOpacity;
        });

        RegisterPropertyChangedCallback(LuminosityOpacityProperty, (s, e) =>
        {
            if (UseCustomProperties && controller is not null)
                ControllerLuminosityOpacity = LuminosityOpacity;
        });

        RegisterPropertyChangedCallback(KindProperty, (s, e) =>
        {
            if (controller is not null)
                ControllerKind = Kind;
        });
    }

    protected override void OnTargetConnected(ICompositionSupportsSystemBackdrop connectedTarget, XamlRoot xamlRoot)
    {
        base.OnTargetConnected(connectedTarget, xamlRoot);

        controller = new();
        target = connectedTarget;

        controller.AddSystemBackdropTarget(connectedTarget);
        controller.SetSystemBackdropConfiguration(GetDefaultSystemBackdropConfiguration(connectedTarget, xamlRoot));

        ControllerKind = Kind;
        if (UseCustomProperties)
        {
            ControllerTint = Tint;
            ControllerTintOpacity = TintOpacity;
            ControllerLuminosityOpacity = LuminosityOpacity;
        }
        else
        {
            Tint = ControllerTint;
            TintOpacity = ControllerTintOpacity;
            LuminosityOpacity = ControllerLuminosityOpacity;
        }
    }

    protected override void OnTargetDisconnected(ICompositionSupportsSystemBackdrop disconnectedTarget)
    {
        base.OnTargetDisconnected(disconnectedTarget);

        controller.RemoveSystemBackdropTarget(disconnectedTarget);
        controller.Dispose();
    }
}
