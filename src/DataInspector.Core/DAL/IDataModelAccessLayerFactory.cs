namespace DataInspector.Core.DAL {
    public interface IDataModelAccessLayerFactory {
        string EmitDataAccessLayerCode(DataModelPropertySheet dataModelPropertySheet, DALBuilderContext buildContext);
    }
}
