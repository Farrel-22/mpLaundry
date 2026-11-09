using System;
using System.Collections.Generic;
using System.Text;

namespace mpLaundry.Common
{
    public record ServiceResult(bool Sukses, string Pesan, Guid? Id = null)
    {
        public static ServiceResult Ok(string pesan, Guid? id = null) =>
            new(true, pesan, id);

        public static ServiceResult Gagal(string pesan) =>
            new(false, pesan);
    }
}
