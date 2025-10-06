using AutoMapper;
using Training.Common.Core;
using Training.Common.DTO.Base;
using Training.Common.DTO.Identity.Role;
using Training.Common.Infrastructure.UnitOfWork;
using Training.Domain;
using Training.Domain.Entities.Base;
using Training.Integration.CacheRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Training.Application.Services.Base
{
    public class BaseService<T, TAddDto, TEditDto, TGetDto, TKey, TKeyDto>
        : IBaseService<T, TAddDto, TEditDto, TGetDto, TKey, TKeyDto>
        where T : BaseEntity<TKey>
        where TAddDto : IEntityDto<TKeyDto>
        where TEditDto : IEntityDto<TKeyDto>
        where TGetDto : IEntityDto<TKeyDto>
    {
        protected readonly IUnitOfWork<T> UnitOfWork;
        protected readonly IMapper Mapper;
        protected IHttpContextAccessor HttpContextAccessor;
        protected IConfiguration Configuration;
        protected ICacheRepository CacheRepository;
        protected List<string> AppCodes = new();
        private readonly ILogger _logger;
        private readonly JsonSerializerSettings _serializerSettings = new()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        protected TokenClaimDto ClaimData { get; set; }

        protected internal BaseService(IServiceBaseParameter<T> businessBaseParameter)
        {
            _logger = businessBaseParameter.Logger ?? throw new ArgumentNullException(nameof(businessBaseParameter.Logger));
            HttpContextAccessor = businessBaseParameter.HttpContextAccessor;
            UnitOfWork = businessBaseParameter.UnitOfWork;
            Mapper = businessBaseParameter.Mapper;
            CacheRepository = businessBaseParameter.CacheRepository;
            Configuration = businessBaseParameter.Configuration;
            var claims = HttpContextAccessor?.HttpContext?.User;
            var ip = HttpContextAccessor?.HttpContext?.Connection.RemoteIpAddress?.ToString();
            ClaimData = GetTokenClaimDto(claims);
            ClaimData.IpAddress = ip;

        }


        public virtual async Task<Result<TGetDto>> GetByIdAsync(object id)
        {
            try
            {
                T query = await UnitOfWork.Repository.GetAsync(id);
                if (query == null)
                {
                    return Result<TGetDto>.Failure(MessagesConstants.EntityNotFound);
                }
                var data = Mapper.Map<T, TGetDto>(query);
                return Result<TGetDto>.Success(data, MessagesConstants.Success);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetByIdAsync)}");
                _logger.LogError(JsonConvert.SerializeObject(e, _serializerSettings));
                return Result<TGetDto>.Failure(MessagesConstants.GetError);
            }
        }

        public virtual async Task<Result<TEditDto>> GetByIdForEditAsync(object id)
        {
            try
            {
                T query = await UnitOfWork.Repository.GetAsync(id);
                if (query == null)
                {
                    return Result<TEditDto>.Failure(MessagesConstants.EntityNotFound);
                }
                var data = Mapper.Map<T, TEditDto>(query);
                return Result<TEditDto>.Success(data, MessagesConstants.Success);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetByIdForEditAsync)}");
                _logger.LogError(JsonConvert.SerializeObject(e, _serializerSettings));
                return Result<TEditDto>.Failure(MessagesConstants.GetError);
            }
        }

        public virtual async Task<Result<IEnumerable<TGetDto>>> GetAllAsync(bool disableTracking = false, Expression<Func<T, bool>> predicate = null)
        {
            try
            {
                IEnumerable<T> query;
                if (predicate != null)
                {
                    query = await UnitOfWork.Repository.FindAsync(predicate);
                }
                else
                {
                    query = await UnitOfWork.Repository.GetAllAsync(disableTracking: disableTracking);
                }

                var data = Mapper.Map<IEnumerable<T>, IEnumerable<TGetDto>>(query);
                return Result<IEnumerable<TGetDto>>.Success(data, MessagesConstants.Success);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetAllAsync)}");
                _logger.LogError(JsonConvert.SerializeObject(e, _serializerSettings));
                return Result<IEnumerable<TGetDto>>.Failure(MessagesConstants.EntitiesRetrievalError);
            }
        }

        public virtual async Task<Result<TKeyDto>> AddAsync(TAddDto model)
        {
            try
            {
                T entity = Mapper.Map<TAddDto, T>(model);
                SetEntityCreatedBaseProperties(entity);
                await UnitOfWork.Repository.AddAsync(entity);
                var affectedRows = await UnitOfWork.SaveChangesAsync();
                if (affectedRows > 0)
                {
                    return Result<TKeyDto>.Success(model.Id, MessagesConstants.AddSuccess);
                }
                return Result<TKeyDto>.Failure(MessagesConstants.AddError);
            }
            catch (Exception e)
            {
                _logger.LogError($"{MessagesConstants.AddError}-{nameof(AddAsync)}");
                _logger.LogError(JsonConvert.SerializeObject(e, _serializerSettings));
                return Result<TKeyDto>.Failure(MessagesConstants.AddError);
            }
        }

        public virtual async Task<Result<IEnumerable<TKeyDto>>> AddListAsync(List<TAddDto> model)
        {
            try
            {
                List<T> entities = Mapper.Map<List<TAddDto>, List<T>>(model);
                await UnitOfWork.Repository.AddRangeAsync(entities);
                var affectedRows = await UnitOfWork.SaveChangesAsync();
                if (affectedRows > 0)
                {
                    var ids = model.Select(x => x.Id);
                    return Result<IEnumerable<TKeyDto>>.Success(ids, MessagesConstants.AddSuccess);
                }
                return Result<IEnumerable<TKeyDto>>.Failure(MessagesConstants.AddError);
            }
            catch (Exception e)
            {
                _logger.LogError($"{MessagesConstants.AddError}-{nameof(AddListAsync)}");
                _logger.LogError(JsonConvert.SerializeObject(e, _serializerSettings));
                return Result<IEnumerable<TKeyDto>>.Failure(MessagesConstants.AddError);
            }
        }

        public virtual async Task<Result<TKeyDto>> UpdateAsync(TAddDto model)
        {
            try
            {
                T entityToUpdate = await UnitOfWork.Repository.GetAsync(model.Id);
                if (entityToUpdate == null)
                {
                    return Result<TKeyDto>.Failure(MessagesConstants.EntityNotFound);
                }
                var newEntity = Mapper.Map(model, entityToUpdate);
                SetEntityModifiedBaseProperties(newEntity);
                UnitOfWork.Repository.Update(entityToUpdate, newEntity);
                var affectedRows = await UnitOfWork.SaveChangesAsync();
                if (affectedRows > 0)
                {
                    return Result<TKeyDto>.Success(model.Id, MessagesConstants.UpdateSuccess);
                }
                return Result<TKeyDto>.Failure(MessagesConstants.UpdateError);
            }
            catch (Exception e)
            {
                _logger.LogError($"{MessagesConstants.UpdateError}-{nameof(UpdateAsync)}");
                _logger.LogError(JsonConvert.SerializeObject(e, _serializerSettings));
                return Result<TKeyDto>.Failure(MessagesConstants.UpdateError);
            }
        }

        public virtual async Task<Result> DeleteAsync(object id)
        {
            try
            {
                var entityToDelete = await UnitOfWork.Repository.GetAsync(id);
                if (entityToDelete == null)
                {
                    return Result.Failure(MessagesConstants.EntityNotFound);
                }
                UnitOfWork.Repository.Remove(entityToDelete);
                var affectedRows = await UnitOfWork.SaveChangesAsync();
                if (affectedRows > 0)
                {
                    return Result.Success(MessagesConstants.DeleteSuccess);
                }
                return Result.Failure(MessagesConstants.DeleteError);
            }
            catch (Exception e)
            {
                _logger.LogError($"{MessagesConstants.DeleteError}-{nameof(DeleteAsync)}");
                _logger.LogError(JsonConvert.SerializeObject(e, _serializerSettings));
                return Result.Failure(MessagesConstants.DeleteError);
            }
        }

        public virtual async Task<Result> DeleteSoftAsync(object id)
        {
            try
            {
                var entityToDelete = await UnitOfWork.Repository.GetAsync(id);
                if (entityToDelete == null)
                {
                    return Result.Failure(MessagesConstants.EntityNotFound);
                }
                SetEntityModifiedBaseProperties(entityToDelete);
                UnitOfWork.Repository.RemoveLogical(entityToDelete);
                var affectedRows = await UnitOfWork.SaveChangesAsync();
                if (affectedRows > 0)
                {
                    return Result.Success(MessagesConstants.DeleteSuccess);
                }
                return Result.Failure(MessagesConstants.DeleteError);
            }
            catch (Exception e)
            {
                _logger.LogError($"{MessagesConstants.DeleteError}-{nameof(DeleteSoftAsync)}");
                _logger.LogError(JsonConvert.SerializeObject(e, _serializerSettings));
                return Result.Failure(MessagesConstants.DeleteError);
            }
        }


        protected virtual void SetEntityCreatedBaseProperties(BaseEntity<TKey> entity)
        {
            entity.CreatedById = ClaimData.UserId;
            entity.CreatedByEmployeeId = ClaimData.EmployeeId;
            entity.CreatedDate = DateTime.Now;
            entity.CreatedByEmployeeAr = ClaimData.EmployeeAr;
            entity.IpAddress = ClaimData.IpAddress;

        }

        protected virtual void SetEntityModifiedBaseProperties(BaseEntity<TKey> entity)
        {
            entity.ModifiedById = ClaimData.UserId;
            entity.ModifiedByEmployeeId = ClaimData.EmployeeId;
            entity.ModifiedDate = DateTime.Now;
            entity.ModifiedByEmployeeAr = ClaimData.EmployeeAr;
            entity.IpAddress = ClaimData.IpAddress;

        }

        protected virtual async Task<List<RoleDto>> GetRoles(string nationalId)
        {
            AppCodes.Add(Configuration["AppCodes:HumanResources"]);
            var result = await CacheRepository.GetEmployeeAsync(nationalId);
            var roles = result.Roles.Where(x => AppCodes.Contains(x.AppCode));
            return roles.ToList();
        }

        private TokenClaimDto GetTokenClaimDto(ClaimsPrincipal claims)
        {
            var claimData = new TokenClaimDto()
            {
                UserId = claims?.FindFirst(t => t.Type == "UserId")?.Value,
                EmployeeId = claims?.FindFirst(t => t.Type == "EmployeeId")?.Value,
                EmployeeAr = claims?.FindFirst(t => t.Type == "EmployeeAr")?.Value,
                UnitId = claims?.FindFirst(t => t.Type == "UnitId")?.Value,
                TeamId = claims?.FindFirst(t => t.Type == "TeamId")?.Value,
                NationalId = claims?.FindFirst(t => t.Type == "NationalId")?.Value
            };
            return claimData;
        }
    }
}
