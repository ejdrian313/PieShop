using Microsoft.EntityFrameworkCore;
using PieShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieShop.Models
{
    public class PieRepository : IPieRepository
    {
        private readonly AppDbContext _appDbContext;

        public PieRepository(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        public IEnumerable<Pie> Pies => _appDbContext.Pies.Include(pie => pie.Category);

        public IEnumerable<Pie> PiesOfTheWeek => _appDbContext.Pies.Include(pie => pie.Category).Where(pie => pie.IsPieOfTheWeek);

        public Pie GetPieById(int pieId)
        {
            return _appDbContext.Pies.FirstOrDefault(p => p.PieId == pieId);
        }
    }
}
