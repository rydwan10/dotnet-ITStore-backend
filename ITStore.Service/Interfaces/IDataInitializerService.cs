using System;
using System.Threading.Tasks;

namespace ITStore.Services.Interfaces
{
	public interface IDataInitializerService
	{
		public Task<string> Initialize();
	}
}

