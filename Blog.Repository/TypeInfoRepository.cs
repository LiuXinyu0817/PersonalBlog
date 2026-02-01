using Blog.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Repository
{
    public class TypeInfoRepository:BaseRepository<Model.TypeInfo>, ITypeInfoRepository
    {
    }
}
