using ComicBookShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ComicBookShared.Data
{
    public class Repository
    {
        private Context _context = null;

        public Repository(Context context)
        {
            _context = context;
        }

        public IList<ComicBook> GetComicBooks()
        {
            return _context.ComicBooks
                    .Include(cb => cb.Series)
                    .OrderBy(cb => cb.Series.Title)
                    .ThenBy(cb => cb.IssueNumber)
                    .ToList();
        }

        public ComicBook GetComicBookDetail(int? id)
        {
            return _context.ComicBooks
                    .Include(cb => cb.Series)
                    .Include(cb => cb.Artists.Select(a => a.Artist))
                    .Include(cb => cb.Artists.Select(a => a.Role))
                    .Where(cb => cb.Id == id)
                    .SingleOrDefault();
        }

        public void AddComicBook(ComicBook comicBook)
        {
            _context.ComicBooks.Add(comicBook);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Context Context()
        {
            return _context;
        }

        public ComicBook GetComicBookEdit(int? id)
        {
            return _context.ComicBooks
                .Where(cb => cb.Id == id)
                .SingleOrDefault();
        }

        public ComicBook GetComicBook(int? id)
        {
            return _context.ComicBooks
                .Include(cb => cb.Series)
                .Where(cb => cb.Id == id)
                .SingleOrDefault();
        }

        public void SetStateToModified(ComicBook comicBook)
        {
            _context.Entry(comicBook).State = EntityState.Modified;
        }

        public void SetStateToDeleted(ComicBook comicBook)
        {
            _context.Entry(comicBook).State = EntityState.Deleted;
        }

        public void SetStateToDeleted(ComicBookArtist comicBookArtist)
        {
            _context.Entry(comicBookArtist).State = EntityState.Deleted;
        }

        public bool Validate(ComicBook comicBook)
        {
            return _context.ComicBooks.Any(cb => cb.Id != comicBook.Id &&
                                                  cb.SeriesId == comicBook.SeriesId &&
                                                  cb.IssueNumber == comicBook.IssueNumber);
        }

        public bool Validate(ComicBookArtist comicBookArtist)
        {
            return _context.ComicBookArtists
                    .Any(cba => cba.ComicBookId == comicBookArtist.ComicBookId &&
                                cba.ArtistId == comicBookArtist.ArtistId &&
                                cba.RoleId == comicBookArtist.RoleId);
        }

        public ComicBook GetComicBookArtistAdd(int comicBookId)
        {
            return _context.ComicBooks
                .Include(cb => cb.Series)
                .Where(cb => cb.Id == comicBookId)
                .SingleOrDefault();
        }

        public void AddComicBookArtist(ComicBookArtist comicBookArtist)
        {
            _context.ComicBookArtists.Add(comicBookArtist);
        }

        public ComicBookArtist GetComicBookArtist(int? id)
        {
            return _context.ComicBookArtists
                .Include(cba => cba.Artist)
                .Include(cba => cba.Role)
                .Include(cba => cba.ComicBook.Series)
                .Where(cba => cba.Id == (int)id)
                .SingleOrDefault();
        }

        public IList<Artist> GetArtists()
        {
            return _context.Artists.OrderBy(a => a.Name).ToList();
        }

        public IList<Role> GetRoles()
        {
            return _context.Roles.OrderBy(r => r.Name).ToList();
        }

        public IList<Series> GetSeriesList()
        {
            return _context.Series.OrderBy(s => s.Title).ToList();
        }
    }
}
