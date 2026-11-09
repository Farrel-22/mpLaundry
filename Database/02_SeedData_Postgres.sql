-- =============================================================
-- 02_SeedData_Postgres.sql | LaundryDb | PostgreSQL / DBeaver
-- Data awal: 5 Pelanggan, 8 Layanan
-- =============================================================

DO $$
DECLARE
    v_sys VARCHAR(50) := '00000000-0000-0000-0000-000000000001';
    v_now TIMESTAMP := CURRENT_TIMESTAMP;
BEGIN
    -- 1. SEED PELANGGAN
    INSERT INTO "mPelanggan"
        ("txtKodePelanggan", "txtNama", "txtAlamat", "txtNoTelepon", "txtEmail",
         "bolActive", "refInsertedBy", "dtInserted")
    VALUES
        ('PLG001', 'Budi Santoso',  'Jl. Merdeka 1, Jakarta', '081200000001',
         'budi@mail.com', TRUE, v_sys, v_now),
        ('PLG002', 'Siti Aminah',   'Jl. Sudirman 5, Bekasi', '081200000002',
         NULL, TRUE, v_sys, v_now),
        ('PLG003', 'Andi Wijaya',   'Jl. Pemuda 10, Bogor',   '081200000003',
         'andi@mail.com', TRUE, v_sys, v_now),
        ('PLG004', 'Dewi Lestari',  NULL,                     '081200000004',
         NULL, TRUE, v_sys, v_now),
        ('PLG005', 'Rudi Hartono',  'Jl. Diponegoro 7, Depok', '081200000005',
         NULL, TRUE, v_sys, v_now)
    ON CONFLICT ("txtKodePelanggan") DO NOTHING;

    -- 2. SEED LAYANAN
    INSERT INTO "mLayanan"
        ("txtKodeLayanan", "txtNamaLayanan", "txtSatuan", "decHarga", "intEstimasiHari",
         "bolActive", "refInsertedBy", "dtInserted")
    VALUES
        ('LYN001', 'Cuci Kering Setrika', 'KG',   7000, 2, TRUE, v_sys, v_now),
        ('LYN002', 'Cuci Kering',         'KG',   5000, 2, TRUE, v_sys, v_now),
        ('LYN003', 'Setrika Saja',        'KG',   4000, 1, TRUE, v_sys, v_now),
        ('LYN004', 'Cuci Express 1 Hari', 'KG',  12000, 1, TRUE, v_sys, v_now),
        ('LYN005', 'Bed Cover',           'PCS', 25000, 3, TRUE, v_sys, v_now),
        ('LYN006', 'Selimut',             'PCS', 20000, 3, TRUE, v_sys, v_now),
        ('LYN007', 'Jas / Blazer',        'PCS', 30000, 3, TRUE, v_sys, v_now),
        ('LYN008', 'Cuci Sepatu',         'PCS', 35000, 4, TRUE, v_sys, v_now)
    ON CONFLICT ("txtKodeLayanan") DO NOTHING;
END $$;

