namespace SolarApi.Api.DataModels;

using Il2CppReloaded.DataModels;

using Il2CppSource.DataModels;

using Il2CppTekly.DataModels.Models;

public sealed record BootDataModels(
    SettingsDataModel SettingsDataModel,
    ControllerDataModel ControllerDataModel,
    LoadingDataModel LoadingDataModel,
    InputDataModel InputDataModel)
{
    public IReadOnlyList<ModelBase> Items { get; }
        = [SettingsDataModel, ControllerDataModel, LoadingDataModel, InputDataModel];
}
