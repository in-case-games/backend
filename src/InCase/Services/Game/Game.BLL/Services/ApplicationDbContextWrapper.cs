using System.Linq.Expressions;
using Game.BLL.Interfaces;
using Game.DAL.Data;

namespace Game.BLL.Services;
public class ApplicationDbContextWrapper(ApplicationDbContext context) : IApplicationDbContextWrapper
{
	public virtual void SetEntryIsModifyProperty<T, TProperty>(
		T entity, 
		Expression<Func<T, TProperty>> propertyExpression, 
		bool isModify = true) where T : class
	{
		context.Entry(entity).Property(propertyExpression).IsModified = isModify;
	}
}