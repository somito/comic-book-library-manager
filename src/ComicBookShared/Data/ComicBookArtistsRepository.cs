﻿using ComicBookShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ComicBookShared.Data
{
    public class ComicBookArtistsRepository : BaseRepository<ComicBookArtist>
    {
        public ComicBookArtistsRepository(Context context)
            : base(context)
        {
        }

        public bool Validate(ComicBookArtist comicBookArtist)
        {
            return Context.ComicBookArtists
                    .Any(cba => cba.ComicBookId == comicBookArtist.ComicBookId &&
                                cba.ArtistId == comicBookArtist.ArtistId &&
                                cba.RoleId == comicBookArtist.RoleId);
        }

        public override ComicBookArtist Get(int? id, bool includeRelatedEntities = true)
        {
            var comicBookArtists = Context.ComicBookArtists.AsQueryable();

            if (includeRelatedEntities)
            {
                comicBookArtists = comicBookArtists
                    .Include(cba => cba.Artist)
                    .Include(cba => cba.Role)
                    .Include(cba => cba.ComicBook.Series);
            }

            return comicBookArtists
                .Where(cba => cba.Id == id)
                .SingleOrDefault();
        }

        public override IList<ComicBookArtist> GetList()
        {
            throw new NotImplementedException();
        }
    }
}
