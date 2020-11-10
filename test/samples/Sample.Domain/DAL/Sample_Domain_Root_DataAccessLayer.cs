using System;
using DataInspector.DataAccess.DAL;
using Sample.Domain;

namespace Sample.Domain.DAL {
	public class Sample_Domain_Root_DataAccessLayer : ReflectiveDAL<Root> {
		public Sample_Domain_Root_DataAccessLayer() : base() { }
	}
}

