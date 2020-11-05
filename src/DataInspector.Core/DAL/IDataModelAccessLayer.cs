namespace DataInspector.Core.DAL {
    public interface IDataModelAccessLayer<TDataModel>
        where TDataModel : class {

        object FetchValue(TDataModel targetObject, string expression);

        TOutput FetchValue<TOutput>(TDataModel targetObject, string expression);
    }
}
