using SkysCondensedCablingLib.Server;

namespace TernaryComponents.Server;

public enum TernaryState
{
    Neutral,
    Positive,
    Negative,
    Invalid
}

public static class TernaryPegExtensions
{
    public static TernaryState GetInTernary(this SuperInputPeg peg)
    {
        return BoolPairToMyEnum((peg[0], peg[1]));
    }

    public static TernaryState GetInTernary(this SuperOutputPeg peg)
    {
        return BoolPairToMyEnum((peg[0], peg[1]));
    }

    public static void SetInTernary(this SuperOutputPeg peg, TernaryState state)
    {
        (peg[0], peg[1]) = MyEnumToBoolPair(state);
    }

    private static TernaryState BoolPairToMyEnum((bool bit0, bool bit1) pair) => pair switch
    {
        (false, false) => TernaryState.Neutral,
        (true, false) => TernaryState.Positive,
        (false, true) => TernaryState.Negative,
        (true, true) => TernaryState.Invalid,
    };

    private static (bool bit0, bool bit1) MyEnumToBoolPair(TernaryState state) => state switch
    {
        TernaryState.Neutral => (false, false),
        TernaryState.Positive => (true, false),
        TernaryState.Negative => (false, true),
        TernaryState.Invalid => (true, true),
    };
}