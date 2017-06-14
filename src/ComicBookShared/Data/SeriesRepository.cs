using ComicBookShared.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicBookShared.Data
{
    public class SeriesRepository : BaseRepository<Series>
    {
        public SeriesRepository(Context context)
            : base(context)
        {
        }

        public override IList<Series> GetList()
        {
            return Context.Series
                    .OrderBy(s => s.Title)
                    .ToList();
        }

        public override Series Get(int? id, bool includeRelatedEntities = true)
        {
            var series = Context.Series.AsQueryable();

            if (includeRelatedEntities)
            {
                series = series.Include(s => s.ComicBooks);
            }
            
            return series
                .Where(s => s.Id == id)
                .SingleOrDefault();
        }

        public bool Validate(Series series)
        {
            return Context.Series.Any(s => s.Id != series.Id &&
                                                  s.Title == series.Title);
        }
    }
}
