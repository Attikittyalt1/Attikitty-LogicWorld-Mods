using CondensedCablingBasicLogicComponents.Shared;
using EccsGuiBuilder.Client.Layouts.Helper;
using EccsGuiBuilder.Client.Wrappers;
using EccsGuiBuilder.Client.Wrappers.AutoAssign;
using LogicSettings;
using LogicUI.MenuParts;
using LogicWorld.UI;
using System;

namespace CondensedCablingBasicLogicComponents.Client;

public class SuperSizeSliderGUI : EditComponentMenu<SuperSizeSliderData>, IAssignMyFields
{

    [Setting_SliderInt("CondensedCablingBasicLogicComponents.MaxSuperSize")]
    public static int MaxSuperSize { get; set; } = 8;

    public static void Build(string windowLocalizationKey, string sliderLocalizationkey)
    {
        WS.window(windowLocalizationKey)
            .setYPosition(870)
            .setMinSize(800, 0)
            .setDefaultSize(800, 0)
            .configureContent(content => content
                .layoutVertical()
                .add(WS.textLine
                    .setLocalizationKey(sliderLocalizationkey)
                    .setFontSize(40)
                )
                .add(WS.slider
                    .injectionKey(nameof(SliderValue))
                    .setMin(1)
                    .setMax(32)
                    .fixedSize(400, 38)
                )
            )
            .add<SuperSizeSliderGUI>()
            .build();
    }

    [AssignMe] public InputSlider SliderValue;

    public override void Initialize()
    {
        base.Initialize();

        //Setup events and handlers:
        SliderValue.OnValueChanged += value =>
        {
            foreach (var entry in ComponentsBeingEdited)
            {
                entry.Data.CurrentValue = (int)value;
            }

        };
    }

    protected override void OnStartEditing()
    {
        var data = FirstComponentBeingEdited.Data;
        SliderValue.SetValueWithoutNotify(data.CurrentValue);
        SliderValue.Max = Math.Min(MaxSuperSize, data.GetMax());
        SliderValue.Min = data.GetMin();
        SliderValue.SliderInterval = data.GetInterval();
    }
}