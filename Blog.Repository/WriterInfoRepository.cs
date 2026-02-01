using Blog.IRepository;
using Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Repository
{
    public class WriterInfoRepository:BaseRepository<WriterInfo>, IWriterInfoRepository
    {
    }
}
