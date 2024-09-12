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
    public class MeccsekListajaModel : PageModel
    {
        private readonly Foci.Models.FociDbContext _context;

        public MeccsekListajaModel(Foci.Models.FociDbContext context)
        {
            _context = context;
        }

        public IList<Meccs> Meccs { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Meccs = await _context.Meccsek.ToListAsync();
        }
    }
}
