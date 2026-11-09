using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace mpLaundry.Common.Helpers;

public static class Format
{
    private static readonly CultureInfo Indonesia = CultureInfo.GetCultureInfo("id-ID");

    // 17500 -> "Rp 17.500"
    public static string Rupiah(decimal nilai) =>
        "Rp " + nilai.ToString("N0", Indonesia);

    // 2.50 -> "2,5"
    public static string Angka(decimal nilai) =>
        nilai.ToString("0.##", Indonesia);

    // 2026-11-06 -> "06 Nov 2026"
    public static string Tanggal(DateTime nilai) =>
        nilai.ToString("dd MMM yyyy", Indonesia);
}
