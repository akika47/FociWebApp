using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Foci.Models;

namespace Foci.Pages
{

    public record class Allas
    {
        private int helyezes;
        private string csapatNev;
        private int merkozesekSzama;
        private int gyozelem;
        private int vereseg;
        private int pontszam;
        private int lottGol;
        private int kapottGol;

        public int Helyezes { get => helyezes; set => helyezes = value; }
        public string CsapatNev { get => csapatNev; set => csapatNev = value; }
        public int MerkozesekSzama { get => merkozesekSzama; set => merkozesekSzama = value; }
        public int Gyozelem { get => gyozelem; set => gyozelem = value; }
        public int Vereseg { get => vereseg; set => vereseg = value; }
        public int Pontszam { get => pontszam; set => pontszam = value; }
        public int LottGol { get => lottGol; set => lottGol = value; }
        public int KapottGol { get => kapottGol; set => kapottGol = value; }

        public Allas(int helyezes, string csapatNev, int merkozesekSzama, int gyozelem, int vereseg, int pontszam, int lottGol, int kapottGol)
        {
            Helyezes = helyezes;
            CsapatNev = csapatNev;
            MerkozesekSzama = merkozesekSzama;
            Gyozelem = gyozelem;
            Vereseg = vereseg;
            Pontszam = pontszam;
            LottGol = lottGol;
            KapottGol = kapottGol;
        }
    }
    public class BajnoksagAllasaModel : PageModel
    {
        private readonly Foci.Models.FociDbContext _context;

        public BajnoksagAllasaModel(Foci.Models.FociDbContext context)
        {
            _context = context;
        }

        public IList<Meccs> Meccs { get;set; } = default!;
        public IList<Allas> Allas { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Meccs = await _context.Meccsek.ToListAsync();
            string[] csapatok = Meccs.Select(x => x.HazaiCsapat).Distinct().ToArray();

            List<Allas> allasok = new List<Allas>();

            foreach (var item in csapatok)
            {
                var meccsek = Meccs.Where(x => x.HazaiCsapat == item || x.VendegCsapat == item).ToList();

                int merkozesekSzama = meccsek.Count;
                int gyozelmeKolt = 0;
                int veresegKolt = 0;
                int pontszam = 0;
                int lottGol = 0;
                int kapottGol = 0;

                foreach (var meccs in meccsek)
                {
                    if (meccs.HazaiCsapat == item)
                    {
                        if (meccs.HazaiVeg > meccs.VendegVeg)
                        {
                            gyozelmeKolt++;
                            pontszam += 3;
                        }
                        else if (meccs.HazaiVeg == meccs.VendegVeg)
                        {
                            pontszam++;
                        }
                        else
                        {
                            veresegKolt++;
                        }
                        lottGol += meccs.HazaiFelido;
                        kapottGol += meccs.VendegFelido;
                    }
                    else
                    {
                        if (meccs.VendegVeg > meccs.HazaiVeg)
                        {
                            gyozelmeKolt++;
                            pontszam += 3;
                        }
                        else if (meccs.VendegVeg == meccs.HazaiVeg)
                        {
                            pontszam++;
                        }
                        else
                        {
                            veresegKolt++;
                        }
                        lottGol += meccs.VendegFelido;
                        kapottGol += meccs.HazaiFelido;
                    }
                }

                allasok.Add(new Allas(0, item, merkozesekSzama, gyozelmeKolt, veresegKolt, pontszam, lottGol, kapottGol));
            }

            allasok = allasok.OrderByDescending(x => x.Pontszam).ToList();

            for (int i = 0; i < allasok.Count; i++)
            {
                allasok[i] = new Allas(i + 1, allasok[i].CsapatNev, allasok[i].MerkozesekSzama, allasok[i].Gyozelem, allasok[i].Vereseg, allasok[i].Pontszam, allasok[i].LottGol, allasok[i].KapottGol);
            }

            Allas = allasok;
        }
    }
}
