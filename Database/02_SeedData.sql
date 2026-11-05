-- =============================================================
-- 02_SeedData.sql  |  data awal: 5 pelanggan, 8 layanan
-- =============================================================
USE LaundryDb;
GO
DECLARE @sys NVARCHAR(50) = '00000000-0000-0000-0000-000000000001';
DECLARE @now DATETIME = GETDATE();
 
INSERT INTO dbo.mPelanggan
    (txtKodePelanggan, txtNama, txtAlamat, txtNoTelepon, txtEmail,
     bolActive, refInsertedBy, dtInserted)
VALUES
    ('PLG001', 'Budi Santoso',  'Jl. Merdeka 1, Jakarta', '081200000001',
     'budi@mail.com', 1, @sys, @now),
    ('PLG002', 'Siti Aminah',   'Jl. Sudirman 5, Bekasi', '081200000002',
     NULL, 1, @sys, @now),
    ('PLG003', 'Andi Wijaya',   'Jl. Pemuda 10, Bogor',   '081200000003',
     'andi@mail.com', 1, @sys, @now),
    ('PLG004', 'Dewi Lestari',  NULL,                     '081200000004',
     NULL, 1, @sys, @now),
    ('PLG005', 'Rudi Hartono',  'Jl. Diponegoro 7, Depok', '081200000005',
     NULL, 1, @sys, @now);
 
INSERT INTO dbo.mLayanan
    (txtKodeLayanan, txtNamaLayanan, txtSatuan, decHarga, intEstimasiHari,
     bolActive, refInsertedBy, dtInserted)
VALUES
    ('LYN001', 'Cuci Kering Setrika', 'KG',   7000, 2, 1, @sys, @now),
    ('LYN002', 'Cuci Kering',         'KG',   5000, 2, 1, @sys, @now),
    ('LYN003', 'Setrika Saja',        'KG',   4000, 1, 1, @sys, @now),
    ('LYN004', 'Cuci Express 1 Hari', 'KG',  12000, 1, 1, @sys, @now),
    ('LYN005', 'Bed Cover',           'PCS', 25000, 3, 1, @sys, @now),
    ('LYN006', 'Selimut',             'PCS', 20000, 3, 1, @sys, @now),
    ('LYN007', 'Jas / Blazer',        'PCS', 30000, 3, 1, @sys, @now),
    ('LYN008', 'Cuci Sepatu',         'PCS', 35000, 4, 1, @sys, @now);
GO

