using System.Linq.Expressions;

namespace Game.BLL.Interfaces;
public interface IApplicationDbContextWrapper
{
	public void SetEntryIsModifyProperty<T, TProperty>(
		T entity, 
		Expression<Func<T, TProperty>> expression, 
		bool isModify = true) where T : class;
}