using System;
using System.Collections.Generic;

namespace mpLaundry.Common.ViewModels;

public class DashboardVM
{
    // ── Ringkasan Status Cucian (FSD Flow) ──
    public int OrderDiterima { get; set; }
    public int OrderDiproses { get; set; }
    public int OrderSelesai { get; set; }
    public int OrderDiambil { get; set; }
    public int TotalOrder { get; set; }

    // ── Ringkasan Keuangan & Pembayaran ──
    public int OrderBelumLunas { get; set; }
    public int OrderLunas { get; set; }
    public decimal TotalPendapatan { get; set; } // Kas Diterima Hari Ini (Ter-reset setiap hari)
    public decimal TotalPendapatanSemua { get; set; } // Akumulasi Total Kas Keseluruhan
    public int TotalOrderHariIni { get; set; } // Total order yang masuk hari ini

    // ── Ringkasan Master Data ──
    public int TotalPelanggan { get; set; }
    public int TotalLayanan { get; set; }

    // ── Transaksi Terbaru (5 data terakhir) ──
    public List<DashboardRecentOrderVM> RecentOrders { get; set; } = new();
}

public class DashboardRecentOrderVM
{
    public Guid Id { get; set; }
    public string NoOrder { get; set; } = "";
    public string NamaPelanggan { get; set; } = "";
    public string NoHpPelanggan { get; set; } = "";
    public DateTime TanggalMasuk { get; set; }
    public DateTime EstimasiSelesai { get; set; }
    public string Status { get; set; } = "";
    public decimal TotalHarga { get; set; }
    public decimal Bayar { get; set; }
    public bool BolLunas { get; set; }
    public decimal SisaTagihan => TotalHarga - Bayar;
}
