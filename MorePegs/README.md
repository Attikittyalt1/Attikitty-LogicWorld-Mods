# Board Pegs
Adds pegs that connect via the grid on their parent board.

More specifically, each board peg has a hidden link to every other board peg in the same row or column as it. If placed on the side of a board, they only connect in the direction of the board and not to any other board pegs on the same side.

These can be found in the Miscellaneous/Routing section of the components inventory.

## Install / Dependencies:

Just drop the `BoardPegs` folder into your `GameData` folder.

For this mod to function, you will need the following mods:
- [`EccsLogicWorldAPI`](https://github.com/Ecconia/Ecconia-LogicWorld-Mods/tree/master/EccsLogicWorldAPI) by @Ecconia
- [`HarmonyForLogicWorld`](https://github.com/Ecconia/Ecconia-LogicWorld-Mods/tree/master/HarmonyForLogicWorld) by @Ecconia
- [`SkysGeneralLib`](https://github.com/skyjoe999/SkysLogicWorldMods/tree/main/SkysGeneralLib) by @skyjoe999