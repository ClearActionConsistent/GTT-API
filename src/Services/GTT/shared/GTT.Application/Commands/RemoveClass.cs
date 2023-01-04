using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GTT.Application.Repositories;
using GTT.Application.Response;
using MediatR;

namespace GTT.Application.Commands
{
    public class RemoveClass
    {
       public record Command(
         int classId    
       ) : IRequest<BaseResponseModel>;

       internal class Validator : AbstractValidator<Command>
       {
            public Validator()
            {
                RuleFor(x => x.classId)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("ClassId is required")
                    .GreaterThan(0).WithMessage("ClassId is greater than 0");
            }
       }

        internal class Handler : IRequestHandler<Command, BaseResponseModel>
        {

            private IClassRepository _classRepository;

            public Handler(IClassRepository classRepository)
            {
                _classRepository = classRepository;
            }

            public async Task<BaseResponseModel> Handle(Command request, CancellationToken cancellationToken)
            {
                //handle request command to delete class information
                var getClass = await _classRepository.GetByIdAsync(request.classId);

                if (getClass == null)
                {
                    return new BaseResponseModel(HttpStatusCode.NotFound, "Class is invalid");
                } 
                
                var result = await _classRepository.DeleteAsync(request.classId);

                if (result <= 0)
                {
                    return new BaseResponseModel(HttpStatusCode.BadRequest,"Can not delete class");
                }

                return new BaseResponseModel(HttpStatusCode.OK, "Successfully");
            }
        }
    }
}
