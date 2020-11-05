using System;
using DataInspector.Core.DAL;

namespace DataInspector.DataAccess.DAL {
    public class ReflectiveDAL<TDataModel> : IDataModelAccessLayer<TDataModel>
        where TDataModel : class {

        public object FetchValue(TDataModel targetObject, string expression) {
            throw new NotImplementedException();
        }

        public TOutput FetchValue<TOutput>(TDataModel targetObject, string expression) {
            throw new NotImplementedException();
        }
    }
}
