using BLL.DTO;
using BLL.Interfaces;
using BLL.Mappers;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class TegService : ITegService
    {
        private readonly IUnitOfWork _unitOfWork;
        private TegMapper _tegMapper;
        private ArticleMapper _articleMapper;
        private TegMapper TegMapper
        {
            get
            {
                if (_tegMapper == null)
                {
                    _tegMapper = new TegMapper();
                }
                return _tegMapper;
            }
        }

        private ArticleMapper ArticleMapper
        {
            get
            {
                if( _articleMapper == null)
                {
                    _articleMapper = new ArticleMapper();
                }
                return _articleMapper;
            }
        }

        public TegService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        
        public IEnumerable<TegDTO> GetAllTegs()
        {
            return TegMapper.Map(_unitOfWork.TegRepository.Get());
        }

        public IEnumerable<ArticleDTO> GetArticlesByTeg(IEnumerable<TegDTO> tegs)
        {
            List<Article> articles = new List<Article>();
            foreach (TegDTO teg in tegs) {
                Teg tegEntity;
                if (teg.Id != null)
                {
                    tegEntity = _unitOfWork.TegRepository.GetById(teg.Id.GetValueOrDefault());
                    if (tegEntity == null) throw new ArgumentNullException(nameof(tegEntity));
                }
                else if (teg.Name != null)
                {
                    tegEntity = _unitOfWork.TegRepository.Get(t => t.Name == teg.Name).FirstOrDefault();
                    if (tegEntity == null) throw new ArgumentNullException(nameof(tegEntity));

                }
                else
                {
                    throw new ArgumentNullException(nameof(teg));
                }
                articles.AddRange(_unitOfWork.ArticleRepository.Get(a => a.ArticleTegs
                .Contains(new ArticleTeg { ArticleId = a.Id, TegId = tegEntity.Id })));
                }
            return ArticleMapper.Map(articles);
        }

        public async Task<TegDTO> GetTegById(int id)
        {
            var teg = await _unitOfWork.TegRepository.GetByIdAsync(id);
            if (teg == null) throw new ArgumentNullException(nameof(teg));
            return TegMapper.Map(teg);
        }
    }
}
