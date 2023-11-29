using AuthServerJWT.Core.Interfaces;
using AuthServerJWT.Core.Repositories;
using AuthServerJWT.Core.UnitOfWork;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServerJWT.Service.Services
{
    public class GenericService<TEntity, TDto> : IServiceGeneric<TEntity, TDto> where TDto : class where TEntity : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<TEntity> _repository;

        public GenericService(IGenericRepository<TEntity> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<TDto>> AddAsync(TDto entity)
        {
            var newEntity=ObjectMapper.Mapper.Map<TEntity>(entity);
            await _repository.AddAsync(newEntity);
            await _unitOfWork.CommitAsync();
            var newDto=ObjectMapper.Mapper.Map<TDto>(newEntity);
            return Response<TDto>.Success(newDto,200);

        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            var products = ObjectMapper.Mapper.Map<List<TDto>>(await _repository.GetAllAsync());

            return Response<IEnumerable<TDto>>.Success(products, 200);
        }

        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
           var product= await _repository.GetByIdAsync(id);
           if(product == null)
            {
                return Response<TDto>.Fail("Id not found", 404, true);
            }
           return Response<TDto>.Success(ObjectMapper.Mapper.Map<TDto>(product),200);
        }

        public async Task<Response<NoDataDto>> Remove(int id)
        {
            var isExistEntity=await _repository.GetByIdAsync(id);
            if(isExistEntity == null)
            {
                return Response<NoDataDto>.Fail("Id not found", 404,true);
            }
            _repository.Remove(isExistEntity);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(204);
        }

        public async Task<Response<NoDataDto>> Update(TDto entity,int id)
        {
            var isExistEntity = await _repository.GetByIdAsync(id);
            if (isExistEntity == null)
            {
                return Response<NoDataDto>.Fail("Id not found", 404, true);
            }
            var updateEntity=ObjectMapper.Mapper.Map<TEntity>(entity);
            _repository.Update(updateEntity);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(204);
        }

        public Response<IEnumerable<TDto>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            var list=_repository.Where(predicate);
            return Response<IEnumerable<TDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<TDto>>(list),200);
        }
    }
}
