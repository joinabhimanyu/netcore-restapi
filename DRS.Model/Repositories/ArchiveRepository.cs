using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRS.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace DRS.Model.Repositories
{
    public class ArchiveRepository : GenericRepository<Archive>
    {
        public ArchiveRepository(DRSDBContext context) : base(context)
        {
        }

        //public override IEnumerable<Archive> Get()
        //{
        //    return DbSet.Include(x=>x.DocumentCategory).Take(100).ToList();
        //}

        //public override IEnumerable<Archive> GetAll()
        //{
        //    return DbSet.Include(x => x.DocumentCategory).ToList();
        //}
    }
}
