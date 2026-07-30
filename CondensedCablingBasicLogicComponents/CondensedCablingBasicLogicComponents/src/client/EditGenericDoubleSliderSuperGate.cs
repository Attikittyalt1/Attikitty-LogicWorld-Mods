using CondensedCablingBasicLogicComponents.Shared;
using EccsGuiBuilder.Client.Layouts.Helper;
using EccsGuiBuilder.Client.Wrappers;
using EccsGuiBuilder.Client.Wrappers.AutoAssign;
using LogicSettings;
using LogicUI.MenuParts;
using LogicWorld.UI;
using System;

namespace CondensedCablingBasicLogicComponents.Client;

public class EditGenericDoubleSliderSuperGate : EditComponentMenu<SuperSizeDoubleSliderData>, IAssignMyFields
{

    public static void Build(string windowName, string containerNameA, string containerNameB, string sliderLocalizationkeyA, string sliderLocalizationkeyB)
    {
        WS.window(windowName)
            .setYPosition(870)
            .setMinSize(800, 0)
            .setDefaultSize(800, 0)
            .configureContent(content => content
                .layoutVertical()
                .addContainer(containerNameA, link => link
                    .layoutVertical()
                    .add(WS.textLine
                        .setLocalizationKey(sliderLocalizationkeyA)
                        .setFontSize(40)
                    )
                    .add(WS.slider
                        .injectionKey(nameof(SliderValueA))
                        .fixedSize(400, 38)
                        .setInterval(1)
                        .setMin(1)
                        .setMax(Math.Min(MyClient.MaxSuperSize, 256))
                    )
                )
                .addContainer(containerNameB, link => link
                    .layoutVertical()
                    .add(WS.textLine
                        .setLocalizationKey(sliderLocalizationkeyB)
                        .setFontSize(40)
                    )
                    .add(WS.slider
                        .injectionKey(nameof(SliderValueB))
                        .fixedSize(400, 38)
                        .setInterval(1)
                        .setMin(1)
                        .setMax(Math.Min(MyClient.MaxSuperSize, 256))
                    )
                )
            )
            .add<EditGenericDoubleSliderSuperGate>()
            .build();
    }

    [AssignMe] public InputSlider SliderValueA;
    [AssignMe] public InputSlider SliderValueB;

    public override void Initialize()
    {
        base.Initialize();

        //Setup events and handlers:
        SliderValueA.OnValueChanged += value =>
        {
            foreach (var entry in ComponentsBeingEdited)
            {
                entry.Data.BitSizeA = (int)value;
            }

        };
        SliderValueB.OnValueChanged += value =>
        {
            foreach (var entry in ComponentsBeingEdited)
            {
                entry.Data.BitSizeB = (int)value;
            }

        };
    }

    protected override void OnStartEditing()
    {
        var data = FirstComponentBeingEdited.Data;
        SliderValueA.SetValueWithoutNotify(data.BitSizeA);
        SliderValueB.SetValueWithoutNotify(data.BitSizeB);
    }
}