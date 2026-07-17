using System;
using System.Collections.Generic;
using System.Linq;

namespace MorePegs.Shared;

public static class TupleExtensions
{
    public static (List<A>, List<B>) Unpack<A, B>(this List<(A, B)> list) =>
        list.Aggregate(
            (
                new List<A>(list.Count()),
                new List<B>(list.Count())
            ),
            (unpacked, tuple) =>
            {
                unpacked.Item1.Add(tuple.Item1);
                unpacked.Item2.Add(tuple.Item2);
                return unpacked;
            }
        );

    public static IEnumerable<T> Merge<T>(this (IEnumerable<T>, IEnumerable<T>) list) => list.Item1.Concat(list.Item2);
}