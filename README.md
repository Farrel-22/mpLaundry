# 🧺 Sistem Informasi Bersih Laundry (mpLaundry)
**Aplikasi Pengelolaan Kasir & Transaksi Laundry Berbasis ASP.NET Core MVC (.NET 10)**  
*Mini Project Bootcamp Fullstack Developer — IT Kalbe Nutritionals*

---

## 📌 1. Deskripsi Singkat
**Sistem Informasi Bersih Laundry** adalah aplikasi web modern yang dibangun untuk menggantikan sistem pencatatan nota fisik usaha laundry. Aplikasi ini mengotomatiskan seluruh alur operasional laundry, mulai dari pencatatan master pelanggan dan tarif layanan, pembuatan nota transaksi dengan perhitungan otomatis (kiloan desimal & satuan), alur status cucian 4 tahap, hingga pelunasan dan dashboard ringkasan kas masuk harian.

Aplikasi dirancang dengan mematuhi prinsip **Clean Architecture (3-Layer Architecture)**, pemisahan tanggung jawab yang ketat (*Separation of Concerns*), serta standar penamaan database (*DB Naming Convention*) dan jejak audit (*Audit Trail*) enterprise.

---

## 🛠️ 2. Prasyarat Sistem (*Prerequisites*)
Sebelum menjalankan aplikasi, pastikan komputer Anda telah terinstal:
- **.NET SDK 10.0** atau yang lebih baru (`dotnet --version`)
- **Visual Studio 2022 / 2025** (dengan workload *ASP.NET and web development*) atau Visual Studio Code
- **PostgreSQL 14+** (atau **SQL Server / LocalDB**)
- **DBeaver** atau **pgAdmin 4** (untuk manajemen database)
- Browser modern (Google Chrome, Microsoft Edge)

---

## 🗄️ 3. Urutan Menjalankan Skrip Database (Database First)
Database dibuat menggunakan pendekatan **Database First**. Skrip SQL telah disediakan pada folder `Database/`:

### A. Untuk PostgreSQL (Rekomendasi Utama):
1. Buat database baru bernama **`laundrydb`** di PostgreSQL:
   ```sql
   CREATE DATABASE laundrydb;
   ```
2. Jalankan skrip tabel dan constraint:
   - Eksekusi file: `Database/01_CreateTables_Postgres.sql`
3. Jalankan skrip data awal (*seed data*):
   - Eksekusi file: `Database/02_SeedData_Postgres.sql`

*(Catatan: Tersedia juga file `01_CreateTables.sql` dan `02_SeedData.sql` jika menggunakan Microsoft SQL Server).*

---

## ⚙️ 4. Pengaturan Connection String
Buka file `mpLaundry/appsettings.json`, lalu sesuaikan kredensial koneksi database PostgreSQL lokal Anda:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=laundrydb;Username=postgres;Password=admin"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

---

## ⚡ 5. Perintah Scaffold (Entity Framework Core)
Jika skema tabel database diubah di masa mendatang, perbarui model dengan perintah `dotnet-ef` berikut:

```powershell
# Jalankan dari root folder solution
dotnet ef dbcontext scaffold "Host=localhost;Port=5432;Database=laundrydb;Username=postgres;Password=admin" Npgsql.EntityFrameworkCore.PostgreSQL --context LaundryDbContext --context-dir ScaffoldContainer --output-dir ScaffoldContainer --project mpLaundry.DAL --no-pluralize --no-onconfiguring --force
```

> **Aturan Penting:** File di dalam folder `ScaffoldContainer/` **dilarang diedit manual**. Semua validasi form (`[Required]`, `[Range]`, dll.) dan audit trail diimplementasikan menggunakan **Partial Class** di folder `mpLaundry.DAL/Partials/` sehingga aman dan tidak akan tertimpa saat scaffold ulang.

---

## 🚀 6. Cara Menjalankan Aplikasi
1. Buka file solution **`mpLaundry.slnx`** menggunakan Visual Studio.
2. Pastikan project **`mpLaundry`** (Web Presentation) terpilih sebagai *Startup Project*.
3. Tekan **`Ctrl + F5`** (*Start Without Debugging*) atau jalankan via CLI:
   ```powershell
   dotnet run --project mpLaundry
   ```
4. Aplikasi akan otomatis berjalan di peramban pada alamat:
   👉 **`http://localhost:5000`**

---

## 🏗️ 7. Struktur Arsitektur Solusi (3-Layer Architecture)
Solution dipecah menjadi 4 project terpisah demi modularitas dan maintainability tinggi:
- **`mpLaundry` (Presentation Layer):** Berisi Controller, Views (Razor .cshtml), file CSS/JS, dan routing. Controller dilarang meng-inject DbContext secara langsung, melainkan melalui Service Interface.
- **`mpLaundry.Services` (Business Logic Layer):** Memuat Interface dan Service (`LaundryService`, `PelangganService`, `LayananService`) tempat seluruh perhitungan matematis, penomoran urut otomatis, aturan bisnis, dan transaksi database dipusatkan.
- **`mpLaundry.DAL` (Data Access Layer):** Mengelola DbContext Entity Framework Core, hasil scaffold, dan Partial Class untuk metadata validasi & otomatisasi 4 kolom audit trail.
- **`mpLaundry.Common` (Shared Utilities):** Berisi ViewModel (`LaundryCreateVM`, `DashboardVM`), `ServiceResult`, status konstan cucian, dan format mata uang Rupiah/tanggal.

---

## 🌟 8. Fitur Utama & Alur Transaksi Kasir
1. **Master Pelanggan & Layanan:**
   - CRUD lengkap dengan pencarian realtime.
   - Penomoran kode unik otomatis (`PLG001`, `LYN001`).
   - Proteksi integritas relasional (layanan yang memiliki riwayat transaksi dilarang dihapus).
2. **Form Transaksi Order Dinamis:**
   - Tambah/hapus baris item dinamis menggunakan JavaScript realtime.
   - Format penomoran nota urut otomatis harian: `LD-yyyyMMdd-XXXX`.
   - Mendukung kuantitas pecahan desimal untuk KG (contoh: 2,5 kg) dan bilangan bulat untuk PCS.
   - Formula otomatis tanggal estimasi selesai (mengikuti layanan dengan hari estimasi terlama).
   - Penginputan uang muka (DP) dengan titik ribuan otomatis.
3. **Alur Status & Pelunasan 4 Tahap:**
   - Transisi status: `Diterima` ➔ `Diproses` ➔ `Selesai` ➔ `Diambil`.
   - Validasi ketat penyerahan: Cucian **wajib LUNAS** sebelum status dapat diubah menjadi `Diambil`.
   - Pembatasan pembatalan: Order hanya boleh dihapus jika status masih `Diterima`.
4. **Cetak Nota Kasir Bersih:**
   - Terintegrasi cetak browser `window.print()` dengan CSS `@media print` yang otomatis menyembunyikan navbar dan tombol sistem sehingga nota kasir tercetak rapi.
5. **Dashboard Operasional Kasir:**
   - Ringkasan total order per status cucian.
   - Kalkulasi pendapatan kas harian (*Daily Cash Settlement*).
   - Indikator peringatan dini cucian terlambat (*overdue badge*).
6. **Notifikasi Standar Banner Terpusat:**
   - Seluruh notifikasi sukses dan validasi penolakan tersentralisasi melalui banner alert hijau (`TempData["Sukses"]`) dan merah (`TempData["Gagal"]`) tanpa pop-up browser jadul.

---

## 📋 9. Kepatuhan Standar Pengujian (TC-01 s/d TC-18)
Aplikasi ini telah diuji secara komprehensif dan memenuhi **18 dari 18 Skenario Pengujian** dari Bab 11 panduan mentor, yang didokumentasikan lengkap dengan tangkapan layar di:  
📄 **`DOKUMEN_HASIL_PENGUJIAN_TC01_TC18.docx`**

| ID | Skenario Uji | Status |
| :---: | :--- | :---: |
| **TC-01** | Tambah data pelanggan valid | ✅ Passed |
| **TC-02** | Validasi pencegahan kode ganda (duplikat) | ✅ Passed |
| **TC-03** | Validasi layanan tidak valid (harga 0, hari 0) | ✅ Passed |
| **TC-04** | Perlindungan hapus layanan yang memiliki transaksi | ✅ Passed |
| **TC-05** | Validasi wajib minimal 1 item layanan cucian | ✅ Passed |
| **TC-06** | Transaksi 2,5 kg cuci + 1 bed cover DP Rp 10.000 | ✅ Passed |
| **TC-07** | Validasi satuan PCS bulat & KG minimal 1 | ✅ Passed |
| **TC-08** | Validasi uang muka tidak boleh melebihi total tagihan | ✅ Passed |
| **TC-09** | Penomoran nota urut otomatis di hari yang sama (`0001`, `0002`) | ✅ Passed |
| **TC-10** | Validasi penyerahan cucian (Diambil) wajib lunas | ✅ Passed |
| **TC-11** | Pembayaran pelunasan tagihan & pencatatan tanggal diambil | ✅ Passed |
| **TC-12** | Pembatalan order Diterima vs proteksi order Diproses | ✅ Passed |
| **TC-13** | Price Snapshot (konsistensi harga historis nota lama) | ✅ Passed |
| **TC-14** | Filter layanan nonaktif pada form kasir | ✅ Passed |
| **TC-15** | Audit Trail Insert otomatis (`dtInserted` terisi, `dtUpdated` NULL) | ✅ Passed |
| **TC-16** | Audit Trail Update otomatis (`dtUpdated` terisi timestamp baru) | ✅ Passed |
| **TC-17** | Skema database sesuai standar IT Kalbe (lowerCamelCase & 4 audit) | ✅ Passed |
| **TC-18** | Review kode arsitektur 3-layer, Dependency Injection, & partial class | ✅ Passed |

---
**Pengembang:** Farrel  
**Institusi / Program:** Bootcamp Fullstack Developer • IT Kalbe Nutritionals  
**Tahun:** 2026
