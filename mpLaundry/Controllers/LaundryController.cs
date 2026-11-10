using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using mpLaundry.Common;
using mpLaundry.Common.ViewModels;
using mpLaundry.Services;
using mpLaundry.Services.Interfaces;

namespace mpLaundry.Controllers;

public class LaundryController(
    ILaundryService laundryService,
    IPelangganService pelangganService,
    ILayananService layananService
) : Controller
{
    // 1. Tampilkan Daftar Semua Order
    public async Task<IActionResult> Index(DateTime? dari, DateTime? sampai, string? status)
    {
        ViewBag.Dari = dari;
        ViewBag.Sampai = sampai;
        ViewBag.Status = new SelectList(StatusLaundry.Urutan, status);
        var data = await laundryService.GetAllAsync(dari, sampai, status);
        return View(data);
    }
    // 2. Buka Form Order Baru (GET)
    public async Task<IActionResult> Create()
    {
        await IsiDropdownAsync();
        return View(new LaundryCreateVM());
    }
    // 3. Proses Simpan Order Baru (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LaundryCreateVM vm)
    {
        if (ModelState.IsValid)
        {
            var hasil = await laundryService.CreateAsync(vm);
            if (hasil.Sukses)
            {
                TempData["Sukses"] = hasil.Pesan;
                // Begitu berhasil disimpan, kasir langsung diarahkan ke halaman Nota/Details
                return RedirectToAction(nameof(Details), new { id = hasil.Id });
            }
            ModelState.AddModelError("", hasil.Pesan);
        }
        await IsiDropdownAsync();
        return View(vm);
    }
    // 4. Halaman Nota Kasir / Detail Order
    public async Task<IActionResult> Details(Guid id)
    {
        var data = await laundryService.GetByIdAsync(id);
        return data is null ? NotFound() : View(data);
    }
    // 5. Majukan Status Order (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(Guid id)
    {
        var hasil = await laundryService.UpdateStatusAsync(id);
        TempData[hasil.Sukses ? "Sukses" : "Gagal"] = hasil.Pesan;
        return RedirectToAction(nameof(Details), new { id });
    }
    // 6. Pembayaran Pelunasan Sisa Tagihan (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Bayar(Guid id, decimal jumlah)
    {
        var hasil = await laundryService.BayarAsync(id, jumlah);
        TempData[hasil.Sukses ? "Sukses" : "Gagal"] = hasil.Pesan;
        return RedirectToAction(nameof(Details), new { id });
    }
    // 7. Hapus Order (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var hasil = await laundryService.DeleteAsync(id);
        TempData[hasil.Sukses ? "Sukses" : "Gagal"] = hasil.Pesan;
        return hasil.Sukses
            ? RedirectToAction(nameof(Index))
            : RedirectToAction(nameof(Details), new { id });
    }
    // Helper untuk mengisi data Dropdown Pelanggan dan Layanan Aktif
    private async Task IsiDropdownAsync()
    {
        ViewBag.Pelanggan = new SelectList(
            await pelangganService.GetAktifAsync(), "PkPelangganId", "TxtNama");
        ViewBag.Layanan = await layananService.GetAktifAsync();
    }
}

