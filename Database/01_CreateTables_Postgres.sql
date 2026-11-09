-- =============================================================
-- 01_CreateTables_Postgres.sql | LaundryDb | PostgreSQL / DBeaver
-- Ikuti DB Naming Convention & Standard PostgreSQL
-- =============================================================

-- Ekstensi untuk UUID gen_random_uuid() (bawaan Postgres 13+)
CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-- ---------- 1. MASTER PELANGGAN ----------
CREATE TABLE IF NOT EXISTS "mPelanggan" (
    "pkPelangganId"     UUID            PRIMARY KEY DEFAULT gen_random_uuid(),
    "txtKodePelanggan"  VARCHAR(10)     NOT NULL,
    "txtNama"           VARCHAR(100)    NOT NULL,
    "txtAlamat"         VARCHAR(200)    NULL,
    "txtNoTelepon"      VARCHAR(20)     NOT NULL,
    "txtEmail"          VARCHAR(100)    NULL,
    "bolActive"         BOOLEAN         NOT NULL DEFAULT TRUE,
    "refInsertedBy"     VARCHAR(50)     NOT NULL,
    "dtInserted"        TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "refUpdatedBy"      VARCHAR(50)     NULL,
    "dtUpdated"         TIMESTAMP       NULL,
    CONSTRAINT "UQ_mPelanggan_txtKodePelanggan" UNIQUE ("txtKodePelanggan")
);

-- ---------- 2. MASTER LAYANAN ----------
CREATE TABLE IF NOT EXISTS "mLayanan" (
    "pkLayananId"       UUID            PRIMARY KEY DEFAULT gen_random_uuid(),
    "txtKodeLayanan"    VARCHAR(10)     NOT NULL,
    "txtNamaLayanan"    VARCHAR(100)    NOT NULL,
    "txtSatuan"         VARCHAR(10)     NOT NULL,
    "decHarga"          NUMERIC(18,2)   NOT NULL,
    "intEstimasiHari"   INT             NOT NULL,
    "bolActive"         BOOLEAN         NOT NULL DEFAULT TRUE,
    "refInsertedBy"     VARCHAR(50)     NOT NULL,
    "dtInserted"        TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "refUpdatedBy"      VARCHAR(50)     NULL,
    "dtUpdated"         TIMESTAMP       NULL,
    CONSTRAINT "UQ_mLayanan_txtKodeLayanan" UNIQUE ("txtKodeLayanan"),
    CONSTRAINT "CK_mLayanan_txtSatuan" CHECK ("txtSatuan" IN ('KG', 'PCS')),
    CONSTRAINT "CK_mLayanan_decHarga" CHECK ("decHarga" > 0),
    CONSTRAINT "CK_mLayanan_intEstimasiHari" CHECK ("intEstimasiHari" >= 1)
);

-- ---------- 3. TRANSAKSI ORDER (HEADER) ----------
CREATE TABLE IF NOT EXISTS "trLaundry" (
    "pkLaundryId"       UUID            PRIMARY KEY DEFAULT gen_random_uuid(),
    "txtNoOrder"        VARCHAR(20)     NOT NULL,
    "dtMasuk"           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "dtEstimasiSelesai" TIMESTAMP       NOT NULL,
    "dtDiambil"         TIMESTAMP       NULL,
    "refPelangganId"    UUID            NOT NULL,
    "txtStatus"         VARCHAR(20)     NOT NULL,
    "decTotalHarga"     NUMERIC(18,2)   NOT NULL,
    "decBayar"          NUMERIC(18,2)   NOT NULL DEFAULT 0,
    "bolLunas"          BOOLEAN         NOT NULL DEFAULT FALSE,
    "txtKeterangan"     VARCHAR(250)    NULL,
    "refInsertedBy"     VARCHAR(50)     NOT NULL,
    "dtInserted"        TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "refUpdatedBy"      VARCHAR(50)     NULL,
    "dtUpdated"         TIMESTAMP       NULL,
    CONSTRAINT "UQ_trLaundry_txtNoOrder" UNIQUE ("txtNoOrder"),
    CONSTRAINT "CK_trLaundry_txtStatus" CHECK ("txtStatus" IN ('Diterima', 'Diproses', 'Selesai', 'Diambil')),
    CONSTRAINT "CK_trLaundry_decBayar" CHECK ("decBayar" >= 0 AND "decBayar" <= "decTotalHarga"),
    CONSTRAINT "FK_trLaundry_mPelanggan" FOREIGN KEY ("refPelangganId")
        REFERENCES "mPelanggan" ("pkPelangganId")
);

-- ---------- 4. TRANSAKSI ORDER (DETAIL) ----------
CREATE TABLE IF NOT EXISTS "trLaundryDetail" (
    "pkLaundryDetailId" UUID            PRIMARY KEY DEFAULT gen_random_uuid(),
    "refLaundryId"      UUID            NOT NULL,
    "refLayananId"      UUID            NOT NULL,
    "decQty"            NUMERIC(10,2)   NOT NULL,
    "decHargaSatuan"    NUMERIC(18,2)   NOT NULL,
    "decSubtotal"       NUMERIC(18,2)   NOT NULL,
    "txtCatatan"        VARCHAR(100)    NULL,
    "refInsertedBy"     VARCHAR(50)     NOT NULL,
    "dtInserted"        TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "refUpdatedBy"      VARCHAR(50)     NULL,
    "dtUpdated"         TIMESTAMP       NULL,
    CONSTRAINT "FK_trLaundryDetail_trLaundry" FOREIGN KEY ("refLaundryId")
        REFERENCES "trLaundry" ("pkLaundryId") ON DELETE CASCADE,
    CONSTRAINT "FK_trLaundryDetail_mLayanan" FOREIGN KEY ("refLayananId")
        REFERENCES "mLayanan" ("pkLayananId")
);

-- Index foreign key & status
CREATE INDEX IF NOT EXISTS "IX_trLaundry_refPelangganId" ON "trLaundry" ("refPelangganId");
CREATE INDEX IF NOT EXISTS "IX_trLaundry_txtStatus" ON "trLaundry" ("txtStatus");
CREATE INDEX IF NOT EXISTS "IX_trLaundryDetail_refLaundryId" ON "trLaundryDetail" ("refLaundryId");
CREATE INDEX IF NOT EXISTS "IX_trLaundryDetail_refLayananId" ON "trLaundryDetail" ("refLayananId");
