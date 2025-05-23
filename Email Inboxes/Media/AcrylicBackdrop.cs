using Microsoft.UI.Composition.SystemBackdrops;

namespace Email_Inboxes.Media;

public partial class AcrylicBackdrop : Backdrop<DesktopAcrylicController, DesktopAcrylicKind>
{
    protected override DesktopAcrylicKind ControllerKind
    {
        get => Controller.Kind;
        set => Controller.Kind = value;
    }

    protected override Color ControllerTint
    {
        get => Controller.TintColor;
        set => Controller.TintColor = value;
    }

    protected override float ControllerTintOpacity
    {
        get => Controller.TintOpacity;
        set => Controller.TintOpacity = value;
    }

    protected override float ControllerLuminosityOpacity
    {
        get => Controller.LuminosityOpacity;
        set => Controller.LuminosityOpacity = value;
    }

    protected override void ResetPropertiesRequested() => Controller.ResetProperties();
}