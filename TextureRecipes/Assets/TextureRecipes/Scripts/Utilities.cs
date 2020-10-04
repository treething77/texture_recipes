using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Utilities
{
    public static string MakeNiceName(string name)
    {
        var niceName = Regex.Replace(
            Regex.Replace(
                name,
                @"(\P{Ll})(\P{Ll}\p{Ll})",
                "$1 $2"
            ),
            @"(\p{Ll})(\P{Ll})",
            "$1 $2"
        );
        return niceName;
    }
}
