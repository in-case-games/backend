using InCase.Domain.Entities;

namespace InCase.Domain.Dtos
{
    public class ResponseMessageDto<T>: BaseEntity where T: BaseEntity
    {
        public T? Data { get; set; }
        public bool Success { get; set; }
    }
}
