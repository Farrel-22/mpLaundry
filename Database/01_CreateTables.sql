-- =============================================================
-- 01_CreateTables.sql  |  LaundryDb  |  ikuti DB Naming Convention
-- =============================================================
IF DB_ID('LaundryDb') IS NULL CREATE DATABASE LaundryDb;
GO
USE LaundryDb;
GO
 
-- ---------- MASTER ----------
CREATE TABLE dbo.mPelanggan (
    pkPelangganId     UNIQUEIDENTIFIER NOT NULL
                      CONSTRAINT DF_mPelanggan_pk DEFAULT NEWSEQUENTIALID(),
    txtKodePelanggan  NVARCHAR(10)     NOT NULL,
    txtNama           NVARCHAR(100)    NOT NULL,
    txtAlamat         NVARCHAR(200)    NULL,
    txtNoTelepon      NVARCHAR(20)     NOT NULL,
    txtEmail          NVARCHAR(100)    NULL,
    bolActive         BIT              NOT NULL,
    refInsertedBy     NVARCHAR(50)     NOT NULL,
    dtInserted        DATETIME         NOT NULL,
    refUpdatedBy      NVARCHAR(50)     NULL,
    dtUpdated         DATETIME         NULL,
    CONSTRAINT PK_mPelanggan PRIMARY KEY (pkPelangganId),
    CONSTRAINT UQ_mPelanggan_txtKodePelanggan UNIQUE (txtKodePelanggan)
);
 
CREATE TABLE dbo.mLayanan (
    pkLayananId       UNIQUEIDENTIFIER NOT NULL
                      CONSTRAINT DF_mLayanan_pk DEFAULT NEWSEQUENTIALID(),
    txtKodeLayanan    NVARCHAR(10)     NOT NULL,
    txtNamaLayanan    NVARCHAR(100)    NOT NULL,
    txtSatuan         NVARCHAR(10)     NOT NULL,
    decHarga          DECIMAL(18,2)    NOT NULL,
    intEstimasiHari   INT              NOT NULL,
    bolActive         BIT              NOT NULL,
    refInsertedBy     NVARCHAR(50)     NOT NULL,
    dtInserted        DATETIME         NOT NULL,
    refUpdatedBy      NVARCHAR(50)     NULL,
    dtUpdated         DATETIME         NULL,
    CONSTRAINT PK_mLayanan PRIMARY KEY (pkLayananId),
    CONSTRAINT UQ_mLayanan_txtKodeLayanan UNIQUE (txtKodeLayanan),
    CONSTRAINT CK_mLayanan_txtSatuan CHECK (txtSatuan IN ('KG', 'PCS')),
    CONSTRAINT CK_mLayanan_decHarga CHECK (decHarga > 0),
    CONSTRAINT CK_mLayanan_intEstimasiHari CHECK (intEstimasiHari >= 1)
);
 
-- ---------- TRANSAKSI ----------
CREATE TABLE dbo.trLaundry (
    pkLaundryId       UNIQUEIDENTIFIER NOT NULL
                      CONSTRAINT DF_trLaundry_pk DEFAULT NEWSEQUENTIALID(),
    txtNoOrder        NVARCHAR(20)     NOT NULL,
    dtMasuk           DATETIME         NOT NULL,
    dtEstimasiSelesai DATETIME         NOT NULL,
    dtDiambil         DATETIME         NULL,
    refPelangganId    UNIQUEIDENTIFIER NOT NULL,
    txtStatus         NVARCHAR(20)     NOT NULL,
    decTotalHarga     DECIMAL(18,2)    NOT NULL,
    decBayar          DECIMAL(18,2)    NOT NULL,
    bolLunas          BIT              NOT NULL,
    txtKeterangan     NVARCHAR(250)    NULL,
    refInsertedBy     NVARCHAR(50)     NOT NULL,
    dtInserted        DATETIME         NOT NULL,
    refUpdatedBy      NVARCHAR(50)     NULL,
    dtUpdated         DATETIME         NULL,
    CONSTRAINT PK_trLaundry PRIMARY KEY (pkLaundryId),
    CONSTRAINT UQ_trLaundry_txtNoOrder UNIQUE (txtNoOrder),
    CONSTRAINT CK_trLaundry_txtStatus
        CHECK (txtStatus IN ('Diterima', 'Diproses', 'Selesai', 'Diambil')),
    CONSTRAINT CK_trLaundry_decBayar
        CHECK (decBayar >= 0 AND decBayar <= decTotalHarga),
    CONSTRAINT FK_trLaundry_mPelanggan FOREIGN KEY (refPelangganId)
        REFERENCES dbo.mPelanggan (pkPelangganId)
);
 
CREATE TABLE dbo.trLaundryDetail (
    pkLaundryDetailId UNIQUEIDENTIFIER NOT NULL
                      CONSTRAINT DF_trLaundryDetail_pk DEFAULT NEWSEQUENTIALID(),
    refLaundryId      UNIQUEIDENTIFIER NOT NULL,
    refLayananId      UNIQUEIDENTIFIER NOT NULL,
    decQty            DECIMAL(10,2)    NOT NULL,
    decHargaSatuan    DECIMAL(18,2)    NOT NULL,
    decSubtotal       DECIMAL(18,2)    NOT NULL,
    txtCatatan        NVARCHAR(100)    NULL,
    refInsertedBy     NVARCHAR(50)     NOT NULL,
    dtInserted        DATETIME         NOT NULL,
    refUpdatedBy      NVARCHAR(50)     NULL,
    dtUpdated         DATETIME         NULL,
    CONSTRAINT PK_trLaundryDetail PRIMARY KEY (pkLaundryDetailId),
    CONSTRAINT FK_trLaundryDetail_trLaundry FOREIGN KEY (refLaundryId)
        REFERENCES dbo.trLaundry (pkLaundryId) ON DELETE CASCADE,
    CONSTRAINT FK_trLaundryDetail_mLayanan FOREIGN KEY (refLayananId)
        REFERENCES dbo.mLayanan (pkLayananId)
);
 
-- index untuk foreign key dan filter status
CREATE INDEX IX_trLaundry_refPelangganId ON dbo.trLaundry (refPelangganId);
CREATE INDEX IX_trLaundry_txtStatus ON dbo.trLaundry (txtStatus);
CREATE INDEX IX_trLaundryDetail_refLaundryId ON dbo.trLaundryDetail (refLaundryId);
CREATE INDEX IX_trLaundryDetail_refLayananId ON dbo.trLaundryDetail (refLayananId);
GO

