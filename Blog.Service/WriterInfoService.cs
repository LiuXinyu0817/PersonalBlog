using Blog.IRepository;
using Blog.IService;
using Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service
{
    public class WriterInfoService : BaseService<WriterInfo>, IWriterInfoService
    {
        private readonly IWriterInfoRepository _writerInfoRepository;
        public WriterInfoService(IWriterInfoRepository writerInfoRepository)
        {
            base._IBaseRepository = writerInfoRepository;
            _writerInfoRepository = writerInfoRepository;
        }
    }
}
