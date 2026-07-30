using CondensedCablingBasicLogicComponents.Shared;
using EccsGuiBuilder.Client.Layouts.Helper;
using EccsGuiBuilder.Client.Wrappers;
using EccsGuiBuilder.Client.Wrappers.AutoAssign;
using LogicSettings;
using LogicUI.MenuParts;
using LogicWorld.UI;
using System;

namespace CondensedCablingBasicLogicComponents.Client;

public class EditGenericSingleSliderSuperGate : EditComponentMenu<SuperSizeSingleSliderData>, IAssignMyFields
{

    public static void Build(string windowName, string containerName, string sliderLocalizationkey)
    {
        WS.window(windowName)
            .setYPosition(870)
            .setMinSize(800, 0)
            .setDefaultSize(800, 0)
            .configureContent(content => content
                .layoutVertical()
                .addContainer(containerName, link => link
                    .layoutVertical()
                    .add(WS.textLine
                        .setLocalizationKey(sliderLocalizationkey)
                        .setFontSize(40)
                    )
                    .add(WS.slider
                        .injectionKey(nameof(SliderValue))
                        .fixedSize(400, 38)
                        .setInterval(1)
                        .setMin(1)
                        .setMax(Math.Min(MyClient.MaxSuperSize, 256))
                    )
                )
            )
            .add<EditGenericSingleSliderSuperGate>()
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
                entry.Data.BitSize = (int)value;
            }

        };
    }

    protected override void OnStartEditing()
    {
        var data = FirstComponentBeingEdited.Data;
        SliderValue.SetValueWithoutNotify(data.BitSize);
    }
}