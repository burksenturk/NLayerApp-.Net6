using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NLayer.Core.DTOs;
using System.ComponentModel;

namespace NLayer.API.Filters
{
	public class ValidateFilterAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if(!context.ModelState.IsValid) 
			{
				//artık elimde bir hata sınıfı var (errorsa gelince ModelError çıkıyor) SElectMany e kadar olan kısımda.
				//Bu kod, hataları toplar.ModelState içindeki hataları tespit eder, bu hataları errors adlı bir liste içine toplar.
				var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

				context.Result=new BadRequestObjectResult(CustomResponseDto<NoContentDto>.Fail(400, errors));

			}
		}
	}
}
