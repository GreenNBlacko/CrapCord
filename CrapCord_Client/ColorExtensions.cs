﻿using System.Drawing;
using System.Numerics;

namespace CrapCord_Client;

public static class ColorExtensions {
    public static Vector4 ToVector(this Color color) {
        return new Vector4(color.R, color.G, color.B, color.A);
    }
}