using Microsoft.AspNetCore.Mvc;
using mpLaundry.DAL.ScaffoldContainer;
using mpLaundry.Services.Interfaces;

namespace mpLaundry.Controllers;

public class LayananController : Controller
{
    private readonly ILayananService _svc;

    public LayananController(ILayananService svc)
    {
        _svc = svc;
    }

    // ──────────────────────────────────────────────
    //  INDEX — Daftar semua layanan + pencarian
    // ──────────────────────────────────────────────
    public async Task<IActionResult> Index(string? cari)
    {
        ViewBag.Cari = cari;
        var data = await _svc.GetAllAsync(cari);
        return View(data);
    }

    // ──────────────────────────────────────────────
    //  DETAILS — Lihat detail layanan
    // ──────────────────────────────────────────────
    public async Task<IActionResult> Details(Guid id)
    {
        var layanan = await _svc.GetByIdAsync(id);
        if (layanan == null)
        {
            TempData["Gagal"] = "Data layanan tidak ditemukan.";
            return RedirectToAction(nameof(Index));
        }
        return View(layanan);
    }

    // ──────────────────────────────────────────────
    //  CREATE — Tambah layanan baru
    // ──────────────────────────────────────────────
    public IActionResult Create()
    {
        var model = new MLayanan
        {
            TxtSatuan = "KG",       // default satuan
            IntEstimasiHari = 3,    // default estimasi
            BolActive = true
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MLayanan model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _svc.CreateAsync(model);

        if (!result.Sukses)
        {
            TempData["Gagal"] = result.Pesan;
            return View(model);
        }

        TempData["Sukses"] = result.Pesan;
        return RedirectToAction(nameof(Index));
    }

    // ──────────────────────────────────────────────
    //  EDIT — Ubah data layanan
    // ──────────────────────────────────────────────
    public async Task<IActionResult> Edit(Guid id)
    {
        var layanan = await _svc.GetByIdAsync(id);
        if (layanan == null)
        {
            TempData["Gagal"] = "Data layanan tidak ditemukan.";
            return RedirectToAction(nameof(Index));
        }
        return View(layanan);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MLayanan model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _svc.UpdateAsync(model);

        if (!result.Sukses)
        {
            TempData["Gagal"] = result.Pesan;
            return View(model);
        }

        TempData["Sukses"] = result.Pesan;
        return RedirectToAction(nameof(Index));
    }

    // ──────────────────────────────────────────────
    //  DELETE — Hapus layanan (POST only)
    // ──────────────────────────────────────────────
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _svc.DeleteAsync(id);

        if (result.Sukses)
            TempData["Sukses"] = result.Pesan;
        else
            TempData["Gagal"] = result.Pesan;

        return RedirectToAction(nameof(Index));
    }
}

