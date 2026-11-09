using System;
using System.Collections.Generic;
using System.Text;

namespace mpLaundry.Common;

// Konstanta alur status order laundry.
// Status hanya boleh maju satu langkah secara berurutan.
public static class StatusLaundry
{
    public const string Diterima = "Diterima";
    public const string Diproses = "Diproses";
    public const string Selesai = "Selesai";
    public const string Diambil = "Diambil";

    public static readonly string[] Urutan = [Diterima, Diproses, Selesai, Diambil];

    public static string? Berikutnya(string status)
    {
        var i = Array.IndexOf(Urutan, status);
        return i >= 0 && i < Urutan.Length - 1 ? Urutan[i + 1] : null;
    }

    public static bool BolehMaju(string dari, string ke)
    {
        return Berikutnya(dari) == ke;
    }
}
