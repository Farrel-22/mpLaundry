using Microsoft.AspNetCore.Mvc;
using mpLaundry.DAL.ScaffoldContainer;
using mpLaundry.Services.Interfaces;

namespace mpLaundry.Controllers;

public class PelangganController : Controller
{
    private readonly IPelangganService _svc;

    public PelangganController(IPelangganService svc)
    {
        _svc = svc;
    }

    // 2. INDEX: Tampilkan daftar semua pelanggan + fitur cari
    public async Task<IActionResult> Index(string? cari)
    {
        ViewBag.Cari = cari;
        var data = await _svc.GetAllAsync(cari);
        return View(data);
    }

    // 3. DETAILS: Lihat profil lengkap satu pelanggan
    public async Task<IActionResult> Details(Guid id)
    {
        var pelanggan = await _svc.GetByIdAsync(id);
        if (pelanggan == null)
        {
            TempData["Gagal"] = "Data pelanggan tidak ditemukan.";
            return RedirectToAction(nameof(Index));
        }
        return View(pelanggan);
    }

    // 4. CREATE (GET): Buka form input pelanggan baru
    public IActionResult Create()
    {
        var model = new MPelanggan
        {
            BolActive = true // default langsung aktif
        };
        return View(model);
    }

    // 5. CREATE (POST): Proses simpan data ke database lewat Service
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MPelanggan model)
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

    // 6. EDIT (GET): Buka form edit dengan data yang sudah terisi
    public async Task<IActionResult> Edit(Guid id)
    {
        var pelanggan = await _svc.GetByIdAsync(id);
        if (pelanggan == null)
        {
            TempData["Gagal"] = "Data pelanggan tidak ditemukan.";
            return RedirectToAction(nameof(Index));
        }
        return View(pelanggan);
    }

    // 7. EDIT (POST): Proses simpan perubahan data
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MPelanggan model)
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

    // 8. DELETE (POST): Hapus pelanggan (Wajib POST + AntiForgeryToken)
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