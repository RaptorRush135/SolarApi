namespace SolarApi.Api.DataModels;

using Il2CppReloaded.DataModels;

using Il2CppSource.DataModels;

using Il2CppTekly.DataModels.Models;

public sealed record AppDataModels(
    UsersModel UsersModel,
    AlmanacModel AlmanacModel,
    LevelDataModel LevelDataModel,
    PlatformDataModel PlatformDataModel,
    DialogDataModel DialogDataModel,
    FeDataModel FeDataModel,
    RunHistoryModel RunHistoryModel,
    AchievementsModel AchievementsModel)
{
    public IReadOnlyList<ModelBase> Items { get; }
        = [UsersModel, AlmanacModel, LevelDataModel, PlatformDataModel,
            DialogDataModel, FeDataModel, RunHistoryModel, AchievementsModel,];
}
