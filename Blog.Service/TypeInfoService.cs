using Blog.IRepository;
using Blog.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service
{
    public class TypeInfoService : BaseService<Model.TypeInfo>, ITypeInfoService
    {
        private readonly ITypeInfoRepository _typeInfoRepository;
        public TypeInfoService(ITypeInfoRepository typeInfoRepository)
        {
            base._IBaseRepository = typeInfoRepository;
            _typeInfoRepository = typeInfoRepository;
        }
    }
}
